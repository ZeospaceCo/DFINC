using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Task.BE
{
    [Serializable]
    public class BaseTaskInfo
    {
        public BaseTaskInfo() { }

        public int CompanyID {get;set;}
        public int BaseTaskID { get; set; }                
        public int ListID {get;set;}
        public int GroupID { get; set; }          
        public int BaseID { get; set; }
        public string Name  {get;set;}
        public int BaseOrder { get; set; }
        public int WriteUserID {get;set;}
        public DateTime WriteDate { get; set; }        

    }
}
