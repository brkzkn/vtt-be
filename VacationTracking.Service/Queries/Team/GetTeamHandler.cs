using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Models;
using VacationTracking.Domain.Queries.Team;
using TeamDb = VacationTracking.Domain.Models.Team;

namespace VacationTracking.Service.Queries.Team
{
    public class GetTeamHandler : IRequestHandler<GetTeamQuery, TeamDto>
    {
        private readonly IRepository<TeamDb> _repository;
        private readonly IRepository<Company> _companyRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetTeamHandler(IRepository<TeamDb> repository, IRepository<Company> companyRepository, IMapper mapper, ILogger<GetTeamHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _companyRepository = companyRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TeamDto> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            //// TODO: Check user permission. 
            //// User should has owner or admin permission
            ///
            var a = _companyRepository.Queryable().ToList();

            var team = await _repository.Queryable()
                                        //.Include(x => x.TeamMembers)
                                        //.Where(x => x.TeamId == request.TeamId && x.CompanyId == request.CompanyId)
                                        .ToListAsync();

            throw new NotImplementedException();
            //var teamMembers = await _teamMemberRepository.GetListAsync(request.TeamId);
            //team.TeamMembers = new List<TeamMember>();
            //team.TeamMembers.AddRange(teamMembers);

            //if (team != null)
            //{
            //    _logger.LogInformation($"Got a request get customer Id: {team.TeamId}");
            //    var teamDto = _mapper.Map<Domain.Dtos.TeamDto>(team);
            //    return teamDto;
            //}

            //return null;
        }
    }
}
