using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Songbook.Win.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Songbook.Win.Persistent.Services
{
    public class BasePersistentService
    {
        private readonly string DatabaseName = "Songs";
        protected SqliteConnection Connection;
        protected string ConnectionString => $@"Data Source={DatabaseName};cache=shared";

        public BasePersistentService() 
        {
            CreateDatabaseIfNotExists();
            SQLitePCL.Batteries.Init();
        }

        private void CreateDatabaseIfNotExists() 
        {
            if (!File.Exists(DatabaseName)) 
            {
                string createDatabaseQuery = @"CREATE TABLE Languages (
                                                  Id   INTEGER      PRIMARY KEY AUTOINCREMENT,
                                                  Name VARCHAR (50),
                                                  Code VARCHAR (50) );

                                                CREATE TABLE Songbooks (
                                                  Id              INTEGER       PRIMARY KEY AUTOINCREMENT,
                                                  LanguageId      INTEGER       REFERENCES Languages (Id),
                                                  Name            VARCHAR (300),
                                                  Description     VARCHAR (50),
                                                  UpdatedLastTime DATETIME);

                                                CREATE TABLE Songs (
                                                  Id         INTEGER       PRIMARY KEY AUTOINCREMENT,
                                                  SongBookId INTEGER       REFERENCES Songbooks (Id) ON DELETE CASCADE,
                                                  Number     INTEGER,
                                                  Title      VARCHAR (300),
                                                  KeyChord   VARCHAR (15));
                                                CREATE TABLE Verses (
                                                  SongId     INTEGER      REFERENCES Songs (Id) ON DELETE CASCADE,
                                                  VerseType  VARCHAR (50),
                                                  VerseOrder INTEGER,
                                                  VerseText  TEXT);";
                using (var db = new SqliteConnection(ConnectionString))
                {
                    db.Execute(createDatabaseQuery);
                }

                string insertLanguageQuery = "INSERT INTO  Languages (Name, Code) VALUES (@Name, @Code)";
                using (var db = new SqliteConnection(ConnectionString))
                {
                    var languageList = JsonConvert.DeserializeObject<List<Language>>(File.ReadAllText("Default_Languages.json"));
                    foreach (var lang in languageList)
                    {
                        db.Execute(insertLanguageQuery, new { Name = lang.Name, Code = lang.Code });
                    }
                }
            }
        }
    }
}
