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
We managed to implement all the functionality we aspired to implement during this project. A lot of features could have been added, but we decided on some core features that we felt like we could manage within the time frame, and managed those. Had we more time, we could have discussed which other features we might implement. 
Examples:
- A character count above the cheep box, denoting how many characters can yet be used, up to 160.
- Profile pictures, fetched from GitHub.
- Functionality to reference other users in a cheep, and a page with the newest cheeps in which the user is tagged.
- Emoticon reactions to cheeps, such as happy face or tractor.
- Comments under cheeps.
- A link redirecting to a user's GitHub page.

#### Work flow
We have implemented the code in short incremental steps as, starting with a GitHub issue, describing a desired change in the code, who would benefit from this change, a more detailed description and a definition of done.
The issues are created to match the project description provided by our professors Helge and Rasmus, or to match changes and details that we as a group or individually found relevant to implement.
On creation, an issue is placed in one of three categories:
- To-do: a place for issues that contain an overall idea for the group to think on, not something that should be implemented as code. 
- Backlog: An issue in the code that has no immediate solution; the group must talk about it and figure out how to turn this into an issue ready for implementation OR an issue that has a concrete solution, but should not be focused on yet, as the code base is not ready or the issue is not relevant to work with at the moment
- Ready: Issues ready for a team member to assign themselves and being implementation.

A team member then creates a GitHub branch with the name of the issue and starts on implementing the change described.
Often we have worked on larger/more fundemental changes in groups of 2-3 or as the whole team (mob programming).
We have (with varying consistency) written the code for an issue in smaller commits of working code.
When the code for an issue is done, the code is pushed to the online repository on its branch. A pull request is made, the code is reviewed by one or more team members, different from the author of the code. The reviewer may then decide if more work is needed from the issue asignee, or if the change is ready to merge with the main branch.
Upon merge, the issue is deleted.
See figure \ref{The life of an issue}, a user flow diagram showing the process of an issue:
![The life of an issue](https://github.com/ITU-BDSA23-GROUP12/Chirp/blob/366-process-team-work/docs/images/issue%20process.drawio.png)
## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others