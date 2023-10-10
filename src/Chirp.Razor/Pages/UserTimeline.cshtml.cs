﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _reopsitory;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public ActionResult OnGet(string author)
    {
        string? pagevalue = Request.Query["page"];
        if(pagevalue == null){
            Cheeps = _repository.GetCheepsFromAuthor(1,author);
        }else {
            Cheeps = _repository.GetCheepsFromAuthor(Int32.Parse(pagevalue), author);
        }
        return Page();
    }
}
