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
Our Chirp application is written following the clean (or onion) architecture. This clean architecture of Chirp can be seen visualized in figure \ref{OnionDiagram}. The idea with building chirp with a clean architecture, is that it implements both the principle of Domain Driven Development (DDD) and Dependency Inversion.

When looking at figure \ref{OnionDiagram}, dependencies must be read goin inwards toward the center of the illustration. In there we have the Chip.Core package. Besides building Chirp with clean architecture, we also make use of the repository pattern to allow for abstraction and organization of datahandling. In the _Chirp.Core_ package we hold two repository-pattern specific classes. The Data Transfer Objects (DTO), and the repository interfaces. This follows the DDD and the repository pattern, placing an abstraction of the bussiness logic at the very center of our applicaiton.

![This is the caption\label{OnionDiagram}](https://raw.githubusercontent.com/ITU-BDSA23-GROUP12/Chirp/main/docs/images/OnionDiagram.png)

The next layer in the architecture is the data layer which manifests in Chirp as the _Chirp.Infrastructure_ package. Here lies all classes related to the datahandling and databasemanagement. Following dependencies going towards the center, this package depend on the _Chirp.Core_ package, as it holds the implementation of the repository interfaces. This dependency shows how the architecture implements the Dependency Inversion principle, which states that implementations should depend on abstractions and not the other way around.

The outermost layer is the entry point for our Chirp application. In the code this is the _Chirp.Web_ package, which holds a RazorPage Application. This package depends on both the _Chirp.Core_ and the _Chirp.Infrastructure_ package. It is also in this class the Program.cs file resides, which is responsible for configuring the application, including the configuration for dependency injection. Here it configures implementation from the _Chirp.Infrastructure_ layer to be used when abstractions from the _Chirp.Core_ layer is requested.

For a more indepth visualization of which classes resides in which of the packages mentioned, see the png-file located at 'Chirp/docs/images/PackageDiagram.drawio.png', a complete UML package diagram for _Chirp!_.
## Architecture of deployed application

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