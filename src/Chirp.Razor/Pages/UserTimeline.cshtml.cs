using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        string? pagevalue = Request.Query["page"];
        if(pagevalue == null){
            Cheeps = _service.GetCheepsFromAuthor(1,author);
        }else {
            Cheeps = _service.GetCheepsFromAuthor(Int32.Parse(pagevalue), author);
        }
        return Page();
    }
}
