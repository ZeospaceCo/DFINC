using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.RM.BE;

namespace Zeospace.DeskPlus.RM.DAL
{
    public class List : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 폴더별 게시물 개수
        /// </summary>
        /// <param name="_companyID"></param>        
        /// <param name="_categoryID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _categoryID, string _searchField, string _searchValue, string _startDate, string _endDate)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_List_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _startDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _endDate;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 검색 페이징 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _categoryID, string _pageSize, string _pageSizePageNo,
            string _searchField, string _searchValue, string _startDate, string _endDate, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_List_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _startDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _endDate;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-15
        /// 작성목적 : 폴더별 자원리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _categoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_List_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-06
        /// 작성목적 : 폴더별 자원리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <returns></returns>
        public DataSet GetWeekList(int _companyID, int _categoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_List_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetWeekList");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-
        /// 작성목적 : 자원상세 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataTableReader GetListInfo(int _companyID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_List_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
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


        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-03-08
        /// 작성목적 : QRCode 갯수 
        /// </summary>
        /// <param name="_companyID"></param>        
        /// <param name="_categoryID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetQRCodeListCount(int _companyID, string _searchField, string _searchValue, string _startDate, string _endDate)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_List_SelQRCodePageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _startDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _endDate;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-03-08
        /// 작성목적 : QRCode 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetQRCodeList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchField, string _searchValue, string _startDate, string _endDate, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_RM_List_SelQRCodePageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _startDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _endDate;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-18
        /// 작성목적 : 본인이 주 관리자로 있는 자원리스트 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetListUser(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_List_SelByUserID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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
        /// 작성자   : famo23
        /// 작성일자 : 2016-03-18
        /// 작성목적 :  자원리스트 Top (홈화면)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataTableReader GetTopList(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_List_SelTopList");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
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
        /// 작성자   : famo23
        /// 작성일자 : 2016-03-18
        /// 작성목적 :  자원리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataTableReader GetHomeList(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_List_SelList");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
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
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-08
        /// 작성목적 : 본인이 주 관리자로 있는 자원리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <returns></returns>
        public DataTableReader GetManagerList(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_List_SelListEtc");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
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
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 자원 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_RM_List_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 30).Value = _info.Code.ToNull().ToXSS();
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 50).Value = _info.Title.ToNull().ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToXSS();                   
                    cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = _info.Qty;
                    cmd.Parameters.Add("@UsableQty", SqlDbType.Int).Value = _info.UsableQty;
                    cmd.Parameters.Add("@IconName", SqlDbType.Char, 1).Value = _info.IconName.ToNull();
                    cmd.Parameters.Add("@Place", SqlDbType.NVarChar, 50).Value = _info.Place.ToNull().ToXSS();
                    cmd.Parameters.Add("@Authority", SqlDbType.Char, 1).Value = _info.Authority.ToNull();
                    cmd.Parameters.Add("@QRCodeUse", SqlDbType.Char, 1).Value = _info.QRCodeUse.ToNull();
                    cmd.Parameters.Add("@QRCodePath", SqlDbType.NVarChar, 1000).Value = _info.QRCodePath.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 50).Value = _info.WriteUserName.ToNull().ToXSS(); 

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

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 삭제 (ActiveUse)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_activeUse"></param>
        /// <param name="_tmpListID"></param>
        /// <returns></returns>
        public bool Upd(int _companyID, string _activeUse, string _tmpListID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_List_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ActiveUse", SqlDbType.Char, 1).Value = _activeUse;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 4000).Value = _tmpListID;

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
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 자원 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_RM_List_UpdByID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 30).Value = _info.Code.ToNull().ToXSS();
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 50).Value = _info.Title.ToNull().ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToXSS();
                    cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = _info.Qty;
                    cmd.Parameters.Add("@UsableQty", SqlDbType.Int).Value = _info.UsableQty;
                    cmd.Parameters.Add("@IconName", SqlDbType.Char, 1).Value = _info.IconName.ToNull();
                    cmd.Parameters.Add("@Place", SqlDbType.NVarChar, 50).Value = _info.Place.ToNull().ToXSS();
                    cmd.Parameters.Add("@Authority", SqlDbType.Char, 1).Value = _info.Authority.ToNull();
                    cmd.Parameters.Add("@QRCodeUse", SqlDbType.Char, 1).Value = _info.QRCodeUse.ToNull();
                    cmd.Parameters.Add("@QRCodePath", SqlDbType.NVarChar, 1000).Value = _info.QRCodePath.ToNull();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();

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
        /// 작성자 : 심재운
        /// 용도 : 순서변경
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _list.</param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, string _list)
        {
            try
            {
                if (_list.Length > 0)
                {
                    string[] listStr = _list.Split(',');
                    int ListID, CategoryID;

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < listStr.Length; i++)
                        {
                            ListID = listStr[i].ToString().Split('|')[0].ToString().ToNumber<int>();
                            CategoryID = listStr[i].ToString().Split('|')[1].ToString().ToNumber<int>();

                            var cmd = new SqlCommand("DP_RM_List_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = ListID;
                            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CategoryID;
                            cmd.Parameters.Add("@ListOrder", SqlDbType.Int).Value = i + 1;

                            ExecuteNonQuery(cmd);
                        }
                    }
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

        #endregion
    }
}
