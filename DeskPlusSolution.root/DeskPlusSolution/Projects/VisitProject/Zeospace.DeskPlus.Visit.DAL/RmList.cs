using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Visit.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Visit.DAL
{
    public class RmList : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 예약자원 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _reserveID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_RmList_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;

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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 예약자원 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <returns></returns>
        public DataTableReader GetRmList(int _companyID, int _reserveID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_RmList_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;

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
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 예약자원 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public DataTableReader GetRmReserveList(int _companyID, int _reserveGroupID, string _startDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_RmList_SelByStartDate");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _startDate;

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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 예약자원 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public DataTableReader GetRmAllList(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_RmList_SelByGroupID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;                    

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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 예약자원 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <returns></returns>
        public DataTableReader GetRmListByReserveID(int _companyID, int _reserveID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_RmList_SelByReserveID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;

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
        /// 작성일자 : 2014-02-20
        /// 작성목적 : 예약자원 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public DataTableReader GetRM(int _companyID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_RmList_SelByReservationID");
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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 방문 예약 자원 설정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _reserveID, int _reservationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_RmList_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _reservationID;
                    
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
        /// 작성일자 : 2014-02-13
        /// 작성목적 : 자원 리스트 삭제
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
                    var cmd = new SqlCommand("DP_VIS_RmList_Del", cn);
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
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 자원 예약 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <returns></returns>
        public bool DelAll(int _companyID, int _reserveID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_RmList_DelAll", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveID", SqlDbType.Int).Value = _reserveID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 자원 예약 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <returns></returns>
        public bool DelAllGroupID(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_RmList_DelAllGroupID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
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
