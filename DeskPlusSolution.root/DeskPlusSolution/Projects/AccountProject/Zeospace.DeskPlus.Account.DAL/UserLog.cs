using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class UserLog : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.UserLogInfo GetUserLogInfo(IDataReader _reader)
        {
            return new UserLogInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                AccessTime = (_reader["AccessTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["AccessTime"],
                IPAddress = _reader["IPAddress"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the user log info list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<UserLogInfo> GetUserLogInfoList(IDataReader reader)
        {
            List<UserLogInfo> list = new List<UserLogInfo>();
            while (reader.Read())
                list.Add(GetUserLogInfo(reader));

            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 사용자별 아이피 로그 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.UserLogInfo> GetUserLog(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserAccessLog_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserLogInfoList(reader);
                }
            }
        }

        #endregion 추가/수정/삭제
    }
}