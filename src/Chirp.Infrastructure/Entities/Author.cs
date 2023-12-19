using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Chirp.Infrastructure;
/// <summary>
/// The Author entity class represents a Chirp user. When a user is authenticated we save them as an Author.
/// An author has basic information ID (we use GUID), name & email.
/// When creating new Authors it is assigned a new GUID which is why the field does not need to be required.
/// An author can Cheep, i.e. post messages. An author's cheeps are saved in 'Cheeps'.
/// Authors can follow each other, why we need lists for followers and followed users.
/// 
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