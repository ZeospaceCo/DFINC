using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.HR.BE;
using System.Configuration;

namespace Zeospace.DeskPlus.HR.DAL
{
    public class IP : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the menu data link.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private IPInfo GetIP(IDataReader _reader)
        {
            return new IPInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                IPID = (_reader["IPID"] == DBNull.Value) ? -1 : _reader["IPID"].ToString().ToNumber<int>(),
                StartIP = _reader["StartIP"].ToString().ToNull(),
                EndIP = _reader["EndIP"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the menu data link list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<IPInfo> GetIPList(IDataReader reader)
        {
            List<IPInfo> list = new List<IPInfo>();

            while (reader.Read())
                list.Add(GetIP(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// kkzz52
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(IPInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_HR_IP_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@StartIP", SqlDbType.Char, 12).Value = _info.StartIP.ToNull().ToXSS();
                    cmd.Parameters.Add("@EndIP", SqlDbType.Char, 12).Value = _info.EndIP.ToNull().ToXSS();

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
        /// kkzz52
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_menuIPID">The IPID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _IPID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_HR_IP_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@IPID", SqlDbType.Int).Value = _IPID;

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
        /// 작성자 : kkzz52
        /// 사내IP 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<BE.IPInfo> GetIP(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.HR, "QRY_HR_IP_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetIPList(reader);
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
