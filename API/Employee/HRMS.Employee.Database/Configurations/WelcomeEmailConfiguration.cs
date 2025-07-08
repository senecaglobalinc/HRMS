using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WelcomeEmail = HRMS.Employee.Entities.WelcomeEmail;

namespace HRMS.Employee.Database.Configurations
{
    public class WelcomeEmailConfiguration : IEntityTypeConfiguration<WelcomeEmail>
    {
        public void Configure(EntityTypeBuilder<WelcomeEmail> builder)
        {
            builder.ToTable("WelcomeEmail");

            builder
              .HasKey(v => v.Id);

            builder
                .Property(v => v.EmployeeId);


            builder
                .Property(v => v.IsWelcome);


            builder
             .Property(v => v.CreatedBy);
            

            builder
               .Property(v => v.ModifiedBy);
               

            builder
               .Property(v => v.CreatedDate);



            builder
                .Property(v => v.ModifiedDate);
               

            builder
                .Property(v => v.IsActive);
                

            builder.Ignore(v => v.SystemInfo);
            builder.Ignore(v => v.CurrentUser);
        }
    }
}
