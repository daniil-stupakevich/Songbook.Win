using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Lib.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songbook.Win.Lib.Interfaces
{
    public interface ISongbookService
    {
        Task<IEnumerable<Language>> GetSongbookLanguagesAsync();
        Task<List<SongbookModel>> GetSongbooksAsync();
        Task<Song> GetSongFromSongbookAsync(int songbookId, int songId);
        Task<Song> GetSongById(int songId);
        Task<List<Song>> SearchSongAsync(string keyWord);
        Task<SongbookModel> ImportSongBookFromFileAsync(string filepath, SongbookImportModel songbookModel, ImportSongbookFormat format = ImportSongbookFormat.SongOfGod);
    }
}
