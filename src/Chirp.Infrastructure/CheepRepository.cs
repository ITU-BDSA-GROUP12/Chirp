namespace Chirp.Infrastructure;
public class CheepRepository : ICheepRepository{

    readonly ChirpDBContext _context;
    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

    public async Task<List<CheepDto>> GetCheeps(int page){

                    //https://learn.microsoft.com/en-us/dotnet/csharp/linq/write-linq-queries
        return await 
            (from Cheep in _context.Cheeps
            orderby Cheep.TimeStamp descending
            select new CheepDto //in LINQ the select clause is responsible for making new objects
            {
                Author = Cheep.Author.Name,
                Message = Cheep.Text,
                Timestamp = Cheep.TimeStamp.ToString()
            }).Skip(page*32).Take(32).ToListAsync(); //The toListAsync is important because CheepDTO does not have a GetAwaiter
    }

    public async Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author){

        return await
            (from Cheep in _context.Cheeps
            where Cheep.Author.Name == author
            orderby Cheep.TimeStamp descending
            select new CheepDto
            {
                Author = Cheep.Author.Name,
                Message = Cheep.Text,
                Timestamp = Cheep.TimeStamp.ToString()
            }).Skip(page*32).Take(32).ToListAsync();
    }
}
