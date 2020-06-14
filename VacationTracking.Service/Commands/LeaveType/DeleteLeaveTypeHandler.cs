using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.LeaveType;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Exceptions;
using LeaveTypeDb = VacationTracking.Domain.Models.LeaveType;

namespace VacationTracking.Service.Commands.LeaveType
{
    public class DeleteLeaveTypeHandler : IRequestHandler<DeleteLeaveTypeCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<LeaveTypeDb> _repository;

        public DeleteLeaveTypeHandler(IUnitOfWork unitOfWork,
                                      IRepository<LeaveTypeDb> repository,
                                      ILogger<DeleteLeaveTypeHandler> logger,
                                      IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Queryable().SingleOrDefaultAsync(x => x.CompanyId == request.CompanyId
                                                                              && x.LeaveTypeId == request.LeaveTypeId
                                                                              && x.IsDeleted == false);

            if (entity == null)
                throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"LeaveTypeId: {request.LeaveTypeId} is not found.", 404);
            if (entity.IsDefault)
                throw new VacationTrackingException(ExceptionMessages.DefaultLeaveTypeCannotDelete, $"LeaveType {entity.LeaveTypeName} is default and cannot be deleted", 400);

            entity.IsDeleted = true;
            _repository.Update(entity);
            int affectedRow = await _unitOfWork.SaveChangesAsync();

            return affectedRow > 0;
        }
    }
}
