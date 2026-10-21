using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Base.Web.Upload.DPUpload
{
    public partial class Main : Base.BasePage
    {
        public string GUID = "";
        public string CallBackEvent = "";
        public string ModuleName = "";//모듈(APPORVAL, BOARD, PIM)
        public string FileSizePerFile = "";//하나의 파일에 대한 용량 제한 사이즈
        public string FileSizePerUser = "";//개인용 용량 사이즈
        public string FileType = ""; //필터 파일확장자
        public string FileTypeDesc = ""; //필더 설명
        public string FileCount = "0"; //파일 업로드 개수
        public int CompanyID;
        public int UserID;                                
        public string BlockExt = String.Empty;
        public string FilePathUser = "";

        public string Types = string.Empty;
        public string FileLength = string.Empty;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Upload.DPUpload.Main).Assembly;	//어셈블리 선언
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
            BlockExt = AppSettingHelper.GetAppSetting(AppSettingType.COMMON, "BlockExt", "");

            #region 파일 경로 암호화
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            FilePathUser = baseFileDirectory + "{0}/Users/{1}/Spool/{2}";
            FilePathUser = string.Format(FilePathUser, CompanyID, UserID, GUID);
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", FilePathUser);
            FilePathUser = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            #endregion

            LoadFileSizePerFile();
            LoadLimitTotalSize();
            LoadFileFilter();
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 모듈별, 하나의 파일 용량 제한 사이즈 구하기(kb)
        /// </summary>
        private void LoadFileSizePerFile()
        {
            switch (ModuleName.ToUpper())
            {
                case "APPROVAL" :
                    FileSizePerFile = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.APPROVAL, "AttachLimitSize", "1");//전자결재 파일 개당 제한 용량(KB)
                    break;
                case "ADDRESS":
                    FileSizePerFile = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.ADDRESS, "AttachLimitSize", "1");//주소록 파일 개당 제한 용량(KB)
                    break;
                case "BOARD":                    
                    FileSizePerFile = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "AttachLimitSize", "1");//게시판 파일 개당 제한 용량(KB)
                    break;
                case "PIM":
                    FileSizePerFile = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.PIM, "AttachLimitSize", "1");//일정관리 파일 개당 제한 용량(KB)
                    break;
                case "OPINION":
                    FileSizePerFile = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.OPINION, "AttachLimitSize", "1");//설문조사 파일 개당 제한 용량(KB)
                    break;
                case "WEBHARD":
                    if (Types != "U")
                    {
                        FileSizePerFile = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.WEBHARD, "AttachLimitSize", "1");//웹하드 파일 개당 제한 용량(KB)
                    }
                    else
                    {
                        FileSizePerFile = FileLength;
                    }
                    break;
                case "RM":
                    FileSizePerFile = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.RESOURCEMANAGER, "AttachLimitSize", "1");//자원관리 파일 개당 제한 용량(KB)
                    break;
                case "COMMUNITY":
                    FileSizePerFile = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMUNITY, "AttachLimitSize", "1");//커뮤니티 파일 개당 제한 용량(KB)
                    break;
                default :
                    FileSizePerFile = AppSettingHelper.GetAppSetting(AppSettingType.COMMON, "LimitSize", "");
                    break;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 파일 필더 정의
        /// </summary>
        private void LoadFileFilter()
        {
            //*.jpg;*.jpeg;*.png;*.gif
            if (FileType.IndexOf(';') != -1)
            {
                string fileFilter = "";
                foreach (string filter in FileType.Split(';'))
                {
                    if (filter != "*.*") fileFilter += filter.Replace("*.", "") + "|";
                }

                FileType = fileFilter.TrimEnd('|');
            }
            else
            {
                if (FileType == "*.*") FileType = "";
                else FileType = FileType.Replace("*.", "");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 개인별 용량 사이즈 구하기
        /// </summary>
        private void LoadLimitTotalSize()
        {
            //개발해야 할 사항임.
            //FileSizePerUser = "0"; //0 이면 무한대 파일 업로드 사용가능
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
            FileSizePerUser = GetDecryptParam("fileSizePerUser", ""); ; //0 이면 무한대 파일 업로드 사용가능

            Types = GetDecryptParam("types", "P");                       //웹하드 구분
            FileLength = GetDecryptParam("fileLength", "0");             //공용폴더 파일별 제한용량
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
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        #region Property (프로퍼티)
        
        #endregion
    }
}