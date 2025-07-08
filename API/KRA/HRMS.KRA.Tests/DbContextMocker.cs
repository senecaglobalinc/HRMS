using HRMS.KRA.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HRMS.KRA.Tests
{
    public static class DbContextMocker
    {
        public static KRAContext GetKRAClientDbContext(string dbName)
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<KRAContext>()
                            .UseInMemoryDatabase(databaseName: dbName)
                            // don't raise the error warning us that the in memory db doesn't support transactions
                            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                            .Options;
            // Create instance of DbContext
            var dbContext = new KRAContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }
    }
}
