using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.User;

namespace VacationTracking.Service.Queries.User
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetUserHandler(IMapper mapper, ILogger<GetUserHandler> logger)
        {
            _userRepository = null; // userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var user = await _userRepository.GetAsync(request.CompanyId, request.UserId);

            if (user != null)
            {
                _logger.LogInformation($"Got a request get userId: {user.UserId}");
                var userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }

            // TODO: throw exception not found
            return null;
        }
    }
}
