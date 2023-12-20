using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Chirp.Web.Pages;

public class DiscoverModel : PageModel
{
    // Holds the functionality and variables required to render the Discover page.

    public ICheepRepository _CheepRepository;
    public IAuthorRepository _AuthorRepository;
    public List<CheepDto>? Cheeps { get; set; }

    public List<Guid>? recomendedAuthors { get; set; }

    public List<CheepDto>? recomendedCheeps { get; set; }

    public List<Guid>? FollowedAuthors { get; set; }


    public int PageNumber { get; set; }

    public DiscoverModel(ICheepRepository repository, IAuthorRepository authorRepository)
    {
        _CheepRepository = repository;
        _AuthorRepository = authorRepository;
        recomendedCheeps = [];
        FollowedAuthors ??= new List<Guid>();
    }

    public async Task<IActionResult> OnGet() //use of Task https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-7.0
    {
        // Handles the request when the user navigates to the Discover page.
        recomendedAuthors = await _AuthorRepository.GetFollowersFollower(User.FindFirstValue("emails"));
        if (recomendedAuthors != null)
        {
            foreach (Guid authorId in recomendedAuthors)
            {
                CheepDto? cheepFromRecommendedAuthor = await _CheepRepository.GetFirstCheepFromAuthor(authorId);
                if (cheepFromRecommendedAuthor != null) recomendedCheeps!.Add(cheepFromRecommendedAuthor);
            }
        }


        //Use followauthors to show unfollow/follow button based on whether or not you follow authors
        FollowedAuthors = await _AuthorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));

        return Page();
    }

    //https://www.learnrazorpages.com/razor-pages/handler-methods
    public async Task<IActionResult> OnPostFollow(string followName)
    {
        // Handles the request when the user clicks the Follow button.
        await _AuthorRepository.FollowAnAuthor(User.FindFirstValue("emails")!, followName);

        return Page();
    }

    public async Task<IActionResult> OnPostUnFollow(string followName)
    {
        // Handles the request when the user clicks the Unfollow button.
        await _AuthorRepository.UnFollowAnAuthor(User.FindFirstValue("emails")!, followName);

        return Page();
    }

}
