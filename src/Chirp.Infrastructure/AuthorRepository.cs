namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{

    readonly ChirpDBContext _context;
    public AuthorRepository(ChirpDBContext context)
    {
        _context = context;
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
        _context.Authors.Add(new Author
        {
            AuthorId = Guid.NewGuid(),
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>()
        });
        await _context.SaveChangesAsync();
    }
}