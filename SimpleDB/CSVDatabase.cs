namespace SimpleDB;
using System.Globalization;
using CsvHelper;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    string path;
    public CSVDatabase(string path)
    {
        this.path = path;
    }     
    public List<T> Read(int? limit = null)
    {
        using (StreamReader reader = new StreamReader(path))
        using (CsvReader csv = new CsvReader(reader , CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList(); // Reading cheeps from CSV File
        }
    }
    public void Store(T record)
    {
        using (StreamWriter sw = File.AppendText(path))  
        using (CsvWriter csv = new CsvWriter(sw , CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(record);
        }

    }
}
