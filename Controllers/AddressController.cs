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
        public AddressController(IAddressService addressService){
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

        [HttpGet("{searchType}/{ascending}/GetAddressesSorted")]
        public async Task<ActionResult<ServiceResponse<List<GetAddressDto>>>> GetAddressesSorted(string searchType, bool ascending){
            return Ok(await _addressService.GetAddressesSorted(searchType, ascending));
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

        [HttpGet("{addressId1}/{addressId2}GetDistanceBetweenTwoAddresses")]
        public async Task<ActionResult<ServiceResponse<float>>> GetDistanceBetweenTwoAddresses(int addressId1, int addressId2){
            return Ok(await _addressService.GetDistanceBetweenTwoAddresses(addressId1, addressId2));
        }
    }
}