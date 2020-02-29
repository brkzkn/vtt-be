using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.IRepositories;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.User;

namespace VacationTracking.Service.Queries.User
{
    public class GetUserListHandler : IRequestHandler<GetUserListQuery, IList<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetUserListHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserListHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IList<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            // TODO: Check user permission. 
            // User should has owner or admin permission
            var users = await _userRepository.GetListAsync(request.CompanyId);

            if (users != null)
            {
                _logger.LogInformation($"Got a request get users by CompanyId: {request.CompanyId}");
                List<UserDto> userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    userDtos.Add(_mapper.Map<UserDto>(user));
                }
                return userDtos;
            }

            //throw exception
            return null;
        }
    }
}
