
using Azure;
using Testcontainers.MsSql;

namespace Chirp.test.Chirp.Infrastructure.Tests;

public class IntergrationtestCheepRepository : IAsyncLifetime
{
    private readonly MsSqlContainer _container;

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
            .RuleFor(c => c.Cheeps, new List<Cheep>());
        var authors = authorFaker.Generate(10);

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
        var repository = new CheepRepository(context);

        //Act
        var cheeps = await repository.GetCheeps(1);

        //
        cheeps.Count.Should().Be(32);
    }

    
}