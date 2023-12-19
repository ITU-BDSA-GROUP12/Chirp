namespace Chirp.Infrastructure;

public class CheepValidator : AbstractValidator<Cheep>
{
    // The CheepValidator class, used to validate a Cheep entity before storing it in the database.
    public CheepValidator()
    {
        // Set up the rules that a valid Cheep entity must follow.
        RuleFor(cheep => cheep.CheepId).NotNull();
        RuleFor(cheep => cheep.AuthorId).NotNull();
        RuleFor(cheep => cheep.Author).SetValidator(new AuthorValidator());
        RuleFor(cheep => cheep.Text).Length(1, 160);
    }
}