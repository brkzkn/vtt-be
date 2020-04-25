using MediatR;
using Newtonsoft.Json;
using System;

namespace VacationTracking.Domain.Commands.Vacation
{
    public class UpdateVacationCommand : IRequest<bool>
    {
        [JsonConstructor]
        public UpdateVacationCommand(Guid companyId, Guid vacationId, Guid responsedBy, string status, string note)
        {
            CompanyId = companyId;
            VacationId = vacationId;
            ResponsedBy = responsedBy;
            Status = status;
            Note = note;
        }

        public Guid CompanyId { get; set; }
        public Guid VacationId { get; set; }
        public Guid ResponsedBy { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}
