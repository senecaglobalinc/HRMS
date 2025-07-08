using HRMS.Admin.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Tests
{
    public static class DbContextMocker
    {
        public static AdminContext GetAdminClientDbContext(string dbName)
        {
            
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<AdminContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Create instance of DbContext
            var dbContext = new AdminContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }
    }
}
