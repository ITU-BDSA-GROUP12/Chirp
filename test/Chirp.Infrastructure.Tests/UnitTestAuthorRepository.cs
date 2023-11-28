namespace Chirp.Infrastructure.Tests;

public class UnitTestAuthorRepository
{

    [Theory]
    [InlineData("Helge")]
    [InlineData("Rasmus")]
    public async void TestAuthorRepositoryGetAuthorDTOByName(string name)
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
        AuthorValidator author_validator = new AuthorValidator();
        var repository = new AuthorRepository(context, author_validator);

        // Act
        AuthorDto author = await repository.GetAuthorDTOByName(name);

        // Assert
        Assert.Equal(author.Name, name);
    }

    [Theory]
    [InlineData("ropf@itu.dk")]
    [InlineData("rnie@itu.dk")]
    public async void TestAuthorRepositoryGetAuthorDTOByEmail(string email)
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
        AuthorValidator author_validator = new AuthorValidator();
        var repository = new AuthorRepository(context, author_validator);

        // Act
        AuthorDto author = await repository.GetAuthorDTOByEmail(email);

        // Assert
        Assert.Equal(author.Email, email);
    }

    [Theory]
    [InlineData("Helge", "ropf@itu.dk")]
    [InlineData("Rasmus", "rnie@itu.dk")]
    public async void TestAuthorRepositoryCreateAuthor(string name, string email)
    {

        //Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();
        AuthorValidator author_validator = new AuthorValidator();
        var repository = new AuthorRepository(context, author_validator);

        // Act
        await repository.CreateAuthor(name, email);
        AuthorDto newly_created_author = await repository.GetAuthorDTOByEmail(email);

        // Assert
        Assert.Equal(newly_created_author.Email, email);

    }
}