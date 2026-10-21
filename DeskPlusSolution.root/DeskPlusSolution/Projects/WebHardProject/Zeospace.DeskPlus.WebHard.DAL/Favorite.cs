using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.WebHard.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.WebHard.DAL
{
    public class Favorite : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private FavoriteInfo GetFavorite(IDataReader _reader)
        {
            return new FavoriteInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FavoriteID = (_reader["FavoriteID"] == DBNull.Value) ? -1 : _reader["FavoriteID"].ToString().ToNumber<int>(),
                ParentFavoriteID = (_reader["ParentFavoriteID"] == DBNull.Value) ? -1 : _reader["ParentFavoriteID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                FavoriteOrder = (_reader["FavoriteOrder"] == DBNull.Value) ? -1 : _reader["FavoriteOrder"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                Types = _reader["Types"].ToString().ToNull()
            };
        }


        /// <summary>
        /// Gets the favorite list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<FavoriteInfo> GetFavoriteList(IDataReader reader)
        {
            List<FavoriteInfo> list = new List<FavoriteInfo>();
            while (reader.Read())
                list.Add(GetFavorite(reader));
            return list;
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-21
        /// 작성목적 : 즐겨찾기 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int AddFavorite(FavoriteInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_WHD_Favorite_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentFavoriteID", SqlDbType.Int).Value = _info.ParentFavoriteID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 40).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 2).Value = _info.Types.ToNull().ToXSS();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }

        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-21
        /// 작성목적 : 즐겨찾기 폴더에 웹하드 폴더 넣기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_parentFavoriteID"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public bool AddFavoriteFolder(int _companyID, int _userID, int _folderID, int _parentFavoriteID, string _types)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_WHD_Favorite_InsCopy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ParentFavoriteID", SqlDbType.Int).Value = _parentFavoriteID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 2).Value = _types.ToNull().ToXSS();

                    cn.Open();

                    ExecuteNonQuery(cmd);

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

      

        #endregion

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-23
        /// 작성목적 : 즐겨찾기 폴더 정보가져오기 (부모 정보에 따라서)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_parentFavoriteID"></param>
        /// <returns></returns>
        public DataSet GetFavoriteTree(int _companyID, int _userID, int _parentFavoriteID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.WEBHARD, "QRY_WHD_Favorite_SelGetTree");

                    //최상위
                    if (_parentFavoriteID <= 0)
                    {
                        query = string.Format(query, "is null");
                    }
                    else if (_parentFavoriteID > 0)
                    {
                        query = string.Format(query, "=" + _parentFavoriteID.ToString().ToInject());
                    }

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                    
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetFavoriteTree");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-26
        /// 작성목적 : 즐겨찾기 개별 데이터 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_favoriteID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetFavoriteList(int _companyID, int _favoriteID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.WEBHARD, "QRY_WHD_Favorite_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Value = _favoriteID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
        
        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-26
        /// 작성목적 : 즐겨찾기 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModFavorite(FavoriteInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_WHD_Favorite_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Value = _info.FavoriteID;
                    cmd.Parameters.Add("@ParentFavoriteID", SqlDbType.Int).Value = _info.ParentFavoriteID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@FavoriteOrder", SqlDbType.Int).Value = _info.FavoriteOrder;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 40).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 2).Value = _info.Types.ToNull().ToXSS();

                    cn.Open();

                    ExecuteNonQuery(cmd);

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
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-27
        /// 작성목적 : 즐겨찾기 이동
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_favoriteID"></param>
        /// <param name="_parentFavoriteID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool ModParentFavorite(int _companyID, int _favoriteID, int _parentFavoriteID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_WHD_Favorite_UpdPID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Value = _favoriteID;
                    cmd.Parameters.Add("@ParentFavoriteID", SqlDbType.Int).Value = _parentFavoriteID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-27
        /// 작성목적 : 즐겨찾기 폴더 순서 변경하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_sortFolList"></param>
        /// <returns></returns>
        public bool ModOrderFavorite(int _companyID, int _userID, string _sortFolList)
        {
            try
            {
                if (_sortFolList.Length > 0)
                {
                    string[] _folderListStr = _sortFolList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _folderListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_WHD_Favorite_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                            cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Value = _folderListStr[i];
                            cmd.Parameters.Add("@FavoriteOrder", SqlDbType.Int).Value = (i + 1);

                            ExecuteNonQuery(cmd);
                        }
                    }
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

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-26
        /// 작성목적 : 즐겨찾기 폴더 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_favoriteID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID, int _favoriteID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_WHD_Favorite_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Value = _favoriteID;

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
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-03
        /// 작성목적 : Types에 따른 즐겨찾기 삭제
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

                    var cmd = new SqlCommand("DP_WHD_Favorite_DelTypes", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar,2).Value = _types;

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
