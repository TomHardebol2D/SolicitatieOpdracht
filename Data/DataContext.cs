using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolicitatieOpdracht.Models;

namespace SolicitatieOpdracht.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Address>().HasData(
                new Models.Address{ Id = 1, StreetName = "Beverhoeven", HouseNumber = 12, PostalCode = "3831TG", Place = "Leusden", Country = "Netherlands"},
                new Models.Address{ Id = 2, StreetName = "Uilenhoeven", HouseNumber = 8, PostalCode = "3832TG", Place = "Leusden", Country = "Netherlands"},
                new Models.Address{ Id = 3, StreetName = "Reehoeven", HouseNumber = 22, PostalCode = "3831TG", Place = "Leusden", Country = "Netherlands"}
            );
        }
        
        public DbSet<Models.Address> addresses => Set<Models.Address>();

    }
}