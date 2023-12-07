using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    public ICheepRepository _cheepRepository;
    public IAuthorRepository _authorRepository;
    public List<CheepDto>? Cheeps { get; set; }
    public int PageNumber { get; set; }

    public bool HasNextPage { get; set; }   

    public List<Guid>? FollowedAuthors { get; set; }

    public string validationMessage { get; set; } = "";

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public async Task<IActionResult> OnGet(string author)
    {
        string? pagevalue = Request.Query["page"];
        PageNumber = pagevalue == null ? 1 : Int32.Parse(pagevalue);

        FollowedAuthors ??= new List<Guid>();
        FollowedAuthors = await _authorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));
        //checks if its the user timeline or another user timeline
        if(author == User.Identity.Name){
            Cheeps = await _cheepRepository.GetCheepsUserTimeline(PageNumber, author, FollowedAuthors);
        } else{
            Cheeps = await _cheepRepository.GetCheepsFromAuthor(PageNumber, author);
        }

        HasNextPage = await _cheepRepository.HasNextPageOfPrivateTimeline(PageNumber, author, FollowedAuthors);
        
        return Page();
    }

    [BindProperty]
    public string Text { get; set; }

    public async Task<IActionResult> OnPost()
    {
        string redirectUrl = "~/";

        validationMessage = "";
        if (String.IsNullOrEmpty(Text)) {
            validationMessage = "Cheep cannot be empty";
            return OnGet(User.Identity.Name).Result;
        }

        if (Text.Length > 160)
        {
            validationMessage = "Cheep cannot be longer than 160 characters";
            return OnGet(User.Identity.Name).Result;
        }

        if (User.Identity == null) throw new Exception("User attempted to send cheep without being logged in");
        string? userName = User.Identity.Name;
        string? userEmail = User.FindFirstValue("emails");
        if (userName == null) throw new Exception("User has no username");
        if (userEmail == null) throw new Exception("User has not registrered an email");

        AuthorDto author = new()
        {
            Name = userName,
            Email = userEmail// from https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
        };
        await _cheepRepository.CreateCheep(Text, author);

        return Redirect(Url.Content(redirectUrl));
    }
    public async Task<IActionResult> OnPostFollow(string followName)
    {
        string emailOfUserThatWantsToFollow = User.FindFirstValue("emails") ?? "";
        await _authorRepository.FollowAnAuthor(emailOfUserThatWantsToFollow, followName);

        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }

    public async Task<IActionResult> OnPostUnFollow(string followName)
    {
        string emailOfUserThatWantsToUnfollow = User.FindFirstValue("emails") ?? "";
        await _authorRepository.UnFollowAnAuthor(emailOfUserThatWantsToUnfollow, followName);

        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }

}