using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Team;

namespace VacationTracking.Service.Commands.Team
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeamHandler(IUnitOfWork unitOfWork, ILogger<CreateTeamHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //using (_unitOfWork)
            //{
            //    _unitOfWork.Begin();
            //    var team = await _unitOfWork.TeamRepository.GetAsync(request.TeamId, request.CompanyId);
            //    if (team == null)
            //        throw new ArgumentNullException(nameof(TeamDto));

            //    var affectedRows = await _unitOfWork.TeamMemberRepository.RemoveAsync(request.TeamId);
            //    affectedRows = await _unitOfWork.TeamRepository.RemoveAsync(request.CompanyId, request.TeamId);

            //    _unitOfWork.Commit();
            //    return affectedRows > 0;
            //}
        }
    }
}
