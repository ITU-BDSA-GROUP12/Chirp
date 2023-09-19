namespace SimpleDB;
using System.Globalization;
using CsvHelper;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    string path;
    public CSVDatabase(string path)
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
        using (CsvReader csv = new CsvReader(reader , CultureInfo.InvariantCulture))
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
        using (CsvWriter csv = new CsvWriter(sw , CultureInfo.InvariantCulture))
        {
            csv.NextRecord();
            csv.WriteRecord(record);
            
        }

    }
}
