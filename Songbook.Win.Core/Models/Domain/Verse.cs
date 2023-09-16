using Songbook.Win.Core.Models.Domain.Enum;

namespace Songbook.Win.Core.Models.Domain
{
    public class Verse
    {
        public int SongId { get; set; }
        public VerseType VerseType { get; set; }
        public int VerseOrder{ get; set; }
        public string VerseText { get; set; }
    }
}
