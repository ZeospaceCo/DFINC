using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class EnforceStamp : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.EnforceStampInfo GetEnforceStampInfo(IDataReader _reader)
        {
            return new EnforceStampInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                EnforceStampID = (_reader["EnforceStampID"] == DBNull.Value) ? -1 : _reader["EnforceStampID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                ExpFirstBase = _reader["ExpFirstBase"].ToString().ToNull(),
                ExpFirstKo = _reader["ExpFirstKo"].ToString().ToNull(),
                ExpFirstEn = _reader["ExpFirstEn"].ToString().ToNull(),
                ExpFirstJa = _reader["ExpFirstJa"].ToString().ToNull(),
                ExpFirstFr = _reader["ExpFirstFr"].ToString().ToNull(),
                ExpFirstZhcn = _reader["ExpFirstZhcn"].ToString().ToNull(),
                ExpFirstZhtw = _reader["ExpFirstZhtw"].ToString().ToNull(),
                ExpMidBase = _reader["ExpMidBase"].ToString().ToNull(),
                ExpMidKo = _reader["ExpMidKo"].ToString().ToNull(),
                ExpMidEn = _reader["ExpMidEn"].ToString().ToNull(),
                ExpMidJa = _reader["ExpMidJa"].ToString().ToNull(),
                ExpMidFr = _reader["ExpMidFr"].ToString().ToNull(),
                ExpMidZhcn = _reader["ExpMidZhcn"].ToString().ToNull(),
                ExpMidZhtw = _reader["ExpMidZhtw"].ToString().ToNull(),
                ExpEndBase = _reader["ExpEndBase"].ToString().ToNull(),
                ExpEndKo = _reader["ExpEndKo"].ToString().ToNull(),
                ExpEndEn = _reader["ExpEndEn"].ToString().ToNull(),
                ExpEndJa = _reader["ExpEndJa"].ToString().ToNull(),
                ExpEndFr = _reader["ExpEndFr"].ToString().ToNull(),
                ExpEndZhcn = _reader["ExpEndZhcn"].ToString().ToNull(),
                ExpEndZhtw = _reader["ExpEndZhtw"].ToString().ToNull(),
                ZipCode = _reader["ZipCode"].ToString().ToNull(),
                AddressFirstBase = _reader["AddressFirstBase"].ToString().ToNull(),
                AddressFirstKo = _reader["AddressFirstKo"].ToString().ToNull(),
                AddressFirstEn = _reader["AddressFirstEn"].ToString().ToNull(),
                AddressFirstJa = _reader["AddressFirstJa"].ToString().ToNull(),
                AddressFirstFr = _reader["AddressFirstFr"].ToString().ToNull(),
                AddressFirstZhcn = _reader["AddressFirstZhcn"].ToString().ToNull(),
                AddressFirstZhtw = _reader["AddressFirstZhtw"].ToString().ToNull(),
                AddressEndBase = _reader["AddressEndBase"].ToString().ToNull(),
                AddressEndKo = _reader["AddressEndKo"].ToString().ToNull(),
                AddressEndEn = _reader["AddressEndEn"].ToString().ToNull(),
                AddressEndJa = _reader["AddressEndJa"].ToString().ToNull(),
                AddressEndFr = _reader["AddressEndFr"].ToString().ToNull(),
                AddressEndZhcn = _reader["AddressEndZhcn"].ToString().ToNull(),
                AddressEndZhtw = _reader["AddressEndZhtw"].ToString().ToNull(),
                PhoneBase = _reader["PhoneBase"].ToString().ToNull(),
                PhoneKo = _reader["PhoneKo"].ToString().ToNull(),
                PhoneEn = _reader["PhoneEn"].ToString().ToNull(),
                PhoneJa = _reader["PhoneJa"].ToString().ToNull(),
                PhoneFr = _reader["PhoneFr"].ToString().ToNull(),
                PhoneZhcn = _reader["PhoneZhcn"].ToString().ToNull(),
                PhoneZhtw = _reader["PhoneZhtw"].ToString().ToNull(),
                FaxBase = _reader["FaxBase"].ToString().ToNull(),
                FaxKo = _reader["FaxKo"].ToString().ToNull(),
                FaxEn = _reader["FaxEn"].ToString().ToNull(),
                FaxJa = _reader["FaxJa"].ToString().ToNull(),
                FaxFr = _reader["FaxFr"].ToString().ToNull(),
                FaxZhcn = _reader["FaxZhcn"].ToString().ToNull(),
                FaxZhtw = _reader["FaxZhtw"].ToString().ToNull(),
                Homepage = _reader["Homepage"].ToString().ToNull(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                WriteUserID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],

                #region ####  부서 다국어 시작 #######

                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull()

                #endregion ####  부서 다국어 시작 #######
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<EnforceStampInfo> GetEnforceStampList(IDataReader reader)
        {
            List<EnforceStampInfo> list = new List<EnforceStampInfo>();
            while (reader.Read())
                list.Add(GetEnforceStampInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 전체 시행문인장 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public List<BE.EnforceStampInfo> GetEnforceStamp(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_EnforceStampSel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetEnforceStampList(reader);
                }
            }
        }

        /// <summary>
        /// 특정 시행문인장 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public List<BE.EnforceStampInfo> GetEnforceStamp(int _companyID, int _enforceStampID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_EnforceStamp_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@EnforceStampID", SqlDbType.Int).Value = _enforceStampID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetEnforceStampList(reader);
                }
            }
        }

        /// <summary>
        /// 시행문인장 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue, string _order)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_EnforceStamp_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
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
        /// 시행문인장 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageCount(int _companyID,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue)
        {
            int rtnval = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_EnforceStamp_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                rtnval = ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
            return rtnval;
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 시행문인장 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.EnforceStampInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_EnforceStamp_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ExpFirstBase", SqlDbType.NVarChar, 100).Value = _info.ExpFirstBase.ToNull();
                    cmd.Parameters.Add("@ExpFirstKo", SqlDbType.NVarChar, 100).Value = _info.ExpFirstKo.ToNull();
                    cmd.Parameters.Add("@ExpFirstEn", SqlDbType.NVarChar, 100).Value = _info.ExpFirstEn.ToNull();
                    cmd.Parameters.Add("@ExpFirstJa", SqlDbType.NVarChar, 100).Value = _info.ExpFirstJa.ToNull();
                    cmd.Parameters.Add("@ExpFirstFr", SqlDbType.NVarChar, 100).Value = _info.ExpFirstFr.ToNull();
                    cmd.Parameters.Add("@ExpFirstZhcn", SqlDbType.NVarChar, 100).Value = _info.ExpFirstZhcn.ToNull();
                    cmd.Parameters.Add("@ExpFirstZhtw", SqlDbType.NVarChar, 100).Value = _info.ExpFirstZhtw.ToNull();
                    cmd.Parameters.Add("@ExpMidBase", SqlDbType.NVarChar, 100).Value = _info.ExpMidBase.ToNull();
                    cmd.Parameters.Add("@ExpMidKo", SqlDbType.NVarChar, 100).Value = _info.ExpMidKo.ToNull();
                    cmd.Parameters.Add("@ExpMidEn", SqlDbType.NVarChar, 100).Value = _info.ExpMidEn.ToNull();
                    cmd.Parameters.Add("@ExpMidJa", SqlDbType.NVarChar, 100).Value = _info.ExpMidJa.ToNull();
                    cmd.Parameters.Add("@ExpMidFr", SqlDbType.NVarChar, 100).Value = _info.ExpMidFr.ToNull();
                    cmd.Parameters.Add("@ExpMidZhcn", SqlDbType.NVarChar, 100).Value = _info.ExpMidZhcn.ToNull();
                    cmd.Parameters.Add("@ExpMidZhtw", SqlDbType.NVarChar, 100).Value = _info.ExpMidZhtw.ToNull();
                    cmd.Parameters.Add("@ExpEndBase", SqlDbType.NVarChar, 100).Value = _info.ExpEndBase.ToNull();
                    cmd.Parameters.Add("@ExpEndKo", SqlDbType.NVarChar, 100).Value = _info.ExpEndKo.ToNull();
                    cmd.Parameters.Add("@ExpEndEn", SqlDbType.NVarChar, 100).Value = _info.ExpEndEn.ToNull();
                    cmd.Parameters.Add("@ExpEndJa", SqlDbType.NVarChar, 100).Value = _info.ExpEndJa.ToNull();
                    cmd.Parameters.Add("@ExpEndFr", SqlDbType.NVarChar, 100).Value = _info.ExpEndFr.ToNull();
                    cmd.Parameters.Add("@ExpEndZhcn", SqlDbType.NVarChar, 100).Value = _info.ExpEndZhcn.ToNull();
                    cmd.Parameters.Add("@ExpEndZhtw", SqlDbType.NVarChar, 100).Value = _info.ExpEndZhtw.ToNull();
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 50).Value = _info.ZipCode.ToNull();
                    cmd.Parameters.Add("@AddressFirstBase", SqlDbType.NVarChar, 100).Value = _info.AddressFirstBase.ToNull();
                    cmd.Parameters.Add("@AddressFirstKo", SqlDbType.NVarChar, 100).Value = _info.AddressFirstKo.ToNull();
                    cmd.Parameters.Add("@AddressFirstEn", SqlDbType.NVarChar, 100).Value = _info.AddressFirstEn.ToNull();
                    cmd.Parameters.Add("@AddressFirstJa", SqlDbType.NVarChar, 100).Value = _info.AddressFirstJa.ToNull();
                    cmd.Parameters.Add("@AddressFirstFr", SqlDbType.NVarChar, 100).Value = _info.AddressFirstFr.ToNull();
                    cmd.Parameters.Add("@AddressFirstZhcn", SqlDbType.NVarChar, 100).Value = _info.AddressFirstZhcn.ToNull();
                    cmd.Parameters.Add("@AddressFirstZhtw", SqlDbType.NVarChar, 100).Value = _info.AddressFirstZhtw.ToNull();
                    cmd.Parameters.Add("@AddressEndBase", SqlDbType.NVarChar, 100).Value = _info.AddressEndBase.ToNull();
                    cmd.Parameters.Add("@AddressEndKo", SqlDbType.NVarChar, 100).Value = _info.AddressEndKo.ToNull();
                    cmd.Parameters.Add("@AddressEndEn", SqlDbType.NVarChar, 100).Value = _info.AddressEndEn.ToNull();
                    cmd.Parameters.Add("@AddressEndJa", SqlDbType.NVarChar, 100).Value = _info.AddressEndJa.ToNull();
                    cmd.Parameters.Add("@AddressEndFr", SqlDbType.NVarChar, 100).Value = _info.AddressEndFr.ToNull();
                    cmd.Parameters.Add("@AddressEndZhcn", SqlDbType.NVarChar, 100).Value = _info.AddressEndZhcn.ToNull();
                    cmd.Parameters.Add("@AddressEndZhtw", SqlDbType.NVarChar, 100).Value = _info.AddressEndZhtw.ToNull();
                    cmd.Parameters.Add("@PhoneBase", SqlDbType.VarChar, 20).Value = _info.PhoneBase.ToNull();
                    cmd.Parameters.Add("@PhoneKo", SqlDbType.VarChar, 20).Value = _info.PhoneKo.ToNull();
                    cmd.Parameters.Add("@PhoneEn", SqlDbType.VarChar, 20).Value = _info.PhoneEn.ToNull();
                    cmd.Parameters.Add("@PhoneJa", SqlDbType.VarChar, 20).Value = _info.PhoneJa.ToNull();
                    cmd.Parameters.Add("@PhoneFr", SqlDbType.VarChar, 20).Value = _info.PhoneFr.ToNull();
                    cmd.Parameters.Add("@PhoneZhcn", SqlDbType.VarChar, 20).Value = _info.PhoneZhcn.ToNull();
                    cmd.Parameters.Add("@PhoneZhtw", SqlDbType.VarChar, 20).Value = _info.PhoneZhtw.ToNull();
                    cmd.Parameters.Add("@FaxBase", SqlDbType.VarChar, 20).Value = _info.FaxBase.ToNull();
                    cmd.Parameters.Add("@FaxKo", SqlDbType.VarChar, 20).Value = _info.FaxKo.ToNull();
                    cmd.Parameters.Add("@FaxEn", SqlDbType.VarChar, 20).Value = _info.FaxEn.ToNull();
                    cmd.Parameters.Add("@FaxJa", SqlDbType.VarChar, 20).Value = _info.FaxJa.ToNull();
                    cmd.Parameters.Add("@FaxFr", SqlDbType.VarChar, 20).Value = _info.FaxFr.ToNull();
                    cmd.Parameters.Add("@FaxZhcn", SqlDbType.VarChar, 20).Value = _info.FaxZhcn.ToNull();
                    cmd.Parameters.Add("@FaxZhtw", SqlDbType.VarChar, 20).Value = _info.FaxZhtw.ToNull();
                    cmd.Parameters.Add("@Homepage", SqlDbType.NVarChar, 50).Value = _info.Homepage.ToNull();
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 100).Value = _info.FilePath.ToNull();
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 100).Value = _info.FileName.ToNull();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID.ToNullInt();

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
        /// 시행문인장 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(BE.EnforceStampInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_EnforceStamp_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@EnforceStampID", SqlDbType.Int).Value = _info.EnforceStampID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ExpFirstBase", SqlDbType.NVarChar, 100).Value = _info.ExpFirstBase.ToNull();
                    cmd.Parameters.Add("@ExpFirstKo", SqlDbType.NVarChar, 100).Value = _info.ExpFirstKo.ToNull();
                    cmd.Parameters.Add("@ExpFirstEn", SqlDbType.NVarChar, 100).Value = _info.ExpFirstEn.ToNull();
                    cmd.Parameters.Add("@ExpFirstJa", SqlDbType.NVarChar, 100).Value = _info.ExpFirstJa.ToNull();
                    cmd.Parameters.Add("@ExpFirstFr", SqlDbType.NVarChar, 100).Value = _info.ExpFirstFr.ToNull();
                    cmd.Parameters.Add("@ExpFirstZhcn", SqlDbType.NVarChar, 100).Value = _info.ExpFirstZhcn.ToNull();
                    cmd.Parameters.Add("@ExpFirstZhtw", SqlDbType.NVarChar, 100).Value = _info.ExpFirstZhtw.ToNull();
                    cmd.Parameters.Add("@ExpMidBase", SqlDbType.NVarChar, 100).Value = _info.ExpMidBase.ToNull();
                    cmd.Parameters.Add("@ExpMidKo", SqlDbType.NVarChar, 100).Value = _info.ExpMidKo.ToNull();
                    cmd.Parameters.Add("@ExpMidEn", SqlDbType.NVarChar, 100).Value = _info.ExpMidEn.ToNull();
                    cmd.Parameters.Add("@ExpMidJa", SqlDbType.NVarChar, 100).Value = _info.ExpMidJa.ToNull();
                    cmd.Parameters.Add("@ExpMidFr", SqlDbType.NVarChar, 100).Value = _info.ExpMidFr.ToNull();
                    cmd.Parameters.Add("@ExpMidZhcn", SqlDbType.NVarChar, 100).Value = _info.ExpMidZhcn.ToNull();
                    cmd.Parameters.Add("@ExpMidZhtw", SqlDbType.NVarChar, 100).Value = _info.ExpMidZhtw.ToNull();
                    cmd.Parameters.Add("@ExpEndBase", SqlDbType.NVarChar, 100).Value = _info.ExpEndBase.ToNull();
                    cmd.Parameters.Add("@ExpEndKo", SqlDbType.NVarChar, 100).Value = _info.ExpEndKo.ToNull();
                    cmd.Parameters.Add("@ExpEndEn", SqlDbType.NVarChar, 100).Value = _info.ExpEndEn.ToNull();
                    cmd.Parameters.Add("@ExpEndJa", SqlDbType.NVarChar, 100).Value = _info.ExpEndJa.ToNull();
                    cmd.Parameters.Add("@ExpEndFr", SqlDbType.NVarChar, 100).Value = _info.ExpEndFr.ToNull();
                    cmd.Parameters.Add("@ExpEndZhcn", SqlDbType.NVarChar, 100).Value = _info.ExpEndZhcn.ToNull();
                    cmd.Parameters.Add("@ExpEndZhtw", SqlDbType.NVarChar, 100).Value = _info.ExpEndZhtw.ToNull();
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 50).Value = _info.ZipCode.ToNull();
                    cmd.Parameters.Add("@AddressFirstBase", SqlDbType.NVarChar, 100).Value = _info.AddressFirstBase.ToNull();
                    cmd.Parameters.Add("@AddressFirstKo", SqlDbType.NVarChar, 100).Value = _info.AddressFirstKo.ToNull();
                    cmd.Parameters.Add("@AddressFirstEn", SqlDbType.NVarChar, 100).Value = _info.AddressFirstEn.ToNull();
                    cmd.Parameters.Add("@AddressFirstJa", SqlDbType.NVarChar, 100).Value = _info.AddressFirstJa.ToNull();
                    cmd.Parameters.Add("@AddressFirstFr", SqlDbType.NVarChar, 100).Value = _info.AddressFirstFr.ToNull();
                    cmd.Parameters.Add("@AddressFirstZhcn", SqlDbType.NVarChar, 100).Value = _info.AddressFirstZhcn.ToNull();
                    cmd.Parameters.Add("@AddressFirstZhtw", SqlDbType.NVarChar, 100).Value = _info.AddressFirstZhtw.ToNull();
                    cmd.Parameters.Add("@AddressEndBase", SqlDbType.NVarChar, 100).Value = _info.AddressEndBase.ToNull();
                    cmd.Parameters.Add("@AddressEndKo", SqlDbType.NVarChar, 100).Value = _info.AddressEndKo.ToNull();
                    cmd.Parameters.Add("@AddressEndEn", SqlDbType.NVarChar, 100).Value = _info.AddressEndEn.ToNull();
                    cmd.Parameters.Add("@AddressEndJa", SqlDbType.NVarChar, 100).Value = _info.AddressEndJa.ToNull();
                    cmd.Parameters.Add("@AddressEndFr", SqlDbType.NVarChar, 100).Value = _info.AddressEndFr.ToNull();
                    cmd.Parameters.Add("@AddressEndZhcn", SqlDbType.NVarChar, 100).Value = _info.AddressEndZhcn.ToNull();
                    cmd.Parameters.Add("@AddressEndZhtw", SqlDbType.NVarChar, 100).Value = _info.AddressEndZhtw.ToNull();
                    cmd.Parameters.Add("@PhoneBase", SqlDbType.VarChar, 20).Value = _info.PhoneBase.ToNull();
                    cmd.Parameters.Add("@PhoneKo", SqlDbType.VarChar, 20).Value = _info.PhoneKo.ToNull();
                    cmd.Parameters.Add("@PhoneEn", SqlDbType.VarChar, 20).Value = _info.PhoneEn.ToNull();
                    cmd.Parameters.Add("@PhoneJa", SqlDbType.VarChar, 20).Value = _info.PhoneJa.ToNull();
                    cmd.Parameters.Add("@PhoneFr", SqlDbType.VarChar, 20).Value = _info.PhoneFr.ToNull();
                    cmd.Parameters.Add("@PhoneZhcn", SqlDbType.VarChar, 20).Value = _info.PhoneZhcn.ToNull();
                    cmd.Parameters.Add("@PhoneZhtw", SqlDbType.VarChar, 20).Value = _info.PhoneZhtw.ToNull();
                    cmd.Parameters.Add("@FaxBase", SqlDbType.VarChar, 20).Value = _info.FaxBase.ToNull();
                    cmd.Parameters.Add("@FaxKo", SqlDbType.VarChar, 20).Value = _info.FaxKo.ToNull();
                    cmd.Parameters.Add("@FaxEn", SqlDbType.VarChar, 20).Value = _info.FaxEn.ToNull();
                    cmd.Parameters.Add("@FaxJa", SqlDbType.VarChar, 20).Value = _info.FaxJa.ToNull();
                    cmd.Parameters.Add("@FaxFr", SqlDbType.VarChar, 20).Value = _info.FaxFr.ToNull();
                    cmd.Parameters.Add("@FaxZhcn", SqlDbType.VarChar, 20).Value = _info.FaxZhcn.ToNull();
                    cmd.Parameters.Add("@FaxZhtw", SqlDbType.VarChar, 20).Value = _info.FaxZhtw.ToNull();
                    cmd.Parameters.Add("@Homepage", SqlDbType.NVarChar, 50).Value = _info.Homepage.ToNull();
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 100).Value = _info.FilePath.ToNull();
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 100).Value = _info.FileName.ToNull();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID.ToNullInt();

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
        /// 작성자 : famo23
        /// 작성일자 : 2013-12-30
        /// 작성목적 : 파일정보 수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_enforceStampID"></param>
        /// <returns></returns>
        public bool ModFile(int _companyID, int _enforceStampID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_EnforceStamp_UpdByFile", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@EnforceStampID", SqlDbType.Int).Value = _enforceStampID;

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
        /// 시행문인장 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_enforceStampID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _enforceStampID, int _deptID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_EnforceStamp_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@EnforceStampID", SqlDbType.Int).Value = _enforceStampID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

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