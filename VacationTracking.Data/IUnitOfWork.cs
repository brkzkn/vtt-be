using Npgsql;
using System;
using VacationTracking.Data.Repositories;

namespace VacationTracking.Data
{
    public interface IUnitOfWork : IDisposable
    {
        TeamRepository TeamRepository { get; }
        HolidayRepository HolidayRepository{ get; }
        TeamMemberRepository TeamMemberRepository { get; }
        NpgsqlConnection Connection { get; }

        void Begin();
        void Commit();
        void Rollback();
    }
}
