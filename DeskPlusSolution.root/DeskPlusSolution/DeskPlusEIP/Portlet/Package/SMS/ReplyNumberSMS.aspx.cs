using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ComponentArt.Web.UI;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMS
{
    public partial class ReplyNumberSMS : Base.BasePagePortlet
    {
        public int CompanyID;
        public int UserID;

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

            Component.SMS.SuremReplyPhoneInfo replyInfo = new SuremReplyPhoneInfo();
            Component.SMS.ReplyPhoneDAL bllReply = new ReplyPhoneDAL();
            DataSet ds = bllReply.GetReplyPhone(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0);

            if (ds.Tables[0] != null)
            {
                DataView dv = ds.Tables[0].DefaultView;
                string textValue = String.Empty;
                string keyValue = String.Empty;
                for (int i = 0; i < dv.Count; i++)
                {
                    textValue = dv[i]["PhoneNumber1"].ToString() + "-" + dv[i]["PhoneNumber2"].ToString() + "-" + dv[i]["PhoneNumber3"].ToString();
                    keyValue = textValue + "," + dv[i]["SuremReplyPhoneID"].ToString();
                    ReqList.Items.Add(new ListItem(textValue, keyValue));                    
                }

                if (ReqList.Items.Count > 0)
                {
                    Component.SMS.SuremReplyPhoneBaseInfo be = new SuremReplyPhoneBaseInfo();
                    Component.SMS.ReplyPhoneBaseInfoDAL bll = new ReplyPhoneBaseInfoDAL();
                    DataSet dsReplyBase = bll.GetReplyPhoneBase(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                    if (dsReplyBase.Tables[0] != null)
                    {
                        string phoneNumber = String.Empty;
                        foreach (DataRow dbRow in dsReplyBase.Tables[0].Rows)
                        {
                            phoneNumber = dbRow["ReplyPhoneNumber"].ToString() + "," + dbRow["SuremReplyPhoneID"].ToString();
                        }

                        if (!String.IsNullOrEmpty(phoneNumber))
                        {
                            ReqList.SelectedValue = phoneNumber;
                        }
                    }
                }
            }
        }




        /// <summary>
        /// 회신 전화번호 삭제
        /// </summary>
        /// <param name="_param">The _param.</param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string AjaxDelReply(string _param)
        {
            string rtn = "0";
            //회사이이디,사용자아이디,키값
            if (_param.Length > 0)
            {
                string[] paramStr = _param.Split(',');
                Component.SMS.SuremReplyPhoneInfo replyInfo = new SuremReplyPhoneInfo();
                Component.SMS.ReplyPhoneDAL bllReply = new ReplyPhoneDAL();
                replyInfo.CompanyID = Convert.ToInt32(paramStr[0]);
                replyInfo.UserID = Convert.ToInt32(paramStr[1]);
                replyInfo.SuremReplyPhoneID = Convert.ToInt32(paramStr[2]);

                //삭제후 insert 함!!
                if (bllReply.Del(replyInfo))
                {
                    Component.SMS.SuremReplyPhoneBaseInfo be = new SuremReplyPhoneBaseInfo();
                    Component.SMS.ReplyPhoneBaseInfoDAL bll = new ReplyPhoneBaseInfoDAL();
                    be.SuremReplyPhoneID = Convert.ToInt32(paramStr[2]);
                    be.UserID = Convert.ToInt32(paramStr[1]);

                    if (bll.Del(be))
                    {
                        rtn = "1";
                    }
                }
            }

            return rtn;
        }        
    }
}
