using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Common
{
    public partial class AttachView : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string attachName = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Common.AttachView).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadAttach();
            }
        }

        /// <summary>
        ///작성자   : famo23
        ///작성일자 : 2011-12-19
        ///작성목적 : mht 첨부파일 열기
        /// </summary>
        private void LoadAttach()
        {
            attachName = GetDecryptParam("AttachName", "0");
            attachName = Server.UrlDecode(attachName);

            string filePath = Server.MapPath(FilePath + "/" + CompanyID + "/users/" + UserID + "/Spool/Mobile/" + attachName);

            Helper.MailParser.MailMessage mailMessage = new Zeospace.DeskPlusMobile.Web.Helper.MailParser.MailMessage();
            Helper.MailParser.EmlParse emlparse = new Zeospace.DeskPlusMobile.Web.Helper.MailParser.EmlParse();

            mailMessage = null;
            mailMessage = emlparse.Mmsg;

            mailMessage.Full_File_Name = filePath;

            emlparse.Start_Parsing(); //Eml Parsing 시작

            contentHTML.InnerHtml = mailMessage.Contents_Html;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        public string Today { get; set; }


    }
}