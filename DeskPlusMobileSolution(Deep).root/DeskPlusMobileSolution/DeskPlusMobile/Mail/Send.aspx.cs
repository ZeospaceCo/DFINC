using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.Mail
{
    public partial class Send : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string action = "";
        public string code = "";
        public string mail_sn = "";
        public string mail = "";
        public string mailID = "";
        public string hearder = "";
        public string MailUserID = "";
        public string MailAddress = "";
        public int pageNo = 0;
        public string history = "";
        public string gubun = "";
        public string ownerID = "";
        public string searchValue = "";
        public string mailDomain = "";
        public int draftListID;
        public string FileCheck;
        public string blockExt;
        public string upFileSize;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Mail.Send).Assembly;	//어셈블리 선언
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

                LoadParam();                
                LoadAuth();
                LoadData();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            action = GetDecryptParam("Action", "");
            code = GetDecryptParam("Code", "");
            mail_sn = GetDecryptParam("Mail_sn", "");
            mail = GetDecryptParam("Mail", "").TrimEnd(';');
            mailID = GetDecryptParam("MailID", "");
            hearder = GetDecryptParam("Hearder", "");
            //MailUserID = WebMailID; 기존에 Zetta 에서 사용하던 쿠키/세션정보에 등록하고 사용하는 WebMailID 는 Deep 에서는 의미 없음 by seo
            MailAddress = UserInfo.MailID + "@" + UserInfo.MailDomain;
            pageNo = GetDecryptParam("PageNo", "0").ToNumber<int>();
            history = GetDecryptParam("History", "1");

            gubun = GetDecryptParam("gubun", "G");
            ownerID = GetDecryptParam("ownerID", "");            
            searchValue = GetDecryptParam("MSv", "");
            mailDomain = UserInfo.MailDomain;

            draftListID = GetDecryptParam("DraftListID", "0").ToNumber<int>();
            SwfUploadGUID = GetDecryptParam("SwfUploadGUID", "");
            FileCheck = GetDecryptParam("FileCheck", "N");
        }


        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            blockExt = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "BlockExt");
            upFileSize = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "UpFileSize");

            if (draftListID > 0)
            {
                //GetDraft();
            }

            try
            {
                if (FileCheck == "N")
                {
                    SwfUploadGUID = System.Guid.NewGuid().ToString();
                    //string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MailMobile/");
                    //if (Directory.Exists(uploadPath)) Directory.Delete(uploadPath, true);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        private void LoadAuth()
        {
            toMenu.Visible = false;
            ccMenu.Visible = false;
            bccMenu.Visible = false;

            if (UserInfo.MenuRoles != null)
            {
                for (int i = 0; i < UserInfo.MenuRoles.Length; i++)
                {
                    if (UserInfo.MenuRoles[i] == "1")
                    {
                        toMenu.Visible = true;
                        ccMenu.Visible = true;
                        bccMenu.Visible = true;
                    }
                }
            }
        }

        public string Today { get; set; }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-03-14
        /// 작성목적 : 발송승인사용체크        
        /// </summary>
        public string UseMailApprove
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "UseMailApprove");
            }
        }

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }
    }
}