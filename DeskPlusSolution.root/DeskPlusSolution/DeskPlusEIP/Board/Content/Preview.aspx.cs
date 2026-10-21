using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;


namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class Preview : Board.Web.Base.BasePageBoard
    {
        
        public string Content = "";

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.Preview).Assembly;	//어셈블리 선언
            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

        }


        public void LoadData()
        {
            if (Request.Form["MimeValue"] != null && Request.Form["Gubun"] != null)
            {                
                
                if (Request.Form["Gubun"].ToUpper() == "F")
                {
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    System.Guid guid = System.Guid.NewGuid();
                    //string reqTime = FileHelper.GetYearMonth();

                    string UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Spool/" + guid.ToString();

                    string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                    string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                    string filePath = FileHelper.GetServerMapPath(UploadPath);
                    string serverNameFolderName = serverName + UploadPath;

                    if (!System.IO.Directory.Exists(filePath)) System.IO.Directory.CreateDirectory(filePath);                    

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass util
                         = new DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass();

                    util.ProcessDecoding(Request.Form["MimeValue"].ToString(), filePath, serverNameFolderName, 0);
                    Content = util.DecodedHTML; 
                }
                else
                {
                    Content = Request.Form["MimeValue"].ToString();
                }                

            }
            else
            {
                Content = resMng.GetString("NoContent");
            }
  
        
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
        }
        #endregion
    }
}