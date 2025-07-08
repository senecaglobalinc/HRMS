using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateExitActivityConfiguration : IEntityTypeConfiguration<AssociateExitActivity>
    {
        public void Configure(EntityTypeBuilder<AssociateExitActivity> builder)
        {
            builder.HasKey(e => e.AssociateExitActivityId);
            
            builder
               .Property(c => c.DepartmentId)
               .HasColumnName("DepartmentId");

            builder
               .Property(c => c.NoDues)
               .HasColumnName("NoDues");

            builder
               .Property(c => c.DueAmount)
               .HasColumnName("DueAmount");            

            builder
               .Property(c => c.StatusId)
               .HasColumnName("StatusId");

            builder.Property(e => e.Remarks)
               .HasMaxLength(250)
               .HasColumnType("varchar(250)")
               .IsUnicode(false);

            builder.Property(e => e.AssetsNotHanded)
               .HasMaxLength(250)
               .HasColumnType("varchar(250)")
               .IsUnicode(false);

            builder.HasOne(d => d.AssociateAbscond)
               .WithMany(p => p.AssociateExitActivity)
               .HasForeignKey(d => d.AssociateAbscondId);

            builder.HasOne(d => d.AssociateExit)
               .WithMany(p => p.AssociateExitActivity)
               .HasForeignKey(d => d.AssociateExitId);

            EntityConfiguration.Add(builder);
        }
    }
}
