using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Community.BE;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.DAL
{    
    public class Setting : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        ///
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        private SettingInfo GetSetting(IDataReader _reader)
        {
            return new SettingInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CommunityID = (_reader["CommunityID"] == DBNull.Value) ? -1 : _reader["CommunityID"].ToString().ToNumber<int>(),
                MenuID = (_reader["MenuID"] == DBNull.Value) ? -1 : _reader["MenuID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                PageSize = (_reader["PageSize"] == DBNull.Value) ? -1 : _reader["PageSize"].ToString().ToNumber<int>(),
                Types = _reader["Types"].ToString().ToNull()
            };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected List<SettingInfo> GetSettingList(IDataReader reader)
        {
            List<SettingInfo> list = new List<SettingInfo>();
            while (reader.Read())
                list.Add(GetSetting(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-06-18
        /// 작성목적 : 리스트별 사용자 설정 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_menuID"></param>
        /// <param name="_userID"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public List<SettingInfo> GetList(int _companyID, int _communityID, int _menuID, int _userID, string _types)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMUNITY, "QRY_CMN_Setting_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _communityID;
                cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _types;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSettingList(reader);
                }
            }
        }


        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-18
        /// 작성목적 : 리스트별 사용자 정보 설정하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AddMod(SettingInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_Setting_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _info.CommunityID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _info.MenuID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;                    
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

        #endregion
    }
}
