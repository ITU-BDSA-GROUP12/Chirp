using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Chirp.Infrastructure;
/// <summary>
/// The Author entity class represents a Chirp user. When a user is authenticated we save them as an Author.
/// An author has basic information ID (we use GUID), name & email.
/// An author can _Cheep_, i.e. send messages. An author's cheeps are saved in 'Cheeps'.
/// Furthermore, authors can follow each other, which is why we need two lists for followers and the followed users.
/// Hvorfor fuck er ting required, nullable?? Warnings?
/// </summary>

public class Author
{
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
    public required List<Author> FollowedAuthors { get; set; }
    public required List<Author> AuthorFollowers { get; set; }
}