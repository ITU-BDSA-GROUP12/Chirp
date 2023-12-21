namespace Chirp.Infrastructure;

public class AuthorValidator : AbstractValidator<Author>
{
    // A class to validate the instances of the Author entity before they are stored in the database.
    public AuthorValidator()
    {
        // Set up the rules that a valid Author entity must follow.
        RuleFor(author => author.AuthorId).NotNull();
        RuleFor(author => author.Name).NotEmpty();
        RuleFor(author => author.Email).NotEmpty();
    }
}