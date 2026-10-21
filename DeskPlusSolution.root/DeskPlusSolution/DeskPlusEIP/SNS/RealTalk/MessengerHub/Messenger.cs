using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR;
using Zeospace.DeskPlus.SNS.Web.RealTalk.MessengerHub;


namespace Zeospace.DeskPlus.SNS.Web.RealTalk.MessengerHub
{
    public class Messenger
    {
        private readonly static Lazy<Messenger> _instance = new Lazy<Messenger>(() => new Messenger());
        private readonly ConcurrentDictionary<string, Message> _messages = new ConcurrentDictionary<string, Message>();

        private Messenger()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Messenger Instance
        {
            get
            {
                return _instance.Value;
            }
        }


        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Message> GetAllMessages()
        {
            return _messages.Values;
        }

        /// <summary>
        /// Broads the cast message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void BroadCastMessage(Object message, string group)
        {
            GetClients(group).add(message);
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <returns></returns>
        private static dynamic GetClients(string group)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();

            //return context.Clients.All;
            return context.Clients.Group(group);


            //return context.Clients.All[group];
        }   
    }
}