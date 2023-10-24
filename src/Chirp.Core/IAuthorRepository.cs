namespace Chirp.Core;

public interface IAuthorRepository
{
    public Task<AuthorDto> GetAuthorByName(string name);
    public Task<AuthorDto> GetAuthorByEmail(string email);
    public void CreateAuthor(string name, string email);
}