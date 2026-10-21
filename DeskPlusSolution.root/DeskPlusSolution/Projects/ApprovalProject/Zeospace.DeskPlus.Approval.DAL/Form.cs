using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class Form : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.FormInfo GetFormInfo(IDataReader _reader)
        {
            return new FormInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FormID = (_reader["FormID"] == DBNull.Value) ? -1 : _reader["FormID"].ToString().ToNumber<int>(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                FormBase = _reader["FormBase"].ToString().ToNull(),
                FormKo = _reader["FormKo"].ToString().ToNull(),
                FormEn = _reader["FormEn"].ToString().ToNull(),
                FormJa = _reader["FormJa"].ToString().ToNull(),
                FormFr = _reader["FormFr"].ToString().ToNull(),
                FormZhcn = _reader["FormZhcn"].ToString().ToNull(),
                FormZhtw = _reader["FormZhtw"].ToString().ToNull(),
                NameCode = _reader["NameCode"].ToString().ToNull(),
                ContentHTML = _reader["ContentHTML"].ToString().ToNull().ToEditorXSS(),
                IsLegacy = _reader["IsLegacy"].ToString().ToNull(),
                IsAfterSubmitCall = _reader["IsAfterSubmitCall"].ToString().ToNull(),
                CustomFileName = _reader["CustomFileName"].ToString().ToNull(),
                NickName = _reader["NickName"].ToString().ToNull(),
                IsLinked = _reader["IsLinked"].ToString().ToNull(),
                IsLinkedType = _reader["IsLinkedType"].ToString().ToNull(),
                IsLinkedEtc = _reader["IsLinkedEtc"].ToString().ToNull(),
                IsForce = _reader["IsForce"].ToString().ToNull(),
                IsBundle = _reader["IsBundle"].ToString().ToNull(),
                IsActive = _reader["IsActive"].ToString().ToNull(),
                IsMobile = _reader["IsMobile"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],

                /*Folder정보*/
                TypeInfo = _reader["TypeInfo"].ToString().ToNull(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                PathNameBase = _reader["PathNameBase"].ToString().ToNull(),
                PathNameKo = _reader["PathNameKo"].ToString().ToNull(),
                PathNameEn = _reader["PathNameEn"].ToString().ToNull(),
                PathNameJa = _reader["PathNameJa"].ToString().ToNull(),
                PathNameFr = _reader["PathNameFr"].ToString().ToNull(),
                PathNameZhcn = _reader["PathNameZhcn"].ToString().ToNull(),
                PathNameZhtw = _reader["PathNameZhtw"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<FormInfo> GetFormInfoList(IDataReader reader)
        {
            List<FormInfo> list = new List<FormInfo>();
            while (reader.Read())
                list.Add(GetFormInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        ///
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public List<BE.FormInfo> GetForm(int _companyID, int _formID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Form_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFormInfoList(reader);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public List<BE.FormInfo> GetForm(int _companyID, string _nickName)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Form_SelByNickNameInfo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@NickName", SqlDbType.NVarChar, 50).Value = _nickName;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFormInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 양식리스트 갯수 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_sPeriod"></param>
        /// <param name="_sPeriodVal"></param>
        /// <returns></returns>
        public int GetList(int _companyID, string _sKey, string _sVal, string _sStartDate, string _sEndDate)
        {
            int rtnCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Form_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _sStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _sEndDate;

                cn.Open();

                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }

        /// <summary>
        /// 양식리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_sPeriod"></param>
        /// <param name="_sPeriodVal"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _pageSize, int _pageNo, string _sKey, string _sVal, string _sStartDate, string _sEndDate, string _order)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Form_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _sStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _sEndDate;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 50).Value = _order;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Form[GetList]", ex);
                throw;
            }
        }

        /// <summary>
        /// 직접연동호출명-중복체크
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_nickName"></param>
        /// <returns></returns>
        public int GetNickName(int _companyID, string _nickName)
        {
            int rtnCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Form_SelByNickName", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@NickName", SqlDbType.NVarChar, 50).Value = _nickName;

                cn.Open();

                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }

        /// <summary>
        /// 특정폴더의 양식리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public List<BE.FormInfo> GetFormByFolderID(int _companyID, int _folderID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Form_SelByFolderID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFormInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 특정폴더내의 양식리스트 가져오기
        /// </summary>
        /// <param name="_tableName"></param>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public DataSet GetForm(string _tableName, int _companyID, int _folderID, bool _isActive)
        {
            string activeDept = (_isActive) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Form_SelByFolderID");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

                cn.Open();

                return ExecuteDataSet(cmd, _tableName);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-25
        /// 작성목적 : 결재망 양식 제외하고 모든 양식 트리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetTree(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Form_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetDisuseTree");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 양식추가
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <returns></returns>
        public int Add(FormInfo _formInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_formInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Form_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _formInfo.CompanyID;
                    cmd.Parameters.Add("@FormBase", SqlDbType.NVarChar, 100).Value = _formInfo.FormBase;
                    cmd.Parameters.Add("@FormKo", SqlDbType.NVarChar, 100).Value = _formInfo.FormKo.ToNull();
                    cmd.Parameters.Add("@FormEn", SqlDbType.NVarChar, 100).Value = _formInfo.FormEn.ToNull();
                    cmd.Parameters.Add("@FormJa", SqlDbType.NVarChar, 100).Value = _formInfo.FormJa.ToNull();
                    cmd.Parameters.Add("@FormFr", SqlDbType.NVarChar, 100).Value = _formInfo.FormFr.ToNull();
                    cmd.Parameters.Add("@FormZhcn", SqlDbType.NVarChar, 100).Value = _formInfo.FormZhcn.ToNull();
                    cmd.Parameters.Add("@FormZhtw", SqlDbType.NVarChar, 100).Value = _formInfo.FormZhtw.ToNull();
                    cmd.Parameters.Add("@NameCode", SqlDbType.NVarChar, 100).Value = _formInfo.NameCode.ToNull();
                    cmd.Parameters.Add("@ContentHTML", SqlDbType.NText).Value = _formInfo.ContentHTML.ToEditorXSS();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _formInfo.IsActive;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _formInfo.WriteUserID;

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
        /// 양식수정(내용)
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <returns></returns>
        public bool Mod(FormInfo _formInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_formInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Form_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _formInfo.CompanyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formInfo.FormID;
                    cmd.Parameters.Add("@ContentHTML", SqlDbType.NText).Value = _formInfo.ContentHTML.ToEditorXSS();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _formInfo.IsActive;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _formInfo.ModifyUserID;

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
        /// 양식설정정보 변경
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <returns></returns>
        public bool ModEtc(FormInfo _formInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_formInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Form_UpdEtc", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _formInfo.CompanyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formInfo.FormID;
                    cmd.Parameters.Add("@FormBase", SqlDbType.NVarChar, 100).Value = _formInfo.FormBase;
                    cmd.Parameters.Add("@FormKo", SqlDbType.NVarChar, 100).Value = _formInfo.FormKo;
                    cmd.Parameters.Add("@FormEn", SqlDbType.NVarChar, 100).Value = _formInfo.FormEn;
                    cmd.Parameters.Add("@FormJa", SqlDbType.NVarChar, 100).Value = _formInfo.FormJa;
                    cmd.Parameters.Add("@FormFr", SqlDbType.NVarChar, 100).Value = _formInfo.FormFr;
                    cmd.Parameters.Add("@FormZhcn", SqlDbType.NVarChar, 100).Value = _formInfo.FormZhcn;
                    cmd.Parameters.Add("@FormZhtw", SqlDbType.NVarChar, 100).Value = _formInfo.FormZhtw;
                    cmd.Parameters.Add("@NameCode", SqlDbType.NVarChar, 100).Value = _formInfo.NameCode;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _formInfo.FolderID;
                    cmd.Parameters.Add("@IsLegacy", SqlDbType.Char, 1).Value = _formInfo.IsLegacy;
                    cmd.Parameters.Add("@IsAfterSubmitCall", SqlDbType.Char, 1).Value = _formInfo.IsAfterSubmitCall;
                    cmd.Parameters.Add("@CustomFileName", SqlDbType.NVarChar, 50).Value = _formInfo.CustomFileName;
                    cmd.Parameters.Add("@NickName", SqlDbType.NVarChar, 50).Value = _formInfo.NickName;
                    cmd.Parameters.Add("@IsLinked", SqlDbType.Char, 1).Value = _formInfo.IsLinked;
                    cmd.Parameters.Add("@IsLinkedType", SqlDbType.Char, 1).Value = _formInfo.IsLinkedType;
                    cmd.Parameters.Add("@IsLinkedEtc", SqlDbType.NVarChar, 500).Value = _formInfo.IsLinkedEtc;
                    cmd.Parameters.Add("@IsForce", SqlDbType.Char, 1).Value = _formInfo.IsForce;
                    cmd.Parameters.Add("@IsBundle", SqlDbType.Char, 1).Value = _formInfo.IsBundle;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _formInfo.IsActive;
                    cmd.Parameters.Add("@IsMobile", SqlDbType.Char, 1).Value = _formInfo.IsMobile;                    
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _formInfo.ModifyUserID;

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
        /// 활성화/비활성화
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public bool UpdActive(int _companyID, int _formID, string _isActive)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Form_UpdIsActive", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _isActive;

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
        /// 양식순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, int _formID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Form_UpdOrder", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;
                    cmd.Parameters.Add("@FormOrder", SqlDbType.Int).Value = _sequence;

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
        /// 양식삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _formID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Form_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = _formID;

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