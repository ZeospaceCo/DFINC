using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using GenericEntities;

//using GoogleCalendarHelper;
using Google.GData.Calendar;
using Zeospace.DeskPlus.Component.GoogleAdapter.GenericEntities;


namespace Zeospace.DeskPlus.Component.GoogleAdapter
{
    public class GoogleEventsSyncDataSource : DataSynchronizer.ISyncDataSource<GenericEntities.GenericEvent>
    {
        CalendarService service;

       

        public GoogleEventsSyncDataSource(string _AppName,string _Email,string _pass)
        {   
            service = CalendarHelper.CalendarHelper.GetService(_AppName, _Email, _pass);
        }

        #region ISyncDataSource<GenericEvent> Members

        public string Id
        {
            get { return "Google Calander Sync Data Source"; }
        }

        public IEnumerable<GenericEvent> GetItemHeaders(DateTime? lastSyncTime)
        {
            var googleEvents = CalendarHelper.CalendarHelper.GetAllEvents(service, lastSyncTime);
            List<GenericEvent> genericEvents = new List<GenericEvent>();
            foreach (var googleEvent in googleEvents)
            {
                GenericEvent genericEvent = new GenericEvent();
                genericEvent.Title = googleEvent.Title.Text;
                genericEvent.Contents = googleEvent.Content.Content;
                genericEvent.Location = googleEvent.Locations.First().ValueString;
                genericEvent.StartTime = googleEvent.Times.First().StartTime;
                genericEvent.EndTime = googleEvent.Times.First().EndTime;
                genericEvent.UID = googleEvent.Uid.Value;
                genericEvents.Add(genericEvent);
            }
            return genericEvents;
        }

        public void LoadItemContents(IEnumerable<GenericEvent> items)
        {
            //Nothing to load here
        }

        public void WriteItems(IEnumerable<GenericEvent> items)
        {
            foreach (var item in items)
            {
               CalendarHelper.CalendarHelper.AddEvent(service, item.Title, item.Contents, item.Location, item.StartTime, item.EndTime);
            }
        }

        #endregion
    }
}
