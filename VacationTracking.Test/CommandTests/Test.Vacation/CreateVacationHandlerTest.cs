using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Vacation;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Enums;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Commands.Vacation;
using VacationTracking.Service.Validation.Commands.Vacation;
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
        private readonly IRepository<Vacation> _repository;
        private readonly IRepository<Holiday> _holidayRepository;
        private readonly IRepository<LeaveType> _leaveTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreateVacationHandler _handler;
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

            _repository = new Repository<Vacation>(_fixture.Context);
            _holidayRepository = new Repository<Holiday>(_fixture.Context);
            _leaveTypeRepository = new Repository<LeaveType>(_fixture.Context);
            _unitOfWork = new UnitOfWork(_fixture.Context);

            _handler = new CreateVacationHandler(unitOfWork: _unitOfWork,
                                                 repository: _repository, 
                                                 holidayRepository: _holidayRepository, 
                                                 leaveTypeRepository: _leaveTypeRepository, 
                                                 logger: _logger, 
                                                 mapper: _mapper);
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
            var validator = new VacationCommandValidator();

            // Act
            var result = await _handler.Handle(request, cancellationToken);
            var validationResult  = await validator.ValidateAsync(request);

            // Assert
            Assert.Equal(1, result.VacationId);
            Assert.Equal(VacationStatus.Pending, result.VacationStatus);
            Assert.Equal(DateTime.Now.Date, result.CreatedAt.Date);
            Assert.Equal(1, result.CreatedBy);
            Assert.False(result.IsHalfDay);
            Assert.Equal("test reason", result.Reason);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Should_CreateVacation_When_PassHolidayDaysForOtherCompany()
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
            _fixture.Context.Holidays.Add(new Holiday()
            {
                CompanyId = 2,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 10),
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                IsFullDay = true,
                Name = "Company_2_Holiday"
            });

            _fixture.Context.SaveChanges();

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
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal(1, result.VacationId);
            Assert.Equal(VacationStatus.Pending, result.VacationStatus);
            Assert.Equal(DateTime.Now.Date, result.CreatedAt.Date);
            Assert.Equal(1, result.CreatedBy);
            Assert.False(result.IsHalfDay);
            Assert.Equal("test reason", result.Reason);
        }

        [Fact]
        public async Task Should_CreateVacation_When_FillReasonIfRequired()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 1,
                IsDefault = true,
                IsAllowNegativeBalance = true,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = true,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = true,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 20);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: "test reason",
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();
            var validator = new VacationCommandValidator();
            // Act
            var result = await _handler.Handle(request, cancellationToken);
            var validation = await validator.ValidateAsync(request);
            // Assert
            Assert.Equal(1, result.VacationId);
            Assert.Equal(VacationStatus.Pending, result.VacationStatus);
            Assert.Equal(DateTime.Now.Date, result.CreatedAt.Date);
            Assert.Equal(1, result.CreatedBy);
            Assert.False(result.IsHalfDay);
            Assert.Equal("test reason", result.Reason);
            Assert.True(validation.IsValid);
        }

        [Fact]
        public async Task Should_CreateVacationWithNegativeBalance_When_LeaveTypeAllows()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = false,
                DefaultDaysPerYear = 1,
                IsDefault = true,
                IsAllowNegativeBalance = true,
                IsApproverRequired = false,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = true,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 20);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: true);

            var cancellationToken = new CancellationToken();

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal(1, result.VacationId);
            Assert.Equal(VacationStatus.Pending, result.VacationStatus);
            Assert.Equal(DateTime.Now.Date, result.CreatedAt.Date);
            Assert.Equal(1, result.CreatedBy);
            Assert.False(result.IsHalfDay);
            Assert.Equal("test reason", result.Reason);
        }

        [Fact]
        public async Task Should_CreateVacationWithoutApproval_When_LeaveTypeAllows()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 0,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = false,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = true,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 20);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: "test reason",
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal(1, result.VacationId);
            Assert.Equal(VacationStatus.Approved, result.VacationStatus);
            Assert.Equal(DateTime.Now.Date, result.CreatedAt.Date);
            Assert.Equal(1, result.CreatedBy);
            Assert.False(result.IsHalfDay);
            Assert.Equal("test reason", result.Reason);
        }

        [Fact]
        public async Task Should_CreateVacationWithoutLimitCheck_When_LeaveTypeAllows()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = false,
                DefaultDaysPerYear = 0,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = true,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 20);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: true);

            var cancellationToken = new CancellationToken();

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal(1, result.VacationId);
            Assert.Equal(VacationStatus.Pending, result.VacationStatus);
            Assert.Equal(DateTime.Now.Date, result.CreatedAt.Date);
            Assert.Equal(1, result.CreatedBy);
            Assert.False(result.IsHalfDay);
            Assert.Equal("test reason", result.Reason);
        }

        [Fact]
        public async Task Should_CreateVacation_When_VacationIsHalfDaysAndUserHasHalfDayVacation()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 2,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = true,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.Vacations.Add(new Vacation()
            {
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 8),
                IsHalfDay = true,
                LeaveTypeId = 1,
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                UserId = 1,
                VacationStatus = VacationStatus.Approved
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 8);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: "test reason",
                                                    isHalfDay: true);

            var cancellationToken = new CancellationToken();
            var validator = new VacationCommandValidator();
            // Act
            var result = await _handler.Handle(request, cancellationToken);
            var validation = await validator.ValidateAsync(request);

            // Assert
            Assert.Equal(2, result.VacationId);
            Assert.Equal(VacationStatus.Pending, result.VacationStatus);
            Assert.Equal(DateTime.Now.Date, result.CreatedAt.Date);
            Assert.Equal(1, result.CreatedBy);
            Assert.True(result.IsHalfDay);
            Assert.Equal("test reason", result.Reason);
            Assert.True(validation.IsValid);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassHolidayDays()
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
            _fixture.Context.Holidays.Add(new Holiday()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 10),
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                IsFullDay = true,
                Name = "Company_1_Holiday"
            });

            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 7);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 11);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: "test reason",
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.VacationDateIsNotValid, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassExistVacationDays()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 2,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = false,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.Vacations.Add(new Vacation()
            {
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 10),
                IsHalfDay = false,
                LeaveTypeId = 1,
                StartDate = new DateTime(year: 2020, month: 6, day: 6),
                UserId = 1,
                VacationStatus = VacationStatus.Approved
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 8);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.VacationDateIsNotValid, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_VacationIsFullAndExistVacationDayIsHalf()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 2,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = false,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.Vacations.Add(new Vacation()
            {
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 6),
                IsHalfDay = true,
                LeaveTypeId = 1,
                StartDate = new DateTime(year: 2020, month: 6, day: 6),
                UserId = 1,
                VacationStatus = VacationStatus.Approved
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 6);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 6);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.VacationDateIsNotValid, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidLeaveTypeId()
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
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
            });
            _fixture.Context.Holidays.Add(new Holiday()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 10),
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                IsFullDay = true,
                Name = "Company_1_Holiday"
            });

            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 10);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: -1,
                                                    startDate,
                                                    endDate,
                                                    reason: "test reason",
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.VacationLeaveTypeIsNotValid, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInactiveLeaveType()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = false,
                DefaultDaysPerYear = 20,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
            });
            _fixture.Context.Holidays.Add(new Holiday()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 10),
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                IsFullDay = true,
                Name = "Company_1_Holiday"
            });

            _fixture.Context.SaveChanges();

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

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.VacationLeaveTypeIsNotValid, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_DoesNotFillReasonIfRequired()
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
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.Holidays.Add(new Holiday()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 10),
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                IsFullDay = true,
                Name = "Company_1_Holiday"
            });

            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 10);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.VacationReasonIsRequired, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_ReachLimitOfLeaveType()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = false,
                DefaultDaysPerYear = 2,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.Holidays.Add(new Holiday()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 10),
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                IsFullDay = true,
                Name = "Company_1_Holiday"
            });
            _fixture.Context.Vacations.Add(new Vacation()
            {
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                EndDate = DateTime.Now.Date,
                IsHalfDay = false,
                LeaveTypeId = 1,
                StartDate = DateTime.Now.Date,
                UserId = 1,
                VacationStatus = VacationStatus.Approved
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 10);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.LeaveTypeDoesNotAllowNegativeBalance, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_VacationIsHalfDayIfLeaveTypeDoesNotAllow()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 2,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = false,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.Vacations.Add(new Vacation()
            {
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 8),
                IsHalfDay = true,
                LeaveTypeId = 1,
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                UserId = 1,
                VacationStatus = VacationStatus.Approved
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 8);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: true);

            var cancellationToken = new CancellationToken();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.LeaveTypeDoesNotAllowHalfDays, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ThrowException_When_VacationIsFullAndUserHasHalfDays()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 2,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.Vacations.Add(new Vacation()
            {
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 8),
                IsHalfDay = true,
                LeaveTypeId = 1,
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                UserId = 1,
                VacationStatus = VacationStatus.Approved
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 8);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: "test reason",
                                                    isHalfDay: false);

            var cancellationToken = new CancellationToken();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await _handler.Handle(request, cancellationToken);
            });

            Assert.Equal(ExceptionMessages.VacationDateIsNotValid, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        [Fact]
        public async Task Should_ValidatorReturnFalse_When_EndDateGreaterThanStartDate()
        {
            // Arrange
            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 3);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: false);

            var validator = new VacationCommandValidator();

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Should_ValidatorReturnFalse_When_PassTwoDaysWithIsHalfDaysTrue()
        {
            // Arrange
            _fixture.Context.LeaveTypes.Add(new LeaveType()
            {
                CompanyId = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                IsActive = true,
                DefaultDaysPerYear = 2,
                IsDefault = true,
                IsAllowNegativeBalance = false,
                IsApproverRequired = true,
                IsDeleted = false,
                IsHalfDaysActivated = false,
                IsHideLeaveTypeName = false,
                IsReasonRequired = true,
                IsUnlimited = false,
                LeaveTypeName = "Test Leave Type",
                LeaveTypeId = 1
            });
            _fixture.Context.Vacations.Add(new Vacation()
            {
                CreatedAt = DateTime.Now,
                CreatedBy = -1,
                EndDate = new DateTime(year: 2020, month: 6, day: 8),
                IsHalfDay = true,
                LeaveTypeId = 1,
                StartDate = new DateTime(year: 2020, month: 6, day: 8),
                UserId = 1,
                VacationStatus = VacationStatus.Approved
            });
            _fixture.Context.SaveChanges();

            DateTime startDate = new DateTime(year: 2020, month: 6, day: 8);
            DateTime endDate = new DateTime(year: 2020, month: 6, day: 10);

            var request = new CreateVacationCommand(companyId: 1,
                                                    userId: 1,
                                                    leaveTypeId: 1,
                                                    startDate,
                                                    endDate,
                                                    reason: string.Empty,
                                                    isHalfDay: true);

            var validator = new VacationCommandValidator();
            // Act
            var validationResult = await validator.ValidateAsync(request);

            // Assert
            Assert.False(validationResult.IsValid);
        }
    }
}
