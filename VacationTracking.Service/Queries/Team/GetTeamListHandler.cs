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
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.Team;
using TeamDb = VacationTracking.Domain.Models.Team;

namespace VacationTracking.Service.Queries.Team
{
    public class GetTeamListHandler : IRequestHandler<GetTeamListQuery, IList<TeamDto>>
    {
        private readonly IRepository<TeamDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetTeamListHandler(IRepository<TeamDb> repository, IMapper mapper, ILogger<GetTeamListHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<TeamDto>> Handle(GetTeamListQuery request, CancellationToken cancellationToken)
        {
            var teams = await _repository.Queryable().Where(x => x.CompanyId == request.CompanyId).ToListAsync();
            List<TeamDto> teamDtos = new List<TeamDto>();

            if (teams != null)
            {
                _logger.LogInformation($"Got a request get teams by CompanyId: {request.CompanyId}");
                foreach (var team in teams)
                {
                    teamDtos.Add(_mapper.Map<TeamDto>(team));
                }
                return teamDtos;
            }
            _logger.LogInformation($"Team not found by CompanyId: {request.CompanyId}");

            return teamDtos;
        }
    }
}
