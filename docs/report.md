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
The gist of our application lies in two main activities; writing cheeps and reading cheeps. A **cheep** is a short message of 1-160 characters, and is not messaged to a particular person. Each cheep is publicly available for everyone to see, and there is not functionality to direct the attention of particular users onto your cheep. Lastly, the user has an option to follow other users, to get easier access to cheeps written by them. \ref{Authenticated Public Timeline}
The use of our application is defined by a range of tabs, a **cheep box** and two different kinds of users - **authenticated** users and **unauthenticated** users. That is, users that are logged in and users that are not. The two kinds of users differ in the elements that they have access to on the site. 
Below is a screenshot of the main page and then a short description of the different elements.
![Chirp public timeline\label{Authenticated Public Timeline}](https://raw.githubusercontent.com/ITU-BDSA23-GROUP12/Chirp/main/docs/images/public%20timeline.png)

##### Public Timeline
The **public timeline** contains a list of each (non-deleted) cheep ever sent, and is available to both authenticated and unauthenticated users. The cheeps are displayed on pages of 32 cheeps, and the pages can browsed between using the **next** and **previous** buttons on the button of the page.
##### Register
If the user has not registered before, clicking the **register** tab redirects the user to a prompt for signing in to GitHub to continue to Chirp. After signing in with GitHub, the user is registered and logged in to Chirp, and is now an authenticated user. If the user have registered before, this information might still be stored in cookies managed by the browser, and will then not be asked to sign in with GitHub - they will simply be logged in right away and immediately turn into an authenticated user. Once authenticated, the user's GitHub username is also used as the user's Chirp username.
Only unauthenticated users have access to the register tab.
##### Login
The **login** tab does the exact same thing as the register tab - their behaviour is the same in every way.
This tab is also only accessible for unauthenticated users.
##### The follow mechanic
An authenticated user has, attached to each cheep on the public timeline, a **follow** button. Uppon clicking this, the follow button is replaced by an **unfollow** button, and the user will be regarded as following the user that sent the particular cheep.
##### My Timeline
The **my timeline** tab is similar to the public timeline, but here only the user's own cheeps and cheeps written by users that the user is following are displayed. This tab is only accessible for authenticated users, and is only privately accessible.
##### Discover
Let the user of the application be A.
The **discover** tab contains the latest cheep of each user B that is deemed interesting for A. B is deemed interesting if at least two users followed by A are following B. Here, A can browse through users that might be more relevant to A. The discover tab is only accessible to authenticated users.
##### Logout
The **logout** tab turns the user into an unauthenticated user. This tab is only accessible to authenticated users.
##### About me
The **about** me page displays information about the user. It:
- displays the GitHub **username** and **email** used for authentication. 
- holds a **Forget Me!** button that **deletes all information** accisiated with the user, including sent cheeps, from the application.
- displays a list of all **cheeps written** by the user.
- displays a list of all **users followed** by the user.
- displays a list of all **users following** the user.

The about me tab is only accessible for authenticated users, and is only privately accessible.
##### The cheep box
The **cheep box** is a text entry field accompanied with a **Share 🚜** button to send any text entered as a cheep. The cheep box is available on the **public timeline** page and on the **my timeline** page.
#### Sending a cheep
Below, a user flow diagram showing a typical scenario of a user logging in and sending a cheep.
![Sending a cheep user flow diagram\label{sending cheep}](https://github.com/ITU-BDSA23-GROUP12/Chirp/blob/main/docs/images/cheep%20user%20flow.png)
    
## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others