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
        AuthorDto? author = await repository.GetAuthorDTOByName(name);

        // Assert
        Assert.Equal(author!.Name, name);
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
        AuthorDto? author = await repository.GetAuthorDTOByEmail(email);

        // Assert
        Assert.Equal(author!.Email, email);
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
        AuthorDto? newly_created_author = await repository.GetAuthorDTOByEmail(email);

        // Assert
        Assert.Equal(newly_created_author!.Email, email);
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

        Author? follower = await context.Authors.FirstOrDefaultAsync(a => a.Email == follower_email);
        Author? followed = await context.Authors.FirstOrDefaultAsync(a => a.Name == followed_name);



        await repository.FollowAnAuthor(follower_email, followed_name);


        // Assert

        Assert.Contains(follower!.FollowedAuthors, author => author == followed);

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

        Author? follower = await context.Authors.FirstOrDefaultAsync(a => a.Email == follower_email);
        Author? followed = await context.Authors.FirstOrDefaultAsync(a => a.Name == followed_name);



        await repository.FollowAnAuthor(follower_email, followed_name);
        await repository.UnFollowAnAuthor(follower_email, followed_name);


        // Asser

        Assert.DoesNotContain(followed, follower!.FollowedAuthors);

    }

    [Fact]
    public async void AnyoneFollowingTwoUsersThatBothFollowAThirdUserWillHaveTheThirdRecommendedByTheGetFollersFollowerMethod()
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

        // A follows B and C
        // B and C follow D
        // D is recommended to A


        await repository.CreateAuthor(
            "A name",
            "A email"
        );
        await repository.CreateAuthor(
            "B name",
            "B email"
        );
        await repository.CreateAuthor(
            "C name",
            "C email"
        );
        await repository.CreateAuthor(
            "D name",
            "D email"
        );

        Task _1 = repository.FollowAnAuthor("A email", "B name");
        Task _2 = repository.FollowAnAuthor("A email", "C name");
        Task _3 = repository.FollowAnAuthor("B email", "D name");
        Task _4 = repository.FollowAnAuthor("C email", "D name");
        await _1; await _2; await _3; await _4;

        // Assert

        List<Guid>? list_containing_only_D_Guid = await repository.GetFollowersFollower("A email");
        Assert.NotNull(list_containing_only_D_Guid);

        Guid D_guid = list_containing_only_D_Guid[0];
        string? D_name = await repository.GetAuthorNameByID(D_guid);
        Assert.Equal("D name", D_name);
    }

    [Fact]
    public async void TestDeleteAuthor()
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

        Author? author = await context.Authors.FirstOrDefaultAsync(a => a.Email == "ropf@itu.dk");
        Assert.Contains(context.Authors, testauthor => testauthor == author);
        Assert.Contains(context.Cheeps, cheep => cheep.Author == author);

        // Act

        await repository.DeleteAuthor("ropf@itu.dk");


        // Asser
        Assert.DoesNotContain(context.Authors, testauthor => testauthor == author);
        Assert.DoesNotContain(context.Cheeps, cheep => cheep.Author == author);
    }

    [Fact]
    public async void TestThatAnAuthorRepositoryCanNotStoreInvalidAuthor()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();

        AuthorRepository author_repository = new AuthorRepository(context, new AuthorValidator());

        // Act
        string valid_name = "valid name";
        string valid_email = "valid email";
        string invalid_name = "";
        string invalid_email = "";


        // Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => author_repository.CreateAuthor(valid_name, invalid_email));
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => author_repository.CreateAuthor(invalid_name, valid_email));
    }
}