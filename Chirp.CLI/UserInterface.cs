using System.Collections.Generic;

namespace Chirp.CLI;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (Cheep cheep in cheeps)
            {
                long timeSeconds = cheep.Timestamp + 7200; //Plus 7200 to adjust timezone 
                var timeStamp = DateTimeOffset.FromUnixTimeSeconds(timeSeconds).DateTime; //Convert to DateTime
                string formattedTimeStamp = timeStamp.ToString("dd/MM/yy HH:mm:ss"); //Format timeStamp - used GPT for this

                Console.WriteLine($"{cheep.Author} @ {formattedTimeStamp} : {cheep.Message}");
            }
    }

}
