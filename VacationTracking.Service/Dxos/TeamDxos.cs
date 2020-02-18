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
                cfg.CreateMap<Domain.Models.Teams, Domain.Dtos.TeamDto>()
                    .ForMember(dst => dst.TeamId, opt => opt.MapFrom(src => src.TeamId))
                    .ForMember(dst => dst.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.TeamName))
                    .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                    .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                    .ForMember(dst => dst.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                    .ForMember(dst => dst.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
            });

            _mapper = config.CreateMapper();
        }

        public TeamDto MapCustomerDto(Teams team)
        {
            return _mapper.Map<Domain.Models.Teams, Domain.Dtos.TeamDto>(team);
        }
    }
}
