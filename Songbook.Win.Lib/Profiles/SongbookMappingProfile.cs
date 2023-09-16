using AutoMapper;
using Songbook.Win.Core.Models.Atmosphere;
using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Lib.Models;

namespace Songbook.Win.Lib.Profiles
{
    public class SongbookMappingProfile : Profile
    {
        public SongbookMappingProfile()
        {
            CreateMap<SongbookImportModel, SongbookModel>().ReverseMap();
        }
    }
}
