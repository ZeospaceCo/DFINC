using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class RatingInfo
    {
        public RatingInfo()
        {
        }

        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        public int RatingID { get; set; }

        public int Rating { get; set; }

        public string Comments { get; set; }

        public int WriteUserID { get; set; }

        public int DeptCodeID { get; set; }

        public DateTime WriteDate { get; set; }

        public string DeptName { get; set; }

        public string UserName { get; set; }

        public string WriteDateStr { get; set; }
    }
}