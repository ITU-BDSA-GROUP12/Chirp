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
During development of the application, we have used github workflows to automate some processes. It is defined in a YAML file and set to run on a trigger event. We have used one for Build and testing the application, one for creating a release on Github and one for deploying our .NET app to Azure and the schemas for our database. Workflows consist of one or more jobs, which have a sequence of steps that has to be executed. The diagrams below illustrate the steps and jobs of each workflow.

### Build and test workflow
This workflow is triggered when a branch is pushed or a pull request is created. It locates the source code and sets up a .NET Core environment. Then it downloads the missing, if any dependencies are missing, builds the application and runs the test suite. 
This workflow is useful streamlining reviewing pull requests. See figure \ref{BuildAndTest-workflow}
![UML Diagram of build and test workflow\lable{BuildAndTest-workflow}](https://github.com/ITU-BDSA23-GROUP12/Chirp/blob/main/docs/images/BuildAndTest-Workflow.drawio.png?raw=true)


### Release workflow
Whenever a new tag is pushed into main with our format, the release workflow is triggered. Again the .NET Core environment is set up, and then we add the artifact to our release with softprops/action-gh-release.
Initially this streamlining helped making releases with executables on Github, whenever our main was given new tags. However we have later switched to only release source code, as we have evolved _Chirp!_ into a web application. We manually had to check if Main built and passed the tests before given new tags, but could have been included. See figure \ref{Release-workflow}
![UML Diagram of the release workflow\lable{Release-workflow}](https://github.com/ITU-BDSA23-GROUP12/Chirp/blob/main/docs/images/Release-Workflow.drawio.png?raw=true)

### Deployment workflow
This workflow consist of to jobs: 'BuildAndTest' and 'deploy'. Jobs can be run concurrently, but we need 'BuildAndTest' to run successfully, before we want to bother with deploying, hence the key word 'needs', which means we only run the 'deploy' when 'BuildAndTest' is done. 
The workflow can be triggered manually on github or by push to main. Again it sets up a .Net Core environment, restores the dependencies, builds and runs our test suite. Create and upload application  artifacts to the github actions workflow system with the name '.net-app'.  Then the new migrations are bundled together and uploaded with the name 'efbundle'.
Then the second part of the workflow called 'deploy', takes care of uploading the new web-app to Azure and deploy the new migrations to the Azure database. See figure \ref{deploy-workflow}
![UML Diagram of the deploy workflow\lable{deploy-workflow}](https://github.com/ITU-BDSA23-GROUP12/Chirp/blob/main/docs/images/Deployment-Workflow.drawio.png?raw=true)

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
A large level model is used for language understanding and generation. We have used ChatGPT and Co-Pilot in this project. We used these to expedite the coding in certain areas. 
We tried asking ChatGPT when hitting an error that we could not figure out why we recieved. ChatGPT is often helpful with both finding but also explain what the error is to understand _why_ we get the error. This has been great to get a better understanding of what we need to fix to make good and functional code. Furthermore ChatGPT has proven to be a solid tool for discovering keywords and to figure out what documentation needs to be delved into.
ChatGPT is also good at comprehending how components work together, whereas documentation for each part (being Azure, .NET or even GitHub) is really good at focusing on the specific domain. When "integrating" these domains, ChatGPT has been helpful with sharing its insights and filled the gaps between documentation. This has definitely saved us time.
Co-Pilot were used on the fly, as we were coding it would suggest what we might write, and often it was right and expidited the coding. It was also helpful, as we could write a comment stating what we want and Co-Pilot will then suggest the code for this. 
We have always been careful when using these tools as they may be wrong, inaccurate, etc. and researched upon information it gave that we were going to use. The way these tool were used in the process, made the code writing a bit faster, and sometimes **way** faster to debug.
