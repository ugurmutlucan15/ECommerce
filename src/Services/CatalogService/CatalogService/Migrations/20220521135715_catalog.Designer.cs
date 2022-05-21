﻿// <auto-generated />
using CatalogService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CatalogService.Migrations
{
    [DbContext(typeof(ECommerceContext))]
    [Migration("20220521135715_catalog")]
    partial class catalog
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CatalogService.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products", "dbo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Apple Phone",
                            Price = 12000m
                        },
                        new
                        {
                            Id = 2,
                            Name = "Samsung Phone",
                            Price = 5000m
                        },
                        new
                        {
                            Id = 3,
                            Name = "LG Phone",
                            Price = 7000m
                        },
                        new
                        {
                            Id = 4,
                            Name = "Xaomi Phone",
                            Price = 10000m
                        },
                        new
                        {
                            Id = 5,
                            Name = "Nokia Phone",
                            Price = 2000m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
