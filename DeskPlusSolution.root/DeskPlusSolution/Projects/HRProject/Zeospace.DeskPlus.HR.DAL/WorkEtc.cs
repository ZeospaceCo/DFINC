using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.HR.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Zeospace.DeskPlus.HR.DAL
{
    public class WorkEtc : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET

        /// <summary>
        /// 유연근무 대상 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public DataTableReader GetWorkEtcList(int _companyID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_HR_WorkEtc_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 유연근무 대상 추가
        /// </summary>
        /// <param name="_unitItemInfo"></param>
        /// <returns></returns>
        public bool Add(BE.WorkEtcInfo _workEtcInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_workEtcInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_HR_WorkEtc_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _workEtcInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _workEtcInfo.UserID;
                    cmd.Parameters.Add("@WSTime", SqlDbType.DateTime).Value = _workEtcInfo.WSTime;
                    cmd.Parameters.Add("@WCTime", SqlDbType.DateTime).Value = _workEtcInfo.WCTime;
                    cmd.Parameters.Add("@WeekDays", SqlDbType.VarChar, 50).Value = _workEtcInfo.WeekDays;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _workEtcInfo.Types;

                    cn.Open();
                    ExecuteScalar(cmd);
                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 유연근무 대상 삭제(건당)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_adminID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _workEtcID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_HR_WorkEtc_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@WorkEtcID", SqlDbType.Int).Value = _workEtcID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion
    }
}
