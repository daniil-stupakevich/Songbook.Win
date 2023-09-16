using Songbook.Win.Core.Services.Import;
using Songbook.Win.Persistent.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Songbook.Win.Console
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            SongOfGodImportService sog = new SongOfGodImportService();
            SongbookPersistentService songbookPersistentService = new SongbookPersistentService();
            ConfigurationPersistentService confPersistentService = new ConfigurationPersistentService();
            AtmposphereImportService atmImportService = new AtmposphereImportService();

            var langs = await atmImportService.GetLanguagesAsync();
            await confPersistentService.SaveLanguageCollection(langs.ToList());
            var langsDb = await confPersistentService.GetLanguages();

            var songbooks = await atmImportService.GetAvailiableSongBooksAsync();
            var songs = await atmImportService.GetSongsFromSongBook(songbooks.FirstOrDefault(x => x.Id == 2).Id);
            var songDetail = await atmImportService.GetSongDetails(songs.FirstOrDefault().Id);

            /*
            var langs = await confPersistentService.GetLanguages();

            var songbook = sog.ReadSongBookFromSOG(@"e:\Song Of God 1.4 release without images\assets\psalms\Благословлю, Господь, Тебя.sog");
            songbook.Name = "Vilnus";
            songbook.LanguageId = langs.FirstOrDefault().Id;
            songbook.UpdatedLastTime = DateTime.UtcNow;

            var insertedSongBook = await songbookPersistentService.SaveSongBook(songbook);

            var songbooks = await songbookPersistentService.GetSongBooks();
            var song = songbooks.FirstOrDefault().Songs.FirstOrDefault(x => x.Number == 441);
            var songFromDb = await songbookPersistentService.GetSongFromSongbook(song.SongbookId, song.Id);
            var songs = await songbookPersistentService.SearchSong("елей");
            */
        }
    }
}
