using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public  class AssociateInformationReportConfiguration:IEntityTypeConfiguration<Entities.AssociateInformationReport>
    {
        public void Configure(EntityTypeBuilder<Entities.AssociateInformationReport> builder)
        {
            builder.ToTable("AssociateInformationReport");

            builder.HasNoKey();
            builder
             .Property(e => e.EmployeeCode);

            builder
                .Property(e => e.AssociateName);

            builder
                .Property(e => e.Designation);
            builder
               .Property(e => e.Grade);
            builder
               .Property(e => e.Experience);           
            builder
             .Property(e => e.ProjectName);
            builder
             .Property(e => e.Skill);
            builder
             .Property(e => e.Technology);
            builder
            .Property(e => e.IsCritical);
            builder
          .Property(e => e.JoinDate);
            builder
          .Property(e => e.ClientName);
            builder
          .Property(e => e.Allocationpercentage);
            builder
          .Property(e => e.LeadName);
            builder
          .Property(e => e.ReportingManagerName);

            builder
          .Property(e => e.ProgramManagerName);
            builder
          .Property(e => e.IsResigned);
            builder
          .Property(e => e.ResignationDate);
            builder
          .Property(e => e.LastWorkingDate);
            builder
          .Property(e => e.IsLongLeave);
            builder
          .Property(e => e.LongLeaveStartDate);
            builder
          .Property(e => e.TentativeJoinDate);
            builder
          .Property(e => e.IsFutureProjectMarked);
            builder
          .Property(e => e.FutureProjectName);
            builder
          .Property(e => e.FutureProjectTentativeDate);
          builder
                .Property(e => e.EmployeeId);
        }
    } 
}