using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Send
{
    /// <summary>
    /// Send의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Send : Base.BaseWSWebHard
    {
        #region ADD()
               

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-08
        /// 작성목적 : 파일 전달
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SendFile(string p)
        {
            try
            {
                SetDecryptString(p);
                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                string fileList = GetDecryptParam("fileList", "");
                string order = GetDecryptParam("order", "WriteDate desc");
                int uID = GetDecryptParam("uID", "0").ToNumber<int>();

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string tmpPath = filePath + "{0}/Users/{1}/WebHard/{2}/";
                string webHardPath = string.Format(tmpPath, CurrentUserInfo.CompanyID, uID, "-3");
                string beforePath = string.Format(tmpPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID);

                fileList = fileList.TrimEnd(',');

                DAL.File dal = new DAL.File();
                DataTableReader dtr = dal.GetFileList(CurrentUserInfo.CompanyID, fileList, order);

                List<BE.FileInfo> al = new List<BE.FileInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.FileInfo info = new BE.FileInfo();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.UserID = uID;
                            info.FolderID = -3; //전달 폴더 (-3)
                            info.FileID = dtr["FileID"].ToString().ToNumber<int>();
                            info.FilePath = webHardPath;
                            info.FileGUID = dtr["FileGUID"].ToString();
                            info.NewGUID = System.Guid.NewGuid().ToString() + dtr["FileType"].ToString();
                            info.SendUserID = CurrentUserInfo.UserID;                            

                            if (HasScreenShot(beforePath + dtr["FileGUID"].ToString()))
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
