using AutoMapper;
using Newtonsoft.Json;
using Songbook.Win.Core.Models.Atmosphere;
using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Core.Profiles;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Songbook.Win.Core.Services.Import
{
    public class AtmposphereImportService
    {
        private HttpClient _httpClient = new HttpClient();
        private IMapper _mapper;

        public AtmposphereImportService()
        {
            var config = new MapperConfiguration(cfg => {
               cfg.AddProfile<AtmosphereToDomainProfile>();   
            });
            _mapper = config.CreateMapper();
        }

        public async Task<IEnumerable<Language>> GetLanguagesAsync() 
        {
            IEnumerable<Language> langs = null;
            var response = await _httpClient.GetAsync("https://api.atmosphereapp.ru/languages");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var langsAtm = JsonConvert.DeserializeObject<IEnumerable<LanguageAtm>>(content);
                langs = _mapper.Map<IEnumerable<Language>>(langsAtm);
            }

            return langs;
        }

        public async Task<IEnumerable<SongbookModel>> GetAvailiableSongBooksAsync() 
        {
            IEnumerable<SongbookModel> songbooks = null;
            var response = await _httpClient.GetAsync("https://api.atmosphereapp.ru/songbooks");
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                var songbooksAtm = JsonConvert.DeserializeObject<IEnumerable<SongbookAtm>>(content);
                songbooks = _mapper.Map<IEnumerable<SongbookModel>>(songbooksAtm);
            }



            return songbooks;
        }

        public async Task<IEnumerable<Song>> GetSongsFromSongBook(int songBookId)
        {
            IEnumerable<Song> songs = null;
            var response = await _httpClient.GetAsync($"https://api.atmosphereapp.ru/songs/web/songbook/{songBookId}");
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                var songsAtm = JsonConvert.DeserializeObject<IEnumerable<SongAtm>>(content);
                songs = _mapper.Map<IEnumerable<Song>>(songsAtm);
            }
            return songs;
        }

        public async Task<SongDetailsAtm> GetSongDetails(int songId)
        {
            SongDetailsAtm song = null;
            var response = await _httpClient.GetAsync($"https://api.atmosphereapp.ru/songs/{songId}");
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                song = JsonConvert.DeserializeObject<SongDetailsAtm>(content);
            }
            return song;
        }
    }
}
