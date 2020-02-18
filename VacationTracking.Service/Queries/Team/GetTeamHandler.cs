using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Team;
using VacationTracking.Service.Dxos;

namespace VacationTracking.Service.Queries.Team
{
    public class GetTeamHandler : IRequestHandler<GetTeamQuery, TeamDto>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamDxos _teamDxos;
        private readonly ILogger _logger;

        public GetTeamHandler(ITeamRepository teamRepository, ITeamDxos teamDxos, ILogger<GetTeamHandler> logger)
        {
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _teamDxos = teamDxos ?? throw new ArgumentNullException(nameof(teamDxos));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TeamDto> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetAsync(request.TeamId);

            if (team != null)
            {
                _logger.LogInformation($"Got a request get customer Id: {team.TeamId}");
                var teamDto = _teamDxos.MapCustomerDto(team);
                return teamDto;
            }

            return null;
        }
    }
}
