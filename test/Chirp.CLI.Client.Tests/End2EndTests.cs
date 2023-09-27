using System;
using System.Globalization;
using System.Diagnostics;
using CsvHelper;
using Chirp.CLI;
using System.IO;
using System.Runtime.InteropServices;

public class End2End
{


    [Fact]
    public void Test_That_A_Cheep_Is_Stored_As_Expected()
    {
        // Arrange
        // ArrangeTestDatabase();
        // Act
        using (var process = new Process())
        {
            process.StartInfo.FileName = dotNetPath();
            process.StartInfo.Arguments = "bin/Debug/net7.0/Chirp.CLI.dll cheep \"this is a cheep for testing E2E cheeping\""; //The cheep msg
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI.Client/";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Synchronously read the standard output of the spawned process.
            process.WaitForExit();
        }

        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = dotNetPath();
            process.StartInfo.Arguments = "bin/Debug/net7.0/Chirp.CLI.dll read --limit 1";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI.Client/";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }

        // Assert
        System.Console.WriteLine(output);
        Assert.EndsWith("this is a cheep for testing E2E cheeping", output.Trim());

        RemoveLastCheep(); //Cleanup step
    }

    [Fact]
    public void Test_Read_Cheep_Limit_1()
    {
        // Arrange
        // ArrangeTestDatabase();
        // Act
        using (StreamWriter writer = File.AppendText("../../../../../src/Chirp.CSVDBService/data/chirp_cli_db.csv"))
        using (CsvWriter csv = new CsvWriter(writer , CultureInfo.InvariantCulture))
        {
            csv.NextRecord();
            csv.WriteRecord(new Cheep("allan","this is a cheep for testing E2E reading",1694520339));
        }


        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = dotNetPath();
            process.StartInfo.Arguments = "bin/Debug/net7.0/Chirp.CLI.dll read --limit 1";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI.Client/";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        // string[] lines = output.Split("\n");
        // string cheep1 = lines[0].Trim();
        // string cheep2 = lines[1].Trim();
        // string cheep3 = lines[2].Trim();
        // string cheep4 = lines[3].Trim();
        // string cheep5 = lines[4].Trim();

        // Assert

        Assert.StartsWith("allan" , output.Trim());
        Assert.EndsWith("this is a cheep for testing E2E reading" , output.Trim());

        RemoveLastCheep(); //Cleanup step
    }

    //A cleanup step that removes the cheep created by the tests.
    private void RemoveLastCheep() //This method is written by chat.openai.com
    {
        string csvFilePath = "../../../../../src/Chirp.CSVDBService/data/chirp_cli_db.csv";

        // Read all records from the CSV file
        List<Cheep> cheepRecords = new List<Cheep>();
        using (StreamReader reader = new StreamReader(csvFilePath))
        using (CsvReader csvReader = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            cheepRecords = csvReader.GetRecords<Cheep>().ToList();
        }

        // Check if there are records to delete
        if (cheepRecords.Count > 0)
        {
            // Remove the last cheep record
            cheepRecords.RemoveAt(cheepRecords.Count - 1);

            // Write the modified data back to the CSV file
            using (StreamWriter writer = new StreamWriter(csvFilePath))
            using (CsvWriter csvWriter = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csvWriter.WriteRecords(cheepRecords);
            }
        }
        
    }

    // private void ArrangeTestDatabase()
    // {

    //     IEnumerable<Cheep> records = new List<Cheep> {
    //         new Cheep("ropf","Hello, BDSA students!",1690891760),
    //         new Cheep("rnie","Welcome to the course!",1690978778),
    //         new Cheep("rnie","I hope you had a good summer.",1690979858),
    //         new Cheep("ropf","Cheeping cheeps on Chirp :)",1690981487),
    //         new Cheep("allan","Rasmus Cock er cool :)",1693905159),
    //         new Cheep("allan","Hello coffee! Can you formulate this better?",1693905353),
    //         new Cheep("allan","Rasmus Cock er ok ig",1694520339),

    //     };
    //     using (StreamWriter writer = File.AppendText("../../../../../src/Chirp.CSVDBService/data/chirp_cli_db.csv"))
    //     using (CsvWriter csv = new CsvWriter(writer , CultureInfo.InvariantCulture))
    //     {
    //         csv.WriteRecords(records);
    //     }
    // }

    //Generate path for dotnetcore based on platform
    private string dotNetPath()
    {   
        
       //This line is taken from chat.openai.com
        string path;
        if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            path = "/usr/local/share/dotnet/dotnet";
        } else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            path = @"C:\program files\dotnet\dotnet";
        } else {
            path = "/usr/bin/dotnet";
        }
        return path;
    }
}
