using FluentValidation;
namespace Chirp.Infrastructure;

public class CheepValidator : AbstractValidator<Cheep>
{
    public CheepValidator()
    {
        RuleFor(cheep => cheep.CheepId).NotNull();
        RuleFor(cheep => cheep.AuthorId).NotNull();
        RuleFor(cheep => cheep.Author).SetValidator(new AuthorValidator());
        RuleFor(cheep => cheep.Text).Length(1, 160);
    }
}