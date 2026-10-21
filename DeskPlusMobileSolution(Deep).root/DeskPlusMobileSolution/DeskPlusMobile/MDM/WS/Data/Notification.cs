using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.MDM.WS.BE;

namespace Zeospace.DeskPlusMobile.Web.MDM.WS.Data
{
    public class Notification
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_notificationInfo"></param>
        /// <returns></returns>
        public bool NotificationAdd(NotificationInfo _notificationInfo)
        {
            SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int),
                                               new SqlParameter("@ReceiverID", SqlDbType.Int),
											   new SqlParameter("@Message", SqlDbType.NVarChar, 200),                                               
                                               new SqlParameter("@TargetUrl", SqlDbType.NVarChar, 500),
                                               new SqlParameter("@KindSubject", SqlDbType.NVarChar, 50),
                                               new SqlParameter("@WriteUserID", SqlDbType.Int),
                                               new SqlParameter("@WriteDate", SqlDbType.DateTime)
                };

            parms[0].Value = _notificationInfo.CompanyID;
            parms[1].Value = _notificationInfo.ReceiverID;
            parms[2].Value = _notificationInfo.Message;
            parms[3].Value = _notificationInfo.TargetUrl;
            parms[4].Value = _notificationInfo.KindSubject;
            parms[5].Value = _notificationInfo.WriteUserID;
            parms[6].Value = _notificationInfo.WriteDate;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_MDM_NotificationBox_Ins", parms);
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("NotificationAdd(NotificationInfo): " + errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 이중호
        /// 작성일자 : 2019-08-22
        /// 작성목적 : 총 알람 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_receiverID"></param>
        /// <returns></returns>
        public int GetTotalCnt(string _companyID, int _receiverID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@ReceiverID", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _receiverID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_MDM_NotificationBox_SelCnt", parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetTotalCnt: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이중호
        /// 작성일자 : 2019-08-22
        /// 작성목적 : 총 알람 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_receiverID"></param>
        /// <returns></returns>
        public int GetNotReadListCount(string _companyID, int _receiverID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@ReceiverID", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _receiverID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_MDM_NotificationBox_SelNotReadCnt", parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetNotReadListCount: " + errMsg);
                throw;
            }
        }
    }
}