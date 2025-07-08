using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class AddendumConfiguration : IEntityTypeConfiguration<Addendum>
    {
        public void Configure(EntityTypeBuilder<Addendum> builder)
        {
            builder
                .HasKey(e => e.AddendumId);

            builder
                .Property(e => e.AddendumDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.AddendumNo)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.IsActive)
                .HasColumnType("boolean");            

            builder
                .Property(e => e.Note)
                .HasMaxLength(250)
                .HasColumnType("varchar(250)")
                .IsUnicode(false);

            builder
                .Property(e => e.RecipientName)
                .HasMaxLength(30)
                .HasColumnType("varchar(30)")
                .IsUnicode(false);

            builder
                .Property(e => e.SOWId)
                .IsRequired()
                .HasColumnName("SOWId")
                .HasMaxLength(15)
                .HasColumnType("varchar(15)")
                .IsUnicode(false);

            builder
                .Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .HasDefaultValueSql("inet_client_addr()")
                .IsUnicode(false);

            //builder
            //    .HasOne(d => d.IdNavigation)
            //    .WithMany(p => p.Addendum)
            //    .HasForeignKey(d => d.Id)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(d => d.Project)
                .WithMany(p => p.Addendum)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_Addendum_Project")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(d => d.SOW)
                .WithMany(p => p.Addendum)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_Addendum_SOW");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
