using Dapper;
using Microsoft.Data.Sqlite;
using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Persistent.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Songbook.Win.Persistent.Services
{
    public class SongbookPersistentService : BasePersistentService, ISongbookPersistentService
    {
        public async Task<List<SongbookModel>> GetSongbooksAsync()
        {
            using (var db = new SqliteConnection(ConnectionString))
            {
                string getSongbooksQuery = "SELECT * FROM Songbooks";
                string getSongsQuery = "SELECT * FROM Songs";

                var songbooks = await db.QueryAsync<SongbookModel>(getSongbooksQuery);
                var songs = await db.QueryAsync<Song>(getSongsQuery);

                songbooks = songbooks.GroupJoin( songs, songbook => songbook.Id, song => song.SongbookId,
                    (songbook, songGroup) => 
                    {
                        songbook.Songs = songGroup.ToList();
                        return songbook;
                    });
                return songbooks.ToList();
            }
        }

        public async Task<Song> GetSongFromSongbookAsync(int songBookId, int songId)
        {
            var verseMap = new Dictionary<int, Song>();
            using (var db = new SqliteConnection(ConnectionString))
            {
                string query = @"SELECT s.*, v.* 
                                 FROM Songs s INNER JOIN Verses v ON v.SongID = s.Id 
                                 WHERE s.SongbookId=@SongbookId AND s.Id = @SongId";
                
                var result = await db.QueryAsync<Song, Verse, Song>(query, map: (song, verse) =>
                {
                    verse.SongId = song.Id;

                    if (verseMap.TryGetValue(song.Id, out Song existingSong))
                    {
                        song = existingSong;
                    }
                    else
                    {
                        song.Verses = new List<Verse>();
                        verseMap.Add(song.Id, song);
                    }

                    song.Verses.Add(verse);
                    return song;
                }, new { SongbookId = songBookId, SongId = songId }, splitOn: "SongId");

                return verseMap.Values.FirstOrDefault();
            }
        }

        public async Task<Song> GetSongByIdAsync(int songId) 
        {
            var verseMap = new Dictionary<int, Song>();
            using (var db = new SqliteConnection(ConnectionString))
            {
                string query = @"SELECT s.*, v.* 
                                 FROM Songs s INNER JOIN Verses v ON v.SongID = s.Id 
                                 WHERE s.Id = @SongId";

                var result = await db.QueryAsync<Song, Verse, Song>(query, map: (song, verse) =>
                {
                    verse.SongId = song.Id;

                    if (verseMap.TryGetValue(song.Id, out Song existingSong))
                    {
                        song = existingSong;
                    }
                    else
                    {
                        song.Verses = new List<Verse>();
                        verseMap.Add(song.Id, song);
                    }

                    song.Verses.Add(verse);
                    return song;
                }, new { SongId = songId }, splitOn: "SongId");

                return verseMap.Values.FirstOrDefault();
            }
        }

        public async Task DeleteSongbookAsync(int songbookId)
        {
            using (var db = new SqliteConnection(ConnectionString))
            {
                await db.ExecuteAsync("DELETE FROM Songs WHERE SongbookId = @SongbookId", new { SongbookId = songbookId });
                await db.ExecuteAsync("DELETE FROM Songbooks WHERE Id = @SongbookId", new { SongbookId = songbookId });
            }
        }

        public async Task<List<Song>> SearchSongAsync(string keyWord, int? songBookId = null)
        {
            var verseMap = new Dictionary<int, Song>();
            using (var db = new SqliteConnection(ConnectionString))
            {
                var whereQuery = songBookId.HasValue ? @"s.SongbookId=@SongbookId AND v.VerseText like @KeyWord" : @"v.VerseText like @KeyWord";

                string query = $@"SELECT s.*, v.* 
                                 FROM Songs s INNER JOIN Verses v ON v.SongID = s.Id 
                                 WHERE {whereQuery}";

                var result = await db.QueryAsync<Song, Verse, Song>(query, map: (song, verse) =>
                {
                    verse.SongId = song.Id;

                    if (verseMap.TryGetValue(song.Id, out Song existingSong))
                    {
                        song = existingSong;
                    }
                    else
                    {
                        song.Verses = new List<Verse>();
                        verseMap.Add(song.Id, song);
                    }

                    song.Verses.Add(verse); 
                    return song;
                }, new { SongbookId = songBookId, KeyWord = $"%{keyWord}%" }, splitOn: "SongId");

                return verseMap.Values.ToList();
            }
        }

        public async Task<int> SaveSongbookAsync(SongbookModel songbook)
        {
            using (var db = new SqliteConnection(ConnectionString))
            {
                db.Open();
                using (var transactionScope = db.BeginTransaction())
                {
                    string clearTables = @"DELETE FROM Songbooks WHERE Id = @Id;";
                    await db.ExecuteAsync(clearTables, new { Id = songbook.Id }, transaction: transactionScope);

                    string insertSongbookSql = @"INSERT INTO Songbooks (LanguageId, Name, Description, UpdatedLastTime)
                                             VALUES (@LanguageId, @Name, @Description, @UpdatedLastTime);
                                             SELECT last_insert_rowid() as Id;";

                    string insertSongSql = @"INSERT INTO Songs (SongbookId, Number, Title, KeyChord) 
                                         VALUES (@SongbookId, @Number, @Title, @KeyChord);
                                         SELECT last_insert_rowid() as Id;";

                    string insertVersesSql = @"INSERT INTO Verses (SongId, VerseType, VerseOrder, VerseText) 
                                           VALUES (@SongId, @VerseType, @VerseOrder, @VerseText)";

                    long songbookId = await db.ExecuteScalarAsync<long>(insertSongbookSql,
                        new
                        {
                            LanguageId = songbook.LanguageId,
                            Name = songbook.Name,
                            Description = songbook.Description ?? string.Empty,
                            UpdatedLastTime = songbook.UpdatedLastTime.ToString("yyyy-MM-dd HH:mm:ss")
                        });

                    foreach (var song in songbook.Songs)
                    {
                        long songId = await db.ExecuteScalarAsync<long>(insertSongSql,
                            new
                            {
                                SongbookId = songbookId,
                                Number = song.Number,
                                Title = song.Title,
                                KeyChord = song.KeyChord
                            }, transaction: transactionScope);

                        foreach (var verse in song.Verses)
                        {
                            await db.ExecuteAsync(insertVersesSql,
                                new
                                {
                                    SongId = songId,
                                    VerseType = verse.VerseType,
                                    VerseOrder = verse.VerseOrder,
                                    VerseText = verse.VerseText
                                }, commandType: System.Data.CommandType.Text, transaction: transactionScope);
                        }
                    }
                    transactionScope.Commit();
                }

                return await db.QueryFirstAsync<int>("SELECT Id FROM Songbooks WHERE Name = @Name", 
                    new { Name = songbook.Name});
            }
        }

        public async Task<SongbookModel> GetSongbookByIdAsync(int songbookId)
        {
            using (var db = new SqliteConnection(ConnectionString))
            {
                string getSongbooksQuery = "SELECT * FROM Songbooks WHERE Id = @Id";
                string getSongsQuery = "SELECT * FROM Songs WHERE SongbookId = @Id";

                var songbook = await db.QueryFirstAsync<SongbookModel>(getSongbooksQuery, new { Id = songbookId });
                var songs = await db.QueryAsync<Song>(getSongsQuery, new { Id = songbookId });
                songbook.Songs = songs.ToList();
                return songbook;
            }
        }
    }
}
