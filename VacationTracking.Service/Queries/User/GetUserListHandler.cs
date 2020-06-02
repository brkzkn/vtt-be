using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationTracking.Data.Repository;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.User;
using UserDb = VacationTracking.Domain.Models.User;

namespace VacationTracking.Service.Queries.User
{
    public class GetUserListHandler : IRequestHandler<GetUserListQuery, IEnumerable<UserDto>>
    {
        private readonly IRepository<UserDb> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetUserListHandler(IRepository<UserDb> repository, IMapper mapper, ILogger<GetUserListHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IEnumerable<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.Queryable().Where(x => x.CompanyId == request.CompanyId
                                                              && x.Status == Domain.Enums.UserStatus.Active)
                                                     .ToListAsync();

            List<UserDto> userDtos = new List<UserDto>();
            _logger.LogInformation($"Got a request get users by CompanyId: {request.CompanyId}");

            if (users.Count > 0)
            {
                userDtos.AddRange(users.Select(x => _mapper.Map<UserDto>(x)).ToList());
            }

            return userDtos;
        }
    }
}
