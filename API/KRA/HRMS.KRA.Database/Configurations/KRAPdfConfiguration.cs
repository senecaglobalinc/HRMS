using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    public class KRAPdfConfiguration : IEntityTypeConfiguration<KRAPdf>
    {
        public void Configure(EntityTypeBuilder<KRAPdf> builder)
        {
            builder.ToTable("KRAPdf");
            builder.HasKey(wf => wf.KRAPdfId);

            builder
               .Property(wf => wf.FinancialYear)
               .HasColumnType("varchar(100)")
               .IsRequired();

            builder
               .Property(wf => wf.EmployeeCode)
               .HasMaxLength(255)
               .HasColumnType("varchar(50)");

            builder
              .Property(wf => wf.EmployeeEmail)
                .HasColumnType("varchar(250)")
                .IsRequired();

            builder
              .Property(wf => wf.CreatedBy)
              .HasMaxLength(100)
              .HasColumnType("varchar(100)");

            builder
                .Property(wf => wf.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(wf => wf.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(wf => wf.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(wf => wf.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(wf => wf.CurrentUser);            

        }
    }
}
