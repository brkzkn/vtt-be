using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Api.Models;
using VacationTracking.Domain.Commands.LeaveType;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.LeaveType;

namespace VacationTracking.Api.Controller
{
    [Route("api/leave-type")]
    [ApiController]
    public class LeaveTypeController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const int _companyId = 1;
        private const int _userId = 1;
        public LeaveTypeController(IMediator mediator) : base(mediator)
        {

        }

        /// <summary>
        /// Get leave type by id
        /// </summary>
        /// <param name="id">Id of leave type</param>
        /// <returns>LeaveType information</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(LeaveTypeDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<LeaveTypeDto>> GetAsync(int id)
        {
            //TODO: Set companyId from logged-in users
            return Single(await QueryAsync(new GetLeaveTypeQuery(id, _companyId)));
        }

        /// <summary>
        /// Get leave type list
        /// </summary>
        /// <returns>List of leaveType information</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<LeaveTypeDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IList<LeaveTypeDto>>> GetListAsync()
        {
            //TODO: Set companyId from logged-in users
            return Single(await QueryAsync(new GetLeaveTypeListQuery(1)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(LeaveTypeDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LeaveTypeDto>> CreateAsync([FromBody]LeaveTypeModel model)
        {
            var request = new CreateLeaveTypeCommand(_companyId,
                                                     _userId,
                                                     model.IsHalfDaysActivated,
                                                     model.IsHideLeaveTypeName,
                                                     model.TypeName,
                                                     model.IsApprovalRequired,
                                                     model.DefaultDaysPerYear,
                                                     model.IsUnlimited,
                                                     model.IsReasonRequired,
                                                     model.IsAllowNegativeBalance,
                                                     model.Color);

            return Single(await CommandAsync(request));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            var request = new DeleteLeaveTypeCommand(id, _companyId);

            return Single(await CommandAsync(request));
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(LeaveTypeDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LeaveTypeDto>> UpdateAsync(int id, [FromBody]LeaveTypeModel model)
        {
            var request = new UpdateLeaveTypeCommand(_companyId,
                                                     _userId,
                                                     id,
                                                     model.IsHalfDaysActivated,
                                                     model.IsHideLeaveTypeName,
                                                     model.TypeName,
                                                     model.IsApprovalRequired,
                                                     model.DefaultDaysPerYear,
                                                     model.IsUnlimited,
                                                     model.IsReasonRequired,
                                                     model.IsAllowNegativeBalance,
                                                     model.Color,
                                                     model.IsActive);

            return Single(await CommandAsync(request));
        }

    }
}