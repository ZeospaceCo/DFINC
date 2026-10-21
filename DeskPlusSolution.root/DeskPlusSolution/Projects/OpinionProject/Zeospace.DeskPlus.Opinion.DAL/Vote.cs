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
    public class Vote : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 투표추가
        /// </summary>
        /// <param name="_attachInfo"></param>
        /// <returns></returns>
        public int Add(VoteInfo _voteInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_voteInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Vote_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _voteInfo.CompanyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _voteInfo.QuesID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _voteInfo.UserID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _voteInfo.UnitID;
                    cmd.Parameters.Add("@UnitItemID", SqlDbType.Int).Value = _voteInfo.UnitItemID;
                    cmd.Parameters.Add("@UnitItemCont", SqlDbType.NVarChar, 500).Value = _voteInfo.UnitItemCont;
                    cmd.Parameters.Add("@Point", SqlDbType.Int).Value = _voteInfo.Point;

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

        #endregion

        #region 가져오기

        /// <summary>
        /// 가점형 설문결과가져오기(단일가점형)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public DataSet GetResultVoteList(int _companyID, int _quesID, int _unitID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelByQuesIDUnitID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

                cn.Open();
                return ExecuteDataSet(cmd, "dsList");
            }
        }
        /// <summary>
        /// 가점형 설문결과가져오기(다중가점형)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public DataSet GetResultVoteList(int _companyID, int _quesID, int _unitID, int _unitItemID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelByQuesIDUnitIDItemID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                cmd.Parameters.Add("@UnitItemID", SqlDbType.Int).Value = _unitItemID;

                cn.Open();
                return ExecuteDataSet(cmd, "dsList");
            }
        }
        /// <summary>
        /// 특정설문의 특정 항목에 대한 투표리스트 정보
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public DataTableReader GetVoteList(int _companyID, int _quesID, int _unitID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelEtcByQuesIDUnitID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }
        /// <summary>
        /// 투표현황리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public DataTableReader GetVotePageList(int _companyID, int _quesID, int _pageSize, int _pageNo)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelVoteList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }
        /// <summary>
        /// 투표현황리스트(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <returns></returns>
        public int GetVotePageList(int _companyID, int _quesID)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelVoteCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cn.Open();
                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }

        /// <summary>
        /// 항목별 투표현황리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_unitItemID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <returns></returns>
        public DataTableReader GetVotePageListAdmin(int _companyID, int _quesID, int _unitID, int _unitItemID, int _pageSize, int _pageNo)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelVoteListAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                cmd.Parameters.Add("@UnitItemID", SqlDbType.Int).Value = _unitItemID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 항목별 투표현황리스트(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_unitItemID"></param>
        /// <returns></returns>
        public int GetVotePageListAdmin(int _companyID, int _quesID, int _unitID, int _unitItemID)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelVoteCntAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                cmd.Parameters.Add("@UnitItemID", SqlDbType.Int).Value = _unitItemID;
                cn.Open();
                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }

        /// <summary>
        /// 항목별 투표현황리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_unitItemID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <returns></returns>
        public DataTableReader GetVotePageListAdminC(int _companyID, int _quesID, int _unitID, string _unitItemCont, int _pageSize, int _pageNo)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelVoteListAdminC", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                cmd.Parameters.Add("@UnitItemCont", SqlDbType.NVarChar, 500).Value = _unitItemCont;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 항목별 투표현황리스트(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_unitItemID"></param>
        /// <returns></returns>
        public int GetVotePageListAdminC(int _companyID, int _quesID, int _unitID, string _unitItemCont)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelVoteCntAdminC", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                cmd.Parameters.Add("@UnitItemCont", SqlDbType.NVarChar, 500).Value = _unitItemCont;
                cn.Open();
                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }

        /// <summary>
        /// 항목별 투표현황리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_unitItemID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <returns></returns>
        public DataTableReader GetVotePageListAdminD(int _companyID, int _quesID, int _unitID, int _unitItemID, string _unitItemCont, int _pageSize, int _pageNo)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelVoteListAdminD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                cmd.Parameters.Add("@UnitItemID", SqlDbType.Int).Value = _unitItemID;
                cmd.Parameters.Add("@UnitItemCont", SqlDbType.NVarChar, 500).Value = _unitItemCont;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 항목별 투표현황리스트(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_unitItemID"></param>
        /// <returns></returns>
        public int GetVotePageListAdminD(int _companyID, int _quesID, int _unitID, int _unitItemID, string _unitItemCont)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Vote_SelVoteCntAdminD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                cmd.Parameters.Add("@UnitItemID", SqlDbType.Int).Value = _unitItemID;
                cmd.Parameters.Add("@UnitItemCont", SqlDbType.NVarChar, 500).Value = _unitItemCont;
                cn.Open();
                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }
        #endregion
    }
}
