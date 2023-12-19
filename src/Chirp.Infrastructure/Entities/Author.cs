namespace Chirp.Infrastructure;
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