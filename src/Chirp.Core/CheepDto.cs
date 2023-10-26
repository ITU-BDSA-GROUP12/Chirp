public record CheepDto
{ //data transfer object (DTO) https://learn.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
    public required string Author { get; set; }
    public required string Message { get; set; }
    public required string Timestamp { get; set; }
}