using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class Status : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        ///
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        private StatusInfo GetStatus(IDataReader _reader)
        {
            return new StatusInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                StatusID = (_reader["StatusID"] == DBNull.Value) ? -1 : _reader["StatusID"].ToString().ToNumber<int>(),
                IsType = _reader["IsType"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),                
                Status = _reader["Status"].ToString().ToNull()
            };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected List<StatusInfo> GetStatusList(IDataReader reader)
        {
            List<StatusInfo> list = new List<StatusInfo>();
            while (reader.Read())
                list.Add(GetStatus(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-08
        /// 작성목적 : 사용자 별 상태값 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_isType"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public List<StatusInfo> GetList(int _companyID, string _isType, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_Status_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@IsType", SqlDbType.Char, 2).Value = _isType;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetStatusList(reader);
                }
            }
        }


        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-10
        /// 작성목적 : 사용자 정보 상태값 설정하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AddMod(StatusInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Status_InsUpd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@IsType", SqlDbType.Char, 2).Value = _info.IsType;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar,20).Value = _info.Status;

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
