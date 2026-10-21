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
    public class Target : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader
        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 대상자추가
        /// </summary>
        /// <param name="_unitItemInfo"></param>
        /// <returns></returns>
        public bool Add(TargetInfo _targetInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_targetInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Target_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _targetInfo.CompanyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _targetInfo.QuesID;
                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = _targetInfo.OwnerID;
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _targetInfo.Type;

                    cn.Open();
                    ExecuteScalar(cmd);
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

        /// <summary>
        /// 대상자삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_targetID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _targetID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Target_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@TargetID", SqlDbType.Int).Value = _targetID;

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
        /// 설문대상 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public DataTableReader GetTargetList(int _companyID, int _quesID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Target_SelByQuesID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion
    }
}
