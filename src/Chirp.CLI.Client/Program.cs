using System.IO;
using System;
using CsvHelper;
using System.Globalization;
using System.ComponentModel.Design;
using System.CommandLine;
using System.Runtime.CompilerServices;
using SimpleDB;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

//All references to "GPT" in comments are references to chat.opanai.com

namespace Chirp.CLI;

//Definition of the record, used by the csvHelper to format the values in the csv file to variables we define.
    public record Cheep(string Author, string Message, long Timestamp);

public class Program
{
    //The accesspoint to the database, to the singleton pattern CSVDatabase
    static IDatabaseRepository<Cheep> data_access = CSVDatabase<Cheep>.Instance;

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
        var limitOption = new Option<int?>
            (name: "--limit",
            description : "limits the number of cheeps to be displayed",
            parseArgument: result => //from validation https://learn.microsoft.com/en-us/dotnet/standard/commandline/model-binding#custom-validation-and-binding
                {
                    if (!int.TryParse(result.Tokens.First().Value, out int limitValue))
                    {
                        result.ErrorMessage = "--limit only accepts an integer as an argument";
                        return null;
                    }
                    return limitValue;
                }
            );


        //we add the readCommand to the rootCommand, to engage with readCommand type "dotnet run read" in the terminal
        rootCommand.Add(readCommand);

        //we add the cheepCommand to the rootCommand, to engage with cheepCommand type "dotnet run cheep" in the terminal
        rootCommand.Add(cheepCommand);

        // //We add the messageArgument to the cheepCommand, to engage with messageArgument type "dotnet run cheep <'strimg'> " in the terminal
        cheepCommand.Add(messageArgument);

        //We add the limitOption to the readCommand, to engage with limitOption type "dotnet run read --limit <int>" in the terminal
        readCommand.Add(limitOption);

        // gets the commands from the commandline and exectutes the following code
        readCommand.SetHandler(async (limitOptionValue) =>
        {
            List<Cheep> records;
            string baseURL = "http://bdsagroup12bchirpremotedb.azurewebsites.net/";
            string uri = $"cheeps?limit={limitOptionValue}";

            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);

            var response = await client.GetAsync(uri); 

            if (response.IsSuccessStatusCode)//response.IsSuccessStatusCode From GPT
            {
                // The HTTP request was successful
                records = await response.Content.ReadFromJsonAsync<List<Cheep>>();
                //The records are passed to the UserInterface, which handles how the records are presented to the user
                UserInterface.PrintCheeps(records);
            }
            else
            {
                // Handle the case when the HTTP request was not successful - From GPT
                string errorMessage = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                System.Console.WriteLine($"Error Message: {errorMessage}");
            }
        }, limitOption);

        //Handling of reseving message and pass it to the database
        cheepCommand.SetHandler(async (messageArgumentValue) => 
        {   
            string baseURL = "http://bdsagroup12bchirpremotedb.azurewebsites.net/";
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);
            
            long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; //Used GPT for this
            Cheep cheep = new Cheep(Environment.UserName, messageArgumentValue, unixTimestamp);
            // data_access.Store(cheep);
            
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "/cheep" , cheep
            );
            response.EnsureSuccessStatusCode();
            /*static async Task PostAsync(client)
            {
                using StringContent jsonContent = new(
                    JsonSerializer.Serialize(cheep) , 
                    Encoding.UFT8 , 
                    "application/json")
            }*/
        }, messageArgument);

        await rootCommand.InvokeAsync(args);
    }
}

 
