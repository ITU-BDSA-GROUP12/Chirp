namespace Chirp.Infrastructure;
public class CheepRepository : ICheepRepository
{
    // The CheepRepository class, implementing the ICheepRepository interface.
    // An object of this class is used by the application to manipulate the cheeps in the database.


    // The database context for the Chirp database, used by the repository to access the database.
    readonly ChirpDBContext _context;

    // Used to validate a Cheep entity before storing it in the database. Ensures that invalid data will not reach the database.
    readonly CheepValidator _validator;

    public CheepRepository(ChirpDBContext context, CheepValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<List<CheepDto>> GetCheeps(int page)
    {
        // Get the cheeps to be displayed on the public timeline on the given page.

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
    public async Task<bool> HasNextPageOfPublicTimeline(int page)
    {
        // Assert whether or not there is a next page of cheeps on the public timeline.
        //https://learn.microsoft.com/en-us/dotnet/csharp/linq/write-linq-queries
        List<CheepDto> cheeps_on_next_page = await GetCheeps(page + 1);
        return cheeps_on_next_page.Any();
    }

    public async Task<bool> HasNextPageOfPrivateTimeline(int page, string UserName, List<Guid> authorIds)
    {
        // Assert whether or not there is a next page of cheeps on the private timeline.
        //https://learn.microsoft.com/en-us/dotnet/csharp/linq/write-linq-queries
        List<CheepDto> cheeps_on_next_page = await GetCheepsUserTimeline(page + 1, UserName, authorIds);
        return cheeps_on_next_page.Any();
    }

    public async Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author)
    {
        // Return all the cheeps from the given author on the given page.
        //we resuse the GetCheepsUserTimeline method because it does the same thing if we pass an empty list of authorIds
        List<CheepDto> cheeps = await GetCheepsUserTimeline(page, author, new List<Guid>());
        return cheeps;
    }

    public async Task<List<CheepDto>> GetCheepsUserTimeline(int page, string UserName, List<Guid> authorIds)
    {
        // If it is not the users timeline, when the authorIds list is empty the method only returns the cheep of the Author.
        // If it is the users timeline, then the authorIds contains all Ids of the Authors who the user follows. 
        // The method will then return the users cheeps and all cheeps from all the authors the user follows.
        // authorIds consist of authorId that the user follows
        List<CheepDto> cheepList = await (from cheep in _context.Cheeps
                                          where (authorIds.Contains(cheep.AuthorId) || cheep.Author.Name == UserName)
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
        // Create a new cheep and store it in the database.

        // The author of the cheep to be stored, from the provided AuthorDto.
        Author? author = _context.Authors.FirstOrDefault(a => a.Email == user.Email);
        if (author == null)
        {
            // If there is no Author in the databsae that corresponds with the data in User, an error has occured. Throw an exception.
            throw new ValidationException($"User with name {user.Name} and email {user.Email} is not in the database.");
        }

        // Instantiate the cheep to be stored with the provided data.
        var newCheep = new Cheep()
        {
            CheepId = Guid.NewGuid(),
            AuthorId = author.AuthorId,
            Author = author,
            Text = message,
            TimeStamp = DateTime.Now
        };

        // Validate the cheep to be stored.
        FluentValidation.Results.ValidationResult validationResult = _validator.Validate(newCheep);

        if (!validationResult.IsValid)
        {
            // If the cheep is not valid, an exception is thrown so now invalid data will not reach the database.
            throw new ValidationException("Attemptted to store invalid Cheep in database.");
        }

        // Update the database with the new cheep.
        _context.Cheeps.Add(newCheep);
        await _context.SaveChangesAsync();
    }

    public async Task<CheepDto?> GetFirstCheepFromAuthor(Guid authorId)
    {
        // Return a CheepDto of the first cheep from the given author.

        Cheep? cheep = await _context.Cheeps.Where(c => c.AuthorId == authorId).OrderByDescending(c => c.TimeStamp).FirstOrDefaultAsync();

        // If the given author has not cheeped yet, just return null. No exception needed.
        if (cheep == null)
        {
            return null;
        }

        // Return the CheepDto of the cheep.
        return new CheepDto
        {
            AuthorId = cheep.AuthorId,
            Author = cheep.Author.Name,
            Message = cheep.Text,
            Timestamp = cheep.TimeStamp.ToString().Split(new char[] { '.', })[0]
        };
    }

}
