using System.IO;
using System;
using CsvHelper;
using System.Globalization;
using System.ComponentModel.Design;
using System.CommandLine;
using System.Runtime.CompilerServices;
using SimpleDB;

//All references to "GPT" in comments are references to chat.opanai.com

class Program
{
    static async Task Main(string[] args)
    {


        string path = "../src/chirp_cli_db.csv"; //Path to CSV file 

        //the rootCommand is "dotnet run"
        var rootCommand = new RootCommand();

        // by making a new Command, we create a subCommand to the rootCommand, namingly "read"
        var readCommand = new Command("read", "Displays cheeps");

        // by making a new Option, we create anoption to the subCommand, namingly "--limit"
        // Which gives an integer to our data_access.Read(), so that we limit the amount of cheeps displayed
        var limitOption = new Option<int?> //"int?" makes the interger nullable
            (name: "--limit",
            description: "limits the number of cheeps to be displayed",
            getDefaultValue: () => null );


        //we add the readCommand to the rootCommand, to engage with readCommand type "dotnet run read" in the terminal
        rootCommand.Add(readCommand);
        //We add the limitOption to the readCommand, to engage with limitOption type "dotnet run read --limit <int>" in the terminal
        readCommand.Add(limitOption);

        // gets the commands from the commandline and exectutes the following code
        readCommand.SetHandler((limitOption) =>
        {
            //CSV Read part from: https://joshclose.github.io/CsvHelper/getting-started/
            IDatabaseRepository<Cheep> data_access = new CSVDatabase<Cheep>("../src/chirp_cli_db.csv");
            IEnumerable<Cheep> records = data_access.Read(limitOption);

            foreach (Cheep cheep in records)
            {
                long timeSeconds = cheep.Timestamp + 7200; //Plus 7200 to adjust timezone 
                var timeStamp = DateTimeOffset.FromUnixTimeSeconds(timeSeconds).DateTime; //Convert to DateTime
                string formattedTimeStamp = timeStamp.ToString("dd/MM/yy HH:mm:ss"); //Format timeStamp - used GPT for this

                Console.WriteLine($"{cheep.Author} @ {formattedTimeStamp} : {cheep.Message}");
            }

        }, limitOption);

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
            if (args[0]=="cheep")
                {   //CSV Write part from: https://joshclose.github.io/CsvHelper/getting-started/
                    long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; //Used GPT for this
                     Cheep cheep = new Cheep(Environment.UserName , args[1] , unixTimestamp);
                    data_access.Store(cheep);
        }*/

        if (args[0] == "cheep")
        {   //CSV Write part from: https://joshclose.github.io/CsvHelper/getting-started/
            long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; //Used GPT for this
            Cheep cheep = new Cheep(Environment.UserName, args[1], unixTimestamp);
            using (StreamWriter sw = File.AppendText(path))
            using (CsvWriter csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(cheep);
            }
        }
        await rootCommand.InvokeAsync(args);


    }

    public record Cheep(string Author, string Message, long Timestamp);

}