using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Linq;
using System;
using System.Threading.Tasks;
using Azure.Identity;

namespace Chirp.Web.Pages;

public class AboutMePageModel : PageModel
{
    public ICheepRepository _cheepRepository;
    public IAuthorRepository _authorRepository;
    private readonly IConfiguration _config;
    public List<CheepDto>? Cheeps { get; set; }
    public List<Guid>? FollowersID { get; set; }
    public List<string>? FollowersName { get; set; }
    public string? Author { get; set; }
    public string? Email { get; set; }



    public AboutMePageModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IConfiguration config)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        _config = config;
    }

    public async Task<IActionResult> OnGet()
    {
        string? pagevalue = Request.Query["page"];

        Author = User.Identity.Name;
        Email = User.FindFirstValue("emails");
        Console.WriteLine(Email);
        if (pagevalue == null)
        {
            Cheeps = await _cheepRepository.GetCheepsFromAuthor(1, Author);
        }
        else
        {
            Cheeps = await _cheepRepository.GetCheepsFromAuthor(Int32.Parse(pagevalue), Author);
        }

        FollowersID = await _authorRepository.GetFollowedAuthors(Email);

        FollowersName = new List<string>();
        foreach (Guid id in FollowersID)
        {

            string user = await _authorRepository.GetAuthorNameByID(id);
            if (user != null)
            {
                FollowersName.Add(user);
            }
        }


        return Page();
    }

     public async Task<IActionResult> OnPostDelete(string UserEmail)
        {
            try
            {
                var clientId = "e122fcdf-99a1-4b19-b7a4-adf859e617ca";
                var tenantId = "ab2f43aa-cecc-43ed-a142-34012b9a7a3b";
                var clientSecret = _config["ClientSecret"];

                var scopes = new[] { "https://graph.microsoft.com/.default"};

                var options = new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                };

                var clientSecretCredential = new ClientSecretCredential(
                    tenantId, clientId, clientSecret, options);

                var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

                // Get User ID by Email
                string userId = await GetUserIdByEmailAsync(graphClient, UserEmail);

                // Delete User by ID
                await DeleteUserAsync(graphClient, userId);

                Console.WriteLine($"User with email {UserEmail} deleted successfully.");

                string redirectUrl = "/MicrosoftIdentity/Account/SignOut";
                return Redirect(Url.Content(redirectUrl));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user with email: {UserEmail}, {ex.Message}");
                string redirectUrl = "~/AboutMePage";
                return Redirect(Url.Content(redirectUrl));
            }
        }

    private async Task<string> GetUserIdByEmailAsync(GraphServiceClient graphClient, string UserEmail)
    {
         var user = await graphClient
            .Users[UserEmail]
            .GetAsync();

        Console.WriteLine(user.Id);
        return user?.Id;
    }

    private async Task DeleteUserAsync(GraphServiceClient graphClient, string userId)
    {
        if (userId != null)
        {
            await graphClient.Users[userId].DeleteAsync();
        }
        else
        {
            throw new InvalidOperationException("User ID not found for the given email.");
        }
    }
}