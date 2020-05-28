using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Common;

namespace VacationTracking.Service.Commands.Team
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, TeamDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamHandler(IUnitOfWork unitOfWork, ILogger<CreateTeamHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<TeamDto> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            //var teamEntity = new Domain.Models.Team();
            //Guid teamId = Guid.NewGuid();
            //teamEntity.TeamName = request.Name;
            //teamEntity.CompanyId = request.CompanyId;
            //teamEntity.CreatedAt = DateTime.Now;
            //teamEntity.CreatedBy = request.UserId;
            //teamEntity.TeamId = teamId;

            //teamEntity.TeamMembers = new List<TeamMember>();
            //teamEntity.TeamMembers = TeamFunctions.MergeMemberAndApprover(teamId, request.Members, request.Approvers);

            //using (_unitOfWork)
            //{
            //    _unitOfWork.Begin();
            //    var affectedRow = await _unitOfWork.TeamMemberRepository.SetAsNonMemberToOtherTeams(request.Members);
            //    affectedRow = await _unitOfWork.TeamMemberRepository.RemoveNotActiveMemberAsync();
            //    affectedRow = await _unitOfWork.TeamRepository.InsertAsync(teamEntity);
            //    affectedRow = await _unitOfWork.TeamMemberRepository.InsertAsync(teamEntity.TeamMembers);

            //    _unitOfWork.Commit();
            //}

            ////TODO: Fire "teamCreated" event

            //return _mapper.Map<TeamDto>(teamEntity);
        }
    }
}
