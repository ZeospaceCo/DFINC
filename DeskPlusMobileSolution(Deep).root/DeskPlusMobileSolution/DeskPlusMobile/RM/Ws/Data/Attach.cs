using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.RM.Ws.BE;
using System.Text;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlusMobile.Web.RM.Ws.Data
{
    public class Attach
    {
        /// <summary>
        /// 자원 파일 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_info.CompanyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "INT_PROC_Attach");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@CategoryID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Gubun", SqlDbType.Char, 1),
                                                            new SqlParameter("@FilePath", SqlDbType.NVarChar, 1000),
                                                            new SqlParameter("@FileName", SqlDbType.NVarChar, 500),
                                                            new SqlParameter("@FileType", SqlDbType.NVarChar, 5),
                                                            new SqlParameter("@FileLength", SqlDbType.BigInt),
                                                            new SqlParameter("@FileGUID", SqlDbType.VarChar, 50)
                };
                    parms[0].Value = _info.CompanyID;
                    parms[1].Value = _info.CategoryID;
                    parms[2].Value = _info.ListID;
                    parms[3].Value = _info.Gubun.ToNull().ToXSS();
                    parms[4].Value = _info.FilePath.ToNull();
                    parms[5].Value = _info.FileName.ToNull();
                    parms[6].Value = _info.FileType.ToNull();
                    parms[7].Value = _info.FileLength;
                    parms[8].Value = _info.FileGUID.ToNull();

                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return Convert.ToInt32(obj);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : 방영환
        /// 작성일자 : 2015-11-02
        /// 작성목적 : 예약자원 파일 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pCatID"></param>
        /// <param name="_isDeptBoard"></param>
        /// <param name="_deptID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataSet GetAttach(int _companyID, int _categoryID, int _reservationID, string _gubun)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_Attach_ReID");

                proc = string.Format(proc, _companyID, _categoryID, _reservationID, _gubun);
                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetAttach");
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAttach: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 예약자원 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_attachFileID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _reservationID, int _attachFileID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "DEL_PROC_Attach");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@CategoryID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@AttachFileID", SqlDbType.Int),
                                                            new SqlParameter("@AttachFileID", SqlDbType.Char, 1)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _categoryID;
                    parms[2].Value = _reservationID;
                    parms[3].Value = _attachFileID;
                    parms[4].Value = _gubun;

                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 예약자원 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "DEL_PROC_Attach_Gubun");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@CategoryID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Gubun", SqlDbType.Char, 1)
                };
                    parms[0].Value = _companyID;
                    parms[1].Value = _categoryID;
                    parms[2].Value = _listID;
                    parms[3].Value = _gubun;


                    SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                throw;
            }
        }

    }
}