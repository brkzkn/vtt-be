using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VacationTracking.Data.IRepositories;

namespace VacationTracking.Data.Repositories
{
    public class HolidayTeamRepository : BaseRepository, IHolidayTeamRepository
    {
        public HolidayTeamRepository(IDbConnection connection)
            : base(connection)
        {
        }
    }
}
