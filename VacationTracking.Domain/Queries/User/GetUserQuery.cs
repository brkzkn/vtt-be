using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Queries.User
{
    public class GetUserQuery : QueryBase<UserDto>
    {
        public GetUserQuery(int userId, int companyId)
        {
            UserId = userId;
            CompanyId = companyId;
        }

        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }
}
