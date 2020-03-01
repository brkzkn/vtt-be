﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationTracking.Domain.Dtos;
using VacationTracking.Domain.Queries.LeaveType;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const string _companyId = "3e4a39de-386a-4b1c-b42e-262549ed11e0";
        private const string _userId = "739bc9fa-dfec-4757-80ae-371f7e6a3af6";
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
        public async Task<ActionResult<LeaveTypeDto>> GetHolidayAsync(Guid id)
        {
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid(_companyId);

            return Single(await QueryAsync(new GetLeaveTypeQuery(id, companyId)));
        }

        /// <summary>
        /// Get leave type list
        /// </summary>
        /// <returns>List of leaveType information</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<LeaveTypeDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IList<LeaveTypeDto>>> GetLeaveTypeAsync()
        {
            //TODO: Set companyId from logged-in users
            Guid companyId = new Guid(_companyId);

            return Single(await QueryAsync(new GetLeaveTypeListQuery(companyId)));
        }

    }
}