using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public ActionResult OnGet()
    {
        string? pagevalue = Request.Query["page"];
        if(pagevalue == null){
            Cheeps = _repository.GetCheeps(1);
        }else {
            Cheeps = _repository.GetCheeps(Int32.Parse(pagevalue));
        }
        return Page();
    }
}
