using MediatR;
using Newtonsoft.Json;
using System;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Vacation
{
    public class CreateVacationCommand : IRequest<VacationDto>
    {
        [JsonConstructor]
        public CreateVacationCommand(int companyId, int userId, int leaveTypeId, DateTime startDate, DateTime endDate, string reason, bool isHalfDay)
        {
            CompanyId = companyId;
            UserId = userId;
            LeaveTypeId = leaveTypeId;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            IsHalfDay = isHalfDay;
        }

        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsHalfDay { get; set; }
    }
}
