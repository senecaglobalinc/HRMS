using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public static class EntityConfiguration 
    {
        public static void Add<T>(EntityTypeBuilder<T> builder) where T : BaseEntity
        {
            builder
               .Property(rs => rs.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(rs => rs.ModifiedDate)
                    .HasColumnType("timestamp with time zone");

            builder
               .Property(rs => rs.SystemInfo)
                   .HasMaxLength(50)
                   .HasColumnType("varchar(50)");

            builder
               .Property(rs => rs.IsActive)
                   .HasColumnType("boolean");

            builder
               .Property(rs => rs.CreatedBy)
                   .HasMaxLength(100)
                   .HasColumnType("varchar(100)");

            builder
               .Property(rs => rs.ModifiedBy)
                   .HasMaxLength(50)
                   .HasColumnType("varchar(100)");

            builder.Ignore(rs => rs.CurrentUser);
        }
        
    }
}
