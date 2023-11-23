using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using FluentValidation;

namespace Chirp.Infrastructure.Tests;

public class UnitTestCheepRepository
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

        CheepValidator cheep_validator = new CheepValidator();
        var repository = new CheepRepository(context, cheep_validator);

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
        var a1 = new Author() { AuthorId = Guid.NewGuid(), Name = "Helge", Email = "ropf@itu.dk", Cheeps = new List<Cheep>(), FollowedAuthors = new List<Author>() };
        var c1 = new Cheep() { CheepId = Guid.NewGuid(), AuthorId = a1.AuthorId, Author = a1, Text = "Hello, BDSA students!", TimeStamp = DateTime.Parse("2023-08-01 12:16:48") };
        a1.Cheeps = new List<Cheep>() { c1 };
        context.Authors.AddRange(new List<Author>() { a1 });
        context.Cheeps.AddRange(new List<Cheep>() { c1 });
        context.SaveChanges();
        CheepValidator cheep_validator = new CheepValidator();
        var repository = new CheepRepository(context, cheep_validator);

        // Act
        List<CheepDto> result = await repository.GetCheeps(0);

        // Assert
        Assert.Equal(1, result.Count);

        context.Database.EnsureDeleted();
    }

    [Fact]
    public async void TestGetCheepsFromAuthor()
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
        CheepValidator cheep_validator = new CheepValidator();
        var repository = new CheepRepository(context, cheep_validator);

        List<CheepDto> ExpectedListOfCheeps = new()
        {
            new CheepDto
            {
                Author = "Helge",
                Message = "Hello, BDSA students!",
                Timestamp = "2023-08-01 12:16:48"
            }
        };

        // Act
        List<CheepDto> ListOfCheeps = await repository.GetCheepsFromAuthor(0, "Helge");

        // Assert
        Assert.Equal(ExpectedListOfCheeps, ListOfCheeps);
    }

    [Fact]
    public async void UnitTestCreateACheep()
    {

        //Arrange
        var text = "Hej dette er en test cheep";
        var user = new AuthorDto
        {
            Name = "Testperson",
            Email = "Test@mail.haps",
        };

        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context); //Seed the database.
        CheepValidator cheep_validator = new CheepValidator();
        var repository = new CheepRepository(context, cheep_validator);
        //Make sure the test author is in our db
        context.Authors.Add(new Author()
        {
            AuthorId = Guid.NewGuid(),
            Name = "Testperson",
            Email = "Test@mail.haps",
            Cheeps = new List<Cheep>(),
            FollowedAuthors = new List<Author>()
        });
        await context.SaveChangesAsync();

        // Act
        await repository.CreateCheep(text, user);
        Cheep? LatestCheep = context.Cheeps.OrderByDescending(c => c.TimeStamp).FirstOrDefault(); // pulls out the latest created cheep

        // Assert
        if (LatestCheep == null)
        {
            Assert.True(false);
        }
        Assert.Equal(LatestCheep.Text, text);
    }

    [Fact]
    public async void TestThatACheepRepositoryCanNotStoreInvalidCheep()
    {
        // arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();
        CheepRepository cheepRepository = new CheepRepository(context, new CheepValidator());
        AuthorRepository authorRepository = new AuthorRepository(context, new AuthorValidator());
        string valid_email = "valid email";
        string valid_name = "valid name";

        // act
        await authorRepository.CreateAuthor(valid_name, valid_email);
        AuthorDto valid_author = await authorRepository.GetAuthorDTOByEmail(valid_email);
        AuthorDto author_with_no_name = new AuthorDto
        {
            Name = "",
            Email = valid_email
        };
        AuthorDto author_with_no_email = new AuthorDto
        {
            Name = valid_name,
            Email = ""
        };

        string too_short_message = "";
        char[] too_long_char_array = new char[161];
        for (int i = 0; i < 161; i++)
        {
            too_long_char_array[i] = 'A';
        }
        string too_long_message = new string(too_long_char_array);
        string valid_message = "valid message";

        // assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(too_short_message, valid_author));
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(too_long_message, valid_author));
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(valid_message, author_with_no_name));
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(valid_message, author_with_no_email));
    }

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