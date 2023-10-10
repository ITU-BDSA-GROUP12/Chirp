public interface ICheepRepository
{
    public Task<List<Cheep>> GetCheeps(int page);
    public Task< List<Cheep>> GetCheepsFromAuthor(int page, string author);
}

public class CheepRepository : ICheepRepository{

    readonly CheepDBContext _context;

    public async Task<List<Cheep>> GetCheeps(int page){

        return await _context.Cheeps
                        .where(c => !c.isDeleted);
    }

    public async Task< List<Cheep>> GetCheepsFromAuthor(int page, string author){
        return await _context.Cheeps
                        .where(c => !c.isDeleted);
    }
}