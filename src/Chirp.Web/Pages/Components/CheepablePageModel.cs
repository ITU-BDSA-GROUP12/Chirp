using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Chirp.Web.Pages.Components;

public class CheepablePageModel : PageModel
{

    public ICheepRepository _CheepRepository;

    public List<Guid>? FollowedAuthors { get; set; }

    public CheepablePageModel(ICheepRepository repository)
    {
        _CheepRepository = repository;
    }

    [BindProperty]
    public string Text { get; set; }

    public async Task<IActionResult> OnPost()
    {
        if (User.Identity == null) throw new Exception("User attempted to send cheep without being logged in");
        string? userName = User.Identity.Name;
        string? userEmail = User.FindFirstValue("emails");
        if (userName == null) throw new Exception("User has no username");
        if (userEmail == null) throw new Exception("User has not registrered an email");

        AuthorDto author = new()
        {
            Name = User.Identity.Name,
            Email = User.FindFirstValue("emails")// from https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
        };
        await _CheepRepository.CreateCheep(Text, author);

        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }
}
