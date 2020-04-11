using Npgsql;
using System;
using VacationTracking.Data.Repositories;

namespace VacationTracking.Data
{
    public interface IUnitOfWork : IDisposable
    {
        TeamRepository TeamRepository { get; }
        HolidayRepository HolidayRepository{ get; }
        LeaveTypeRepository LeaveTypeRepository { get; }
        TeamMemberRepository TeamMemberRepository { get; }
        VacationRepository VacationRepository { get; }
        NpgsqlConnection Connection { get; }

        void Begin();
        void Commit();
        void Rollback();
    }
}
