using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class History : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.HistoryInfo GetHistoryInfo(IDataReader _reader)
        {
            return new HistoryInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                HistoryID = (_reader["HistoryID"] == DBNull.Value) ? -1 : _reader["HistoryID"].ToString().ToNumber<int>(),
                ModifyDeptCodeID = (_reader["ModifyDeptCodeID"] == DBNull.Value) ? -1 : _reader["ModifyDeptCodeID"].ToString().ToNumber<int>(),
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyNameBase = _reader["ModifyNameBase"].ToString().ToNull(),
                ModifyNameKo = _reader["ModifyNameKo"].ToString().ToNull(),
                ModifyNameEn = _reader["ModifyNameEn"].ToString().ToNull(),
                ModifyNameJa = _reader["ModifyNameJa"].ToString().ToNull(),
                ModifyNameFr = _reader["ModifyNameFr"].ToString().ToNull(),
                ModifyNameZhcn = _reader["ModifyNameZhcn"].ToString().ToNull(),
                ModifyNameZhtw = _reader["ModifyNameZhtw"].ToString().ToNull(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],
                ModifyDateYMD = _reader["ModifyDateYMD"].ToString().ToNull(),
                DocNumber = _reader["DocNumber"].ToString().ToNull(),
                Number1 = _reader["Number1"].ToString().ToNull(),
                Number2 = _reader["Number2"].ToString().ToNull(),
                Number3 = _reader["Number3"].ToString().ToNull(),
                Number4 = (_reader["Number4"] == DBNull.Value) ? -1 : _reader["Number4"].ToString().ToNumber<int>(),
                WriteTime = (_reader["WriteTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteTime"],
                WriteTimeYMD = _reader["WriteTimeYMD"].ToString().ToNull(),
                EmployeeNumber = _reader["EmployeeNumber"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                LifeTime = _reader["LifeTime"].ToString().ToNull(),
                AccessTitleID = (_reader["AccessTitleID"] == DBNull.Value) ? -1 : _reader["AccessTitleID"].ToString().ToNumber<int>(),
                ApprovalState = _reader["ApprovalState"].ToString().ToNull(),
                AccountTag = _reader["AccountTag"].ToString().ToNull(),
                Subject = _reader["Subject"].ToString().ToNull(),
                ContentHTML = _reader["ContentHTML"].ToString().ToNull(),
                ContentTEXT = _reader["ContentTEXT"].ToString().ToNull(),
                CompleteDate = (_reader["CompleteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["CompleteDate"],
                CompleteDateYMD = _reader["CompleteDateYMD"].ToString().ToNull(),
                Viewing = _reader["Viewing"].ToString().ToNull(),
                IsConfirm = _reader["IsConfirm"].ToString().ToNull(),
                IsLegacy = _reader["IsLegacy"].ToString().ToNull(),
                CustomFileName = _reader["CustomFileName"].ToString().ToNull(),
                IsAfterSubmitCall = _reader["IsAfterSubmitCall"].ToString().ToNull(),
                IsDRM = _reader["IsDRM"].ToString().ToNull(),
                IsGuard = _reader["IsGuard"].ToString().ToNull(),
                IsLinkedEtc = _reader["IsLinkedEtc"].ToString().ToNull(),
                IsUseForce = _reader["IsUseForce"].ToString().ToNull(),
                IsUseFlow = _reader["IsUseFlow"].ToString().ToNull(),
                FormID = (_reader["FormID"] == DBNull.Value) ? -1 : _reader["FormID"].ToString().ToNumber<int>(),
                FormBase = _reader["FormBase"].ToString().ToNull(),
                FormKo = _reader["FormKo"].ToString().ToNull(),
                FormEn = _reader["FormEn"].ToString().ToNull(),
                FormJa = _reader["FormJa"].ToString().ToNull(),
                FormFr = _reader["FormFr"].ToString().ToNull(),
                FormZhcn = _reader["FormZhcn"].ToString().ToNull(),
                FormZhtw = _reader["FormZhtw"].ToString().ToNull(),
                FormMany = _reader["FormMany"].ToString().ToNull(),
                UpPathID = _reader["UpPathID"].ToString().ToNull(),
                GroupID = (_reader["GroupID"] == DBNull.Value) ? -1 : _reader["GroupID"].ToString().ToNumber<int>(),
                Etc1 = _reader["Etc1"].ToString().ToNull(),
                Etc2 = _reader["Etc2"].ToString().ToNull(),
                Etc3 = _reader["Etc3"].ToString().ToNull(),
                Etc4 = _reader["Etc4"].ToString().ToNull(),
                Etc5 = _reader["Etc5"].ToString().ToNull(),
                Etc6 = _reader["Etc6"].ToString().ToNull(),
                Etc7 = _reader["Etc7"].ToString().ToNull(),
                Etc8 = _reader["Etc8"].ToString().ToNull(),
                Etc9 = _reader["Etc9"].ToString().ToNull(),
                Etc10 = _reader["Etc10"].ToString().ToNull(),
                AttachFileInfo = _reader["AttachFileInfo"].ToString().ToNull(),
                BaseDocInfo = _reader["BaseDocInfo"].ToString().ToNull(),
                Opinion = _reader["Opinion"].ToString().ToNull(),

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
                DutyZhtw = _reader["DutyZhtw"].ToString().ToNull(),

                #endregion ####  부서,직급,직책 다국어 시작 #######
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<HistoryInfo> GetHistoryInfoList(IDataReader reader)
        {
            List<HistoryInfo> list = new List<HistoryInfo>();
            while (reader.Read())
                list.Add(GetHistoryInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2012-06-21
        /// 작성목적 : 수정내역 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_History_SelListByUpPath", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

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
        /// 작성자   : shimpark
        /// 작성일자 : 2012-06-21
        /// 작성목적 : 수정내역 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public int GetCount(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_History_SelGetCount");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

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
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 히스토리 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <param name="_historyID">The _history ID.</param>
        /// <returns></returns>
        public List<BE.HistoryInfo> GetHistoryID(int _companyID, int _documentID, int _historyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_History_SelByHistoryID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@HistoryID", SqlDbType.Int).Value = _historyID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetHistoryInfoList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(HistoryInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_History_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@AttachFileInfo", SqlDbType.NVarChar, 1000).Value = _info.AttachFileInfo.ToNull();
                    cmd.Parameters.Add("@BaseDocInfo", SqlDbType.NVarChar, 1000).Value = _info.BaseDocInfo.ToNull();
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 500).Value = _info.Opinion.ToNull();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyDeptID", SqlDbType.Int).Value = _info.ModifyDeptID;

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

        #endregion MOD()

        #region DEL()

        #endregion DEL()
    }
}