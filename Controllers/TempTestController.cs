using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolicitatieOpdracht.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Device.Location;
using Microsoft.Extensions.Configuration;

namespace SolicitatieOpdracht.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class TempTestController : ControllerBase
    {
        static HttpClient client = new HttpClient();

        [HttpGet("GetAddress")]
        public async Task<ActionResult<ServiceResponse<double>>> GetAddressCode(){
            var response = new ServiceResponse<double>();

            Address data1 = new Address(){
                Id = 1,
                StreetName = "Beverhoeve",
                HouseNumber = 10,
                PostalCode = "3831TG",
                Place = "Leusden",
                Country = "Nederland"
            };
            Address data2 = new Address(){
                Id = 2,
                StreetName = "Orteliuslaan",
                HouseNumber = 1000,
                PostalCode = "3528BD",
                Place = "Utrecht",
                Country = "Nederland"
            };

            try{
                IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
                string api_key = configuration["APIKeyPositionStack"]!;
                
                string address1 = $"{data1.HouseNumber} {data1.StreetName}, {data1.Place}";
                string address2 = $"{data2.HouseNumber} {data2.StreetName}, {data2.Place}";
                
                HttpResponseMessage responseAddress1 = await client.GetAsync($"http://api.positionstack.com/v1/forward?access_key={api_key}&query={address1}");
                HttpResponseMessage responseAddress2 = await client.GetAsync($"http://api.positionstack.com/v1/forward?access_key={api_key}&query={address2}");

                if (responseAddress1 is null || responseAddress2 is null){
                    throw new Exception("Could not find address");
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

                response.Message = "Distance in kilometers";
                response.Data = distance;
            }
            catch(Exception ex){
                response.Success = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        private double distanceBetweenTwoCoordinates(double lat1, double lat2, double lon1, double lon2){
            var coord1 = new GeoCoordinate(lat1, lon1); 
            var coord2 = new GeoCoordinate(lat2, lon2); 

            return coord1.GetDistanceTo(coord2);
        }
    }
}