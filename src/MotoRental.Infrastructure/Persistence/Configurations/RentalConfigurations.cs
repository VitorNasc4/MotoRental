using MotoRental.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotoRental.Infrastructure.Persistence.Configurations
{
    public class RentalConfigurations : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder
                .HasKey(r => r.Id);

            builder
                .Property(s => s.Id)
                .HasColumnType("text");

            builder
                .Property(r => r.MotorcycleId)
                .IsRequired();

            builder
                .Property(r => r.DeliveryPersonId)
                .IsRequired();

            builder
                .Property(r => r.StartDate)
                .IsRequired();

            builder
                .Property(r => r.EndDate)
                .IsRequired();

            builder
                .Property(r => r.TotalCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder
                .Property(r => r.Penalty)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne<Motorcycle>()
                .WithMany()
                .HasForeignKey(r => r.MotorcycleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne<DeliveryPerson>()
                .WithMany()
                .HasForeignKey(r => r.DeliveryPersonId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
