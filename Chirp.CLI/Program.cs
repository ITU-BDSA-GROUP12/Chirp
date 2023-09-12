using System.IO;
using System;
using CsvHelper;
using System.Globalization;
using System.ComponentModel.Design;
using System.CommandLine;
using System.Runtime.CompilerServices;

//All references to "GPT" in comments are references to chat.opanai.com

class Program
{
    static async Task Main(string[] args)
    {

        string path = "../src/chirp_cli_db.csv"; //Path to CSV file 
        var rootCommand = new RootCommand();
        var readCommand = new Command("read", "Displays cheeps");

        var limitOption = new Option<int>
            (name: "--limit",
            description: "limits the number of cheeps to be displayed",
            getDefaultValue: () => 30);



        rootCommand.Add(readCommand);
        readCommand.Add(limitOption);

        readCommand.SetHandler((limitOption)=>{
            //CSV Read part from: https://joshclose.github.io/CsvHelper/getting-started/
            using (StreamReader reader = new StreamReader(path))
            using (CsvReader csv = new CsvReader(reader,CultureInfo.InvariantCulture))
            {
                IEnumerable<Cheep> records = csv.GetRecords<Cheep>(); // Reading cheeps from CSV File
                
                foreach (Cheep cheep in records)
                {
                    long timeSeconds = cheep.Timestamp + 7200; //Plus 7200 to adjust timezone 
                    var timeStamp = DateTimeOffset.FromUnixTimeSeconds(timeSeconds).DateTime; //Convert to DateTime
                    string formattedTimeStamp = timeStamp.ToString("dd/MM/yy HH:mm:ss"); //Format timeStamp - used GPT for this

                    Console.WriteLine($"{cheep.Author} @ {formattedTimeStamp} : {cheep.Message}");
                }
            }
        },limitOption);

        /*

        if (args[0]=="read")
        {   //CSV Read part from: https://joshclose.github.io/CsvHelper/getting-started/
            using (StreamReader reader = new StreamReader(path))
            using (CsvReader csv = new CsvReader(reader,CultureInfo.InvariantCulture))
            {
                IEnumerable<Cheep> records = csv.GetRecords<Cheep>(); // Reading cheeps from CSV File
            
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
        }*/

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
    await rootCommand.InvokeAsync(args);
    
}

    public record Cheep(string Author , string Message , long Timestamp);

}