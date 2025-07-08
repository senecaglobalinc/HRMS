using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EmployeeKRARoleTypeHistoryConfiguration : IEntityTypeConfiguration<EmployeeKRARoleTypeHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeKRARoleTypeHistory> builder)
        {
            builder.ToTable("EmployeeKRARoleTypeHistory");

            builder
               .Property(c => c.Id)
               .HasColumnName("ID")
               .HasDefaultValueSql("newId()");

            builder
               .Property(c => c.EmployeeId)
               .HasColumnName("EmployeeId");          
               

            builder
              .Property(c => c.RoleTypeId);

            builder
              .Property(c => c.RoleTypeValidFrom);

            builder
             .Property(c => c.RoleTypeValidTo);

            builder
               .Property(c => c.IsActive);

            builder
             .Property(c => c.CreatedBy);

            builder
             .Property(c => c.CreatedDate);

            builder
             .Property(c => c.ModifiedBy);

            builder
             .Property(c => c.ModifiedDate);

            builder
             .Property(c => c.SystemInfo);

            builder
             .Ignore(c => c.CurrentUser);
        }
    }
}
