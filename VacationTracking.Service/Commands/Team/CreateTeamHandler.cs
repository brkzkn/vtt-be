using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Models;

namespace VacationTracking.Service.Commands.Team
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, TeamDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;

        public CreateTeamHandler(ITeamRepository teamRepository, ILogger<CreateTeamHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        }

        public async Task<TeamDto> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            
            var teamEntity = new Domain.Models.Team();
            Guid teamId = Guid.NewGuid();
            teamEntity.TeamName = request.Name;
            teamEntity.CompanyId = request.CompanyId;
            teamEntity.CreatedAt = DateTime.Now;
            teamEntity.CreatedBy = request.UserId;
            teamEntity.TeamId = teamId;

            teamEntity.TeamMembers = new List<TeamMember>();
            Dictionary<Guid, TeamMember> teamMembersDic = new Dictionary<Guid, TeamMember>();
            foreach (var member in request.Members)
            {
                TeamMember teamMember = new TeamMember()
                {
                    IsApprover = false,
                    IsMember = true,
                    TeamId = teamId,
                    UserId = member
                };
                teamMembersDic.Add(member, teamMember);
            }

            foreach (var approver in request.Approvers)
            {
                if (!teamMembersDic.TryGetValue(approver, out TeamMember teamMember))
                {
                    teamMember = new TeamMember()
                    {
                        IsApprover = true,
                        IsMember = false,
                        TeamId = teamId,
                        UserId = approver
                    };
                    teamMembersDic.Add(approver, teamMember);
                }
                else
                {
                    teamMember.IsApprover = true;
                }
            }

            teamEntity.TeamMembers = teamMembersDic.Values.AsList();
            var result = await _teamRepository.CreateTeamAsync(teamEntity);

            //TODO: Fire "teamCreated" event

            return _mapper.Map<TeamDto>(result);
            
        }
    }
}
