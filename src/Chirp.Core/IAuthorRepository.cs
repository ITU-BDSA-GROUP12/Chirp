namespace Chirp.Core;

public interface IAuthorRepository
{
    public Task<AuthorDto?> GetAuthorByName(string name);
    public Task<AuthorDto?> GetAuthorByEmail(string email);
    public Task CreateAuthor(string name, string email);
}