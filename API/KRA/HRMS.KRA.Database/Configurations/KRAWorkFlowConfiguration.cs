using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    public class KRAWorkFlowConfiguration : IEntityTypeConfiguration<KRAWorkFlow>
    {
        public void Configure(EntityTypeBuilder<KRAWorkFlow> builder)
        {
            builder.ToTable("KRAWorkFlow");
            builder.HasKey(wf => wf.KRAWorkFlowId);

            builder
               .Property(wf => wf.FinancialYearId)
               .HasColumnType("int")
               .IsRequired();

            builder
               .Property(wf => wf.RoleTypeId)
               .HasMaxLength(255)
               .HasColumnType("int");

            builder
              .Property(wf => wf.StatusId)
                .HasColumnType("int")
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
            builder.Ignore(wf => wf.IsActive);

        }
    }
}
