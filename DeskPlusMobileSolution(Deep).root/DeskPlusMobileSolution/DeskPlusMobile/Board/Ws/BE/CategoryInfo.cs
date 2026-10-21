using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BE
{
    /// <summary>
    /// 게시판-분류 Entity
    /// </summary>

    [Serializable]
    public class CategoryInfo
    {
        public CategoryInfo() { }
        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ParentCategoryID { get; set; }
        public int CategoryOrder { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string PathID { get; set; }
        public string PathNameBase { get; set; }
        public string PathNameKo { get; set; }
        public string PathNameEn { get; set; }
        public string PathNameJa { get; set; }
        public string PathNameFr { get; set; }
        public string PathNameZhcn { get; set; }
        public string PathNameZhtw { get; set; }
        public int Levels { get; set; }
        public string Explan { get; set; }
        public string OptionShowImage { get; set; }
        public string OptionUseReply { get; set; }
        public string OptionUseShortReply { get; set; }
        public string OptionShowViewer { get; set; }
        public string OptionUseSecure { get; set; }
        public string OptionUseDept { get; set; }
        public string OptionCustomView { get; set; }
        public string OptionVote { get; set; }
        public string OptionVoteView { get; set; }
        public string OptionUseView { get; set; }
        public string OptionAnonymity { get; set; }
        public int OptionAlarm { get; set; }
        public string OptionListType { get; set; }
        public string OptionMobile { get; set; }
        public int TemplateID { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Auth;
        public string CateInfo { get; set; }
    }
}