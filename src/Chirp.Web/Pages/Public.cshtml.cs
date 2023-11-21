    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;



    namespace Chirp.Web.Pages;

    public class PublicModel : PageModel
    {
        
        public ICheepRepository _repository;
        public List<CheepDto>? Cheeps { get; set; }

        public PublicModel(ICheepRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> OnGet() //use of Task https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-7.0
        {
            string? pagevalue = Request.Query["page"];
            if (pagevalue == null)
            {
                Cheeps = await _repository.GetCheeps(1);
            }
            else
            {
                Cheeps = await _repository.GetCheeps(Int32.Parse(pagevalue));
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
        public async Task OnPostPrint()
        {
            // string redirectUrl = "~/GAYY";
            // Redirect(Url.Content(redirectUrl));
            Console.WriteLine("GAYY");
        }

        //https://www.learnrazorpages.com/razor-pages/handler-methods
        public async Task<IActionResult> OnPostFollow()
        {
            //…
            string redirectUrl = "~/gaaayyy";

            return Redirect(Url.Content(redirectUrl));
        }
    }
