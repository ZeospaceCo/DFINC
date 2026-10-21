using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    /// <summary>
    /// 작업
    /// </summary>
    [Serializable]
    public class WorkInfo
    {
        public WorkInfo()
        {
        }

        public int CompanyID { get; set; }

        /// <summary>
        /// 원본글작성자 아이디
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 댓글작성자 아이디
        /// </summary>
        public int WriteUserID { get; set; }

        public string Gubun { get; set; }

        public string GubunTitle { get; set; }

        public string PathName { get; set; }

        public int ReplyCount { get; set; }

        public int ReplyUserCount { get; set; }

        public string ReplyUserStr { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the reg date.
        /// </summary>
        /// <value>
        /// The reg date.
        /// </value>
        public string RegDate { get; set; }

        /// <summary>
        /// new 이미지 보이기 여부
        /// </summary>
        /// <value>
        /// The show new.
        /// </value>
        public string ShowNew { get; set; }

        public string AccountTag { get; set; }

        public int GroupID { get; set; }

        /// <summary>
        /// 공지여부
        /// </summary>
        public int OptionTop { get; set; }

        public string StyleType { get; set; }

        public string SectionCount { get; set; }

        public string StateStr { get; set; }
    }
}