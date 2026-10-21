using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class ListViewersInfo : Common.BE.DeptCodeInfo
    {
        public ListViewersInfo()
        {
        }

        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        public DateTime CRUDDate { get; set; }

        public string CRUDDateStr { get; set; }

        public string CRUDType { get; set; }

        public int UserID { get; set; }

        public int DeptCodeID { get; set; }

        public string UserName { get; set; }
    }
}