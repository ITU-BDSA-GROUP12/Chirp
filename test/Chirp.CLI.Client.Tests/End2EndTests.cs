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
    public void Test_Read_And_Write_Cheep_1_Cheep() //E2E test of read --limit 1 and cheep
    {
        // Arrange
        using (var process = new Process())
        {
            process.StartInfo.FileName = dotNetPath();
            process.StartInfo.Arguments = "bin/Debug/net7.0/Chirp.CLI.dll cheep \"this is a cheep for endtoend test\""; //The cheep msg
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI.Client/";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            // Synchronously read the standard output of the spawned process.
            process.WaitForExit();
        }
        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = dotNetPath();
            process.StartInfo.Arguments = "bin/Debug/net7.0/Chirp.CLI.dll read --limit 1"; //The read command with limit 1
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
        Assert.EndsWith("this is a cheep for endtoend test" , output.Trim());
    }


    //Generate path for dotnetcore based on platform
    private string dotNetPath()
    {   
        // The feature of extracting the runtimeinformation is inspired by stackoverflow
        //https://stackoverflow.com/questions/38790802/determine-operating-system-in-net-core
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
