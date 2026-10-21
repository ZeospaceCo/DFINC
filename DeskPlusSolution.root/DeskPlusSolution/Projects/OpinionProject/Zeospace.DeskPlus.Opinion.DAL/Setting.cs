using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Opinion.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Opinion.DAL
{
    public class Setting : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.SettingInfo GetSetting(IDataReader _idr)
        {
            return new SettingInfo
            {
                CompanyID = (_idr["CompanyID"] == DBNull.Value) ? -1 : _idr["CompanyID"].ToString().ToNumber<int>(),
                UserID = (_idr["UserID"] == DBNull.Value) ? -1 : _idr["UserID"].ToString().ToNumber<int>(),
                MenuInfo = _idr["MenuInfo"].ToString(),
                ListType = _idr["ListType"].ToString(),
                PageSize = (_idr["PageSize"] == DBNull.Value) ? -1 : _idr["PageSize"].ToString().ToNumber<int>(),
                WriteDate = (_idr["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_idr["WriteDate"]
            };
        }

        protected List<BE.SettingInfo> GetSettingList(IDataReader reader)
        {
            List<BE.SettingInfo> list = new List<SettingInfo>();
            while (reader.Read())
            {
                list.Add(GetSetting(reader));
            }
            return list;
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 설정추가
        /// </summary>
        /// <param name="_unitItemInfo"></param>
        /// <returns></returns>
        public bool Add(SettingInfo _settingInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_settingInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Setting_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _settingInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _settingInfo.UserID;
                    cmd.Parameters.Add("@MenuInfo", SqlDbType.NVarChar, 50).Value = _settingInfo.MenuInfo;
                    cmd.Parameters.Add("@ListType", SqlDbType.Char, 1).Value = _settingInfo.ListType;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _settingInfo.PageSize;

                    cn.Open();
                    ExecuteScalar(cmd);
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

        #region 가져오기
        /// <summary>
        /// 메뉴별 사용자환경설정정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_menuInfo"></param>
        /// <returns></returns>
        public List<BE.SettingInfo> GetSetting(int _companyID, int _userID, string _menuInfo)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Setting_SelByUserID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@MenuInfo", SqlDbType.NVarChar, 50).Value = _menuInfo;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSettingList(reader);
                }
            }
        }

        #endregion
    }
}
