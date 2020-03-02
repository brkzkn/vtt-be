using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data;
using VacationTracking.Domain.Commands.LeaveType;

namespace VacationTracking.Service.Commands.LeaveType
{
    public class DeleteLeaveTypeHandler : IRequestHandler<DeleteLeaveTypeCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLeaveTypeHandler(IUnitOfWork unitOfWork, ILogger<DeleteLeaveTypeHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            using (_unitOfWork)
            {
                var affectedRows = await _unitOfWork.LeaveTypeRepository.RemoveAsync(request.LeaveTypeId, request.CompanyId);

                return affectedRows > 0;
            }
        }
    }
}
