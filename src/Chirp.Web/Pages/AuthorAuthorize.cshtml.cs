using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class AuthorAuthorizeModel : PageModel
{
    private readonly IAuthorRepository _repository;
    public AuthorAuthorizeModel(IAuthorRepository repository)
    {
        _repository = repository;
    }
    public async Task<IActionResult> OnGet() //use of Task https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-7.0
    {
        if (User.Identity.IsAuthenticated)
        {
            AuthorDto? author = await _repository.GetAuthorByName(User.Identity.Name);
            if (author == null)
            {
               await _repository.CreateAuthor(User.Identity.Name, User.FindFirstValue(ClaimTypes.Email));
            }
        }
        return Redirect(Url.Content("~/"));
    }
}