using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class LinkURLInfo
    {
        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public int LinkURLID { get; set; }
        public string URLName { get; set; }
        public string URL { get; set; }
    }
}
