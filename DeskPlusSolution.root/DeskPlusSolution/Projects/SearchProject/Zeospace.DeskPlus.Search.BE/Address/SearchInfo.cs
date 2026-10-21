using System;

namespace Zeospace.DeskPlus.Search.BE.Address
{
    [Serializable]
    public class SearchInfo
    {
        public SearchInfo()
        {
        }

        public string Module { get; set; }

        public string Type { get; set; }

        public string ID { get; set; }

        public int CompanyID { get; set; }

        public string GroupID { get; set; }

        public int CategoryID { get; set; }

        public int ObjectID { get; set; }

        public int UserID { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

        public string DeptName { get; set; }

        public string Company { get; set; }

        public string MobilePhone { get; set; }

        public string OfficePhone { get; set; }

        public string Title { get; set; }

        public string Duty { get; set; }

        public string Memo { get; set; }

        public string WriteDate { get; set; }

        public string NameBase { get; set; }

        public string DeptBase { get; set; }
    }
}