using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    public class OpenShareInfo
    {
        public OpenShareInfo()
        {
        }

        public string Kind { get; set; }
        public int UID { get; set; }
        public string Name { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
    }
}
