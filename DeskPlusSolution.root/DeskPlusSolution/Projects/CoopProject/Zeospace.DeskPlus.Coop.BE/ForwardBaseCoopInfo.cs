using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ForwardBaseCoopInfo
    {
        public int CompanyID { get; set; }
        public int ForwardListID { get; set; }        
        public int ForwardBaseCoopID { get; set; }
        public int ListID { get; set; }
        public int GroupID { get; set; }
        public int BaseCoopID { get; set; }
        public int BaseOrder { get; set; }        

        //제목
        public string Subject { get; set; }
    }
}
