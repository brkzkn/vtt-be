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
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class UpdateLeaveTypeHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<UpdateLeaveTypeHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateLeaveTypeHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<UpdateLeaveTypeHandler>();

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

        /// Test.1: Tüm alanlar update ediliyor mu?
        [Fact]
        public async Task Should_UpdateLeaveType_When_PassParameters()
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

            var handler = new UpdateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new UpdateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     leaveTypeId: 1,
                                                     isHalfDaysActivated: false,
                                                     isHideLeaveTypeName: false,
                                                     typeName: "Test Name - 1",
                                                     isApprovalRequired: false,
                                                     defaultDaysPerYear: 5,
                                                     isUnlimited: false,
                                                     isReasonRequired: false,
                                                     allowNegativeBalance: false,
                                                     color: "aaaaaa",
                                                     isActive: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("Test Name - 1", result.LeaveTypeName);
            Assert.Equal("aaaaaa", result.ColorCode);
            Assert.False(result.IsHalfDaysActivated);
            Assert.False(result.IsHideLeaveTypeName);
            Assert.False(result.IsApproverRequired);
            Assert.Equal(5, result.DefaultDaysPerYear);
            Assert.False(result.IsUnlimited);
            Assert.False(result.IsReasonRequired);
            Assert.False(result.IsAllowNegativeBalance);
            Assert.Equal(1, result.ModifiedBy);
        }

        /// Test.2: Update edilen isim unique mi?
        [Fact]
        public async Task Should_ThrowException_When_PassExistingLeaveTypeName()
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

            entity = new LeaveType()
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
                LeaveTypeName = "Test Name - 2"
            };
            _fixture.Context.LeaveTypes.Add(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new UpdateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new UpdateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     leaveTypeId: 1,
                                                     isHalfDaysActivated: false,
                                                     isHideLeaveTypeName: false,
                                                     typeName: "Test Name - 2",
                                                     isApprovalRequired: false,
                                                     defaultDaysPerYear: 5,
                                                     isUnlimited: false,
                                                     isReasonRequired: false,
                                                     allowNegativeBalance: false,
                                                     color: "aaaaaa",
                                                     isActive: true);

            // Act
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            // Assert
            Assert.Equal(ExceptionMessages.LeaveTypeNameAlreadyExist, exception.Message);
            Assert.Equal(400, exception.Code);
        }

        /// Test.3: Silinmiş bir leaveType ismi düzgün bir şekilde ayarlanıyor mu?
        [Fact]
        public async Task Should_UpdateLeaveType_When_PassDeletedLeaveTypeName()
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

            entity = new LeaveType()
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
                LeaveTypeName = "Test Name - 2"
            };
            _fixture.Context.LeaveTypes.Add(entity);
            _fixture.Context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<LeaveType> repository = new Repository<LeaveType>(_fixture.Context);

            var handler = new UpdateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new UpdateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     leaveTypeId: 1,
                                                     isHalfDaysActivated: false,
                                                     isHideLeaveTypeName: false,
                                                     typeName: "Test Name - 2",
                                                     isApprovalRequired: false,
                                                     defaultDaysPerYear: 5,
                                                     isUnlimited: false,
                                                     isReasonRequired: false,
                                                     allowNegativeBalance: false,
                                                     color: "aaaaaa",
                                                     isActive: true);

            // Act
            var tcs = new CancellationToken();
            var result = await handler.Handle(request, tcs);

            // Assert
            Assert.Equal("Test Name - 2", result.LeaveTypeName);
        }

        /// Test.4: Farklı company'e ait leaveTypeId update ediliyor mu? 
        [Fact]
        public async Task Should_ThrowException_When_PassInvalidLeaveTypeId()
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

            var handler = new UpdateLeaveTypeHandler(unitOfWork, repository, _logger, _mapper);

            var request = new UpdateLeaveTypeCommand(companyId: 1,
                                                     userId: 1,
                                                     leaveTypeId: 1,
                                                     isHalfDaysActivated: false,
                                                     isHideLeaveTypeName: false,
                                                     typeName: "Test Name - 2",
                                                     isApprovalRequired: false,
                                                     defaultDaysPerYear: 5,
                                                     isUnlimited: false,
                                                     isReasonRequired: false,
                                                     allowNegativeBalance: false,
                                                     color: "aaaaaa",
                                                     isActive: true);

            // Act
            var tcs = new CancellationToken();
            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                var result = await handler.Handle(request, tcs);
            });

            // Assert
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
            Assert.Equal(404, exception.Code);
        }
    }
}
