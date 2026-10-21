using System;

namespace Zeospace.DeskPlus.Search.BE.Board
{
    [Serializable]
    public class ListInfo : SearchInfo
    {
        public ListInfo()
        {
        }

        public int GroupID { get; set; }

        public int ListOrder { get; set; }

        public int Depth { get; set; }

        public string ContentHTML { get; set; }

        public string ContentTEXT { get; set; }

        public string DeptName { get; set; }

        public string TitleName { get; set; }

        public string DutyName { get; set; }

        public string UserName { get; set; }
    }
}