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

    public List<Guid>? FollowedAuthors { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public async Task<IActionResult> OnGet(string author)
    {
        string? pagevalue = Request.Query["page"];
        PageNumber = pagevalue == null ? 1 : Int32.Parse(pagevalue);
        
        if(author == User.Identity.Name){
            List<Guid> FollowedAuthors = await _authorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));
            if (pagevalue == null)
            {
                Cheeps = await _cheepRepository.GetCheepsUserTimeline(1, author, FollowedAuthors);
            }
            else
            {
                Cheeps = await _cheepRepository.GetCheepsUserTimeline(Int32.Parse(pagevalue), author, FollowedAuthors);
            }
        } else{
            if (pagevalue == null)
            {
                Cheeps = await _cheepRepository.GetCheepsFromAuthor(1, author);
            }
            else
            {
                Cheeps = await _cheepRepository.GetCheepsFromAuthor(Int32.Parse(pagevalue), author);
            }
        }
        FollowedAuthors ??= new List<Guid>();

        FollowedAuthors = await _authorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));
        return Page();
    }

    [BindProperty]
    public string Text { get; set; }

  public async Task<IActionResult> OnPost()
    {
        AuthorDto author = new()
        {
            Name = User.Identity.Name,
            Email = User.FindFirstValue("emails")// from https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
        };
        await _cheepRepository.CreateCheep(Text, author);
         
        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }
      public async Task<IActionResult> OnPostFollow(string followName)
        {
            await _authorRepository.FollowAnAuthor(User.FindFirstValue("emails"), followName);

            string redirectUrl = "~/";

            return Redirect(Url.Content(redirectUrl));
        }

        public async Task<IActionResult> OnPostUnFollow(string followName)
        {
            await _authorRepository.UnFollowAnAuthor(User.FindFirstValue("emails"), followName);

            string redirectUrl = "~/";

            return Redirect(Url.Content(redirectUrl));
        }

}