namespace Chirp.Infrastructure;
public class Cheep
{
    // The Cheep entity, representing a cheep in the application.
    // Not only is it a class holding information about a cheep that we might use in the
    // application, it is also used by the ORM to define structure of the database table holding cheeps.
    public Guid CheepId { get; set; }
    public Guid AuthorId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}

