using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class TemplateInfo
    {
        public TemplateInfo() { }

        public int CompanyID { get; set; }
        public int ItemSubID { get; set; }
        public int TemplateID { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
