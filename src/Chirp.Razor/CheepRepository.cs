using Microsoft.EntityFrameworkCore;

public interface ICheepRepository
{
    public Task<List<CheepDto>> GetCheeps(int page);
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author);
}

public class CheepRepository : ICheepRepository{

    readonly ChirpDBContext _context;

    public async Task<List<CheepDto>> GetCheeps(int page){

        return await _context.Cheeps.Join(_context.Authors,
                    Cheep => Cheep.AuthorId,
                    Author => Author.AuthorId,
                    (Cheep, Author) =>
                        new CheepDto
                        {
                            Author = Author.Name,
                            Message = Cheep.Text,
                            Timestamp = Cheep.TimeStamp.ToString()
                        }
                    ).ToListAsync();
    }

    public async Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author){

        return await _context.Cheeps.Join( _context.Authors,
                    Cheep => Cheep.AuthorId,
                    Author => Author.AuthorId,
                    (Cheep, Author) => 
                        new CheepDto{
                            Author = Author.Name,
                            Message = Cheep.Text,
                            Timestamp = Cheep.TimeStamp.ToString()
                         }
                    ).ToListAsync();
    }
}