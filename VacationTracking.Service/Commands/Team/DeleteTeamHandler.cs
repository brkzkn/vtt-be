using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Commands.Team
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;

        public DeleteTeamHandler(ITeamRepository teamRepository, ILogger<CreateTeamHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        }

        public async Task<bool> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetAsync(request.TeamId, request.CompanyId);
            if (team == null)
                throw new ArgumentNullException(nameof(TeamDto));
            
            var result = await _teamRepository.DeleteTeamAsync(request.CompanyId, request.TeamId);

            return result;
        }
    }
}
