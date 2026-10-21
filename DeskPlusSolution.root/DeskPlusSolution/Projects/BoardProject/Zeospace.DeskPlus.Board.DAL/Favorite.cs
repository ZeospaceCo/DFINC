using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
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
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
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

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public int AddFavorite(FavoriteInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Favorite_Ins", cn);
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
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _info.Types.ToNull().ToXSS();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool ModFavorite(FavoriteInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Favorite_Upd", cn);
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
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _info.Types.ToNull().ToXSS();

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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 즐겨찾기 폴더 카테고리 순서변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_categoryList">The _category list.</param>
        /// <returns></returns>
        public bool ModOrderFavorite(int _companyID, int _userID, string _categoryList)
        {
            try
            {
                if (_categoryList.Length > 0)
                {
                    string[] _categoryListStr = _categoryList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _categoryListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_BOA_Favorite_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                            cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Value = _categoryListStr[i];
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
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 즐겨찾기 폴더 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_favoriteID">The _favorite ID.</param>
        /// <returns></returns>
        public bool DelFavorite(int _companyID, int _userID, int _favoriteID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_BOA_Favorite_Del", cn);
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
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기 폴더에 게시판 카테고리 넣기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_parentFavoriteID">The _parent favorite ID.</param>
        /// <returns></returns>
        public bool AddFavoriteCategory(int _companyID, int _userID, int _categoryID, int _parentFavoriteID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Favorite_InsCopy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ParentFavoriteID", SqlDbType.Int).Value = _parentFavoriteID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;

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

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 상위즐겨찾기 수정하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_favoriteID">The _favorite ID.</param>
        /// <param name="_parentFavoriteID">The _parent favorite ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool ModParentFavorite(int _companyID, int _favoriteID, int _parentFavoriteID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_BOA_Favorite_UpdPID", cn);
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
                throw;
            }
        }

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기 트리 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetFavoriteTree(int _companyID, int _userID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Favorite_SelGetTree");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetFavoriteTree");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기하위 카테고리 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentFavoriteID">The _parent favorite ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetParentFavorite(int _companyID, int _parentFavoriteID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Favorite_SelByPID");

                    //최상위
                    if (_parentFavoriteID == 0)
                    {
                        query = string.Format(query, "is null");
                    }
                    else if (_parentFavoriteID > 0)
                    {
                        query = string.Format(query, "=" + _parentFavoriteID.ToString().ToInject());
                    }

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_favoriteID">The _favorite ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetFavoriteList(int _companyID, int _favoriteID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Favorite_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FavoriteID", SqlDbType.Int).Value = _favoriteID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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