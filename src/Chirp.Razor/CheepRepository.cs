public interface ICheepRepository
{
    public Task<List<CheepDto>> GetCheeps(int page);
    public Task< List<CheepDto>> GetCheepsFromAuthor(int page, string author);
}

public class CheepRepository : ICheepRepository{

    readonly CheepDBContext _context;

    public async Task<List<CheepDto>> GetCheeps(int page){

        return await _context.Cheeps
                        .where(c => !c.isDeleted)
                        .select (c => new CheepDto{
                            Author = c.Author,
                            Message = c.Message,
                            Timestamp = c.Timestamp.toString()
                        })
                        .ToListAsync();
    }

    public async Task< List<CheepDto>> GetCheepsFromAuthor(int page, string author){
        return await _context.Cheeps
                        .where(c => !c.isDeleted)
                        .select (c => new CheepDto{
                            Author = c.Author,
                            Message = c.Message,
                            Timestamp = c.Timestamp.toString()
                        })
                        .ToListAsync();
    }
}