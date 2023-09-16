using AutoMapper;
using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Core.Services.Import;
using Songbook.Win.Lib.Interfaces;
using Songbook.Win.Lib.Models.Enums;
using Songbook.Win.Lib.Profiles;
using Songbook.Win.Persistent.Services;
using Songbook.Win.Persistent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songbook.Win.Lib
{
    public class SongbookService : ISongbookService
    {
        private readonly ISongbookPersistentService _songbookPersistentService;
        private readonly IConfigurationPersistentService _configurationPersistentService;
        private readonly IMapper _mapper;

        public SongbookService()
        {
            _songbookPersistentService = new SongbookPersistentService();
            _configurationPersistentService = new ConfigurationPersistentService();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<SongbookMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        public async Task<List<SongbookModel>> GetSongbooksAsync()
        {
            return await _songbookPersistentService.GetSongbooksAsync();
        }

        public async Task<Song> GetSongFromSongbookAsync(int songbookId, int songId)
        {
            return await _songbookPersistentService.GetSongFromSongbookAsync(songbookId, songId);
        }

        public async Task<SongbookModel> ImportSongBookFromFileAsync(string filepath, SongbookImportModel songbookModel, ImportSongbookFormat format = ImportSongbookFormat.SongOfGod)
        {
            switch (format) 
            {
             case ImportSongbookFormat.SongOfGod:
                    var importService = new SongOfGodImportService();
                    var songbook = importService.ReadSongBookFromSOG(songbookModel, filepath);
                    var id = await _songbookPersistentService.SaveSongbookAsync(_mapper.Map<SongbookModel>(songbook));
                    return await _songbookPersistentService.GetSongbookByIdAsync(id);
                default:
                    throw new Exception("Unknown import format.");
            }
        }

        public async Task<List<Song>> SearchSongAsync(string keyWord)
        {
            return await _songbookPersistentService.SearchSongAsync(keyWord);
        }

        public async Task<IEnumerable<Language>> GetSongbookLanguagesAsync()
        {
            return await _configurationPersistentService.GetLanguagesAsync();
        }

        public async Task<Song> GetSongById(int songId)
        {
            return await _songbookPersistentService.GetSongByIdAsync(songId);
        }
    }
}
