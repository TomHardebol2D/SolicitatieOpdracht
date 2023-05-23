﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolicitatieOpdracht.Data;

#nullable disable

namespace SolicitatieOpdracht.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SolicitatieOpdracht.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HouseNumber")
                        .HasColumnType("int");

                    b.Property<string>("Place")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("addresses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = "Netherlands",
                            HouseNumber = 12,
                            Place = "Leusden",
                            PostalCode = "3831TG",
                            StreetName = "Beverhoeven"
                        },
                        new
                        {
                            Id = 2,
                            Country = "Netherlands",
                            HouseNumber = 8,
                            Place = "Leusden",
                            PostalCode = "3832TG",
                            StreetName = "Uilenhoeven"
                        },
                        new
                        {
                            Id = 3,
                            Country = "Netherlands",
                            HouseNumber = 22,
                            Place = "Leusden",
                            PostalCode = "3831TG",
                            StreetName = "Reehoeven"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
