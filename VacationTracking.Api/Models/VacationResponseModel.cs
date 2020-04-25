namespace VacationTracking.Api.Models
{
    public class VacationResponseModel
    {
        public string Status { get; set; }

        /// <summary>
        /// An approver can share note for own response.
        /// </summary>
        public string Note { get; set; }
    }
}
