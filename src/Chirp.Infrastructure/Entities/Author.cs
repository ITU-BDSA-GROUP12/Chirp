using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Chirp.Infrastructure;
public class Author
{   
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required bool IsDeleted { get; set; }
    public required List<Cheep> Cheeps { get; set; }
    public required List<Guid> FollowedAuthors { get; set; }
}