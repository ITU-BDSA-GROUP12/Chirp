using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    public ICheepRepository _repository;
    public List<CheepDto>? Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> OnGet(string author)
    {
        string? pagevalue = Request.Query["page"];
        if (pagevalue == null)
        {
            Cheeps = await _repository.GetCheepsFromAuthor(1, author);
        }
        else
        {
            Cheeps = await _repository.GetCheepsFromAuthor(Int32.Parse(pagevalue), author);
        }
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
        await _repository.CreateCheep(Text, author);
         
        string redirectUrl = "~/";

        return Redirect(Url.Content(redirectUrl));
    }
}