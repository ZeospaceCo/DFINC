using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Report.BE;
using System.Web;
using System.Transactions;

namespace Zeospace.DeskPlus.Report.BLL
{
    public class Attach
    {
        #region GET()
        #endregion

        #region ADD()

        /// <summary>
        /// 첨부파일 추가하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_spoolPath">The _spool path.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, string _spoolPath, string _yearMonth)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.Report, _listID, false, _yearMonth);
            DAL.Attach dal = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    AttachInfo fileInfo = new AttachInfo();
                    fileInfo.CompanyID = _companyID;
                    fileInfo.ListID = _listID;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    dal.Add(fileInfo);
                }
            }

            return true;
        }
        #endregion

        #region MOD()
        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : 심재운
        /// 첨부파일 id별 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID, int _attachFileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.Attach dalAttach = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dalAttach.GetAttachLists(_companyID, _listID, _attachFileID);

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dalAttach.Del(_companyID, _listID, _attachFileID);
                            System.IO.File.Delete(filePath);
                        }
                    }

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Report.Del : " + ex);
                return false;
            }
        }

        #endregion
    }
}
