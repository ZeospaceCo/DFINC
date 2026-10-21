using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Report.BE;

namespace Zeospace.DeskPlus.Report.DAL
{
    public class List : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListInfo GetList(IDataReader _reader)
        {
            return new ListInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                YYYYMM = _reader["YYYYMM"].ToString().ToNull(),
                StartDate = _reader["StartDate"].ToString().ToNull(),
                EndDate = _reader["EndDate"].ToString().ToNull(),
                NowJob = _reader["NowJob"].ToString().ToNull(),
                NextJob = _reader["NextJob"].ToString().ToNull(),
                Issue = _reader["Issue"].ToString().ToNull(),
                Flag = _reader["Flag"].ToString().ToNull(),
                IsClose = _reader["IsClose"].ToString().ToNull(),
                IsLinked = _reader["IsLinked"].ToString().ToNull(),
                FolderID = _reader["FolderID"].ToString().ToNull().ToNumber<int>(),

                CompUserID = (_reader["CompUserID"] == DBNull.Value) ? -1 : _reader["CompUserID"].ToString().ToNumber<int>(),
                CompletDate = (_reader["CompletDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["CompletDate"],

                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                WriteDateYMD = _reader["WriteDateYMD"].ToString().ToNull(),
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],

                #region ####  부서,직급,직책 다국어 시작 #######

                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),

                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                DutyBase = _reader["DutyBase"].ToString().ToNull(),
                DutyKo = _reader["DutyKo"].ToString().ToNull(),
                DutyEn = _reader["DutyEn"].ToString().ToNull(),
                DutyJa = _reader["DutyJa"].ToString().ToNull(),
                DutyFr = _reader["DutyFr"].ToString().ToNull(),
                DutyZhcn = _reader["DutyZhcn"].ToString().ToNull(),
                DutyZhtw = _reader["DutyZhtw"].ToString().ToNull(),

                #endregion ####  부서,직급,직책 다국어 시작 #######

                #region ####  승인자 부서,직급,직책 다국어 시작 #######

                CompNameBase = _reader["CompNameBase"].ToString().ToNull(),
                CompNameKo = _reader["CompNameKo"].ToString().ToNull(),
                CompNameEn = _reader["CompNameEn"].ToString().ToNull(),
                CompNameJa = _reader["CompNameJa"].ToString().ToNull(),
                CompNameFr = _reader["CompNameFr"].ToString().ToNull(),
                CompNameZhcn = _reader["CompNameZhcn"].ToString().ToNull(),
                CompNameZhtw = _reader["CompNameZhtw"].ToString().ToNull(),

                CompDeptBase = _reader["CompDeptBase"].ToString().ToNull(),
                CompDeptKo = _reader["CompDeptKo"].ToString().ToNull(),
                CompDeptEn = _reader["CompDeptEn"].ToString().ToNull(),
                CompDeptJa = _reader["CompDeptJa"].ToString().ToNull(),
                CompDeptFr = _reader["CompDeptFr"].ToString().ToNull(),
                CompDeptZhcn = _reader["CompDeptZhcn"].ToString().ToNull(),
                CompDeptZhtw = _reader["CompDeptZhtw"].ToString().ToNull(),
                CompTitleBase = _reader["CompTitleBase"].ToString().ToNull(),
                CompTitleKo = _reader["CompTitleKo"].ToString().ToNull(),
                CompTitleEn = _reader["CompTitleEn"].ToString().ToNull(),
                CompTitleJa = _reader["CompTitleJa"].ToString().ToNull(),
                CompTitleFr = _reader["CompTitleFr"].ToString().ToNull(),
                CompTitleZhcn = _reader["CompTitleZhcn"].ToString().ToNull(),
                CompTitleZhtw = _reader["CompTitleZhtw"].ToString().ToNull(),
                CompDutyBase = _reader["CompDutyBase"].ToString().ToNull(),
                CompDutyKo = _reader["CompDutyKo"].ToString().ToNull(),
                CompDutyEn = _reader["CompDutyEn"].ToString().ToNull(),
                CompDutyJa = _reader["CompDutyJa"].ToString().ToNull(),
                CompDutyFr = _reader["CompDutyFr"].ToString().ToNull(),
                CompDutyZhcn = _reader["CompDutyZhcn"].ToString().ToNull(),
                CompDutyZhtw = _reader["CompDutyZhtw"].ToString().ToNull()

