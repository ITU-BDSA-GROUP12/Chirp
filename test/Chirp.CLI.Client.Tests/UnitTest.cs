namespace Chirp.Tests;
using Chirp.CLI;

public class UnitTest
{
    [Fact]
    public void ConvertToTimestampTest()
    {
        //Arrange
        long input = 1690891760;

        //Act
        string output = UserInterface.ConvertToTimestamp(input);

        //Assert
        Assert.Equal("08/01/23 14:09:20", output);

    }
}