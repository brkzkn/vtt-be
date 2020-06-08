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
    public class UpdateTeamHandlerTest
    {
        private readonly VacationTrackingDbContextFixture _fixture;
        private readonly NullLogger<UpdateTeamHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateTeamHandlerTest(VacationTrackingDbContextFixture fixture)
        {
            _fixture = fixture;
            _logger = new NullLogger<UpdateTeamHandler>();

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

        ///Test.1: Takım güncelleme.
        [Fact]
        public async Task Should_UpdateTeam_When_PassValidParameters()
        {
            throw new NotImplementedException();
        }
        ///Test.2: Geçersiz userId takıma ekleme
        [Fact]
        public async Task Should_ThrowException_When_PassInvalidUserId()
        {
            throw new NotImplementedException();
        }
        ///Test.3: Takım üyelerinden birini çıkartıp diğerini ekleme.
        [Fact]
        public async Task Should_UpdateTeamMember_When_PassMemberUserIds()
        {
            throw new NotImplementedException();
        }

        ///Test.4: En az bir approver olması lazım.
        [Fact]
        public async Task Should_ThrowException_When_EmptyApprover()
        {
            throw new NotImplementedException();
        }

        ///Test.5: Güncelleme sırasında takım isminin unique olması.
        [Fact]
        public async Task Should_ThrowException_When_ExistingTeamName()
        {
            throw new NotImplementedException();
        }
    }
}
