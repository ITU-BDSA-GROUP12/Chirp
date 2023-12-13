namespace Chirp.test.Chirp.Infrastructure.Tests;

public class IntergrationtestCheepRepository : IAsyncLifetime
{
    private readonly MsSqlContainer _container;

    private readonly CheepValidator _cheepValidator;
    private readonly AuthorValidator _authorValidator;

    public Author? AuthorTest { get; set; }

    public IntergrationtestCheepRepository()
    {
        _cheepValidator = new CheepValidator();
        _authorValidator = new AuthorValidator();
        _container = new MsSqlBuilder()
                        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                        .Build();
    }
    public async Task DisposeAsync() => await _container.DisposeAsync();
    

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>()
                    .UseSqlServer(_container.GetConnectionString());
        using var context = new ChirpDBContext(optionsBuilder.Options);
        await context.Database.MigrateAsync();

        var authorFaker = new Faker<Author>()
            .RuleFor(c => c.AuthorId, f => Guid.NewGuid())
            .RuleFor(c => c.Name, f => f.Person.UserName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Cheeps, new List<Cheep>())
            .RuleFor(c => c.FollowedAuthors, new List<Author>())
            .RuleFor(c => c.AuthorFollowers, new List<Author>());
        var authors = authorFaker.Generate(10);

        AuthorTest = authors[0];

        var cheepFaker = new Faker<Cheep>()
            .RuleFor(c => c.Author, f => f.PickRandom(authors))
            .RuleFor(c => c.Text, f => f.Lorem.Sentence())
            .RuleFor(c => c.TimeStamp, f => f.Date.Past())
            .RuleFor(c => c.Author, f => f.PickRandom(authors));
        var cheeps = cheepFaker.Generate(100);

        context.Cheeps.AddRange(cheeps);
        await context.SaveChangesAsync();
    }

    [Fact]
    [Trait("Category", "Intergration")]

    public async Task ReadFirst32Cheeps(){
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlServer(_container.GetConnectionString());
        using var context = new ChirpDBContext(optionsBuilder.Options);
        var cheepRepository = new CheepRepository(context, _cheepValidator);
        var authorRepository = new AuthorRepository(context, _authorValidator);

        //Act
        var cheeps = await cheepRepository.GetCheeps(1);

        //
        cheeps.Count.Should().Be(32);
    }
    
    [Fact]
    public async Task TestOfIsThereNextPagePublicTimeline(){
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlServer(_container.GetConnectionString());
        using var context = new ChirpDBContext(optionsBuilder.Options);
        var repository = new CheepRepository(context, _cheepValidator);

        //Act
        var result = await repository.HasNextPageOfPublicTimeline(4);
        var amountOfCheeps = await repository.GetCheeps(4);
        //
        result.Should().BeFalse();
    }

        [Fact]
    public async Task TestOfIsThereNextPageOfPrivateTimeline(){
        //Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlServer(_container.GetConnectionString());
        using var context = new ChirpDBContext(optionsBuilder.Options);
        var cheepRepository = new CheepRepository(context, _cheepValidator);
        var authorRepository = new AuthorRepository(context, _authorValidator);

        //Act
        var followingAuthorTest = await authorRepository.GetFollowedAuthors(AuthorTest!.Email) ?? new List<Guid>();
        var result = await cheepRepository.HasNextPageOfPrivateTimeline(3,AuthorTest.Name, followingAuthorTest);
        var check = await cheepRepository.GetCheepsUserTimeline(4, AuthorTest.Name, followingAuthorTest);
        
        
        //Assert
        if(check.Count == 0){
            result.Should().BeFalse();
        } else{
            result.Should().BeTrue();
        }
        
    }
    [Fact]
    public async void TestEnsureFirstCheepFromAuthorReturnsLatestCheepFromThatAuthor() {
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

        await authorRepository.CreateAuthor(authorName, authorEmail);
        Guid authorId = context.Authors.Where(a => a.Email == authorEmail).Select(a => a.AuthorId).FirstOrDefault();
        AuthorDto? authorDto = await authorRepository.GetAuthorDTOByEmail(authorEmail);


        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message1", TimeStamp = DateTime.Now.AddDays(1), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message2", TimeStamp = DateTime.Now.AddDays(2), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message3", TimeStamp = DateTime.Now.AddDays(3), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message4", TimeStamp = DateTime.Now.AddDays(4), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message9", TimeStamp = DateTime.Now.AddDays(9), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message6", TimeStamp = DateTime.Now.AddDays(6), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message7", TimeStamp = DateTime.Now.AddDays(7), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message8", TimeStamp = DateTime.Now.AddDays(8), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        context.Cheeps.Add(new Cheep { CheepId = Guid.NewGuid(), Text = "message5", TimeStamp = DateTime.Now.AddDays(5), AuthorId = authorId, Author = context.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault()! });
        await cheepRepository.CreateCheep("message9", authorDto!);
        
        //Act
        var result = await cheepRepository.GetFirstCheepFromAuthor(authorId);

        //Assert
        result?.Message.Should().Be("message9");

    }

    [Fact]
    public async void TestTheAuthorWithMostIncommenFollowersIsRecommendedFirst() {
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

        var authorNameA = "A testName";
        var authorNameB = "B testName";
        var authorNameC = "C testName";
        var authorNameD = "D testName";
        var authorNameE = "E testName";
        var authorNameF = "F testName";
        var authorNameG = "G testName";
        var authorNameH = "H testName";
        var authorEmailA = "A testEmail";
        var authorEmailB = "B testEmail";
        var authorEmailC = "C testEmail";
        var authorEmailD = "D testEmail";
        var authorEmailE = "E testEmail";
        var authorEmailF = "F testEmail";
        var authorEmailG = "G testEmail";
        var authorEmailH = "H testEmail";

        await authorRepository.CreateAuthor(authorNameA, authorEmailA);
        await authorRepository.CreateAuthor(authorNameB, authorEmailB);
        await authorRepository.CreateAuthor(authorNameC, authorEmailC);
        await authorRepository.CreateAuthor(authorNameD, authorEmailD);
        await authorRepository.CreateAuthor(authorNameE, authorEmailE);
        await authorRepository.CreateAuthor(authorNameF, authorEmailF);
        await authorRepository.CreateAuthor(authorNameG, authorEmailG);
        await authorRepository.CreateAuthor(authorNameH, authorEmailH);

        await authorRepository.FollowAnAuthor(authorEmailA, authorNameB);
        await authorRepository.FollowAnAuthor(authorEmailA, authorNameC);
        await authorRepository.FollowAnAuthor(authorEmailA, authorNameD);

        await authorRepository.FollowAnAuthor(authorEmailB, authorNameE);
        await authorRepository.FollowAnAuthor(authorEmailB, authorNameF);
        await authorRepository.FollowAnAuthor(authorEmailB, authorNameG);

        await authorRepository.FollowAnAuthor(authorEmailC, authorNameE);
        await authorRepository.FollowAnAuthor(authorEmailC, authorNameF);
    

        await authorRepository.FollowAnAuthor(authorEmailD, authorNameE);
       

        List<Guid>? authorIds = new List<Guid>();  
        authorIds = await authorRepository.GetFollowersFollower(authorEmailA);

        //Act
        var resultLength = authorIds!.Count;
        var resultFirstPlace = await authorRepository.GetAuthorNameByID(authorIds[0]);
        var resultSecondPlace = await authorRepository.GetAuthorNameByID(authorIds[1]);


        //Assert
        resultLength.Should().Be(2);
        resultFirstPlace.Should().Be(authorNameE);
        resultSecondPlace.Should().Be(authorNameF);
    }


    
}