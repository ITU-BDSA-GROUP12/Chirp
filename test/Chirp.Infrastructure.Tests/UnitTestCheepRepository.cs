namespace Chirp.Infrastructure.Tests;

public class UnitTestCheepRepository
{
    //Integration tests for cheepRepository
    [Fact]
    public async void return32CheepsFromGetCheepsTest()
    {
        // A page of cheeps must contain at most 32 cheeps.
        // Therefore, if the database contains more than 32 cheeps, CheepRepository.GetCheeps(int page) must return exactly 32 cheeps.
        // This test verifies that this behaviour is observed.

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
        // If there is less than 32 cheeps in the repository, CheepRepository.GetCheeps(int page) must return the maximum number of cheeps available.
        // This test verifies that this behaviour is observed.

        //Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options);   //Creates a context, and passes in the options.
        await context.Database.EnsureCreatedAsync();
        var a1 = new Author() { AuthorId = Guid.NewGuid(), Name = "Helge", Email = "ropf@itu.dk", Cheeps = new List<Cheep>(), FollowedAuthors = [], AuthorFollowers = [] };
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
        Assert.Single(result);

        context.Database.EnsureDeleted();
    }

    [Fact]
    public async void TestGetCheepsFromAuthor()
    {
        // Verify that GetCheepsFromAuthor returns all cheeps made by a given author.

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
        //To extract the Author Id we have to extract the Author from the context:
        var Author = await context.Authors.FirstOrDefaultAsync(a => a.Email == "ropf@itu.dk");

        List<CheepDto> ExpectedListOfCheeps = new()
        {
            new CheepDto
            {
                AuthorId = Author!.AuthorId,
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
        // Verify that the CheepRepository.CreateCheep(string message , AuthorDto user) method stores cheeps correctly.

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
            FollowedAuthors = new List<Author>(),
            AuthorFollowers = new List<Author>()
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
        // The CheepValidation class uses FluentValidation to assert that a given cheep is valid.
        // The CreateCheep uses this to assert that a given cheep is valid before storing it in the database.
        // This test verifies that an invalid cheep can not be stored in database.

        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();
        CheepRepository cheepRepository = new CheepRepository(context, new CheepValidator());
        AuthorRepository authorRepository = new AuthorRepository(context, new AuthorValidator());
        string valid_email = "valid@email.ending";
        string valid_name = "valid name";

        // Act
        await authorRepository.CreateAuthor(valid_name, valid_email);
        AuthorDto valid_author = await authorRepository.GetAuthorDTOByEmail(valid_email) ?? new AuthorDto { Name = valid_name, Email = valid_email };
        // create various authors and messages that should not be considered valid by the CheepValidator.
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

        // Assert
        // verify that the correct exception is thrown when invalid cheep messages or authors are provided
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(too_short_message, valid_author));
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(too_long_message, valid_author));
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(valid_message, author_with_no_email));
    }

    [Fact]
    public async void TestGetCheepsNotReturnCheepsFromDeletedUsers()
    {
        // If a user is deleted, cheeps from them should no longer be displayed by the app.
        // This test verifies that cheeps from users that are deleted will not be retrieved from database.

        //Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();

        // instantiate repositories
        CheepValidator cheep_validator = new CheepValidator();
        var cheepRepository = new CheepRepository(context, cheep_validator);
        AuthorValidator author_validator = new AuthorValidator();
        var authorRepository = new AuthorRepository(context, author_validator);

        // create author
        var authorName = "testName";
        var authorEmail = "test@email.com";

        await authorRepository.CreateAuthor(authorName, authorEmail);

        var authorDTO = await authorRepository.GetAuthorDTOByEmail("test@email.com");

        // create a cheep from newly created author
        await cheepRepository.CreateCheep("TestCheep for getCheepDeleteTest", authorDTO!);

        // Act


        // get cheeps written from newly created author, and verify that the newly created cheep was stored correctly

        var result = await cheepRepository.GetCheeps(0);
        Assert.Contains(result, cheepDto => cheepDto.Author == authorDTO!.Name);

        // delete the newly created author. the cheep should no longer be retrievable.
        await authorRepository.DeleteAuthor(authorEmail);

        result = await cheepRepository.GetCheeps(0);

        // Assert

        // Verify that the newly created cheep is not retrieved, when the newly deleted author is used for retrieval.
        Assert.DoesNotContain(result, cheepDto => cheepDto.Author == authorDTO!.Name);
    }

    [Fact]
    public async void TestGetCheepsFromAuthorNotReturnCheepsFromDeletedUsers()
    {


        //Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        // instantiate repositories
        await context.Database.EnsureCreatedAsync();
        CheepValidator cheep_validator = new CheepValidator();
        var cheepRepository = new CheepRepository(context, cheep_validator);
        AuthorValidator author_validator = new AuthorValidator();
        var authorRepository = new AuthorRepository(context, author_validator);


        var authorName = "testName";
        var authorEmail = "test@email.com";

        await authorRepository.CreateAuthor(authorName, authorEmail);

        var authorDTO = await authorRepository.GetAuthorDTOByEmail("test@email.com");
        await cheepRepository.CreateCheep("TestCheep for getCheepFromAuthorDeleteTest", authorDTO!);

        // Act

        var result = await cheepRepository.GetCheepsFromAuthor(0, authorName);
        Assert.Contains(result, cheepDto => cheepDto.Author == authorDTO!.Name);

        await authorRepository.DeleteAuthor(authorEmail);

        result = await cheepRepository.GetCheepsFromAuthor(0, authorName);

        // Asser
        Assert.DoesNotContain(result, cheepDto => cheepDto.Author == authorDTO!.Name);
    }

    [Fact]
    public async void TestGetCheepsUserTimelineNotReturnCheepsFromDeletedUsers()
    {
        //Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();
        CheepValidator cheep_validator = new CheepValidator();
        var cheepRepository = new CheepRepository(context, cheep_validator);
        AuthorValidator author_validator = new AuthorValidator();
        var authorRepository = new AuthorRepository(context, author_validator);

        var authorName = "testName";
        var authorEmail = "test@email.com";

        var authorName2 = "testName2";
        var authorEmail2 = "test2@email.com";

        await authorRepository.CreateAuthor(authorName, authorEmail);
        await authorRepository.CreateAuthor(authorName2, authorEmail2);

        var authorDTO = await authorRepository.GetAuthorDTOByEmail("test@email.com");
        await cheepRepository.CreateCheep("TestCheep for getCheepUserTimeline1DeleteTest", authorDTO!);

        var authorDTO2 = await authorRepository.GetAuthorDTOByEmail("test2@email.com");
        await cheepRepository.CreateCheep("TestCheep for getCheepUserTimeline2DeleteTest", authorDTO2!);

        await authorRepository.FollowAnAuthor(authorEmail, authorName2);

        var author1 = await context.Authors.FirstOrDefaultAsync(a => a.Email == authorEmail);

        List<Guid> followedAuthorsId = new();

        foreach (var followedAuthor in author1!.FollowedAuthors)
        {
            followedAuthorsId.Add(followedAuthor.AuthorId);
        }

        // Act

        var result = await cheepRepository.GetCheepsUserTimeline(0, authorName, followedAuthorsId);
        Assert.Contains(result, cheepDto => cheepDto.Author == authorDTO2!.Name);

        await authorRepository.DeleteAuthor(authorEmail2);

        result = await cheepRepository.GetCheepsUserTimeline(0, authorName, followedAuthorsId);

        // Assert
        Assert.DoesNotContain(result, cheepDto => cheepDto.Author == authorDTO2!.Name);
    }

    [Fact]
    public async void TestGetUserTimeline()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();
        CheepValidator cheep_validator = new CheepValidator();
        var cheepRepository = new CheepRepository(context, cheep_validator);
        AuthorValidator author_validator = new AuthorValidator();
        var authorRepository = new AuthorRepository(context, author_validator);

        var authorNameA = "A testName";
        var authorEmailA = "A testA@email.com";

        var authorNameB = "B testName";
        var authorEmailB = "B test@email.com";

        await authorRepository.CreateAuthor(authorNameA, authorEmailA);
        await authorRepository.CreateAuthor(authorNameB, authorEmailB);

        await authorRepository.FollowAnAuthor(authorEmailA, authorNameB);

        AuthorDto? authorA = await authorRepository.GetAuthorDTOByEmail(authorEmailA);
        Assert.NotNull(authorA);
        AuthorDto? authorB = await authorRepository.GetAuthorDTOByEmail(authorEmailB);
        Assert.NotNull(authorB);

        await cheepRepository.CreateCheep("Hello A", authorA);
        await cheepRepository.CreateCheep("Hello b", authorB);

        // Act
        List<Guid>? followersA = await authorRepository.GetFollowedAuthors(authorEmailA);
        List<Guid>? followersB = await authorRepository.GetFollowedAuthors(authorEmailB);

        Assert.NotNull(followersA);
        Assert.NotNull(followersB);


        List<CheepDto> cheepsA = await cheepRepository.GetCheepsUserTimeline(0, authorNameA, followersA);
        List<CheepDto> cheepsB = await cheepRepository.GetCheepsUserTimeline(0, authorNameB, followersB);

        // Assert
        Assert.Equal(2, cheepsA.Count);
        Assert.Single(cheepsB);
    }

    [Fact]
    public async void TestGetCheepsFromAnAuthor()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:"); //Configuring connenction using in-memory connectionString
        connection.Open(); // Open the connection. (So EF Core doesnt close it automatically)

        var options = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(connection)
            .Options; //Create an instance of DBConnectionOptions, and configure it to use SQLite connection.

        using var context = new ChirpDBContext(options); //Creates a context, and passes in the options.

        await context.Database.EnsureCreatedAsync();
        CheepValidator cheep_validator = new CheepValidator();
        var cheepRepository = new CheepRepository(context, cheep_validator);
        AuthorValidator author_validator = new AuthorValidator();
        var authorRepository = new AuthorRepository(context, author_validator);

        var authorName = "testName";
        var authorEmail = "test@email.com";
        await authorRepository.CreateAuthor(authorName, authorEmail);


        var authorDTO = await authorRepository.GetAuthorDTOByEmail("test@email.com");
        Assert.NotNull(authorDTO);

        // Act
        await cheepRepository.CreateCheep("TestCheep for getCheepUserTimeline1DeleteTest1", authorDTO);
        await cheepRepository.CreateCheep("TestCheep for getCheepUserTimeline1DeleteTest2", authorDTO);
        await cheepRepository.CreateCheep("TestCheep for getCheepUserTimeline1DeleteTest3", authorDTO);
        await cheepRepository.CreateCheep("TestCheep for getCheepUserTimeline1DeleteTest4", authorDTO);
        await cheepRepository.CreateCheep("TestCheep for getCheepUserTimeline1DeleteTest5", authorDTO);
        await cheepRepository.CreateCheep("TestCheep for getCheepUserTimeline1DeleteTest6", authorDTO);


        List<CheepDto> cheeps = await cheepRepository.GetCheepsFromAuthor(0, authorName);

        // Assert
        Assert.Equal(6, cheeps.Count);
    }



}