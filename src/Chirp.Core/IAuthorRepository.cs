
namespace Chirp.Core;

public interface IAuthorRepository
{
    public Task<AuthorDto?> GetAuthorDTOByName(string name);
    public Task<AuthorDto?> GetAuthorDTOByEmail(string email);
    public Task CreateAuthor(string name, string email);
    public Task FollowAnAuthor(string followingEmail, string followedEmail);
    public Task UnFollowAnAuthor(string followingEmail, string unFollowingEmail);
    public Task<List<Guid>?> GetFollowedAuthors(string? authorEmail);
}