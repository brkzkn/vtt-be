using Npgsql;
using System;
using VacationTracking.Data.IRepositories;
using VacationTracking.Data.Repositories;

namespace VacationTracking.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ITeamRepository TeamRepository { get; }
        IHolidayRepository HolidayRepository{ get; }
        ILeaveTypeRepository LeaveTypeRepository { get; }
        ITeamMemberRepository TeamMemberRepository { get; }
        IVacationRepository VacationRepository { get; }
        NpgsqlConnection Connection { get; }

        void Begin();
        void Commit();
        void Rollback();
    }
}
