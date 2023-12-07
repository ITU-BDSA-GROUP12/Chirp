using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Chirp.Web.Pages;

public class PublicModel : Components.CheepablePageModel
{

    public ICheepRepository _CheepRepository;
    public IAuthorRepository _AuthorRepository;
    public List<CheepDto>? Cheeps { get; set; }

    public List<Guid>? FollowedAuthors { get; set; }

    public int PageNumber { get; set; }

    public bool HasNextPage;

    public PublicModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository) : base(cheepRepository)
    {
        _AuthorRepository = authorRepository;
        _CheepRepository = cheepRepository;
    }

    public async Task<IActionResult> OnGet() //use of Task https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-7.0
    {
        string? pagestring = Request.Query["page"];
        int pagevalue;
        if (pagestring == null) pagevalue = 1;
        else pagevalue = Int32.Parse(pagestring);
        PageNumber = pagevalue;


        Cheeps = await _CheepRepository.GetCheeps(pagevalue);

        if (User.Identity.IsAuthenticated)
        {
            FollowedAuthors ??= new List<Guid>();

            FollowedAuthors = await _AuthorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));
        }
        HasNextPage = await _CheepRepository.HasNextPageOfPublicTimeline(pagevalue);
        return Page();
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
