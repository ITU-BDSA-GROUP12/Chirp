public record AuthorDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Guid AuthorId { get; set; }
}