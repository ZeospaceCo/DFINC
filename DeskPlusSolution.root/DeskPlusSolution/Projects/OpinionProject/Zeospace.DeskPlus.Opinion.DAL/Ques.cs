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
    public class Ques : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.QuesInfo GetQues(IDataReader _idr)
        {
            return new QuesInfo
            {
                CompanyID = (_idr["CompanyID"] == DBNull.Value) ? -1 : _idr["CompanyID"].ToString().ToNumber<int>(),
                QuesID = (_idr["QuesID"] == DBNull.Value) ? -1 : _idr["QuesID"].ToString().ToNumber<int>(),
                Title = _idr["Title"].ToString(),
                Cont = _idr["Cont"].ToString().ToEditorXSS(),
                StartDate = (_idr["StartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_idr["StartDate"],
                EndDate = (_idr["EndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_idr["EndDate"],
                GroupName = _idr["GroupName"].ToString(),
                IsMain = _idr["IsMain"].ToString(),
                IsReply = _idr["IsReply"].ToString(),
                IsOpen = _idr["IsOpen"].ToString(),
                IsVoteUser = _idr["IsVoteUser"].ToString(),
                IsSecret = _idr["IsSecret"].ToString(),
                IsActive = _idr["IsActive"].ToString(),
                SuperDeptID = (_idr["SuperDeptID"] == DBNull.Value) ? -1 : (int)_idr["SuperDeptID"],
                WriteDate = (_idr["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_idr["WriteDate"],
                WriteUserID = (_idr["WriteUserID"] == DBNull.Value) ? -1 : (int)_idr["WriteUserID"],
                DeptCodeID = (_idr["DeptCodeID"] == DBNull.Value) ? -1 : (int)_idr["DeptCodeID"],
                SuperDeptNmBase = _idr["SuperDeptNmBase"].ToString(),
                SuperDeptNmKo = _idr["SuperDeptNmKo"].ToString(),
                SuperDeptNmEn = _idr["SuperDeptNmEn"].ToString(),
                SuperDeptNmJa = _idr["SuperDeptNmJa"].ToString(),
                SuperDeptNmFr = _idr["SuperDeptNmFr"].ToString(),
                SuperDeptNmZhcn = _idr["SuperDeptNmZhcn"].ToString(),
                SuperDeptNmZhtw = _idr["SuperDeptNmZhtw"].ToString(),
            };
        }

        protected List<BE.QuesInfo> GetQuessList(IDataReader reader)
        {
            List<BE.QuesInfo> list = new List<QuesInfo>();
            while (reader.Read())
            {
                list.Add(GetQues(reader));
            }
            return list;
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 설문추가
        /// </summary>
        /// <param name="_attachInfo"></param>
        /// <returns></returns>
        public int Add(QuesInfo _quesInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_quesInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Ques_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _quesInfo.CompanyID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 500).Value = _quesInfo.Title.ToXSS();
                    cmd.Parameters.Add("@Cont", SqlDbType.NText).Value = _quesInfo.Cont.ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _quesInfo.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _quesInfo.EndDate;
                    cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar, 100).Value = _quesInfo.GroupName;
                    cmd.Parameters.Add("@IsMain", SqlDbType.Char, 1).Value = _quesInfo.IsMain;
                    cmd.Parameters.Add("@IsReply", SqlDbType.Char, 1).Value = _quesInfo.IsReply;
                    cmd.Parameters.Add("@IsOpen", SqlDbType.Char, 1).Value = _quesInfo.IsOpen;
                    cmd.Parameters.Add("@IsVoteUser", SqlDbType.Char, 1).Value = _quesInfo.IsVoteUser;
                    cmd.Parameters.Add("@IsSecret", SqlDbType.Char, 1).Value = _quesInfo.IsSecret;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _quesInfo.IsActive;
                    cmd.Parameters.Add("@SuperDeptID", SqlDbType.Int).Value = _quesInfo.SuperDeptID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _quesInfo.WriteUserID;

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
        /// 설문수정
        /// </summary>
        /// <param name="_quesInfo"></param>
        /// <returns></returns>
        public bool Mod(QuesInfo _quesInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_quesInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Ques_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _quesInfo.CompanyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesInfo.QuesID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 500).Value = _quesInfo.Title.ToXSS();
                    cmd.Parameters.Add("@Cont", SqlDbType.NText).Value = _quesInfo.Cont.ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _quesInfo.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _quesInfo.EndDate;
                    cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar, 100).Value = _quesInfo.GroupName;
                    cmd.Parameters.Add("@IsMain", SqlDbType.Char, 1).Value = _quesInfo.IsMain;
                    cmd.Parameters.Add("@IsReply", SqlDbType.Char, 1).Value = _quesInfo.IsReply;
                    cmd.Parameters.Add("@IsOpen", SqlDbType.Char, 1).Value = _quesInfo.IsOpen;
                    cmd.Parameters.Add("@IsVoteUser", SqlDbType.Char, 1).Value = _quesInfo.IsVoteUser;
                    cmd.Parameters.Add("@IsSecret", SqlDbType.Char, 1).Value = _quesInfo.IsSecret;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _quesInfo.IsActive;
                    cmd.Parameters.Add("@SuperDeptID", SqlDbType.Int).Value = _quesInfo.SuperDeptID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _quesInfo.WriteUserID;

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
        /// <summary>
        /// 활성화/비활성화
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public bool UpdActive(int _companyID, int _quesID, string _isActive)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Ques_UpdIsActive", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _isActive;

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
        /// 설문삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _quesID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Ques_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

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
        /// 특정설문정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public List<BE.QuesInfo> GetQues(int _companyID, int _quesID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Ques_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetQuessList(reader);
                }
            }
        }

        /// <summary>
        /// 설문리스트 갯수 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <returns></returns>
        public int GetQuesList(int _companyID, string _section, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Ques_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _section;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                cmd.Parameters.Add("@Period", SqlDbType.NVarChar, 50).Value = _sPeriod;
                cmd.Parameters.Add("@PeriodValue", SqlDbType.NVarChar, 50).Value = _sPeriodVal;

                cn.Open();

                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }

        /// <summary>
        /// 설문리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetQuesList(int _companyID, string _section, int _pageSize, int _pageNo, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal, string _order)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Ques_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _section;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                    cmd.Parameters.Add("@Period", SqlDbType.NVarChar, 50).Value = _sPeriod;
                    cmd.Parameters.Add("@PeriodValue", SqlDbType.NVarChar, 50).Value = _sPeriodVal;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 50).Value = _order;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.DAL.Ques[GetQuesList]", ex);
                return null;
            }
        }

        /// <summary>
        /// 사용자 설문리스트 가져오기(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_section"></param>
        /// <param name="_isVote"></param>
        /// <param name="_userID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_sPeriod"></param>
        /// <param name="_sPeriodVal"></param>
        /// <returns></returns>
        public int GetQuesUserList(int _companyID, int _deptID, int _userID, string _section, string _isVote, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Ques_SelUserPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _section;
                cmd.Parameters.Add("@IsVote", SqlDbType.Char, 1).Value = _isVote;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                cmd.Parameters.Add("@Period", SqlDbType.NVarChar, 50).Value = _sPeriod;
                cmd.Parameters.Add("@PeriodValue", SqlDbType.NVarChar, 50).Value = _sPeriodVal;

                cn.Open();

                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }
        /// <summary>
        /// 사용자 설문리스트 가져오기(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_section"></param>
        /// <param name="_isVote"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_sPeriod"></param>
        /// <param name="_sPeriodVal"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetQuesUserList(int _companyID, int _deptID, int _userID, string _section, string _isVote, int _pageSize, int _pageNo, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal, string _order)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Ques_SelUserPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _section;
                    cmd.Parameters.Add("@IsVote", SqlDbType.Char, 1).Value = _isVote;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                    cmd.Parameters.Add("@Period", SqlDbType.NVarChar, 50).Value = _sPeriod;
                    cmd.Parameters.Add("@PeriodValue", SqlDbType.NVarChar, 50).Value = _sPeriodVal;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 50).Value = _order;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.DAL.Ques[GetQuesUserList]", ex);
                return null;
            }
        }
        #endregion
    }
}
