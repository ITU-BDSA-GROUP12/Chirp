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

## How to run test suite locally
The test suite of Chirp is divided in two test-projects. 'Chirp.Infrastructure.Tests' and 'Chirp.Web.Test'.
In order to run all test suites of the Chirp application, you simply need to be located in the root directory (Chirp) and run the following command:
```
dotnet test
```
First this should run the UI Tests located in the 'Chirp.Web.Test' project, and should return a prompt with 3 tests passed.
After that it will run the integration tests from the 'Chirp.Infrastructure.Tests' project, which should return a promt with 26 tests passed.

As mentioned the 'Chirp.Web.Test' project holds user interface tests made using playwright. Upon review of the test class in this project it will be clear that there are more tests than the 3 being run. This is because we have five UI tests where we use a Github 'test account' to test logged in features, these have been a great help during development. The problem is that in order for them to run on a new computer, the Github 'test account' will have to verify the new device with an email code. Therefore we have chosen to comment these tests out, in order for the reader to be able to run our test suite.
If you wish to run the test suite including these five tests, then get in contact with any member of the team, and we will then help verify your device.
# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others