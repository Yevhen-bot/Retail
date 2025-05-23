﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_Access.Configurations
{
    public class BuildingConfig : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder
                .ComplexProperty(w => w.Adress, adressbuilder =>
                {
                    adressbuilder
                        .Property(h => h.Country)
                        .HasColumnName("Country")
                        .IsRequired()
                        .HasMaxLength(40);

                    adressbuilder
                        .Property(h => h.City)
                        .HasColumnName("City")
                        .IsRequired()
                        .HasMaxLength(40);

                    adressbuilder
                        .Property(h => h.Street)
                        .HasColumnName("Street")
                        .IsRequired()
                        .HasMaxLength(100);

                    adressbuilder
                        .Property(h => h.HouseNumber)
                        .HasColumnName("HouseNumber")
                        .IsRequired()
                        .HasMaxLength(4);
                })
                .ComplexProperty(w => w.Role, rolebuilder =>
                {
                    rolebuilder
                        .Property(r => r.RoleName)
                        .HasColumnName("Role")
                        .IsRequired()
                        .HasMaxLength(20);
                });

            builder
                .OwnsMany(c => c.Products, Pbuilder =>
                {
                    Pbuilder
                        .ToTable("Building_Products");

                    Pbuilder
                        .Property(p => p.Name)
                        .HasColumnName("ProductName")
                        .IsRequired()
                        .HasMaxLength(50);
                    Pbuilder
                        .Property(p => p.Price)
                        .HasColumnName("ProductPrice")
                        .IsRequired()
                        .HasPrecision(18, 2);
                    Pbuilder
                        .Property(p => p.MPU)
                        .HasColumnName("MetersPerUnit")
                        .IsRequired()
                        .HasPrecision(18, 2);
                    Pbuilder
                        .Property(p => p.Quantity)
                        .HasColumnName("Quantity");
                });
        }
    }
}
