using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Visit.BE;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Transactions;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Visit.BLL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 파일 등록 
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public bool Add(ReserveInfo _info, int _reserveGroupID)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_info.HiddenSpoolPath, "", FolderType.VISIT, _reserveGroupID, false, _info.WriteDateYM);
            DAL.Attach dal = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    AttachInfo Info = new AttachInfo();
                    Info.CompanyID = _info.CompanyID;
                    Info.ReserveGroupID = _reserveGroupID;
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
        /// 작성일자 : 2014-02-14
        /// 작성목적 : 방문 예약 자원 수정 (첨부파일 수정)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_beforeReserveGroupID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_writeDateYM"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _beforeReserveGroupID, int _reserveGroupID, string _writeDateYM)
        {
            try
            {
                bool rtnVal = true;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 실제 파일 복사 
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string targetPath = baseFileDirectory + _companyID + "/Visit/" + _writeDateYM + "/" + _reserveGroupID + "/";

                    DAL.Attach dal = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dal.GetAttach(_companyID, _beforeReserveGroupID);

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
                        System.IO.File.Copy(tmpBeforePath, tmpTargetPath);

                        BE.AttachInfo info = new AttachInfo();
                        info.CompanyID = _companyID;
                        info.ReserveGroupID = _reserveGroupID;
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
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 자원 수정 (첨부파일 수정) (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_beforeReserveGroupID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_writeDateYM"></param>
        /// <returns></returns>
        public bool ModRepeat(int _companyID, int _beforeReserveGroupID, int _reserveGroupID, string _writeDateYM)
        {
            try
            {
                bool rtnVal = true;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 실제 파일 복사 
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string targetPath = baseFileDirectory + _companyID + "/Visit/" + _writeDateYM + "/" + _reserveGroupID + "/";

                    DAL.Attach dal = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dal.GetAttach(_companyID, _beforeReserveGroupID);

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
                        info.ReserveGroupID = _reserveGroupID;
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
                    if (!dal.Del(_companyID, _beforeReserveGroupID))
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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveGroupID, int _attachFileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.Attach dal = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dal.GetAttach(_companyID, _reserveGroupID, _attachFileID);

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dal.Del(_companyID, _reserveGroupID, _attachFileID);
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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 방문 예약 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.Attach dal = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dal.GetAttach(_companyID, _reserveID);

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dal.Del(_companyID, _reserveID, be.AttachFileID);
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
