using System;

namespace VacationTracking.Domain.Commands.Vacation
{
    public interface IVacationCommand
    {
        int CompanyId { get; }
        int UserId { get; }
        int LeaveTypeId { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        string Reason { get; }
        bool IsHalfDay { get; }
    }
}