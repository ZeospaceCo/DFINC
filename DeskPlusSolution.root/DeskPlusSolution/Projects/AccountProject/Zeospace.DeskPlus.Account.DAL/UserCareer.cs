using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserCareer : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
          cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
            cmd.Parameters.Add("@CareerID", SqlDbType.Int).Value = _info.CareerID;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
            cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar,100).Value = _info.CompanyName.ToNull().ToXSS();
            cmd.Parameters.Add("@CareerPeriod", SqlDbType.NVarChar,100).Value = _info.CareerPeriod.ToNull().ToXSS();
            cmd.Parameters.Add("@TitleName", SqlDbType.NVarChar,100).Value = _info.TitleName.ToNull().ToXSS();
            cmd.Parameters.Add("@JobType", SqlDbType.NVarChar,100).Value = _info.JobType.ToNull().ToXSS();
            cmd.Parameters.Add("@Etc", SqlDbType.NVarChar,100).Value = _info.Etc.ToNull().ToXSS();
            cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = _info.Orders;
            cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
            cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
            cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
            cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _info.ModifyDate;
        */

        #region DataReader

        private UserCareerInfo GetUserCareer(IDataReader _reader)
        {
            return new UserCareerInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                    (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                    (_reader["CareerID"] == DBNull.Value) ? -1 : (int)_reader["CareerID"],
                    (_reader["StartDate"] == DBNull.Value) ? "" : _reader["StartDate"].ToString(),
                    (_reader["EndDate"] == DBNull.Value) ? "" : _reader["EndDate"].ToString(),
                    AESManager.Decrypt(_reader["CompanyName"].ToString()).ToNull().ToReverseXSS(),
                    _reader["CareerPeriod"].ToString().ToReverseXSS(),
                    _reader["TitleName"].ToString().ToReverseXSS(),
                    _reader["JobType"].ToString().ToReverseXSS(),
                    _reader["Etc"].ToString().ToReverseXSS(),
                    (_reader["Orders"] == DBNull.Value) ? -1 : (int)_reader["Orders"],
                    (_reader["WriteUserID"] == DBNull.Value) ? -1 : (int)_reader["WriteUserID"],
                    (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                    (_reader["ModifyUserID"] == DBNull.Value) ? -1 : (int)_reader["ModifyUserID"],
                    (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
                );
        }

        protected List<UserCareerInfo> GetUserCareerList(IDataReader reader)
        {
            List<UserCareerInfo> list = new List<UserCareerInfo>();
            while (reader.Read())
                list.Add(GetUserCareer(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사용자의 부서 추가하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.UserCareerInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserCareer_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _info.StartDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 8).Value = _info.EndDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 100).Value = AESManager.Encrypt(_info.CompanyName.ToNull().ToXSS());
                    cmd.Parameters.Add("@CareerPeriod", SqlDbType.NVarChar, 100).Value = _info.CareerPeriod.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleName", SqlDbType.NVarChar, 100).Value = _info.TitleName.ToNull().ToXSS();
                    cmd.Parameters.Add("@JobType", SqlDbType.NVarChar, 100).Value = _info.JobType.ToNull().ToXSS();
                    cmd.Parameters.Add("@Etc", SqlDbType.NVarChar, 100).Value = _info.Etc.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_careerID">The _career ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _careerID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserCareer_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CareerID", SqlDbType.Int).Value = _careerID;

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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 경력 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.UserCareerInfo> GetUserCareerInfo(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserCareer_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserCareerList(reader);
                }
            }
        }

        #endregion 가져오기
    }
}