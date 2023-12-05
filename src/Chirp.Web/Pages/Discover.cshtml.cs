    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;



    namespace Chirp.Web.Pages;

    public class DiscoverModel : PageModel
    {
        
        public ICheepRepository _CheepRepository;
        public IAuthorRepository _AuthorRepository;
        public List<CheepDto>? Cheeps { get; set; }

        public List<Guid>? recomendedAuthors { get; set; }

        public List<CheepDto>? recomendedCheeps { get; set; }

        public List<Guid>? FollowedAuthors { get; set; }


        public int PageNumber { get; set; } 

        public DiscoverModel(ICheepRepository repository, IAuthorRepository authorRepository)
        {
            _CheepRepository = repository;
            _AuthorRepository = authorRepository;
        }

        public async Task<IActionResult> OnGet() //use of Task https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-7.0
        {
            string? pagevalue = Request.Query["page"];
            recomendedCheeps = new List<CheepDto>();
            
            recomendedAuthors = await _AuthorRepository.GetFollowersFollower(User.FindFirstValue("emails"));
            foreach (Guid authorId in recomendedAuthors)
            {
                recomendedCheeps.Add(await _CheepRepository.GetFirstCheepFromAuthor(authorId));
            }

            FollowedAuthors ??= new List<Guid>();
            FollowedAuthors = await _AuthorRepository.GetFollowedAuthors(User.FindFirstValue("emails"));

            return Page();
        }


        [BindProperty]
        public string Text { get; set; }

        //https://www.learnrazorpages.com/razor-pages/handler-methods
        public async Task<IActionResult> OnPostFollow(string followName)
        {
            await _AuthorRepository.FollowAnAuthor(User.FindFirstValue("emails"), followName);

            return Page();
        }

        public async Task<IActionResult> OnPostUnFollow(string followName)
        {
            await _AuthorRepository.UnFollowAnAuthor(User.FindFirstValue("emails"), followName);

            return Page();
        }

    }
