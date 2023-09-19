using System.Collections.Generic;

namespace Chirp.CLI;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (Cheep cheep in cheeps)
        {
            string formattedTimeStamp = ConvertToTimestamp(cheep.Timestamp);
            Console.WriteLine($"{cheep.Author} @ {formattedTimeStamp} : {cheep.Message}");
        }
    }

    public static string ConvertToTimestamp(long Seconds)
    {
        long timeSeconds = Seconds + 7200; //Plus 7200 to adjust timezone 
        var timeStamp = DateTimeOffset.FromUnixTimeSeconds(timeSeconds).DateTime; //Convert to DateTime
        string formattedTimeStamp = timeStamp.ToString("dd/MM/yy HH:mm:ss"); //Format timeStamp - used GPT for this
        return formattedTimeStamp;
    }

}
