namespace Chirp.Web.Tests;

public class IntegrationTest
{

    //Integration tests for cheepRepository
    [Fact]
    public async void return32CheepsFromGetCheepsTest()
    {

        //Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context); //Seed the database.
        var repository = new CheepRepository(context);

        // Act
        List<CheepDto> result = await repository.GetCheeps(0);

        // Assert
        Assert.Equal(32, result.Count);


    }

    [Fact]
    public async void returnAllCheepsIfLessThan32Cheeps()
    {

        //Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options);   //Creates a context, and passes in the options.
        await context.Database.EnsureCreatedAsync();
        var a1 = new Author() { AuthorId = Guid.NewGuid(), Name = "Helge", Email = "ropf@itu.dk", Cheeps = new List<Cheep>() };
        var c1 = new Cheep() { CheepId = Guid.NewGuid(), AuthorId = a1.AuthorId, Author = a1, Text = "Hello, BDSA students!", TimeStamp = DateTime.Parse("2023-08-01 12:16:48") };
        a1.Cheeps = new List<Cheep>() { c1 };
        context.Authors.AddRange(new List<Author>() { a1 });
        context.Cheeps.AddRange(new List<Cheep>() { c1 });
        context.SaveChanges();
        var repository = new CheepRepository(context);

        // Act
        List<CheepDto> result = await repository.GetCheeps(0);

        // Assert
        Assert.Equal(1, result.Count);

        context.Database.EnsureDeleted();
    }
}