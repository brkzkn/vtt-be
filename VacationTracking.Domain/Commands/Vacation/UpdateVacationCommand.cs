using MediatR;
using Newtonsoft.Json;

namespace VacationTracking.Domain.Commands.Vacation
{
    public class UpdateVacationCommand : IRequest<bool>
    {
        [JsonConstructor]
        public UpdateVacationCommand(int companyId, int vacationId, int responsedBy, string status, string note)
        {
            CompanyId = companyId;
            VacationId = vacationId;
            ResponsedBy = responsedBy;
            Status = status;
            Note = note;
        }

        public int CompanyId { get; set; }
        public int VacationId { get; set; }
        public int ResponsedBy { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}
