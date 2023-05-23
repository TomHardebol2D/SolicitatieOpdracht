using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SolicitatieOpdracht.Dtos;
using SolicitatieOpdracht.Models;

namespace SolicitatieOpdracht
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Address, GetAddressDto>();
            CreateMap<AddAddressDto, Address>();
            CreateMap<UpdateAddressDto, AddAddressDto>();
        }
    }
}