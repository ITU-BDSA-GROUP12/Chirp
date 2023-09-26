namespace SimpleDB;

using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using CsvHelper;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    //Setup of singelton pattern
    private static CSVDatabase<T> instance = null;
    private static readonly object padlock = new object();
    string path;
    private CSVDatabase()
    {
        path = "/.data/chirp_cli_db.csv";
    }

    //CSV Read part from: https://joshclose.github.io/CsvHelper/getting-started/
    //Reviewed by Lukas 
    public IEnumerable<T> Read(int? limit = null)
    {
        if (limit <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(limit)} must be greater than 0");
        }
        if (Path.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<T> all_records = csv.GetRecords<T>().ToList();
                foreach (T record in all_records.Reverse<T>())
                {
                    yield return record;
                    limit = limit == null ? null : limit - 1;
                    if (limit == 0) { break; }
                }
            }
        }
        else
        {
            Console.WriteLine($"There are no cheeps to read from here: {path}\nPlease specify a different path, or simply begin cheeping to create some cheeps!");
        }
    }
    //CSV Write part from: https://joshclose.github.io/CsvHelper/getting-started/
    public void Store(T record)
    {
        if (!Path.Exists(path))
        {
            CreatePath();
            PrepareCsvFile(record);
        }
        using (StreamWriter sw = File.AppendText(path))
        using (CsvWriter csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
        {
            csv.NextRecord();
            csv.WriteRecord(record);
        }

    }

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

