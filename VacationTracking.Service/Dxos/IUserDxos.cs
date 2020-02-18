using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Dxos
{
    public interface IUserDxos
    {
        UserDto MapUserDto(Domain.Models.User user);
    }
}
