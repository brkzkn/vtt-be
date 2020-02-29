using AutoMapper;

namespace VacationTracking.Service.Mapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Domain.Models.User, Domain.Dtos.UserDto>();
            CreateMap<Domain.Models.TeamMember, Domain.Dtos.TeamMemberDto>();
            CreateMap<Domain.Models.Team, Domain.Dtos.TeamDto>();
            CreateMap<Domain.Models.Holiday, Domain.Dtos.HolidayDto>();
        }
    }
}
