using System.IO;
using System;
using CsvHelper;
using System.Globalization;
using System.ComponentModel.Design;
using System.CommandLine;
using System.Runtime.CompilerServices;
using SimpleDB;

//this is a test comment
// yes ye 
// 20
// hej med dig


//All references to "GPT" in comments are references to chat.opanai.com

namespace Chirp.CLI;

//Definition of the record, used by the csvHelper to format the values in the csv file to variables we define.
    public record Cheep(string Author, string Message, long Timestamp);

public class Program
{
    static string path = "../chirp_cli_db.csv"; //Path to CSV file 
    //The accesspoint to the database
    static IDatabaseRepository<Cheep> data_access = new CSVDatabase<Cheep>(path);

    //The usage of System.CommandLine is inspired by the documentation https://learn.microsoft.com/en-us/dotnet/standard/commandline/
    static async Task Main(string[] args)
    {

        //the rootCommand is "dotnet run"
        var rootCommand = new RootCommand();

        // by making a new Command, we create a subCommand to the rootCommand, namingly "read"
        var readCommand = new Command("read", "Displays cheeps");

        // by making a new Command, we create a subCommand to the rootCommand, namingly "cheep"
        var cheepCommand = new Command("cheep", "stores the message");

        // by making a new Argument, we create an argument to the cheepCommand, namingly "message", which takes a string as argument
        var messageArgument = new Argument<string>(
            name: "message",
            description: "provides a message to be stored"); //Make an Error hanlder

        // by making a new Option, we create an option to the subCommand, namingly "--limit"
        // Which gives an integer to our data_access.Read(), so that we limit the amount of cheeps displayed
        var limitOption = new Option<int?> //"int?" makes the interger nullable
            (name: "--limit",
            description: "limits the number of cheeps to be displayed",
            getDefaultValue: () => null );


        //we add the readCommand to the rootCommand, to engage with readCommand type "dotnet run read" in the terminal
        rootCommand.Add(readCommand);

        //we add the cheepCommand to the rootCommand, to engage with cheepCommand type "dotnet run cheep" in the terminal
        rootCommand.Add(cheepCommand);

        // //We add the messageArgument to the cheepCommand, to engage with messageArgument type "dotnet run cheep <'strimg'> " in the terminal
        cheepCommand.Add(messageArgument);

        //We add the limitOption to the readCommand, to engage with limitOption type "dotnet run read --limit <int>" in the terminal
        readCommand.Add(limitOption);

        // gets the commands from the commandline and exectutes the following code
        readCommand.SetHandler((limitOptionValue) =>
        {
            
            IEnumerable<Cheep> records = data_access.Read(limitOptionValue);
            
            //The records are passed to the UserInterface, which handles how the records are presented to the user
            UserInterface.PrintCheeps(records);

        }, limitOption);

        //Handling of reseving message and pass it to the database
        cheepCommand.SetHandler((messageArgumentValue) => 
        {   
            long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; //Used GPT for this
            Cheep cheep = new Cheep(Environment.UserName, messageArgumentValue, unixTimestamp);
            
            data_access.Store(cheep);

        }, messageArgument);

        await rootCommand.InvokeAsync(args);
    }
}

 