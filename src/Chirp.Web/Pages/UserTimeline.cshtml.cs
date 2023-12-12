using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : Components.TimelinePageModel
{
    public ICheepRepository _cheepRepository;
    public IAuthorRepository _authorRepository;
    public List<CheepDto>? Cheeps { get; set; }
    public int PageNumber { get; set; }

    public bool HasNextPage { get; set; }

    public List<Guid>? FollowedAuthors { get; set; }


    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository) : base(cheepRepository, authorRepository)
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
        if (author == User.Identity.Name)
        {
            Cheeps = await _cheepRepository.GetCheepsUserTimeline(PageNumber, author, FollowedAuthors);
        }
        else
        {
            Cheeps = await _cheepRepository.GetCheepsFromAuthor(PageNumber, author);
        }

        HasNextPage = await _cheepRepository.HasNextPageOfPrivateTimeline(PageNumber, author, FollowedAuthors);

        return Page();
    }

}