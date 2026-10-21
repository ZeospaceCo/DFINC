using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    /// <summary>
    /// PortletCategory
    /// </summary>
    public class PortletCategory : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the portlet category.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private PortletCategoryInfo GetPortletCategory(IDataReader _reader)
        {
            return new PortletCategoryInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ParentCategoryID = (_reader["ParentCategoryID"] == DBNull.Value) ? -1 : _reader["ParentCategoryID"].ToString().ToNumber<int>(),
                CategoryOrder = (_reader["CategoryOrder"] == DBNull.Value) ? -1 : _reader["CategoryOrder"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                PathID = _reader["PathID"].ToString().ToNull(),
                PathNameKo = _reader["PathNameKo"].ToString().ToNull(),
                PathNameEn = _reader["PathNameEn"].ToString().ToNull(),
                PathNameJa = _reader["PathNameJa"].ToString().ToNull(),
                PathNameFr = _reader["PathNameFr"].ToString().ToNull(),
                PathNameZhcn = _reader["PathNameZhcn"].ToString().ToNull(),
                PathNameZhtw = _reader["PathNameZhtw"].ToString().ToNull(),
                Levels = (_reader["Levels"] == DBNull.Value) ? -1 : _reader["Levels"].ToString().ToNumber<int>(),
                Description = _reader["Description"].ToString().ToNull(),
                ActiveCategory = _reader["ActiveCategory"].ToString().ToNull(),
                IsAdmin = _reader["IsAdmin"].ToString().ToNull(),
                WriteUserID = _reader["WriteUserID"].ToString().ToNumber<int>()
            };
        }

        protected List<PortletCategoryInfo> GetPortletCategoryList(IDataReader reader)
        {
            List<PortletCategoryInfo> list = new List<PortletCategoryInfo>();

            while (reader.Read())
                list.Add(GetPortletCategory(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 포틀릿 카테고리 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(PortletCategoryInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletCategory_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentCategoryID", SqlDbType.Int).Value = _info.ParentCategoryID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@ActiveCategory", SqlDbType.Char, 1).Value = _info.ActiveCategory.ToNull();
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = _info.IsAdmin.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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
        /// 심재운
        /// 포틀릿 메뉴 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(PortletCategoryInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletCategory_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@ActiveCategory", SqlDbType.Char, 1).Value = _info.ActiveCategory.ToNull();
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = _info.IsAdmin.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletCategory_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;

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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 순서변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryList">The _category list.</param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, string _categoryList)
        {
            try
            {
                if (_categoryList.Length > 0)
                {
                    string[] categoryListStr = _categoryList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < categoryListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_EIP_PortletCategory_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryListStr[i];
                            cmd.Parameters.Add("@CategoryOrder", SqlDbType.Int).Value = (i + 1);

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
        /// 포틀릿 메뉴 이동하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_toCategoryID">The _to category ID.</param>
        /// <returns></returns>
        public bool ModMove(int _companyID, int _categoryID, int _toCategoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_EIP_PortletCategory_UpdMove", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ToCategoryID", SqlDbType.Int).Value = _toCategoryID;

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
        /// 용도 : 포틀릿 메뉴 상위 변경
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_parentCategoryID">The _parent category ID.</param>
        /// <returns></returns>
        public bool ModParentCategoryID(int _companyID, int _categoryID, int _parentCategoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_EIP_PortletCategory_UpdPID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ParentCategoryID", SqlDbType.Int).Value = _parentCategoryID;

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
        /// 카테고리 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_admin">if set to <c>true</c> [_admin].</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataSet GetPortletCategory(int _companyID, int _userID, bool _admin, bool _active)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_PortletCategory_Sel");
                string subQuery = "";
                if (_active) subQuery += " AND ActiveCategory = '1' ";

                if (_admin)
                {
                    subQuery += " AND IsAdmin = '1'";
                }
                else
                {
                    subQuery += " AND IsAdmin = '0' AND WriteUserID = " + _userID + " ";
                }

                query = string.Format(query, subQuery);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetPortletCategory");
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
        /// 포틀릿 메뉴 아이디로  정보 가져오기 (0 이면 최상위 포틀릿 메뉴들 가져옴)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_admin">if set to <c>true</c> [_admin].</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataSet GetPortletCategory(int _companyID, int _categoryID, int _userID, bool _admin, bool _active)
        {
            try
            {
                string query = "";
                string subQuery = "";
                if (_active) subQuery += " AND ActiveCategory = '1' ";

                if (_admin)
                {
                    subQuery += " AND IsAdmin = '1'";
                }
                else
                {
                    subQuery += " AND IsAdmin = '0' AND WriteUserID = " + _userID + " ";
                }

                if (_categoryID == 0)
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_PortletCategory_SelByPID");
                    query = string.Format(query, "is null", subQuery);

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        var cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cn.Open();

                        return ExecuteDataSet(cmd, "GetPortletCategory");
                    }
                }
                else
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_PortletCategory_SelByID");
                    query = string.Format(query, subQuery);

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        var cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                        cn.Open();

                        return ExecuteDataSet(cmd, "GetPortletCategory");
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

        /// <summary>
        /// 작성자 : 심재운
        /// 하위포틀릿 메뉴 정보 가져오기 (트리용)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCategoryID">The _parent category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_admin">if set to <c>true</c> [_admin].</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataSet GetPortletCategoryByPID(int _companyID, string _parentCategoryID, int _userID, bool _admin, bool _active)
        {
            try
            {
                string subQuery = "";
                if (_active) subQuery += " AND ActiveCategory = '1' ";

                if (_admin)
                {
                    subQuery += " AND IsAdmin = '1'";
                }
                else
                {
                    subQuery += " AND IsAdmin = '0' AND WriteUserID = " + _userID + " ";
                }

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_PortletCategory_SelByPID");

                if (_parentCategoryID == "0")
                {
                    query = string.Format(query, "is null", subQuery);
                }
                else
                {
                    query = string.Format(query, "=" + _parentCategoryID.ToInject(), subQuery);
                }

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetPortletCategory");
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
        /// 트리용 (포틀릿카테고리 + 포틀릿)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_admin">if set to <c>true</c> [_admin].</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataSet GetPortletCategoryTree(int _companyID, int _userID, bool _admin, bool _active)
        {
            try
            {
                string admin = (_admin) ? "1" : "0";
                string active = (_active) ? "1" : "0";

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletCategory_SelTree", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Active", SqlDbType.Char, 1).Value = active;
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = admin;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetPortletCategoryTree");
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