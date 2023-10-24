namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{

    readonly ChirpDBContext _context;
    public AuthorRepository(ChirpDBContext context)
    {
        _context = context;
    }

    public async Task<AuthorDto> GetAuthorByName(string name)
    {
        Author author = _context.Authors.FirstOrDefault(a => a.Name == name);
        return new AuthorDto
        {
            Name = author.Name,
            Email = author.Email,
            AuthorId = author.AuthorId
        };
    }

    public async Task<AuthorDto> GetAuthorByEmail(string email)
    {
        Author author = _context.Authors.FirstOrDefault(a => a.Email == email);
        return new AuthorDto
        {
            Name = author.Name,
            Email = author.Email,
            AuthorId = author.AuthorId
        };
    }

    public async void CreateAuthor(string name, string email)
    {
        _context.Authors.Add(new Author
        {
            AuthorId = Guid.NewGuid(),
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>()
        });
    }
}