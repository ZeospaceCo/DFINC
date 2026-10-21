using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class SequenceBackUp : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.SequenceBackUpInfo GetSequenceBackUpInfo(IDataReader _reader)
        {
            return new SequenceBackUpInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                Sequence = (_reader["Sequence"] == DBNull.Value) ? -1 : _reader["Sequence"].ToString().ToNumber<int>(),
                ApprovalTag = _reader["ApprovalTag"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                MyTurn = _reader["MyTurn"].ToString().ToNull(),
                ApprovalDate = (_reader["ApprovalDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ApprovalDate"],
                ApprovalDateYMD = _reader["ApprovalDateYMD"].ToString().ToNull(),
                ApprovalType = _reader["ApprovalType"].ToString().ToNull(),
                Viewing = _reader["Viewing"].ToString().ToNull(),
                ViewingTime = (_reader["ViewingTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ViewingTime"],
                AutoReadLaterON = _reader["AutoReadLaterON"].ToString().ToNull(),
                Opinion = _reader["Opinion"].ToString().ToNull(),
                Proxy = _reader["Proxy"].ToString().ToNull(),
                ProxyUserID = (_reader["ProxyUserID"] == DBNull.Value) ? -1 : _reader["ProxyUserID"].ToString().ToNumber<int>(),
                ProxyNameBase = _reader["ProxyNameBase"].ToString().ToNull(),
                ProxyNameKo = _reader["ProxyNameKo"].ToString().ToNull(),
                ProxyNameEn = _reader["ProxyNameEn"].ToString().ToNull(),
                ProxyNameJa = _reader["ProxyNameJa"].ToString().ToNull(),
                ProxyNameFr = _reader["ProxyNameFr"].ToString().ToNull(),
                ProxyNameZhcn = _reader["ProxyNameZhcn"].ToString().ToNull(),
                ProxyNameZhtw = _reader["ProxyNameZhtw"].ToString().ToNull(),
                ProxyManyName = _reader["ProxyManyName"].ToString().ToNull(),
                ProxyDeptCodeID = (_reader["ProxyDeptCodeID"] == DBNull.Value) ? -1 : _reader["ProxyDeptCodeID"].ToString().ToNumber<int>(),
                ApprovalDateStr = _reader["ApprovalDate"].ToString().ToNull(),

                P_DeptBase = _reader["P_DeptBase"].ToString().ToNull(),
                P_DeptKo = _reader["P_DeptKo"].ToString().ToNull(),
                P_DeptEn = _reader["P_DeptEn"].ToString().ToNull(),
                P_DeptJa = _reader["P_DeptJa"].ToString().ToNull(),
                P_DeptFr = _reader["P_DeptFr"].ToString().ToNull(),
                P_DeptZhcn = _reader["P_DeptZhcn"].ToString().ToNull(),
                P_DeptZhtw = _reader["P_DeptZhtw"].ToString().ToNull(),

                P_TitleBase = _reader["P_TitleBase"].ToString().ToNull(),
                P_TitleKo = _reader["P_TitleKo"].ToString().ToNull(),
                P_TitleEn = _reader["P_TitleEn"].ToString().ToNull(),
                P_TitleJa = _reader["P_TitleJa"].ToString().ToNull(),
                P_TitleFr = _reader["P_TitleFr"].ToString().ToNull(),
                P_TitleZhcn = _reader["P_TitleZhcn"].ToString().ToNull(),
                P_TitleZhtw = _reader["P_TitleZhtw"].ToString().ToNull(),

                #region ####  부서,직급,직책 다국어 시작 #######

                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                TitleID = (_reader["TitleID"] == DBNull.Value) ? -1 : _reader["TitleID"].ToString().ToNumber<int>(),
                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                DutyID = (_reader["DutyID"] == DBNull.Value) ? -1 : _reader["DutyID"].ToString().ToNumber<int>(),
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

        /// <summary>
        /// Gets the sequence info list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<SequenceBackUpInfo> GetSequenceInfoList(IDataReader reader)
        {
            List<SequenceBackUpInfo> list = new List<SequenceBackUpInfo>();
            while (reader.Read())
                list.Add(GetSequenceBackUpInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-22
        /// 작성목적 : 결재자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public List<BE.SequenceBackUpInfo> GetApprovalUserInfo(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_SeqBackup_SelByUpPath", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSequenceInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   :
        /// 작성일자 : 2012-05-22
        /// 작성목적 : 결재자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public List<BE.SequenceBackUpInfo> GetSeqLine(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_SeqBackup_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSequenceInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 특정문서의 결재라인 정보 가져오기 (발신/수신/참조)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public DataTableReader GetSeqLineTot(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_SeqBackup_SelByLine", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.SequenceBackUp[GetSeqLineTot]", ex);
                throw;
            }
        }

        /// <summary>
        /// 특정문서의 부서정보가 변경된 결재라인 정보 가져오기 (발신/수신/참조)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public DataTableReader GetSeqModfyLineTot(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_SeqBackup_SelModfyByLine", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.SequenceBackUp[GetSeqModfyLineTot]", ex);
                throw;
            }
        }

        #endregion GET()

        #region MOD()
        /// <summary>
        /// [공람확인-완료]
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModConfirm(BE.SequenceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_SeqBackup_UpdConfirm", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 500).Value = _info.Opinion.ToNull();
                    #endregion

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
        /// [후열 문서확인-완료]
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModAfterwardConfirm(BE.SequenceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_SeqBackup_UpdAfterwardConfirm", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 500).Value = _info.Opinion.ToNull();
                    #endregion

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
        #endregion
    }
}