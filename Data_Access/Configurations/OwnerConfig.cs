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
    public class OwnerConfig : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder
                .HasMany(c => c.Buildings)
                .WithOne(b => b.Owner);

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
                .ComplexProperty(w => w.Email, emailbuilder =>
                {
                    emailbuilder
                        .Property(e => e.EmailAddress)
                        .HasColumnName("Email")
                        .IsRequired()
                        .HasMaxLength(50);
                });
        }
    }
}
