using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Opinion.BE;
using System.Web;
using System.Transactions;

namespace Zeospace.DeskPlus.Opinion.BLL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 파일추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_spoolPath"></param>
        /// <param name="_yearMonth"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _quesID, string _spoolPath)
        {
            try
            {
                string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                targetPath += "{0}/Opinion/Ques/{1}";
                targetPath = string.Format(targetPath, _companyID, _quesID);

                ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, targetPath, FolderType.Opinion, _quesID, true, "");
                DAL.Attach dalAttach = new DAL.Attach();
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        AttachInfo attInfo = new AttachInfo();
                        attInfo.CompanyID = _companyID;
                        attInfo.QuesID = _quesID;
                        attInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                        attInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        attInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        attInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        attInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;
                        dalAttach.Add(attInfo);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }
        #endregion

        #region MOD
        #endregion

        #region DEL

        /// <summary>
        /// 파일삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _quesID, int _attachID)
        {
            try
            {
                HttpContext context = HttpContext.Current;

                DAL.Attach dalAttach = new DAL.Attach();
                List<BE.AttachInfo> alAttach = dalAttach.GetAttach(_companyID, _quesID, _attachID);
                foreach (BE.AttachInfo be in alAttach)
                {
                    string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);
                    
                    if (dalAttach.Del(_companyID, _quesID, be.AttachID))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                return true;

                #region 주석
                //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                //{
                //    HttpContext context = HttpContext.Current;

                //    DAL.Attach dalAttach = new DAL.Attach();
                //    List<BE.AttachInfo> alAttach = dalAttach.GetAttach(_companyID, _quesID, _attachID);
                //    foreach (BE.AttachInfo be in alAttach)
                //    {
                //        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                //        if (System.IO.File.Exists(filePath))
                //        {
                //            if (dalAttach.Del(_companyID, _quesID, be.AttachID))
                //            {
                //                System.IO.File.Delete(filePath);
                //            }
                //        }
                //    }

                //    ts.Complete();

                //    return true;
                //}
                #endregion
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
