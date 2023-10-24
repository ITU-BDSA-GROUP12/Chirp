public record CheepDto { //data transfer object (DTO) https://learn.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
    public string Author { get; set; }
    public string Message { get; set; }
    public string Timestamp { get; set; }
}