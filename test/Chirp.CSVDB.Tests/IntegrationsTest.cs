namespace Chirp.CSVDB.Tests;
using SimpleDB;
using Chirp.CLI; 


public class UnitTest1
{
    [Fact]
    public void StoredTestEqualsReadTest()
    {

    
        //Arrange
        IDatabaseRepository<Cheep> db = new CSVDatabase<Cheep>("../../../../test_db.csv");
        var storedCheep = new Cheep("ropf", "Hello, BDSA students!", 1690891760);
        Cheep readCheep;
            
        //Act 
        db.Store(storedCheep);
        IEnumerable<Cheep> cheeps = db.Read(1);
        readCheep = cheeps.Last();
    
        //Assert
        Assert.Equal(storedCheep, readCheep);
    }
}