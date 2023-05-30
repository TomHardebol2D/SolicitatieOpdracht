using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SolicitatieOpdracht.Data;
using SolicitatieOpdracht.Dtos;
using SolicitatieOpdracht.Models;
using System.Linq.Dynamic.Core;

using System.Device.Location;
using System.Text.Json;

namespace SolicitatieOpdracht.Services.Address
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        static HttpClient client = new HttpClient();
        string api_key;
        public AddressService(IMapper mapper, DataContext context){
            _mapper = mapper;
            _context = context;

            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            api_key = configuration["APIKeyPositionStack"]!;
        }

        public async Task<ServiceResponse<List<GetAddressDto>>> GetAddresses(){
            var response = new ServiceResponse<List<GetAddressDto>>();
            var addresses = await _context.addresses.ToListAsync();
            response.Data = addresses.Select(a => _mapper.Map<GetAddressDto>(a)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetAddressDto>> GetAddress(int id){
            var response = new ServiceResponse<GetAddressDto>();
            var address = await _context.addresses.FirstOrDefaultAsync(a => a.Id == id);
            
            if (address is null){
                response.Success = false;
                response.Message = $"Could not find address with id {id}";
                return response;
            }

            response.Data = _mapper.Map<GetAddressDto>(address);
            return response;
        }
       
        public async Task<ServiceResponse<GetAddressDto>> AddAddress(AddAddressDto address){
            var serviceResponse = new ServiceResponse<GetAddressDto>();
            
            if(!IsValidAddress(address)){
                serviceResponse.Success = false;
                serviceResponse.Message = "Address not valid";
                return serviceResponse;
            }

            var newAddress = _mapper.Map<Models.Address>(address);

            _context.addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetAddressDto>(newAddress);
            return serviceResponse;
        }

        private static bool IsValidAddress(AddAddressDto address){
            var properties = address.GetType().GetProperties();

            foreach (var property in properties){
                var propertyValue = property.GetValue(address)?.ToString();

                if (propertyValue == "" || propertyValue == "string" || propertyValue == "0"){
                    return false;
                }
            }
            
            return true;
        }

        public async Task<ServiceResponse<GetAddressDto>> UpdateAddress(UpdateAddressDto updateAddress){
            var serviceResponse = new ServiceResponse<GetAddressDto>();

            if(IsValidAddress(_mapper.Map<AddAddressDto>(updateAddress))){
                serviceResponse.Success = false;
                serviceResponse.Message = "Address not valid";
                return serviceResponse;
            }

            try{
                var address = await _context.addresses.FirstOrDefaultAsync(a => a.Id == updateAddress.Id);
                if (address is null){
                    throw new Exception("Could not find address");
                }
                
                address.StreetName = updateAddress.StreetName;
                address.HouseNumber = updateAddress.HouseNumber;
                address.PostalCode = updateAddress.PostalCode;
                address.Place = updateAddress.Place;
                address.Country = updateAddress.Country;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetAddressDto>(address);
            }
            catch (Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAddressDto>>> DeleteAddress(int id){
            var serviceResponse = new ServiceResponse<List<GetAddressDto>>();

            try{
                var address = await _context.addresses.FirstOrDefaultAsync(a => a.Id == id);
                if (address is null){
                    throw new Exception($"Address with id {id} not found.");
                }
                
                _context.addresses.Remove(address);
                await _context.SaveChangesAsync();
                
                serviceResponse.Data = await _context.addresses.Select(a => _mapper.Map<GetAddressDto>(a)).ToListAsync();
            }
            catch (Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAddressDto>>> GetAddresses(string searchOption){
            var serviceResponse = new ServiceResponse<List<GetAddressDto>>();
            serviceResponse.Data = new List<GetAddressDto>();
            try{
                var addresses = await _context.addresses.Select(a => _mapper.Map<GetAddressDto>(a)).ToListAsync();
                foreach (var address in addresses){
                    var properties = address.GetType().GetProperties();

                    foreach (var property in properties){
                        var propertyValue = property.GetValue(address)?.ToString();

                        if (!string.IsNullOrEmpty(propertyValue) && propertyValue.Equals(searchOption)){
                            serviceResponse.Data.Add(address);
                            continue;
                        }
                    }
                }

            }
            catch (Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAddressDto>>> GetAddressesSorted(string sortType, bool ascending){
            var serviceResponse = new ServiceResponse<List<GetAddressDto>>();

            try{
                var addresses = await _context.addresses.ToListAsync();
                var sortedAddresses = addresses.OrderBy(p => p.GetType().GetProperty(sortType.ToLower(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(p, null)).ToList();
                
                if  (sortedAddresses is null){
                    throw new Exception("No address found could be because the sort type is not valid.");
                }
                
                serviceResponse.Data = sortedAddresses.Select(a => _mapper.Map<GetAddressDto>(a)).ToList();                
            }
            catch (Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<float>> GetDistanceBetweenTwoAddresses(int addressId1, int addressId2){
            var serviceResponse = new ServiceResponse<float>();

            try{
                var data1 = await _context.addresses.FirstOrDefaultAsync(a => a.Id == addressId1);
                var data2 = await _context.addresses.FirstOrDefaultAsync(a => a.Id == addressId2);

                if (data1 is null || data2 is null){
                    throw new Exception("Could not find addresses in database");
                }

                
                string address1 = $"{data1.HouseNumber} {data1.StreetName}, {data1.Place}";
                string address2 = $"{data2.HouseNumber} {data2.StreetName}, {data2.Place}";
                
                HttpResponseMessage responseAddress1 = await client.GetAsync($"http://api.positionstack.com/v1/forward?access_key={api_key}&query={address1}");
                HttpResponseMessage responseAddress2 = await client.GetAsync($"http://api.positionstack.com/v1/forward?access_key={api_key}&query={address2}");

                if (responseAddress1 is null || responseAddress2 is null){
                    throw new Exception("Could not find address in positionstack");
                }

                var jsonAddress1 = JsonDocument.Parse(responseAddress1.Content.ReadAsStringAsync().Result);
                var jsonAddress2 = JsonDocument.Parse(responseAddress2.Content.ReadAsStringAsync().Result);

                if (jsonAddress1 is null || jsonAddress2 is null){
                    throw new Exception("Could not convert api response to json");
                }

                var lat1 = jsonAddress1.RootElement.GetProperty("data")[0].GetProperty("latitude").GetDouble();
                var lon1 = jsonAddress1.RootElement.GetProperty("data")[0].GetProperty("longitude").GetDouble();

                var lat2 = jsonAddress2.RootElement.GetProperty("data")[0].GetProperty("latitude").GetDouble();
                var lon2 = jsonAddress2.RootElement.GetProperty("data")[0].GetProperty("longitude").GetDouble();

                var distance = this.distanceBetweenTwoCoordinates(lat1, lat2, lon1, lon2);
                distance = (double.Round(distance / 1000, 1));

                serviceResponse.Message = "Distance in kilometers";
                serviceResponse.Data = (float)distance;
            }
            catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;

            throw new NotImplementedException();
        }

        private double distanceBetweenTwoCoordinates(double lat1, double lat2, double lon1, double lon2){
            var coord1 = new GeoCoordinate(lat1, lon1); 
            var coord2 = new GeoCoordinate(lat2, lon2); 

            return coord1.GetDistanceTo(coord2);
        }
    }
}