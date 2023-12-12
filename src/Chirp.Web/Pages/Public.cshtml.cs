using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Chirp.Web.Pages;

public class PublicModel : Components.TimelinePageModel
{

    public ICheepRepository _cheepRepository;
    public IAuthorRepository _authorRepository;
    public List<CheepDto>? Cheeps { get; set; }

    public List<Guid>? FollowedAuthors { get; set; }

    public int PageNumber { get; set; }

    public bool HasNextPage;


    public PublicModel(ICheepRepository repository, IAuthorRepository authorRepository) : base(repository, authorRepository)
    {
        _cheepRepository = repository;
        _authorRepository = authorRepository;
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


}
