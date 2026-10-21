using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Search.BE;

namespace Zeospace.DeskPlus.Search.DAL
{
    public class IndexLogDetail : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2013-11-28
        /// 작성목적 : 인덱스 로그 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataTableReader GetLog(int _companyID, int _logID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.SEARCH, "QRY_SEARCH_IndexLogDetail_SelLogID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@LogID", SqlDbType.Int).Value = _logID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2013-11-28
        /// 작성목적 : 인덱스 로그 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataTableReader GetLog(int _companyID, string _logIDs)
        {
            if (_companyID == 0 || String.IsNullOrEmpty(_logIDs)) return null;
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.SEARCH, "QRY_SEARCH_IndexLogDetail_SelLogIDs");
                    query = string.Format(query, _logIDs.ToInject());
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-28
        /// 작성목적 : 인덱스 로그 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(IndexLogDetailInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_SEARCH_IndexLogDetail_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@LogID", SqlDbType.Int).Value = _info.LogID;
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 500).Value = _info.Message;

                    cn.Open();

                    return ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return 0;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-28
        /// 작성목적 : 환경설정 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(IndexLogInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_SEARCH_IndexLog_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@LogID", SqlDbType.Int).Value = _info.LogID;
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar, 50).Value = _info.Category.ToXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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

        #endregion ADD()
    }
}