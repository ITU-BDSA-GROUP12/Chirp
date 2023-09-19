using System;
using System.Globalization;
using System.Diagnostics;
using CsvHelper;
using Chirp.CLI;

public class End2End
{
    [Fact]
    public void TestReadCheep()
    {
        // Arrange
        ArrangeTestDatabase();
        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "C:/Program Files/dotnet/dotnet.exe";
            process.StartInfo.Arguments = "C:/Mads/progs/analysis_design_and_software_architecture/project/Chirp/test/Chirp.CLI.Client.Tests/bin/Debug/net7.0/Chirp.CLI.dll read --limit 5";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI.Client/";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        string[] lines = output.Split("\n");
        string cheep1 = lines[0];
        string cheep2 = lines[1];
        string cheep3 = lines[2];
        string cheep4 = lines[3];
        string cheep5 = lines[4];

        // Assert

        string mikkel = "hej mikkel";
        Assert.EndsWith("mikkel" , mikkel);

        Assert.StartsWith("rnie" , cheep1);
        Assert.EndsWith("I hope you had a good summer" , cheep1);
    
        Assert.StartsWith("ropf" , cheep2);
        Assert.EndsWith("Cheeping cheeps on Chirp :)" , cheep2);
        
        Assert.StartsWith("allan" , cheep3);
        Assert.EndsWith("Rasmus Cock er cool :)" , cheep3);
        
        Assert.StartsWith("allan" , cheep4);
        Assert.EndsWith("Hello coffee! Can you formulate this better?" , cheep4);
        
        Assert.StartsWith("allan" , cheep5);
        Assert.EndsWith("Rasmus Cock er ok ig" , cheep5);
    }
    private void ArrangeTestDatabase()
    {

        IEnumerable<Cheep> records = new List<Cheep> {
            new Cheep("ropf","Hello, BDSA students!",1690891760),
            new Cheep("rnie","Welcome to the course!",1690978778),
            new Cheep("rnie","I hope you had a good summer.",1690979858),
            new Cheep("ropf","Cheeping cheeps on Chirp :)",1690981487),
            new Cheep("allan","Rasmus Cock er cool :)",1693905159),
            new Cheep("allan","Hello coffee! Can you formulate this better?",1693905353),
            new Cheep("allan","Rasmus Cock er ok ig",1694520339),

        };
        using (StreamWriter writer = new StreamWriter("../../../../../src/chirp_cli_db.csv"))
        using (CsvWriter csv = new CsvWriter(writer , CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}
