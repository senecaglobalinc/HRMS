using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class SkillVersionConfiguration : IEntityTypeConfiguration<SkillVersion>
    {
        public void Configure(EntityTypeBuilder<SkillVersion> builder)
        {
            builder.ToTable("SkillVersion");

            builder.Property(e => e.ID);

            builder.Property(e => e.EmployeeId);

            builder.Property(e => e.EmployeeSkillId);

            builder.Property(e => e.Version);
           
            builder.Ignore(e => e.IsActive);

            builder.Ignore(e => e.CreatedBy);

            builder.Ignore(e => e.ModifiedBy);

            builder.Ignore(e => e.CreatedDate);

            builder.Ignore(e => e.ModifiedDate);

            builder.Ignore(e => e.SystemInfo);               

            builder.Ignore(e => e.CurrentUser);
        }
    }
}
