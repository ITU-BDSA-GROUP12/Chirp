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

    public async Task<AuthorDto?> GetAuthorByName(string name)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == name);
        if (author == null)
        {
            return null;
        }
        return new AuthorDto
        {
            Name = author.Name,
            Email = author.Email,
            AuthorId = author.AuthorId
        };
    }

    public async Task<AuthorDto?> GetAuthorByEmail(string email)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.Email == email);
        if (author == null)
        {
            return null;
        }
        return new AuthorDto
        {
            Name = author.Name,
            Email = author.Email,
            AuthorId = author.AuthorId
        };
    }

    public async Task CreateAuthor(string name, string email)
    {
        Author newAuthor = new()
        {
            AuthorId = Guid.NewGuid(),
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>()
        };
        FluentValidation.Results.ValidationResult validationResult = _validator.Validate(newAuthor);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Attempted to store invalid Author in database.");
        }
        _context.Authors.Add(newAuthor);
        await _context.SaveChangesAsync();
    }
}