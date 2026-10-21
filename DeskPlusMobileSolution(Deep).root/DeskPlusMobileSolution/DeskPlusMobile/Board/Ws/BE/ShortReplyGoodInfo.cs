using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BE
{
    public class ShortReplyGoodInfo
    {
        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public int ShortReplyID { get; set; }
        public int UserID { get; set; }
        public int Good { get; set; }
        public int Bad { get; set; }
    }
}