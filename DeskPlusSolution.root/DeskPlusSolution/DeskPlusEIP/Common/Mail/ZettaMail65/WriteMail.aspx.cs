using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.Web.Mail.ZettaMail65
{
    public partial class WriteMail : Base.BasePageCommon
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MailServerIP = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");
            ToMail = GetDecryptParam("toMail", "").Trim().TrimEnd(';');
        }

        /// <summary>
        /// 수신자 메일 주소
        /// </summary>
        public string ToMail { get; set; }
        public string MailServerIP { get; set; }
    }
}