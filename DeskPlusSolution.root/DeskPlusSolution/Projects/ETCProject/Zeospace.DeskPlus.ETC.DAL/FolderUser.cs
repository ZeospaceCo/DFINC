using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;

namespace Zeospace.DeskPlus.ETC.DAL
{
    public class FolderUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.FolderUserInfo GetFolderUserInfo(IDataReader _reader)
        {
            return new FolderUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FolderID = _reader["FolderID"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                PageSize = (_reader["PageSize"] == DBNull.Value) ? -1 : _reader["PageSize"].ToString().ToNumber<int>(),
                Types = _reader["Types"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]

            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<FolderUserInfo> GetFolderUserInfoList(IDataReader reader)
        {
            List<FolderUserInfo> list = new List<FolderUserInfo>();
            while (reader.Read())
                list.Add(GetFolderUserInfo(reader));
            return list;
        }

        #endregion

        #region GET()
        /// <summary>
        /// 작성자   : 김형석
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 리스트 가져오기
        /// 수정일자 : 카테고리별 사용자 설정 정보 가져오기
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_types">BL : 게시판 TL : 임시저장 IL : 중요표시</param>
        /// <returns></returns>
        public List<FolderUserInfo> GetList(int _companyID, string _folderID, int _userID, string _types)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ETC, "QRY_ETC_FolderUser_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.NVarChar, 30).Value = _folderID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Types", SqlDbType.NVarChar, 20).Value = _types;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFolderUserInfoList(reader);
                }
            }
        }
        #endregion

        #region ADD()

        /// <summary>
        /// 김형석
        /// 사용자 환경(페이지 사이즈) 추가 및 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddMod(FolderUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ETC_FolderUser_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.NVarChar, 30).Value = _info.FolderID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@Types", SqlDbType.NVarChar, 20).Value = _info.Types;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }

        }

        #endregion

        #region MOD()
        #endregion

        #region DEL()
        #endregion 
    }
}
