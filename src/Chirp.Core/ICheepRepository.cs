namespace Chirp.Core;

public interface ICheepRepository
{
    // The interface for the CheepRepository class, exposing the methods for manipulating the cheeps in the database.
    public Task<List<CheepDto>> GetCheeps(int page);
    public Task<bool> HasNextPageOfPublicTimeline(int page);
    public Task<bool> HasNextPageOfPrivateTimeline(int page, string UserName, List<Guid> authorIds);
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author);
    public Task<List<CheepDto>> GetCheepsUserTimeline(int page, string UserName, List<Guid> authorIds);

    public Task CreateCheep(string message, AuthorDto user);
    public Task<CheepDto?> GetFirstCheepFromAuthor(Guid authorId);

}