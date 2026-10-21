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

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Folder
{
    /// <summary>
    /// CopyMove 의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class CopyMove : Base.BaseWSWebHard
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
        public string CopyFile(string p)
        {
            try
            {
                SetDecryptString(p);

                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                int realFolderID = GetDecryptParam("realFolderID", "0").ToNumber<int>();
                string fileList = GetDecryptParam("fileList", "");
                string order = GetDecryptParam("order", "WriteDate desc");
                int uID = GetDecryptParam("uID", "0").ToNumber<int>();
                string types = GetDecryptParam("types", "");                

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

                string tmpPath = string.Empty;
                string tmpPathPublic = string.Empty;
                string webHardPath = string.Empty;
                string beforePath = string.Empty;
                string typesPublic = string.Empty;

                int MoveCopyFileLength = 0;     //파일 제한용량
                int MoveCopyFolderLength = 0;   //폴더 전체용량
                int MoveCopyUseCount = 0;       //이동 사용용량
                int UseCount = 0;       //폴더 사용용량

                bool Filechk = true;
                                   
                tmpPath = filePath + "{0}/Users/{1}/WebHard/{2}/";
                tmpPathPublic = filePath + "{0}/WebHard/{1}/";

                if (types == "U" || types == "L")
                {
                    beforePath = string.Format(tmpPathPublic, CurrentUserInfo.CompanyID, folderID);
                }
                else
                {
                    beforePath = string.Format(tmpPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID);
                }

                DAL.Folder dalf = new DAL.Folder();

                DataSet ds = dalf.GetFolder(CurrentUserInfo.CompanyID, realFolderID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["UserID"].ToString() == "0")
                        {
                            typesPublic = "U";
                        }

                        //파일제한용량 가져오기
                        MoveCopyFileLength = (dr["FileLength"].ToString().ToNumber<int>() * 1024);
                        //폴더제한용량 가져오기
                        MoveCopyFolderLength = (dr["FolderLength"].ToString().ToNumber<int>() * 1024);
                    }
                }

                if (typesPublic == "U")
                {
                    webHardPath = string.Format(tmpPathPublic, CurrentUserInfo.CompanyID, realFolderID);
                }
                else
                {                   
                    webHardPath = string.Format(tmpPath, CurrentUserInfo.CompanyID, uID, realFolderID);

                    if (CurrentUserInfo.UserID == uID)
                    {
                        //개인
                        //파일제한용량 가져오기
                        MoveCopyFileLength = AppSettingHelper.GetAppSetting(AppSettingType.WEBHARD, "AttachLimitSize", "0").ToString().ToNumber<int>();
                        //폴더제한용량 가져오기
                        MoveCopyFolderLength = CurrentUserInfo.PrivateQuota * 1024 * 1024;
                    }
                    else
                    {
                        DAL.File dalfi = new DAL.File();
                        DataTableReader dtrfi = dalfi.GetShareQuota(CurrentUserInfo.CompanyID, uID);

                        if (dtrfi != null)
                        {
                            if (dtrfi.HasRows)
                            {
                                while (dtrfi.Read())
                                {
                                    //파일제한용량 가져오기
                                    MoveCopyFileLength = AppSettingHelper.GetAppSetting(AppSettingType.WEBHARD, "AttachLimitSize", "0").ToString().ToNumber<int>();
                                    //폴더제한용량 가져오기
                                    MoveCopyFolderLength = dtrfi["PrivateQuota"].ToString().ToNumber<int>() * 1024 * 1024;
                                }
                            }
                        }
                    }
                }               

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
                            info.FolderID = realFolderID;
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

                            //폴더 사용량 가져오기
                            UseCount = UseCount + dtr["FileLength"].ToString().ToNumber<int>();

                            int BeforeFileLength = dtr["FileLength"].ToString().ToNumber<int>();

                            //파일별 용량 체크
                            if (MoveCopyFileLength != 0 && MoveCopyFileLength < BeforeFileLength)
                            {
                                Filechk = false;
                            }  

                            al.Add(info);
                        }
                    }
                }

                //파일 용량체크
                if (Filechk == false) { return "2"; }

                //폴더용량체크
                if (typesPublic == "U" || typesPublic == "L")
                {
                    DataSet dsu = dal.GetPublicUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, realFolderID, "U");

                    if (dsu != null)
                    {
                        foreach (DataRow dru in dsu.Tables[0].Rows)
                        {
                            // 공용폴더 사용한 용량 가져오기
                            MoveCopyUseCount = dru["FileLengthSum"].ToString().ToNumber<int>();
                        }
                    }
                }
                else
                {
                    //개인이 사용한 용량 가져오기
                    MoveCopyUseCount = dal.GetUseCount(CurrentUserInfo.CompanyID, uID).ToNumber<int>();
                }

                if (MoveCopyFolderLength != 0 && (MoveCopyFolderLength - MoveCopyUseCount) < UseCount)
                {
                    return "3";
                }                

                BLL.File bll = new BLL.File();
                bool rtn = bll.Copy(al, beforePath);

                if (rtn)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }              
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "0";
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
        public string MoveFile(string p)
        {
            try
            {
                SetDecryptString(p);
                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                int realFolderID = GetDecryptParam("realFolderID", "0").ToNumber<int>();
                string fileList = GetDecryptParam("fileList", "");
                string order = GetDecryptParam("order", "WriteDate desc");
                int uID = GetDecryptParam("uID", "0").ToNumber<int>();
                string types = GetDecryptParam("types", "");

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

                string tmpPath = string.Empty;
                string tmpPathPublic = string.Empty;
                string webHardPath = string.Empty;
                string beforePath = string.Empty;
                string typesPublic = string.Empty;

                int MoveCopyFileLength = 0;     //파일 제한용량
                int MoveCopyFolderLength = 0;   //폴더 전체용량
                int MoveCopyUseCount = 0;       //이동 사용용량
                int UseCount = 0;       //폴더 사용용량

                bool Filechk = true;

                tmpPath = filePath + "{0}/Users/{1}/WebHard/{2}/";
                tmpPathPublic = filePath + "{0}/WebHard/{1}/";

                if (types == "U" || types == "L")
                {
                    beforePath = string.Format(tmpPathPublic, CurrentUserInfo.CompanyID, folderID);
                }
                else
                {
                    beforePath = string.Format(tmpPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID);
                }

                DAL.Folder dalf = new DAL.Folder();

                DataSet ds = dalf.GetFolder(CurrentUserInfo.CompanyID, realFolderID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["UserID"].ToString() == "0")
                        {
                            typesPublic = "U";
                        }

                        //파일제한용량 가져오기
                        MoveCopyFileLength = (dr["FileLength"].ToString().ToNumber<int>() * 1024);
                        //폴더제한용량 가져오기
                        MoveCopyFolderLength = (dr["FolderLength"].ToString().ToNumber<int>() * 1024);
                    }
                }

                if (typesPublic == "U")
                {
                    webHardPath = string.Format(tmpPathPublic, CurrentUserInfo.CompanyID, realFolderID);
                }
                else
                {
                    webHardPath = string.Format(tmpPath, CurrentUserInfo.CompanyID, uID, realFolderID);

                    if (CurrentUserInfo.UserID == uID)
                    {
                        //개인
                        //파일제한용량 가져오기
                        MoveCopyFileLength = AppSettingHelper.GetAppSetting(AppSettingType.WEBHARD, "AttachLimitSize", "0").ToString().ToNumber<int>();
                        //폴더제한용량 가져오기
                        MoveCopyFolderLength = CurrentUserInfo.PrivateQuota * 1024 * 1024;
                    }
                    else
                    {
                        DAL.File dalfi = new DAL.File();
                        DataTableReader dtrfi = dalfi.GetShareQuota(CurrentUserInfo.CompanyID, uID);

                        if (dtrfi != null)
                        {
                            if (dtrfi.HasRows)
                            {
                                while (dtrfi.Read())
                                {
                                    //파일제한용량 가져오기
                                    MoveCopyFileLength = AppSettingHelper.GetAppSetting(AppSettingType.WEBHARD, "AttachLimitSize", "0").ToString().ToNumber<int>();
                                    //폴더제한용량 가져오기
                                    MoveCopyFolderLength = dtrfi["PrivateQuota"].ToString().ToNumber<int>() * 1024 * 1024;
                                }
                            }
                        }
                    }
                }

                fileList = fileList.TrimEnd(',');
                string tmpFileID = String.Empty;

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
                            info.FolderID = realFolderID;
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

                            //폴더 사용량 가져오기
                            UseCount = UseCount + dtr["FileLength"].ToString().ToNumber<int>();

                            int BeforeFileLength = dtr["FileLength"].ToString().ToNumber<int>();

                            //파일별 용량 체크
                            if (MoveCopyFileLength != 0 && MoveCopyFileLength < BeforeFileLength)
                            {
                                Filechk = false;
                            }

                            tmpFileID += dtr["FileID"].ToString().ToNumber<int>() + ",";

                            al.Add(info);
                        }
                    }
                }

                if (CurrentUserInfo.UserID != uID || typesPublic == "U")
                {
                    //파일 용량체크
                    if (Filechk == false) { return "2"; }
                }

                //폴더용량체크
                if (typesPublic == "U" || typesPublic == "L")
                {
                    DataSet dsu = dal.GetPublicUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, realFolderID, "U");

                    if (dsu != null)
                    {
                        foreach (DataRow dru in dsu.Tables[0].Rows)
                        {
                            // 공용폴더 사용한 용량 가져오기
                            MoveCopyUseCount = dru["FileLengthSum"].ToString().ToNumber<int>();
                        }
                    }
                }
                else
                {
                    //개인이 사용한 용량 가져오기
                    MoveCopyUseCount = dal.GetUseCount(CurrentUserInfo.CompanyID, uID).ToNumber<int>();
                }

                if (CurrentUserInfo.UserID != uID || typesPublic == "U")
                {
                    if (MoveCopyFolderLength != 0 && (MoveCopyFolderLength - MoveCopyUseCount) < UseCount)
                    {
                        return "3";
                    }
                }

                tmpFileID = tmpFileID.TrimEnd(',');

                BLL.File bll = new BLL.File();
                bool rtn = bll.Move(al, beforePath, CurrentUserInfo.UserID, tmpFileID);

                if (rtn)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }   
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "0";
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
