namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{

    readonly ChirpDBContext _context;
    readonly AuthorValidator _validator;
    public AuthorRepository(ChirpDBContext context, AuthorValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<AuthorDto?> GetAuthorDTOByName(string name)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == name);
        if (author == null)
        {
            return null;
        }
        return new AuthorDto
        {
            Name = author.Name,
            Email = author.Email
        };
    }

    public async Task<string> GetAuthorNameByID(Guid id)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
        if (author == null)
        {
            return null;
        }
        
        return author.Name;
    }
    public async Task<AuthorDto?> GetAuthorDTOByEmail(string email)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.Email == email);
        if (author == null)
        {
            return null;
        }
        return new AuthorDto
        {
            Name = author.Name,
            Email = author.Email
        };
    }

    public async Task CreateAuthor(string name, string email)
    {
        Author newAuthor = new()
        {
            AuthorId = Guid.NewGuid(),
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>(),
            FollowedAuthors = new List<Guid>()
        };
        FluentValidation.Results.ValidationResult validationResult = _validator.Validate(newAuthor);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Attempted to store invalid Author in database.");
        }
        _context.Authors.Add(newAuthor);
        await _context.SaveChangesAsync();
    }

   public async Task FollowAnAuthor(string followingEmail, string followedName)
{
    var followingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == followingEmail);
    var followedAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Name == followedName);

    // Check if either followingAuthor or followedAuthor is null
    if (followingAuthor == null || followedAuthor == null)
    {
        throw new Exception("One or both authors not found");
    }

    // Ensure that FollowedAuthors collection is initialized
    followingAuthor.FollowedAuthors ??= new List<Guid>();

    // Check if the followedAuthor is not already in the FollowedAuthors collection
    if (!followingAuthor.FollowedAuthors.Contains(followedAuthor.AuthorId))
    {
        followingAuthor.FollowedAuthors.Add(followedAuthor.AuthorId);
        await _context.SaveChangesAsync();
    }
    else
    {
        throw new Exception("User already follows this author");
    }
}

public async Task UnFollowAnAuthor(string followingEmail, string unFollowingName)
{
    var followingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == followingEmail);
    var unFollowingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Name == unFollowingName);

    // Check if either followingAuthor or unFollowingAuthor is null
    if (followingAuthor == null || unFollowingAuthor == null)
    {
        throw new Exception("One or both authors not found");
    }

    // Ensure that FollowedAuthors collection is initialized
    followingAuthor.FollowedAuthors ??= new List<Guid>();

    // Check if the unFollowingAuthor is in the FollowedAuthors collection
    if (followingAuthor.FollowedAuthors.Contains(unFollowingAuthor.AuthorId))
    {
        followingAuthor.FollowedAuthors.Remove(unFollowingAuthor.AuthorId);
        await _context.SaveChangesAsync();
    }
    else
    {
        throw new Exception("User is trying to unfollow an author they are not following");
    }
}

    public async Task<List<Guid>?> GetFollowedAuthors(string? authorEmail)
    {
        if (authorEmail == null)
        {
            return null;
        }

        var author = await _context.Authors
            .Where(a => a.Email == authorEmail)
            .FirstOrDefaultAsync();

        if (author == null)
        {
            return null;
        }

        List<Guid> followedAuthors = new();
        foreach (var followedAuthorId in author.FollowedAuthors)
        {
            // Fetch the corresponding Author entity for each Guid in FollowedAuthors
            var followedAuthor = await _context.Authors.FindAsync(followedAuthorId);
            if (followedAuthor != null)
            {
                followedAuthors.Add(followedAuthor.AuthorId);
            }
        }

        return followedAuthors;
    }

}