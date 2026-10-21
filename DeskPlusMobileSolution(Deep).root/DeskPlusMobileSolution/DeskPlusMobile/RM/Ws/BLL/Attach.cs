using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.RM.Ws.BE;
using Zeospace.DeskPlusMobile.Web.RM.Ws.Data;
using System.Data;
using System.Xml;
using System.Transactions;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.RM.Ws.BLL
{
    public class Attach
    {
        /// <summary>
        /// 예약 자원 수정 (첨부파일 수정) (반복일정에서 빼기)
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

                    string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
                    string targetPath = baseFileDirectory + _companyID + "/ResourceManager/" + _writeDateYM + "/" + _parentReservationID + "/";

                    Data.Attach dal = new Data.Attach();
                    DataSet ds = dal.GetAttach(_companyID, 0, _beforeParentReservationID, "0");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string newGuid = System.Guid.NewGuid().ToString();

                        string tmpBeforePath = FileHelper.GetServerMapPath(dr["FilePath"].ToString() + dr["FileGUID"].ToString());
                        string tmpTargetPath = FileHelper.GetServerMapPath(targetPath + newGuid + dr["FileType"].ToString());

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
                        info.FileName = dr["FileName"].ToString();
                        info.FileType = dr["FileType"].ToString();
                        info.FileLength = dr["FileLength"].ToString().ToNumber<long>();
                        info.FileGUID = newGuid + dr["FileType"].ToString();

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
                Helper.AppException.WriteException("ModRepeat", ex);
                return false;
            }
        }



        /// <summary>
        /// 예약자원 파일 삭제
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
                    Data.Attach dal = new Data.Attach();
                    DataSet ds = dal.GetAttach(_companyID, 0, _reservationID, "0");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string filePath = context.Server.MapPath(dr["FilePath"].ToString() + dr["FileGUID"].ToString());

                        if (System.IO.File.Exists(filePath))
                        {
                            dal.Del(_companyID, 0, _reservationID, dr["AttachFileID"].ToString().ToNumber<int>(), "0");
                            System.IO.File.Delete(filePath);
                        }
                    }


                    ts.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Del", ex);
                return false;
            }
        }
    }
}