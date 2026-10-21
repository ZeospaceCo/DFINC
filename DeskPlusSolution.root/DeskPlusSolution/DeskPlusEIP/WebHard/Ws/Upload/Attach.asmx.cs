using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.WebHard.Web.Ws.Upload
{
    /// <summary>
    /// Attach의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Attach : Base.BaseWSWebHard
    {

        #region ADD()
        
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-27
        /// 작성목적 : 파일 업로드
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AttachFile(string p)
        {
            try
            {
                SetDecryptString(p);
                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                string tmpValue = GetDecryptParam("tmpValue", "");
                int uID = GetDecryptParam("uID", "0").ToNumber<int>();
                string guid = GetDecryptParam("guid", "");

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string tmpPath = filePath + "{0}/Users/{1}/WebHard/{2}/";
                string tmpSpoolPath = filePath + "{0}/Users/{1}/Spool/{2}/";
                
                string beforePath = string.Format(tmpPath, CurrentUserInfo.CompanyID, uID, folderID);
                string spoolPath = string.Format(tmpSpoolPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, guid);

                tmpValue = tmpValue.TrimEnd('|');

                List<BE.FileInfo> al = new List<BE.FileInfo>();
                if (tmpValue.Length > 0)
                {
                    foreach (string _value in tmpValue.Split('|'))
                    {
                        string[] arrValue = _value.Split(':');

                        BE.FileInfo info = new BE.FileInfo();
                        info.FilePath = spoolPath;
                        info.FileGUID = arrValue[0];
                        info.FileName = arrValue[1];

                        al.Add(info);
                    }
                }

                BLL.File bll = new BLL.File();
                return bll.Attach(al, beforePath);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion
    }
}
