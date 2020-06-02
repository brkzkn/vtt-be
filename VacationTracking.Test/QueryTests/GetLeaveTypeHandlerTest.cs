using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using VacationTracking.Domain.Queries.LeaveType;
using VacationTracking.Service.Queries.LeaveType;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetLeaveTypeHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetLeaveTypeHandler> _logger;
        private readonly IMapper _mapper;
        public GetLeaveTypeHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetLeaveTypeHandler>();

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

        [Fact]
        public async Task Should_ReturnLeaveTypeObject_When_PassValidLeaveTypeId()
        {
            // Arrange
            var leaveType = new LeaveType()
            {
                LeaveTypeId = 1,
                CompanyId = 1,
                LeaveTypeName = "Leave Type Test",
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsDeleted = false,
                IsActive = true
            };

            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new GetLeaveTypeHandler(repository, _mapper, _logger);
            var queryRequest = new GetLeaveTypeQuery(leaveTypeId: 1, companyId: 1);
            _fixture.Context.LeaveTypes.Add(leaveType);
            _fixture.Context.SaveChanges();

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(queryRequest, tcs);

            // Assert
            Assert.Equal(1, result.LeaveTypeId);
            Assert.Equal(1, result.CompanyId);
        }

        [Fact]
        public async Task Should_ReturnLeaveTypeObject_When_PassInactiveLeaveTypeId()
        {
            // Arrange
            var leaveType = new LeaveType()
            {
                LeaveTypeId = 1,
                CompanyId = 1,
                LeaveTypeName = "Leave Type Test",
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsDeleted = false,
                IsActive = false
            };

            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new GetLeaveTypeHandler(repository, _mapper, _logger);
            var queryRequest = new GetLeaveTypeQuery(leaveTypeId: 1, companyId: 1);
            _fixture.Context.LeaveTypes.Add(leaveType);
            _fixture.Context.SaveChanges();

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(queryRequest, tcs);

            // Assert
            Assert.Equal(1, result.LeaveTypeId);
            Assert.Equal(1, result.CompanyId);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassDeletedLeaveTypeId()
        {
            // Arrange
            var leaveType = new LeaveType()
            {
                LeaveTypeId = 1,
                CompanyId = 1,
                LeaveTypeName = "Leave Type Test",
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsDeleted = true,
                IsActive = false
            };

            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new GetLeaveTypeHandler(repository, _mapper, _logger);
            var queryRequest = new GetLeaveTypeQuery(leaveTypeId: 1, companyId: 1);
            _fixture.Context.LeaveTypes.Add(leaveType);
            _fixture.Context.SaveChanges();

            // Act
            var tcs = new CancellationToken();


            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(queryRequest, tcs);
            });

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidLeaveTypeId()
        {
            // Arrange
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new GetLeaveTypeHandler(repository, _mapper, _logger);
            var queryRequest = new GetLeaveTypeQuery(leaveTypeId: -1, companyId: 1);

            // Act
            var tcs = new CancellationToken();


            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(queryRequest, tcs);
            });

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassLeaveTypeIdDoesNotBelongsToCompanyId()
        {
            // Arrange
            var leaveType = new LeaveType()
            {
                LeaveTypeId = 1,
                CompanyId = 1,
                LeaveTypeName = "Leave Type Test",
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsDeleted = true,
                IsActive = false
            };

            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new GetLeaveTypeHandler(repository, _mapper, _logger);
            var queryRequest = new GetLeaveTypeQuery(leaveTypeId: 1, companyId: 2);
            _fixture.Context.LeaveTypes.Add(leaveType);
            _fixture.Context.SaveChanges();

            // Act
            var tcs = new CancellationToken();


            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(queryRequest, tcs);
            });

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }

    }
}
