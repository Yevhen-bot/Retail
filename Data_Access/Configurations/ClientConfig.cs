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
    public class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder
                .HasMany(c => c.Orders)
                .WithOne(o => o.Client)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(c => c.Buildings)
                .WithMany(b => b.Clients);

            builder
                .ComplexProperty(w => w.Name, namebuilder =>
                {
                    namebuilder
                        .Property(n => n.FirstName)
                        .HasColumnName("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);
                    namebuilder
                        .Property(n => n.LastName)
                        .HasColumnName("LastName")
                        .IsRequired()
                        .HasMaxLength(50);
                })
                .ComplexProperty(w => w.Age, agebuilder =>
                {
                    agebuilder
                        .Property(a => a.BirhtDate)
                        .HasColumnName("BirthDate")
                        .IsRequired()
                        .HasColumnType("date");
                })
                .ComplexProperty(w => w.Salary, salarybuilder =>
                {
                    salarybuilder
                        .Property(s => s.Amount)
                        .HasColumnName("Salary")
                        .IsRequired()
                        .HasPrecision(18, 2);
                })
                .ComplexProperty(w => w.Email, emailbuilder =>
                {
                    emailbuilder
                        .Property(e => e.EmailAddress)
                        .HasColumnName("Email")
                        .IsRequired()
                        .HasMaxLength(50);
                });

            builder
                .OwnsMany(c => c.Preferences, Pbuilder =>
                {
                    Pbuilder
                        .ToTable("Preferences");

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
