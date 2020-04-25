using MediatR;
using Newtonsoft.Json;
using System;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Vacation
{
    public class CreateVacationCommand : IRequest<VacationDto>
    {
        [JsonConstructor]
        public CreateVacationCommand(Guid companyId, Guid userId, Guid leaveTypeId, DateTime startDate, DateTime endDate, string reason, bool isHalfDay)
        {
            CompanyId = companyId;
            UserId = userId;
            LeaveTypeId = leaveTypeId;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            IsHalfDay = isHalfDay;
        }

        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public Guid LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsHalfDay { get; set; }
    }
}
