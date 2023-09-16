using Songbook.Win.Core.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songbook.Win.Persistent.Services.Interfaces
{
    public interface ISongbookPersistentService
    {
        Task<List<SongbookModel>> GetSongbooksAsync();
        Task<SongbookModel> GetSongbookByIdAsync(int songbookId);
        Task<Song> GetSongFromSongbookAsync(int songBookId, int songId);
        Task<Song> GetSongByIdAsync(int songId);
        Task DeleteSongbookAsync(int songbookId);
        Task<List<Song>> SearchSongAsync(string keyWord, int? songBookId = null);
        Task<int> SaveSongbookAsync(SongbookModel songbook);        
    }
}
