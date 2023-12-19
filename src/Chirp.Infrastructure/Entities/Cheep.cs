namespace Chirp.Infrastructure;
/// <summary>
/// The Cheep entity class represents the message an author can post.
/// When posting/creating a new Cheep the CheepId is given and the AuthorId is saved.
/// To save time when loading Cheeps a reference to the Author itself is saved with the Cheep.
/// The Cheep contains the message => Text field.
/// The Timestamp for when the message was send is also saved by the app server.
/// </summary>


public class Cheep
{
    public Guid CheepId { get; set; }
    public Guid AuthorId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}

