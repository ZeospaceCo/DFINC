using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Education.BE;
using System.Web;
using System.Transactions;

namespace Zeospace.DeskPlus.Education.BLL
{
    public class Attach : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 추가하기 

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_spoolPath"></param>
        /// <param name="_yearMonth"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, int _groupID, string _spoolPath, string _yearMonth)
        {
            string targetPath = GetAttachPath(_companyID, _listID, _yearMonth);
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, targetPath, FolderType.None, _listID, false, _yearMonth);
            DAL.Attach dalAttach = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.AttachInfo fileInfo = new AttachInfo();
                    fileInfo.CompanyID = _companyID;                    
                    fileInfo.ListID = _listID;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    dalAttach.Add(fileInfo);                    
                }
            }

            return true;
        }

        #endregion 

        #region MOD

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_spoolPath"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _listID, int _groupID, string _spoolPath)
        {
            string targetPath = GetAttachPath(_companyID, _listID, "");
            ArrayList fileList = FileHelper.GetCopyFileList(_spoolPath, targetPath, FolderType.None, _listID);
            DAL.Attach dalAttach = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.AttachInfo fileInfo = new AttachInfo();
                    fileInfo.CompanyID = _companyID;
                    fileInfo.ListID = _listID;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    dalAttach.Add(fileInfo);
                }
            }

            return true;
        }
     
        #endregion

        private string GetAttachPath(int companyID, int listID, string yearMonth)
        {
            string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if (String.IsNullOrEmpty(yearMonth)) yearMonth = FileHelper.GetYearMonth();
            return basePath.TrimEnd('/') + "/" + companyID + "/Education/" + yearMonth + "/" + listID;
        }

        #region 삭제하기

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID, int _attachID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 첨부파일 정보가져오기
                    DAL.Attach dalAttach = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dalAttach.GetAttach(_companyID, _listID, _attachID);

                    // 관련 DB 삭제
                    DAL.Attach dal = new DAL.Attach();
                    bool response = dal.Del(_companyID, _listID, _attachID);

                    HttpContext context = HttpContext.Current;

                    if (response)
                    {
                        foreach (BE.AttachInfo be in alAttach)
                        {
                            string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                    }

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                return false;
            }
        }

        #endregion 

    }
}
