using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserSNS : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private UserSNSInfo GetUserSNS(IDataReader _reader)
        {
            return new UserSNSInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                UserSNSID = (_reader["UserSNSID"] == DBNull.Value) ? -1 : _reader["UserSNSID"].ToString().ToNumber<int>(),
                FaceBook = _reader["FaceBook"].ToString().ToNull(),
                FaceBookPwd = _reader["FaceBookPwd"].ToString().ToNull(),
                Twitter = _reader["Twitter"].ToString().ToNull(),
                TwitterPwd = _reader["TwitterPwd"].ToString().ToNull(),
                Me2day = _reader["Me2day"].ToString().ToNull(),
                Me2dayPwd = _reader["Me2dayPwd"].ToString().ToNull(),
                Google = _reader["Google"].ToString().ToNull(),
                GooglePwd = _reader["GooglePwd"].ToString().ToNull(),
                NaverBlog = _reader["NaverBlog"].ToString().ToNull(),
                NaverBlogPwd = _reader["NaverBlogPwd"].ToString().ToNull()
            };
        }

        protected List<UserSNSInfo> GetUserSNSList(IDataReader reader)
        {
            List<UserSNSInfo> list = new List<UserSNSInfo>();
            while (reader.Read())
                list.Add(GetUserSNS(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사용자의 SNS 추가하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.UserSNSInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserSNS_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@FaceBook", SqlDbType.VarChar, 500).Value = _info.FaceBook.ToNull();
                    cmd.Parameters.Add("@FaceBookPwd", SqlDbType.VarChar, 50).Value = (_info.FaceBookPwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.FaceBookPwd.ToNull());
                    cmd.Parameters.Add("@Twitter", SqlDbType.VarChar, 500).Value = _info.Twitter.ToNull();
                    cmd.Parameters.Add("@TwitterPwd", SqlDbType.VarChar, 50).Value = (_info.TwitterPwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.TwitterPwd.ToNull());
                    cmd.Parameters.Add("@Me2day", SqlDbType.VarChar, 500).Value = _info.Me2day.ToNull();
                    cmd.Parameters.Add("@Me2dayPwd", SqlDbType.VarChar, 50).Value = (_info.Me2dayPwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.Me2dayPwd.ToNull());
                    cmd.Parameters.Add("@Google", SqlDbType.VarChar, 500).Value = _info.Google.ToNull();
                    cmd.Parameters.Add("@GooglePwd", SqlDbType.VarChar, 50).Value = (_info.GooglePwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.GooglePwd.ToNull());
                    cmd.Parameters.Add("@NaverBlog", SqlDbType.VarChar, 500).Value = _info.NaverBlog.ToNull();
                    cmd.Parameters.Add("@NaverBlogPwd", SqlDbType.VarChar, 50).Value = (_info.NaverBlogPwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.NaverBlogPwd.ToNull());
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
        /// 용도 : 사용자의 SNS 수정하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(BE.UserSNSInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserSNS_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@FaceBook", SqlDbType.VarChar, 500).Value = _info.FaceBook.ToNull();
                    cmd.Parameters.Add("@FaceBookPwd", SqlDbType.VarChar, 50).Value = (_info.FaceBookPwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.FaceBookPwd.ToNull());
                    cmd.Parameters.Add("@Twitter", SqlDbType.VarChar, 500).Value = _info.Twitter.ToNull();
                    cmd.Parameters.Add("@TwitterPwd", SqlDbType.VarChar, 50).Value = (_info.TwitterPwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.TwitterPwd.ToNull());
                    cmd.Parameters.Add("@Me2day", SqlDbType.VarChar, 500).Value = _info.Me2day.ToNull();
                    cmd.Parameters.Add("@Me2dayPwd", SqlDbType.VarChar, 50).Value = (_info.Me2dayPwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.Me2dayPwd.ToNull());
                    cmd.Parameters.Add("@Google", SqlDbType.VarChar, 500).Value = _info.Google.ToNull();
                    cmd.Parameters.Add("@GooglePwd", SqlDbType.VarChar, 50).Value = (_info.GooglePwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.GooglePwd.ToNull());
                    cmd.Parameters.Add("@NaverBlog", SqlDbType.VarChar, 500).Value = _info.NaverBlog.ToNull();
                    cmd.Parameters.Add("@NaverBlogPwd", SqlDbType.VarChar, 50).Value = (_info.NaverBlogPwd.ToNull() == "") ? "" : AESManager.Encrypt(_info.NaverBlogPwd.ToNull());

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
        /// 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.UserSNSInfo> GetUserSNSInfo(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserSNS_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserSNSList(reader);
                }
            }
        }

        #endregion 가져오기
    }
}