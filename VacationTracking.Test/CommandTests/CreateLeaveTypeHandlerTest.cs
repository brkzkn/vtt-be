using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
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
using VacationTracking.Service.Validation.Commands.LeaveType;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class CreateLeaveTypeHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<CreateLeaveTypeHandler> _logger;
        private readonly IMapper _mapper;
        public CreateLeaveTypeHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<CreateLeaveTypeHandler>();

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
        public async Task Should_CreateLeaveType_When_PassValidParameters()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new CreateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new CreateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     isHalfDaysActivated: true,
                                                     isHideLeaveTypeName: false,
                                                     typeName: "Test Leave Type",
                                                     isApprovalRequired: true,
                                                     defaultDaysPerYear: 0,
                                                     isUnlimited: true,
                                                     isReasonRequired: true,
                                                     allowNegativeBalance: true,
                                                     color: "FFFFFF");

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal(1, result.LeaveTypeId);
            Assert.Equal(1, result.CreatedBy);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassExistLeaveTypeName()
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

            var handler = new CreateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new CreateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     isHalfDaysActivated: true,
                                                     isHideLeaveTypeName: false,
                                                     typeName: "Test Name",
                                                     isApprovalRequired: true,
                                                     defaultDaysPerYear: 0,
                                                     isUnlimited: true,
                                                     isReasonRequired: true,
                                                     allowNegativeBalance: true,
                                                     color: "FFFFFF");

            // Act
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            // Assert
            Assert.Equal(ExceptionMessages.LeaveTypeNameAlreadyExist, exception.Message);
        }

        [Fact]
        public async Task Should_CreateLeaveType_When_PassDeletedLeaveTypeName()
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
                IsDeleted = true,
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

            var handler = new CreateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new CreateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     isHalfDaysActivated: true,
                                                     isHideLeaveTypeName: true,
                                                     typeName: "Test Name",
                                                     isApprovalRequired: true,
                                                     defaultDaysPerYear: 10,
                                                     isUnlimited: true,
                                                     isReasonRequired: true,
                                                     allowNegativeBalance: true,
                                                     color: "FFFFFF");

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("Test Name", result.LeaveTypeName);
            Assert.Equal("FFFFFF", result.ColorCode);
            Assert.True(result.IsHalfDaysActivated);
            Assert.True(result.IsHideLeaveTypeName);
            Assert.True(result.IsApproverRequired);
            Assert.Equal(10, result.DefaultDaysPerYear);
            Assert.True(result.IsUnlimited);
            Assert.True(result.IsReasonRequired);
            Assert.True(result.IsAllowNegativeBalance);
            Assert.Equal(1, result.CreatedBy);
        }

        [Fact]
        public async Task Should_ValidatorReturnFalse_When_PassDaysPerYearEqualZeroForIsUnlimitedFalse()
        {
            LeaveTypeCommandValidator validator = new LeaveTypeCommandValidator();
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new CreateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);
            var request = new CreateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     isHalfDaysActivated: true,
                                                     isHideLeaveTypeName: true,
                                                     typeName: "Test Name",
                                                     isApprovalRequired: true,
                                                     defaultDaysPerYear: 0,
                                                     isUnlimited: false,
                                                     isReasonRequired: true,
                                                     allowNegativeBalance: true,
                                                     color: "FFFFFF");

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Should_ValidatorReturnTrue_When_PassDaysPerYearEqualZeroForIsUnlimitedTrue()
        {
            LeaveTypeCommandValidator validator = new LeaveTypeCommandValidator();
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new CreateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);
            var request = new CreateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     isHalfDaysActivated: true,
                                                     isHideLeaveTypeName: true,
                                                     typeName: "Test Name",
                                                     isApprovalRequired: true,
                                                     defaultDaysPerYear: 0,
                                                     isUnlimited: true,
                                                     isReasonRequired: true,
                                                     allowNegativeBalance: true,
                                                     color: "FFFFFF");

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.True (result.IsValid);
        }

        [Fact]
        public async Task Should_ValidatorReturnTrue_When_PassDaysPerYearGreaterThanZeroForIsUnlimitedTrue()
        {
            LeaveTypeCommandValidator validator = new LeaveTypeCommandValidator();
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new CreateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);
            var request = new CreateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     isHalfDaysActivated: true,
                                                     isHideLeaveTypeName: true,
                                                     typeName: "Test Name",
                                                     isApprovalRequired: true,
                                                     defaultDaysPerYear: 10,
                                                     isUnlimited: true,
                                                     isReasonRequired: true,
                                                     allowNegativeBalance: true,
                                                     color: "FFFFFF");

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Should_ValidatorReturnTrue_When_PassDaysPerYearGreaterThanZeroForIsUnlimitedFalse()
        {
            LeaveTypeCommandValidator validator = new LeaveTypeCommandValidator();
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new CreateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);
            var request = new CreateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     isHalfDaysActivated: true,
                                                     isHideLeaveTypeName: true,
                                                     typeName: "Test Name",
                                                     isApprovalRequired: true,
                                                     defaultDaysPerYear: 10,
                                                     isUnlimited: false,
                                                     isReasonRequired: true,
                                                     allowNegativeBalance: true,
                                                     color: "FFFFFF");

            // Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.True(result.IsValid);
        }
    }
}
