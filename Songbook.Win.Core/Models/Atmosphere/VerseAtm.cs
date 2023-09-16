using System;

namespace Songbook.Win.Core.Models.Atmosphere
{
    public class VerseAtm
    {
        public int Id { get; set; }
        public int? VerseOrder { get; set; }
        public object OrderToShow { get; set; }
        public bool IsRefrain { get; set; }
        public string Line { get; set; }
        public int SongId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
