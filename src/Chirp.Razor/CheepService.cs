using Microsoft.Data.Sqlite;
﻿using System.Data;
using System.IO;
using System.Reflection;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(int page, string author);
}

public class CheepService : ICheepService
{

    public List<CheepViewModel> GetCheeps(int page)
    {
        return LoadLocalSqlite(page);
    }


    private List<CheepViewModel> LoadLocalSqlite(int page, string? author = null)
    {
        var sqlDBFilePath = SeedingDBfileDir();
        CreateFileIfMissing(sqlDBFilePath);
        bool db_initialised = File.ReadAllText(sqlDBFilePath).Trim() != "";
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();
            if (!db_initialised)
            {
                InitialiseDb(connection, sqlDBFilePath);
            }
            SqliteCommand command = CreateQueryCommand(connection, page, author );
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

    void CreateFileIfMissing(string path)
    {
        if (Path.Exists(path)) return; // if the file already exists, do nothing
        path = path.Replace("\\", "/"); // ensure all folders are seperated by / and never \
        string[] path_items = path.Split('/'); // path_items now contains the individual folder names
        string possibly_existing_path = ""; // the path will be built on here, as soon as a missing folder is added to the path it will be created
        foreach (string path_item in path_items)
        {
            possibly_existing_path += "/" + path_item;
            if (path_item.EndsWith(".db")) // the required folders are now in place - create the file

            {
                File.WriteAllText(possibly_existing_path, "");
            }
            else if (!Directory.Exists(possibly_existing_path)) // the latest folder saved in path_item does not exist!
            {
                Directory.CreateDirectory(possibly_existing_path); // create it
            }
        }
    }
    
    private void InitialiseDb(SqliteConnection connection , string sqlDBFilePath)
    {

        SqliteCommand schema_creation_command = connection.CreateCommand();
        schema_creation_command.CommandText = ReadResource("Chirp.Razor.data.schema.sql") + ReadResource("Chirp.Razor.data.dump.sql"); // load in the schema and dump a bunch of data in the db
        schema_creation_command.ExecuteNonQuery();
    }
    

    SqliteCommand CreateQueryCommand(SqliteConnection connection ,int page , string? author)
    {
        // if an author is provided, retrieve only entries with username author - if not, retrieve all entries
        SqliteCommand queryCommand = connection.CreateCommand();
        if (author == null)
        {
            string sqlQuery = // this SQL code fetches every message sent from the database
            @"SELECT u.username as username , m.text as message, m.pub_date as date FROM 
            message m JOIN user u ON u.user_id = m.author_id
            ORDER by m.pub_date desc
            LIMIT 32 OFFSET @pageoffset"; //https://www.sqlitetutorial.net/sqlite-limit/
            queryCommand.CommandText = sqlQuery;
            queryCommand.Parameters.AddWithValue("@pageoffset" , (page - 1)*32);
        }
        else
        {
            string sqlQuery = // this SQL code fetches every message sent by ´´author´´
            @"SELECT u.username AS username , m.text AS message, m.pub_date AS date 
            FROM message m JOIN user u ON u.user_id = m.author_id
            WHERE u.username = @author 
            ORDER BY m.pub_date desc";
            queryCommand.CommandText = sqlQuery;
            queryCommand.Parameters.AddWithValue("@author", author);
            queryCommand.Parameters.AddWithValue("@pageoffset" , (page - 1)*32);
        }
        return queryCommand;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(int page, string author)
    {
        return LoadLocalSqlite(page, author);

    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    private static string SeedingDBfileDir (){ // Retreeves the the given file from the EnvironmentVariable, else creates a path to the Tmp folder
        string chirpDBpath;                     // https://learn.microsoft.com/en-us/dotnet/api/system.environment.setenvironmentvariable?view=net-7.0
        if(Environment.GetEnvironmentVariable("CHIRPDBPATH") == null){
            chirpDBpath = Path.GetTempPath() + "chirp.db";
        } else {
            chirpDBpath = Environment.GetEnvironmentVariable("CHIRPDBPATH");
        }
        return chirpDBpath;
    }

    //This mehtod of reading embedded resources is inspired by:
    // https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
    // chat.openi.com
    // and the two links to documentation:
    public string ReadResource(string name) {
    // Determine path
    var assembly = Assembly.GetExecutingAssembly(); //https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getexecutingassembly?view=net-7.0
    string resourcePath = name;

        if (resourcePath != null)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath)) //https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getmanifestresourcestream?view=net-7.0#system-reflection-assembly-getmanifestresourcestream(system-string)
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        else {
            return "";
        }
    }

}
