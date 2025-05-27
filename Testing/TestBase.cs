using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;

namespace Testing;

public abstract class TestBase
{
    protected SchoolDbContext GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<SchoolDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new SchoolDbContext(options);
    }

    public void SetupTempData(Controller controller)
    {
        var tempDataProvider = new Mock<ITempDataProvider>();
        controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider.Object);
    }

    protected (SchoolDbContext context, SqliteConnection connection) GetSqliteInMemoryDbContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<SchoolDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new SchoolDbContext(options);
        context.Database.EnsureCreated();

        return (context, connection);
    }
}
