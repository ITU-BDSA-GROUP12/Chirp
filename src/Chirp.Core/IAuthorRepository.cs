namespace Chirp.Core;

public interface IAuthorRepository
{
    public AuthorDto GetAuthorByName(string name);
    public AuthorDto GetAuthorByEmail(string email);
    public void CreateAuthor(string name, string email);
}