using Newtonsoft.Json;
using Songbook.Win.Core.Models.Domain;
using System.Collections.Generic;

namespace Songbook.Win.Core.Models.Atmosphere
{
    public class SongAtm
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public bool Audio { get; set; }
        public bool Chord { get; set; }
        public object Code { get; set; }
        public object ChordUrl { get; set; }
        public string Name { get; set; }
        public object Description { get; set; }
        public string MainChord { get; set; }
        public string AudioUrl { get; set; }

        [JsonProperty("songBook")]
        public SongbookAtm SongBook { get; set; }
        public object Author { get; set; }
        public Language Language { get; set; }
        public List<Verse> Verses { get; set; }
    }
}
