using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Employee.Database.Configurations
{
    public  class UtilizationReportConfiguration:IEntityTypeConfiguration<Entities.UtilizationReport>
    {
        public void Configure(EntityTypeBuilder<Entities.UtilizationReport> builder)
        {
            builder.ToTable("UtilizationReport");

            builder.HasNoKey();
            builder
             .Property(e => e.AssociateCode)
             .HasColumnType("varchar(50)");
            builder
                .Property(e => e.AssociateName)
                .HasColumnType("varchar(150)");
            builder
                .Property(e => e.DateOfJoining)
                .HasColumnType("timestamp with time zone");
            builder
               .Property(e => e.LastWorkingDate)
               .HasColumnType("timestamp with time zone");
            builder
               .Property(e => e.ProjectsWorked);
            builder
             .Property(e => e.TimeTakenForBillable);
            builder
             .Property(e => e.TotalWorkedDays);
            builder
             .Property(e => e.TotalBillingDays);
            builder
            .Property(e => e.TotalNonBillingDays);
            builder
            .Property(e => e.BillingDaysPercentage)
            .HasColumnType("decimal(18, 2)");
             builder
               .Property(e => e.LastBillingDate)
               .HasColumnType("timestamp with time zone");
        }
    } 
}