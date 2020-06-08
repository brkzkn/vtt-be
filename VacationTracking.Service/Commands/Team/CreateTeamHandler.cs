using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Data.Repository.Team;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using TeamDb = VacationTracking.Domain.Models.Team;
using TeamMemberDb = VacationTracking.Domain.Models.TeamMember;
using UserDb = VacationTracking.Domain.Models.User;

namespace VacationTracking.Service.Commands.Team
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, TeamDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TeamDb> _repository;
        private readonly IRepository<UserDb> _userRepository;

        public CreateTeamHandler(IUnitOfWork unitOfWork,
                                 IRepository<TeamDb> repository,
                                 IRepository<UserDb> userRepository,
                                 ILogger<CreateTeamHandler> logger,
                                 IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<TeamDto> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.IsTeamNameExistAsync(request.CompanyId, request.Name))
                throw new VacationTrackingException(ExceptionMessages.TeamNameAlreadyExist, $"TeamName: {request.Name} is already exist", 400);

            var companyUsers = await _userRepository.Queryable()
                                                    .Where(x => x.CompanyId == request.CompanyId)
                                                    .Select(x => x.UserId)
                                                    .ToListAsync();
            if (request.Approvers.Except(companyUsers).Any())
                throw new VacationTrackingException(ExceptionMessages.InvalidUserId, "Some approver(s) is not valid", 400);

            if (request.Members.Except(companyUsers).Any())
                throw new VacationTrackingException(ExceptionMessages.InvalidUserId, "Some member(s) is not valid", 400);

            var teamEntity = new TeamDb()
            {
                TeamName = request.Name,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.UserId,
            };

            foreach (var teamMember in request.Members)
            {
                teamEntity.TeamMembers.Add(new TeamMemberDb()
                {
                    IsApprover = request.Approvers.Contains(teamMember),
                    IsMember = true,
                    UserId = teamMember
                });
            }

            foreach (var approver in request.Approvers)
            {
                var teamMember = teamEntity.TeamMembers.SingleOrDefault(x => x.UserId == approver);
                if (teamMember == null)
                {
                    teamEntity.TeamMembers.Add(new TeamMemberDb()
                    {
                        IsApprover = true,
                        IsMember = false,
                        UserId = approver
                    });
                }
                else
                {
                    teamMember.IsApprover = true;
                }
            }

            _repository.Attach(teamEntity);
            await _unitOfWork.SaveChangesAsync();

            //TODO: Fire "teamCreated" event
            return _mapper.Map<TeamDto>(teamEntity);
        }
    }
}
