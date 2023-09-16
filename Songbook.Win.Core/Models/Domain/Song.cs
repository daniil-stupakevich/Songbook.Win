using System.Collections.Generic;

namespace Songbook.Win.Core.Models.Domain
{
    public class Song
    {
        public int Id { get; set; }
        public int SongbookId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string KeyChord { get; set; }
        public string ImagePath { get; set; }
        public List<Verse> Verses { get; set; }      
    }
}
