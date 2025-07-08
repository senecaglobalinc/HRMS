using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder) 
        {
            builder.ToTable("Status");

            builder
               .HasKey(st => st.Id);

            builder
             .Property(st => st.StatusId)
             .HasColumnType("int");

            builder
                .Property(st => st.StatusCode)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
                .Property(st => st.StatusDescription)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");
          
            builder
                .HasOne(st => st.CategoryMaster)
                .WithMany(cat => cat.Status)
                .HasForeignKey(st => st.CategoryMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(st => st.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(st => st.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(st => st.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(st => st.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(st => st.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(st => st.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(st => st.CurrentUser);
        }
    }
}
