using System.IO;
using System;
using CsvHelper;
using System.Globalization;
using System.ComponentModel.Design;
using SimpleDB;

//All references to "GPT" in comments are references to chat.opanai.com





if (args[0]=="read")
{   //CSV Read part from: https://joshclose.github.io/CsvHelper/getting-started/
    IDatabaseRepository<Cheep> data_access = new CSVDatabase<Cheep>("../src/chirp_cli_db.csv");
    IEnumerable<Cheep> records = data_access.Read();
    
        if (args.Length == 1) 
        {
            foreach (Cheep cheep in records)
            {
                long timeSeconds = cheep.Timestamp + 7200; //Plus 7200 to adjust timezone 
                var timeStamp = DateTimeOffset.FromUnixTimeSeconds(timeSeconds).DateTime; //Convert to DateTime
                string formattedTimeStamp = timeStamp.ToString("dd/MM/yy HH:mm:ss"); //Format timeStamp - used GPT for this

                Console.WriteLine($"{cheep.Author} @ {formattedTimeStamp} : {cheep.Message}");
            }
        }
        else if (args.Length == 2)
        {
            int cheeps_left = int.Parse(args[1]);
            foreach (Cheep cheep in records)

            {
                Console.WriteLine($"{cheep.Author} @ {cheep.Timestamp} : {cheep.Message}");
                cheeps_left -= 1;
                if (cheeps_left == 0) { break; }
            }
        }
    }
}

if (args[0]=="cheep")
{   //CSV Write part from: https://joshclose.github.io/CsvHelper/getting-started/
    long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; //Used GPT for this
    Cheep cheep = new Cheep(Environment.UserName , args[1] , unixTimestamp);
    using (StreamWriter sw = File.AppendText(path))  
    using (CsvWriter csv = new CsvWriter(sw , CultureInfo.InvariantCulture))
    {
        csv.WriteRecord(cheep);
    }
}


public record Cheep(string Author , string Message , long Timestamp);