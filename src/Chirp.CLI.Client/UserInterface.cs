using System.Collections.Generic;

namespace Chirp.CLI;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (Cheep cheep in cheeps)
        {
            Console.WriteLine(GetOutputString(cheep));
        }
    }

    public static string GetOutputString(Cheep cheep)
    {
        string formattedTimeStamp = ConvertToTimestamp(cheep.Timestamp);
        return $"{cheep.Author} @ {formattedTimeStamp} : {cheep.Message}";
    }

    public static string ConvertToTimestamp(long Seconds)
    {
        long offset = (long)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalSeconds; //Get offset from UTC in seconds
        long timeSeconds = Seconds + offset;
        var timeStamp = DateTimeOffset.FromUnixTimeSeconds(timeSeconds).DateTime; //Convert to DateTime
        string formattedTimeStamp = timeStamp.ToString("MM/dd/yy HH:mm:ss"); //Format timeStamp - used GPT for this
        return formattedTimeStamp;
    }

}
