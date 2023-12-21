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

Our Application builds on two main consepts: Cheeps and Authors.

### Cheep
A Cheep is the understanding of a post, which holds a message from the author of the post. It has a refernce to its author and a knowlegde of when it was posted.

### Author
An Author represents the user in our program. The user can write posts (Cheeps), which is why they are named Author. An Author has references to all their Cheeps, as well as their name and email. It is possible to follow an Author; therefore, the Author also has references to the Authors who follow it and the Authors it follows.

Our domain model consists of two data entities, which depict the attributes of a 'Cheep' and an 'Author' in the context of our application. Figure \ref{domainModelImage} shows the two classes, 'Cheep' and 'Author,' with their fields and cardinality relationships between each other and within themselves. The 'Cheep' class has a one-to-one relationship with an 'Author' class, and an 'Author' class has a zero-to-many relationship with the 'Cheep' class. Furthermore the 'Author' class has a zero-to-many relationship with authors who follow it and a zero-to-many relationship with authors whom it follows.

![Illustration of the _Chirp!_ data model as UML class diagram.\lable{domainModelImage}](https://raw.githubusercontent.com/ITU-BDSA23-GROUP12/Chirp/main/docs/images/DomainModel.drawio1.png)


## Architecture — In the small
Our Chirp application is written following the clean (or onion) architecture. This clean architecture of Chirp can be seen visualized in figure \ref{OnionDiagram}. The idea with building chirp with a clean architecture, is that it implements both the principle of Domain Driven Development (DDD) and Dependency Inversion.

When looking at figure \ref{OnionDiagram}, dependencies must be read going inwards toward the center of the illustration. In there we have the _Chirp.Core_ package. Besides building Chirp with clean architecture, we also make use of the repository pattern to allow for abstraction and organization of data handling. In the _Chirp.Core_ package we hold two repository-pattern specific classes. The Data Transfer Objects (DTO), and the repository interfaces. This follows the DDD and the repository pattern, placing an abstraction of the business logic at the very center of our application.

![This is the caption\label{OnionDiagram}](https://raw.githubusercontent.com/ITU-BDSA23-GROUP12/Chirp/main/docs/images/OnionDiagram.png)

The next layer in the architecture is the data layer which manifests in Chirp as the _Chirp.Infrastructure_ package. Here lies all classes related to the data handling and database management. Following dependencies going towards the center, this package depends on the _Chirp.Core_ package, as _Chirp.Infrastructure_ holds the implementation of the repository interfaces. This dependency shows how the architecture implements the Dependency Inversion principle, which states that implementations should depend on abstractions and not the other way around.

The outermost layer is the entry point for our Chirp application. In the code this is the _Chirp.Web_ package, which holds a RazorPage Application. This package depends on both the _Chirp.Core_ and the _Chirp.Infrastructure_ package. It is also in this class the Program.cs file resides, which is responsible for configuring the application, including the configuration for dependency injection. Here it configures implementation from the _Chirp.Infrastructure_ layer to be used when abstractions from the _Chirp.Core_ layer is requested.

For a more indepth visualization of which classes reside in which of the packages mentioned, see the png-file located at 'Chirp/docs/images/PackageDiagram.png', a complete UML package diagram for _Chirp!_.
## Architecture of deployed application

In this section, we will delve into the overall architecture of Chirp! as a deployed web application.\ref{ArchitectureDeployment} shows the structure of the architecture. Our application is hosted on the cloud-based Microsoft Azure platform. The code is accessible through an Azure service called 'Azure Web Service.' A client can make an HTTP request to our web application, and the response will return an instance of Chirp! on their computer. We utilize two additional Azure services: Azure SQL Server and Azure Active Directory (AD) B2C.

Azure SQL Server is employed to host our database by providing a connection string to the web service, which connects to our EF Core implementation. Azure AD B2C is used to authenticate users of Chirp! through a SignUpSignIn user flow. This flow redirects a login or signup request to GitHub, our chosen Identity provider. GitHub returns an authentication token, which either creates a new user on Azure AD or logs the user into Chirp!.

We employ Microsoft Graph to delete a user from Azure AD by using a management application with Read-Write API permissions on Users in Azure AD.

![Illustration of the architecture of the deployed _Chirp!_ app as a UML diagram.\lable{ArchitectureDeployment}](https://raw.githubusercontent.com/ITU-BDSA23-GROUP12/Chirp/main/docs/images/ArchitectureDeployment.drawio%20(2).png)

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License
It became clear to us that choosing a license is important to us as developers. This is to avoid legal issues regarding copyrights and to protect ourselves from liabilities. Also it is always good practice to add a license to guide other developers to what they can do with our code.
This is of course even more relevant if the application were to stay and generate profit, and in this case we would need to consider another license.

When choosing a license we generally have to choose between permissive(copyright) and protective(copyleft) licenses, with different protective grades. We are not a community where the code needs contributers, and we do not have a desire to ensure openness. In our group we agree that anyone can re-use our code, however we do not want that anyone has permission to patent our code. Therefore we have chosen the MIT license because of its simplicity and permissive nature. The Apache license grants patent rights from contributers, which is what we do not want. 

Our dependencies are all licensed by MIT or Apache 2.0, and therefore compatible with MIT.
List of all our dependencies:
*.NET 			        MIT
*Fluent validation	    Apache 2.0
*Entity Framework       MIT
*ASP.NET core           MIT	
*Microsoft graph        MIT
*Microsoft identity	    MIT		
*Microsoft.data.sqlite  MIT
*Azure Identity		    MIT

## LLMs, ChatGPT, CoPilot, and others
A large level model is used for language understanding and generation. We have used ChatGPT and Co-Pilot in this project. We used these to expedite the coding in certain areas. 
We tried asking ChatGPT when hitting an error that we could not figure out why we recieved. ChatGPT is often helpful with both finding but also explain what the error is to understand _why_ we get the error. This has been great to get a better understanding of what we need to fix to make good and functional code. Furthermore ChatGPT has proven to be a solid tool for discovering keywords and to figure out what documentation needs to be delved into.
ChatGPT is also good at comprehending how components work together, whereas documentation for each part (being Azure, .NET or even GitHub) is really good at focusing on the specific domain. When "integrating" these domains, ChatGPT has been helpful with sharing its insights and filled the gaps between documentation. This has definitely saved us time.
Co-Pilot were used on the fly, as we were coding it would suggest what we might write, and often it was right and expidited the coding. It was also helpful, as we could write a comment stating what we want and Co-Pilot will then suggest the code for this. 
We have always been careful when using these tools as they may be wrong, inaccurate, etc. and researched upon information it gave that we were going to use. The way these tool were used in the process, made the code writing a bit faster, and sometimes **way** faster to debug.
