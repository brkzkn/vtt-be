using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Team;
using VacationTracking.Service.Dxos;

namespace VacationTracking.Service.Queries.Team
{
    public class GetTeamListHandler : IRequestHandler<GetTeamListQuery, IList<TeamDto>>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamDxos _teamDxos;
        private readonly ILogger _logger;

        public GetTeamListHandler(ITeamRepository teamRepository, ITeamDxos teamDxos, ILogger<GetTeamHandler> logger)
        {
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _teamDxos = teamDxos ?? throw new ArgumentNullException(nameof(teamDxos));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IList<TeamDto>> Handle(GetTeamListQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var teams = await _teamRepository.GetListAsync(request.CompanyId);

            if (teams != null)
            {
                _logger.LogInformation($"Got a request get teams by CompanyId: {request.CompanyId}");
                List<TeamDto> teamDtos = new List<TeamDto>();
                foreach (var team in teams)
                {
                    teamDtos.Add(_teamDxos.MapTeamDto(team));
                }
                return teamDtos;
            }

            return null;
        }
    }
}
