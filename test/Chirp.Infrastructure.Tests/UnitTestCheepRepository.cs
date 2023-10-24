namespace Chirp.Infrastructure.Tests;

public class UnitTestCheepRepository
{

    [Fact]
    public async void UnitTestCreateCheep()
    {

        //Arrange
        var text = "Hej dette er en test cheep";
        var user = new AuthorDto
        {
            AuthorId = Guid.NewGuid(),
            Name = "Testperson",
            Email = "Test@mail.haps"
        };

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
        await repository.CreateCheep(text, user);
        Cheep LatestCheep = context.Cheeps.OrderByDescending(c => c.TimeStamp).FirstOrDefault(); // pulls out the latest created cheep

        // Assert
        Assert.Equal(text, LatestCheep.Text);
    }
}