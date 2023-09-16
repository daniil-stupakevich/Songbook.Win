using Songbook.Win.Core.Models.Domain;
using System;

namespace Songbook.Win.Core.Models.Atmosphere
{
    public class SongbookAtm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
        public bool Hidden { get; set; }
        public string AddedBy { get; set; }
        public object Description { get; set; }
        public int LanguageId { get; set; }
        public bool IsDraft { get; set; }
        public string Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Language Language { get; set; }
    }
}
