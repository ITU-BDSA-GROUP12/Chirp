---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group `12`
author:
- "Markus Brandt Højgaard <mbrh@itu.dk>"
- "Rasmus Søholt Rasmussen <rhra@itu.dk>"
- "Allan Sigge Andersen <asia@itu.dk>"
- "Mads Voss Hvelplund <mhve@itu.dk>"
- "Lukas Brandt Pallesen <lupa@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model

Here comes a description of our domain model.

![Illustration of the _Chirp!_ data model as UML class diagram.](docs/images/domain_model.png)

## Architecture — In the small

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

In this section, we will examine the sequence of calls made during a user journey, from an unauthorized user to an authorized user. Figure \ref{UserRegistration} shows a client computer requesting an HTTP call in their internet browser by calling the root URL of _Chirp!_. The request is received by our Azure-deployed Web Application, which, through the use of Microsoft Identity, checks if the HTTP request has the needed access token to be authorized. Since the user is not authorized, the HTTP response is a limited version of Chirp!, as shown in the previous section _User Activities_.

The next step is for a user to press 'Register/Login.' This action triggers an ASP controller, 'Account,' to generate a URL pointing to the Azure AD B2C Tenant's (Azure Tenant) SignUpSignIn user flow using the ID, secret, and information of our Azure Tenant provided by the connection strings from the app service. (https://learn.microsoft.com/en-us/dotnet/api/microsoft.identity.web.ui.areas.microsoftidentity.controllers.accountcontroller?view=msal-model-dotnet-latest) Since we have chosen Github as the identity provider, our Azure Tenant sends a GET request to a Github OAuth App we have created in our Github repository. The OAuth App provides the user with a login dialog, which the user fills in. If the authentication is successful, the OAuth App provides our Azure Tenant with an access_token through the callback URL provided to the OAuth App. In the Azure Tenant, we have selected some scopes for which information about the user is needed in our Web Application. (https://docs.github.com/en/apps/oauth-apps/building-oauth-apps/authenticating-to-the-rest-api-with-an-oauth-app)

When the callback URL with the access token arrives in the Azure Tenant, it checks if the access_token have the necessary scopes. Since we need the user's email, and Github doesn't provide this, our Azure Tenant needs to provide it for us. If the user exists as a user in the Azure Tenant, then the user's email is provided there, and it can be fetched from there. Otherwise, the Azure Tenant sends a dialog to the client where the user can fill in the email input. Now the Azure Tenant can create a user with sufficient scopes and send the access token and user credentials to the App service through the callback URL provided in the connection strings of the App Service.

Using Microsoft Identity, our web application now sets properties like name, email, and isAuthenticated to the values provided by the access token. Before accessing our Chirp! web application, the login redirects to an AuthorAuthenticator. Our user is at this point stored in the Azure Tenant but may not yet be stored in our SQL Server database, which we need to ensure.

![Sequence diagram of register as a User on _Chirp!_.\lable{UserRegistration}](https://raw.githubusercontent.com/ITU-BDSA23-GROUP12/Chirp/main/docs/images/SequenceOfFunctiallity-RegisterUser.drawio.png)

Therefore, as shown in figure \ref{AuthorAuthentication}, when landing on the AuthorAuthenticator page, a GET request is made to the PageModel, and a method on our AuthorRepository is called to find the Author the user should be if the user is in the database. By the provided email of the user, the AuthorRepository uses EF Core to make a query to the instance of the database through the connection string provided to ChirpDBContext. The query then returns an Author record, which the AuthorRepository returns as an AuthorDTO. If the AuthorDTO is not null, then the user is stored as an Author in the ChirpDBContext. If, however, it is null, then the Author needs to be created in the ChirpDBContext.

The AuthorAuthenticate model now calls a CreateAuthor method on the AuthorRepository, which again uses EF Core to create an Author entity and adds it to the ChirpDBContext. If any errors happen, they will be thrown; otherwise, the Author is now successfully stored in our database, and the user is redirected to the home page, where they now have all functionalities available.

![Sequence diagram of creating a user on the Azure SQL Server if User does not exist.\lable{AuthorAuthenticazion}](https://raw.githubusercontent.com/ITU-BDSA23-GROUP12/Chirp/main/docs/images/SequenceOfFunctionality-AuthorAuthenticazion.drawio.png)

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others