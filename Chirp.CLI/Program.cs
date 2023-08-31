foreach (var arg in args)
    Console.WriteLine(arg);

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