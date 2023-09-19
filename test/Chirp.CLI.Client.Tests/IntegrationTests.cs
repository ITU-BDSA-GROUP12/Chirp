namespace Chirp.Tests;
using Chirp.CLI;

public class IntegrationTest
{
    [Fact]
    public void GetOutputStringTest()
    {
        //Arrange
        Cheep cheep = new Cheep("GPT", "HelloWorld", 1690891760);
        string expected = "GPT @ 08/01/23 14:09:20 : HelloWorld";

        //Act  
        string result = UserInterface.GetOutputString(cheep);

        //Assert
        Assert.Equal(expected, result);

    }
}
