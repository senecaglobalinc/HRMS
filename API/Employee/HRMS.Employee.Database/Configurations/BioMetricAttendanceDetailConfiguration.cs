using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Employee.Database.Configurations
{
    public class BioMetricAttendanceDetailConfiguration : IEntityTypeConfiguration<BioMetricAttendance>
    {
        public void Configure(EntityTypeBuilder<BioMetricAttendance> builder)
        {
            builder.ToTable("BiometricAttendance");
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.AsscociateId).HasColumnName("UserID");
            builder.Property(x => x.AsscociateName).HasColumnName("UserName");
            builder.Property(x => x.InTime).HasColumnName("Punch1_Time");
            builder.Property(x => x.OutTime).HasColumnName("Punch2_Time");
            builder.Property(x => x.WorkTime).HasColumnType("decimal(4, 0)"); 
            builder.Property(x => x.EarlyIn).HasColumnType("decimal(4, 0)"); 
            builder.Property(x => x.LateIn).HasColumnType("decimal(4, 0)"); 
            builder.Property(x => x.EarlyOut).HasColumnType("decimal(4, 0)");
            builder.Property(x => x.WorkingShift);
            builder.Property(x => x.WorkTime_HHMM);
            builder.Property(x => x.SignedStatus);
            builder.Property(x => x.SUMMARY);
            builder.Property(x => x.EarlyIn_HHMM);
            builder.Property(x => x.LateIn_HHMM);
            builder.Property(x => x.EarlyOut_HHMM);
            builder.Property(x => x.Punch1_Date);
            builder.Property(x => x.Punch2_Date);
            builder.Property(x => x.ProcessDate);
            builder.Property(x => x.PunchInfoLog);
            builder.Property(x => x.Location);
            builder.Property(x => x.Remarks);
            builder.Property(x => x.IsRegularized);
            builder.Ignore(v => v.SystemInfo);
            builder.Ignore(v => v.CurrentUser);
            builder.Ignore(v => v.CreatedBy);
            builder.Ignore(v => v.CreatedDate);
            builder.Ignore(v => v.ModifiedBy);
            builder.Ignore(v => v.ModifiedDate);
            builder.Ignore(v => v.IsActive);
        }
    }
}
