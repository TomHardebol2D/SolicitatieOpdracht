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

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetAddressDto>>>> GetAddress(){
            return Ok(await _addressService.GetAddresses());
        }

        [HttpGet("{id}GetSingle")]
        public async Task<ActionResult<ServiceResponse<GetAddressDto>>> GetSingleAddress(int id){
            return Ok(await _addressService.GetAddress(id));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<ServiceResponse<AddAddressDto>>> AddAddress(AddAddressDto address){
            return Ok(await _addressService.AddAddress(address));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<ServiceResponse<AddAddressDto>>> UpdateAddress(UpdateAddressDto updateAddress){
            return Ok(await _addressService.UpdateAddress(updateAddress));
        }
        
        [HttpDelete("Delete")]
        public async Task<ActionResult<ServiceResponse<List<GetAddressDto>>>> DeleteAddress(int id){
            return Ok(await _addressService.DeleteAddress(id));
        }
    }
}