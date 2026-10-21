using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.Web.Mail.ZettaMail65.Address
{
    public partial class Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            string group_sn = Request["group_sn"].ToString();
            string userName = Server.UrlDecode(Request["userName"].ToString());
            string email = Request["Email"].ToString();
            string mobilePhone = Request["mobilePhone"].ToString();
            int user_sn = Convert.ToInt32(Request["user_SN"].ToString());

            string referer = "";

            if (Request.UrlReferrer != null)
            {
                referer = Request.UrlReferrer.ToString();
            }

            Zeospace.DeskPlus.Common.DAL.MailZetta65 dal = new DAL.MailZetta65();

            int companyID = Convert.ToInt32(dal.EipCompanyID(user_sn));

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("companyID : " + companyID, AppSettingType.MAIL);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("group_sn  : " + group_sn, AppSettingType.MAIL);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("userName  : " + userName, AppSettingType.MAIL);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("email  : " + email, AppSettingType.MAIL);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("mobilePhone  : " + mobilePhone, AppSettingType.MAIL);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("user_sn  : " + user_sn, AppSettingType.MAIL);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("referer  : " + referer, AppSettingType.MAIL);

            bool insert = dal.InsertAddress(companyID, Abs(group_sn).ToString(), userName, email, mobilePhone, user_sn);

            if (insert)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("insert 성공 : " + insert, AppSettingType.MAIL);
            }

        }

        /// <summary>
        /// -이면 +, 
        /// </summary>
        /// <param name="_categoryID"></param>
        /// <returns></returns>
        private int Abs(string _categoryID)
        {
            int number = _categoryID.ToNumber<int>();
            return (number > 0) ? number : -number;

        }
    }
}