using System;
using AutoMapper;
using ShowMustNotGoOn.Core.Model;
using ShowMustNotGoOn.MyShowsService.Model;

namespace ShowMustNotGoOn.MyShowsService
{
    public class MyShowsServiceMappingProfile : Profile
    {
        public MyShowsServiceMappingProfile()
        {
            CreateMap<Result, TvShowDescription>()
                .ForMember(dest => dest.MyShowsId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<Result, TvShowInfo>()
                .ForMember(dest => dest.MyShowsId,
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}
