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
    public class List : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListInfo GetList(IDataReader _reader)
        {
            return new ListInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ProposalNo = (_reader["ProposalNo"] == DBNull.Value) ? -1 : _reader["ProposalNo"].ToString().ToNumber<int>(),
                ReceiptNo = (_reader["ReceiptNo"] == DBNull.Value) ? -1 : _reader["ReceiptNo"].ToString().ToNumber<int>(),
                Title = _reader["Title"].ToString().ToNull(),
                Gubun = _reader["Gubun"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                Status = _reader["Status"].ToString().ToNull(),
                BeforeCont = _reader["BeforeCont"].ToString().ToNull(),
                AfterCont = _reader["AfterCont"].ToString().ToNull(),
                BeforeEffect = _reader["BeforeEffect"].ToString().ToNull(),
                AfterEffect = _reader["AfterEffect"].ToString().ToNull(),
                Grade1 = _reader["Grade1"].ToString().ToNull(),
                Score1 = _reader["Score1"].ToString().ToNull(),
                Grade2 = _reader["Grade2"].ToString().ToNull(),
                score2 = _reader["score2"].ToString().ToNull(),
                LastGrade = _reader["LastGrade"].ToString().ToNull(),
                ChargerView = _reader["ChargerView"].ToString().ToNull(),
                Etc1 = _reader["Etc1"].ToString().ToNull(),
                Etc2 = _reader["Etc2"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                Bul_Select = (_reader["Bul_Select"] == DBNull.Value) ? -1 : _reader["Bul_Select"].ToString().ToNumber<int>(),
                YYYYMM = _reader["YYYYMM"].ToString().ToNull(),

                #region 작성자 정보
                GubunCode = _reader["Gubun"].ToString().ToNull(),
                Name = _reader["Name"].ToString().ToNull(),
                ParentDeptName = _reader["ParentDeptName"].ToString().ToNull(),
                DeptName = _reader["DeptName"].ToString().ToNull(),
                TitleName = _reader["Title"].ToString().ToNull(),
                #endregion
            };
        }

        protected List<ListInfo> GetListList(IDataReader reader)
        {
            List<ListInfo> list = new List<ListInfo>();
            while (reader.Read())
                list.Add(GetList(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 제안 보기 검색 페이징 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_judgeGubun">The _judge gubun.</param>
        /// <param name="_startYearMonth">The _start year month.</param>
        /// <param name="_endYearMonth">The _end year month.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _judgeGubun, string _startYearMonth, string _endYearMonth, 
            string _searchField, string _searchValue, int _userID, string _gubun)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PROP_List_SelPageCnt", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@JudgeGubun", SqlDbType.Char, 1).Value = _judgeGubun;
                cmd.Parameters.Add("@StartYearMonth", SqlDbType.Char, 6).Value = _startYearMonth;
                cmd.Parameters.Add("@EndYearMonth", SqlDbType.Char, 6).Value = _endYearMonth;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Gubun", SqlDbType.VarChar, 10).Value = _gubun;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;

        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 제안 보기 검색 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_judgeGubun">The _judge gubun.</param>
        /// <param name="_startYearMonth">The _start year month.</param>
        /// <param name="_endYearMonth">The _end year month.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo, string _judgeGubun,
            string _startYearMonth, string _endYearMonth, string _searchField, string _searchValue, int _userID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_List_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@JudgeGubun", SqlDbType.Char, 1).Value = _judgeGubun;
                    cmd.Parameters.Add("@StartYearMonth", SqlDbType.Char, 6).Value = _startYearMonth;
                    cmd.Parameters.Add("@EndYearMonth", SqlDbType.Char, 6).Value = _endYearMonth;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.VarChar, 10).Value = _gubun;

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

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 통계보기 카운트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_judgeGubun">The _judge gubun.</param>
        /// <param name="_startYearMonth">The _start year month.</param>
        /// <param name="_endYearMonth">The _end year month.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _judgeGubun, string _startYearMonth, string _endYearMonth,
            string _searchField, string _searchValue)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PROP_List_SelSrhPageCnt", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@JudgeGubun", SqlDbType.Char, 1).Value = _judgeGubun;
                cmd.Parameters.Add("@StartYearMonth", SqlDbType.Char, 6).Value = _startYearMonth;
                cmd.Parameters.Add("@EndYearMonth", SqlDbType.Char, 6).Value = _endYearMonth;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;

        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 전체 제안 통계 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_judgeGubun">The _judge gubun.</param>
        /// <param name="_startYearMonth">The _start year month.</param>
        /// <param name="_endYearMonth">The _end year month.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo, 
            string _judgeGubun, string _startYearMonth, string _endYearMonth, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_List_SelSrhPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@JudgeGubun", SqlDbType.Char, 1).Value = _judgeGubun;
                    cmd.Parameters.Add("@StartYearMonth", SqlDbType.Char, 6).Value = _startYearMonth;
                    cmd.Parameters.Add("@EndYearMonth", SqlDbType.Char, 6).Value = _endYearMonth;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

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

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 심사 중, 완료 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetStatusCount(int _companyID)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_List_SelByStatus");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                return ExecuteDataSet(cmd, "GetStatusCount");
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 심사 중, 완료 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _status)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PROP_List_SelStatusPageCnt", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = _status;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 심사 중, 완료 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_judgeGubun">The _judge gubun.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo, string _status)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_List_SelStatusPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = _status;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

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

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 제안 상세 내역 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _proposalNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_List_Sel");

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
        /// 작성목적 : 선택한 년,월의 최대 제안자 이름 건수 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public DataTableReader GetMaxProposerInfo(int _companyID, string _yearMonth)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_List_SelByYYYYMM");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YearMonth", SqlDbType.Char, 6).Value = _yearMonth;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 선택한 년,월의 우수제안자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public DataTableReader GetMaxScoreInfo(int _companyID, string _yearMonth)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_List_SelByScore");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YearMonth", SqlDbType.Char, 6).Value = _yearMonth;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 선택한 년,월의 사업부별 제안현황 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public DataTableReader GetDeptInfo(int _companyID, string _yearMonth)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_List_SelByBul", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@YearMonth", SqlDbType.Char, 6).Value = _yearMonth;

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

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 선택한 년,월의 사업부별 제안현황 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public DataTableReader GetYearInfo(int _companyID, string _year)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_List_SelByYYYYMM", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Year", SqlDbType.Char, 4).Value = _year;

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
        /// 작성목적 : 제안 추가
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(ListInfo _info, string _yearMonth)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_List_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@Title", SqlDbType.VarChar, 100).Value = _info.Title;
                    cmd.Parameters.Add("@Gubun", SqlDbType.VarChar, 10).Value = _info.Gubun;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 1).Value = _info.Status;
                    cmd.Parameters.Add("@BeforeCont", SqlDbType.NText).Value = _info.BeforeCont.ToXSS();
                    cmd.Parameters.Add("@AfterCont", SqlDbType.NText).Value = _info.AfterCont.ToXSS();
                    cmd.Parameters.Add("@BeforeEffect", SqlDbType.NText).Value = _info.BeforeEffect.ToXSS();
                    cmd.Parameters.Add("@AfterEffect", SqlDbType.NText).Value = _info.AfterEffect.ToXSS();
                    cmd.Parameters.Add("@Etc1", SqlDbType.VarChar, 500).Value = _info.Etc1.ToXSS();
                    cmd.Parameters.Add("@Bul", SqlDbType.Int).Value = _info.Bul_Select;
                    cmd.Parameters.Add("@YearMonth", SqlDbType.Char, 6).Value = _yearMonth;

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

        #region MOD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 심사 등급 수정
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <param name="_grade">The _grade.</param>
        /// <returns></returns>
        public bool ModGade(int _companyID, int _proposalNo, string _grade)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_PROP_List_UpdByLastGrade", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;
                    cmd.Parameters.Add("@Grade", SqlDbType.VarChar, 50).Value = _grade;

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
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 제안수정
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_PROP_List_UpdByProposalNo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _info.ProposalNo;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = _info.Title;
                    cmd.Parameters.Add("@Gubun", SqlDbType.NVarChar, 10).Value = _info.Gubun;
                    cmd.Parameters.Add("@BeforeCont", SqlDbType.NText).Value = _info.BeforeCont;
                    cmd.Parameters.Add("@AfterCont", SqlDbType.NText).Value = _info.AfterCont;
                    cmd.Parameters.Add("@BeforeEffect", SqlDbType.NText).Value = _info.BeforeEffect;
                    cmd.Parameters.Add("@Etc1", SqlDbType.VarChar, 500).Value = _info.Etc1;
                    cmd.Parameters.Add("@Bul", SqlDbType.Int).Value = _info.Bul_Select;

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
        /// 작성목적 : 제안 취소(삭제)
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

                    var cmd = new SqlCommand("DP_PROP_List_Del", cn);
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
