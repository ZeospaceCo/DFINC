using System;
using System.Collections;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.BLL
{
    public class UnitAttach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 첨부파일 추가하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_unitID">The _unit ID.</param>
        /// <param name="_spoolPath">The _spool path.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _projectID, int _unitID, string _spoolPath, string _yearMonth)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.PROJECTUNIT, _unitID, false, _yearMonth);
            DAL.UnitAttach dalAttach = new DAL.UnitAttach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.UnitAttachInfo fileInfo = new UnitAttachInfo();
                    fileInfo.CompanyID = _companyID;
                    fileInfo.ProjectID = _projectID;
                    fileInfo.UnitID = _unitID;

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
        /// <param name="_unitID">The _unit ID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _projectID, int _unitID, int _attachFileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.UnitAttach dalAttach = new DAL.UnitAttach();
                    var alAttach = dalAttach.GetAttachFile(_companyID, _projectID, _unitID, _attachFileID);

                    foreach (BE.UnitAttachInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dalAttach.Del(_companyID, _projectID, _unitID, _attachFileID);
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