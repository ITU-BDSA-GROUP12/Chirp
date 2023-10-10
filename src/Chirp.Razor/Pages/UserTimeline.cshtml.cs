using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDto> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> OnGet(string author)
    {
        string? pagevalue = Request.Query["page"];
        if(pagevalue == null){
            Cheeps = await _repository.GetCheepsFromAuthor(1,author);
        }else {
            Cheeps = await _repository.GetCheepsFromAuthor(Int32.Parse(pagevalue), author);
        }
        return Page();
    }
}
