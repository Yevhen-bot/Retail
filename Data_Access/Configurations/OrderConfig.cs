using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_Access.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                   .OwnsMany(c => c.Products, Pbuilder =>
                   {
                       Pbuilder
                           .ToTable("Order_Products");

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
