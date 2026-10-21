using System;

namespace Zeospace.DeskPlus.SNS.BE
{
    [Serializable]
    public class FolderUserInfo
    {
        public FolderUserInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public string Types { get; set; }

        public int PageSize { get; set; }

        public DateTime WriteDate { get; set; }
    }
}