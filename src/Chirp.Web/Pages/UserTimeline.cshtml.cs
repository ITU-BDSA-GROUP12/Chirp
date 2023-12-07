using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : Components.CheepablePageModel
{
    public ICheepRepository _cheepRepository;
    public IAuthorRepository _authorRepository;
    public List<CheepDto>? Cheeps { get; set; }
    public int PageNumber { get; set; }

    public bool HasNextPage { get; set; }

    public List<Guid>? FollowedAuthors { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository) : base(cheepRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public async Task<IActionResult> OnGet(string author)
    {
        string? pagevalue = Request.Query["page"];
        PageNumber = pagevalue == null ? 1 : Int32.Parse(pagevalue);

        FollowedAuthors ??= new List<Guid>();

        //checks if its the user timeline or another user timeline
        if (author == User.Identity.Name)
        {
            FollowedAuthors = await _authorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));
            Cheeps = await _cheepRepository.GetCheepsUserTimeline(PageNumber, author, FollowedAuthors);
        }
        else
        {
            Cheeps = await _cheepRepository.GetCheepsFromAuthor(PageNumber, author);
        }

        HasNextPage = await _cheepRepository.HasNextPageOfPrivateTimeline(PageNumber, author, FollowedAuthors);

        return Page();
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