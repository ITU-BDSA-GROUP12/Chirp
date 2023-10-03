using Microsoft.Data.Sqlite;
﻿using System.Data;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{

    public List<CheepViewModel> GetCheeps()
    {
        return LoadLocalSqlite();
    }

    private List<CheepViewModel> LoadLocalSqlite()
    {
        
        var sqlDBFilePath = "/tmp/chirp.db";
        var sqlQuery = 
        @"SELECT u.username as username , m.text as message, m.pub_date as date FROM 
        message m JOIN user u ON u.user_id = m.author_id
        ORDER by m.pub_date desc";
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                cheeps.Add(new CheepViewModel(
                    reader.GetString(reader.GetOrdinal("username")) , 
                    reader.GetString(reader.GetOrdinal("message")) , 
                    UnixTimeStampToDateTimeString(reader.GetInt32(reader.GetOrdinal("date")))
                    ));
            }
        }
        return cheeps;
    }  
    
    private List<CheepViewModel> LoadLocalSqlite(string author)
    {
        var sqlDBFilePath = "/tmp/chirp.db";
        var sqlQuery = 
        @"SELECT u.username as username , m.text as message, m.pub_date as date 
        FROM message m JOIN user u ON u.user_id = m.author_id
        WHERE u.username = @author ORDER by m.pub_date desc";
        List<CheepViewModel> cheeps = new List<CheepViewModel>(); 
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@author" , author);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                cheeps.Add(new CheepViewModel(
                    reader.GetString(reader.GetOrdinal("username")) , 
                    reader.GetString(reader.GetOrdinal("message")) , 
                    UnixTimeStampToDateTimeString(reader.GetInt32(reader.GetOrdinal("date")))
                    ));
            }
        }
        return cheeps;
    }
    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return LoadLocalSqlite(author);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
