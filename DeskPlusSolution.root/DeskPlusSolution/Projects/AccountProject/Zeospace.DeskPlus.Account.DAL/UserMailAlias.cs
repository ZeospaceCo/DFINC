using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserMailAlias : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private UserMailAliasInfo GetUserMailAlias(IDataReader _reader)
        {
            return new UserMailAliasInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                UserMaiilAliasID = (_reader["UserMaiilAliasID"] == DBNull.Value) ? -1 : _reader["UserMaiilAliasID"].ToString().ToNumber<int>(),
                MailAlias = _reader["MailAlias"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the user mail domain list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<UserMailAliasInfo> GetUserMailAliasList(IDataReader reader)
        {
            List<UserMailAliasInfo> list = new List<UserMailAliasInfo>();
            while (reader.Read())
                list.Add(GetUserMailAlias(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Add(UserMailAliasInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserMailAlias_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@MailAlias", SqlDbType.VarChar, 50).Value = _info.MailAlias.ToNull().ToXSS();

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
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Mod(UserMailAliasInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserMailAlias_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserMaiilAliasID", SqlDbType.Int).Value = _info.UserMaiilAliasID;
                    cmd.Parameters.Add("@MailAlias", SqlDbType.VarChar, 50).Value = _info.MailAlias.ToNull().ToXSS();

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
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Del(UserMailAliasInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserMailAlias_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserMaiilAliasID", SqlDbType.Int).Value = _info.UserMaiilAliasID;

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
        /// 메일 alias 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<UserMailAliasInfo> GetUserMailAlias(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserMailAlias_Sel");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetUserMailAliasList(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 가져오기
    }
}