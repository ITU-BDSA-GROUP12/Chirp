using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.ComponentModel.Design;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/cheeps" , (int? limit) => 
            {           
                List<Cheep> response;                 //From https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0 
                try{                                  //Section: IResult return values
                    response = Read(limit);           // Try-catch: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/exception-handling-statements
                } 
                catch (ArgumentOutOfRangeException e){ 
                    return Results.BadRequest(e.Message); // this line is from GPT
                } 
                catch (FileNotFoundException e){ 
                    return Results.NotFound(e.Message);
                }
                //The response is a list of Cheeps
                return Results.Ok(response);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


app.MapPost("/cheep" , (Cheep cheep) => Store(cheep));

app.Run();

List<Cheep> Read(int? limit = null) //Reads the cheeps, whit a limit as an option
{
    SeedCsvFileIfNeeded();
    string path = "./chirp_cli_db.csv";

    if (limit <= 0)
    {
        throw new ArgumentOutOfRangeException($"--limit must be greater than 0");
    }
    if (Path.Exists(path))
    {
        using (StreamReader reader = new StreamReader(path))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            List<Cheep> all_records = csv.GetRecords<Cheep>().ToList();
            List<Cheep> records = new List<Cheep>();
            if(limit < all_records.Count){
                for (int i = all_records.Count-1; i >= all_records.Count- limit; i--){ //this forloop insures getting the lates cheeps
                    records.Add(all_records[i]);
                }
            } else {
                records = all_records;
            }
            return records;
        }
    }
    else
    {
        throw new FileNotFoundException($"There are no cheeps to read from here: {path}\nPlease specify a different path, or simply begin cheeping to create some cheeps!");
    }
}

void Store(Cheep record)
{
    SeedCsvFileIfNeeded();
    string path = "./chirp_cli_db.csv";
    using (StreamWriter sw = File.AppendText(path))
    using (CsvWriter csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
    {
        csv.NextRecord();
        csv.WriteRecord(record);
    }
}

//This method creates the csv file if it doesnt exits. This way it is made on the azure site.
void SeedCsvFileIfNeeded()
{
    string path = "./chirp_cli_db.csv";

    if (!File.Exists(path))
    {
        // Create the file and add initial data
        using (StreamWriter sw = File.AppendText("./chirp_cli_db.csv"))
        using (CsvWriter csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
        {
            // Add header row if needed
            sw.WriteLine("Author,Message,Timestamp");

            // Add initial data rows
            long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; //Used GPT for this
            Cheep cheep = new Cheep("Admin","Welcome to a clean, empty Chirp-Server",unixTimestamp);
            
            csv.NextRecord();
            csv.WriteRecord(cheep);

            // You can add more initial data rows as needed
        }
    }
}
public record Cheep(string Author , string Message , long Timestamp);




