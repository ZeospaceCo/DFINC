using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
namespace Zeospace.DeskPlus.Pim.DAL
{
    public class GoogleCalendar : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public GoogleCalendar()
        {
        }

        #region Get
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015-06-12
        /// 작성목적 : 구글 일정 가져오기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetGoogleCalendar(BE.GoogleCalendarInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_GoogleCalendar_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                
                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015-06-12
        /// 작성목적 : 구글 계정 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetGoogleAccount(int _companyID, int _userID)
        {

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_SnsSel");

                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                return ExecuteDataTableReader(cmd);

            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015-06-12
        /// 작성목적 : 최신 동기화 시간
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetLatestSynTime(int _companyID, int _userID)
        {

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_GoogleCalendar_SelLatest");

                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                return ExecuteDataTableReader(cmd);

            }
        }
        #endregion

        #region Add

        //public void AddGoogleCalendar(BE.GoogleCalendarInfo _info)
        //{
        //    using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
        //    {
        //        var cmd = new SqlCommand("DP_PIM_Enviroment_Ins", cn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        //        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
        //        cmd.Parameters.Add("@Uid", SqlDbType.VarChar,50).Value = _info.Uid;
        //        cmd.Parameters.Add("@Title", SqlDbType.NVarChar,200).Value = _info.Title.ToXSS();
        //        cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = _info.StartTime;
        //        cmd.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = _info.EndTime;
        //        cmd.Parameters.Add("@Location", SqlDbType.NVarChar,100).Value = _info.Location;
        //        cn.Open();
        //        ExecuteNonQuery(cmd);

        //    }
        //}

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015-06-12
        /// 작성목적 : 구글 테이블에서 일정관리 테이블로 Sync
        /// </summary>
        /// <param name="CompanyID"></param>
        public void SyncGoogle(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PIM_GoogleCalendar_Sync", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015-06-12
        /// 작성목적 : bulk copy
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="CompanyID"></param>
        public void BulkTempGoogle(DataTable dt, int _companyID)
        {
            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(ConnSolution(_companyID)))
            {
                bulkcopy.DestinationTableName = "DP_PIM_GoogleCalendarTemp";
                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (SqlException ex)
                {
                    string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                    Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                }
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 용도 : 구글 계정 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddGoogleAccount(int _companyID, int _userID, string _googleAccount)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_GoogleAccount_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Google", SqlDbType.VarChar, 500).Value = _googleAccount.ToNull();
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

        #region DEL
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015-06-12
        /// 작성목적 : temp 테이블 삭제
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public void DelTempGoogle(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PIM_GoogleCalendarTemp_Del", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();
                ExecuteNonQuery(cmd);
            }
        }
        #endregion

    }
}
