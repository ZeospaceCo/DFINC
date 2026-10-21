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
using ComponentArt.Web.UI;
using System.Collections.Generic;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMS
{
    public partial class SendSMS : Base.BasePagePortlet
    {
        public string ReplyNumberList = "";
        public string TopReplyNumber = "";
        public int CompanyID;
        public int UserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.SMS.SendSMS).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

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

            ReplyNumberList = sb.ToString();

            PhoneNumber = GetPhoneNumber();
        }



        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 
        /// 작성자     : 홍석기
        /// 최초작성일 : 2008-12-24
        /// </summary>
        /// <returns></returns>
        private string GetPhoneNumber()
        {
            //Zeospace.DeskPlus.Component.SMS.DefaultDeptDAL dept = new DefaultDeptDAL();
            StringBuilder sbPhoneNumber = new StringBuilder();

            //sendsms.aspx?phonenumber=suy;good;vcat1;nodazi81 
            string logInIDs = GetParm("phonenumber", "");
            if (logInIDs.Length > 0)
            {
                string[] logInIDsArr = logInIDs.Split(';');

                

                #region 미사용
                //DataSet userList = null;
                //if (logInIDsArr.Length > 200)
                //{
                //    if (logInIDsArr.Length > 400)
                //    {
                //        if (logInIDsArr.Length > 600)
                //        {
                //            userList = dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 601, logInIDsArr.Length));
                //            userList.Merge(dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 401, 600)));
                //            userList.Merge(dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 201, 400)));
                //            userList.Merge(dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 0, 199)));
                //        }
                //        else
                //        {
                //            userList = dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 401, logInIDsArr.Length));
                //            userList.Merge(dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 201, 400)));
                //            userList.Merge(dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 0, 199)));
                //        }
                //    }
                //    else
                //    {
                //        userList = dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 201, logInIDsArr.Length));
                //        if (userList.Tables[0].Rows.Count > 0)
                //        {
                //            userList.Merge(dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 0, 200)));
                //        }
                //        else
                //        {
                //            userList = dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 0, 200));
                //        }
                //    }
                //}
                //else
                //{
                //    userList = dept.GetUserList(CompanyID, splitPhoneNumber(logInIDsArr, 0, logInIDsArr.Length));
                //} 
                #endregion

                StringBuilder sbUserList = new StringBuilder();

                foreach (string login in logInIDs.Split(';'))
                {
                    if (login.Length > 0)
                    {
                        sbUserList.Append("'" + login + "'");
                        sbUserList.Append(",");
                    }
                }
                Zeospace.DeskPlus.Account.DAL.User userDal = new Account.DAL.User();
                List<UserInfo> userList = userDal.GetUserByLoginList(CurrentUserInfo.CompanyID, sbUserList.ToString().TrimEnd(','), true);

                
                foreach (UserInfo be in userList)
                {
                    be.NameBase = this.GetLangName(be.NameBase, be.NameKo, be.NameEn, be.NameJa, be.NameFr, be.NameZhCn, be.NameZhTw, PreLang);
                    sbPhoneNumber.Append(be.NameBase + ":" + be.MobilePhone + ";");
                }
            }

            return sbPhoneNumber.ToString().TrimEnd(';');
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 
        /// 작성자     : 홍석기
        /// 최초작성일 : 2008-12-24
        /// </summary>
        /// <param name="_tmpPhoneNumberArr">The _TMP phone number arr.</param>
        /// <param name="_startNumber">The _start number.</param>
        /// <param name="_endNumber">The _end number.</param>
        /// <returns></returns>
        private string splitPhoneNumber(string[] _tmpPhoneNumberArr, int _startNumber, int _endNumber)
        {
            StringBuilder sbLoginID = new StringBuilder();
            sbLoginID.Append("(");
            for (int j = _startNumber; j < _endNumber - 1; j++)
            {
                string[] userPhoneNumberArr = _tmpPhoneNumberArr[j].Split(':');

                if (sbLoginID.ToString() == "(")
                {
                    sbLoginID.Append("'" + userPhoneNumberArr[0].ToString() + "'");
                }
                else
                {
                    sbLoginID.Append(",'" + userPhoneNumberArr[0].ToString() + "'");
                }
            }
            sbLoginID.Append(")");

            return sbLoginID.ToString();
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

        public string PhoneNumber
        {
            get { return GetViewStateString("PhoneNumber", ""); }

            set { ViewState["PhoneNumber"] = value; }
        }

        #endregion

    }
}
