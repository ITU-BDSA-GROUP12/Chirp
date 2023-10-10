using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public int CheepId { get; set; }
    public Aurthor Aurthor { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}