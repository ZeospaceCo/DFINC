using System;
using System.Collections;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.BLL
{
    public class BoardAttach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 첨부파일 추가하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_boardID">The _board ID.</param>
        /// <param name="_spoolPath">The _spool path.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _projectID, int _menuID, int _boardID, string _spoolPath, string _yearMonth)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.PROJECTBOARD, _boardID, false, _yearMonth);
            DAL.BoardAttach dalAttach = new DAL.BoardAttach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.BoardAttachInfo fileInfo = new BoardAttachInfo();
                    fileInfo.CompanyID = _companyID;
                    fileInfo.ProjectID = _projectID;
                    fileInfo.MenuID = _menuID;
                    fileInfo.BoardID = _boardID;

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

        /// <summary>
        /// 작성자 : 심재운
        /// 첨부파일 id별 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_boardID">The _board ID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _projectID, int _menuID, int _boardID, int _attachFileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.BoardAttach dalAttach = new DAL.BoardAttach();
                    var alAttach = dalAttach.GetAttachFile(_companyID, _projectID, _menuID, _boardID, _attachFileID);

                    foreach (BE.BoardAttachInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dalAttach.Del(_companyID, _projectID, _menuID, _boardID, _attachFileID);
                            System.IO.File.Delete(filePath);
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
    }
}