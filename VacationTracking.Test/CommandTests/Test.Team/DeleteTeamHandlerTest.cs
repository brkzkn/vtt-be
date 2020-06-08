using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Service.Commands.Team;
using VacationTracking.Test.Context;
using VacationTracking.Test.Data;
using Xunit;

namespace VacationTracking.Test.CommandTests
{
    [Collection(nameof(VacationTrackingContext))]
    public class DeleteTeamHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<DeleteTeamHandler> _logger;
        private readonly IMapper _mapper;
        public DeleteTeamHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<DeleteTeamHandler>();

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

        ///Test.1: Team silinince ilgili TeamMember silinmesi
        [Fact]
        public async Task Should_DeleteTeamWithMember_When_PassTeamId()
        {
            throw new NotImplementedException();
        }

        ///Test.2: Default team'in silinmemesi
        [Fact]
        public async Task Should_ThrowException_When_PassTeamIdForDefaultTeam()
        {
            throw new NotImplementedException();
        }

        ///Test.3: Başka bir firmaya ait teamId silinmesi. Hata fırlatması lazım.
        [Fact]
        public async Task Should_ThrowException_When_PassTeamIdForDifferentCompany()
        {
            throw new NotImplementedException();
        }

        ///Test.4: Geçersiz bir teamId parametre olarak gönderilmesi.
        [Fact]
        public async Task Should_ThrowException_When_PassInvalidTeamId()
        {
            throw new NotImplementedException();
        }
    }
}
