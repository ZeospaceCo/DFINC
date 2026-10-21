using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class CategoryUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the category user.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private CategoryUserInfo GetCategoryUser(IDataReader _reader)
        {
            return new CategoryUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                PageSize = (_reader["PageSize"] == DBNull.Value) ? -1 : _reader["PageSize"].ToString().ToNumber<int>(),
                ListType = _reader["ListType"].ToString().ToNull(),
                Types = _reader["Types"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the category user list.
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

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 게시판 카테고리 사용자 환경 추가 및 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddMod(CategoryUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_CategoryUser_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@ListType", SqlDbType.Char, 1).Value = _info.ListType.ToNull().ToXSS();
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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자   : 심재운
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
        public List<CategoryUserInfo> GetList(int _companyID, int _categoryID, int _userID, string _types)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_CategoryUser_Sel");
                    var cmd = new SqlCommand(query, cn);
                    //cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _types;
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
                throw;
            }
        }

        #endregion 가져오기
    }
}