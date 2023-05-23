using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolicitatieOpdracht.Dtos
{
    public class GetAddressDto
    {
        public int Id { get; set; }
        public string StreetName { get; set; } = string.Empty;
        public int HouseNumber { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}