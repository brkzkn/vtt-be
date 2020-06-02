using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Constants;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Exceptions;
using VacationTracking.Domain.Queries.User;
using UserDb = VacationTracking.Domain.Models.User;

namespace VacationTracking.Service.Queries.User
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IRepository<UserDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetUserHandler(IRepository<UserDb> repository, IMapper mapper, ILogger<GetUserHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.Queryable()
                                        .SingleOrDefaultAsync(x => x.CompanyId == request.CompanyId 
                                                                && x.UserId == request.UserId
                                                                && x.Status == Domain.Enums.UserStatus.Active);

            if (user != null)
            {
                _logger.LogInformation($"Got a request get userId: {user.UserId}");
                var userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }

            throw new VacationTrackingException(ExceptionMessages.ItemNotFound, $"User not found by id {request.UserId}", 404);
        }
    }
}
