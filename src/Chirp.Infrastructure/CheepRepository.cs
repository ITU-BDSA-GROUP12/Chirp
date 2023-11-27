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
                AuthorId = Cheep.AuthorId,
                Author = Cheep.Author.Name,
                Message = Cheep.Text,
                Timestamp = Cheep.TimeStamp.ToString().Split(new char[] { '.', })[0]
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
                AuthorId = Cheep.AuthorId,
                Author = Cheep.Author.Name,
                Message = Cheep.Text,
                Timestamp = Cheep.TimeStamp.ToString().Split(new char[] { '.', })[0]
            }).Skip((page - 1) * 32).Take(32).ToListAsync();
    }

    public async Task<List<CheepDto>> GetCheepsUserTimeline(int page, string UserName, List<Guid> authorIds)
    {
        List<CheepDto> cheepList = await (from cheep in _context.Cheeps
                            where authorIds.Contains(cheep.AuthorId) || cheep.Author.Name == UserName
                            orderby cheep.TimeStamp descending
                            select new CheepDto
                            {
                                AuthorId = cheep.AuthorId,
                                Author = cheep.Author.Name,
                                Message = cheep.Text,
                                Timestamp = cheep.TimeStamp.ToString().Split(new char[] { '.', })[0]
                            })
                            .Skip((page - 1) * 32)
                            .Take(32)
                            .ToListAsync();

        return cheepList;
    }

    public async Task CreateCheep(string message, AuthorDto user)
    {
        Author? author = _context.Authors.FirstOrDefault(a => a.Email == user.Email);
        if (author == null)
        {
            throw new ValidationException($"User with name {user.Name} and email {user.Email} is not in the database.");
        }
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
