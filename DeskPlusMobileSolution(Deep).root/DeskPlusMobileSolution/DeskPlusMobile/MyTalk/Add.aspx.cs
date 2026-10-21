using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.IO;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.MyTalk
{
    public partial class Add : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int groupID = 0;
        public int pageNo = 1;
        public int rUserID = 0;
        public int draftListID;
        public string FileCheck;
        public string uploadCheck = "Y";
        public string upFileSize;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.MyTalk.Add).Assembly;	//어셈블리 선언
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

                LoadData();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            groupID = GetDecryptParam("GroupID", "0").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            rUserID = GetDecryptParam("RUserID", "0").ToNumber<int>();
            draftListID = GetDecryptParam("DraftListID", "0").ToNumber<int>();
            SwfUploadGUID = GetDecryptParam("SwfUploadGUID", "");
            FileCheck = GetDecryptParam("FileCheck", "N");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-11-06
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
        /// 작성자   : famo23
        /// 작성일자 : 2015-03-12
        /// 작성목적 : Load
        /// </summary>
        public void LoadData()
        {
            upFileSize = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "UpFileSize");
            cbOpen.Checked = true;

            if (groupID > 0)
            {
                uploadCheck = "N";
            }
            else
            {
                uploadCheck = "Y";
            }

            if (draftListID > 0)
            {
                GetDraft();
            }

            try
            {
                if (FileCheck == "N")
                {
                    SwfUploadGUID = System.Guid.NewGuid().ToString();
                    string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/");
                    if (Directory.Exists(uploadPath)) Directory.Delete(uploadPath, true);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 임시저장 정보 가져오기
        /// </summary>
        private void GetDraft()
        {
            Ws.Data.List dal = new Ws.Data.List();
            DataSet ds = dal.GetDraft(CompanyID, draftListID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cbOpen.Checked = (dr["IsSecurity"].ToString() == "0") ? true : false;
                    memo.Value = dr["Content"].ToString();
                }
            }
        }

        public string Today { get; set; }

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-11-08
        /// 작성목적 : SSL 사용 유무      
        /// </summary>
        public string httpType
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-11-08
        /// 작성목적 : 도메인 정보        
        /// </summary>
        public string MobileDOMAIN
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");
            }
        }
    }
}