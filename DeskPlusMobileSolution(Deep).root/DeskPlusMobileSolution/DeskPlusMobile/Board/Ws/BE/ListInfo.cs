using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ListInfo
    {       
        public int CompanyID  { get; set; }

        public int CategoryID { get; set; } 

        public int ListID  { get; set; }

        public int GroupID { get; set; }

        public int ListOrder { get; set; }

        public int Depth { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string MobileContent { get; set; }

        public int OwnerDeptID { get; set; }

        public string OptionModify { get; set; }

        public string OptionTop { get; set; }

        public int OptionTopDay { get; set; }

        public string OptionAnonymity { get; set; }

        public string ReplyCount { get; set; }

        public int ViewCount { get; set; }
       
        public int ReadCount  { get; set; }

        public string ScreenShot { get; set; }

        public int HeaderID { get; set; }

        public string WriteUserInfo { get; set; }

        public string WriteDateAll { get; set; }
       
        public string DateField  { get; set; }
       
        public string DateValue  { get; set; }

        public string SearchField  { get; set; }
      
        public string SearchValue  { get; set; }

        public string PageSize  { get; set; }

        public string PageSizePageNo  { get; set; }

        public string StartDate  { get; set; }

        public string EndDate  { get; set; }
        
        public string OrderBy  { get; set; }

        public string TitlStyle { get; set; }

        public string HeaderName { get; set; }

        public string ShowNew { get; set; }

        public int WriteUserID { get; set; }

        public string WriteUserName { get; set; }

        public string WriteDate { get; set; }

        public string PassWord { get; set; }

        public string AlarmContent { get; set; }

        public string SMSContent { get; set; }

        public string SMSLoginID { get; set; }

        public string SMSMobilePhone { get; set; }

        public string FontColor { get; set; }

        public string FontSize { get; set; }

        public string Bold { get; set; }

        public string PopUp { get; set; }

    }
}