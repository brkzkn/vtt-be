using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using VacationTracking.Data;

namespace VacationTracking.Test.Context
{
    public class VacationTrackingDbContextFixture : IDisposable
    {
        private VacationTrackingContext _context;
        private DbConnection _connection;
        private DbContextOptions<VacationTrackingContext> _options;

        public void Initialize(bool useInMemory = true, Action seedData = null)
        {
            if (useInMemory)
            {
                // In-memory database only exists while the connection is open
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();
                _options = new DbContextOptionsBuilder<VacationTrackingContext>().UseSqlite(_connection).Options;
            }
            else
            {
                throw new NotImplementedException();
            }
            _context = new VacationTrackingContext(_options);
            _context.Database.EnsureCreated(); // If login error, manually create database
            seedData?.Invoke();
        }

        public VacationTrackingContext Context
        {
            get
            {
                if (_context == null)
                    throw new InvalidOperationException("You must first call Initialize before getting the context.");
                return _context;
            }
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
                _connection.Close();
        }
    }
}
