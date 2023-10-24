using Microsoft.EntityFrameworkCore;

public class CheepRepository : ICheepRepository{

    readonly ChirpDBContext _context;
    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

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
                    ).Skip(page*32).Take(32).ToListAsync();
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
                    ).Where(CheepDto => CheepDto.Author == author ).Skip(page*32).Take(32).ToListAsync();
    }
}