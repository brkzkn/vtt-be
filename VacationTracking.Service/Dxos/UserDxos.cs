using AutoMapper;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Models;

namespace VacationTracking.Service.Dxos
{
    public class UserDxos : IUserDxos
    {
        private readonly IMapper _mapper;

        public UserDxos()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Models.User, Domain.Dtos.UserDto>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dst => dst.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dst => dst.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.EmployeeSince, opt => opt.MapFrom(src => src.EmployeeSince))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dst => dst.AccountType, opt => opt.MapFrom(src => src.AccountType))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dst => dst.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
            });
            
            _mapper = config.CreateMapper();
        }

        public UserDto MapUserDto(User user)
        {
            return _mapper.Map<Domain.Models.User, Domain.Dtos.UserDto>(user);
        }
    }
}
