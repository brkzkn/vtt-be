using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Service.Commands.Team
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand, TeamDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTeamHandler(IUnitOfWork unitOfWork, ILogger<CreateTeamHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<TeamDto> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //var teamEntity = new Domain.Models.Team();
            //teamEntity.TeamId = request.TeamId;
            //teamEntity.TeamName = request.Name;
            //teamEntity.CompanyId = request.CompanyId;
            //teamEntity.UpdatedAt = DateTime.Now;
            //teamEntity.UpdatedBy = request.UserId;
            //teamEntity.TeamMembers = new List<TeamMember>();
            //teamEntity.TeamMembers = TeamFunctions.MergeMemberAndApprover(request.TeamId, request.Members, request.Approvers);

            //using (_unitOfWork)
            //{
            //    _unitOfWork.Begin();
            //    var affectedRow = await _unitOfWork.TeamMemberRepository.RemoveAsync(request.TeamId);

            //    affectedRow = await _unitOfWork.TeamMemberRepository.SetAsNonMemberToOtherTeams(request.Members);
            //    affectedRow = await _unitOfWork.TeamMemberRepository.RemoveNotActiveMemberAsync();
            //    affectedRow = await _unitOfWork.TeamRepository.UpdateAsync(teamEntity);
            //    affectedRow = await _unitOfWork.TeamMemberRepository.InsertAsync(teamEntity.TeamMembers);

            //    _unitOfWork.Commit();
            //}

            ////TODO: Fire "teamCreated" event

            //return _mapper.Map<TeamDto>(teamEntity);
        }
    }
}
