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
                    .ForMember(dst => dst.TeamId, opt => opt.MapFrom(src => src.TeamId))
                    .ForMember(dst => dst.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.TeamName))
                    .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                    .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                    .ForMember(dst => dst.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                    .ForMember(dst => dst.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                    .ForMember(dst => dst.TeamMembers, opt => opt.MapFrom(src => src.TeamMembers));

                cfg.CreateMap<Domain.Models.TeamMember, Domain.Dtos.TeamMemberDto>()
                .ForMember(dst => dst.TeamId, opt => opt.MapFrom(src => src.TeamId))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.IsApprover, opt => opt.MapFrom(src => src.IsApprover))
                .ForMember(dst => dst.IsMember, opt => opt.MapFrom(src => src.IsMember))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User));

                //cfg.CreateMap<Domain.Models.User, Domain.Dtos.UserDto>()
                //.ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                //.ForMember(dst => dst.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                //.ForMember(dst => dst.FullName, opt => opt.MapFrom(src => src.FullName))
                //.ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                //.ForMember(dst => dst.EmployeeSince, opt => opt.MapFrom(src => src.EmployeeSince))
                //.ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status))
                //.ForMember(dst => dst.AccountType, opt => opt.MapFrom(src => src.AccountType))
                //.ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                //.ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                //.ForMember(dst => dst.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                //.ForMember(dst => dst.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
            });

            _mapper = config.CreateMapper();
        }

        public TeamDto MapTeamDto(Team team)
        {
            return _mapper.Map<Domain.Models.Team, Domain.Dtos.TeamDto>(team);
        }

        public TeamMemberDto MapTeamDto(TeamMember teamMember)
        {
            return _mapper.Map<Domain.Models.TeamMember, Domain.Dtos.TeamMemberDto>(teamMember);
        }
    }
}
