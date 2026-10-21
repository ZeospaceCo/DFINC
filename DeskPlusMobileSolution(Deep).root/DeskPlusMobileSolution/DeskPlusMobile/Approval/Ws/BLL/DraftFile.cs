using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BLL
{
    public class DraftFile
    {
        #region ADD
        public bool Add(int _companyID, int _draftID, string _yearMonth, string _orginalPath)
        {
            try
            {
                if (_companyID.Equals(0) || _draftID.Equals(0) || _yearMonth.Equals("") || _orginalPath.Equals("")) return false;

                //  FileData/@CompanyID/Approval/Draft/@년월/@DraftID/Attach
                string targetPath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
                targetPath += "/{0}/Approval/Draft/{1}/{2}/Attach";
                targetPath = string.Format(targetPath, _companyID, _yearMonth, _draftID);

                ArrayList fileList = FileHelper.GetUploadFile(_orginalPath, targetPath, FolderType.Approval, _draftID, false, _yearMonth);
                Ws.Data.DraftFile fAttach = new Ws.Data.DraftFile();
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        Ws.BE.DraftFileInfo info = new Ws.BE.DraftFileInfo();
                        info.CompanyID = _companyID;
                        info.DraftID = _draftID;
                        info.FilePath = ((UploadFileInfo)fileList[i]).filePath.Replace("DeskPlusMobile", "DeskPlusEIP");
                        info.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        info.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        info.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        info.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;
                        info.FileOrder = i + 1;

                        fAttach.Add(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ADD : ", ex);
                throw;
            }
        }
        #endregion

        #region MOD
        public bool Mod(int _companyID, int _draftID, string _yearMonth, string _orginalPath)
        {
            try
            {
                if (_companyID.Equals(0) || _draftID.Equals(0) || _yearMonth.Equals("") || _orginalPath.Equals("")) return false;

                string targetPath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
                targetPath += "/{0}/Approval/Draft/{1}/{2}/Attach";
                targetPath = string.Format(targetPath, _companyID, _yearMonth, _draftID);

                ArrayList fileList = FileHelper.GetCopyFileList(_orginalPath, targetPath, FolderType.Approval, _draftID);
                Ws.Data.DraftFile fAttach = new Ws.Data.DraftFile();
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        Ws.BE.DraftFileInfo info = new Ws.BE.DraftFileInfo();
                        info.CompanyID = _companyID;
                        info.DraftID = _draftID;
                        info.FilePath = ((UploadFileInfo)fileList[i]).filePath.Replace("DeskPlusMobile", "DeskPlusEIP");
                        info.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        info.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        info.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        info.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;
                        info.FileOrder = i + 1;

                        fAttach.Add(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Mod : ", ex);
                throw;
            }
        }
        #endregion
    }
}