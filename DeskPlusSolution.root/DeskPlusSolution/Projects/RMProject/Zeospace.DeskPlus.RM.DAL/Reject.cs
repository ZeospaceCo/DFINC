using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.RM.BE;

namespace Zeospace.DeskPlus.RM.DAL
{
    public class Reject : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-02-28
        /// 작성목적 : 거부 사유 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public DataTableReader GetRejectList(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Reject_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;
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

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-02-28
        /// 작성목적 : 거부 사유 입력하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(RejectInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reject_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _info.ReservationID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 2000).Value = _info.Content.ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;

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

        #endregion ADD()

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-02
        /// 작성목적 : 거부 사유 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Reject_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;                    

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

        #endregion 
    }
}
