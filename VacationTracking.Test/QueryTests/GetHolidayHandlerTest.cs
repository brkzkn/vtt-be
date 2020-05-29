using System.Collections.Generic;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Models;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.QueryTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class GetHolidayHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly ICollection<User> _users;

        public GetHolidayHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _users = Seed.Users();
            
            _fixture.Initialize(true, () =>
            {
                _fixture.Context.Users.AddRange(_users);
                _fixture.Context.SaveChanges();
            });
        }

        [Fact]
        public void UsersById_Should_Return_User()
        {
            // Arrange
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context);
            IRepository<User> userRepository = new Repository<User>(_fixture.Context);
            //var userService = new UserService(userRepository);

            // Act
            //var user = userService.Get("burak.ozkan@mcbuopsfw.onmicrosoft.com");

            // Assert
            //Assert.Equal(1, user.UserId);
        }
    }
}
