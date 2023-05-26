using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolicitatieOpdracht.Dtos;
using SolicitatieOpdracht.Models;
using SolicitatieOpdracht.Services.Address;

namespace SolicitatieOpdracht.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            this._addressService = addressService;
        }

        [HttpGet("GetAllAddresses")]
        public async Task<ActionResult<ServiceResponse<List<GetAddressDto>>>> GetAddresses(){
            return Ok(await _addressService.GetAddresses());
        }

        [HttpGet("{searchOption}/GetSpecificAddress")]
        public async Task<ActionResult<ServiceResponse<List<GetAddressDto>>>> GetAddresses(string searchOption){
            return Ok(await _addressService.GetAddresses(searchOption));
        }

        [HttpGet("{id}/GetSingleAddress")]
        public async Task<ActionResult<ServiceResponse<GetAddressDto>>> GetSingleAddress(int id){
            return Ok(await _addressService.GetAddress(id));
        }

        [HttpPost("AddAddress")]
        public async Task<ActionResult<ServiceResponse<AddAddressDto>>> AddAddress(AddAddressDto address){
            return Ok(await _addressService.AddAddress(address));
        }

        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<ServiceResponse<AddAddressDto>>> UpdateAddress(UpdateAddressDto updateAddress){
            return Ok(await _addressService.UpdateAddress(updateAddress));
        }
        
        [HttpDelete("{id}DeleteAddress")]
        public async Task<ActionResult<ServiceResponse<List<GetAddressDto>>>> DeleteAddress(int id){
            return Ok(await _addressService.DeleteAddress(id));
        }
    }
}