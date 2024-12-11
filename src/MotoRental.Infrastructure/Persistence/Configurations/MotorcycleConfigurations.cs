using MotoRental.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotoRental.Infrastructure.Persistence.Configurations
{
    public class MotorcycleConfigurations : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(s => s.Id)
                .HasColumnType("text");


            builder
                .HasIndex(s => s.Plate)
                .IsUnique();

            builder
                .Property(u => u.Plate)
                .IsRequired();
            
            builder
                .Property(u => u.Year)
                .IsRequired();

            builder
                .Property(u => u.Model)
                .IsRequired();
        }
    }
}
