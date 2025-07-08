using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class TagAssociateConfiguration : IEntityTypeConfiguration<TagAssociate>
    {
        public void Configure(EntityTypeBuilder<TagAssociate> builder)
        {
            builder.ToTable("TagAssociate");

            builder
               .Property(t => t.Id)
               .HasColumnName("ID");

            builder
               .Property(t => t.TagAssociateListName)
               .IsRequired()
               .HasMaxLength(100)
               .IsUnicode(false);

            builder
              .Property(t => t.EmployeeId)
              .IsRequired()
              .HasColumnName("EmployeeId");

            builder
              .Property(t => t.ManagerId)
              .IsRequired()
              .HasColumnName("ManagerId");

            builder
            .Property(t => t.CreatedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

            builder
               .Property(t => t.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(t => t.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(t => t.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(t => t.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(t => t.CurrentUser);
            builder.Ignore(t => t.IsActive);
        }
    }
}
