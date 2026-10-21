using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Xml;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class DraftFile : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        public bool Add(int _companyID, int _draftID, string _yearMonth, string _orginalPath)
        {
            try
            {
                if (_companyID.Equals(0) || _draftID.Equals(0) || _yearMonth.Equals("") || _orginalPath.Equals("")) return false;

                //  FileData/@CompanyID/Approval/Draft/@년월/@DraftID/Attach
                string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                targetPath += "{0}/Approval/Draft/{1}/{2}/Attach";
                targetPath = string.Format(targetPath, _companyID, _yearMonth, _draftID);

                ArrayList fileList = FileHelper.GetUploadFile(_orginalPath, targetPath, FolderType.Approval, _draftID, false, _yearMonth);
                DAL.DraftFile fAttach = new DAL.DraftFile();
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        DraftFileInfo info = new DraftFileInfo();
                        info.CompanyID = _companyID;
                        info.DraftID = _draftID;
                        info.FilePath = ((UploadFileInfo)fileList[i]).filePath;
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
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

                string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                targetPath += "{0}/Approval/Draft/{1}/{2}/Attach";
                targetPath = string.Format(targetPath, _companyID, _yearMonth, _draftID);

                ArrayList fileList = FileHelper.GetCopyFileList(_orginalPath, targetPath, FolderType.Approval, _draftID);
                DAL.DraftFile fAttach = new DAL.DraftFile();
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        DraftFileInfo info = new DraftFileInfo();
                        info.CompanyID = _companyID;
                        info.DraftID = _draftID;
                        info.FilePath = ((UploadFileInfo)fileList[i]).filePath;
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod : " + ex);
                throw;
            }
        }
        #endregion

        #region DEL
        #endregion
    }
}
