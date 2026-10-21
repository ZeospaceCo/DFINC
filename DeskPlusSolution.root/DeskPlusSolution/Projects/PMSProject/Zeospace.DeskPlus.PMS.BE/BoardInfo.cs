using System;
using System.Collections;

namespace Zeospace.DeskPlus.PMS.BE
{
    [Serializable]
    public class BoardInfo : PagingInfo
    {
        public BoardInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ProjectID { get; set; }

        public int MenuID { get; set; }

        public int BoardID { get; set; }

        public int GroupID { get; set; }

        public int ListOrder { get; set; }

        public int Depth { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public string MobileContent { get; set; }

        public int ReadCount { get; set; }

        public int Gubun { get; set; }

        public int DeptCodeID { get; set; }

        public string NameBase { get; set; }

        public string NameKo { get; set; }

        public string NameEn { get; set; }

        public string NameJa { get; set; }

        public string NameFr { get; set; }

        public string NameZhcn { get; set; }

        public string NameZhtw { get; set; }

        public string ManyName { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public string WriteDateStr { get; set; }

        #region 최초 작성자의 부서, 직급, 직책

        public string DeptBase { get; set; }

        public string DeptKo { get; set; }

        public string DeptEn { get; set; }

        public string DeptJa { get; set; }

        public string DeptFr { get; set; }

        public string DeptZhcn { get; set; }

        public string DeptZhtw { get; set; }

        public string TitleBase { get; set; }

        public string TitleKo { get; set; }

        public string TitleEn { get; set; }

        public string TitleJa { get; set; }

        public string TitleFr { get; set; }

        public string TitleZhcn { get; set; }

        public string TitleZhtw { get; set; }

        public string DutyBase { get; set; }

        public string DutyKo { get; set; }

        public string DutyEn { get; set; }

        public string DutyJa { get; set; }

        public string DutyFr { get; set; }

        public string DutyZhcn { get; set; }

        public string DutyZhtw { get; set; }

        #endregion 최초 작성자의 부서, 직급, 직책

        /// <summary>
        /// SpoolPath : Gets or sets the spool path.
        /// </summary>
        /// <value></value>
        public string HiddenSpoolPath { get; set; }

        /// <summary>
        /// 첨부 파일 리스트
        /// </summary>
        public string HiddenFileList { get; set; }

        /// <summary>
        /// 저장된 첨부파일 리스트
        /// </summary>
        public string HiddenSaveFileList { get; set; }

        /// <summary>
        /// ListViewers of BE 담을 property(조회, 수정내역)
        /// </summary>
        public ArrayList ListViewerList { get; set; }

        /// <summary>
        /// AttachFiles of BE 담을 property (첨부파일)
        /// </summary>
        /// <value></value>
        public ArrayList AttachFileList { get; set; }

        /// <summary>
        /// ShortReplies of BE 담을 property (짧은 답글)
        /// </summary>
        /// <value></value>
        public ArrayList ShortReplyList { get; set; }

        // 기타
        public string Row { get; set; }

        public string ShowNew { get; set; }

        public int Filecount { get; set; }

        public int ReplyCount { get; set; }
    }
}