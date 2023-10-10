using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}

public class CheepDto { //data transfer object (DTO) https://learn.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
    public string Author { get; set; }
    public string Message { get; set; }
    public string Timestamp { get; set; }
}