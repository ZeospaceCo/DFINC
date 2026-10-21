using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Component.SMS;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMSNew
{
    public partial class SendSMSP : Base.BasePagePortlet
    {
        public string ReplyNumberList = "";
        public string TopReplyNumber = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드

            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.SMSNew.SendSMSP).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            StringBuilder sb = new StringBuilder();

            Component.SMS.SuremReplyPhoneInfo be = new SuremReplyPhoneInfo();
            Component.SMS.ReplyPhoneDAL bll = new ReplyPhoneDAL();
            DataSet ds = bll.GetReplyPhone(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["Base"].ToString().Equals("1"))
                {
                    TopReplyNumber = dr["PhoneNumber1"].ToString() + dr["PhoneNumber2"].ToString() + dr["PhoneNumber3"].ToString();
                }

                sb.Append("<tr><td width='83' align='left' onmouseout='mouseOut(this);' onmouseover='mouseOver(this);'");
                sb.Append(" onclick=ClickBoardList('");
                sb.Append(dr["PhoneNumber1"].ToString() + dr["PhoneNumber2"].ToString() + dr["PhoneNumber3"].ToString());
                sb.Append("');>&nbsp;");
                sb.Append(dr["PhoneNumber1"].ToString() + dr["PhoneNumber2"].ToString() + dr["PhoneNumber3"].ToString());
                sb.Append("</td></tr>");
            }

            //회신번호
            ReplyNumberList = sb.ToString();

            //메신저에서 넘어온 전화번호
            ParamPhoneNumber = GetParamPhoneNumber();

            SendNumber.Value = CurrentUserInfo.MobilePhone;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   :
        /// 작성자     : 홍석기
        /// 최초작성일 : 2008-12-24
        /// </summary>
        /// <returns></returns>
        private string GetParamPhoneNumber()
        {
            //Zeospace.DeskPlus.Component.SMS.DefaultDeptDAL dept = new DefaultDeptDAL();
            StringBuilder sbPhoneNumber = new StringBuilder();

            //sendsms.aspx?phonenumber=suy:ZEOSPACE.COM;good:ZEOSPACE.COM;vcat1;nodazi81:ZEOSPACE.COM
            string logInIDs = GetParm("phonenumber", "");
            if (logInIDs.Length > 0)
            {
                StringBuilder sbUserList = new StringBuilder();

                foreach (string login in logInIDs.Split(';'))
                {
                    if (login.Length > 0)
                    {
                        if (login.IndexOf(':') != -1)
                        {
                            sbUserList.Append("'" + login.Split(':')[0] + "'");
                        }
                        else
                        {
                            sbUserList.Append("'" + login + "'");
                        }

                        sbUserList.Append(",");
                    }
                }

                Zeospace.DeskPlus.Account.DAL.User userDal = new Account.DAL.User();
                List<UserInfo> userList = userDal.GetUserByLoginList(CurrentUserInfo.CompanyID, sbUserList.ToString().TrimEnd(','), true);

                foreach (UserInfo be in userList)
                {
                    be.NameBase = this.GetLangName(be.NameBase, be.NameKo, be.NameEn, be.NameJa, be.NameFr, be.NameZhCn, be.NameZhTw, PreLang);
                    be.MobilePhone = System.Text.RegularExpressions.Regex.Replace(be.MobilePhone.Trim(), @"[-~.,)(]{1}", "");
                    be.MobilePhone.Replace(" ", "");
                    sbPhoneNumber.Append(be.NameBase + "|" + be.MobilePhone + "$");
                }
            }
            //이름|전화번호$이름|전화번호$
            return sbPhoneNumber.ToString().TrimEnd('$');
        }

        #region 속성

        public string Year
        {
            get
            {
                return System.DateTime.Now.Year.ToString();
            }
        }

        public string Month
        {
            get
            {
                return System.DateTime.Now.Month.ToString();
            }
        }

        public string Day
        {
            get
            {
                return System.DateTime.Now.Day.ToString();
            }
        }

        public string Hour
        {
            get
            {
                return System.DateTime.Now.Hour.ToString();
            }
        }

        public string Minite
        {
            get
            {
                return System.DateTime.Now.Minute.ToString();
            }
        }

        public string Second
        {
            get
            {
                return System.DateTime.Now.Second.ToString();
            }
        }

        public string ParamPhoneNumber
        {
            get { return GetViewStateString("ParamPhoneNumber", ""); }

            set { ViewState["ParamPhoneNumber"] = value; }
        }

        #endregion 속성
    }
}