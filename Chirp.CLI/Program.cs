using System.IO;
using System;

//All references to "GPT" in comments are references to chat.opanai.com

string path = "../src/chirp_cli_db.csv"; //Path to CSV file 

if (args[0]=="read")
{   // Read part from: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
    try
        {
        // Open the text file using a stream reader.
        using var sr = new StreamReader(path);
        
        
        var line = sr.ReadLine();       //Read first line without printing

        //Using https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
        while (!sr.EndOfStream)
        {
            line = sr.ReadLine();       // Read each line
            var values = line.Split(',');
            var message = values[1];    
            long timeSeconds = long.Parse(values[^1])+7200; //Plus 7200 to adjust timezone 
            var timeStamp = DateTimeOffset.FromUnixTimeSeconds(timeSeconds).DateTime; //Convert to DateTime
            string formattedTimeStamp = timeStamp.ToString("dd/MM/yy HH:mm:ss"); //Format timeStamp - used GPT for this

            for (int i = 2; i < values.Length-1;i++) //Reading the message and adding possible commas
            {
                 message += "," + values[i];
            }    
            message = message.Trim('"');            //Trimming quotations
            Console.WriteLine(values[0] +" @ "+ formattedTimeStamp +": "+ message);    
        }      
    }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
}



if (args[0]=="cheep")
{
    long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; //Used GPT for this

    using (StreamWriter sw = File.AppendText(path))  
        {
            string message = args[1]; //Saving the message given as argument inside quotations
            sw.WriteLine(Environment.UserName + ",\"" + message + "\"," + unixTimestamp);
        }
}