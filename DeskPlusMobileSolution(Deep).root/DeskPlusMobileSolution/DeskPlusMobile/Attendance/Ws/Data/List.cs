using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Attendance.Ws.Data
{
    public class List
    {
        /// <summary>
        /// 작성자 : 이지원
        /// 작성목적 : 근태정보 가져오기
        /// 작성일자 : 2018-05-04
        /// </summary>
        /// <returns></returns>
        public string GetAttendance(int _companyID, int _userID, string _selDate)
        {
            string rtnAttendanceData = "|||";

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SelDate", SqlDbType.NVarChar, 10)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;
                parm[2].Value = _selDate;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAttendance, "SEL_PROC_ATTENDANCEDATA");
                DataTableReader dtr = SQLHelper.ExecuteDataTableReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAttendance", parm);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            rtnAttendanceData = dtr["bWS"].ToString() + "|" + dtr["WSTime"].ToString() + "|" + dtr["bWC"].ToString() + "|" + dtr["WCTime"].ToString();
                        }
                    }
                }

                return rtnAttendanceData;

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetAttendance", ex);
                return rtnAttendanceData;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 업무내역 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataSet GetList(string _companyID, int _listID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_TaskInfo");
                sql = string.Format(sql, _companyID, _listID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsList", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetList: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-28
        /// 작성목적 : 총 게시물 카운트 가져오기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_bPage"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetListCount(string _companyID, string _bPage, string _userID, string _searchValue)
        {
            try
            {
                string proc = "";
                switch (_bPage.ToUpper())
                {
                    case "RE": //요청한 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_PROC_List_RECnt");
                        break;
                    case "TO": //요청 받은 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_PROC_List_TOCnt");
                        break;
                    case "CC": //참조 받은 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_PROC_List_CCCnt");
                        break;
                    case "FO": //전달 받은 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_PROC_List_FOCnt");
                        break;
                    case "RC": //최신순 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_PROC_List_RCCnt");
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
                Helper.AppException.WriteException("GetListCount: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-12-22
        /// 작성목적 : 요청자, 수신자, 참조 구분 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataSet GetTypeCnt(string _companyID, int _listID, string _userID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_ListType");
                sql = string.Format(sql, _companyID, _listID, _userID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsList", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetTypeCnt: " + errMsg);
                throw;
            }
        }
    }
}