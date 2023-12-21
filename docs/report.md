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
The issues are created to match the project description provided by our professors, or to match changes and details that we as a group or individually found relevant to implement.
On creation, an issue is placed in one of three categories:
- To-do: a place for issues that contain an overall idea for the group to think on, not something that should be implemented as code. 
- Backlog: An issue in the code that has no immediate solution; the group must talk about it and figure out how to turn this into an issue ready for implementation OR an issue that has a concrete solution, but should not be focused on yet, as the code base is not ready or the issue is not relevant to work with at the moment
- Ready: Issues ready for a team member to assign themselves and being implementation.

When a team member takes responsibility for an issue, he creates a branch linked to the issue with the name of the issue and starts on implementing the change described.
Often we have worked on larger/more fundemental changes in groups of 2-3 or as the whole team (mob-/pair programming).
We have (with varying consistency) written the code for an issue in smaller commits of working code.
When the code for an issue is done, the code is pushed to the online repository on its branch. A pull request is made, the code is reviewed by one or more team members, different from the author of the code. The reviewer may then decide if more work is needed from the issue asignee, or if the change is ready to merge with the main branch.
Upon merge, the issue is closed and moved to Done in our project board.
See figure \ref{The life of an issue}, a user flow diagram showing the process of an issue:
![The life of an issue](https://github.com/ITU-BDSA23-GROUP12/Chirp/blob/366-process-team-work/docs/images/issue%20process.png?raw=true)
## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others