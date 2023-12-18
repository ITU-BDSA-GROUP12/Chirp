using System.ComponentModel.DataAnnotations;
namespace Chirp.Infrastructure;
/// <summary>
/// The Cheep entity class represents the message an author can post.
/// A Cheep has knowledge about its author, being AuthorId and a reference to the Author itself. 
/// The Cheep of course contains the message, which is the Text field. A timestamp for the message is also saved.
/// </summary>


public class Cheep
{
    public Guid CheepId { get; set; }
    public Guid AuthorId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}

