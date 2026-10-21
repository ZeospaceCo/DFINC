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
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class AttachFile : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 파일추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_docID"></param>
        /// <param name="_yearMonth"></param>
        /// <param name="_spoolPath"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _docID, string _yearMonth, string _orginalPath)
        {
            try
            {
                if (_companyID.Equals(0) || _docID.Equals(0) || _yearMonth.Equals("") || _orginalPath.Equals("")) return false; 

                string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                targetPath += "{0}/Approval/Document/{1}/{2}/Attach";
                targetPath = string.Format(targetPath, _companyID, _yearMonth, _docID);

                ArrayList fileList = FileHelper.GetUploadFile(_orginalPath, targetPath, FolderType.Approval, _docID, true, _yearMonth);
                DAL.AttachFile dalAttach = new DAL.AttachFile();
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        AttachFileInfo info = new AttachFileInfo();
                        info.CompanyID = _companyID;
                        info.DocumentID = _docID;
                        info.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                        info.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        info.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        info.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        info.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;
                        info.FileOrder = i + 1;
                        dalAttach.Add(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                throw;  //for, Transaction
                //return false;
            }
        }
        #endregion

        #region MOD
        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_docID"></param>
        /// <param name="_yearMonth"></param>
        /// <param name="_orginalPath"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _docID, string _yearMonth, string _orginalPath)
        {
            try
            {
                if (_companyID.Equals(0) || _docID.Equals(0) || _yearMonth.Equals("") || _orginalPath.Equals("")) return false;

                string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                targetPath += "{0}/Approval/Document/{1}/{2}/Attach";
                targetPath = string.Format(targetPath, _companyID, _yearMonth, _docID);

                ArrayList fileList = FileHelper.GetCopyFileList(_orginalPath, targetPath, FolderType.Approval, _docID);
                DAL.AttachFile dalAttach = new DAL.AttachFile();
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    //fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        AttachFileInfo info = new AttachFileInfo();
                        info.CompanyID = _companyID;
                        info.DocumentID = _docID;
                        info.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                        info.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        info.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        info.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        info.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;
                        info.FileOrder = i + 1;
                        dalAttach.Add(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                throw;  //for, Transaction
                //return false;
            }
        }
        #endregion

        #region DEL
        /// <summary>
        /// 특정파일삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _documentID, int _attachFileID)
        {
            try
            {
                HttpContext context = HttpContext.Current;

                DAL.AttachFile attDal = new DAL.AttachFile();
                List<BE.AttachFileInfo> list = attDal.GetAttachFileLists(_companyID, _documentID, _attachFileID);
                foreach (BE.AttachFileInfo info in list)
                {
                    string filePath = context.Server.MapPath(info.FilePath + info.FileGUID);

                    if (attDal.Del(info.CompanyID, info.DocumentID, info.AttachFileID))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                throw;  //for, Transaction
                //return false;
            }
        }
        #endregion
    }
}
