using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Chirp.Infrastructure;
public class Author
{   
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    [Key]
    public required string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
}