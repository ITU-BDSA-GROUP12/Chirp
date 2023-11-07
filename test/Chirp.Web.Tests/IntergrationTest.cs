// namespace Chirp.Web.Tests;



// public class IntergrationTest : IClassFixture<WebApplicationFactory<Program>>
// {
//     private readonly WebApplicationFactory<Program> _fixture;
//     private readonly HttpClient _client;

//     public IntergrationTest(WebApplicationFactory<Program> fixture)
//     {
//         _fixture = fixture;
//         _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
//     }

//     [Fact]
//     public async void CanSeePublicTimeline()
//     {
//         var response = await _client.GetAsync("/");
//         response.EnsureSuccessStatusCode();
//         var content = await response.Content.ReadAsStringAsync();

//         Assert.Contains("Chirp!", content);
//         Assert.Contains("Public Timeline", content);
//     }

//     [Theory]
//     [InlineData("Helge")]
//     [InlineData("Rasmus")]
//     public async void CanSeePrivateTimeline(string author)
//     {
//         var response = await _client.GetAsync($"/{author}");
//         response.EnsureSuccessStatusCode();
//         var content = await response.Content.ReadAsStringAsync();

//         Assert.Contains("Chirp!", content);
//         Assert.Contains($"{author}'s Timeline", content);
//     }
// }