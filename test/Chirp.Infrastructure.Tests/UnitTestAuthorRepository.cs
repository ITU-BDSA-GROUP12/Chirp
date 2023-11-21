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
        DbInitializer.SeedDatabase(context); //Seed the database.
        AuthorValidator author_validator = new AuthorValidator();
        var repository = new AuthorRepository(context, author_validator);

        // Act
        repository.CreateAuthor(name, email);
        AuthorDto newly_created_author = await repository.GetAuthorDTOByEmail(email);

        // Assert
        Assert.Equal(newly_created_author.Email, email);
    }

    [Fact]
    public async void TestThatFollowAuthorMethodWorks()
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

        string follower_name = "fan of Bamse";
        string follower_email = "fan-of-Bamse@nonexistentmail.com";
        string followed_name = "Bamse";
        string followed_email = "bamse@nonexistentmail.com";

        await repository.CreateAuthor(followed_name, followed_email);
        await repository.CreateAuthor(follower_name, follower_email);

        Author follower = await context.Authors.FirstOrDefaultAsync(a => a.Email == follower_email);
        Author followed = await context.Authors.FirstOrDefaultAsync(a => a.Email == followed_email);



        repository.FollowAnAuthor(follower_email, followed_email);


        // Assert

        Assert.True(follower.FollowedAuthors.Contains(followed));

    }

    [Fact]
    public async void TestThatUnFollowAuthorMethodWorks()
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

        string follower_name = "fan of Bamse";
        string follower_email = "fan-of-Bamse@nonexistentmail.com";
        string followed_name = "Bamse";
        string followed_email = "bamse@nonexistentmail.com";

        await repository.CreateAuthor(followed_name, followed_email);
        await repository.CreateAuthor(follower_name, follower_email);

        Author follower = await context.Authors.FirstOrDefaultAsync(a => a.Email == follower_email);
        Author followed = await context.Authors.FirstOrDefaultAsync(a => a.Email == followed_email);



        repository.FollowAnAuthor(follower_email, followed_email);
        repository.UnFollowAnAuthor(follower_email, followed_email);


        // Assert

        Assert.False(follower.FollowedAuthors.Contains(followed));

    }
}