using FluentValidation;
namespace Chirp.Infrastructure;

public class AuthorValidator : AbstractValidator<Author>
{
    public AuthorValidator()
    {
        RuleFor(author => author.AuthorId).NotNull();
        RuleFor(author => author.Name).NotEmpty();
        RuleFor(author => author.Email).NotEmpty();
    }
}