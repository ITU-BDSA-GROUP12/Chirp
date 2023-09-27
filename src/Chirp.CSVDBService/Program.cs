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



app.MapGet("/cheeps" , (int? limit) => Read(limit));

app.MapPost("/cheep" , (Cheep cheep) => Store(cheep));

app.Run();

List<Cheep> Read(int? limit = null)
{
    SeedCsvFileIfNeeded();
    string path = "./chirp_cli_db.csv";
    if (limit <= 0)
    {
        throw new ArgumentOutOfRangeException($"{nameof(limit)} must be greater than 0");
    }
    if (Path.Exists(path))
    {
        using (StreamReader reader = new StreamReader(path))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            List<Cheep> all_records = csv.GetRecords<Cheep>().ToList();
            return all_records;
        }
    }
    else
    {
        Console.WriteLine($"There are no cheeps to read from here: {path}\nPlease specify a different path, or simply begin cheeping to create some cheeps!");
        return null;
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
    Console.WriteLine("hej");
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

        Console.WriteLine("CSV file created and seeded with initial data.");
    }
}
public record Cheep(string Author , string Message , long Timestamp);




