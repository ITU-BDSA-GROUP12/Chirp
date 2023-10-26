public record AuthorDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public Guid AuthorId { get; set; }
}