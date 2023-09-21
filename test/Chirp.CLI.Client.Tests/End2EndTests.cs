using System;
using System.Globalization;
using System.Diagnostics;
using CsvHelper;
using Chirp.CLI;
using System.IO;

public class End2End
{


    [Fact]
    public void TestCheeps()
    {
        // Arrange
        ArrangeTestDatabase();
        // Act
        using (var process = new Process())
        {
            process.StartInfo.FileName = dotNetPath();
            process.StartInfo.Arguments = "bin/Debug/net7.0/Chirp.CLI.dll cheep \"this is a test cheep\""; //The cheep msg
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
        Assert.EndsWith("this is a test cheep", output.Trim());
    }

    [Fact]
    public void TestReadCheep()
    {
        // Arrange
        ArrangeTestDatabase();
        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = dotNetPath();
            process.StartInfo.Arguments = "bin/Debug/net7.0/Chirp.CLI.dll read --limit 5";
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
        string cheep1 = lines[0].Trim();
        string cheep2 = lines[1].Trim();
        string cheep3 = lines[2].Trim();
        string cheep4 = lines[3].Trim();
        string cheep5 = lines[4].Trim();

        // Assert

        Assert.StartsWith("rnie" , cheep1);
        Assert.EndsWith("I hope you had a good summer." , cheep1);
    
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

    //Generate path for dotnetcore based on platform
    private string dotNetPath()
    {
        string platform = System.Environment.OSVersion.Platform.ToString(); //This line is taken from chat.openai.com
        string path;
        if(platform == "Unix") {
            path = "/usr/local/share/dotnet/dotnet";
        } else if (platform == "Win32NT") {
            path = @"C:\program files\dotnet\dotnet";
        } else {
            path = "/home/user/share/dotnet/dotnet";
        }
        return path;
    }
}
