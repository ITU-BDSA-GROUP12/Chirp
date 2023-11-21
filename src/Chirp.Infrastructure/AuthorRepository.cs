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
            FollowedAuthors = new List<Author>()
        };
        FluentValidation.Results.ValidationResult validationResult = _validator.Validate(newAuthor);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Attempted to store invalid Author in database.");
        }
        _context.Authors.Add(newAuthor);
        await _context.SaveChangesAsync();
    }

    public async Task FollowAnAuthor(string followingEmail, string followedEmail)
    {
        var followingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == followingEmail);
        var followedAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == followedEmail);

        if (followingAuthor.FollowedAuthors.Contains(followedAuthor))
        {
            throw new Exception("User already follows this author");
        } else {
            followingAuthor.FollowedAuthors.Add(followedAuthor);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UnFollowAnAuthor(string followingEmail, string unFollowingEmail)
    {
        var followingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == followingEmail);
        var unFollowingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == unFollowingEmail);
        if(followingAuthor.FollowedAuthors.Contains(unFollowingAuthor)){
            followingAuthor.FollowedAuthors.Remove(unFollowingAuthor);
        } else {
            throw new Exception("User is trying to unfollow an author, they are not following");
        }
        

        await _context.SaveChangesAsync();
    }
}