public record CheepDto
{ //data transfer object (DTO) https://learn.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
    // An immutable record holding only the information about a cheep required for the view.
    public required Guid AuthorId { get; set; }
    public required string Author { get; set; }
    public required string Message { get; set; }
    public required string Timestamp { get; set; }
}