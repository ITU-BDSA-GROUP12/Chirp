using System.IO;
using System;

var shouldRun = true;

while (shouldRun)
{


}

if (args[0]=="read")
{   // Read part from: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
    try
        {
            // Open the text file using a stream reader.
            using (var sr = new StreamReader(@"..\src\chirp_cli_db.csv"))
            {
                // Read the stream as a string, and write the string to the console.
                Console.WriteLine(sr.ReadToEnd());
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }

        //Console.WriteLine(DateTimeOffset())


}

if (args[0]=="write")
{
    //WRITE CHEEP
}



// using System.Threading.Tasks.Dataflow;


// if (args.Length < 3 || args[0] != "say")
// {
//     Console.WriteLine("Please enter a command in the form: say <message> <amount>");
//     return;
// }
// if (args[0] == "say" && args.Length == 3)
// {
//     var amount = int.Parse(args[2]);
//     var random = new Random();

//     for (int i = 0; i < amount; i++)
//     {
//         Console.WriteLine(args[1]);
//         var wait = random.Next(100, 1000);
//         await Task.Delay(TimeSpan.FromMilliseconds(wait));
//     }
// }