                #endregion ####  승인자 부서,직급,직책 다국어 시작 #######
            };
        }

        protected List<ListInfo> GetListList(IDataReader reader)
        {
            List<ListInfo> list = new List<ListInfo>();
            while (reader.Read())
                list.Add(GetList(reader));
            return list;
        }

        #endregion DataReader

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_REPORT_List_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@YYYYMM", SqlDbType.Char, 6).Value = _info.YYYYMM;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@NowJob", SqlDbType.NVarChar, 4000).Value = _info.NowJob.ToNull().ToXSS();
                    cmd.Parameters.Add("@NextJob", SqlDbType.NVarChar, 4000).Value = _info.NextJob.ToNull().ToXSS();
                    cmd.Parameters.Add("@Issue", SqlDbType.NVarChar, 4000).Value = _info.Issue.ToNull().ToXSS();
                    cmd.Parameters.Add("@Flag", SqlDbType.Char, 1).Value = _info.Flag.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose;
                    cmd.Parameters.Add("@IsLinked", SqlDbType.Char, 1).Value = _info.IsLinked;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;

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

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_REPORT_List_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@NowJob", SqlDbType.NVarChar, 4000).Value = _info.NowJob.ToNull().ToXSS();
                    cmd.Parameters.Add("@NextJob", SqlDbType.NVarChar, 4000).Value = _info.NextJob.ToNull().ToXSS();
                    cmd.Parameters.Add("@Issue", SqlDbType.NVarChar, 4000).Value = _info.Issue.ToNull().ToXSS();
                    cmd.Parameters.Add("@Flag", SqlDbType.Char, 1).Value = _info.Flag.ToNull();
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose;
                    cmd.Parameters.Add("@IsLinked", SqlDbType.Char, 1).Value = _info.IsLinked;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;

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
        /// 작성자   : shine
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 확인여부 수정(요청->;확인)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_compUserID">The _comp user ID.</param>
        /// <param name="_flag">The _flag.</param>
        /// <returns></returns>
        public bool ConfirmMod(int _companyID, int _listID, int _compUserID, string _flag)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_REPORT_List_UpdListID", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@CompUserID", SqlDbType.Int).Value = _compUserID;
                    cmd.Parameters.Add("@Flag", SqlDbType.Char, 1).Value = _flag;

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
        /// 본문 내용만 수정하기
        /// 심재운
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ModContent(BE.ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_REPORT_List_UpdContent", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@NowJob", SqlDbType.NVarChar, 4000).Value = _info.NowJob.ToNull().ToXSS();

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

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_REPORT_List_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

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

        #endregion DEL()

        #region GET()

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 검색 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_targetUserID">The _target user ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _yearMonth, string _startDate, int _deptID,
            string _pageSize, string _pageSizePageNo, string _searchField, string _searchValue, int _targetUserID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_List_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YYYYMM", SqlDbType.Char, 6).Value = _yearMonth;
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 100).Value = _startDate;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@TargetUserID", SqlDbType.Int).Value = _targetUserID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 검색 페이징 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _yearMonth, string _startDate, int _deptID, string _searchField, string _searchValue, int _userID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_List_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YYYYMM", SqlDbType.Char, 6).Value = _yearMonth;
                cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _startDate;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 개인함 일지 현황 리스트 가져오기(월단위)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearmonth">The _yearmonth.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetPrivateList(int _companyID, string _yearmonth, string _order, string _searchField, string _searchValue, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_List_SelPrivateList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YYYYMM", SqlDbType.Char, 6).Value = _yearmonth;
                cmd.Parameters.Add("@Order", SqlDbType.Char, 8).Value = _order;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetReportList");
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 보고함 일지 현황 리스트 가져오기(월단위)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearmonth">The _yearmonth.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetInformList(int _companyID, string _yearmonth, string _order, string _searchField, string _searchValue, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_List_SelInformList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YYYYMM", SqlDbType.Char, 6).Value = _yearmonth;
                cmd.Parameters.Add("@Order", SqlDbType.Char, 8).Value = _order;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetInformList");
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 부서함 일지 현황 리스트 가져오기(월, 주단위)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearmonth">The _yearmonth.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_targetDeptID">The _target dept ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetDeptList(int _companyID, string _yearmonth,
            string _startDate, string _order, string _searchField, string _searchValue, int _targetDeptID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_List_SelDeptList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YYYYMM", SqlDbType.Char, 6).Value = _yearmonth;
                cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _startDate;
                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _targetDeptID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetReportDeptList");
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 확인요청함 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearmonth">The _yearmonth.</param>
        /// <param name="_shareUserList">The _share user list.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_flag">The _flag.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetConfirmList(int _companyID, string _yearmonth, string _shareUserList,
            string _startDate, string _order, string _searchField, string _searchValue, string _flag, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_List_SelConfirmList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YYYYMM", SqlDbType.Char, 6).Value = _yearmonth;
                cmd.Parameters.Add("@ShareUserList", SqlDbType.VarChar, 500).Value = _shareUserList;
                cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _startDate;
                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@Flag", SqlDbType.Char, 1).Value = _flag;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 공유함 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_yearmonth">The _yearmonth.</param>
        /// <param name="_userIDList">The _user ID list.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetShareList(int _companyID, string _yearmonth, string _userIDList,
            string _startDate, string _order, string _searchField, string _searchValue, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_List_SelShareList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@YYYYMM", SqlDbType.Char, 6).Value = _yearmonth;
                cmd.Parameters.Add("@UserIDList", SqlDbType.VarChar, 500).Value = _userIDList;
                cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _startDate;
                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetReportShareList");
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">게시물아이디</param>
        /// <returns></returns>
        public List<ListInfo> GetList(int _companyID, string _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_List_SelByID");
                query = string.Format(query, _listID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 부서별 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <returns></returns>
        public List<ListInfo> GetListByDeptID(int _companyID, int _deptID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_List_SelByDeptID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 사용자별 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<ListInfo> GetListByUserID(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_List_SelByUserID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 시작일자별 데이터 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userList">The _user list.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_flag">The _flag.</param>
        /// <returns></returns>
        public DataTableReader GetListByStartDate(int _companyID, string _userList, string _startDate, string _flag)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_List_SelByStartDate");
                query = string.Format(query, _flag, _userList);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _startDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 주간별 보고 내용건 가져오기
        /// 심재운
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_folderID">The _folder identifier.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <returns></returns>
        public List<ListInfo> GetListByFolder(int _companyID, int _folderID, string _startDate)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_List_SelByFolderID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;
                cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _startDate;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListList(reader);
                }
            }
        }

        /// <summary>
        /// 보고용 문건 총 카운트
        /// 심재운
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_folderID">The _folder identifier.</param>
        /// <returns></returns>
        public int GetListByFolder(int _companyID, string _folderID)
        {
            if (_folderID.Length == 0) _folderID = "0";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_List_SelCntByFolderID");
                query = String.Format(query, _folderID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                object obj = ExecuteScalar(cmd);
                return obj.ToString().ToNumber<int>();
            }
        }



        #endregion GET()
    }
}