public record AuthorDto
{
    // An immutable record holding only the information about a user required for the view.
    public required string Name { get; set; }
    public required string Email { get; set; }
}