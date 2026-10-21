using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.MDM.BE;

namespace Zeospace.DeskPlus.MDM.DAL
{
    public class Portal : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET 

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2018-12-17
        /// 작성목적 : 메인 설정 메뉴 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.MDM, "QRY_MDM_Portal_Sel");
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
        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2018-12-18
        /// 작성목적 : 메인 설정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_portalID"></param>
        /// <param name="_orders"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _portalID, int _orders)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_Portal_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PortalID", SqlDbType.Int).Value = _portalID;
                    cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = _orders;
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
        #endregion 
    }
}
