using MediatR;
using System;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Domain.Commands.Vacation
{
    public class CreateUserVacationCommand : IRequest<VacationDto>
    {
        public CreateUserVacationCommand(int companyId,
                                         int userId,
                                         int leaveTypeId,
                                         DateTime startDate,
                                         DateTime endDate,
                                         string reason)
        {
            CompanyId = companyId;
            UserId = userId;
            LeaveTypeId = leaveTypeId;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
        }

        public int CompanyId { get; }
        public int UserId { get; }
        public int LeaveTypeId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string Reason { get; }
    }
}
