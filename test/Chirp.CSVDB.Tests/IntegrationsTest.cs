namespace Chirp.CSVDB.Tests;
using SimpleDB;
using Chirp.CLI;
using System.Diagnostics;

public class UnitTest1
{
    [Fact]
    public void StoredTestEqualsReadTest()
    {

    
        //Arrange
        IDatabaseRepository<Cheep> db = CSVDatabase<Cheep>.Instance;
        var storedCheep = new Cheep("ropf", "some message", 1690891760);
        Cheep readCheep;
            
        //Act 
        db.Store(storedCheep);
        IEnumerable<Cheep> cheeps = db.Read(1);
        readCheep = cheeps.Last();
    
        //Assert
        Assert.Equal(storedCheep, readCheep);
    }
}