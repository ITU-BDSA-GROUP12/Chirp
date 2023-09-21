namespace SimpleDB;
using System.Globalization;
using CsvHelper;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private static CSVDatabase<T> instance = null;
    private static readonly object padlock = new object();
    string path;
    private CSVDatabase(string path = "../src/chirp_cli_db.csv")
    {
        this.path = path;
    }

    //CSV Read part from: https://joshclose.github.io/CsvHelper/getting-started/
    public IEnumerable<T> Read(int? limit = null)
    {
        if (limit <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(limit)} must be greater than 0");
        }
        using (StreamReader reader = new StreamReader(path))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            foreach (T record in csv.GetRecords<T>())
            {
                yield return record;
                limit = limit == null ? null : limit - 1;
                if (limit == 0) { break; }
            }
        }
    }
    //CSV Write part from: https://joshclose.github.io/CsvHelper/getting-started/
    public void Store(T record)
    {
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
}

