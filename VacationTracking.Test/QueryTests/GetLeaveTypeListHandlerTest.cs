using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Queries.LeaveType;
using VacationTracking.Service.Queries.LeaveType;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;
using LeaveTypeDb = VacationTracking.Domain.Models.LeaveType;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetLeaveTypeListHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetLeaveTypeListHandler> _logger;
        private readonly IMapper _mapper;
        public GetLeaveTypeListHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetLeaveTypeListHandler>();

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
        public async Task Should_ReturnLeaveTypeList_When_PassValidCompanyId()
        {
            // Arrange
            var leaveType = new LeaveTypeDb()
            {
                LeaveTypeId = 1,
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsDeleted = false,
                IsActive = true
            };
            _fixture.Context.LeaveTypes.Add(leaveType);
            _fixture.Context.SaveChanges();

            IRepository<LeaveTypeDb> repository = new Repository<LeaveTypeDb>(_fixture.Context);

            var handler = new GetLeaveTypeListHandler(repository, _mapper, _logger);

            var queryRequest = new GetLeaveTypeListQuery(companyId: 1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(queryRequest, tcs);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_PassValidCompanyId()
        {
            // Arrange
            IRepository<LeaveTypeDb> repository = new Repository<LeaveTypeDb>(_fixture.Context);

            var handler = new GetLeaveTypeListHandler(repository, _mapper, _logger);

            var request = new GetLeaveTypeListQuery(companyId: 3);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_PassInvalidCompanyId()
        {
            // Arrange
            var leaveType = new LeaveTypeDb()
            {
                LeaveTypeId = 1,
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsDeleted = false,
                IsActive = true
            };
            _fixture.Context.LeaveTypes.Add(leaveType);
            _fixture.Context.SaveChanges();

            IRepository<LeaveTypeDb> repository = new Repository<LeaveTypeDb>(_fixture.Context);

            var handler = new GetLeaveTypeListHandler(repository, _mapper, _logger);

            var queryRequest = new GetLeaveTypeListQuery(companyId: -1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(queryRequest, tcs);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_ReturnOnlyNotDeletedItemList_When_PassValidCompanyId()
        {
            // Arrange
            var leaveType = new LeaveTypeDb()
            {
                LeaveTypeId = 1,
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsDeleted = false,
                IsActive = true
            };
            _fixture.Context.LeaveTypes.Add(leaveType);
            leaveType = new LeaveTypeDb()
            {
                LeaveTypeId = 2,
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsDeleted = true,
                IsActive = true
            };
            _fixture.Context.LeaveTypes.Add(leaveType);
            _fixture.Context.SaveChanges();

            IRepository<LeaveTypeDb> repository = new Repository<LeaveTypeDb>(_fixture.Context);

            var handler = new GetLeaveTypeListHandler(repository, _mapper, _logger);

            var request = new GetLeaveTypeListQuery(companyId: 1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal(1, result.Count);
        }

    }
}
