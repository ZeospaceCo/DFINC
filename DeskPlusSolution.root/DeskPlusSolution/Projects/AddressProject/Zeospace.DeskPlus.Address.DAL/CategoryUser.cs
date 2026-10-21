using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Address.BE;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.DAL
{
    public class CategoryUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the category user.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.CategoryUserInfo GetCategoryUser(IDataReader _reader)
        {
            return new CategoryUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                Types = _reader["Types"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                PageSize = (_reader["PageSize"] == DBNull.Value) ? -1 : _reader["PageSize"].ToString().ToNumber<int>(),
                ListType = _reader["ListType"].ToString().ToNull()
            };
        }


        /// <summary>
        /// Gets the common list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<CategoryUserInfo> GetCategoryUserList(IDataReader reader)
        {
            List<CategoryUserInfo> list = new List<CategoryUserInfo>();
            while (reader.Read())
                list.Add(GetCategoryUser(reader));
            return list;
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 카테고리별 사용자 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_types">The _types.</param>
        /// <returns></returns>
        public List<BE.CategoryUserInfo> GetCategoryUserLists(BE.CategoryUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ADDRESS, "QRY_ADD_CategoryUser_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _info.Types.ToNull().ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetCategoryUserList(reader);
                    }
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
        /// 작성자 : 심재운
        /// 사용자 카테고리별 저장 및 수정하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddMod(BE.CategoryUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_CategoryUser_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _info.Types.ToNull().ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@ListType", SqlDbType.Char, 1).Value = _info.ListType.ToNull().ToXSS();
                    
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

        #region 가져오기
        #endregion

    }
}
