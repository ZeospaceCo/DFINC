using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class TabPortlet : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private TabPortletInfo GetTabPortlet(IDataReader _reader)
        {
            return new TabPortletInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                TabPortletID = (_reader["TabPortletID"] == DBNull.Value) ? -1 : _reader["TabPortletID"].ToString().ToNumber<int>(),
                TabID = (_reader["TabID"] == DBNull.Value) ? -1 : _reader["TabID"].ToString().ToNumber<int>(),
                PortletID = (_reader["PortletID"] == DBNull.Value) ? -1 : _reader["PortletID"].ToString().ToNumber<int>(),
                ColumnID = _reader["ColumnID"].ToString().ToNull(),
                RowSequence = (_reader["RowSequence"] == DBNull.Value) ? -1 : _reader["RowSequence"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                IsAdmin = _reader["IsAdmin"].ToString().ToNull(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                PortletCode = _reader["PortletCode"].ToString().ToNull(),
                PortletType = _reader["PortletType"].ToString().ToNull(),
                UIType = _reader["UIType"].ToString().ToNull(),
                UITemplate = _reader["UITemplate"].ToString().ToNull(),
                UIURL = _reader["UIURL"].ToString().ToNull(),
                DataType = _reader["DataType"].ToString().ToNull(),
                DataURL = _reader["DataURL"].ToString().ToNull(),
                LinkType = _reader["LinkType"].ToString().ToNull(),
                LinkPopupSize = _reader["LinkPopupSize"].ToString().ToNull(),
                LinkURL = _reader["LinkURL"].ToString().ToNull(),
                IsPesonalization = _reader["IsPesonalization"].ToString().ToNull(),
                ActivePortlet = _reader["ActivePortlet"].ToString().ToNull(),
                Description = _reader["Description"].ToString().ToNull(),
                PhotoName = _reader["PhotoName"].ToString().ToNull(),
                IsMinimize = _reader["IsMinimize"].ToString().ToNull(),
                PageSize = _reader["PageSize"].ToString().ToNumber<int>(),
                EtcParam = _reader["EtcParam"].ToString().ToNull(),
                RoleIDCnt = _reader["RoleIDCnt"].ToString().ToNumber<int>(),
                ParamValue = _reader["ParamValue"].ToString().ToNull(),
                TargetPortletID = _reader["TargetPortletID"].ToString()
            };
        }

        /// <summary>
        /// Gets the menu data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<TabPortletInfo> GetTabPortletList(IDataReader reader)
        {
            List<TabPortletInfo> list = new List<TabPortletInfo>();

            while (reader.Read())
                list.Add(GetTabPortlet(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 탭별 포틀릿 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_roleList">The _role list.</param>
        /// <returns></returns>
        public List<TabPortletInfo> GetTabPortlet(int _companyID, int _tabID, int _userID, string _roleList)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_TabPortlet_Sel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@RoleList", SqlDbType.VarChar, 800).Value = _roleList;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetTabPortletList(reader);
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
        /// 탭포틀릿 순서 저장
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <param name="_columnID">The _column ID.</param>
        /// <param name="_portletCodeList">The _portlet code list.</param>
        /// <returns></returns>
        public bool ModrowSequence(int _companyID, int _tabID, string _columnID, string _portletIDList)
        {
            try
            {
                string[] portletIDListStr = _portletIDList.Split(',');

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    for (int i = 0; i < portletIDListStr.Length; i++)
                    {
                        var cmd = new SqlCommand("DP_EIP_TabPortlet_UpRowSeq", cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;
                        cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = portletIDListStr[i];
                        cmd.Parameters.Add("@ColumnID", SqlDbType.VarChar, 50).Value = _columnID;
                        cmd.Parameters.Add("@RowSequence", SqlDbType.Int).Value = (i + 1);

                        cmd.ExecuteNonQuery();
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
        /// 탭의 포틀릿 추가하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_tabID">The _tab ID.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <param name="_columnID">The _column ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool AddTabPortlet(int _companyID, int _tabID, int _portletID, string _columnID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_EIP_TabPortlet_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _tabID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;
                    cmd.Parameters.Add("@ColumnID", SqlDbType.VarChar, 50).Value = _columnID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cmd.ExecuteNonQuery();
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
        /// 탭포틀릿 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_tabPortletID">The _tab portlet ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _tabPortletID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_EIP_TabPortlet_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TabPortletID", SqlDbType.Int).Value = _tabPortletID;

                    cmd.ExecuteNonQuery();
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

        #endregion 가져오기
    }
}