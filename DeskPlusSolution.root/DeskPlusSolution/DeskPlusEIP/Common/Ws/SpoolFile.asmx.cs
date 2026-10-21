using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;

namespace Zeospace.DeskPlus.Common.Web.Ws
{
    /// <summary>
    /// SpoolFile의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SpoolFile : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 임시폴더의 파일 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSpoolFileList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.Ws.SpoolFile).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseCssDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/");

            string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
            string spoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID);

            List<object> listobj = new List<object>();
            DirectoryInfo di = new DirectoryInfo(Server.MapPath(spoolPath));
            if (di.Exists)
            {
                FileInfo[] sortList = di.GetFiles();
                Array.Sort(sortList,
                    new Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.FileListSort());

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                //수정날짜 asc 순별로
                foreach (FileInfo f in sortList)
                {
                    BE.SpoolFileInfo info = new BE.SpoolFileInfo();
                    info.FileGUID = System.Guid.NewGuid().ToString();
                    info.FileName = f.Name;
                    info.FileType = f.Extension;
                    info.FileLength = f.Length;


                    #region 스크린샷
                    info.FilePath = spoolPath;
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", info.FilePath + info.FileName);
                    string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                    info.FilePath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                    #endregion

                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(f.Length) / 1024) + " KB";

                    string fileOk = "0";
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (info.FileType.ToLower() == allowedExtensions[i])
                        {
                            fileOk = "1";
                            break;
                        }
                    }

                    info.ImageType = fileOk;
                    info.FileDel = resMng.GetString("Del");

                    listobj.Add(info);
                }
            }

            return listobj;
        }
    }
}
