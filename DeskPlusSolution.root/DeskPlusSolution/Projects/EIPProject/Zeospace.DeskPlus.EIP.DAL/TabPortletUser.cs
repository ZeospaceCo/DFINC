using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class TabPortletUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private TabPortletUserInfo GetTabPortletUser(IDataReader _reader)
        {
            return new TabPortletUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                TabPortletID = (_reader["TabPortletID"] == DBNull.Value) ? -1 : _reader["TabPortletID"].ToString().ToNumber<int>(),
                TabID = (_reader["TabID"] == DBNull.Value) ? -1 : _reader["TabID"].ToString().ToNumber<int>(),
                PortletID = (_reader["PortletID"] == DBNull.Value) ? -1 : _reader["PortletID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                IsMinimize = _reader["IsMinimize"].ToString().ToNull(),
                PageSize = _reader["PageSize"].ToString().ToNumber<int>(),
                EtcParam = _reader["EtcParam"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the menu data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<TabPortletUserInfo> GetTabPortletUserList(IDataReader reader)
        {
            List<TabPortletUserInfo> list = new List<TabPortletUserInfo>();

            while (reader.Read())
                list.Add(GetTabPortletUser(reader));
            return list;
        }

        #endregion DataReader

        #region 추가 / 수정 / 삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 사용자별 포틀릿 설정정보 추가 및 변경
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddMod(TabPortletUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_TabPortletUser_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@TabPortletID", SqlDbType.Int).Value = _info.TabPortletID;
                    cmd.Parameters.Add("@TabID", SqlDbType.Int).Value = _info.TabID;
                    cmd.Parameters.Add("@PortletID", SqlDbType.VarChar, 800).Value = _info.PortletID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@IsMinimize", SqlDbType.Char, 1).Value = _info.IsMinimize;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@EtcParam", SqlDbType.NVarChar, 500).Value = _info.EtcParam;

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

        #endregion 추가 / 수정 / 삭제
    }
}