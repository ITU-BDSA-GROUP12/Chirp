using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class UserTimelineModel : Components.TimelinePageModel
{
    // Holds the functionality and variables required to render the User Timeline page.
    // The functionality for the cheep box and follow/unfollow buttons come from the TimelinePageModel superclass.
    public ICheepRepository _cheepRepository;
    public IAuthorRepository _authorRepository;
    public List<CheepDto>? Cheeps { get; set; } // The list of cheeps to be dispalyed on the User Timeline page.
    public int PageNumber { get; set; } // The page number of the current page of cheeps. Each page contains 32 cheeps.

    public bool HasNextPage { get; set; } // Asserts whether or not there is a next page of cheeps to be displayed.

    // The IDs of the authors that the user follows. Followed users will also have their cheeps displayed on the User Timeline page.
    public List<Guid>? FollowedAuthors { get; set; }


    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository) : base(cheepRepository, authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        FollowedAuthors = [];
    }

    public async Task<IActionResult> OnGet(string author)
    {
        string? pagevalue = Request.Query["page"]; // The page number provided as a string
        PageNumber = pagevalue == null ? 1 : Int32.Parse(pagevalue);
        // The page number is now decided. The cheeps can now be fetched from the database.

        // Fetch the followed authors
        FollowedAuthors = await _authorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));
        // Checks if its the user timeline or another user timeline. User timelines of other users will not contain cheeps of the followed users.
        if (author == User.Identity!.Name)
        {
            Cheeps = await _cheepRepository.GetCheepsUserTimeline(PageNumber, author, FollowedAuthors!);
        }
        else
        {
            Cheeps = await _cheepRepository.GetCheepsFromAuthor(PageNumber, author);
        }

        // Assert if the next page button should be shown.
        HasNextPage = await _cheepRepository.HasNextPageOfPrivateTimeline(PageNumber, author, FollowedAuthors!);

        return Page();
    }

}