using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client> 
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");

            builder
                .HasKey(c => c.ClientId);

            //builder
            //    .HasIndex(e => e.ClientCode)
            //    .IsUnique();

            //builder
            //    .HasIndex(e => e.ClientName)
            //    .IsUnique();

            builder
                .Property(c => c.ClientCode)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(c => c.ClientRegisterName)
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            builder
               .Property(c => c.ClientNameHash)
               .HasMaxLength(450)
               .HasColumnType("varchar(450)");

            builder
                .Property(c => c.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(c => c.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(c => c.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(c => c.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(c => c.CurrentUser);

        }
    }
}