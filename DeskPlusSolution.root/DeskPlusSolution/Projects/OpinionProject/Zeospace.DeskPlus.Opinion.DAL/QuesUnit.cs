using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Opinion.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Opinion.DAL
{
    public class QuesUnit : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 설문질문추가
        /// </summary>
        /// <param name="_unitItemInfo"></param>
        /// <returns></returns>
        public int Add(QuesUnitInfo _quesUnitInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_quesUnitInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_QuesUnit_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _quesUnitInfo.CompanyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesUnitInfo.QuesID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _quesUnitInfo.UnitID;

                    cn.Open();
                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 설문-질문삭제(건당)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesUnitID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _quesUnitID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_QuesUnit_DelByQuesUnitID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@QuesUnitID", SqlDbType.Int).Value = _quesUnitID;

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
        /// 항목순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitItemID"></param>
        /// <param name="_seq"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, int _quesUnitID, int _seq)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_QuesUnit_UpdByQuesUnitID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@QuesUnitID", SqlDbType.Int).Value = _quesUnitID;
                    cmd.Parameters.Add("@Seq", SqlDbType.Int).Value = _seq;

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

        #region 가져오기

        /// <summary>
        /// 특정설문 질문 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public DataTableReader GetQuesUnitList(int _companyID, int _quesID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_QuesUnit_SelByQuesID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }
        /// <summary>
        /// 특정설문에 대한 질문+질문항목내용 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public DataTableReader GetQuesUnitEtc(int _companyID, int _quesID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_QuesUnitEtc_SelByQuesID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 특정설문의 특정질문에 대한 설문현황조회
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public DataTableReader GetQUResultList(int _companyID, int _quesID, int _unitID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_QuesUnitEtc_SelByQuesIDUnitID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }
        #endregion
    }
}
