using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class Enforce : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.EnforceInfo GetEnforceInfo(IDataReader _reader)
        {
            return new EnforceInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                EnforceID = (_reader["EnforceID"] == DBNull.Value) ? -1 : _reader["EnforceID"].ToString().ToNumber<int>(),
                ReqUserID = (_reader["ReqUserID"] == DBNull.Value) ? -1 : _reader["ReqUserID"].ToString().ToNumber<int>(),
                ReqNameBase = _reader["ReqNameBase"].ToString().ToNull(),
                ReqNameKo = _reader["ReqNameKo"].ToString().ToNull(),
                ReqNameEn = _reader["ReqNameEn"].ToString().ToNull(),
                ReqNameJa = _reader["ReqNameJa"].ToString().ToNull(),
                ReqNameFr = _reader["ReqNameFr"].ToString().ToNull(),
                ReqNameZhcn = _reader["ReqNameZhcn"].ToString().ToNull(),
                ReqNameZhtw = _reader["ReqNameZhtw"].ToString().ToNull(),
                ReqManyName = _reader["ReqManyName"].ToString().ToNull(),
                ReqDeptCodeID = (_reader["ReqDeptCodeID"] == DBNull.Value) ? -1 : _reader["ReqDeptCodeID"].ToString().ToNumber<int>(),
                ReqUserMail = _reader["ReqUserMail"].ToString().ToNull(),
                ReqNumber = _reader["ReqNumber"].ToString().ToNull(),
                ReqCount = (_reader["ReqCount"] == DBNull.Value) ? -1 : _reader["ReqCount"].ToString().ToNumber<int>(),
                EnforceStampID = (_reader["EnforceStampID"] == DBNull.Value) ? -1 : _reader["EnforceStampID"].ToString().ToNumber<int>(),
                StampDeptID = (_reader["StampDeptID"] == DBNull.Value) ? -1 : _reader["StampDeptID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                Subject = _reader["Subject"].ToString().ToNull(),
                ContentHTML = _reader["ContentHTML"].ToString().ToNull().ToEditorXSS(),
                Receive = _reader["Receive"].ToString().ToNull(),
                Reference = _reader["Reference"].ToString().ToNull(),
                EnforceDate = _reader["EnforceDate"].ToString().ToNull(),
                ExpFirst = _reader["ExpFirst"].ToString().ToNull(),
                ExpMid = _reader["ExpMid"].ToString().ToNull(),
                ExpEnd = _reader["ExpEnd"].ToString().ToNull(),
                ZipCode = _reader["ZipCode"].ToString().ToNull(),
                AddressFirst = _reader["AddressFirst"].ToString().ToNull(),
                AddressEnd = _reader["AddressEnd"].ToString().ToNull(),
                Phone = _reader["Phone"].ToString().ToNull(),
                Fax = _reader["Fax"].ToString().ToNull(),
                Homepage = _reader["Homepage"].ToString().ToNull(),
                SendType = _reader["SendType"].ToString().ToNull(),
                StampType = _reader["StampType"].ToString().ToNull(),
                DocType = _reader["DocType"].ToString().ToNull(),
                Quantity = _reader["Quantity"].ToString().ToNull(),
                Purpose = _reader["Purpose"].ToString().ToNull(),
                TargetMail = _reader["TargetMail"].ToString().ToNull(),
                TargetMailSubject = _reader["TargetMailSubject"].ToString().ToNull(),
                FormPath = _reader["FormPath"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ConfirmUserID = (_reader["ConfirmUserID"] == DBNull.Value) ? -1 : _reader["ConfirmUserID"].ToString().ToNumber<int>(),
                ConfirmNameBase = _reader["ConfirmNameBase"].ToString().ToNull(),
                ConfirmNameKo = _reader["ConfirmNameKo"].ToString().ToNull(),
                ConfirmNameEn = _reader["ConfirmNameEn"].ToString().ToNull(),
                ConfirmNameJa = _reader["ConfirmNameJa"].ToString().ToNull(),
                ConfirmNameFr = _reader["ConfirmNameFr"].ToString().ToNull(),
                ConfirmNameZhcn = _reader["ConfirmNameZhcn"].ToString().ToNull(),
                ConfirmNameZhtw = _reader["ConfirmNameZhtw"].ToString().ToNull(),
                ConfirmManyName = _reader["ConfirmManyName"].ToString().ToNull(),
                ConfirmDeptCodeID = (_reader["ConfirmDeptCodeID"] == DBNull.Value) ? -1 : _reader["ConfirmDeptCodeID"].ToString().ToNumber<int>(),
                ConfirmDate = (_reader["ConfirmDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ConfirmDate"],
                ConfirmNumber = _reader["ConfirmNumber"].ToString().ToNull(),
                ConfirmCount = (_reader["ConfirmCount"] == DBNull.Value) ? -1 : _reader["ConfirmCount"].ToString().ToNumber<int>(),
                ConfirmOpinion = _reader["ConfirmOpinion"].ToString().ToNull(),
                State = _reader["State"].ToString().ToNull(),
                IsSend = _reader["IsSend"].ToString().ToNull(),

                ReqDeptID = (_reader["ReqDeptID"] == DBNull.Value) ? -1 : _reader["ReqDeptID"].ToString().ToNumber<int>(),
                ReqTitleID = (_reader["ReqTitleID"] == DBNull.Value) ? -1 : _reader["ReqTitleID"].ToString().ToNumber<int>(),
                ReqDutyID = (_reader["ReqDutyID"] == DBNull.Value) ? -1 : _reader["ReqDutyID"].ToString().ToNumber<int>(),
                ConfirmDeptID = (_reader["ConfirmDeptID"] == DBNull.Value) ? -1 : _reader["ConfirmDeptID"].ToString().ToNumber<int>(),
                ConfirmTitleID = (_reader["ConfirmTitleID"] == DBNull.Value) ? -1 : _reader["ConfirmTitleID"].ToString().ToNumber<int>(),
                ConfirmDutyID = (_reader["ConfirmDutyID"] == DBNull.Value) ? -1 : _reader["ConfirmDutyID"].ToString().ToNumber<int>(),

                #region 문서정보

                DocNumber = _reader["DocNumber"].ToString().ToNull(),
                WriteTime = (_reader["WriteTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteTime"],
                CompleteDate = (_reader["CompleteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["CompleteDate"],

                #endregion 문서정보

                #region ####  이름,부서,직급,직책 다국어 시작 #######

                ReqDeptBase = _reader["ReqDeptBase"].ToString().ToNull(),
                ReqDeptKo = _reader["ReqDeptKo"].ToString().ToNull(),
                ReqDeptEn = _reader["ReqDeptEn"].ToString().ToNull(),
                ReqDeptJa = _reader["ReqDeptJa"].ToString().ToNull(),
                ReqDeptFr = _reader["ReqDeptFr"].ToString().ToNull(),
                ReqDeptZhcn = _reader["ReqDeptZhcn"].ToString().ToNull(),
                ReqDeptZhtw = _reader["ReqDeptZhtw"].ToString().ToNull(),
                ReqTitleBase = _reader["ReqTitleBase"].ToString().ToNull(),
                ReqTitleKo = _reader["ReqTitleKo"].ToString().ToNull(),
                ReqTitleEn = _reader["ReqTitleEn"].ToString().ToNull(),
                ReqTitleJa = _reader["ReqTitleJa"].ToString().ToNull(),
                ReqTitleFr = _reader["ReqTitleFr"].ToString().ToNull(),
                ReqTitleZhcn = _reader["ReqTitleZhcn"].ToString().ToNull(),
                ReqTitleZhtw = _reader["ReqTitleZhtw"].ToString().ToNull(),
                ReqDutyBase = _reader["ReqDutyBase"].ToString().ToNull(),
                ReqDutyKo = _reader["ReqDutyKo"].ToString().ToNull(),
                ReqDutyEn = _reader["ReqDutyEn"].ToString().ToNull(),
                ReqDutyJa = _reader["ReqDutyJa"].ToString().ToNull(),
                ReqDutyFr = _reader["ReqDutyFr"].ToString().ToNull(),
                ReqDutyZhcn = _reader["ReqDutyZhcn"].ToString().ToNull(),
                ReqDutyZhtw = _reader["ReqDutyZhtw"].ToString().ToNull(),

                ConfirmDeptBase = _reader["ConfirmDeptBase"].ToString().ToNull(),
                ConfirmDeptKo = _reader["ConfirmDeptKo"].ToString().ToNull(),
                ConfirmDeptEn = _reader["ConfirmDeptEn"].ToString().ToNull(),
                ConfirmDeptJa = _reader["ConfirmDeptJa"].ToString().ToNull(),
                ConfirmDeptFr = _reader["ConfirmDeptFr"].ToString().ToNull(),
                ConfirmDeptZhcn = _reader["ConfirmDeptZhcn"].ToString().ToNull(),
                ConfirmDeptZhtw = _reader["ConfirmDeptZhtw"].ToString().ToNull(),
                ConfirmTitleBase = _reader["ConfirmTitleBase"].ToString().ToNull(),
                ConfirmTitleKo = _reader["ConfirmTitleKo"].ToString().ToNull(),
                ConfirmTitleEn = _reader["ConfirmTitleEn"].ToString().ToNull(),
                ConfirmTitleJa = _reader["ConfirmTitleJa"].ToString().ToNull(),
                ConfirmTitleFr = _reader["ConfirmTitleFr"].ToString().ToNull(),
                ConfirmTitleZhcn = _reader["ConfirmTitleZhcn"].ToString().ToNull(),
                ConfirmTitleZhtw = _reader["ConfirmTitleZhtw"].ToString().ToNull(),
                ConfirmDutyBase = _reader["ConfirmDutyBase"].ToString().ToNull(),
                ConfirmDutyKo = _reader["ConfirmDutyKo"].ToString().ToNull(),
                ConfirmDutyEn = _reader["ConfirmDutyEn"].ToString().ToNull(),
                ConfirmDutyJa = _reader["ConfirmDutyJa"].ToString().ToNull(),
                ConfirmDutyFr = _reader["ConfirmDutyFr"].ToString().ToNull(),
                ConfirmDutyZhcn = _reader["ConfirmDutyZhcn"].ToString().ToNull(),
                ConfirmDutyZhtw = _reader["ConfirmDutyZhtw"].ToString().ToNull()

                #endregion ####  이름,부서,직급,직책 다국어 시작 #######
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<EnforceInfo> GetEnforceInfoList(IDataReader reader)
        {
            List<EnforceInfo> list = new List<EnforceInfo>();
            while (reader.Read())
                list.Add(GetEnforceInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 특정 시행문정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<EnforceInfo> GetEnforce(int _companyID, int _enforceID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Enforce_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@EnforceID", SqlDbType.Int).Value = _enforceID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetEnforceInfoList(reader);
                }
            }
        }

        #region 공문리스트

        /// <summary>
        /// 공문리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageListEnforce(int _companyID, int _UserID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListEnforce", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 공문리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageCountEnforce(int _companyID, int _userID, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntEnforce", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 공문리스트

        #region 공문신청리스트

        /// <summary>
        /// 공문신청리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageListEnforceSubmit(int _companyID, int _UserID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListEnforceSubmit", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 공문신청리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageCountEnforceSubmit(int _companyID, int _userID, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntEnforceSubmit", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 공문신청리스트

        #region 공문발송리스트

        /// <summary>
        /// 공문발송리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageListEnforceSend(int _companyID, int _UserID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListEnforceSend", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 공문발송리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageCountEnforceSend(int _companyID, int _userID, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntEnforceSend", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 공문발송리스트

        #region 공문신청리스트-관리자

        /// <summary>
        /// 공문신청리스트-관리자
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageListEnforceSubmitAdmin(int _companyID, int _UserID, string _enforceStampIDArr, string _stampDeptIDArr, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListEnforceSubmitAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@EnforceStampIDArr", SqlDbType.VarChar, 50).Value = _enforceStampIDArr;
                cmd.Parameters.Add("@StampDeptIDArr", SqlDbType.VarChar, 50).Value = _stampDeptIDArr;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 공문신청리스트-관리자
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageCountEnforceSubmitAdmin(int _companyID, int _userID, string _enforceStampIDArr, string _stampDeptIDArr, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntEnforceSubmitAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@EnforceStampIDArr", SqlDbType.VarChar, 50).Value = _enforceStampIDArr;
                cmd.Parameters.Add("@StampDeptIDArr", SqlDbType.VarChar, 50).Value = _stampDeptIDArr;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 공문신청리스트-관리자

        #region 공문발송리스트-관리자

        /// <summary>
        /// 공문발송리스트-관리자
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageListEnforceSendAdmin(int _companyID, int _UserID, string _enforceStampIDArr, string _stampDeptIDArr, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListEnforceSendAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@EnforceStampIDArr", SqlDbType.VarChar, 50).Value = _enforceStampIDArr;
                cmd.Parameters.Add("@StampDeptIDArr", SqlDbType.VarChar, 50).Value = _stampDeptIDArr;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 공문발송리스트-관리자
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageCountEnforceSendAdmin(int _companyID, int _userID, string _enforceStampIDArr, string _stampDeptIDArr, string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntEnforceSendAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@EnforceStampIDArr", SqlDbType.VarChar, 50).Value = _enforceStampIDArr;
                cmd.Parameters.Add("@StampDeptIDArr", SqlDbType.VarChar, 50).Value = _stampDeptIDArr;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 공문발송리스트-관리자

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 공문신청
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.EnforceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Enforce_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ReqUserID", SqlDbType.Int).Value = _info.ReqUserID;
                    cmd.Parameters.Add("@ReqUserMail", SqlDbType.NVarChar, 50).Value = _info.ReqUserMail.ToNull();
                    cmd.Parameters.Add("@EnforceStampID", SqlDbType.Int).Value = _info.EnforceStampID;
                    cmd.Parameters.Add("@StampDeptID", SqlDbType.Int).Value = _info.StampDeptID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 100).Value = _info.Subject.ToNull().ToXSS();
                    cmd.Parameters.Add("@ContentHTML", SqlDbType.NText).Value = _info.ContentHTML.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@Receive", SqlDbType.NVarChar, 100).Value = _info.Receive.ToNull();
                    cmd.Parameters.Add("@Reference", SqlDbType.NVarChar, 100).Value = _info.Reference.ToNull();
                    cmd.Parameters.Add("@EnforceDate", SqlDbType.NVarChar, 100).Value = _info.EnforceDate.ToNull();
                    cmd.Parameters.Add("@ExpFirst", SqlDbType.NVarChar, 100).Value = _info.ExpFirst.ToNull();
                    cmd.Parameters.Add("@ExpMid", SqlDbType.NVarChar, 100).Value = _info.ExpMid.ToNull();
                    cmd.Parameters.Add("@ExpEnd", SqlDbType.NVarChar, 100).Value = _info.ExpEnd.ToNull();
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 50).Value = _info.ZipCode.ToNull();
                    cmd.Parameters.Add("@AddressFirst", SqlDbType.NVarChar, 100).Value = _info.AddressFirst.ToNull();
                    cmd.Parameters.Add("@AddressEnd", SqlDbType.NVarChar, 100).Value = _info.AddressEnd.ToNull();
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = _info.Phone.ToNull();
                    cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = _info.Fax.ToNull();
                    cmd.Parameters.Add("@Homepage", SqlDbType.NVarChar, 100).Value = _info.Homepage.ToNull();
                    cmd.Parameters.Add("@SendType", SqlDbType.Char, 1).Value = _info.SendType.ToNull();
                    cmd.Parameters.Add("@StampType", SqlDbType.Char, 1).Value = _info.StampType.ToNull();
                    cmd.Parameters.Add("@DocType", SqlDbType.Char, 1).Value = _info.DocType.ToNull();
                    cmd.Parameters.Add("@Quantity", SqlDbType.VarChar, 10).Value = _info.Quantity.ToNull();
                    cmd.Parameters.Add("@Purpose", SqlDbType.NVarChar, 500).Value = _info.Purpose.ToNull();
                    cmd.Parameters.Add("@TargetMail", SqlDbType.NVarChar, 100).Value = _info.TargetMail.ToNull();
                    cmd.Parameters.Add("@TargetMailSubject", SqlDbType.NVarChar, 100).Value = _info.TargetMailSubject.ToNull();
                    cmd.Parameters.Add("@FormPath", SqlDbType.NVarChar, 100).Value = _info.FormPath.ToNull();

                    #endregion 파라미터 처리

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

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 공문수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(BE.EnforceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Enforce_UpdEnforceID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@EnforceID", SqlDbType.Int).Value = _info.EnforceID;
                    cmd.Parameters.Add("@EnforceStampID", SqlDbType.Int).Value = _info.EnforceStampID;
                    cmd.Parameters.Add("@StampDeptID", SqlDbType.Int).Value = _info.StampDeptID;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 100).Value = _info.Subject.ToNull().ToXSS();
                    cmd.Parameters.Add("@ContentHTML", SqlDbType.NText).Value = _info.ContentHTML.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@Receive", SqlDbType.NVarChar, 100).Value = _info.Receive.ToNull();
                    cmd.Parameters.Add("@Reference", SqlDbType.NVarChar, 100).Value = _info.Reference.ToNull();
                    cmd.Parameters.Add("@EnforceDate", SqlDbType.NVarChar, 100).Value = _info.EnforceDate.ToNull();
                    cmd.Parameters.Add("@ExpFirst", SqlDbType.NVarChar, 100).Value = _info.ExpFirst.ToNull();
                    cmd.Parameters.Add("@ExpMid", SqlDbType.NVarChar, 100).Value = _info.ExpMid.ToNull();
                    cmd.Parameters.Add("@ExpEnd", SqlDbType.NVarChar, 100).Value = _info.ExpEnd.ToNull();
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 50).Value = _info.ZipCode.ToNull();
                    cmd.Parameters.Add("@AddressFirst", SqlDbType.NVarChar, 100).Value = _info.AddressFirst.ToNull();
                    cmd.Parameters.Add("@AddressEnd", SqlDbType.NVarChar, 100).Value = _info.AddressEnd.ToNull();
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = _info.Phone.ToNull();
                    cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = _info.Fax.ToNull();
                    cmd.Parameters.Add("@Homepage", SqlDbType.NVarChar, 100).Value = _info.Homepage.ToNull();
                    cmd.Parameters.Add("@SendType", SqlDbType.Char, 1).Value = _info.SendType.ToNull();
                    cmd.Parameters.Add("@StampType", SqlDbType.Char, 1).Value = _info.StampType.ToNull();
                    cmd.Parameters.Add("@DocType", SqlDbType.Char, 1).Value = _info.DocType.ToNull();
                    cmd.Parameters.Add("@Quantity", SqlDbType.VarChar, 10).Value = _info.Quantity.ToNull();
                    cmd.Parameters.Add("@Purpose", SqlDbType.NVarChar, 500).Value = _info.Purpose.ToNull();
                    cmd.Parameters.Add("@TargetMail", SqlDbType.NVarChar, 100).Value = _info.TargetMail.ToNull();
                    cmd.Parameters.Add("@TargetMailSubject", SqlDbType.NVarChar, 100).Value = _info.TargetMailSubject.ToNull();
                    cmd.Parameters.Add("@FormPath", SqlDbType.NVarChar, 100).Value = _info.FormPath.ToNull();

                    #endregion 파라미터 처리

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
        /// 공문승인/반려
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModConfirm(BE.EnforceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Enforce_UpdConfirm", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@EnforceID", SqlDbType.Int).Value = _info.EnforceID;
                    cmd.Parameters.Add("@EnforceStampID", SqlDbType.Int).Value = _info.EnforceStampID;
                    cmd.Parameters.Add("@StampDeptID", SqlDbType.Int).Value = _info.StampDeptID;
                    cmd.Parameters.Add("@ConfirmUserID", SqlDbType.Int).Value = _info.ConfirmUserID;
                    cmd.Parameters.Add("@State", SqlDbType.Char, 1).Value = _info.State.ToNull();
                    cmd.Parameters.Add("@ConfirmOpinion", SqlDbType.NVarChar, 500).Value = _info.ConfirmOpinion.ToNull();

                    #endregion 파라미터 처리

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

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 시행문삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_enforceID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _enforceID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Enforce_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@EnforceID", SqlDbType.Int).Value = _enforceID;

                    #endregion 파라미터 처리

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

        #endregion DEL()
    }
}