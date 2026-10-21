using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class PortletDataLink : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private PortletDataLinkInfo GetPortletDataLink(IDataReader _reader)
        {
            return new PortletDataLinkInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                PortletDataLinkID = (_reader["PortletDataLinkID"] == DBNull.Value) ? -1 : _reader["PortletDataLinkID"].ToString().ToNumber<int>(),
                PortletID = (_reader["PortletID"] == DBNull.Value) ? -1 : _reader["PortletID"].ToString().ToNumber<int>(),
                EncriptType = _reader["EncriptType"].ToString().ToNull(),
                EncriptValue = _reader["EncriptValue"].ToString().ToNull(),
                TargetType = _reader["TargetType"].ToString().ToNull(),
                TargetDataType = _reader["TargetDataType"].ToString().ToNull(),
                LinkName = _reader["LinkName"].ToString().ToNull(),
                LinkValue = _reader["LinkValue"].ToString().ToNull(),
                LinkType = _reader["LinkType"].ToString().ToNull(),
                Description = _reader["Description"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the menu data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<PortletDataLinkInfo> GetPortletDataLinkList(IDataReader reader)
        {
            List<PortletDataLinkInfo> list = new List<PortletDataLinkInfo>();

            while (reader.Read())
                list.Add(GetPortletDataLink(reader));
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
        public int Add(PortletDataLinkInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletDataLink_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _info.PortletID;
                    cmd.Parameters.Add("@EncriptType", SqlDbType.Char, 1).Value = _info.EncriptType.ToNull();
                    cmd.Parameters.Add("@EncriptValue", SqlDbType.NVarChar, 100).Value = _info.EncriptValue.ToNull();
                    cmd.Parameters.Add("@TargetType", SqlDbType.Char, 1).Value = _info.TargetType.ToNull();
                    cmd.Parameters.Add("@TargetDataType", SqlDbType.Char, 1).Value = _info.TargetDataType.ToNull();
                    cmd.Parameters.Add("@LinkName", SqlDbType.VarChar, 200).Value = _info.LinkName.ToNull().ToXSS();
                    cmd.Parameters.Add("@LinkValue", SqlDbType.NVarChar, 300).Value = _info.LinkValue.ToNull();
                    cmd.Parameters.Add("@LinkType", SqlDbType.Char, 2).Value = _info.LinkType.ToNull();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 200).Value = _info.Description.ToNull().ToXSS();

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
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_portletDataLinkID">The _menu data link ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _portletDataLinkID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_PortletDataLink_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletDataLinkID", SqlDbType.Int).Value = _portletDataLinkID;

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
        /// 작성자 : 심재운
        /// data link 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_portletID">The _portlet ID.</param>
        /// <returns></returns>
        public List<BE.PortletDataLinkInfo> GetDataLink(int _companyID, int _portletID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_PortletDataLink_SelByPortletID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.Int).Value = _portletID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetPortletDataLinkList(reader);
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