using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Data.SqlClient;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.WebHard
{
    public partial class Send : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string mail = "";
        public string mailID = "";
        public string hearder = "";
        public string MailAddress = "";

        public int folderID = 0;
        public int pageNo = 1;
        public string order = "";
        public string publicGubun = "";
        public string shareGubun = "";
        public string fileID = "";
        public string FileCheck;
        public string blockExt;
        public string upFileSize;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.WebHard.Send).Assembly;	//어셈블리 선언
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
            mail = GetDecryptParam("Mail", "");
            mailID = GetDecryptParam("MailID", "");
            hearder = GetDecryptParam("Hearder", "");

            MailAddress = UserInfo.MailID + "@" + UserInfo.MailDomain;

            folderID = GetDecryptParam("FolderID", "0").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            order = GetDecryptParam("Order", "WriteDate desc");
            shareGubun = GetDecryptParam("ShareGubun", "N");
            publicGubun = GetDecryptParam("PublicGubun", "");
            fileID = GetDecryptParam("FileID", "0");
            SwfUploadGUID = GetDecryptParam("SwfUploadGUID", "");
            FileCheck = GetDecryptParam("FileCheck", "N");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-04-18
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
        /// 작성일자 : 2012-04-18
        /// 작성목적 : Load
        /// </summary>
        public void LoadData()
        {
            blockExt = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "BlockExt");
            upFileSize = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "UpFileSize");

            try
            {                
                if (FileCheck == "N")
                {                    
                    SwfUploadGUID = System.Guid.NewGuid().ToString();
                   
                    //string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MailMobile/" + SwfUploadGUID + "/");
                    string uploadPath = Server.MapPath("/DeskPlusMobile/MailAttach/" + CompanyID + "/Users/" + UserID + "/" + SwfUploadGUID + "/");
                    // Spool 폴더 생성  
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    fileID = fileID.TrimEnd(',');

                    SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)                                           
                                              ,new SqlParameter("@FileID", SqlDbType.VarChar,4000)  
                    };
                    parms[0].Value = UserInfo.CompanyID;
                    parms[1].Value = fileID;

                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_File_FileID");
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsFile", parms);

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Attach += dr["FileName"].ToString() + "<br>";

                            string oriFilePath = Server.MapPath(dr["FilePath"].ToString().Replace("DeskPlusEIP", "DeskPlusMobile") + dr["FileGUID"].ToString());

                            File.Copy(oriFilePath, uploadPath + dr["FileName"].ToString(), true);
                        }
                    }
                    FileCheck = "Y";
                }                
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoadTitle()", ex);
                throw;
            }
        }

        /// <summary>
        /// Attach
        /// </summary>
        public string Attach
        {
            get { return GetViewStateString("Attach", ""); }
            set { ViewState["Attach"] = value; }
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