namespace SimpleDB;
using CsvHelper;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    string path;
    public CSVDatabase(string path)
    {
        this.path = path;
    }     
    public IEnumerable<T> Read(int? limit = null)
    {
        using (StreamReader reader = new StreamReader(path))
        using (CsvReader csv = new CsvReader(reader,CsvHelper.CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>(); // Reading cheeps from CSV File
        }
    }
    public void Store(T record)
    {

    }
}
