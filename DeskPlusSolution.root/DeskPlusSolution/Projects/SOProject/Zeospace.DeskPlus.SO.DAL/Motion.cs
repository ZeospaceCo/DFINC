using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.SO.BE;

namespace Zeospace.DeskPlus.SO.DAL
{
    public class Motion : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2016-02-24
        /// 작성목적 : 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetMotion(int _companyID, int _thingsID, string _selDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    //string query = AppSettingHelper.GetDynamicQuery(AppSettingType.SEARCH, "QRY_SO_Motion_Sel");
                    string query = @"SELECT TOP 24 * FROM [dbo].[DP_SO_Motion] WHERE CompanyID = @CompanyID AND ThingsID = @ThingsID AND ActionType='GetTemperature' AND WriteDateYMD <= @SelDate ORDER BY MotionID DESC";

                    //query = String.Format(query, _companyID, _thingsID, _selDate.Replace("-", ""));

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ThingsID", SqlDbType.Int).Value = _thingsID;
                    cmd.Parameters.Add("@SelDate", SqlDbType.Char, 8).Value = _selDate.Replace("-", "");
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMotion");
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
        /// 작성일자 : 2016-02-24
        /// 작성목적 : 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(MotionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_SO_Motion_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ThingsID", SqlDbType.Int).Value = _info.ThingsID;
                    cmd.Parameters.Add("@ActionType", SqlDbType.NVarChar, 50).Value = _info.ActionType.ToXSS();
                    cmd.Parameters.Add("@Temperature", SqlDbType.NVarChar, 5).Value = _info.Temperature.ToXSS();
                    cmd.Parameters.Add("@Battery", SqlDbType.NVarChar, 3).Value = _info.Battery.ToXSS();
                    cmd.Parameters.Add("@Motion", SqlDbType.NVarChar, 50).Value = _info.Motion.ToXSS();

                    cn.Open();

                    return Convert.ToInt32(ExecuteScalar(cmd));
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return 0;
            }
        }

        #endregion ADD()
    }
}
