using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class ListUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        ///
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        private ListUserInfo GetCabinetUser(IDataReader _reader)
        {
            return new ListUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListUserID = (_reader["ListUserID"] == DBNull.Value) ? -1 : _reader["ListUserID"].ToString().ToNumber<int>(),
                IsType = _reader["IsType"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                PageSize = (_reader["PageSize"] == DBNull.Value) ? -1 : _reader["PageSize"].ToString().ToNumber<int>()
            };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected List<ListUserInfo> GetCabinetUserList(IDataReader reader)
        {
            List<ListUserInfo> list = new List<ListUserInfo>();
            while (reader.Read())
                list.Add(GetCabinetUser(reader));
            return list;
        }

        #endregion

        #region GET()


        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-10
        /// 작성목적 : 메뉴별 사용자 페이징 설정 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_isType"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public List<ListUserInfo> GetList(int _companyID, string _isType, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ListUser_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@IsType", SqlDbType.Char, 2).Value = _isType;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCabinetUserList(reader);
                }
            }
        }


        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-10
        /// 작성목적 : 메뉴별 사용자 페이징 정보 설정하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AddMod(ListUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListUser_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@IsType", SqlDbType.Char, 2).Value = _info.IsType;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;                  

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
