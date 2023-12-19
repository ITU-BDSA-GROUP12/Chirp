using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Components;

public class TimelinePageModel(ICheepRepository repository, IAuthorRepository authorRepository) : PageModel
{
    // Holds the functionality for the cheep text box and the Share button.
    // It is the superclass of the PublicPageModel and UserTimelinePageModel classes, providing the functionality for both pages.

    public ICheepRepository _CheepRepository = repository; // The repository used to manipulate the cheeps in the database.
    public IAuthorRepository _AuthorRepository = authorRepository; // The repository used to manipulate the authors in the database.

    // The validation message holds the reason why a cheep could not be created, if an error occurs.
    public string validationMessage { get; set; } = "";

    [BindProperty]
    public string? Text { get; set; } // The text entered by the user in the cheep text box.

    public async Task<IActionResult> OnPost()
    {
        // Handles the request when clicking the Share button on the timeline page.
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

        // store the cheep in the database with message entered and the author that created it.
        await _CheepRepository.CreateCheep(Text, author);
        return Redirect(Url.Content(redirectUrl));
    }

    //https://www.learnrazorpages.com/razor-pages/handler-methods
    public async Task<IActionResult> OnPostFollow(string followName)
    {
        // Updates the database when the user follows another user.
        string emailOfUserThatWantsToFollow = User.FindFirstValue("emails") ?? "";
        await _AuthorRepository.FollowAnAuthor(emailOfUserThatWantsToFollow, followName);

        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }

    public async Task<IActionResult> OnPostUnFollow(string followName)
    {
        // Updates the database when the user unfollows another user.
        string emailOfUserThatWantsToUnfollow = User.FindFirstValue("emails") ?? "";
        await _AuthorRepository.UnFollowAnAuthor(emailOfUserThatWantsToUnfollow, followName);


        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }
}