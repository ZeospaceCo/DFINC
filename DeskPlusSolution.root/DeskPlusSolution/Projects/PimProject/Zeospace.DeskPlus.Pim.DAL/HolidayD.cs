using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.DAL
{
    public class HolidayD : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()
        /// <summary>
        /// 미반반복일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>        
        public DataTableReader GetList(int companyID, string searchField, string searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution()))
            {
                var cmd = new SqlCommand("DP_PIM_HolidayD_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 20).Value = searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 20).Value = searchValue;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

       /// <summary>
        /// 데이터 체크
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>  
        public int GetCheckCount(int _companyID, string _solarLunar, string _holiDay)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_HolidayD_CheckCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SolarLunar", SqlDbType.Char, 1).Value = _solarLunar;
                    cmd.Parameters.Add("@HoliDay", SqlDbType.NVarChar, 10).Value = _holiDay;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 데이터 체크
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>  
        public int GetCheckCountByHoliDayID(int _companyID, int _holiDayID, string _solarLunar, string _holiDay)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_HolidayD_CheckCnt_SelByHoliDayID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@HoliDayID", SqlDbType.Int).Value = _holiDayID;
                    cmd.Parameters.Add("@SolarLunar", SqlDbType.Char, 1).Value = _solarLunar;
                    cmd.Parameters.Add("@HoliDay", SqlDbType.NVarChar, 10).Value = _holiDay;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// ID로 데이터 가져오기
        /// </summary>
        public DataTableReader GetDataByHoliDayID(int _companyID, int _holiDayID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PIM_HolidayD_SelByHoliDayID", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@HoliDayID", SqlDbType.Int).Value = _holiDayID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
        #endregion

        #region ADD()
        /// <summary>
        /// Adds the list.
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Add(BE.HolidayDInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_HolidayD_Ins", cn);


                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@HoliDayKind", SqlDbType.Char, 1).Value = _info.HoliDayKind.ToNull();
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 50).Value = _info.Title.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@SolarLunar", SqlDbType.Char, 1).Value = _info.SolarLunar.ToNull();
                    cmd.Parameters.Add("@HoliDay", SqlDbType.NVarChar, 10).Value = _info.HoliDay.ToNull();
                    cmd.Parameters.Add("@IsHoliday", SqlDbType.Char, 1).Value = _info.IsHoliday.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 20).Value = _info.WriteUserName.ToNull();                    

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

        #region MOD()
        /// <summary>
        /// Adds the list.
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Mod(BE.HolidayDInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_HolidayD_Upd", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@HoliDayID", SqlDbType.Int).Value = _info.HoliDayID;
                    cmd.Parameters.Add("@HoliDayKind", SqlDbType.Char, 1).Value = _info.HoliDayKind.ToNull();
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 50).Value = _info.Title.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@SolarLunar", SqlDbType.Char, 1).Value = _info.SolarLunar.ToNull();
                    cmd.Parameters.Add("@HoliDay", SqlDbType.NVarChar, 10).Value = _info.HoliDay.ToNull();
                    cmd.Parameters.Add("@IsHoliday", SqlDbType.Char, 1).Value = _info.IsHoliday.ToNull();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 20).Value = _info.ModifyUserName.ToNull();

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
        public bool Del(int _companyID, int _holiDayID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_HolidayD_DEL", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@HoliDayID", SqlDbType.Int).Value = _holiDayID;

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
