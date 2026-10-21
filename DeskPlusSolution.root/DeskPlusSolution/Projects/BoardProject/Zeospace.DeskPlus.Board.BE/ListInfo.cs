using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class ListInfo : Common.BE.DeptCodeInfo
    {
        public ListInfo()
        {
        }

        #region DP_BOARD 외에 프로퍼티

        public string ROW { get; set; }

        public string CA_Title { get; set; }

        public int Filecount { get; set; }

        public int ReplyCount { get; set; }

        public string HeaderBase { get; set; }

        public string HeaderKo { get; set; }

        public string HeaderEn { get; set; }

        public string HeaderJa { get; set; }

        public string HeaderFr { get; set; }

        public string HeaderZhcn { get; set; }

        public string HeaderZhtw { get; set; }

        public string HeaderName { get; set; }

        public string AlarmContent { get; set; }

        public string SMSContent { get; set; }

        public string SMSLoginID { get; set; }

        public string SMSMobilePhone { get; set; }

        public string PathName { get; set; }

        public string HiddenSpoolPath { get; set; }

        public int ImpoCnt { get; set; }

        public string ShowNew { get; set; }

        /// <summary>
        /// 암호화 보기 페이지
        /// </summary>
        public string HrefUrl { get; set; }

        #endregion DP_BOARD 외에 프로퍼티

        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        public int GroupID { get; set; }

        public int ListOrder { get; set; }

        public int Depth { get; set; }

        public string Title { get; set; }

        public string TitleNoHtml { get; set; }

        public string TitleShort { get; set; }

        public string Content { get; set; }

        public string MobileContent { get; set; }

        public string BlogContent { get; set; }

        public int OwnerDeptID { get; set; }

        public string OptionModify { get; set; }

        public string OptionTop { get; set; }

        public int OptionTopDay { get; set; }

        public string OptionAnonymity { get; set; }

        public int ReadCount { get; set; }

        public string ScreenShot { get; set; }

        public int HeaderID { get; set; }

        public int DeptCodeID { get; set; }

        public int ImgCount { get; set; }

        public int WriteUserID { get; set; }

        public string WriteUserName { get; set; }

        public string WriteDate { get; set; }

        public DateTime WriteDateDT { get; set; }

        public int ModifyUserID { get; set; }

        public string ModifyUserName { get; set; }

        public string ModifyDate { get; set; }

        public DateTime ModifyDateDT { get; set; }

        public string PassWord { get; set; }

        public string Count { get; set; }

        public string ScreenShotURL { get; set; }
        public string ScreenShotThumURL { get; set; }

        public string AlarmUser { get; set; }
        public string UseAlarm { get; set; }
    }
}