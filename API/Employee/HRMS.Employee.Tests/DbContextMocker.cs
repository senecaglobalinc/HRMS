using HRMS.Common.Redis;
using HRMS.Employee.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Tests
{
    public static class DbContextMocker
    {
       
        public static EmployeeDBContext GetEmployeeClientDbContext(string dbName)
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<EmployeeDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Create instance of DbContext
            var dbContext = new EmployeeDBContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }
    }
}
