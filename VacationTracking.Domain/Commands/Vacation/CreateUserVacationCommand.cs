using MediatR;
using Newtonsoft.Json;
using System;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Vacation
{
    public class CreateUserVacationCommand : IRequest<VacationDto>
    {
        [JsonConstructor]
        public CreateUserVacationCommand(int companyId, int userId, int leaveTypeId, DateTime startDate, DateTime endDate, string reason)
        {
            CompanyId = companyId;
            UserId = userId;
            LeaveTypeId = leaveTypeId;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
        }

        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}
