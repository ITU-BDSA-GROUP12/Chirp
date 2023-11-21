namespace Chirp.Core;

public interface IAuthorRepository
{
    public Task<AuthorDto?> GetAuthorDTOByName(string name);
    public Task<AuthorDto?> GetAuthorDTOByEmail(string email);
    public Task CreateAuthor(string name, string email);
}