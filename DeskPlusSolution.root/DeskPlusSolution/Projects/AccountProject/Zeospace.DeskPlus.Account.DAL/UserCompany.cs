using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserCompany : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the user career.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private UserCompanyInfo GetUserCompanyInfo(IDataReader _reader)
        {
            return new UserCompanyInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                GroupGUID = (_reader["GroupGUID"] == DBNull.Value) ? "" : _reader["GroupGUID"].ToString(),
                UserCompanyID = (_reader["UserCompanyID"] == DBNull.Value) ? -1 : _reader["UserCompanyID"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        protected List<BE.UserCompanyInfo> GetUserCompanyList(IDataReader reader)
        {
            List<UserCompanyInfo> list = new List<UserCompanyInfo>();
            while (reader.Read())
                list.Add(GetUserCompanyInfo(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 타회사추가하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.UserCompanyInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserCompany_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@GroupGUID", SqlDbType.VarChar, 100).Value = _info.GroupGUID;
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
        /// 회사별 사용자 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_groupGUID">The _group GUID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, string _groupGUID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserCompany_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@GroupGUID", SqlDbType.VarChar, 100).Value = _groupGUID;

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

        /// <summary>
        /// 작성자 : 심재운
        /// 타부서회사 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.UserCompanyInfo> GetUserCompany(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserCompany_SelByID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserCompanyList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 GroupGUID 로 타부서회사 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_groupGUID">The _group GUID.</param>
        /// <returns></returns>
        public List<BE.UserCompanyInfo> GetUserCompanyByGUID(int _companyID, string _groupGUID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserCompany_SelByComIDGUID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@GroupGUID", SqlDbType.VarChar, 100).Value = _groupGUID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserCompanyList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// GroupGUID 로 타부서회사 가져오기
        /// </summary>
        /// <param name="_groupGUID">The _group GUID.</param>
        /// <returns></returns>
        public List<BE.UserCompanyInfo> GetUserCompanyByGUID(string _groupGUID)
        {
            using (var cn = new SqlConnection(ConnSolution()))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserCompany_SelByGUID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@GroupGUID", SqlDbType.VarChar, 100).Value = _groupGUID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserCompanyList(reader);
                }
            }
        }
    }
}