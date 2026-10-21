using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.HR.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Configuration;


namespace Zeospace.DeskPlus.HR.DAL
{
    public class WorkEnv : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET

        /// <summary>
        /// 기본근무시간 설정 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeCabinet">if set to <c>true</c> [_active cabinet].</param>
        /// <returns></returns>
        public DataTableReader GetHREnv(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.HR, "QRY_HR_WorkEnv_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <returns></returns>
        public bool Mod(BE.WorkEnvInfo _workEnv)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_workEnv.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_HR_WorkEnv_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _workEnv.CompanyID;
                    cmd.Parameters.Add("@WorkYN", SqlDbType.Char, 1).Value = _workEnv.WorkYN;
                    cmd.Parameters.Add("@WSTime", SqlDbType.DateTime).Value = _workEnv.WSTime;
                    cmd.Parameters.Add("@WCTime", SqlDbType.DateTime).Value = _workEnv.WCTime;
                    cmd.Parameters.Add("@Scope", SqlDbType.Int).Value = _workEnv.Scope;
                    cmd.Parameters.Add("@WCTimeYN", SqlDbType.Char, 1).Value = _workEnv.WCTimeYN;
                    cmd.Parameters.Add("@WCTimeExec", SqlDbType.DateTime).Value = _workEnv.WCTimeExec;
                    cmd.Parameters.Add("@CecomYN", SqlDbType.Char, 1).Value = _workEnv.CecomYN;

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

        #endregion MOD()
    }
}
