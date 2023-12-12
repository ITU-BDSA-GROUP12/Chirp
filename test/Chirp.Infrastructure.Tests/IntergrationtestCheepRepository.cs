
using Azure;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Ocsp;
using Testcontainers.MsSql;

namespace Chirp.test.Chirp.Infrastructure.Tests;

public class IntergrationtestCheepRepository : IAsyncLifetime
{
    private readonly MsSqlContainer _container;

    readonly CheepValidator _cheepValidator;
    readonly AuthorValidator _authorValidator;

    public Author authorTest { get; set; }

    public IntergrationtestCheepRepository()
    {
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

        authorTest = authors[0];

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
        var followingAuthorTest = await authorRepository.GetFollowedAuthors(authorTest.Email) ?? new List<Guid>();
        var result = await cheepRepository.HasNextPageOfPrivateTimeline(3,authorTest.Name, followingAuthorTest);
        var check = await cheepRepository.GetCheepsUserTimeline(4, authorTest.Name, followingAuthorTest);
        
        
        //Assert
        if(check.Count == 0){
            result.Should().BeFalse();
        } else{
            result.Should().BeTrue();
        }
        
 }


    
}