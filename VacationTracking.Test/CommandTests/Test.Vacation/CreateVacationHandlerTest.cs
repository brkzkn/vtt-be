using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Vacation;
using VacationTracking.Domain.Enums;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Commands.Vacation;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class CreateVacationHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<CreateVacationHandler> _logger;
        private readonly IMapper _mapper;
        public CreateVacationHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<CreateVacationHandler>();

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
        public async Task Should_CreateVacation_When_PassValidParameters()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 20,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = false,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
            });
            _fixture.Context.SaveChanges();

            IRepository<Vacation> repository = new Repository<Vacation>(_fixture.Context);
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);

            var handler = new CreateVacationHandler(unitOfWork, repository, _logger, _mapper);
            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 10);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: "test reason",
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal(1, result.VacationId);
            Assert.Equal(VacationStatus.Pending, result.VacationStatus);
            Assert.Equal(DateTime.Now.Date, result.CreatedAt.Date);
            Assert.Equal(1, result.CreatedBy);
            Assert.False(result.IsHalfDay);
            Assert.Equal("test reason", result.Reason);
        }

        [Fact]
        public async Task Should_CreateVacation_When_PassHolidayDaysForOtherCompany()
        {
            /// Create vacation with other company's holiday days
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_CreateVacation_When_FillReasonIfRequired()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_CreateVacationWithNegativeBalance_When_LeaveTypeAllows()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_CreateVacationWithoutApproval_When_LeaveTypeAllows()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_CreateVacationWithoutLimitCheck_When_LeaveTypeAllows()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_CreateVacation_When_VacationIsHalfDaysAndUserHasHalfDayVacation()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_PassHolidayDays()
        {
            /// Throw exception for company holidays
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_PassExistVacationDays()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidVacationType()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInactiveLeaveType()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_DoesNotFillReasonIfRequired()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_ReachLimitOfLeaveType()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_EndDateGreaterThanStartDate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_VacationIsHalfDayIfLeaveTypeDoesNotAllow()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_PassNonWorkingDays()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_VacationIsFullAndUserHasHalfDays()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_ThrowException_When_PassTwoDaysWithIsHalfDaysTrue()
        {
            throw new NotImplementedException();
        }

    }
}
