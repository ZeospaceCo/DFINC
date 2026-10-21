using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Base.Web.Upload.SilverUpload
{
    public partial class Main : Base.BasePage
    {
        public string GUID = "";
        public string CallBackEvent = "";
        public string ModuleName = "";//모듈(APPORVAL, BOARD, PIM)
        public string FileTotalLimitSize = "";//하나의 파일에 대한 용량 제한 사이즈
        public string FileType = "";
        public string FileTypeDesc = "";
        public string FileCount = "0"; //파일 업로드 개수
        public int CompanyID;
        public int UserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Upload.SilverUpload.Main).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }        

        /// <summary>
        /// 작성자 : 심재운
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            LoadLimitTotalFileSize();
        }


        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            GUID = GetDecryptParam("swfUploadGUID", "");            
            FileType = GetDecryptParam("fileType", "");
            FileCount = GetDecryptParam("fileCount", "0");
            FileTypeDesc = GetDecryptParam("fileTypeDesc", "");
            CallBackEvent = GetDecryptParam("callBackEvent", "");
            ModuleName = GetDecryptParam("moduleName", "");
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 모듈별, 하나의 파일 용량 제한 사이즈 구하기(kb)
        /// </summary>
        private void LoadLimitTotalFileSize()
        {
            switch (ModuleName.ToUpper())
            {
                case "APPROVAL":
                    FileTotalLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.APPROVAL, "AttachLimitSize", "1");//전자결재 파일 개당 제한 용량(KB)
                    break;
                case "ADDRESS":
                    FileTotalLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.ADDRESS, "AttachLimitSize", "1");//주소록 파일 개당 제한 용량(KB)
                    break;
                case "BOARD":
                    FileTotalLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "AttachLimitSize", "1");//게시판 파일 개당 제한 용량(KB)
                    break;
                case "PIM":
                    FileTotalLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.PIM, "AttachLimitSize", "1");//일정관리 파일 개당 제한 용량(KB)
                    break;
                case "OPINION":
                    FileTotalLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.OPINION, "AttachLimitSize", "1");//설문조사 파일 개당 제한 용량(KB)
                    break;
                case "WEBHARD":
                    FileTotalLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.WEBHARD, "AttachLimitSize", "1");//웹하드 파일 개당 제한 용량(KB)
                    break;
                case "RM":
                    FileTotalLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.RESOURCEMANAGER, "AttachLimitSize", "1");//자원관리 파일 개당 제한 용량(KB)
                    break;
                case "COMMUNITY":
                    FileTotalLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMUNITY, "AttachLimitSize", "1");//커뮤니티 파일 개당 제한 용량(KB)
                    break;
                default:
                    FileTotalLimitSize = AppSettingHelper.GetAppSetting(AppSettingType.COMMON, "LimitSize", "");
                    break;
            }            

            //개인별 용량 구하기 ... 차후 개발 --- 개인 용량 모듈별 구해서 (-) 일 경우, 총 용량 사이즈 제한을 -1 로 처리해서 업로드 차단시킨다.
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnApply.Text = resMng.GetString("Alert_Ok");
                cbtnClose.Text = resMng.GetString("Alert_Cancel");
            }
        }
    }
}