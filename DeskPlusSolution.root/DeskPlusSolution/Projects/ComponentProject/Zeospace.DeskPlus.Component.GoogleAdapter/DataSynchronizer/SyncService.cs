using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Collections;

namespace Zeospace.DeskPlus.Component.GoogleAdapter.DataSynchronizer
{
    public class SyncService
    {
        SyncManager<GenericEntities.GenericEvent> eventManager = new SyncManager<GenericEntities.GenericEvent>();
        public string LastUpdateDate;
        

        public SyncService(string _AppName, string _Email, string _pass)
        {
            //Initialize Event Manager
            eventManager.Name = "..Event Synchronizer";
            eventManager.Source1 = new GoogleEventsSyncDataSource(_AppName,_Email,_pass);
        }

        public IList SynchronizeEvents(DateTime synctime)
        {
            IList list = eventManager.Synchronize(synctime).ToList();
            return list;
        }
    }
}
