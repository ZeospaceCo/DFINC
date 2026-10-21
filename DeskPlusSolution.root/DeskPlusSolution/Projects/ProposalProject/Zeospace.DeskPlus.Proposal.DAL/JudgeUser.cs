using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Proposal.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Proposal.DAL
{
    public class JudgeUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 심사 중, 완료 승인자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public DataSet GetJudgeUserInfo(int _companyID)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_JudgeUser_SelByUserID");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetJudgeUserInfo");
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 선택한 제안의 심사자 정보 가져오기
        /// </summary>
        /// <param name="_compnayID">The _compnay ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        public DataTableReader GetJudgeUserInfo(int _companyID, int _proposalNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_JudgeUser_SelByPNo");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 심사자가 심사할 리스트 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID, string _status)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PROP_JudgeUser_SelPageCnt", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 1).Value = _status;
                
                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 심사자가 심사할 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo, int _userID, string _status)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_JudgeUser_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 1).Value = _status;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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

        #region ADD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 심사자 추가
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _proposalNo, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_JudgeUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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

        #region DEL

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 제안 취소(심사자 삭제)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _proposalNo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_PROP_JudgeUser_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;

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
