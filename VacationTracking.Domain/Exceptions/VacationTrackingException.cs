using System;

namespace VacationTracking.Domain.Exceptions
{
    public class VacationTrackingException : Exception
    {
        private int _code;
        private string _description;

        public int Code
        {
            get => _code;
        }
        public string Description
        {
            get => _description;
        }

        public VacationTrackingException(string message, string description, int code) : base(message)
        {
            _code = code;
            _description = description;
        }
    }
}
