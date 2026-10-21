using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Portal.Ws.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class BoardInfo
    {
        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public int ListID { get; set; }

        public string Title { get; set; }

        public string ReplyCount { get; set; }

        public int ViewCount { get; set; }

        public int ReadCount { get; set; }

        public string WriteUserInfo { get; set; }

        public string WriteDateAll { get; set; }

        public string DateField { get; set; }

        public string DateValue { get; set; }

        public string SearchField { get; set; }

        public string SearchValue { get; set; }

        public string PageSize { get; set; }

        public string PageSizePageNo { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string OwnerDeptID { get; set; }

        public string OrderBy { get; set; }

        public string TitlStyle { get; set; }

        public string OptionTop { get; set; }

        public int Depth { get; set; }

        public string HeaderName { get; set; }

        public string ShowNew { get; set; }

        public string OptionAnonymity { get; set; }

        public string CategoryName { get; set; }
    }
}