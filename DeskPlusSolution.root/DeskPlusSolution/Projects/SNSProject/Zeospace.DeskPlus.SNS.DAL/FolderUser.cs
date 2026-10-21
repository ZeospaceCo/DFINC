using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.SNS.BE;

namespace Zeospace.DeskPlus.SNS.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class FolderUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        ///
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        private FolderUserInfo GetFolderUser(IDataReader _reader)
        {
            return new FolderUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                Types = _reader["Types"].ToString().ToNull(),
                PageSize = (_reader["PageSize"] == DBNull.Value) ? -1 : _reader["PageSize"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected List<FolderUserInfo> GetFolderUserList(IDataReader reader)
        {
            List<FolderUserInfo> list = new List<FolderUserInfo>();
            while (reader.Read())
                list.Add(GetFolderUser(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-05-14
        /// 작성목적 : 폴더별 사용자 설정 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public List<FolderUserInfo> GetList(int _companyID, int _userID, string _types)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.SNS, "QRY_SNS_FolderUser_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Types", SqlDbType.Char, 1).Value = _types;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFolderUserList(reader);
                }
            }
        }

        #endregion GET()

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-14
        /// 작성목적 : 폴더별 사용자 정보 설정하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AddMod(FolderUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_SNS_FolderUser_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 1).Value = _info.Types.ToNull().ToXSS();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion MOD()
    }
}