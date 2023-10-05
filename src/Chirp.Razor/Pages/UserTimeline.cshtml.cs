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
            Cheeps = _service.GetCheepsFromAuthor(author,1);
        }else {
            Cheeps = _service.GetCheepsFromAuthor(author,Int32.Parse(pagevalue));
        }
        return Page();
    }
}
