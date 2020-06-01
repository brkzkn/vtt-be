using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Enums;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Queries.Team;
using TeamDb = VacationTracking.Domain.Models.Team;

namespace VacationTracking.Service.Queries.Team
{
    public class GetTeamHandler : IRequestHandler<GetTeamQuery, TeamDto>
    {
        private readonly IRepository<TeamDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetTeamHandler(IRepository<TeamDb> repository, IMapper mapper, ILogger<GetTeamHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TeamDto> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            var team = await _repository.Queryable()
                                        .Include(x => x.TeamMembers)
                                            .ThenInclude(x => x.User)
                                        .SingleOrDefaultAsync(x => x.TeamId == request.TeamId && x.CompanyId == request.CompanyId);

            if (team != null)
            {
                _logger.LogInformation($"Got a request get team Id: {team.TeamId}");
                var teamDto = _mapper.Map<TeamDto>(team);
                teamDto.TeamMembers = _mapper.Map<IList<TeamMemberDto>>(team.TeamMembers.Where(x => x.User.Status == UserStatus.Active));

                return teamDto;
            }

            throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"Team not found by id {request.TeamId}", 404);
        }
    }
}
