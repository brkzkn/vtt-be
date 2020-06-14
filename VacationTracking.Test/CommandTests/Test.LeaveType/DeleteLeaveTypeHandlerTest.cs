using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.LeaveType;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Commands.LeaveType;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class DeleteLeaveTypeHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<DeleteLeaveTypeHandler> _logger;
        private readonly IMapper _mapper;
        public DeleteLeaveTypeHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<DeleteLeaveTypeHandler>();

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(Service.Mapper.AutoMapping));
            });
            _mapper = mockMapper.CreateMapper();

            _fixture.Initialize(true, () =>
            {
                _fixture.Context.Companies.AddRange(Seed.Companies());
                _fixture.Context.Users.AddRange(Seed.Users());
                _fixture.Context.Teams.AddRange(Seed.Teams());
                _fixture.Context.TeamMembers.AddRange(Seed.TeamMembers());
                _fixture.Context.SaveChanges();
            });
        }

        /// Test.1: LeaveType'ın başarıyla silinmesi (IsDeleted: false olacak)
        [Fact]
        public async Task Should_DeleteLeaveType_When_PassValidLeaveTypeId()
        {
            // Arrange
            var entity = new LeaveType()
            {
                ColorCode = "ffffff",
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                DefaultDaysPerYear = 10,
                IsActive = true,
                IsAllowNegativeBalance = true,
                IsApproverRequired = false,
                IsDefault = false,
                IsDeleted = false,
                IsHalfDaysActivated = true,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Name"
            };
            _fixture.Context.LeaveTypes.Add(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new DeleteLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteLeaveTypeCommand(companyId: 1, leaveTypeId: 1);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);
            var leaveType = _fixture.Context.LeaveTypes.SingleOrDefault(x => x.LeaveTypeId == 1);
            // Assert
            Assert.True(result);
            Assert.True(leaveType.IsDeleted);
        }

        /// Test.2: Başka bir firmaya ait leaveTypeId gönderilmesi
        [Fact]
        public async Task Should_DeleteLeaveType_When_PassInvalidCompanyId()
        {
            // Arrange
            var entity = new LeaveType()
            {
                ColorCode = "ffffff",
                CompanyId = 2,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                DefaultDaysPerYear = 10,
                IsActive = true,
                IsAllowNegativeBalance = true,
                IsApproverRequired = false,
                IsDefault = false,
                IsDeleted = false,
                IsHalfDaysActivated = true,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Name"
            };
            _fixture.Context.LeaveTypes.Add(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new DeleteLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteLeaveTypeCommand(companyId: 1, leaveTypeId: 1);

            // Act
            var tcs = new CancellationToken();
            // Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
            Assert.Equal(404, exception.Code);
        }

        /// Test.3: Yanlış bir leaveTypeId gönderilmesi
        [Fact]
        public async Task Should_DeleteLeaveType_When_PassInvalidLeaveTypeId()
        {
            // Arrange
            var entity = new LeaveType()
            {
                ColorCode = "ffffff",
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                DefaultDaysPerYear = 10,
                IsActive = true,
                IsAllowNegativeBalance = true,
                IsApproverRequired = false,
                IsDefault = false,
                IsDeleted = false,
                IsHalfDaysActivated = true,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Name"
            };
            _fixture.Context.LeaveTypes.Add(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new DeleteLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteLeaveTypeCommand(companyId: 1, leaveTypeId: -1);

            // Act
            var tcs = new CancellationToken();
            // Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
            Assert.Equal(404, exception.Code);
        }

        /// Test.4: Default tipinde bir leaveTypeId'nin silinmesi
        [Fact]
        public async Task Should_DeleteLeaveType_When_LeaveTypeIsDefault()
        {
            // Arrange
            var entity = new LeaveType()
            {
                ColorCode = "ffffff",
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                DefaultDaysPerYear = 10,
                IsActive = true,
                IsAllowNegativeBalance = true,
                IsApproverRequired = false,
                IsDefault = true,
                IsDeleted = false,
                IsHalfDaysActivated = true,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Name"
            };
            _fixture.Context.LeaveTypes.Add(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new DeleteLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new DeleteLeaveTypeCommand(companyId: 1, leaveTypeId: 1);

            // Act
            var tcs = new CancellationToken();
            // Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            Assert.Equal(ExceptionMessages.DefaultLeaveTypeCannotDelete, exception.Message);
            Assert.Equal(400, exception.Code);
        }
    }
}
