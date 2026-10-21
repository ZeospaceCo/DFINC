using System;

namespace Zeospace.DeskPlus.Board.BE
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class CategoryUserInfo
    {
        public CategoryUserInfo()
        {
        }

        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int UserID { get; set; }

        public int PageSize { get; set; }

        public string ListType { get; set; }

        /// <summary>
        /// BL : 게시판 TL : 임시저장 IL : 중요표시
        /// </summary>
        public string Types { get; set; }
    }
}