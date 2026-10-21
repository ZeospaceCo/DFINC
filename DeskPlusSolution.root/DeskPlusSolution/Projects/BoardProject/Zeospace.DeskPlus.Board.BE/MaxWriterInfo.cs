using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class MaxWriterInfo : Common.BE.DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public string PhotoImage { get; set; }
        public string Name { get; set; }
        public int Cnt { get; set; }
    }
}
