using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Data
{
    public class List
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-26
        /// 작성목적 : 총 게시물 카운트 가져오기 (진행중문서)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_bPage"></param>
        /// <param name="_targetUID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetListCount(string _companyID, string _bPage, int _targetUID, string _searchValue)
        {
            try
            {
                string proc = "";
                switch (_bPage.ToUpper())
                {
                    case "APP": //미결리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ApproveCnt");
                        break;
                    case "REP": //상신리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ReportCnt");
                        break;
                    case "REL": //관계있는리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_RelatedCnt");
                        break;
                }
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _targetUID;
                parm[2].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetTotalCnt: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-26
        /// 작성목적 : 총 게시물 카운트 가져오기 (부서별 문서)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_titleID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPublicListCount(string _companyID, string _userID, int _deptID, int _titleID, string _searchValue)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_PublicCnt");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)
                                            ,new SqlParameter("@TitleID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _deptID;
                parm[3].Value = _titleID;
                parm[4].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetPublicListCount: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-26
        /// 작성목적 : 총 게시물 카운트 가져오기 (개인별, 완결결재, 반려, 전결, 전달, 받은 회람, 보낸 회람)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_bPage"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetDocListCount(string _companyID, string _bPage, string _userID, string _searchValue)
        {
            try
            {
                string proc = "";
                switch (_bPage)
                {
                    case "Private": //개인별 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_PrivateCnt");
                        break;
                    case "Approval": //완결 결재 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ApprovalCnt");
                        break;
                    case "Reject": //반려 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_RejectCnt");
                        break;
                    case "Arbitrary": //전결 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ArbitraryCnt");
                        break;
                    case "Forward": //전달 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ForwardCnt");
                        break;
                    case "CirRecv": //받은 회람함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_RecvCnt");
                        break;
                    case "CirSend": //보낸 회람함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SendCnt");
                        break;

                }
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDocListCount: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-26
        /// 작성목적 : 총 게시물 카운트 가져오기 (참조 문서)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetCarboncopyListCount(string _companyID, string _userID, int _deptID, string _searchValue)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_CarboncopyCnt");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _deptID;
                parm[3].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetCarboncopyListCount: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-07-10
        /// 작성목적 : 상신문서함 카운트 가져오기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetSubmitListCount(string _companyID, string _userID, string _searchValue)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SubmitCnt");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetSubmitListCount: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-07-11
        /// 작성목적 : 임시보관함 카운트 가져오기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetDraftListCount(string _companyID, string _userID, string _searchValue)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DraftCnt");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDraftListCount: " + errMsg);
                throw;
            }
        }
    }
}

