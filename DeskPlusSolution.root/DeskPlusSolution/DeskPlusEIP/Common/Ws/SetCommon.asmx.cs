using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.IO;

namespace Zeospace.DeskPlus.Common.Web.Ws
{
    /// <summary>
    /// SetCommon의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetCommon : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
    {

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> DelSpoolFileList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.Ws.SetCommon).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);
            
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");
            string fileName = GetDecryptParam("fileName", "");
            
            string rootPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string filePath = rootPath + "{0}/Users/{1}/Spool/{2}/";
            string spoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID);

            string delFile = rootPath + "{0}/Users/{1}/Spool/{2}/{3}";
            delFile = string.Format(delFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID, fileName);

            List<object> listobj = new List<object>();
            DirectoryInfo di = new DirectoryInfo(Server.MapPath(spoolPath));
            if (di.Exists)
            {
                if(File.Exists(Server.MapPath(delFile)))
                {
                    System.IO.File.Delete(Server.MapPath(delFile));
                }

                FileInfo[] sortList = di.GetFiles();
                Array.Sort(sortList,
                    new Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.FileListSort());

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (FileInfo f in sortList)
                {
                    BE.SpoolFileInfo info = new BE.SpoolFileInfo();

                    //info.FilePath = spoolPath;
                    info.FileName = f.Name;
                    info.FileType = f.Extension;
                    info.FileLength = f.Length;
                    info.FileGUID = System.Guid.NewGuid().ToString();
     
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


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 다국어 날짜 반환하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDateString(string p)
        {
            SetDecryptString(p);

            string gubun = GetDecryptParam("gubun", "");//week, month

            if (gubun == "week" || gubun == "month")
            {                
                DateTime startDate = System.DateTime.Now;
                DateTime endDate = new DateTime();

                if (gubun == "week") endDate = startDate.AddDays(7);
                if (gubun == "month") endDate = startDate.AddMonths(1).AddDays(-1);

                string startDateStr = DateHelpers.GetCultureDate(startDate, PreLangReal).Substring(0, 10);
                string endDateStr = DateHelpers.GetCultureDate(endDate, PreLangReal).Substring(0, 10);

                return startDateStr + "," + endDateStr;
            }

            return ",";
        }
    }
}
