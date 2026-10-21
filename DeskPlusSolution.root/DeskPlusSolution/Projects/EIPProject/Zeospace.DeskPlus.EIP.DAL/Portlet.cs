using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;
using System.Web;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class Portlet : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private List<PortletInfo> GetPortletList(IDataReader reader)
        {
            List<PortletInfo> list = new List<PortletInfo>();

            while (reader.Read())
            {
                list.Add(new PortletInfo
                {
                    CompanyID = (reader["CompanyID"] == DBNull.Value) ? -1 : reader["CompanyID"].ToString().ToNumber<int>(),
                    PortletID = (reader["PortletID"] == DBNull.Value) ? -1 : reader["PortletID"].ToString().ToNumber<int>(),
                    CategoryID = (reader["CategoryID"] == DBNull.Value) ? -1 : reader["CategoryID"].ToString().ToNumber<int>(),
                    NameBase = reader["NameBase"].ToString().ToNull(),
                    NameKo = reader["NameKo"].ToString().ToNull(),
                    NameEn = reader["NameEn"].ToString().ToNull(),
                    NameJa = reader["NameJa"].ToString().ToNull(),
                    NameFr = reader["NameFr"].ToString().ToNull(),
                    NameZhcn = reader["NameZhcn"].ToString().ToNull(),
                    NameZhtw = reader["NameZhtw"].ToString().ToNull(),
                    PortletType = reader["PortletType"].ToString().ToNull(),
                    UIType = reader["UIType"].ToString().ToNull(),
                    UITemplate = reader["UITemplate"].ToString().ToNull(),
                    UIURL = reader["UIURL"].ToString().ToNull(),
                    DataType = reader["DataType"].ToString().ToNull(),
                    DataURL = reader["DataURL"].ToString().ToNull(),
                    LinkType = reader["LinkType"].ToString().ToNull(),
                    LinkPopupSize = reader["LinkPopupSize"].ToString().ToNull(),
                    LinkURL = reader["LinkURL"].ToString().ToNull(),
                    IsPesonalization = reader["IsPesonalization"].ToString().ToNull(),
                    ActivePortlet = reader["ActivePortlet"].ToString().ToNull(),
                    Description = reader["Description"].ToString().ToNull(),
                    PhotoName = reader["PhotoName"].ToString().ToNull(),
                    IsAdmin = reader["IsAdmin"].ToString().ToNull(),
                    WriteUserID = (reader["WriteUserID"] == DBNull.Value) ? -1 : reader["WriteUserID"].ToString().ToNumber<int>(),
                    WriteDate = (reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)reader["WriteDate"],
                    ModifyUserID = (reader["ModifyUserID"] == DBNull.Value) ? -1 : reader["ModifyUserID"].ToString().ToNumber<int>(),
                    ModifyDate = (reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)reader["ModifyDate"],
                    CatNameBase = reader["CatNameBase"].ToString().ToNull(),
                    CatNameKo = reader["CatNameKo"].ToString().ToNull(),
                    CatNameEn = reader["CatNameEn"].ToString().ToNull(),
                    CatNameJa = reader["CatNameJa"].ToString().ToNull(),
                    CatNameFr = reader["CatNameFr"].ToString().ToNull(),
                    CatNameZhcn = reader["CatNameZhcn"].ToString().ToNull(),
                    CatNameZhtw = reader["CatNameZhtw"].ToString().ToNull()
                });
            }
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(PortletInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_Ins", cn);
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
                    cmd.Parameters.Add("@PortletType", SqlDbType.Char, 1).Value = _info.PortletType.ToNull();
                    cmd.Parameters.Add("@UIType", SqlDbType.Char, 1).Value = _info.UIType.ToNull();
                    cmd.Parameters.Add("@UITemplate", SqlDbType.Char, 1).Value = _info.UITemplate.ToNull();
                    cmd.Parameters.Add("@UIURL", SqlDbType.VarChar, 200).Value = _info.UIURL.ToNull();
                    cmd.Parameters.Add("@DataType", SqlDbType.Char, 1).Value = _info.DataType.ToNull();
                    cmd.Parameters.Add("@DataURL", SqlDbType.VarChar, 2000).Value = _info.DataURL.ToNull();
                    cmd.Parameters.Add("@LinkType", SqlDbType.Char, 1).Value = _info.LinkType.ToNull();
                    cmd.Parameters.Add("@LinkPopupSize", SqlDbType.VarChar, 50).Value = _info.LinkPopupSize.ToNull().ToXSS();
                    cmd.Parameters.Add("@LinkURL", SqlDbType.VarChar, 200).Value = _info.LinkURL.ToNull();
                    cmd.Parameters.Add("@IsPesonalization", SqlDbType.Char, 1).Value = _info.IsPesonalization.ToNull();
                    cmd.Parameters.Add("@ActivePortlet", SqlDbType.Char, 1).Value = _info.ActivePortlet.ToNull();
                    cmd.Parameters.Add("@SendMethod", SqlDbType.Char, 1).Value = _info.SendMethod.ToNull();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@PhotoName", SqlDbType.VarChar, 100).Value = _info.PhotoName.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@PortletCode", SqlDbType.VarChar, 500).Value = _info.PortletCode.ToNull();
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = _info.IsAdmin.ToNull().ToXSS();

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
        /// 심재운
        /// 회사별 포틀릿 복사하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_multiCompanyID">The _multi company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool AddCopy(int _companyID, int _multiCompanyID, int _categoryID, int _portletID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_InsCopy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@MultiCompanyID", SqlDbType.Int).Value = _multiCompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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
        /// 심재운
        /// 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(PortletInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _info.PortletID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@PortletType", SqlDbType.Char, 1).Value = _info.PortletType.ToNull();
                    cmd.Parameters.Add("@UIType", SqlDbType.Char, 1).Value = _info.UIType.ToNull();
                    cmd.Parameters.Add("@UITemplate", SqlDbType.Char, 1).Value = _info.UITemplate.ToNull();
                    cmd.Parameters.Add("@UIURL", SqlDbType.VarChar, 200).Value = _info.UIURL.ToNull();
                    cmd.Parameters.Add("@DataType", SqlDbType.Char, 1).Value = _info.DataType.ToNull();
                    cmd.Parameters.Add("@DataURL", SqlDbType.VarChar, 2000).Value = _info.DataURL.ToNull();
                    cmd.Parameters.Add("@LinkType", SqlDbType.Char, 1).Value = _info.LinkType.ToNull();
                    cmd.Parameters.Add("@LinkPopupSize", SqlDbType.VarChar, 50).Value = _info.LinkPopupSize.ToNull().ToXSS();
                    cmd.Parameters.Add("@LinkURL", SqlDbType.VarChar, 200).Value = _info.LinkURL.ToNull();
                    cmd.Parameters.Add("@IsPesonalization", SqlDbType.Char, 1).Value = _info.IsPesonalization.ToNull();
                    cmd.Parameters.Add("@ActivePortlet", SqlDbType.Char, 1).Value = _info.ActivePortlet.ToNull();
                    cmd.Parameters.Add("@SendMethod", SqlDbType.Char, 1).Value = _info.SendMethod.ToNull();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@PhotoName", SqlDbType.VarChar, 100).Value = _info.PhotoName.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@PortletCode", SqlDbType.VarChar, 500).Value = _info.PortletCode.ToNull();
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = _info.IsAdmin.ToNull().ToXSS();

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
        /// 심재운
        /// 비활성화 및 활성화
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <returns></returns>
        public bool ModActive(int _companyID, int _categoryID, int _portletID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_EIP_Portlet_UpdByActive", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;

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
        /// 심재운
        /// 폴더 이동
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <returns></returns>
        public bool ModMove(int _companyID, int _categoryID, string _portletID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_UpdMove", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.VarChar, 500).Value = _portletID;

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
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _portletID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;
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
                return false;
            }
        }

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 포틀릿 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_portlet">The _menu ID.</param>
        /// <returns></returns>
        public List<BE.PortletInfo> GetPortlet(int _companyID, int _categoryID, int _portlet)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Portlet_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portlet;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetPortletList(reader);
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
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_portletList">The _portlet list.</param>
        /// <param name="_roleList">The _role list.</param>
        /// <returns></returns>
        public List<BE.PortletInfo> GetMainPortletList(int _companyID, int _userID, string _portletList, string _roleList)
        {
            try
            {
                List<PortletInfo> list = new List<PortletInfo>();

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_SelGetSubRoleList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PortletList", SqlDbType.VarChar, 800).Value = _portletList;
                    cmd.Parameters.Add("@RoleList", SqlDbType.VarChar, 800).Value = _roleList;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PortletInfo
                            {
                                CompanyID = (reader["CompanyID"] == DBNull.Value) ? -1 : reader["CompanyID"].ToString().ToNumber<int>(),
                                PortletID = (reader["PortletID"] == DBNull.Value) ? -1 : reader["PortletID"].ToString().ToNumber<int>(),
                                CategoryID = (reader["CategoryID"] == DBNull.Value) ? -1 : reader["CategoryID"].ToString().ToNumber<int>(),
                                NameBase = reader["NameBase"].ToString().ToNull(),
                                NameKo = reader["NameKo"].ToString().ToNull(),
                                NameEn = reader["NameEn"].ToString().ToNull(),
                                NameJa = reader["NameJa"].ToString().ToNull(),
                                NameFr = reader["NameFr"].ToString().ToNull(),
                                NameZhcn = reader["NameZhcn"].ToString().ToNull(),
                                NameZhtw = reader["NameZhtw"].ToString().ToNull(),
                                PortletType = reader["PortletType"].ToString().ToNull(),
                                UIType = reader["UIType"].ToString().ToNull(),
                                UITemplate = reader["UITemplate"].ToString().ToNull(),
                                UIURL = reader["UIURL"].ToString().ToNull(),
                                DataType = reader["DataType"].ToString().ToNull(),
                                DataURL = reader["DataURL"].ToString().ToNull(),
                                LinkType = reader["LinkType"].ToString().ToNull(),
                                LinkPopupSize = reader["LinkPopupSize"].ToString().ToNull(),
                                LinkURL = reader["LinkURL"].ToString().ToNull(),
                                IsPesonalization = reader["IsPesonalization"].ToString().ToNull(),
                                ActivePortlet = reader["ActivePortlet"].ToString().ToNull(),
                                Description = reader["Description"].ToString().ToNull(),
                                PhotoName = reader["PhotoName"].ToString().ToNull(),
                                IsAdmin = reader["IsAdmin"].ToString().ToNull(),
                                RoleIDCnt = (reader["RoleIDCnt"] == DBNull.Value) ? -1 : reader["RoleIDCnt"].ToString().ToNumber<int>(),
                                ParamValue = reader["ParamValue"].ToString().ToNull(),
                                TPortletID = reader["TPortletID"].ToString().ToNumber<int>()
                            });
                        }
                    }
                }

                HttpContext ctx = HttpContext.Current;
                ctx.Cache.Remove("RolePortlet_" + _companyID);
                ctx.Cache.Insert("RolePortlet" + _companyID, list, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);

                return list;
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
        /// 나의 권한 포틀릿 리스트 가져오기 (관리자용만)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_portletList">The _portlet list.</param>
        /// <param name="_roleList">The _role list.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <returns></returns>
        public List<BE.PortletInfo> GetPortletMyRoleList(int _companyID, int _userID, string _portletList, string _roleList, int _portletID)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                List<PortletInfo> list = new List<PortletInfo>();

                if (context.Cache["RolePortlet" + _companyID] == null)
                {
                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        var cmd = new SqlCommand("DP_EIP_Portlet_SelGetMyRoleList", cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                        cmd.Parameters.Add("@PortletList", SqlDbType.VarChar, 800).Value = _portletList;
                        cmd.Parameters.Add("@RoleList", SqlDbType.VarChar, 800).Value = _roleList;
                        cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;

                        cn.Open();
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new PortletInfo
                                {
                                    CompanyID = (reader["CompanyID"] == DBNull.Value) ? -1 : reader["CompanyID"].ToString().ToNumber<int>(),
                                    PortletID = (reader["PortletID"] == DBNull.Value) ? -1 : reader["PortletID"].ToString().ToNumber<int>(),
                                    CategoryID = (reader["CategoryID"] == DBNull.Value) ? -1 : reader["CategoryID"].ToString().ToNumber<int>(),
                                    NameBase = reader["NameBase"].ToString().ToNull(),
                                    NameKo = reader["NameKo"].ToString().ToNull(),
                                    NameEn = reader["NameEn"].ToString().ToNull(),
                                    NameJa = reader["NameJa"].ToString().ToNull(),
                                    NameFr = reader["NameFr"].ToString().ToNull(),
                                    NameZhcn = reader["NameZhcn"].ToString().ToNull(),
                                    NameZhtw = reader["NameZhtw"].ToString().ToNull(),
                                    PortletType = reader["PortletType"].ToString().ToNull(),
                                    UIType = reader["UIType"].ToString().ToNull(),
                                    UITemplate = reader["UITemplate"].ToString().ToNull(),
                                    UIURL = reader["UIURL"].ToString().ToNull(),
                                    DataType = reader["DataType"].ToString().ToNull(),
                                    DataURL = reader["DataURL"].ToString().ToNull(),
                                    LinkType = reader["LinkType"].ToString().ToNull(),
                                    LinkPopupSize = reader["LinkPopupSize"].ToString().ToNull(),
                                    LinkURL = reader["LinkURL"].ToString().ToNull(),
                                    IsPesonalization = reader["IsPesonalization"].ToString().ToNull(),
                                    ActivePortlet = reader["ActivePortlet"].ToString().ToNull(),
                                    Description = reader["Description"].ToString().ToNull(),
                                    PhotoName = reader["PhotoName"].ToString().ToNull(),
                                    IsAdmin = reader["IsAdmin"].ToString().ToNull(),
                                    RoleIDCnt = (reader["RoleIDCnt"] == DBNull.Value) ? -1 : reader["RoleIDCnt"].ToString().ToNumber<int>(),
                                    ParamValue = reader["ParamValue"].ToString().ToNull(),
                                    TPortletID = reader["TPortletID"].ToString().ToNumber<int>()
                                });
                            }
                        }
                    }
                }
                else if (context.Cache["RolePortlet" + _companyID] != null)
                {
                    list = (List<BE.PortletInfo>)context.Cache["RolePortlet" + _companyID];
                }

                return list;
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
        /// 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _categoryID, string _searchKey, string _searchValue)
        {
            try
            {
                List<int> userList = new List<int>();

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_SelPageCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey.ToNull().ToXSS();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();

                    cn.Open();

                    return Convert.ToInt32(ExecuteScalar(cmd));
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
        /// 페이징 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _categoryID,
            string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _order)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

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
        /// 작성자 : shine
        /// 포탈화면의 포틀릿 추가의 페이징 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <param name="_roleList">The _role list.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _categoryID, int _tabID, string _roleList,
            string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _order, bool _active)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_SelTabPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;
                    cmd.Parameters.Add("@RoleList", SqlDbType.VarChar, 800).Value = _roleList.ToNull().ToXSS();
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey.ToNull().ToXSS();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order.ToNull().ToXSS();
                    cmd.Parameters.Add("@Active", SqlDbType.Char, 1).Value = (_active == true) ? "1" : "0";

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
        /// 작성자 : shine
        /// 포탈화면의 포틀릿 추가의 페이징 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_roleList">The _role list.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public int GetPageCount(int _companyID, int _categoryID, int _tabID, int _userID, string _roleList,
            string _searchKey, string _searchValue, bool _active)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_SelTabPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _tabID;
                    cmd.Parameters.Add("@RoleList", SqlDbType.VarChar, 800).Value = _roleList;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey.ToNull().ToXSS();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                    cmd.Parameters.Add("@Active", SqlDbType.Char, 1).Value = (_active == true) ? "1" : "0";

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
        /// 작성자 : shine
        /// 포틀릿코드의 중복 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <param name="_portletCode">The _portlet code.</param>
        /// <returns></returns>
        public int GetPotletCodeCount(int _companyID, int _portletID, string _portletCode)
        {
            try
            {
                List<int> userList = new List<int>();

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_SelByPortletCode", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;
                    cmd.Parameters.Add("@PortletCode", SqlDbType.VarChar, 500).Value = _portletCode;

                    cn.Open();

                    return Convert.ToInt32(ExecuteScalar(cmd));
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
        /// 개인별 포틀릿, 페이징 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <returns></returns>
        public DataTableReader GetUserPageList(int _companyID, int _categoryID, bool _isAdmin, int _userID,
            string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _order, int _tabID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_SelUserPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = (_isAdmin) ? "1" : "0";
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;

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
        /// 개인별 포틀릿 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <returns></returns>
        public int GetUserPageListCount(int _companyID, int _categoryID, bool _isAdmin, int _userID, string _searchKey, string _searchValue, int _tabID)
        {
            try
            {
                List<int> userList = new List<int>();

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Portlet_SelUserPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = (_isAdmin) ? "1" : "0";
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey.ToNull().ToXSS();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;

                    cn.Open();

                    return Convert.ToInt32(ExecuteScalar(cmd));
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