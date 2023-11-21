namespace Chirp.Infrastructure;
public class CheepRepository : ICheepRepository
{

    readonly ChirpDBContext _context;
    readonly CheepValidator _validator;
    public CheepRepository(ChirpDBContext context, CheepValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<List<CheepDto>> GetCheeps(int page)
    {

        //https://learn.microsoft.com/en-us/dotnet/csharp/linq/write-linq-queries
        return await
            (from Cheep in _context.Cheeps
             orderby Cheep.TimeStamp descending
             select new CheepDto //in LINQ the select clause is responsible for making new objects
             {
                 Author = Cheep.Author.Name,
                 Message = Cheep.Text,
                 Timestamp = $"{Cheep.TimeStamp.Year}-{Cheep.TimeStamp.Month}-{Cheep.TimeStamp.Day} {Cheep.TimeStamp.Hour}:{Cheep.TimeStamp.Minute}:{Cheep.TimeStamp.Second}"
             }).Skip((page - 1) * 32).Take(32).ToListAsync(); //The toListAsync is important because CheepDTO does not have a GetAwaiter
    }

    public async Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author)
    {
        return await
           (from Cheep in _context.Cheeps
            where Cheep.Author.Name == author
            orderby Cheep.TimeStamp descending
            select new CheepDto
            {
                Author = Cheep.Author.Name,
                Message = Cheep.Text,
                Timestamp = $"{Cheep.TimeStamp.Year}-{Cheep.TimeStamp.Month}-{Cheep.TimeStamp.Day} {Cheep.TimeStamp.Hour}:{Cheep.TimeStamp.Minute}:{Cheep.TimeStamp.Second}"
            }).Skip((page - 1) * 32).Take(32).ToListAsync();
    }

    public async Task CreateCheep(string message, AuthorDto user)
    {
        Author? author = _context.Authors.FirstOrDefault(a => a.Email == user.Email);

        var newCheep = new Cheep()
        {
            CheepId = Guid.NewGuid(),
            AuthorId = author.AuthorId,
            Author = author,
            Text = message,
            TimeStamp = DateTime.Now
        };

        FluentValidation.Results.ValidationResult validationResult = _validator.Validate(newCheep);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Attemptted to store invalid Cheep in database.");
        }

        _context.Cheeps.Add(newCheep);

        await _context.SaveChangesAsync();
    }
}
