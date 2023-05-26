using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolicitatieOpdracht.Dtos;
using SolicitatieOpdracht.Models;

namespace SolicitatieOpdracht.Services.Address
{
    public interface IAddressService
    {
        Task<ServiceResponse<List<GetAddressDto>>> GetAddresses();
        Task<ServiceResponse<List<GetAddressDto>>> GetAddresses(string searchOption);
        Task<ServiceResponse<GetAddressDto>> GetAddress(int id);
        Task<ServiceResponse<GetAddressDto>> AddAddress(AddAddressDto address);
        Task<ServiceResponse<GetAddressDto>> UpdateAddress(UpdateAddressDto updateAddress);
        Task<ServiceResponse<List<GetAddressDto>>> DeleteAddress(int id);
    }
}