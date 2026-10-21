using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class BaseAppInfo 
    {
        public BaseAppInfo() { }

        public int CompanyID { get; set; }
        public int BaseAppID { get; set; }
        public int ListID { get; set; }
        public int GroupID { get; set; }  
        public int DocumentID { get; set; }        
        public string Name { get; set; }
        public string IsLegacy { get; set; }
        public string Path { get; set; }
        public int BaseOrder { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
