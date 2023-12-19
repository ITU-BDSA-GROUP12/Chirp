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

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally
#### Prerequisites
In order to run Chirp locally one should follow theese steps:

Start by cloning the Chirp repository by running the following command in your terminal. After this step you should have the entiry Chirp repository in a directory called Chirp.
```
git clone https://github.com/ITU-BDSA23-GROUP12/Chirp.git
```
Next you move to your newly cloned repositorys root directory:
```
cd Chirp
```
Before being able to build, test or run the Chirp application you need to make sure you have docker installed, and then start a mssql using the following command:
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Password>" \
   -p 1433:1433 --name sqlServer1 --hostname sqlServer1 \
   -d \
   mcr.microsoft.com/mssql/server:2022-latest
```
Next you need to add two usersecrets. One is used to establish connection to your newly created local database, the other is used to connect with our azure management app to handle deletion requests.
Start by moving from the root directory to the Chirp.Web package:
```
cd src/Chirp.Web
```
Then run the following commands.:
```
dotnet user-secrets init
```
```
dotnet user-secrets set "ConnectionStrings:DefaultConnection": "Server=127.0.0.1,1433; Database=Master; User Id=SA; <YourStrong@Password>; TrustServerCertificate=true"
```
```
dotnet user-secrets set "ConnectionStrings:DeleteSecret": "n4x8Q~ULLEGdgNVvcSoSqwoHFJ0AgyYfyroMQchJ"
```

Now you should be ready to build, test and run the Chirp application.
#### Building the project
To build the entire project make sure you are in the root directory (Chirp) and run the following command:
```
dotnet build
```
A prompt should appear in your terminal, saying build succes, 0 warnings, 0 errors.

If you wish to only build individual packages within the application, then first move to the given package before running the build command. Eg.:
```
cd src/Chirp.Web
```
```
dotnet build
```
#### Running the application
To run the Chirp application locally you have to move to the Chirp.Web package (/src/Chip.Web).
When standing in the Chirp.Web package, run the following command:
```
dotnet run
```
This should prompt a message in your terminal including the line:
"info: Microsoft.Hosting.Lifetime[14]
Now listening on: https://localhost:7028"

Chirp is now running locally on your machine, and you can acces it by going to: https://localhost:7028 in your browser of choice.

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others