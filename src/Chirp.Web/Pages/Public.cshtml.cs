using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDto> Cheeps { get; set; }

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> OnGet() //use of Task https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-7.0
    {
        string? pagevalue = Request.Query["page"];
        if(pagevalue == null){
            Cheeps = await _repository.GetCheeps(0);
        }else {
            Cheeps = await _repository.GetCheeps(Int32.Parse(pagevalue));
        }
        return Page();
    }
}
