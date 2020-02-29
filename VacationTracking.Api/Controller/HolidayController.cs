using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VacationTracking.Api.Models;
using VacationTracking.Domain.Commands.Holiday;
using VacationTracking.Domain.Dtos;

namespace VacationTracking.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ApiControllerBase
    {
        // TODO: User should has admin or owner permission
        //private const string _companyId = "3aba39de-386a-4b1c-b42e-262549ed11e0";
        private const string _companyId = "3e4a39de-386a-4b1c-b42e-262549ed11e0";
        private const string _userId = "739bc9fa-dfec-4757-80ae-371f7e6a3af6";

        public HolidayController(IMediator mediator) : base(mediator)
        {

        }


        [HttpPost]
        [ProducesResponseType(typeof(HolidayDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<HolidayDto>> CreateTeamAsync([FromBody]HolidayModel model)
        {
            Guid companyId = new Guid(_companyId);
            Guid userId = new Guid(_userId);

            var request = new CreateHolidayCommand(companyId, 
                                                   userId, 
                                                   model.Teams, 
                                                   model.EndDate, 
                                                   model.StartDate, 
                                                   model.Name, 
                                                   model.IsForAllTeams, 
                                                   model.IsFullDay);

            return Single(await QueryAsync(request));
        }

    }
}