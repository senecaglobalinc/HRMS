using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public  class EmployeeHistoryConfigurations : IEntityTypeConfiguration<Employee.Entities.EmployeeHistory>
    {
        public void Configure(EntityTypeBuilder<Employee.Entities.EmployeeHistory> builder)
        {
            builder.ToTable("EmployeeHistory");
            builder
               .Property(e => e.Id);
            builder
              .Property(e => e.EmployeeId);
            builder
              .Property(e => e.GradeId);
            builder
              .Property(e => e.DesignationId);
            builder
              .Property(e => e.DepartmentId);
            builder
              .Property(e => e.ReportingManagerId);
            builder
              .Property(e => e.PracticeAreaId);
            builder
            .Property(e => e.CreatedDate);
            builder
            .Ignore(e => e.CreatedBy);
            builder
            .Ignore(e => e.ModifiedBy);
            builder
            .Ignore(e => e.ModifiedDate);
            builder
            .Ignore(e => e.IsActive);
            builder
            .Ignore(e => e.CurrentUser);
            builder
            .Ignore(e => e.SystemInfo);
        }
    }
}
