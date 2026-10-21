using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Address.BE;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.DAL
{
    public class Private : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {        

        /// <summary>
        /// Gets the private.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.PrivateInfo GetPrivate(IDataReader _reader)
        {
            return new BE.PrivateInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                PrivateID = (_reader["PrivateID"] == DBNull.Value) ? -1 : _reader["PrivateID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                InterfaceID = _reader["InterfaceID"].ToString().ToNull(),
                LoginID = _reader["LoginID"].ToString().ToNull(),
                Sex = _reader["Sex"].ToString().ToNull(),
                LastName = _reader["LastName"].ToString().ToNull(),
                FamilyName = _reader["FamilyName"].ToString().ToNull(),
                MiddleName = _reader["MiddleName"].ToString().ToNull(),
                WholeName = _reader["WholeName"].ToString().ToNull(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                NickName = _reader["NickName"].ToString().ToNull(),
                Mail = _reader["Mail"].ToString().ToNull(),
                HomeZipCode = _reader["HomeZipCode"].ToString().ToNull(),
                HomeCountry = _reader["HomeCountry"].ToString().ToNull(),
                HomeCity = _reader["HomeCity"].ToString().ToNull(),
                HomeGu = _reader["HomeGu"].ToString().ToNull(),
                HomeAddress = _reader["HomeAddress"].ToString().ToNull(),
                HomePhone = _reader["HomePhone"].ToString().ToNull(),
                HomeFax = _reader["HomeFax"].ToString().ToNull(),
                MobilePhone = _reader["MobilePhone"].ToString().ToNull(),
                PrivateHomePage = _reader["PrivateHomePage"].ToString().ToNull(),
                CompanyZipCode = _reader["CompanyZipCode"].ToString().ToNull(),
                CompanyCountry = _reader["CompanyCountry"].ToString().ToNull(),
                CompanyCity = _reader["CompanyCity"].ToString().ToNull(),
                CompanyGu = _reader["CompanyGu"].ToString().ToNull(),
                CompanyAddress = _reader["CompanyAddress"].ToString().ToNull(),
                CompanyPhone = _reader["CompanyPhone"].ToString().ToNull(),
                CompanyFax = _reader["CompanyFax"].ToString().ToNull(),
                CompanyHomePage = _reader["CompanyHomePage"].ToString().ToNull(),
                Pager = _reader["Pager"].ToString().ToNull(),
                CompanyBase = _reader["CompanyBase"].ToString().ToNull(),
                CompanyKo = _reader["CompanyKo"].ToString().ToNull(),
                CompanyEn = _reader["CompanyEn"].ToString().ToNull(),
                CompanyJA = _reader["CompanyJA"].ToString().ToNull(),
                CompanyFr = _reader["CompanyFr"].ToString().ToNull(),
                CompanyZhcn = _reader["CompanyZhcn"].ToString().ToNull(),
                CompanyZhtw = _reader["CompanyZhtw"].ToString().ToNull(),
                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                DeptManyName = _reader["DeptManyName"].ToString().ToNull(),
                OfficeAddress = _reader["OfficeAddress"].ToString().ToNull(),
                Memo = _reader["Memo"].ToString().ToNull(),
                Birthday = _reader["Birthday"].ToString().ToNull(),
                Lunar = _reader["Lunar"].ToString().ToNull(),
                IpPhone = _reader["IpPhone"].ToString().ToNull(),
                DefaultPhone = _reader["DefaultPhone"].ToString().ToNull(),
                PhotoName = _reader["PhotoName"].ToString().ToNull(),
                IsReceive = _reader["IsReceive"].ToString().ToNull(),
                SendUserID = (_reader["SendUserID"] == DBNull.Value) ? -1 : _reader["SendUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        /// <summary>
        /// Gets the private list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<PrivateInfo> GetPrivateList(IDataReader reader)
        {
            List<PrivateInfo> list = new List<PrivateInfo>();
            while (reader.Read())
                list.Add(GetPrivate(reader));
            return list;
        }

        #region GET()

        /// <summary>
        /// 작성자 : 심재운
        /// 주소록 트리 생성하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_ownerID">The _owner ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public DataSet GetTree(int _companyID, string _ownerID, int _userID, string _preLang)
        {
            _ownerID = (_ownerID == "0") ? "" : _ownerID;

            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_Tree", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@OwnerID", SqlDbType.VarChar, 100).Value = _ownerID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PreLang", SqlDbType.VarChar, 10).Value = _preLang;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetAddressTree");
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
        /// 주소록 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_types">The _types.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_filter">The _filter.</param>
        /// <returns></returns>
        public int GetPageCount(int _companyID, string _categoryID, int _userID, string _types, 
            string _searchKey, string _searchValue, string _filter)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_SelPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 1000).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 30).Value = _types;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 100).Value = _filter;

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
        /// 주소록 카운트 가져오기(하위포함)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_types">The _types.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_filter">The _filter.</param>
        /// <returns></returns>
        public int GetPageCountWithSub(int _companyID, string _categoryID, int _userID, string _types,
            string _searchOption, string _searchKey, string _searchValue, string _filter)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_SelPageCntWithSub", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 1000).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 30).Value = _types;
                    cmd.Parameters.Add("@SearchOption", SqlDbType.NVarChar, 50).Value = _searchOption;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 100).Value = _filter;

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
        /// 주소록 페이징 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_types">The _types.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_filter">The _filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _categoryID, int _userID, string _types, string _pageSize, string _pageSizePageNo,
            string _searchKey, string _searchValue, string _filter, string _orders)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 1000).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 30).Value = _types;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 100).Value = _filter;
                    cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _orders;

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
        /// 주소록 페이징 리스트(하위포함)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_types">The _types.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_filter">The _filter.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public DataTableReader GetPageListWithSub(int _companyID, string _categoryID, int _userID, string _types, string _pageSize, string _pageSizePageNo,
            string _searchOption, string _searchKey, string _searchValue, string _filter, string _orders)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_SelPageListWithSub", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 1000).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 30).Value = _types;
                    cmd.Parameters.Add("@SearchOption", SqlDbType.NVarChar, 50).Value = _searchOption;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                    cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 100).Value = _filter;
                    cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _orders;

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
        /// 작성자 : 심재운
        /// 개인 주소록 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_privateID">The _private ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.PrivateInfo> GetPrivateLists(int _companyID, int _categoryID, int _privateID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ADDRESS, "QRY_ADD_Private_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@PrivateID", SqlDbType.Int).Value = _privateID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetPrivateList(reader);
                    }
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
        /// 사용자 정보 갖고오기(인쇄용)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userList">The _user list.</param>
        /// <returns></returns>
        public DataTableReader GetUserListByAddress(int _companyID, string _userList, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ADDRESS, "QRY_ADD_Private_SelByUsers");
                query = string.Format(query, _userList.ToInject(), _userID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-06-07
        /// 작성목적 : 주소록 연동 (이름으로 검색할시)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _userID, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_Sel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

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
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 주소록 (회사, 공용, 개인) 하위 폴더 포함
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_userID"></param>
        /// <param name="_types"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetListWithSub(int _companyID, string _categoryID, int _userID, string _types, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_SelListWithSub", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 1000).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 30).Value = _types;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
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
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 주소록 (전달받은 주소록, 직급주소록)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_userID"></param>
        /// <param name="_types"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, string _categoryID, int _userID, string _types, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_SelList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 1000).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Types", SqlDbType.VarChar, 30).Value = _types;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

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
        /// 개인주소록 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BE.PrivateInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_Private_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _info.LoginID.ToNull().ToXSS();
                    cmd.Parameters.Add("@Sex", SqlDbType.Char, 1).Value = _info.Sex.ToNull();
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = _info.LastName.ToNull().ToXSS();
                    cmd.Parameters.Add("@FamilyName", SqlDbType.NVarChar, 20).Value = _info.FamilyName.ToNull().ToXSS();
                    cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar, 20).Value = _info.MiddleName.ToNull().ToXSS();
                    cmd.Parameters.Add("@WholeName", SqlDbType.NVarChar, 40).Value = _info.WholeName.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@NickName", SqlDbType.NVarChar, 30).Value = _info.NickName.ToNull().ToXSS();
                    cmd.Parameters.Add("@Mail", SqlDbType.NVarChar, 80).Value = _info.Mail.ToNull();
                    cmd.Parameters.Add("@HomeZipCode", SqlDbType.NVarChar, 50).Value = _info.HomeZipCode.ToNull();
                    cmd.Parameters.Add("@HomeCountry", SqlDbType.NVarChar, 20).Value = _info.HomeCountry.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeCity", SqlDbType.NVarChar, 20).Value = _info.HomeCity.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeGu", SqlDbType.NVarChar, 20).Value = _info.HomeGu.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeAddress", SqlDbType.NVarChar, 100).Value = _info.HomeAddress.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomePhone", SqlDbType.NVarChar, 100).Value = _info.HomePhone.ToNull();
                    cmd.Parameters.Add("@HomeFax", SqlDbType.VarChar, 20).Value = _info.HomeFax.ToNull();
                    cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar, 100).Value = _info.MobilePhone.ToNull();
                    cmd.Parameters.Add("@PrivateHomePage", SqlDbType.NVarChar, 100).Value = _info.PrivateHomePage.ToNull();
                    cmd.Parameters.Add("@CompanyZipCode", SqlDbType.NVarChar, 50).Value = _info.CompanyZipCode.ToNull();
                    cmd.Parameters.Add("@CompanyCountry", SqlDbType.NVarChar, 20).Value = _info.CompanyCountry.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyCity", SqlDbType.NVarChar, 20).Value = _info.CompanyCity.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyGu", SqlDbType.NVarChar, 20).Value = _info.CompanyGu.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyAddress", SqlDbType.NVarChar, 100).Value = _info.CompanyAddress.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyPhone", SqlDbType.NVarChar, 100).Value = _info.CompanyPhone.ToNull();
                    cmd.Parameters.Add("@CompanyFax", SqlDbType.VarChar, 20).Value = _info.CompanyFax.ToNull();
                    cmd.Parameters.Add("@CompanyHomePage", SqlDbType.NVarChar, 100).Value = _info.CompanyHomePage.ToNull();
                    cmd.Parameters.Add("@Pager", SqlDbType.VarChar, 20).Value = _info.Pager.ToNull();
                    cmd.Parameters.Add("@CompanyBase", SqlDbType.NVarChar, 100).Value = _info.CompanyBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyKo", SqlDbType.NVarChar, 30).Value = _info.CompanyKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyEn", SqlDbType.NVarChar, 100).Value = _info.CompanyEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyJA", SqlDbType.NVarChar, 100).Value = _info.CompanyJA.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyFr", SqlDbType.NVarChar, 100).Value = _info.CompanyFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyZhcn", SqlDbType.NVarChar, 100).Value = _info.CompanyZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyZhtw", SqlDbType.NVarChar, 100).Value = _info.CompanyZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleBase", SqlDbType.NVarChar, 100).Value = _info.TitleBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleKo", SqlDbType.NVarChar, 30).Value = _info.TitleKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleEn", SqlDbType.NVarChar, 100).Value = _info.TitleEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleJa", SqlDbType.NVarChar, 100).Value = _info.TitleJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleFr", SqlDbType.NVarChar, 100).Value = _info.TitleFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleZhcn", SqlDbType.NVarChar, 100).Value = _info.TitleZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleZhtw", SqlDbType.NVarChar, 100).Value = _info.TitleZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = _info.DeptBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptKo", SqlDbType.NVarChar, 30).Value = _info.DeptKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptEn", SqlDbType.NVarChar, 100).Value = _info.DeptEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptJa", SqlDbType.NVarChar, 100).Value = _info.DeptJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptFr", SqlDbType.NVarChar, 100).Value = _info.DeptFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptZhcn", SqlDbType.NVarChar, 100).Value = _info.DeptZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptZhtw", SqlDbType.NVarChar, 100).Value = _info.DeptZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@OfficeAddress", SqlDbType.NVarChar, 100).Value = _info.OfficeAddress.ToNull().ToXSS();
                    cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 500).Value = _info.Memo.ToNull().ToXSS();
                    cmd.Parameters.Add("@Birthday", SqlDbType.VarChar, 20).Value = _info.Birthday.ToNull();
                    cmd.Parameters.Add("@Lunar", SqlDbType.Char, 1).Value = _info.Lunar.ToNull();
                    cmd.Parameters.Add("@IpPhone", SqlDbType.VarChar, 20).Value = _info.IpPhone.ToNull();
                    cmd.Parameters.Add("@DefaultPhone", SqlDbType.Char, 1).Value = _info.DefaultPhone.ToNull();
                    cmd.Parameters.Add("@PhotoName", SqlDbType.VarChar, 100).Value = _info.PhotoName.ToNull();
                    cmd.Parameters.Add("@IsReceive", SqlDbType.Char, 1).Value = _info.IsReceive.ToNull().ToXSS();
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _info.SendUserID;



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
        /// 작성자 : 심재운
        /// CSV 데이터 전달주소록에 할당하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_csvIDs"></param>
        /// <returns></returns>
        public bool AddReceived(int _companyID, int _userID, string _csvIDs)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_CSV_InsByReceived", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@CSVID", SqlDbType.VarChar, 5000).Value = _csvIDs;

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
        /// 주소록 복사하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_toCategoryID">The _to category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_types">The _types.</param>
        /// <returns></returns>
        public bool AddCopy(int _companyID, int _categoryID, int _userID, int _toCategoryID,
            string _listID, string _types)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_Private_InsCopy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@ToCategoryID", SqlDbType.Int).Value = _toCategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 1000).Value = _listID;
                    cmd.Parameters.Add("@Types", SqlDbType.Char, 2).Value = _types;


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
        /// 작성자 : shine
        /// 주소록 전달 추가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_sendUserID">The _send user ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public bool AddSend(int _companyID, int _userID, int _sendUserID, string _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_Private_InsReceived", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _sendUserID;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 1000).Value = _listID;

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
        

        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(BE.PrivateInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_Private_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@PrivateID", SqlDbType.Int).Value = _info.PrivateID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _info.LoginID.ToNull().ToXSS();
                    cmd.Parameters.Add("@Sex", SqlDbType.Char, 1).Value = _info.Sex.ToNull().ToXSS();
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = _info.LastName.ToNull().ToXSS();
                    cmd.Parameters.Add("@FamilyName", SqlDbType.NVarChar, 20).Value = _info.FamilyName.ToNull().ToXSS();
                    cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar, 20).Value = _info.MiddleName.ToNull().ToXSS();
                    cmd.Parameters.Add("@WholeName", SqlDbType.NVarChar, 40).Value = _info.WholeName.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@NickName", SqlDbType.NVarChar, 30).Value = _info.NickName.ToNull().ToXSS();
                    cmd.Parameters.Add("@Mail", SqlDbType.NVarChar, 80).Value = _info.Mail.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeZipCode", SqlDbType.NVarChar, 50).Value = _info.HomeZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeCountry", SqlDbType.NVarChar, 20).Value = _info.HomeCountry.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeCity", SqlDbType.NVarChar, 20).Value = _info.HomeCity.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeGu", SqlDbType.NVarChar, 20).Value = _info.HomeGu.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeAddress", SqlDbType.NVarChar, 100).Value = _info.HomeAddress.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomePhone", SqlDbType.NVarChar, 100).Value = _info.HomePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomeFax", SqlDbType.VarChar, 20).Value = _info.HomeFax.ToNull().ToXSS();
                    cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar, 100).Value = _info.MobilePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@PrivateHomePage", SqlDbType.NVarChar, 100).Value = _info.PrivateHomePage.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyZipCode", SqlDbType.NVarChar, 50).Value = _info.CompanyZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyCountry", SqlDbType.NVarChar, 20).Value = _info.CompanyCountry.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyCity", SqlDbType.NVarChar, 20).Value = _info.CompanyCity.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyGu", SqlDbType.NVarChar, 20).Value = _info.CompanyGu.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyAddress", SqlDbType.NVarChar, 100).Value = _info.CompanyAddress.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyPhone", SqlDbType.NVarChar, 100).Value = _info.CompanyPhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyFax", SqlDbType.VarChar, 20).Value = _info.CompanyFax.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyHomePage", SqlDbType.NVarChar, 100).Value = _info.CompanyHomePage.ToNull().ToXSS();
                    cmd.Parameters.Add("@Pager", SqlDbType.VarChar, 20).Value = _info.Pager.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyBase", SqlDbType.NVarChar, 100).Value = _info.CompanyBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyKo", SqlDbType.NVarChar, 30).Value = _info.CompanyKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyEn", SqlDbType.NVarChar, 100).Value = _info.CompanyEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyJA", SqlDbType.NVarChar, 100).Value = _info.CompanyJA.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyFr", SqlDbType.NVarChar, 100).Value = _info.CompanyFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyZhcn", SqlDbType.NVarChar, 100).Value = _info.CompanyZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyZhtw", SqlDbType.NVarChar, 100).Value = _info.CompanyZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleBase", SqlDbType.NVarChar, 100).Value = _info.TitleBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleKo", SqlDbType.NVarChar, 30).Value = _info.TitleKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleEn", SqlDbType.NVarChar, 100).Value = _info.TitleEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleJa", SqlDbType.NVarChar, 100).Value = _info.TitleJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleFr", SqlDbType.NVarChar, 100).Value = _info.TitleFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleZhcn", SqlDbType.NVarChar, 100).Value = _info.TitleZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@TitleZhtw", SqlDbType.NVarChar, 100).Value = _info.TitleZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = _info.DeptBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptKo", SqlDbType.NVarChar, 30).Value = _info.DeptKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptEn", SqlDbType.NVarChar, 100).Value = _info.DeptEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptJa", SqlDbType.NVarChar, 100).Value = _info.DeptJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptFr", SqlDbType.NVarChar, 100).Value = _info.DeptFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptZhcn", SqlDbType.NVarChar, 100).Value = _info.DeptZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptZhtw", SqlDbType.NVarChar, 100).Value = _info.DeptZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@OfficeAddress", SqlDbType.NVarChar, 100).Value = _info.OfficeAddress.ToNull().ToXSS();
                    cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 500).Value = _info.Memo.ToNull().ToXSS();
                    cmd.Parameters.Add("@Birthday", SqlDbType.VarChar, 20).Value = _info.Birthday.ToNull().ToXSS();
                    cmd.Parameters.Add("@Lunar", SqlDbType.Char, 1).Value = _info.Lunar.ToNull().ToXSS();
                    cmd.Parameters.Add("@IpPhone", SqlDbType.VarChar, 20).Value = _info.IpPhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@DefaultPhone", SqlDbType.Char, 1).Value = _info.DefaultPhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@PhotoName", SqlDbType.VarChar, 100).Value = _info.PhotoName.ToNull().ToXSS();
                    cmd.Parameters.Add("@IsReceive", SqlDbType.Char, 1).Value = _info.IsReceive.ToNull().ToXSS();
                    cmd.Parameters.Add("@SendUserID", SqlDbType.Int).Value = _info.SendUserID;

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
        /// 개인 주소록 이동하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_toCategoryID">The _to category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public bool ModMove(int _companyID, int _categoryID, int _userID, int _toCategoryID, string _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_Private_InsMove", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@ToCategoryID", SqlDbType.Int).Value = _toCategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 1000).Value = _listID;

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

        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 삭제 (privateid 는 콤마값으로 넘어온다)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_privateID">The _private ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, string _privateID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ADD_Private_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@PrivateID", SqlDbType.VarChar,1000).Value = _privateID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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

        #endregion
    }
}
