using Songbook.Win.Core.Models.Domain;

namespace Songbook.Win.Core.Services.Import.Interfaces
{
    public interface ISongOfGodImportService
    {
        SongbookModel ReadSongBookFromSOG(SongbookImportModel viewSongbook, string path);
    }
}
