using System.ComponentModel.DataAnnotations; // For the required fields
namespace Chirp.Infrastructure;
/// <summary>
/// The Cheep entity class represents the message an author can post.
/// When posting/creating a new Cheep the CheepId is given and the AuthorId is saved.
/// To save time when loading Cheeps a reference to the Author itself is saved with the Cheep.
/// Since our code relies on this, we marked the author field as required to avoid runtime 
/// exceptions if someone tried to break the code. Also to maintain databse integrity.
/// The Cheep contains the message => Text field. This field needs to be required as it is a user input. 
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

