using System.ComponentModel.DataAnnotations;

public class Author
{
    
    public Guid AuthorId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Cheep> Cheeps { get; set; }
}