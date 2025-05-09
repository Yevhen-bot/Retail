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
    public class WorkerConfig : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
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
                .ComplexProperty(w => w.HomeAdress, adressbuilder =>
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
                .ComplexProperty(w => w.Salary, salarybuilder =>
                {
                    salarybuilder
                        .Property(s => s.Amount)
                        .HasColumnName("Salary")
                        .IsRequired()
                        .HasPrecision(18, 2);
                })
                .ComplexProperty(w => w.Age, agebuilder =>
                {
                    agebuilder
                        .Property(a => a.BirhtDate)
                        .HasColumnName("BirthDate")
                        .IsRequired()
                        .HasColumnType("date");
                })
                .ComplexProperty(w => w.ExaustionLevel, exaustionbuilder =>
                {
                    exaustionbuilder
                        .Property(e => e.Level)
                        .HasColumnName("ExaustionLevel")
                        .IsRequired()
                        .HasPrecision(18, 2);
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
                .HasOne(w => w.Building)
                .WithMany(b => b.Workers)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
