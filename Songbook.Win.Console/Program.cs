using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Lib;
using Songbook.Win.Lib.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Songbook.Win.Console
{
    public class Program
    {
        private static readonly string _songbookFile = @"E:\Song Of God 1.4 release without images\assets\psalms\Благословлю, Господь, Тебя.sog";

        static async Task Main(string[] args)
        {
            ISongbookService _sogService = new SongbookService();
            var languages = await _sogService.GetSongbookLanguagesAsync();
            var songbook = await _sogService.ImportSongBookFromFileAsync(_songbookFile, new SongbookImportModel 
            {
              Name = "Благославлю, Господь Тебя.",
              Description = "Московский сборник",
              LanguageId = languages.FirstOrDefault(x => x.Code == "ru").Id
            });
        }
    }
}
