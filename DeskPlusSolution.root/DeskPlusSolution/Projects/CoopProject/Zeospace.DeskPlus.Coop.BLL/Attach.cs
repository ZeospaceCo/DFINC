using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Coop.BE;
using System.Web;
using System.Transactions;

namespace Zeospace.DeskPlus.Coop.BLL
{
    public class AttachFile : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 첨부파일 추가하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_spoolPath">The _spool path.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, string _spoolPath, string _yearMonth)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.COOP, _listID, false, _yearMonth);
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

        /// <summary>
        /// 작성자 : 심재운
        /// 첨부파일 id별 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID,  int _listID, int _attachFileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.Attach dalAttach = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dalAttach.GetAttachFileLists(_companyID, _listID, _attachFileID);

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        //전달받은협업일지로 인해 파일을 삭제하지는 않는다.
                        //string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        //if (System.IO.File.Exists(filePath))
                        //{                            
                        //    System.IO.File.Delete(filePath);
                        //}

                        dalAttach.Del(be.CompanyID, be.ListID, be.AttachFileID);
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
