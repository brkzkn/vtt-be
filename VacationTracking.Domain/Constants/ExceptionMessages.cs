namespace VacationTracking.Domain.Constants
{
    public static class ExceptionMessages
    {
        public const string ItemNotFound = "ItemNotFound";
        public const string HolidayAlreadyExistForSameDate = "HolidayAlreadyExistForSameDate";
        public const string LeaveTypeNameAlreadyExist = "LeaveTypeNameAlreadyExist";
        public const string DefaultLeaveTypeCannotDelete = "DefaultLeaveTypeCannotDelete";
        public const string InvalidUserId = "InvalidUserId";
        public const string TeamNameAlreadyExist = "TeamNameAlreadyExist";
        public const string DefaultTeamCannotDelete = "DefaultTeamCannotDelete";
        public const string VacationDateIsNotValid = "VacationDateIsNotValid";
        public const string VacationLeaveTypeIsNotValid = "VacationLeaveTypeIsNotValid";
        public const string LeaveTypeDoesNotAllowNegativeBalance = "LeaveTypeDoesNotAllowNegativeBalance";
        public const string LeaveTypeDoesNotAllowHalfDays = "LeaveTypeDoesNotAllowHalfDays";
        public const string VacationReasonIsRequired = "VacationReasonIsRequired";
    }
}
