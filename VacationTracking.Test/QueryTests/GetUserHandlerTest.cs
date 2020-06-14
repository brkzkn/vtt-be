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
using VacationTracking.Domain.Queries.User;
using VacationTracking.Service.Queries.User;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetUserHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<GetUserHandler> _logger;
        private readonly IMapper _mapper;
        public GetUserHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<GetUserHandler>();

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
        public async Task Should_ReturnUser_When_PassValidUserId()
        {
            // Arrange
            IRepository<User> repository = new Repository<User>(_fixture.Context);

            var handler = new GetUserHandler(repository, _mapper, _logger);

            var query = new GetUserQuery(companyId: 1, userId: 1);

            // Act
            var tcs = new CancellationToken();

            var result = await handler.Handle(query, tcs);

            // Assert
            Assert.Equal(1, result.UserId);
            Assert.Equal(1, result.CompanyId);
        }

        [Fact]
        public async Task Should_ThrowException_When_PassInvalidUserId()
        {
            // Arrange
            IRepository<User> repository = new Repository<User>(_fixture.Context);
            var handler = new GetUserHandler(repository, _mapper, _logger);

            var query = new GetUserQuery(companyId: 1, userId: -1);

            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(query, tcs);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(404, exception.Code);
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }

        [Fact]
        public async Task Should_ThrowException_When_UserIdDoesNotBelongsToCompany()
        {

            // Arrange
            IRepository<User> repository = new Repository<User>(_fixture.Context);
            var handler = new GetUserHandler(repository, _mapper, _logger);

            var query = new GetUserQuery(companyId: 2, userId: 1);

            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(query, tcs);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(404, exception.Code);
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }

        [Fact]
        public async Task Should_ThrowException_When_UserStatusDisabled()
        {

            // Arrange
            IRepository<User> repository = new Repository<User>(_fixture.Context);
            var user = new User
            {
                UserId = 5,
                CompanyId = 1,
                UserName = "test",
                FullName = "Test User",
                Status = Domain.Enums.UserStatus.Disabled,
                CreatedAt = DateTime.Now,
                CreatedBy = -1
            };
            _fixture.Context.Users.Add(user);
            _fixture.Context.SaveChanges();

            var handler = new GetUserHandler(repository, _mapper, _logger);

            var query = new GetUserQuery(companyId: 1, userId: 5);

            // Act
            var tcs = new CancellationToken();

            var exception = await Assert.ThrowsAsync<VacationTrackingException>(async () =>
            {
                await handler.Handle(query, tcs);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(404, exception.Code);
            Assert.Equal(ExceptionMessages.ItemNotFound, exception.Message);
        }
    }
}
