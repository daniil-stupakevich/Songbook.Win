using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Core.Models.Domain.Enum;
using Songbook.Win.Core.Services.Import.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Songbook.Win.Core.Services.Import
{
    public class SongOfGodImportService : ISongOfGodImportService
    {
        public SongbookModel ReadSongBookFromSOG(SongbookImportModel viewSongbook, string path)
        {
            SongbookModel result = new SongbookModel()
            {
                Name = viewSongbook.Name,
                Description = viewSongbook.Description,
                LanguageId = viewSongbook.LanguageId,
                Songs = new List<Song>()
            };

            string text = File.ReadAllText(path, Encoding.UTF8);
            var songs = text.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var parsedSong in songs)
            {
                var verses = parsedSong.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                Song song = new Song()
                {
                    Verses = new List<Verse>()
                };

                int verseOrder = 0;
                for (int i = 0; i < verses.Length; i++)
                {
                        if (i == 0)
                        {
                            song.Number = int.TryParse(verses[i], out _) ? int.Parse(verses[i]) : 0;
                            continue;
                        }

                        if (i == 1)
                        {
                            song.Title = verses[i];
                            song.KeyChord = GetKeyChordFromString(verses[i]);
                            continue;
                        }

                    song.Verses.Add(new Verse
                    {
                        VerseOrder = ++verseOrder,
                        VerseType = verses[i].StartsWith("*") ? VerseType.Chorus : VerseType.Verse,
                        VerseText = verses[i]
                    });
                }

                song.Verses = song.Verses.GroupBy(p => p.VerseText).Select(g => g.First()).ToList();
                result.Songs.Add(song);
            }

            return result;
        }

        private string GetKeyChordFromString(string text) 
        {
            // find the index of the first occurrence of char1 in text
            int start = text.IndexOf('(');
            // if char1 is not found, return an empty string
            if (start == -1)
            {
                return "";
            }
            // find the index of the first occurrence of char2 after start in text
            int end = text.IndexOf(')', start + 1);
            // if char2 is not found, return an empty string
            if (end == -1)
            {
                return "";
            }
            // return the substring between start and end, excluding char1 and char2
            return text.Substring(start + 1, end - start - 1).Trim();
        }
    }
}
