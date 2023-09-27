using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using CsvHelper;

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
    string path = "./data/chirp_cli_db.csv";

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
    string path = "./data/chirp_cli_db.csv";
    using (StreamWriter sw = File.AppendText(path))
    using (CsvWriter csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
    {
        csv.NextRecord();
        csv.WriteRecord(record);
    }
}
public record Cheep(string Author , string Message , long Timestamp);

/*
public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    //Setup of singelton pattern
    private static CSVDatabase<T> instance = null;
    private static readonly object padlock = new object();
    string path;
    private CSVDatabase()
    {
        path = "./data/chirp_cli_db.csv";
    }

    //CSV Read part from: https://joshclose.github.io/CsvHelper/getting-started/
    //Reviewed by Lukas 
    public IEnumerable<T> Read(int? limit = null)
    {
    }
    //CSV Write part from: https://joshclose.github.io/CsvHelper/getting-started/


    //Singleton part from: https://csharpindepth.com/articles/singleton
    public static CSVDatabase<T> Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new CSVDatabase<T>();
                }
                return instance;
            }
        }
    }
    private void PrepareCsvFile(T record)
    {
        string record_string = record.ToString(); // record_string looks something like this: Record{ Property1=Value1, Property2=Value2 ... }
        string substring = record_string.Split('{')[1]; // we are only interested in the property names Property1 Property2 and so on
        string[] substrings = substring.Split(',');
        for (int i = 0 ; i < substrings.Length ; i++)
        {
            substrings[i] = substrings[i].Split('=')[0].Trim();
        }
        // substrings is now full of only the property string names
        using (StreamWriter sw = File.AppendText(path)) // this creates the .csv file
        {
            sw.Write(string.Join("," , substrings)); // this writes in the property names of the record like so: Property1,Property2,...
        }
        // the file is now ready to be written to
    }
    private void CreatePath()
    {
        path = path.Replace("\\" , "/");
        string[] path_items = path.Split("/");
        string possibly_existing_path = "";
        foreach (string path_item in path_items)
        {
            if (path_item.EndsWith(".csv")) // the path up to the missing file now exists - the missing file is created by the StreamWriter
            {
                break;
            }
            possibly_existing_path = possibly_existing_path + path_item + "/";
            if (!Path.Exists(possibly_existing_path))
            {
                Directory.CreateDirectory(possibly_existing_path); // a new folder has been added and possibly_existing_path now definitely exists
            }
        }
    }
}



*/