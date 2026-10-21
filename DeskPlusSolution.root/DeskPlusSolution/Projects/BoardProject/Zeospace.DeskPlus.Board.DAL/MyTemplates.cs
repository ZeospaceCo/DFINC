using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class MyTemplates : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets my template.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private MyTemplateInfo GetMyTemplate(IDataReader _reader)
        {
            return new MyTemplateInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                MyTemplateID = (_reader["MyTemplateID"] == DBNull.Value) ? -1 : _reader["MyTemplateID"].ToString().ToNumber<int>(),
                Title = _reader["Title"].ToString().ToNull(),
                Content = _reader["Content"].ToString().ToNull().ToEditorXSS(),
                MobileContent = _reader["MobileContent"].ToString().ToNull(),
                OwnerDeptID = (_reader["OwnerDeptID"] == DBNull.Value) ? -1 : _reader["OwnerDeptID"].ToString().ToNumber<int>(),
                OptionModify = _reader["OptionModify"].ToString().ToNull(),
                OptionTop = _reader["OptionTop"].ToString().ToNull(),
                ReadCount = (_reader["ReadCount"] == DBNull.Value) ? -1 : _reader["ReadCount"].ToString().ToNumber<int>(),
                ScreenShot = _reader["ScreenShot"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteUserName = _reader["WriteUserName"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],

                #region ####  부서,직급,직책 다국어 시작 #######

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
                DutyZhtw = _reader["DutyZhtw"].ToString().ToNull()

                #endregion ####  부서,직급,직책 다국어 시작 #######
            };
        }

        protected List<MyTemplateInfo> GetMyTemplateList(IDataReader reader)
        {
            List<MyTemplateInfo> list = new List<MyTemplateInfo>();
            while (reader.Read())
                list.Add(GetMyTemplate(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 임시저장
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public int Add(BE.MyTemplateInfo _listinfo)
        {
            try
            {
                _listinfo.MobileContent = _listinfo.MobileContent.ToEditorXSS();
                _listinfo.MobileContent = Regex.Replace(_listinfo.MobileContent, "<img", "<ximg", RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplate_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _listinfo.CategoryID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = _listinfo.Title.ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _listinfo.Content.ToEditorXSS();
                    cmd.Parameters.Add("@MobileContent", SqlDbType.NText).Value = _listinfo.MobileContent;
                    cmd.Parameters.Add("@OwnerDeptID", SqlDbType.Char, 1).Value = _listinfo.OwnerDeptID;
                    cmd.Parameters.Add("@OptionModify", SqlDbType.Char, 1).Value = _listinfo.OptionModify;
                    cmd.Parameters.Add("@OptionTop", SqlDbType.Int).Value = _listinfo.OptionTop;
                    cmd.Parameters.Add("@ReadCount", SqlDbType.Int).Value = _listinfo.ReadCount;
                    cmd.Parameters.Add("@ScreenShot", SqlDbType.NVarChar, 500).Value = _listinfo.ScreenShot.ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _listinfo.WriteUserID;
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 20).Value = _listinfo.WriteUserName.ToXSS();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
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
        /// 작성자 : 심재운
        /// 수정
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_screenGubun">The _screen gubun.</param>
        /// <returns></returns>
        public bool Mod(BE.MyTemplateInfo _listinfo, string _screenGubun)
        {
            try
            {
                _listinfo.MobileContent = _listinfo.MobileContent.ToEditorXSS();
                _listinfo.MobileContent = Regex.Replace(_listinfo.MobileContent, "<img", "<ximg", RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplate_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _listinfo.CategoryID;
                    cmd.Parameters.Add("@MyTemplateID", SqlDbType.Int).Value = _listinfo.MyTemplateID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = _listinfo.Title.ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _listinfo.Content.ToEditorXSS();
                    cmd.Parameters.Add("@MobileContent", SqlDbType.NText).Value = _listinfo.MobileContent;
                    cmd.Parameters.Add("@OwnerDeptID", SqlDbType.Char, 1).Value = _listinfo.OwnerDeptID;
                    cmd.Parameters.Add("@OptionModify", SqlDbType.Char, 1).Value = _listinfo.OptionModify;
                    cmd.Parameters.Add("@OptionTop", SqlDbType.Int).Value = _listinfo.OptionTop;
                    cmd.Parameters.Add("@ScreenShot", SqlDbType.NVarChar, 500).Value = _listinfo.ScreenShot.ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _listinfo.WriteUserID;
                    cmd.Parameters.Add("@ScreenGubun", SqlDbType.Char, 1).Value = _screenGubun;

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

        /// <summary>
        /// 작성자 : 심재운
        /// ScreenShot 수정
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_myTemplateID">The _my template ID.</param>
        /// <param name="_screenShot">The _screen shot.</param>
        /// <returns></returns>
        public bool ModByScreenShot(int _companyID, int _categoryID, int _myTemplateID, string _screenShot)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplate_UpdScreenShot", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@MyTemplateID", SqlDbType.Int).Value = _myTemplateID;
                    cmd.Parameters.Add("@ScreenShot", SqlDbType.NVarChar, 500).Value = _screenShot.ToXSS();

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

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_myTemplateID">The _list ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _myTemplateID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplate_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@MyTemplateID", SqlDbType.Int).Value = _myTemplateID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 임시글을 게시판에 복사하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_myTemplateID">The _my template ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <param name="_screenShot">The _screen shot.</param>
        /// <returns></returns>
        public int Move(int _companyID, int _categoryID, int _myTemplateID, int _deptID, int _userID,
            string _writeUserName, string _screenShot)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplate_InsMove", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@MyTemplateID", SqlDbType.Int).Value = _myTemplateID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 20).Value = _writeUserName;
                    cmd.Parameters.Add("@ScreenShot", SqlDbType.NVarChar, 500).Value = _screenShot;

                    cn.Open();
                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 임시글 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_ListID">The _ list ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.MyTemplateInfo> GetMyTemplateLists(int _companyID, int _categoryID, int _ListID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_MyTemplate_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@MyTemplateID", SqlDbType.Int).Value = _ListID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetMyTemplateList(reader);
                    }
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
        /// 작성일자 : 2011-05-17
        /// 작성목적 : 임시저장 검색 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetMyTemplatePageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue, string _order, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplate_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 500).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;

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
        /// 작성일자 : 2011-05-17
        /// 작성목적 : 임시저장 테이블 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_searchField">The _search field.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public int GetMyTemplatePageCount(int _companyID, string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue, int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplate_SelPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 500).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = ExecuteScalar(cmd).ToString().ToNumber<int>();
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

        #endregion 가져오기
    }
}