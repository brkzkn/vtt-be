using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Team;

namespace VacationTracking.Service.Queries.Team
{
    public class GetTeamHandler : IRequestHandler<GetTeamQuery, TeamDto>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetTeamHandler(ITeamRepository teamRepository, IMapper mapper, ILogger<GetTeamHandler> logger)
        {
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TeamDto> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var team = await _teamRepository.GetAsync(request.TeamId, request.CompanyId);

            if (team != null)
            {
                _logger.LogInformation($"Got a request get customer Id: {team.TeamId}");
                var teamDto = _mapper.Map<Domain.Dtos.TeamDto>(team);
                return teamDto;
            }

            return null;
        }
    }
}
