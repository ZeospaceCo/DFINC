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
    public partial class OpenContentSMSProc : Base.BasePagePortlet
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.SMS.OpenContentSMSProc).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 문자 저장 및 삭제
		/// 작성자     : 심재운
		/// 최초작성일 : 2008-02-05
        /// </summary>
        private void LoadData()
        {           

            string gubun = GetParm("gubun","");

            try
            {

                Component.SMS.SMSHelper dal = new SMSHelper();
                if (gubun.ToLower() == "insert")
                {
                    Component.SMS.SMSInfo smsInfo = new SMSInfo();
                    smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                    smsInfo.Content         = Server.UrlDecode(GetParm("content",""));
                    smsInfo.SMSNoticeTemplateID = 0;
                    smsInfo.WriteUserID = CurrentUserInfo.UserID;
                    smsInfo.WriteUserName = CurrentUserInfo.Name;



                    dal.AddSMSTemplateNotice(smsInfo);
                }
                else if(gubun.ToLower() == "delete")
                {
                    Component.SMS.SMSInfo smsInfo = new SMSInfo();
                    smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                    smsInfo.Content         = Server.UrlDecode(GetParm("content",""));
                    smsInfo.SMSNoticeTemplateID = Convert.ToInt32(GetParm("id","0"));
                    smsInfo.WriteUserID = CurrentUserInfo.UserID;
                    smsInfo.WriteUserName = CurrentUserInfo.Name;

                    dal.DelSMSTemplateNotice(smsInfo);
                }
            }            
            catch (System.Data.SqlClient.SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LoadData() : " + errMsg);
                throw;
            }            
        }
    }
}
