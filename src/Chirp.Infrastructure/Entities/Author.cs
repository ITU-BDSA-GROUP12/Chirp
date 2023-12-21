namespace Chirp.Infrastructure;
/// <summary>
/// The Author entity class represents a Chirp user. When a user is authenticated we save them as an Author.
/// An author has basic information ID (we use GUID), name & email.
/// An author can Cheep, i.e. post messages. An author's cheeps are saved in 'Cheeps'.
/// Authors can follow each other, why we need lists for followers and followed users.
/// </summary>

public class Author
{
    // The Author entity, representing a user of the application.
    // Not only is it a class holding information about an author that we might use in the 
    // application, it is also used by the ORM to define structure of the database table holding authors.
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
    public required List<Author> FollowedAuthors { get; set; }
    public required List<Author> AuthorFollowers { get; set; }
}