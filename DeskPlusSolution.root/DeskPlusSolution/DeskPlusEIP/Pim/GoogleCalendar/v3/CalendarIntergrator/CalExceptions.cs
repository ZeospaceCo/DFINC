using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.Web.GoogleCalendar.v3.CalendarIntergrator
{
    /// <summary>
    /// Unspecified error. Called when an unforeseen.
    /// </summary>
    public class UnknownException : Exception
    {
        public UnknownException() : base("Caused undefined error.") { }
        public UnknownException(string message) : base("Caused undefined error.", new Exception(message)) { }
        public UnknownException(string message, Exception innerException) { }
    }

    /// <summary>
    /// The error that is caused when a user rejected the invitation.
    /// </summary>
    public class UserRejectException : Exception
    {
        public UserRejectException() : base("User rejected the invitation.") { }
        public UserRejectException(string message) : base(message) { }
        public UserRejectException(string message, Exception innerException) { }
    }
}
