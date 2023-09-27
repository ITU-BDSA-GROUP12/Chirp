using Chirp.CSVDBService;

namespace Chirp.CSVDBService.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_HTTP_resssponeMsg_if_limit_is_set_to_1()
    {
        //Arange
        public record Cheep(string Author , string Message , long Timestamp);
        //Act
        List<Cheep> responsList = Read(1);
        //Assert
        
    }
}