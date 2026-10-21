using System;

namespace Zeospace.DeskPlus.Search.BE.RM
{
    [Serializable]
    public class ListInfo : SearchInfo
    {
        public ListInfo()
        {
        }

        public int Authority { get; set; }

        public int UsableQty { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string DifferMinite { get; set; }

        public string UserImg { get; set; }

        public string Usable { get; set; }

        public string Qty { get; set; }

        public string ManagerUserID { get; set; }

        public string MManyName { get; set; }

        public string GroupStartDate { get; set; }
        
    }
}