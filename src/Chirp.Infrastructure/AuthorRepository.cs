namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    // The AuthorRepository class, implementing the IAuthorRepository interface.
    // An object of this class is used by the application to manipulate the authors in the database.


    // The database context for the Chirp database, used by the repository to access the database.
    readonly ChirpDBContext _context;

    // Used to validate an Author entity before storing it in the database. Ensures that invalid data will not reach the database.
    readonly AuthorValidator _validator;
    public AuthorRepository(ChirpDBContext context, AuthorValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<AuthorDto?> GetAuthorDTOByName(string name)
    {
        // Returns an AuthorDto of the first author in the database with the given name, or null if no such author exists.
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == name);
        if (author == null)
        {
            return null;
        }
        return new AuthorDto
        {
            Name = author.Name,
            Email = author.Email
        };
    }

    public async Task<string?> GetAuthorNameByID(Guid id)
    {
        // Returns the name of the author with the given id, or null if no such author exists.
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
        if (author == null)
        {
            return null;
        }

        return author.Name;
    }
    public async Task<AuthorDto?> GetAuthorDTOByEmail(string email)
    {
        // Returns an AuthorDto of the author in the database with the given email, or null if no such author exists.
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.Email == email);
        if (author == null)
        {
            return null;
        }
        return new AuthorDto
        {
            Name = author.Name,
            Email = author.Email
        };
    }

    public async Task CreateAuthor(string name, string email)
    {
        // Creates a new Author entity with the given name and email, and stores it in the database.

        // Create the author object to be stored
        Author newAuthor = new()
        {
            AuthorId = Guid.NewGuid(),
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>(),
            FollowedAuthors = new List<Author>(),
            AuthorFollowers = new List<Author>()
        };


        // Ensure that the author is valid under the rules defined in the AuthorValidator class.
        FluentValidation.Results.ValidationResult validationResult = _validator.Validate(newAuthor);
        if (!validationResult.IsValid)
        {
            // If someone attempted to store invalid data, they are informed by an exception.
            throw new ValidationException("Attempted to store invalid Author in database.");
        }
        // If the data is valid, the author is stored in the database.
        _context.Authors.Add(newAuthor);
        await _context.SaveChangesAsync();
    }

    public async Task FollowAnAuthor(string followingEmail, string followedName)
    {
        // Manipulate the database so that an author with the given email follows an author with the given name.

        // Fetch the respective authors from the database.
        var followingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == followingEmail);
        var followedAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Name == followedName);

        // Check if either followingAuthor or followedAuthor is null.
        if (followingAuthor == null || followedAuthor == null)
        {
            throw new Exception("One or both authors not found");
        }

        // Ensure that FollowedAuthors collection is initialized.
        followingAuthor.FollowedAuthors ??= new List<Author>();

        // Check if the followedAuthor is not already in the FollowedAuthors collection.
        if (!followingAuthor.FollowedAuthors.Contains(followedAuthor))
        {
            // Update the database to reflect the change.
            followingAuthor.FollowedAuthors.Add(followedAuthor);
            await _context.SaveChangesAsync();
        }
        else
        {
            // If the user attempts to follow an author they are already following, an error has occured and an exception is thrown.
            throw new Exception("User already follows this author");
        }
    }

    public async Task UnFollowAnAuthor(string followingEmail, string unFollowingName)
    {
        // Manipulate the database so that an author with the given email unfollows an author with the given name.

        // Fetch the respective authors from the database.
        var followingAuthor = await _context.Authors
            .Include(a => a.FollowedAuthors)   // https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager?fbclid=IwAR2_3oULGneiqhQgfwLOUrUekZxhatAFzAhK6QWegG6qSv8UpxGa8mafOVE
            .FirstOrDefaultAsync(a => a.Email == followingEmail);

        var unFollowingAuthor = await _context.Authors
            .Include(a => a.FollowedAuthors)
            .FirstOrDefaultAsync(a => a.Name == unFollowingName);

        // Check if either followingAuthor or unFollowingAuthor is null
        if (followingAuthor == null || unFollowingAuthor == null)
        {
            throw new Exception("One or both authors not found");
        }



        // Check if the unFollowingAuthor is in the FollowedAuthors collection
        if (followingAuthor.FollowedAuthors.Contains(unFollowingAuthor))
        {
            // Update the database to reflect the change.
            followingAuthor.FollowedAuthors.Remove(unFollowingAuthor);
            await _context.SaveChangesAsync();
        }
        else
        {
            // If the user attempts to unfollow an author they are not following, an error has occured and an exception is thrown.
            throw new Exception("User is trying to unfollow an author they are not following");
        }
    }

    public async Task<List<Guid>?> GetFollowedAuthors(string? authorEmail)
    {
        // Return a list of the IDs of the authors followed by the author with the given email.
        // If the author does not exist, return null.
        // If the author has no followers, return an empty list.
        if (authorEmail == null)
        {
            return null;
        }
        // Fetch the author from the database, from the provided email.
        var author = await _context.Authors
            .Include(a => a.FollowedAuthors)   // https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager?fbclid=IwAR2_3oULGneiqhQgfwLOUrUekZxhatAFzAhK6QWegG6qSv8UpxGa8mafOVE
            .Where(a => a.Email == authorEmail)
            .FirstOrDefaultAsync();


        if (author == null)
        {
            return null;
        }

        List<Guid> followedAuthors = new(); // The list of IDs to be returned.
        foreach (var followedAuthor in author.FollowedAuthors)
        {
            // Fetch the corresponding Author entity for each Guid in FollowedAuthors
            if (followedAuthor != null)
            {
                followedAuthors.Add(followedAuthor.AuthorId);
            }
        }

        return followedAuthors;
    }

    public async Task<List<Guid>?> GetAuthorFollowers(string? authorEmail)
    {

        // Return a list of the ids of the authors that follow the author with the given email.
        if (authorEmail == null)
        {
            return null;
        }

        // Fetch the author from the database, from the provided email.
        var author = await _context.Authors
            .Include(a => a.AuthorFollowers)   // https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager?fbclid=IwAR2_3oULGneiqhQgfwLOUrUekZxhatAFzAhK6QWegG6qSv8UpxGa8mafOVE
            .Where(a => a.Email == authorEmail)
            .FirstOrDefaultAsync();


        if (author == null)
        {

            return null;
        }

        List<Guid> authorFollowers = new(); // The list of guids to be returned.
        foreach (var follower in author.AuthorFollowers)
        {
            // Fetch the corresponding Author entity for each Guid in FollowedAuthors.
            if (follower != null)
            {
                authorFollowers.Add(follower.AuthorId);
            }
        }

        return authorFollowers;
    }

    public async Task DeleteAuthor(string? authorEmail)
    {
        // Permanently delete the author, the author's cheeps, and the the follow information about a user with the given email.

        // Fetch the author to be deleted from the database, from the provided email.
        var author = await _context.Authors
            .Where(a => a.Email == authorEmail)
            .FirstOrDefaultAsync();

        // If the user of the method attempts to delete an author that does not exist, an error has occured. Throw an exception.
        if (author is null)
        {
            throw new Exception("This should not happen. Author cannot be found for deletion.");
        }

        // Here we only call the DbContext.Remove on the author. The rest of the information is deleted by the database due to 
        // a cascade delete defined by the ORM.
        _context.Remove(author);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Guid>?> GetFollowersFollower(string? authorEmail)
    {
        // Let A be the author with the given email. 
        // Let B be the set of authors followed by A. 
        // Let C be the set of authors that are followed by an author in B.
        // Let C1 be the subset of C that is followed by more than one author in B, but not by A.
        // This method returns a list of IDs of the authors in C1.

        var A = await _context.Authors
            .Include(a => a.FollowedAuthors)
             .ThenInclude(fa => fa.FollowedAuthors)
            .Where(a => a.Email == authorEmail)
            .FirstOrDefaultAsync();

        if (A == null)
        {
            return null;
        }

        // Each author in C and how many times they are followed by an author in B.
        Dictionary<Author, int> CAuthorOccurancesPair = new();

        // Iterate through B.
        foreach (var authorInB in A.FollowedAuthors)
        {

            // Iterate through C.
            foreach (var authorInC in authorInB.FollowedAuthors)
            {
                if (authorInC != null)
                {
                    if (CAuthorOccurancesPair.ContainsKey(authorInC))
                    {
                        CAuthorOccurancesPair[authorInC] = CAuthorOccurancesPair[authorInC] + 1;
                    }
                    else
                    {
                        CAuthorOccurancesPair.Add(authorInC, 1);
                    }
                }
            }
        }


        // A list of the set C1. The list of IDs to be returned.
        List<Guid> C1 = [];
        foreach (var authorInC in CAuthorOccurancesPair.OrderByDescending(key => key.Value))
        {
            // If the authorInC is followed by more than one author in B, but not by A, add the ID to C1.
            if (authorInC.Value > 1 && !A.FollowedAuthors.Contains(authorInC.Key) && A.AuthorId != authorInC.Key.AuthorId)
            {
                C1.Add(authorInC.Key.AuthorId);
            }
        }
        return C1;
    }

}