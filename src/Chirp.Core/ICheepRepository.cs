namespace Chirp.Core;

public interface ICheepRepository
{
    public Task<List<CheepDto>> GetCheeps(int page);
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author);
}