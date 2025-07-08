using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Employee.Database.Configurations
{
    public  class AssociateExitReportConfiguration:IEntityTypeConfiguration<Entities.AssociateExitReport>
    {
        public void Configure(EntityTypeBuilder<Entities.AssociateExitReport> builder)
        {
            builder.ToTable("AssociateExitReport");

            builder.HasNoKey();           
        }
    } 
}