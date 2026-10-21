using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Task.BE;
using System.Web;
using System.Transactions;

namespace Zeospace.DeskPlus.Task.BLL
{
    public class DraftAttach : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 추가하기 

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 첨부파일 추가하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <param name="_spoolPath"></param>
        /// <param name="_yearMonth"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _draftID, string _spoolPath, string _yearMonth)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.TaskDraft, _draftID, false, _yearMonth);
            DAL.DraftAttach dalAttach = new DAL.DraftAttach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.DraftAttachInfo fileInfo = new DraftAttachInfo();
                    fileInfo.CompanyID = _companyID;
                    fileInfo.DraftID = _draftID;
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

        #region 삭제하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 첨부파일 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _draftID, int _attachID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 첨부파일 정보가져오기
                    DAL.DraftAttach dalAttach = new DAL.DraftAttach();
                    List<BE.DraftAttachInfo> alAttach = dalAttach.GetAttach(_companyID, _draftID, _attachID);

                    // 관련 DB 삭제
                    DAL.DraftAttach dal = new DAL.DraftAttach();
                    bool response = dal.Del(_companyID, _draftID, _attachID);

                    HttpContext context = HttpContext.Current;

                    if (response)
                    {
                        foreach (BE.DraftAttachInfo be in alAttach)
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
