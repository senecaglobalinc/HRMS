using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class DepartmentDLConfiguration : IEntityTypeConfiguration<DepartmentDL>
    {
        public void Configure(EntityTypeBuilder<DepartmentDL> builder)
        {
            builder.ToTable("DepartmentDL");

            builder
               .HasKey(at => at.Id);

            builder
                .Property(at => at.DepartmentId)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(at => at.DLEmailAdress)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("character varying(256)");
        }
    }
}
