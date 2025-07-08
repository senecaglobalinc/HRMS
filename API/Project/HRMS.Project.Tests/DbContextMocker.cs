using HRMS.Project.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRMS.Project.Tests
{
    public static class DbContextMocker
    {
        
        public  static ProjectDBContext GetProjectDbContext(string dbName)
        {
            
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<ProjectDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            // Create instance of DbContext
           var dBContext = new ProjectDBContext(options);
            dBContext.Database.EnsureCreated();
            // Add entities in memory
            dBContext.Seed();

            return dBContext;
        }
        
    }
    
}
