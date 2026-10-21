using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class Sequence : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        private string userID;
        private string[] rUserID;

        #region DataReader

        private BE.SequenceInfo GetSequenceInfo(IDataReader _reader)
        {
            return new SequenceInfo
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
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<SequenceInfo> GetSequenceInfoList(IDataReader reader)
        {
            List<SequenceInfo> list = new List<SequenceInfo>();
            while (reader.Read())
                list.Add(GetSequenceInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        #region Sequence Line

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-22
        /// 작성목적 : 결재자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public List<BE.SequenceInfo> GetSeqLine(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Seq_Sel");
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
        /// 작성자   : shine
        /// 작성일자 : 2012-05-22
        /// 작성목적 : 결재자 정보 가져오기(결재상황조회용 - 진행,완료 유니온)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public List<BE.SequenceInfo> GetSeqLineUnion(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelByUpPath", cn);
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
        /// 특정문서의 특정결재라인 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public List<BE.SequenceInfo> GetSeq(int _companyID, int _documentID, int _sequence, string _approvalTag)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Seq_SelApprovalTag");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag.ToNull();

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSequenceInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 미결재 병렬결재자 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public List<BE.SequenceInfo> GetZAUserList(int _companyID, int _documentID, int _sequence, string _approvalTag)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelZAUserList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSequenceInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 본인 외, 미결재 병렬결재자 수 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public int GetZAUserCnt(int _companyID, int _documentID, int _sequence, string _approvalTag)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelZAUserCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 본인결재이후, 미결재건수 (발신건수-수신건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public string GetNextCnt(int _companyID, int _documentID, int _groupID, int _sequence, string _approvalTag)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelByNextCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;

                cn.Open();

                return ExecuteScalar(cmd).ToString();
            }
        }

        /// <summary>
        /// 반려대상 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public List<BE.SequenceInfo> GetReject(int _companyID, int _documentID, int _groupID, string _isType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelByReject", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                cmd.Parameters.Add("@IsType", SqlDbType.VarChar, 20).Value = _isType.ToNull();

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
                    var cmd = new SqlCommand("DP_APP_Seq_SelByLine", cn);
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetSeqLineTot]", ex);
                throw;
            }
        }

        /// <summary>
        /// 특정문서의 부서정보가 변경된  결재라인 정보 가져오기 (발신/수신/참조)
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
                    var cmd = new SqlCommand("DP_APP_Seq_SelModfyByLine", cn);
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetSeqModfyLineTot]", ex);
                throw;
            }
        }

        /// <summary>
        /// 특정문서의 미결재라인 정보 가져오기 (발신/수신/참조)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public DataTableReader GetSeqLineTotNonApp(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelByLineNonApp", cn);
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetSeqLineTotNonApp]", ex);
                throw;
            }
        }

        /// <summary>
        /// 특정문서의 미결재라인 정보 가져오기 (발신/수신/참조)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public DataTableReader GetSeqModfyLineTotNonApp(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelModfyByLineNonApp", cn);
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetSeqLineTotNonApp]", ex);
                throw;
            }
        }

        #endregion Sequence Line

        #region Alarm

        /// <summary>
        /// 다음결재자 (@Sequence + Myturn='Y')
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmApprove(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmNext", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        info.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        info.ApprovalTag = dtr["ApprovalTag"].ToString().ToNull();
                        info.TargetUserID = info.UserID;
                        info.TargetDeptID = dtr["DeptID"].ToString().ToNumber<int>();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApprove]", ex);
                throw;
            }
        }

        /// <summary>
        /// 다음결재자 (@Sequence + Myturn='Y')
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmCancel(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmCancel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        info.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        info.ApprovalTag = dtr["ApprovalTag"].ToString().ToNull();
                        info.TargetUserID = info.UserID;
                        info.TargetDeptID = dtr["DeptID"].ToString().ToNumber<int>();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmCancel]", ex);
                throw;
            }
        }

        /// <summary>
        /// 다음결재자 (@Sequence)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmApproveC(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmApproveCNext", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        info.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        info.ApprovalTag = dtr["ApprovalTag"].ToString().ToNull();
                        info.TargetUserID = info.UserID;
                        info.TargetDeptID = dtr["DeptID"].ToString().ToNumber<int>();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApprove]", ex);
                throw;
            }
        }

        /// <summary>
        /// 다음결재자 (@Sequence + Myturn='Y') 모두에게
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmApproveAll(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmNextAll", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApproveAll]", ex);
                throw;
            }
        }

        /// <summary>
        /// 반려대상자
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmReject(int _companyID, int _documentID, int _groupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmReject", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        info.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        info.ApprovalTag = dtr["ApprovalTag"].ToString().ToNull();
                        info.TargetUserID = info.UserID;
                        info.TargetDeptID = dtr["DeptID"].ToString().ToNumber<int>();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmReject]", ex);
                throw;
            }
        }

        /// <summary>
        /// 반려대상자 - 결재라인 모두에게
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmRejectAll(int _companyID, int _documentID, int _groupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmRejectAll", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmReject]", ex);
                throw;
            }
        }

        /// <summary>
        /// 결재라인+참조+수신(ZP)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmComplete(int _companyID, int _documentID, int _groupID, string _accountTag, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmComplete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@AccountTag", SqlDbType.VarChar, 20).Value = _accountTag;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmComplete]", ex);
                throw;
            }
        }

        /// <summary>
        /// 의견첨언
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmOpinion(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmOpinion", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApprove]", ex);
                throw;
            }
        }

        /// <summary>
        /// 의견첨언 모두에게
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmOpinionAll(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmOpinionAll", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApproveAll]", ex);
                throw;
            }
        }

        /// <summary>
        /// 보류
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmApproveD(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmApproveD", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApprove]", ex);
                throw;
            }
        }

        /// <summary>
        /// 보류
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmApproveDC(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmApproveDC", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApprove]", ex);
                throw;
            }
        }

        /// <summary>
        /// 기결재자
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmApproving(int _companyID, int _documentID, int _groupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmApproving", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();
                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApproving]", ex);
                throw;
            }
        }

        /// <summary>
        /// 회람
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmCirculation(int _companyID, string _userList)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    List<BE.UserInfo> list = new List<UserInfo>();

                    if (_userList != "")
                    {
                        userID = _userList.Replace("M$", "");
                        rUserID = userID.Split(',');
                    }

                    for (int i = 0; i < rUserID.Length; i++)
                    {
                        var cmd = new SqlCommand("DP_APP_Seq_SelAlarmCirculation", cn);
                        cn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = rUserID[i].ToString().ToNumber<int>();

                        DataTableReader dtr = ExecuteDataTableReader(cmd);

                        while (dtr.Read())
                        {
                            BE.UserInfo info = new UserInfo();
                            info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Name = dtr["NameBase"].ToString().ToNull();

                            if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                            {
                                info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                            }
                            else
                            {
                                info.Mail = "";
                            }
                            info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                            // 개인알람정보추가 2017-04-14
                            info.PreferredLanguage = dtr["PreLang"].ToString();
                            info.PreLang = dtr["PreLang"].ToString();
                            info.AlarmApproval = dtr["AlarmApproval"].ToString();
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }

                            list.Add(info);
                        }
                        cn.Close();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApprove]", ex);
                throw;
            }
        }

        /// <summary>
        /// 전달
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmForward(int _companyID, string _userList)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    List<BE.UserInfo> list = new List<UserInfo>();

                    if (_userList != "")
                    {
                        userID = _userList.Replace("M$", "");
                        rUserID = userID.Split(',');
                    }

                    for (int i = 0; i < rUserID.Length; i++)
                    {
                        var cmd = new SqlCommand("DP_APP_Seq_SelAlarmForward", cn);
                        cn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = rUserID[i].ToString().ToNumber<int>();

                        DataTableReader dtr = ExecuteDataTableReader(cmd);

                        while (dtr.Read())
                        {
                            BE.UserInfo info = new UserInfo();
                            info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Name = dtr["NameBase"].ToString().ToNull();

                            if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                            {
                                info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                            }
                            else
                            {
                                info.Mail = "";
                            }
                            info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                            // 개인알람정보추가 2017-04-14
                            info.PreferredLanguage = dtr["PreLang"].ToString();
                            info.PreLang = dtr["PreLang"].ToString();
                            info.AlarmApproval = dtr["AlarmApproval"].ToString();
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }

                            list.Add(info);
                        }
                        cn.Close();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApprove]", ex);
                throw;
            }
        }

        /// <summary>
        /// 이관
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmPassed(int _companyID, string _userList)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    List<BE.UserInfo> list = new List<UserInfo>();

                    if (_userList != "")
                    {
                        userID = _userList.Replace("M$", "");
                        rUserID = userID.Split(',');
                    }

                    for (int i = 0; i < rUserID.Length; i++)
                    {
                        var cmd = new SqlCommand("DP_APP_Seq_SelAlarmPassed", cn);
                        cn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = rUserID[i].ToString().ToNumber<int>();

                        DataTableReader dtr = ExecuteDataTableReader(cmd);

                        while (dtr.Read())
                        {
                            BE.UserInfo info = new UserInfo();
                            info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Name = dtr["NameBase"].ToString().ToNull();

                            if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                            {
                                info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                            }
                            else
                            {
                                info.Mail = "";
                            }
                            info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                            // 개인알람정보추가 2017-04-14
                            info.PreferredLanguage = dtr["PreLang"].ToString();
                            info.PreLang = dtr["PreLang"].ToString();
                            info.AlarmApproval = dtr["AlarmApproval"].ToString();
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }

                            list.Add(info);
                        }
                        cn.Close();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApprove]", ex);
                throw;
            }
        }

        /// <summary>
        /// 담당자변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmCounterpartuser(int _companyID, string _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    List<BE.UserInfo> list = new List<UserInfo>();

                    var cmd = new SqlCommand("DP_APP_Counterpartuser_SelAlarm", cn);
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID.ToNumber<int>();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    while (dtr.Read())
                    {
                        BE.UserInfo info = new UserInfo();
                        info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.Name = dtr["NameBase"].ToString().ToNull();

                        if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                        {
                            info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                        }
                        else
                        {
                            info.Mail = "";
                        }
                        info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                        // 개인알람정보추가 2017-04-14
                        info.PreferredLanguage = dtr["PreLang"].ToString();
                        info.PreLang = dtr["PreLang"].ToString();
                        info.AlarmApproval = dtr["AlarmApproval"].ToString();
                        if (info.AlarmApproval.IndexOf("1-").Equals(0))
                        {
                            info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            info.AlarmMessenger = "0";
                            info.AlarmMail = "0";
                            info.AlarmSMS = "0";
                        }

                        list.Add(info);
                    }
                    cn.Close();

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAlarmCounterpartuser", ex);
                throw;
            }
        }

        /// <summary>
        /// 의견추가시 모두에게
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.UserInfo> GetAlarmOpinionAddAll(int _companyID, int _documentID, int _userID, string _accountTag)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_SelAlarmOpinionAddAll", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@AccountTag", SqlDbType.VarChar, 20).Value = _accountTag;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    List<BE.UserInfo> list = new List<UserInfo>();

                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UserInfo info = new UserInfo();
                            info.AlarmType = dtr["AlarmType"].ToString().ToNull();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Name = dtr["NameBase"].ToString().ToNull();

                            if (!dtr["MailID"].ToString().Equals("") && !dtr["MailDomain"].ToString().Equals(""))
                            {
                                info.Mail = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                            }
                            else
                            {
                                info.Mail = "";
                            }
                            info.MobilePhone = dtr["MobilePhone"].ToString().ToNull();

                            // 개인알람정보추가 2017-04-14
                            info.PreferredLanguage = dtr["PreLang"].ToString();
                            info.PreLang = dtr["PreLang"].ToString();
                            info.AlarmApproval = dtr["AlarmApproval"].ToString();
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }

                            list.Add(info);
                        }
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.Sequence[GetAlarmApproveAll]", ex);
                return null;
            }
        }
        #endregion Alarm

        #region ModViewing

        /// <summary>
        /// 문서열람여부 확인 (결재Process관련)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public BE.SequenceInfo GetViewing(int _companyID, int _documentID, int _sequence, string _approvalTag)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelViewing", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                #region 파라미터 처리

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;

                #endregion 파라미터 처리

                cn.Open();

                BE.SequenceInfo info = new SequenceInfo();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        #region BE객체처리

                        info.CompanyID = (reader["CompanyID"] == DBNull.Value) ? -1 : reader["CompanyID"].ToString().ToNumber<int>();
                        info.DocumentID = (reader["DocumentID"] == DBNull.Value) ? -1 : reader["DocumentID"].ToString().ToNumber<int>();
                        info.Sequence = (reader["Sequence"] == DBNull.Value) ? -1 : reader["Sequence"].ToString().ToNumber<int>();
                        info.ApprovalTag = reader["ApprovalTag"].ToString().ToNull();
                        info.UserID = (reader["UserID"] == DBNull.Value) ? -1 : reader["UserID"].ToString().ToNumber<int>();
                        info.NameBase = reader["NameBase"].ToString().ToNull();
                        info.NameKo = reader["NameKo"].ToString().ToNull();
                        info.NameEn = reader["NameEn"].ToString().ToNull();
                        info.NameJa = reader["NameJa"].ToString().ToNull();
                        info.NameFr = reader["NameFr"].ToString().ToNull();
                        info.NameZhcn = reader["NameZhcn"].ToString().ToNull();
                        info.NameZhtw = reader["NameZhtw"].ToString().ToNull();
                        info.ManyName = reader["ManyName"].ToString().ToNull();
                        info.DeptCodeID = (reader["DeptCodeID"] == DBNull.Value) ? -1 : reader["DeptCodeID"].ToString().ToNumber<int>();
                        info.MyTurn = reader["MyTurn"].ToString().ToNull();
                        info.Viewing = reader["Viewing"].ToString().ToNull();
                        info.ViewingTime = (reader["ViewingTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)reader["ViewingTime"];
                        info.DeptID = (reader["DeptID"] == DBNull.Value) ? -1 : reader["DeptID"].ToString().ToNumber<int>();
                        info.DeptBase = reader["DeptBase"].ToString().ToNull();
                        info.DeptKo = reader["DeptKo"].ToString().ToNull();
                        info.DeptEn = reader["DeptEn"].ToString().ToNull();
                        info.DeptJa = reader["DeptJa"].ToString().ToNull();
                        info.DeptFr = reader["DeptFr"].ToString().ToNull();
                        info.DeptZhcn = reader["DeptZhcn"].ToString().ToNull();
                        info.DeptZhtw = reader["DeptZhtw"].ToString().ToNull();
                        info.TitleID = (reader["TitleID"] == DBNull.Value) ? -1 : reader["TitleID"].ToString().ToNumber<int>();
                        info.TitleBase = reader["TitleBase"].ToString().ToNull();
                        info.TitleKo = reader["TitleKo"].ToString().ToNull();
                        info.TitleEn = reader["TitleEn"].ToString().ToNull();
                        info.TitleJa = reader["TitleJa"].ToString().ToNull();
                        info.TitleFr = reader["TitleFr"].ToString().ToNull();
                        info.TitleZhcn = reader["TitleZhcn"].ToString().ToNull();
                        info.TitleZhtw = reader["TitleZhtw"].ToString().ToNull();
                        info.DutyID = (reader["DutyID"] == DBNull.Value) ? -1 : reader["DutyID"].ToString().ToNumber<int>();
                        info.DutyBase = reader["DutyBase"].ToString().ToNull();
                        info.DutyKo = reader["DutyKo"].ToString().ToNull();
                        info.DutyEn = reader["DutyEn"].ToString().ToNull();
                        info.DutyJa = reader["DutyJa"].ToString().ToNull();
                        info.DutyFr = reader["DutyFr"].ToString().ToNull();
                        info.DutyZhcn = reader["DutyZhcn"].ToString().ToNull();
                        info.DutyZhtw = reader["DutyZhtw"].ToString().ToNull();

                        info.DocViewing = reader["DocViewing"].ToString().ToNull();
                        info.DocViewing = (info.DocViewing.Equals("")) ? "N" : info.DocViewing;

                        #endregion BE객체처리
                    }
                }
                return info;
            }
        }

        /// <summary>
        /// 문서열람여부 확인 (그외. 첨언)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public BE.SequenceInfo GetViewing(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelViewingEtc", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                #region 파라미터 처리

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                #endregion 파라미터 처리

                cn.Open();

                BE.SequenceInfo info = new SequenceInfo();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        #region BE객체처리

                        info.DocViewing = reader["DocViewing"].ToString().ToNull();
                        info.DocViewing = (info.DocViewing.Equals("")) ? "N" : info.DocViewing;

                        if (info.DocViewing.Equals("Y"))
                        {
                            info.CompanyID = (reader["CompanyID"] == DBNull.Value) ? -1 : reader["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = (reader["DocumentID"] == DBNull.Value) ? -1 : reader["DocumentID"].ToString().ToNumber<int>();
                            info.Sequence = (reader["Sequence"] == DBNull.Value) ? -1 : reader["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = reader["ApprovalTag"].ToString().ToNull();
                            info.UserID = (reader["UserID"] == DBNull.Value) ? -1 : reader["UserID"].ToString().ToNumber<int>();
                            info.NameBase = reader["NameBase"].ToString().ToNull();
                            info.NameKo = reader["NameKo"].ToString().ToNull();
                            info.NameEn = reader["NameEn"].ToString().ToNull();
                            info.NameJa = reader["NameJa"].ToString().ToNull();
                            info.NameFr = reader["NameFr"].ToString().ToNull();
                            info.NameZhcn = reader["NameZhcn"].ToString().ToNull();
                            info.NameZhtw = reader["NameZhtw"].ToString().ToNull();
                            info.ManyName = reader["ManyName"].ToString().ToNull();
                            info.DeptCodeID = (reader["DeptCodeID"] == DBNull.Value) ? -1 : reader["DeptCodeID"].ToString().ToNumber<int>();
                            info.MyTurn = reader["MyTurn"].ToString().ToNull();
                            info.Viewing = reader["Viewing"].ToString().ToNull();
                            info.ViewingTime = (reader["ViewingTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)reader["ViewingTime"];
                            info.DeptID = (reader["DeptID"] == DBNull.Value) ? -1 : reader["DeptID"].ToString().ToNumber<int>();
                            info.DeptBase = reader["DeptBase"].ToString().ToNull();
                            info.DeptKo = reader["DeptKo"].ToString().ToNull();
                            info.DeptEn = reader["DeptEn"].ToString().ToNull();
                            info.DeptJa = reader["DeptJa"].ToString().ToNull();
                            info.DeptFr = reader["DeptFr"].ToString().ToNull();
                            info.DeptZhcn = reader["DeptZhcn"].ToString().ToNull();
                            info.DeptZhtw = reader["DeptZhtw"].ToString().ToNull();
                            info.TitleID = (reader["TitleID"] == DBNull.Value) ? -1 : reader["TitleID"].ToString().ToNumber<int>();
                            info.TitleBase = reader["TitleBase"].ToString().ToNull();
                            info.TitleKo = reader["TitleKo"].ToString().ToNull();
                            info.TitleEn = reader["TitleEn"].ToString().ToNull();
                            info.TitleJa = reader["TitleJa"].ToString().ToNull();
                            info.TitleFr = reader["TitleFr"].ToString().ToNull();
                            info.TitleZhcn = reader["TitleZhcn"].ToString().ToNull();
                            info.TitleZhtw = reader["TitleZhtw"].ToString().ToNull();
                            info.DutyID = (reader["DutyID"] == DBNull.Value) ? -1 : reader["DutyID"].ToString().ToNumber<int>();
                            info.DutyBase = reader["DutyBase"].ToString().ToNull();
                            info.DutyKo = reader["DutyKo"].ToString().ToNull();
                            info.DutyEn = reader["DutyEn"].ToString().ToNull();
                            info.DutyJa = reader["DutyJa"].ToString().ToNull();
                            info.DutyFr = reader["DutyFr"].ToString().ToNull();
                            info.DutyZhcn = reader["DutyZhcn"].ToString().ToNull();
                            info.DutyZhtw = reader["DutyZhtw"].ToString().ToNull();
                        }
                        else
                        {
                            info.UserID = 0;
                        }

                        #endregion BE객체처리
                    }
                }
                return info;
            }
        }

        public bool ModViewing(int _companyID, int _documentID, int _sequence, string _approvalTag, string _viewing)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_UpdViewing", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;
                    cmd.Parameters.Add("@Viewing", SqlDbType.Char, 1).Value = _viewing;

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
        /// 관리자 - 강제 Viewing초기화
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool ModViewingForce(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_UpdViewingForce", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

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
        #endregion ModViewing

        /// <summary>
        /// 현재 결재여부 확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public string GetIsMyTurn(int _companyID, int _documentID, int _sequence, string _approvalTag, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelMyTurn", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteScalar(cmd).ToString();
            }
        }

        /// <summary>
        /// 현재 결재여부 확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public string GetIsApprove(int _companyID, int _documentID, int _sequence, string _approvalTag, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Seq_SelIsApprove", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteScalar(cmd).ToString();
            }
        }

        #region ZACount

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-28
        /// 작성목적 : 이전 결재 ZA 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public int GetZACount(int _companyID, int _documentID, int _sequence)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Seq_SelZA");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        #endregion ZACount

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.SequenceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@Myturn", SqlDbType.Char, 1).Value = _info.MyTurn.ToNull();
                    cmd.Parameters.Add("@ApprovalType", SqlDbType.Char, 1).Value = _info.ApprovalType.ToNull();
                    cmd.Parameters.Add("@AutoReadLaterON", SqlDbType.Char, 1).Value = _info.AutoReadLaterON.ToNull();
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 500).Value = _info.Opinion.ToNull();

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

        #endregion ADD()

        #region MOD()

        #region 결재상황조회 결재취소 CancelApproval()

        // sp도 지울것.

        ///// <summary>
        ///// 결재상황조회 결재취소 CancelApproval()
        ///// </summary>
        ///// <param name="_info"></param>
        ///// <returns></returns>
        //public bool CancelApproval1(int _companyID, int _documentID, int _sequence, string _approvalTag)
        //{
        //    try
        //    {
        //        using (var cn = new SqlConnection(ConnSolution(_companyID)))
        //        {
        //            var cmd = new SqlCommand("DP_APP_Seq_UpCancelApproval", cn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

        //            #region 파라미터 처리
        //            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
        //            cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
        //            cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
        //            cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag.ToNull();
        //            #endregion

        //            cn.Open();
        //            ExecuteNonQuery(cmd);

        //            return true;
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
        //        Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
        //        return false;
        //    }
        //}

        #endregion 결재상황조회 결재취소 CancelApproval()

        /// <summary>
        /// 승인처리
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_isNextUpd"></param>
        /// <returns></returns>
        public bool ModDecide(BE.SequenceInfo _info, string _isNextUpd)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_UpdDecide", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ApprovalType", SqlDbType.Char, 1).Value = _info.ApprovalType.ToNull();
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 1000).Value = _info.Opinion.ToNull();
                    cmd.Parameters.Add("@Proxy", SqlDbType.Char, 1).Value = _info.Proxy.ToNull();
                    cmd.Parameters.Add("@ProxyUserID", SqlDbType.Int).Value = _info.ProxyUserID;
                    cmd.Parameters.Add("@IsNextUpd", SqlDbType.Char, 1).Value = _isNextUpd.ToNull();

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
        /// 결재순서,결재상태 업데이트 (반려,보류,상신보류 ...)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_myTurn"></param>
        /// <param name="_approvalType"></param>
        /// <returns></returns>
        public bool ModMyTurnState(int _companyID, int _documentID, int _sequence, string _approvalTag, string _myTurn, string _approvalType)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_UpdMyTurnState", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;
                    cmd.Parameters.Add("@Myturn", SqlDbType.Char, 1).Value = _myTurn;
                    cmd.Parameters.Add("@ApprovalType", SqlDbType.Char, 1).Value = _approvalType;

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
        /// 결재취소
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_opinion"></param>
        /// <returns></returns>
        public bool ModCancel(int _companyID, int _documentID, int _sequence, string _approvalTag, string _opinion)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_UpdCancel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag.ToNull();
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 500).Value = _opinion.ToNull();

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
        /// 결재취소 - 관계있는문서
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModCancelRelated(int _companyID, int _documentID, int _sequence, string _approvalTag
            , string _nextMyturn, string _nextApprovalTag, int _nextSequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_UpdCancelApproval", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag.ToNull();
                    cmd.Parameters.Add("@NextMyturn", SqlDbType.Char, 1).Value = _nextMyturn.ToNull();
                    cmd.Parameters.Add("@NextApprovalTag", SqlDbType.VarChar, 20).Value = _nextApprovalTag.ToNull();
                    cmd.Parameters.Add("@NextSequence", SqlDbType.Int).Value = _nextSequence;
                    #endregion

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
        /// [공람확인-진행중]
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModConfirm(BE.SequenceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Seq_UpdConfirm", cn);
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
        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 본문결재선수정 (삭제로직) : 미결재자 삭제 (본문결재선수정)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool DelSeqEtcs(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_SeqEtc_DelByDocID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

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
        /// 본문결재선수정 (삭제로직) : 미결재자 삭제 (본문결재선수정-수신)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool DelSeqEtcs(int _companyID, int _documentID, int _groupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_SeqEtc_DelByGroupID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;

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