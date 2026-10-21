using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlus.Pim.Web.GoogleCalendar.v3.CalendarIntergrator
{
    public interface ICalIntegrator
    {
        object Authorize(HttpRequest Request, string accountName);
    }
}