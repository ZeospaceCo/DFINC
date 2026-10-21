using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class List
    {
        /// <summary>
        /// Gets the board list.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_isDeptBoard">The _is dept board.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <returns></returns>
        public DataSet GetBoardList(int _companyID, string _categoryID, string _pageSize, string _pageSizePageNo, string _isDeptBoard, string _deptID, int _userID, string _searchValue)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] 
            {											   
			   new SqlParameter("@CompanyID", SqlDbType.VarChar, 50),
			   new SqlParameter("@CategoryID", SqlDbType.VarChar, 5000),
			   new SqlParameter("@PageNo", SqlDbType.VarChar,10),
			   new SqlParameter("@PageSize", SqlDbType.VarChar, 10),
			   new SqlParameter("@IsDeptBoard", SqlDbType.Char, 1),
			   new SqlParameter("@DeptID", SqlDbType.VarChar, 50),
               new SqlParameter("@UserID", SqlDbType.VarChar, 50),
               new SqlParameter("@SearchValue", SqlDbType.NVarChar, 100),
		    };

                parms[0].Value = _companyID;
                parms[1].Value = _categoryID;
                parms[2].Value = _pageSize;
                parms[3].Value = _pageSizePageNo;
                parms[4].Value = _isDeptBoard;
                parms[5].Value = _deptID;
                parms[6].Value = _userID;
                parms[7].Value = _searchValue;

                return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_List_Sel", "GetBoaList", parms);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBoardList: " + errMsg);
                throw;
            }

        }

        /// <summary>
        /// Gets the board list.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public DataRow GetBoardList(int _companyID, int _categoryID, int _listID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@CategoryID", SqlDbType.Int),
											   new SqlParameter("@ListID", SqlDbType.Int)
										   };
            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _listID;

            try
            {
                DataSet ds = null;
                DataRow dr = null;
                using (ds = SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_List_SelByCIDLID", "GetBoardListByCategoryIDListID", parms))
                {

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dr = ds.Tables[0].Rows[0];
                    }
                }

                return dr;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBoardList1: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 이전, 이후 listid 값 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public string GetListIDBeforeNext(int _companyID, int _categoryID, int _listID, string _gubun, int _userID, int _deptID, string _searchValue)
        {
            try
            {
                Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category bll = new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category();
                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.CategoryInfo be = bll.GetCategoryOptionEtc(_companyID, _categoryID, _userID, _deptID);

                if (be.Auth == "")
                {
                    return "0";
                }

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List biz = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();

                string targetCategory = _categoryID.ToString();
                if (!be.OptionUseView.Equals("0"))
                {
                    string parentCategorys = bll.GetChildCategoryAuth(_companyID, _userID, _deptID, _categoryID, "0");

                    if (parentCategorys.Length > 0)
                    {
                        string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_QRY_SELECTCATEGORY_USEMOBILE");
                        sql = string.Format(sql, _companyID, parentCategorys);

                        DataSet dss = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsUSEMOBILE", null);

                        if (dss != null)
                        {
                            string tmpCat = "";
                            foreach (DataRow dr in dss.Tables[0].Rows)
                            {
                                tmpCat += dr["CategoryID"].ToString() + ",";
                            }

                            targetCategory = tmpCat.TrimEnd(',');
                        }
                    }
                    else
                    {
                        targetCategory = parentCategorys;
                    }
                }


                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SQL_PROC_BOARD_LIST_TOPDOWN");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@CategoryID", SqlDbType.VarChar, 2000),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Gubun", SqlDbType.VarChar,10),
                                                            new SqlParameter("@SearchValue", SqlDbType.NVarChar, 100),
                    };

                parms[0].Value = _companyID;
                parms[1].Value = targetCategory;
                parms[2].Value = _listID;
                parms[3].Value = _gubun;
                parms[4].Value = _searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetListIDBeforeNext", parms);
                return (ds != null) ? ds.Tables[0].Rows[0][0].ToString() : "0";
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetListBeforeNext: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 조회수 증가시키기
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool ModByReadCount(ListInfo _listInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@CategoryID", SqlDbType.Int),
                                    new SqlParameter("@ListID", SqlDbType.Int),
                                    new SqlParameter("@ReadCount", SqlDbType.Int)
                };

                parms[0].Value = _listInfo.CompanyID;
                parms[1].Value = _listInfo.CategoryID;
                parms[2].Value = _listInfo.ListID;
                parms[3].Value = _listInfo.ReadCount;

                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_listInfo.CompanyID), CommandType.StoredProcedure, "DPM_BOA_List_UpdByReadCount", parms);
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("ModByReadCount: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 읽지 않은 게시물 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryIDs"></param>
        /// <param name="_userID"></param>
        /// <param name="_tmpDay"></param>
        /// <returns></returns>
        public int GetNoReadListCnt(int _companyID, string _categoryIDs, int _userID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_PROC_NOREADLIST");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.VarChar,50),
                                                            new SqlParameter("@CategoryID", SqlDbType.VarChar,5000),
                                                            new SqlParameter("@UserID", SqlDbType.VarChar,500)
                };
                parms[0].Value = _companyID.ToString();
                parms[1].Value = _categoryIDs;
                parms[2].Value = _userID.ToString();

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetNoReadListCnt: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-09-09
        /// 작성목적 : 총 게시물 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryIDs"></param>
        /// <returns></returns>
        public int GetTotalCnt(int _companyID, string _categoryIDs)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_PROC_TOTAL");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.VarChar,50),
                                                            new SqlParameter("@CategoryID", SqlDbType.VarChar,5000)
                };
                parms[0].Value = _companyID.ToString();
                parms[1].Value = _categoryIDs;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetTotalCnt: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-09-09
        /// 작성목적 : 모바일 사용 게시판 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentCategorys"></param>
        /// <returns></returns>
        public DataSet GetUseMobile(int _companyID, string _parentCategorys)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_QRY_SELECTCATEGORY_USEMOBILE");
                sql = string.Format(sql, _companyID.ToString(), _parentCategorys);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsUSEMOBILE", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUseMobile: " + errMsg);
                throw;
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-16
        /// 작성목적 : 게시물 내용가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataSet GetBoardInfo(int _companyID, int _categoryID, int _listID)
        {
            try
            {
                if (_companyID < 0 || _categoryID < 0 || _listID < 0) return null;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_QRY_LIST");
                proc = string.Format(proc, _companyID, _categoryID, _listID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetBoardInfo", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBoardInfo: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-06-26
        /// 작성목적 : 게시물 작성
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public int Add(ListInfo _listinfo)
        {
            try
            {
                _listinfo.MobileContent = _listinfo.MobileContent.ToEditorXSS();
                _listinfo.MobileContent = Regex.Replace(_listinfo.MobileContent, "<img", "<ximg", RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_listinfo.CompanyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "INT_PROC_LIST");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@CategoryID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@GroupID", SqlDbType.Int),
                                                            new SqlParameter("@ListOrder", SqlDbType.Int),
                                                            new SqlParameter("@Depth", SqlDbType.Int),

                                                            new SqlParameter("@Title", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@Content", SqlDbType.NText),
                                                            new SqlParameter("@MobileContent", SqlDbType.NText),
                                                            new SqlParameter("@OwnerDeptID", SqlDbType.Int),
                                                            new SqlParameter("@OptionModify", SqlDbType.Char,1),

                                                            new SqlParameter("@OptionTop", SqlDbType.Char,1),
                                                            new SqlParameter("@OptionTopDay", SqlDbType.Int),
                                                            new SqlParameter("@OptionAnonymity", SqlDbType.Char,1),
                                                            new SqlParameter("@ScreenShot", SqlDbType.NVarChar,500),
                                                            new SqlParameter("@HeaderID", SqlDbType.Int),

                                                            new SqlParameter("@WriteUserID", SqlDbType.Int),
                                                            new SqlParameter("@WriteUserName", SqlDbType.NVarChar,20),
                                                            new SqlParameter("@PassWord", SqlDbType.NVarChar,50)
                };
                    parms[0].Value = _listinfo.CompanyID;
                    parms[1].Value = _listinfo.CategoryID;
                    parms[2].Value = _listinfo.ListID;
                    parms[3].Value = _listinfo.GroupID;
                    parms[4].Value = _listinfo.ListOrder;
                    parms[5].Value = _listinfo.Depth;
                    parms[6].Value = _listinfo.Title.ToXSS();
                    parms[7].Value = _listinfo.Content.ToEditorXSS();
                    parms[8].Value = _listinfo.MobileContent;
                    parms[9].Value = _listinfo.OwnerDeptID;
                    parms[10].Value = _listinfo.OptionModify;
                    parms[11].Value = _listinfo.OptionTop;
                    parms[12].Value = _listinfo.OptionTopDay;
                    parms[13].Value = _listinfo.OptionAnonymity;
                    parms[14].Value = _listinfo.ScreenShot.ToXSS();
                    parms[15].Value = _listinfo.HeaderID;
                    parms[16].Value = _listinfo.WriteUserID;
                    parms[17].Value = _listinfo.WriteUserName.ToXSS();
                    parms[18].Value = _listinfo.PassWord.ToXSS();

                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return Convert.ToInt32(obj);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                return -1;
            }
        }

        /// <summary>
        /// ScreenShot 수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_screenShot"></param>
        /// <returns></returns>
        public bool ModByScreenShot(int _companyID, int _categoryID, int _listID, string _screenShot)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@CategoryID", SqlDbType.Int),
                                    new SqlParameter("@ListID", SqlDbType.Int),
                                    new SqlParameter("@ScreenShot", SqlDbType.NVarChar,500)
                };

                parms[0].Value = _companyID;
                parms[1].Value = _categoryID;
                parms[2].Value = _listID;
                parms[3].Value = _screenShot;

                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DP_BOA_List_UpdScreenShot", parms);
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("ModByScreenShot: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-11-02
        /// 작성목적 : 게시물 카운트 가져오기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pCatID"></param>
        /// <param name="_isDeptBoard"></param>
        /// <param name="_deptID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetListCount(int _companyID, string _pCatID, string _isDeptBoard, int _deptID, string _searchValue)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_PROC_ListCnt");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@CategoryID", SqlDbType.VarChar,5000)
                                            ,new SqlParameter("@IsDeptBoard", SqlDbType.Char,1)
                                            ,new SqlParameter("@DeptID", SqlDbType.VarChar,50)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _pCatID;
                parm[2].Value = _isDeptBoard;
                parm[3].Value = _deptID;
                parm[4].Value = _searchValue;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetListCount: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-11-09
        /// 작성목적 : 게시판 임시저장 (파일등록을 위해서)
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public int AddDraft(ListInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_listinfo.CompanyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "INT_PROC_LISTDraft");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@HeaderID", SqlDbType.Int),
                                                            new SqlParameter("@FontColor", SqlDbType.NVarChar,10),
                                                            new SqlParameter("@FontSize", SqlDbType.NVarChar,10),
                                                            new SqlParameter("@Bold", SqlDbType.Char,1),
                                                            new SqlParameter("@OptionModify", SqlDbType.Char,1),
                                                            new SqlParameter("@OptionAnonymity", SqlDbType.Char,1),
                                                            new SqlParameter("@OptionTop", SqlDbType.Char,1),
                                                            new SqlParameter("@OptionTopDay", SqlDbType.Int),
                                                            new SqlParameter("@PopUp", SqlDbType.Char,1),
                                                            new SqlParameter("@StartDate", SqlDbType.Char,10),
                                                            new SqlParameter("@EndDate", SqlDbType.Char,10),                                                            
                                                            new SqlParameter("@Title", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@Content", SqlDbType.NText),                                                         
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int)
                };
                    parms[0].Value = _listinfo.CompanyID;
                    parms[1].Value = _listinfo.HeaderID;
                    parms[2].Value = _listinfo.FontColor;
                    parms[3].Value = _listinfo.FontSize;
                    parms[4].Value = _listinfo.Bold;
                    parms[5].Value = _listinfo.OptionModify;
                    parms[6].Value = _listinfo.OptionAnonymity;
                    parms[7].Value = _listinfo.OptionTop;
                    parms[8].Value = _listinfo.OptionTopDay;
                    parms[9].Value = _listinfo.PopUp;
                    parms[10].Value = _listinfo.StartDate;
                    parms[11].Value = _listinfo.EndDate;
                    parms[12].Value = _listinfo.Title;
                    parms[13].Value = _listinfo.Content;
                    parms[14].Value = _listinfo.WriteUserID;

                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return Convert.ToInt32(obj);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("AddDraft: " + errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-11-09
        /// 작성목적 : 임시저장한 글 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftListID"></param>
        /// <returns></returns>
        public DataSet GetDraft(string _companyID, int _draftListID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_QRY_Draft");
                sql = string.Format(sql, _companyID, _draftListID);
                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsSeq", null);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDraft: " + errMsg);
                throw;
            }
        }

    }
}