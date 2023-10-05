using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        string? pagevalue = Request.Query["page"];
        if(pagevalue == null){
            Cheeps = _service.GetCheeps(1);
        }else {
            Cheeps = _service.GetCheeps(Int32.Parse(pagevalue));
        }
        return Page();
    }
}
