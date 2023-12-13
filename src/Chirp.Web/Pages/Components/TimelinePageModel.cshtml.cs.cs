using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Components;

public class TimelinePageModel(ICheepRepository repository, IAuthorRepository authorRepository) : PageModel
{

    public ICheepRepository _CheepRepository = repository;
    public IAuthorRepository _AuthorRepository = authorRepository;

    public string validationMessage { get; set; } = "";

    [BindProperty]
    public string? Text { get; set; }

    public async Task<IActionResult> OnPost()
    {
        string redirectUrl = "~/";

        validationMessage = "";
        if (String.IsNullOrEmpty(Text))
        {
            validationMessage = "Cheep cannot be empty";
            return Redirect(Url.Content(redirectUrl));
        }

        if (Text.Length > 160)
        {
            validationMessage = "Cheep cannot be longer than 160 characters";
            return Redirect(Url.Content(redirectUrl));
        }


        AuthorDto author = new()
        {
            Name = User.Identity!.Name!,
            Email = User.FindFirstValue("emails")!// from https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
        };
        await _CheepRepository.CreateCheep(Text, author);


        return Redirect(Url.Content(redirectUrl));
    }

     //https://www.learnrazorpages.com/razor-pages/handler-methods
    public async Task<IActionResult> OnPostFollow(string followName)
    {
        string emailOfUserThatWantsToFollow = User.FindFirstValue("emails") ?? "";
        await _AuthorRepository.FollowAnAuthor(emailOfUserThatWantsToFollow, followName);

        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }

    public async Task<IActionResult> OnPostUnFollow(string followName)
    {
        string emailOfUserThatWantsToUnfollow = User.FindFirstValue("emails") ?? "";
        await _AuthorRepository.UnFollowAnAuthor(emailOfUserThatWantsToUnfollow, followName);


        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }
}