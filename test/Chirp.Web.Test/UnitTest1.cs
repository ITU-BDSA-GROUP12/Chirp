using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task TestThatFollowAuthorMakesTheFollowedAuthorAppearOnAboutMePageUnderFollowedAuthorsAndDisappearWhenUnfollowed()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 500 });
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://bdsagroup12chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("BdsaTester");

        await page.GetByLabel("Password").ClickAsync();

        await page.GetByLabel("Password").FillAsync("Group12Chirp");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        // sometimes Github wants to ensure autherization, when there has be alot of logins to the same site, which happens in the tests
        var authorizeButton = page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA23-GROUP12" });

        if (await authorizeButton.IsVisibleAsync())
        {
            await authorizeButton.ClickAsync();
        }

        await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();

        await Expect(page.Locator("body")).ToContainTextAsync("You don't follow any people yet");

        await page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" }).ClickAsync();

        await page.Locator("li").Filter(new() { HasText = "allanzigge Så kører vi igen" }).GetByRole(AriaRole.Button).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Strong)).ToContainTextAsync("allanzigge");

        await page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Unfollow" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();

        await Expect(page.Locator("body")).ToContainTextAsync("You don't follow any people yet");
    }

    [Test]
    public async Task ItIsPossibleToGoFromPage1ToPage2()
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 500 });
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://bdsagroup12chirprazor.azurewebsites.net/");

        await Expect(page.Locator("body")).ToContainTextAsync("1");

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Previous" })).ToBeHiddenAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Next" }).ClickAsync();

        await Expect(page.Locator("body")).ToContainTextAsync("2");

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Previous" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task ExpectNextNotToBeVisibleAtTheLastPage() //as long as there is no new cheeps
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 500 });
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://bdsagroup12chirprazor.azurewebsites.net/?page=21");

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Next" })).ToBeHiddenAsync();
    }

    [Test]
    public async Task TestExpectedButtonsInNavBar() //as long as there is no new cheeps
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 500 });
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://bdsagroup12chirprazor.azurewebsites.net/");

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Register" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Login" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "My timeline" })).ToBeHiddenAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Discover" })).ToBeHiddenAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Logout [BdsaTester]" })).ToBeHiddenAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "About me" })).ToBeHiddenAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("BdsaTester");

        await page.GetByLabel("Password").ClickAsync();

        await page.GetByLabel("Password").FillAsync("Group12Chirp");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        // sometimes Github wants to ensure autherization, when there has be alot of logins to the same site, which happens in the tests
        // sometimes Github wants to ensure autherization, when there has be alot of logins to the same site, which happens in the tests
        var authorizeButton = page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA23-GROUP12" });

        if (await authorizeButton.IsVisibleAsync())
        {
            await authorizeButton.ClickAsync();
        }

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "My timeline" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Discover" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Logout [BdsaTester]" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "About me" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Register" })).ToBeHiddenAsync();

        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Login" })).ToBeHiddenAsync();
    }

    [Test]
    public async Task TestThatNavBarDirectsUserToExpectedPage() //as long as there is no new cheeps
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 500 });
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://bdsagroup12chirprazor.azurewebsites.net/");

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        await Expect(page.GetByText("Sign in to GitHub to continue")).ToBeVisibleAsync();

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("BdsaTester");

        await page.GetByLabel("Username or email address").PressAsync("Tab");

        await page.GetByLabel("Password").ClickAsync();

        await page.GetByLabel("Password").FillAsync("Group12Chirp");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        // sometimes Github wants to ensure autherization, when there has be alot of logins to the same site, which happens in the tests
        var authorizeButton = page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA23-GROUP12" });

        if (await authorizeButton.IsVisibleAsync())
        {
            await authorizeButton.ClickAsync();
        }

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync(); //Ensure that landingpage after login is public timeline

        await page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "My timeline" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "BdsaTester's Timeline" })).ToBeVisibleAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Discover" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Discover" })).ToBeVisibleAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "About you and your data" })).ToBeVisibleAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Logout [BdsaTester]" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Signed out" })).ToBeVisibleAsync();

    }

    [Test]
    public async Task TestThatCheepBoxIsNotPressentWhenNotLoggedInAndIsPressenOnPublicAndMyTimelineWhenLoggedIn() //as long as there is no new cheeps
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 1000 });
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://bdsagroup12chirprazor.azurewebsites.net/");

        await Expect(page.GetByText("What's on your mind BdsaTester? Share🚜")).ToBeHiddenAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("BdsaTester");

        await page.GetByLabel("Password").ClickAsync();

        await page.GetByLabel("Password").FillAsync("Group12Chirp");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        // sometimes Github wants to ensure autherization, when there has be alot of logins to the same site, which happens in the tests
        var authorizeButton = page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA23-GROUP12" });

        if (await authorizeButton.IsVisibleAsync())
        {
            await authorizeButton.ClickAsync();
        }

        await page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" }).ClickAsync();

        await Expect(page.GetByText("What's on your mind BdsaTester? Share🚜")).ToBeVisibleAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "My timeline" }).ClickAsync();

        await Expect(page.GetByText("What's on your mind BdsaTester? Share🚜")).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestAboutMePageHasInformationOnTheUser() //as long as there is no new cheeps
    {
        await using var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 1000 });
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://bdsagroup12chirprazor.azurewebsites.net/");

        await Expect(page.GetByText("What's on your mind BdsaTester? Share🚜")).ToBeHiddenAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("BdsaTester");

        await page.GetByLabel("Password").ClickAsync();

        await page.GetByLabel("Password").FillAsync("Group12Chirp");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        // sometimes Github wants to ensure autherization, when there has be alot of logins to the same site, which happens in the tests
        var authorizeButton = page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA23-GROUP12" });

        if (await authorizeButton.IsVisibleAsync())
        {
            await authorizeButton.ClickAsync();
        }

        await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "About you and your data" })).ToBeVisibleAsync();

        await Expect(page.GetByText("BdsaTester", new() { Exact = true })).ToBeVisibleAsync();

        await Expect(page.GetByText("bdsatester@yahoo.com")).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Forget Me!" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Your Cheeps" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "People you follow" })).ToBeVisibleAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "People who follow you" })).ToBeVisibleAsync();

    }

    // [Test]
    // public async Task TestCheepingWorksAndIsStoredOnMyTimeLineAndOnAboutMePageWhenPressedForgetMeTheCheepDisappears() //as long as there is no new cheeps
    // {
    //     await using var browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 1000 });
    //     var page = await browser.NewPageAsync();

    //     await page.GotoAsync("https://bdsagroup12chirprazor.azurewebsites.net/");

    //     await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

    //     await page.GetByLabel("Username or email address").ClickAsync();

    //     await page.GetByLabel("Username or email address").FillAsync("BdsaTester");

    //     await page.GetByLabel("Password").ClickAsync();

    //     await page.GetByLabel("Password").FillAsync("Group12Chirp");

    //     await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

    // sometimes Github wants to ensure autherization, when there has be alot of logins to the same site, which happens in the tests
        // if(await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA23-GROUP12" })).ToBeVisibleAsync();){ 
        //     await page.GetByRole(AriaRole.Button, new() { Name = "Authorize ITU-BDSA23-GROUP12" }).ClickAsync();
        // }

    //     await page.Locator("#Text").ClickAsync();

    //     await page.Locator("#Text").FillAsync("Tak til Allan for at glemme sin madpakke");

    //     await page.GetByRole(AriaRole.Button, new() { Name = "Share🚜" }).ClickAsync();

    //     await Expect(page.GetByText("BdsaTester Tak til Allan for at glemme sin madpakke")).ToBeVisibleAsync();

    //     await page.GetByRole(AriaRole.Link, new() { Name = "My timeline" }).ClickAsync();

    //     await Expect(page.GetByText("BdsaTester Tak til Allan for at glemme sin madpakke")).ToBeVisibleAsync();

    //     await page.Locator("#Text").ClickAsync();

    //     await Expect(page.GetByText("What's on your mind BdsaTester? Share🚜")).ToBeVisibleAsync();

    //     await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();

    //     await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Your Cheeps" })).ToBeVisibleAsync();

    //     await Expect(page.GetByText("BdsaTester Tak til Allan for at glemme sin madpakke")).ToBeVisibleAsync();

    //     await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Forget Me!" })).ToBeVisibleAsync();

    //     await page.GetByRole(AriaRole.Button, new() { Name = "Forget Me!" }).ClickAsync();

    //     await page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();

    //     await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Signed out" })).ToBeVisibleAsync();

    //     await page.GetByRole(AriaRole.Link, new() { Name = "Public timeline" }).ClickAsync();

    //     await Expect(page.GetByText("What's on your mind BdsaTester? Share🚜")).ToBeHiddenAsync();

    //     await page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();

    //     await page.GetByPlaceholder("Email Address").ClickAsync();

    //     await page.GetByPlaceholder("Email Address").FillAsync("bdsatester@yahoo.com");

    //     await page.GetByLabel("Continue").ClickAsync();

    //     await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();

    //     await Expect(page.GetByText("BdsaTester Tak til Allan for at glemme sin madpakke")).ToBeVisibleAsync();

    //     await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();

    //     await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Your Cheeps" })).ToBeVisibleAsync();

    //     await Expect(page.GetByText("BdsaTester Tak til Allan for at glemme sin madpakke")).ToBeVisibleAsync();

    // }

}