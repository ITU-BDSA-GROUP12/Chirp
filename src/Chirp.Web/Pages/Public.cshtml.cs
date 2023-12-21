using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;




namespace Chirp.Web.Pages;

public class PublicModel : Components.TimelinePageModel
{
    // Holds the functionality and variables required to render the Public page.
    // The functionality for the cheep box and follow/unfollow buttons come from the TimelinePageModel superclass.
    public ICheepRepository _cheepRepository;
    public IAuthorRepository _authorRepository;

    // Holds the list of cheeps to be displayed on the Public page.
    public List<CheepDto>? Cheeps { get; set; }

    // Holds the IDs of the authors that the user follows. This is useful for deciding whether to display the Follow or Unfollow button.
    public List<Guid>? FollowedAuthors { get; set; }

    // The page number of the current page of cheeps. Each page contains 32 cheeps.
    public int PageNumber { get; set; }

    // Asserts whether or not there is a next page of cheeps to be displayed.
    public bool HasNextPage;


    public PublicModel(ICheepRepository repository, IAuthorRepository authorRepository) : base(repository, authorRepository)
    {
        _cheepRepository = repository;
        _authorRepository = authorRepository;
    }

    public async Task<IActionResult> OnGet() //use of Task https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-7.0
    {
        // Handles the request when the user navigates to the Public page.
        string? pagestring = Request.Query["page"]; // A string containing the page number of the current page of cheeps.
        int pagevalue;
        if (pagestring == null) pagevalue = 1;
        else pagevalue = Int32.Parse(pagestring);
        PageNumber = pagevalue;
        // The page number is now decided. The cheeps can now be fetched from the database.

        // Fetch cheeps to be displyed.
        Cheeps = await _CheepRepository.GetCheeps(pagevalue);

        if (User.Identity!.IsAuthenticated)
        {
            // The followed authors are only initialised if the user is logged in. An unauthenticated user can not follow authors.
            FollowedAuthors ??= new List<Guid>();

            FollowedAuthors = await _AuthorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));
        }
        HasNextPage = await _CheepRepository.HasNextPageOfPublicTimeline(pagevalue);
        return Page();
    }


}
