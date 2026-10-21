using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.WebHard.BE;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.WebHard.DAL
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
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                PageSize = (_reader["PageSize"] == DBNull.Value) ? -1 : _reader["PageSize"].ToString().ToNumber<int>(),
                ListType = _reader["ListType"].ToString().ToNull(),
                Types = _reader["Types"].ToString().ToNull()
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

        #endregion

        #region GET()


        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-01-12
        /// 작성목적 : 폴더별 사용자 설정 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_userID"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public List<FolderUserInfo> GetList(int _companyID, int _folderID, int _userID, string _types)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.WEBHARD, "QRY_WHD_FolderUser_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Types", SqlDbType.Char, 1).Value = _types;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFolderUserList(reader);
                }
            }
        }


        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-11
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
                    var cmd = new SqlCommand("DP_WHD_FolderUser_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@ListType", SqlDbType.Char, 1).Value = _info.ListType.ToNull().ToXSS();
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 1).Value = _info.Types.ToNull().ToXSS();

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

        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-12
        /// 작성목적 : 폴더별 사용자 정보 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _folderID, string _types)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_WHD_FolderUser_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 1).Value = _types;

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
