using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SolicitatieOpdracht.Data;
using SolicitatieOpdracht.Dtos;
using SolicitatieOpdracht.Models;

namespace SolicitatieOpdracht.Services.Address
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public AddressService(IMapper mapper, DataContext context){
            _mapper = mapper;
            _context = context;
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
            
            if(IsValidAddress(address)){
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
            if (address.StreetName == "" || address.HouseNumber == 0 || address.PostalCode == "" || address.Place == "" || address.Country == "")
            {
                return false;
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
                
                serviceResponse.Data = _context.addresses.Select(a => _mapper.Map<GetAddressDto>(a)).ToList();
            }
            catch (Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}