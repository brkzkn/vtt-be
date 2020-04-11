using MediatR;
using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Commands.Vacation
{
    public class UpdateVacationCommand : IRequest<bool>
    {
        [JsonConstructor]
        public UpdateVacationCommand(Guid companyId, Guid vacationId, Guid responsedBy, string status, string response)
        {
            CompanyId = companyId;
            VacationId = vacationId;
            ResponsedBy = responsedBy;
            Status = status;
            Response = response;
        }

        public Guid CompanyId { get; set; }
        public Guid VacationId { get; set; }
        public Guid ResponsedBy { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
    }
}
