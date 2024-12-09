using MotoRental.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotoRental.Infrastructure.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .HasIndex(s => s.Email)
                .IsUnique();

            builder
                .Property(u => u.Email)
                .IsRequired();

            builder
                .Property(u => u.FullName)
                .IsRequired();

            builder
                .Property(u => u.Password)
                .IsRequired();
        }
    }
}
