using AutoMapper;
using WebAPI_Revision.Entities;
using WebAPI_Revision.DTOs.TeamDtos;
using WebAPI_Revision.DTOs.PlayerDtos;

namespace WebAPI_Revision.AutoMapperMappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Team Mapping
            CreateMap<Team, TeamGetDto>();
            CreateMap<TeamPostDto, Team>();
            CreateMap<TeamUpdateDto, Team>();
            CreateMap<Team, TeamWithPlayersGetDto>();

            // Player Mapping
            CreateMap<PlayerPostDto, Player>();
            CreateMap<Player, PlayerGetDto2>();
            CreateMap<Player, PlayerGetDto>();
            CreateMap<Player, PlayerGetDtoInDetails>();
        }
    }
}
