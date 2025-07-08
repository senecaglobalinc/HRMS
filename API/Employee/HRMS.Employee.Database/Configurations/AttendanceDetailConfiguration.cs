using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Employee.Database.Configurations
{
    public class AttendanceDetailConfiguration : IEntityTypeConfiguration<AttendanceDetail>
    {
        public void Configure(EntityTypeBuilder<AttendanceDetail> builder)
        {
            builder.ToTable("AttendanceDetail");            

            builder.Ignore(v => v.SystemInfo);
            builder.Ignore(v => v.CurrentUser);
        }
    }
}
