using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Visit.BE;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Visit.DAL
{
    public class Repeat : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 반복 일정 정보가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_Repeat_Sel");
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

        #endregion

        #region ADD()

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-13
        /// 작성목적 : 방문 예약 반복 설정 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(RepeatInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Repeat_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _info.ReserveGroupID;
                    cmd.Parameters.Add("@RepeatType", SqlDbType.Char, 1).Value = _info.RepeatType.ToNull().ToXSS();
                    cmd.Parameters.Add("@SelectType", SqlDbType.Char, 1).Value = _info.SelectType.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@IntervalCnt", SqlDbType.Int).Value = _info.IntervalCnt;
                    cmd.Parameters.Add("@Option1", SqlDbType.VarChar, 2).Value = _info.Option1.ToNull().ToXSS();
                    cmd.Parameters.Add("@Option2", SqlDbType.VarChar, 2).Value = _info.Option2.ToNull().ToXSS();
                    cmd.Parameters.Add("@Option3", SqlDbType.VarChar, 2).Value = _info.Option3.ToNull().ToXSS();
                    cmd.Parameters.Add("@Option4", SqlDbType.VarChar, 2).Value = _info.Option4.ToNull().ToXSS();
                    cmd.Parameters.Add("@Option5", SqlDbType.VarChar, 2).Value = _info.Option5.ToNull().ToXSS();
                    cmd.Parameters.Add("@Option6", SqlDbType.VarChar, 2).Value = _info.Option6.ToNull().ToXSS();
                    cmd.Parameters.Add("@Option7", SqlDbType.VarChar, 2).Value = _info.Option7.ToNull().ToXSS();
                    cmd.Parameters.Add("@MaxReptCnt", SqlDbType.Int).Value = _info.MaxReptCnt;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;

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

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-14
        /// 작성목적 : 방문 예약 반복 일정 설정 수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_beforereserveGroupID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public bool Upd(int _companyID, int _beforereserveGroupID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Repeat_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@BeforeReserveGroupID", SqlDbType.Int).Value = _beforereserveGroupID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;

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

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-14
        /// 작성목적 : 방문 예약 반복 일정 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Repeat_Del", cn);
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
