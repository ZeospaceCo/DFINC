using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Data
{
    public class Sequence
    {
        #region GET()

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
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@Sequence", SqlDbType.Int)
                                            ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar,20)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = _sequence;
                parm[3].Value = _approvalTag;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Seq_SelZAUserCnt", parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetZAUserCnt: " + errMsg);
                throw;
            }
        }

        #region Alarm

        /// <summary>
        /// 다음결재자 (@Sequence + Myturn='Y')
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.AccountInfo> GetAlarmApprove(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@DocumentID", SqlDbType.Int),
                                               new SqlParameter("@GroupID", SqlDbType.Int),
                                               new SqlParameter("@Sequence", SqlDbType.Int),
										   };
                parms[0].Value = _companyID;
                parms[1].Value = _documentID;
                parms[2].Value = _groupID;
                parms[3].Value = _sequence;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Seq_SelAlarmNext", "dsAttach", parms);

                List<BE.AccountInfo> list = new List<BE.AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new BE.AccountInfo();

                    info.AlarmType = dr["AlarmType"].ToString().ToNull();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.Name = dr["NameBase"].ToString().ToNull();

                    if (!dr["MailID"].ToString().Equals("") && !dr["MailDomain"].ToString().Equals(""))
                    {
                        info.Mail = dr["MailID"].ToString() + "@" + dr["MailDomain"].ToString();
                    }
                    else
                    {
                        info.Mail = "";
                    }
                    info.MobilePhone = dr["MobilePhone"].ToString().ToNull();

                    info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                    info.ApprovalTag = dr["ApprovalTag"].ToString().ToNull();
                    info.TargetUserID = info.UserID;
                    info.TargetDeptID = dr["DeptID"].ToString().ToNumber<int>();

                    // 개인알람정보추가 2017-04-14
                    info.PreferredLanguage = dr["PreLang"].ToString();
                    info.PreLang = dr["PreLang"].ToString();
                    info.AlarmApproval = dr["AlarmApproval"].ToString();
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
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAlarmApprove: " + errMsg);
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
        public List<BE.AccountInfo> GetAlarmCancel(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@DocumentID", SqlDbType.Int),
                                               new SqlParameter("@GroupID", SqlDbType.Int),
                                               new SqlParameter("@Sequence", SqlDbType.Int),
										   };
                parms[0].Value = _companyID;
                parms[1].Value = _documentID;
                parms[2].Value = _groupID;
                parms[3].Value = _sequence;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Seq_SelAlarmCancel", "dsCancel", parms);

                List<BE.AccountInfo> list = new List<BE.AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new BE.AccountInfo();

                    info.AlarmType = dr["AlarmType"].ToString().ToNull();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.Name = dr["NameBase"].ToString().ToNull();

                    if (!dr["MailID"].ToString().Equals("") && !dr["MailDomain"].ToString().Equals(""))
                    {
                        info.Mail = dr["MailID"].ToString() + "@" + dr["MailDomain"].ToString();
                    }
                    else
                    {
                        info.Mail = "";
                    }
                    info.MobilePhone = dr["MobilePhone"].ToString().ToNull();

                    info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                    info.ApprovalTag = dr["ApprovalTag"].ToString().ToNull();
                    info.TargetUserID = info.UserID;
                    info.TargetDeptID = dr["DeptID"].ToString().ToNumber<int>();

                    // 개인알람정보추가 2017-04-14
                    info.PreferredLanguage = dr["PreLang"].ToString();
                    info.PreLang = dr["PreLang"].ToString();
                    info.AlarmApproval = dr["AlarmApproval"].ToString();
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
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAlarmCancel: " + errMsg);
                throw;
            }
        }

        #endregion

        #endregion

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
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@Sequence", SqlDbType.Int),
                                    new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20),
                                    new SqlParameter("@UserID", SqlDbType.Int),
                                    new SqlParameter("@DeptID", SqlDbType.Int),
                                    new SqlParameter("@Myturn", SqlDbType.Char,1),
                                    new SqlParameter("@ApprovalType", SqlDbType.Char,1),
                                    new SqlParameter("@AutoReadLaterON", SqlDbType.Char,1),
                                    new SqlParameter("@Opinion", SqlDbType.NVarChar,500)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DocumentID;
                parms[2].Value = _info.Sequence;
                parms[3].Value = _info.ApprovalTag.ToNull();
                parms[4].Value = _info.UserID;
                parms[5].Value = _info.DeptID;
                parms[6].Value = _info.MyTurn.ToNull();
                parms[7].Value = _info.ApprovalType.ToNull();
                parms[8].Value = _info.AutoReadLaterON.ToNull();
                parms[9].Value = _info.Opinion.ToNull();

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Seq_Ins", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 승인처리
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModDecide(BE.SequenceInfo _info, string _isNextUpd)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@Sequence", SqlDbType.Int),
                                    new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20),
                                    new SqlParameter("@UserID", SqlDbType.Int),
                                    new SqlParameter("@DeptID", SqlDbType.Int),
                                    new SqlParameter("@ApprovalType", SqlDbType.Char,1),
                                    new SqlParameter("@Opinion", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Proxy", SqlDbType.Char,1),
                                    new SqlParameter("@ProxyUserID", SqlDbType.Int),
                                    new SqlParameter("@IsNextUpd", SqlDbType.Char,1),
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DocumentID;
                parms[2].Value = _info.Sequence;
                parms[3].Value = _info.ApprovalTag.ToNull();
                parms[4].Value = _info.UserID;
                parms[5].Value = _info.DeptID;
                parms[6].Value = _info.ApprovalType.ToNull();
                parms[7].Value = _info.Opinion.ToNull();
                parms[8].Value = _info.Proxy.ToNull();
                parms[9].Value = _info.ProxyUserID;
                parms[10].Value = _isNextUpd.ToNull();

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Seq_UpdDecide", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("ModDecide: " + errMsg);
                throw;
            }
        }

        #endregion
    }
}