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

## LLMs, ChatGPT, CoPilot, and others