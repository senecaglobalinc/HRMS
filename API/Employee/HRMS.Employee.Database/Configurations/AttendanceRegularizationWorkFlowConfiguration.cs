using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    class AttendanceRegularizationWorkFlowConfiguration:IEntityTypeConfiguration<AttendanceRegularizationWorkFlow>
    {
        public void Configure(EntityTypeBuilder<AttendanceRegularizationWorkFlow> builder)
        {
            builder.ToTable("AttendanceRegularizationWorkFlow");
            builder.HasKey(x => x.WorkFlowId);
            builder.Property(x => x.SubmittedBy);
            builder.Property(x => x.RegularizationAppliedDate);
            builder.Property(x => x.InTime);
            builder.Property(x => x.OutTime);
            builder.Property(x => x.SubmittedTo);
            builder.Property(x => x.ApprovedBy);
            builder.Property(x => x.ApprovedDate);
            builder.Property(x => x.RejectedBy);
            builder.Property(x => x.RejectedDate);
            builder.Property(x => x.CreatedBy);
            builder.Property(x => x.CreatedDate);
            builder.Property(x => x.ModifiedBy);
            builder.Property(x => x.ModifiedDate);
            builder.Ignore(x => x.IsActive);
            builder.Ignore(x => x.CurrentUser);
            builder.Ignore(x => x.SystemInfo);

        }
    }
}
