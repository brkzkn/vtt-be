using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Team;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using TeamDb = VacationTracking.Domain.Models.Team;

namespace VacationTracking.Service.Commands.Team
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TeamDb> _repository;

        public DeleteTeamHandler(IUnitOfWork unitOfWork, IRepository<TeamDb> repository, ILogger<DeleteTeamHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Queryable().SingleOrDefaultAsync(x => x.CompanyId == request.CompanyId 
                                                                              && x.TeamId == request.TeamId);

            if (entity == null)
                throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"TeamId: {request.TeamId} is not found", 404);

            if (entity.IsDefault)
                throw new VacationTrackingException(ExceptionMessages.DefaultTeamCannotDelete, $"TeamId: {request.TeamId} is default and cannot delete", 400);

            _repository.Delete(entity);
            int affectedRow = await _unitOfWork.SaveChangesAsync();

            return affectedRow > 0;
        }
    }
}
