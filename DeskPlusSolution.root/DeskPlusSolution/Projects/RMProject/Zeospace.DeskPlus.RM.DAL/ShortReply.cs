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
    public class ShortReply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 댓글 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public DataTableReader GetShortReplyList(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_ShortReply_Sel");
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 특정 댓글 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_shortReplyID"></param>
        /// <returns></returns>
        public DataTableReader GetShortReplyListID(int _companyID, int _reservationID, int _shortReplyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_ShortReply_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortReplyID;
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
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 댓글 저장하기
        /// </summary>
        /// <param name="_shortreplyinfo"></param>
        /// <returns></returns>
        public int Add(ShortReplyInfo _shortreplyinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_shortreplyinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_RM_ShortReply_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _shortreplyinfo.CompanyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _shortreplyinfo.ReservationID;                    
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 200).Value = _shortreplyinfo.Content.ToXSS();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _shortreplyinfo.UserID;

                    cn.Open();

                    return ExecuteNonQuery(cmd);
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

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 댓글 수정하기
        /// </summary>
        /// <param name="_shortreplyinfo"></param>
        /// <returns></returns>
        public bool Mod(ShortReplyInfo _shortreplyinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_shortreplyinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_RM_ShortReply_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _shortreplyinfo.CompanyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _shortreplyinfo.ReservationID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortreplyinfo.ShortReplyID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 200).Value = _shortreplyinfo.Content.ToXSS();                                        

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

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 댓글 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_shortReplyID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reservationID, int _shortReplyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_ShortReply_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortReplyID;

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
