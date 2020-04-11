using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using VacationTracking.Data.IRepositories;
using VacationTracking.Data.Repositories;

namespace VacationTracking.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed; 
        #endregion

        #region Private Repositories
        // Add new repository in resetRepositories function
        private TeamRepository _teamRepository;
        private TeamMemberRepository _teamMemberRepository;
        private HolidayRepository _holidayRepository;
        private LeaveTypeRepository _leaveTypeRepository;
        private VacationRepository _vacationRepository;
        #endregion

        #region Public Repositories
        public TeamRepository TeamRepository
        {
            get { return _teamRepository ?? (_teamRepository = new TeamRepository(_connection)); }
        }
        public TeamMemberRepository TeamMemberRepository
        {
            get { return _teamMemberRepository ?? (_teamMemberRepository = new TeamMemberRepository(_connection)); }
        }
        public HolidayRepository HolidayRepository
        {
            get { return _holidayRepository ?? (_holidayRepository = new HolidayRepository(_connection)); }
        }
        public LeaveTypeRepository LeaveTypeRepository
        {
            get { return _leaveTypeRepository ?? (_leaveTypeRepository= new LeaveTypeRepository(_connection)); }
        }
        public VacationRepository VacationRepository
        {
            get { return _vacationRepository ?? (_vacationRepository = new VacationRepository(_connection)); }
        }

        public NpgsqlConnection Connection 
        { 
            get { return (_connection as NpgsqlConnection); } 
        }

        #endregion

        #region Functions
        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }
        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                resetRepositories();
            }
        }
        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        private void resetRepositories()
        {
            _teamRepository = null;
            _teamMemberRepository = null;
        }
        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
