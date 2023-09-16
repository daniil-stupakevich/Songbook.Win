using AutoMapper;
using Songbook.Win.Core.Models.Atmosphere;
using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Core.Models.Domain.Enum;

namespace Songbook.Win.Core.Profiles
{
    public class AtmosphereToDomainProfile : Profile
    {
        public AtmosphereToDomainProfile()
        {
            CreateMap<SongbookAtm, SongbookModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Songs, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedLastTime, opt => opt.MapFrom(src => src.UpdatedAt));

             CreateMap<SongAtm, Song>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SongbookId, opt => opt.MapFrom(src => src.SongBook.Id))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => int.Parse(src.Number)))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.KeyChord, opt => opt.MapFrom(src => src.MainChord))
                .ForMember(dest => dest.Verses, opt => opt.MapFrom(src => src.Verses));

            CreateMap<LanguageAtm, Language>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<VerseAtm, Verse>()
                .ForMember(dest => dest.SongId, opt => opt.MapFrom(src => src.SongId))
                .ForMember(dest => dest.VerseType, opt => opt.MapFrom(src => src.IsRefrain ? VerseType.Chorus : VerseType.Verse))
                .ForMember(dest => dest.VerseOrder, opt => opt.MapFrom(src => src.VerseOrder ?? 0))
                .ForMember(dest => dest.VerseText, opt => opt.MapFrom(src => src.Line));
        }
    }}
