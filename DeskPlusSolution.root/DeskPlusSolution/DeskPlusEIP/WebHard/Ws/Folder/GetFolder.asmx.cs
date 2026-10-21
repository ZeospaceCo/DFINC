using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Folder
{
    /// <summary>
    /// GetFolder의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetFolder : Base.BaseWSWebHard
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-01
        /// 작성목적 : 개인이 사용한 용량
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetUseCount()
        {
            try
            {
                DAL.File dal = new DAL.File();
                return dal.GetUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-08
        /// 작성목적 : 파일 복사
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool CopyFile(string p)
        {
            try
            {
                SetDecryptString(p);

                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                int realFolderID = GetDecryptParam("realFolderID", "0").ToNumber<int>();
                string tmpValue = GetDecryptParam("tmpValue", "");
                int uID = GetDecryptParam("uID", "0").ToNumber<int>();

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string tmpPath = filePath + "{0}/Users/{1}/WebHard/{2}/";
                string webHardPath = string.Format(tmpPath, CurrentUserInfo.CompanyID, uID, realFolderID);
                string beforePath = string.Format(tmpPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID);

                tmpValue = tmpValue.TrimEnd('|');

                List<BE.FileInfo> al = new List<BE.FileInfo>();
                if (tmpValue.Length > 0)
                {
                    foreach (string _value in tmpValue.Split('|'))
                    {
                        string[] arrValue = _value.Split(':');

                        BE.FileInfo info = new BE.FileInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = uID;
                        info.FolderID = realFolderID;
                        info.FileID = arrValue[0].ToNumber<int>();
                        info.FilePath = webHardPath;
                        info.FileGUID = arrValue[1];

                        string[] arrGUID = arrValue[1].Split('.');
                        info.NewGUID = System.Guid.NewGuid().ToString() + "." + arrGUID[1];

                        if (HasScreenShot(beforePath + arrValue[1]))
                        {
                            info.ScreenShot = "Y";
                        }
                        else
                        {
                            info.ScreenShot = "N";
                        }

                        al.Add(info);
                    }
                }

                BLL.File bll = new BLL.File();
                return bll.Copy(al, beforePath);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-08
        /// 작성목적 : 파일 이동
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveFile(string p)
        {
            try
            {
                SetDecryptString(p);
                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                int realFolderID = GetDecryptParam("realFolderID", "0").ToNumber<int>();
                string tmpValue = GetDecryptParam("tmpValue", "");
                int uID = GetDecryptParam("uID", "0").ToNumber<int>();

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string tmpPath = filePath + "{0}/Users/{1}/WebHard/{2}/";
                string webHardPath = string.Format(tmpPath, CurrentUserInfo.CompanyID, uID, realFolderID);
                string beforePath = string.Format(tmpPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID);

                tmpValue = tmpValue.TrimEnd('|');

                string tmpFileID = String.Empty;

                List<BE.FileInfo> al = new List<BE.FileInfo>();
                if (tmpValue.Length > 0)
                {
                    foreach (string _value in tmpValue.Split('|'))
                    {
                        string[] arrValue = _value.Split(':');

                        BE.FileInfo info = new BE.FileInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = uID;
                        info.FolderID = realFolderID;
                        info.FileID = arrValue[0].ToNumber<int>();
                        info.FilePath = webHardPath;
                        info.FileGUID = arrValue[1];

                        if (HasScreenShot(beforePath + arrValue[1]))
                        {
                            info.ScreenShot = "Y";
                        }
                        else
                        {
                            info.ScreenShot = "N";
                        }

                        al.Add(info);

                        tmpFileID += arrValue[0].ToNumber<int>() + ",";
                    }
                }

                tmpFileID = tmpFileID.TrimEnd(',');

                BLL.File bll = new BLL.File();
                return bll.Move(al, beforePath, CurrentUserInfo.UserID, tmpFileID);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-10
        /// 작성목적 : 이미지 파일인지 구분
        /// </summary>
        /// <returns></returns>
        private bool HasScreenShot(string _path)
        {
            bool fileOK = false;

            string fileExtension = System.IO.Path.GetExtension(Server.MapPath(_path)).ToLower();
            string[] allowedExtensions = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "ImgFileExtension", "").Split(',');

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                    break;
                }
            }

            return fileOK;
        }

        #endregion         
    }
}
