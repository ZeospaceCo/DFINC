using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class DocView : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string bPage = "";
        public int pageNo = 0;
        public string dID = "";
        public int targetDeptID = 0;
        public int targetTitleID = 0;
        public string searchValue = "";
        public int keyID = 0;
        public int key2ID = 0;
        public string PhotoImage = "";
        public string cDID = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.DocView).Assembly;	//어셈블리 선언
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

                LoadData();
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            bPage = GetDecryptParam("BPage", "");
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            dID = GetDecryptParam("DID", "0");
            targetDeptID = GetDecryptParam("TargetDeptID", UserInfo.DeptID.ToString()).ToNumber<int>();
            targetTitleID = GetDecryptParam("TargetTitleID", UserInfo.CodeTitleID.ToString()).ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            keyID = GetDecryptParam("KeyID", "0").ToNumber<int>();
            key2ID = GetDecryptParam("Key2ID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-21
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
        /// 작성일자 : 2015-10-23
        /// </summary>
        /// <param name="_dID"></param>
        public void LoadData()
        {
            LoadParam();
            LoadPhotoImage();
            GetShortInfo();

            btnMemo.Visible = false;

            if (bPage == "Reject")
            {
                menu.Visible = false;
            }
            else if (bPage == "Forward")
            {
                btnMemo.Visible = true;
            }
        }

        private void LoadPhotoImage()
        {
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocCompleteInfo");
            sql = string.Format(sql, CompanyID.ToString(), dID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDocInfo", null);

            Account.Data.User dal = new Account.Data.User();
            Account.BE.UserInfo info = dal.GetUser(CompanyID.ToNumber<int>(), ds.Tables[0].Rows[0]["UserID"].ToString().ToNumber<int>());

            string photoName = info.PhotoImage;
            string BaseImgPath = "";
            string imgTag = "";
            string encParams = "";

            Dictionary<string, string> paraDic = null;
            string filePath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");

            if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
            {
                BaseImgPath = filePath + "/{0}/Users/{1}/Picture/" + photoName;
                BaseImgPath = string.Format(BaseImgPath, CompanyID, info.UserID);
            }
            else
            {
                BaseImgPath = "/DeskPlusMobile/Content/ko/Base/Img/DStyle/myimgdefault.gif";
            }

            if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
            {
                #region 스크린샷
                paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", BaseImgPath);
                encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                BaseImgPath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;
                #endregion

                imgTag = "<img src='{0}' style='width:70px; height:auto;' />";
                imgTag = string.Format(imgTag, BaseImgPath);
            }

            PhotoImage = imgTag;

        }

        private void GetShortInfo()
        {
            //결재의견 보기용
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SelectDoc");
            sql = string.Format(sql, CompanyID.ToString(), dID);
            DataSet dsDoc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDoc", null);

            foreach (DataRow dr in dsDoc.Tables[0].Rows)
            {
                cDID = dr["LastDocID"].ToString().ToNull();
            }

        }

        /// <summary>
        /// 작성자 : shimpark
        /// 암호화 문자열 반환하기
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                return Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        public string Today { get; set; }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
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
        /// 작성일자 : 2019-03-19
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