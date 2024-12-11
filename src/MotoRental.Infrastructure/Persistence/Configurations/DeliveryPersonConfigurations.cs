using MotoRental.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotoRental.Infrastructure.Persistence.Configurations
{
    public class DeliveryPersonConfigurations : IEntityTypeConfiguration<DeliveryPerson>
    {
        public void Configure(EntityTypeBuilder<DeliveryPerson> builder)
        {
            builder
                .HasKey(dp => dp.Id);

            builder
                .Property(s => s.Id)
                .HasColumnType("text");

            builder
                .HasIndex(dp => dp.CNPJ)
                .IsUnique();

            builder
                .HasIndex(dp => dp.CNH_Number)
                .IsUnique();

            builder
                .Property(dp => dp.FullName)
                .IsRequired();

            builder
                .Property(dp => dp.Birthday)
                .IsRequired();

            builder
                .Property(dp => dp.CNH_Type)
                .IsRequired();
            
            builder
                .Property(dp => dp.CNH_Number)
                .IsRequired();
            
            builder
                .Property(dp => dp.CNPJ)
                .IsRequired();
        }
    }
}
