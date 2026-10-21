using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Zeospace.DeskPlus.Pim.Web.GoogleCalendar.v3.CalendarIntergrator;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;

namespace Zeospace.DeskPlus.Pim.Web.GoogleCalendar.v3
{
    public abstract class CalIntegrator : ICalIntegrator
    {
        //
        //  Properties

        protected static string
            CLIENT_ID = WebConfigurationManager.AppSettings["GoogleClientID"].ToString(),
            CLIENT_SECRET = WebConfigurationManager.AppSettings["GoogleClientSecret"].ToString(),
            API_KEY = WebConfigurationManager.AppSettings["GoogleAPIKey"].ToString(),
            APPLICATION_NAME = WebConfigurationManager.AppSettings["GoogleApplicationName"].ToString(),
            APICAL_STORAGE = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["GoogleStorage"].ToString());

        [CLSCompliantAttribute(false)]
        protected static GoogleAuthorizationCodeFlow
            CodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = CLIENT_ID,
                    ClientSecret = CLIENT_SECRET
                },
                Scopes = new[] { CalendarService.Scope.Calendar },

                //DataStore = new FileDataStore(APICAL_STORAGE)
                DataStore = new MyFileDataStore(APICAL_STORAGE),
            });

        [CLSCompliantAttribute(false)]
        public CalendarService CalendarService;

        //
        //  Ctor & init

        //
        //  Inherit methods from Interface

        public abstract object Authorize(HttpRequest Request, string accountName);

        //
        //  Calendar List methods

        public abstract object GetCalendarList();

        public abstract object GetCalendar(string calendarId);

        public abstract object GetEvents(string calendarId, DateTime? timeMin = null, DateTime? timeMax = null);
    }
}