using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class Company : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET

        /// <summary>
        /// Gets the company.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private BE.CompanyInfo GetCompany(IDataReader _reader)
        {
            return new CompanyInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CompanyCode = _reader["CompanyCode"].ToString().ToNull(),
                InterfaceCode = _reader["InterfaceCode"].ToString().ToNull(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                EstablishDate = _reader["EstablishDate"].ToString().ToNull(),
                InstitutorName = _reader["InstitutorName"].ToString().ToNull(),
                InstitutorEnglishName = _reader["InstitutorEnglishName"].ToString().ToNull(),
                InstitutorRegistrationNumber = _reader["InstitutorRegistrationNumber"].ToString().ToNull(),
                IndustryClass = _reader["IndustryClass"].ToString().ToNull(),
                BusinessCondition = _reader["BusinessCondition"].ToString().ToNull(),
                HomePage = _reader["HomePage"].ToString().ToNull(),
                ZipCode = _reader["ZipCode"].ToString().ToNull(),
                Address1 = _reader["Address1"].ToString().ToNull(),
                Address2 = _reader["Address2"].ToString().ToNull(),
                EnglishAddress = _reader["EnglishAddress"].ToString().ToNull(),
                Phone = _reader["Phone"].ToString().ToNull(),
                Fax = _reader["Fax"].ToString().ToNull(),
                CompanyQuota = (_reader["CompanyQuota"] == DBNull.Value) ? -1 : _reader["CompanyQuota"].ToString().ToNumber<int>(),
                ActiveCompany = _reader["ActiveCompany"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],
                LicenseCnt = (_reader["LicenseCnt"] == DBNull.Value) ? 0 : _reader["LicenseCnt"].ToString().ToNumber<int>(),
                Slogan = _reader["Slogan"].ToString().ToNull(),
                Lgn_logo = _reader["Lgn_logo"].ToString().ToNull(),
                Vlgn_logo = _reader["Vlgn_logo"].ToString().ToNull(),
                Logo = _reader["Logo"].ToString().ToNull(),
                Alogo = _reader["Alogo"].ToString().ToNull(),
                Bg = _reader["Bg"].ToString().ToNull(),
                Mainback = _reader["Mainback"].ToString().ToNull(),
            };
        }

        /// <summary>
        /// Gets the company list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected List<CompanyInfo> GetCompanyList(IDataReader reader)
        {
            List<CompanyInfo> list = new List<CompanyInfo>();
            while (reader.Read())
                list.Add(GetCompany(reader));
            return list;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사정보 가져오기
        /// </summary>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public List<BE.CompanyInfo> GetCompany(bool _active)
        {
            string active = (_active) ? "'1'" : "'0','1'";
            using (SqlConnection cn = new SqlConnection(ConnSolution()))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Company_Sel");
                query = string.Format(query, active);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCompanyList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public List<BE.CompanyInfo> GetCompany(int _companyID, bool _active)
        {
            string active = (_active) ? "'1'" : "'0','1'";
            using (SqlConnection cn = new SqlConnection(ConnSolution()))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Company_SelByID");
                query = string.Format(query, active);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCompanyList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 회사 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeCompany">if set to <c>true</c> [_active company].</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetCompanyPageList(int _companyID, bool _activeCompany,
            string _pageSize, string _pageSizePageNo, string _order)
        {
            string activeCompany = (_activeCompany) ? "1" : "0";

            using (SqlConnection cn = new SqlConnection(ConnSolution()))
            {
                SqlCommand cmd = new SqlCommand("DP_COM_Company_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ActiveCompany", SqlDbType.Char, 1).Value = activeCompany;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GET

        #region ADD

        /// <summary>
        /// 수정하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BE.CompanyInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution()))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Company_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 20).Value = _info.CompanyCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@InterfaceCode", SqlDbType.VarChar, 100).Value = _info.InterfaceCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 30).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@EstablishDate", SqlDbType.VarChar, 20).Value = _info.EstablishDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@InstitutorName", SqlDbType.NVarChar, 20).Value = _info.InstitutorName.ToNull().ToXSS();
                    cmd.Parameters.Add("@InstitutorEnglishName", SqlDbType.NVarChar, 300).Value = _info.InstitutorEnglishName.ToNull().ToXSS();
                    cmd.Parameters.Add("@InstitutorRegistrationNumber", SqlDbType.VarChar, 15).Value = _info.InstitutorRegistrationNumber.ToNull().ToXSS();
                    cmd.Parameters.Add("@IndustryClass", SqlDbType.NVarChar, 50).Value = _info.IndustryClass.ToNull().ToXSS();
                    cmd.Parameters.Add("@BusinessCondition", SqlDbType.NVarChar, 50).Value = _info.BusinessCondition.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomePage", SqlDbType.NVarChar, 100).Value = _info.HomePage.ToNull().ToXSS();
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 50).Value = _info.ZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@Address1", SqlDbType.NVarChar, 100).Value = _info.Address1.ToNull().ToXSS();
                    cmd.Parameters.Add("@Address2", SqlDbType.NVarChar, 100).Value = _info.Address2.ToNull().ToXSS();
                    cmd.Parameters.Add("@EnglishAddress", SqlDbType.VarChar, 100).Value = _info.EnglishAddress.ToNull().ToXSS();
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = _info.Phone.ToNull().ToXSS();
                    cmd.Parameters.Add("@Fax", SqlDbType.NVarChar, 20).Value = _info.Fax.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyQuota", SqlDbType.Int).Value = _info.CompanyQuota;
                    cmd.Parameters.Add("@ActiveCompany", SqlDbType.Char, 1).Value = _info.ActiveCompany.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@DefaultValue", SqlDbType.Int).Value = _info.DefaultValue;
                    cmd.Parameters.Add("@LicenseCnt", SqlDbType.Int).Value = _info.LicenseCnt;
                    cmd.Parameters.Add("@Slogan", SqlDbType.NText).Value = _info.Slogan.ToXSS();
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

        #endregion ADD

        #region MOD

        /// <summary>
        /// 수정하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(BE.CompanyInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution()))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Company_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 20).Value = _info.CompanyCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@InterfaceCode", SqlDbType.VarChar, 100).Value = _info.InterfaceCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 30).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@EstablishDate", SqlDbType.VarChar, 20).Value = _info.EstablishDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@InstitutorName", SqlDbType.NVarChar, 20).Value = _info.InstitutorName.ToNull().ToXSS();
                    cmd.Parameters.Add("@InstitutorEnglishName", SqlDbType.NVarChar, 300).Value = _info.InstitutorEnglishName.ToNull().ToXSS();
                    cmd.Parameters.Add("@InstitutorRegistrationNumber", SqlDbType.VarChar, 15).Value = _info.InstitutorRegistrationNumber.ToNull().ToXSS();
                    cmd.Parameters.Add("@IndustryClass", SqlDbType.NVarChar, 50).Value = _info.IndustryClass.ToNull().ToXSS();
                    cmd.Parameters.Add("@BusinessCondition", SqlDbType.NVarChar, 50).Value = _info.BusinessCondition.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomePage", SqlDbType.NVarChar, 100).Value = _info.HomePage.ToNull().ToXSS();
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 50).Value = _info.ZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@Address1", SqlDbType.NVarChar, 100).Value = _info.Address1.ToNull().ToXSS();
                    cmd.Parameters.Add("@Address2", SqlDbType.NVarChar, 100).Value = _info.Address2.ToNull().ToXSS();
                    cmd.Parameters.Add("@EnglishAddress", SqlDbType.VarChar, 100).Value = _info.EnglishAddress.ToNull().ToXSS();
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = _info.Phone.ToNull().ToXSS();
                    cmd.Parameters.Add("@Fax", SqlDbType.NVarChar, 20).Value = _info.Fax.ToNull().ToXSS();
                    cmd.Parameters.Add("@CompanyQuota", SqlDbType.Int).Value = _info.CompanyQuota;
                    cmd.Parameters.Add("@ActiveCompany", SqlDbType.Char, 1).Value = _info.ActiveCompany.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@LicenseCnt", SqlDbType.Int).Value = _info.LicenseCnt;
                    cmd.Parameters.Add("@Slogan", SqlDbType.NText).Value = _info.Slogan.ToXSS();

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
        /// 목적 : 인사 활성화
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public bool ModActive(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    var cmd = new SqlCommand("DP_COM_Company_UpdActive", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
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
        /// 이미지 버전 등록 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, string _gubun)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution()))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Company_UpdImage", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                   

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

        #endregion MOD

        #region DEL

        /// <summary>
        /// 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution()))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Company_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

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

        #endregion DEL
    }
}