/// <summary>
/// This class represents a Chirp user. When a user is authenticated we save them as an Author.
/// An author has basic information ID (we use GUID), name & email.
/// An author can _Cheep_, ie. send messages. An authors cheeps are saved here in the Cheeps list.
/// Furthermore, authors can follow each other, which is why we need two lists for followers and the followed users.
/// </summary>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Chirp.Infrastructure;
public class Author
{
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
    public required List<Author> FollowedAuthors { get; set; }
    public required List<Author> AuthorFollowers { get; set; }
}