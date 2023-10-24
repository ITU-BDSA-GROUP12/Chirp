using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public Guid CheepId { get; set; }
    public Guid AuthorId { get; set; }
    public Author Author { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}