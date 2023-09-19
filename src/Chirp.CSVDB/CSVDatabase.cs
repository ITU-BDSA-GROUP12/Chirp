namespace SimpleDB;
using System.Globalization;
using CsvHelper;
using System;
using System.Linq;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    string path;
    public CSVDatabase(string path)
    {
        this.path = path;
    }     

    // CSV Read part from: https://joshclose.github.io/CsvHelper/getting-started/
    public IEnumerable<T> Read(int? limit = null)
    {
        if (limit <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(limit)} must be greater than 0");
        }
        using (StreamReader reader = new StreamReader(path))
        using (CsvReader csv = new CsvReader(reader , CultureInfo.InvariantCulture))
        {
            List<T> records = csv.GetRecords<T>().ToList(); // from https://stackoverflow.com/questions/7617771/converting-from-ienumerable-to-list
            int nTotalRecords = records.Count();
            int? nRecordsUntilYielded = nTotalRecords - limit;
            foreach (T record in records)
            {
                if (nRecordsUntilYielded > 0)
                {
                    nRecordsUntilYielded -= 1;
                    continue;
                }
                yield return record;
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
