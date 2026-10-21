using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class XmlInfo
    {       
        public string KeyID { get; set; }

        public string KeyText { get; set; }

        public string KeyValue { get; set; }    
    }
}