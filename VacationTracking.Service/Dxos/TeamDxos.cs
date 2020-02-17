using AutoMapper;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Models;

namespace VacationTracking.Service.Dxos
{
    public class TeamDxos : ITeamDxos
    {
        private readonly IMapper _mapper;

        public TeamDxos()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Models.Team, Domain.Dtos.TeamDto>()
                    //.ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));
            });

            _mapper = config.CreateMapper();
        }

        public TeamDto MapCustomerDto(Team team)
        {
            return _mapper.Map<Domain.Models.Team, Domain.Dtos.TeamDto>(team);
        }
    }
}
