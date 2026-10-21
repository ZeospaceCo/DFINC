using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.RM.BE;
using System.Transactions;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections;

namespace Zeospace.DeskPlus.RM.BLL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 자원 파일 등록 (자원을 추가할때)
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Add(ListInfo _info, int _listID, string _gubun)
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string targetPath = baseFileDirectory + _info.CompanyID + "/ResourceManager/Admin/" + _info.WriteDateYM + "/" + _listID;

            ArrayList fileList = FileHelper.GetUploadFile(_info.HiddenSpoolPath, targetPath, FolderType.ResourceManager, _listID, false, _info.WriteDateYM);
            DAL.Attach dal = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    AttachInfo Info = new AttachInfo();
                    Info.CompanyID = _info.CompanyID;
                    Info.CategoryID = _info.CategoryID;
                    Info.ListID = _listID;
                    Info.Gubun = _gubun;
                    Info.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    Info.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    Info.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    Info.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    Info.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    if (dal.Add(Info) < 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-17
        /// 작성목적 : 자원 파일 등록 (자원을 예약할때)
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Add(ReserveInfo _info, int _reservationID, string _gubun)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_info.HiddenSpoolPath, "", FolderType.ResourceManager, _reservationID, false, _info.WriteDateYM);
            DAL.Attach dal = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    AttachInfo Info = new AttachInfo();
                    Info.CompanyID = _info.CompanyID;
                    Info.CategoryID = 0;
                    Info.ListID = _reservationID;
                    Info.Gubun = _gubun;
                    Info.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    Info.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    Info.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    Info.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    Info.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    if (dal.Add(Info) < 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion 

        #region MOD()
        
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-12
        /// 작성목적 : 예약 자원 수정 (첨부파일 수정)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_beforeParentReservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_writeDateYM"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _beforeParentReservationID, int _parentReservationID, string _writeDateYM)
        {
            try
            {
                bool rtnVal = true;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 실제 파일 복사 
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string targetPath = baseFileDirectory + _companyID + "/ResourceManager/" + _writeDateYM + "/" + _parentReservationID + "/";

                    DAL.Attach dal = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dal.GetAttach(_companyID, 0, _beforeParentReservationID, "0");

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        string newGuid = System.Guid.NewGuid().ToString();

                        string tmpBeforePath = FileHelper.GetServerMapPath(be.FilePath + be.FileGUID);
                        string tmpTargetPath = FileHelper.GetServerMapPath(targetPath + newGuid  + be.FileType);

                        if (!Directory.Exists(FileHelper.GetServerMapPath(targetPath)))
                        {
                            Directory.CreateDirectory(FileHelper.GetServerMapPath(targetPath));
                        }

                        // 파일 복사 
                        System.IO.File.Copy(tmpBeforePath, tmpTargetPath);

                        BE.AttachInfo info = new AttachInfo();
                        info.CompanyID = _companyID;
                        info.CategoryID = 0;
                        info.ListID = _parentReservationID;
                        info.Gubun = "0";
                        info.FilePath = targetPath;
                        info.FileName = be.FileName;
                        info.FileType = be.FileType;
                        info.FileLength = be.FileLength;
                        info.FileGUID = newGuid + be.FileType;

                        // 파일 insert
                        if (dal.Add(info) < 0)
                        {
                            return false;
                        }
                    }                

                    ts.Complete();
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 예약 자원 수정 (첨부파일 수정) (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_beforeParentReservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_writeDateYM"></param>
        /// <returns></returns>
        public bool ModRepeat(int _companyID, int _beforeParentReservationID, int _parentReservationID, string _writeDateYM)
        {
            try
            {
                bool rtnVal = true;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 실제 파일 복사 
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string targetPath = baseFileDirectory + _companyID + "/ResourceManager/" + _writeDateYM + "/" + _parentReservationID + "/";

                    DAL.Attach dal = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dal.GetAttach(_companyID, 0, _beforeParentReservationID, "0");

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        string newGuid = System.Guid.NewGuid().ToString();

                        string tmpBeforePath = FileHelper.GetServerMapPath(be.FilePath + be.FileGUID);
                        string tmpTargetPath = FileHelper.GetServerMapPath(targetPath + newGuid + be.FileType);

                        if (!Directory.Exists(FileHelper.GetServerMapPath(targetPath)))
                        {
                            Directory.CreateDirectory(FileHelper.GetServerMapPath(targetPath));
                        }

                        // 파일 복사 
                        System.IO.File.Move(tmpBeforePath, tmpTargetPath);

                        BE.AttachInfo info = new AttachInfo();
                        info.CompanyID = _companyID;
                        info.CategoryID = 0;
                        info.ListID = _parentReservationID;
                        info.Gubun = "0";
                        info.FilePath = targetPath;
                        info.FileName = be.FileName;
                        info.FileType = be.FileType;
                        info.FileLength = be.FileLength;
                        info.FileGUID = newGuid + be.FileType;

                        // 파일 insert
                        if (dal.Add(info) < 0)
                        {
                            return false;
                        }
                    }

                    // 2. Attach 테이블 삭제 
                    if (!dal.Del(_companyID, 0, _beforeParentReservationID, "0"))
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod", ex);
                return false;
            }
        }



        #endregion 

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID, int _attachFileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.Attach dal = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dal.GetAttach(_companyID, _categoryID, _listID, _attachFileID);

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dal.Del(_companyID, _categoryID, _listID, _attachFileID);
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 예약 자원 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reservationID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.Attach dal = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dal.GetAttach(_companyID, 0, _reservationID, "0");

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dal.Del(_companyID, 0, _reservationID, be.AttachFileID, "0");
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

        #endregion 
    }
}
