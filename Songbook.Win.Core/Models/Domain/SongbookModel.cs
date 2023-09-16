using System;
using System.Collections.Generic;

namespace Songbook.Win.Core.Models.Domain
{
    public class SongbookModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Song> Songs { get; set; }
        public DateTime UpdatedLastTime { get; set; }
    }
}