using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;

namespace Chirp.Web.Pages;
public class CheepElementModel : PageModel
{
    readonly ICheepRepository _repository;
    public CheepElementModel(ICheepRepository repository)
    {
        _repository = repository;
    }
    public string Text { get; set; }

    public async Task<IActionResult> OnPost()
    {
        AuthorDto author = new AuthorDto
        {
            Name = User.Identity.Name,
            Email = User.FindFirstValue(ClaimTypes.Email)// from https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
        };
        _repository.CreateCheep(Text, author);
        return Redirect(Url.Content("~/skrublinaf"));
    }
}