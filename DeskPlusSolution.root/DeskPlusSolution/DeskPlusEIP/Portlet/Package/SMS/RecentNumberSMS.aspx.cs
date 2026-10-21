using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMS
{
    public partial class RecentNumberSMS : Base.BasePagePortlet
    {
        public string htmlString = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.SMS.RecentNumberSMS).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {   
            Component.SMS.SuremRecentPhoneInfo be = new SuremRecentPhoneInfo();
            Component.SMS.RecentPhoneDAL bll = new RecentPhoneDAL();

            StringBuilder sb = new StringBuilder();

            DataSet ds = bll.GetRecentPhone(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;

                string suremRecentPhoneID = String.Empty;
                string phoneNumber = String.Empty;
                int num;

                for (int i = 0; i < dv.Count; i++)
                {
                    num = i + 1;
                    suremRecentPhoneID = dv[i]["SuremRecentPhoneID"].ToString();
                    phoneNumber = dv[i]["PhoneNumber1"].ToString() + dv[i]["PhoneNumber2"].ToString() + dv[i]["PhoneNumber3"].ToString();

                    sb.Append("<tr id='recent_tr_" + suremRecentPhoneID + "' onmouseover=inCell(this,'#E8EDFF'); onmouseout=outCell(this,'#FFFFFF'); onclick=ifr_inputCell(this,'#bbbbbb','" + suremRecentPhoneID + "','" + num + "','@" + phoneNumber + "'); ondblclick=ifr_recent_sel_dblclk('" + phoneNumber + "',''); style=cursor:hand;>");
                    sb.Append("<td width=80 style=font-size:8pt;padding-left:3px;>" + dv[i]["PhoneNumber1"].ToString() + "-" + dv[i]["PhoneNumber2"].ToString() + "-" + dv[i]["PhoneNumber3"].ToString() + "</td>");
                    sb.Append("<td width=57 style=font-size:8pt;padding-left:3px;></td>");
                    sb.Append("</tr>");
                }
            }
            else
            {
                sb.Append("<tr>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }


            htmlString = sb.ToString();
        }
    }
}
