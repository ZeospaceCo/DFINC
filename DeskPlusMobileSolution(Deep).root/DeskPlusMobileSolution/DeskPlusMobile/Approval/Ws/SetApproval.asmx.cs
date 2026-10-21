using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;
using System.Xml;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Approval.Ws.BE;
using HtmlAgilityPack;
using System.Configuration;
using Zeospace.DeskPlusMobile.Web.Account.BE;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Collections;
using Zeospace.DeskPlusMobile.Web.Approval.Ws.EDM;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws
{
    /// <summary>
    /// SetApproval의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetApproval : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재 진행 중인지 체크 
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ModViewing(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string aTag = GetDecryptParam("ATag", "0");
            string viewing = GetDecryptParam("Viewing", "0");
            string targetUID = GetDecryptParam("TargetUID", "0");

            try
            {
                string rtnval = "";

                if (dID.Equals("") || dID.Equals("0") || aTag.Equals("")) return "!NULL";

                // 해당문서 사용여부 체크
                rtnval = ModViewing(CompanyID, int.Parse(dID), viewing, aTag, int.Parse(targetUID));
                if (!rtnval.Trim().Equals(""))
                {
                    return rtnval;
                }

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModViewing('" + dID + "','" + aTag + "')", ex);
                return "ERROR";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재 진행 중인지 체크 
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_viewing"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_targetUserID"></param>
        /// <returns></returns>
        public string ModViewing(int _companyID, int _documentID, string _viewing, string _approvalTag, int _targetUserID)
        {
            string rtnVal = "";
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@Viewing", SqlDbType.Char, 1)
                                            ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = _viewing;
                parm[3].Value = _approvalTag;
                parm[4].Value = _targetUserID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_Viewing");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsInfo", parm);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0]["ViewCheck"].ToString() == "Y")
                    {
                        string DeptName = this.GetLangName(
                                ds.Tables[0].Rows[0]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptZhtw"].ToString().ToNull(), PreLang);

                        string TitleName = this.GetLangName(
                                ds.Tables[0].Rows[0]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleZhtw"].ToString().ToNull(), PreLang);

                        string Name = this.GetLangName(
                                ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang);

                        rtnVal = DeptName + "/" + TitleName + "/" + Name;
                    }
                }

                return rtnVal;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("ModViewing(" + _companyID.ToString() + "," + _documentID.ToString() + "," + _viewing + "," + _approvalTag + "," + _targetUserID + ")", ex);
                return "ERROR";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재진행을 안하는 것
        /// 작성일자 : 2012-09-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ModDocViewing(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string aTag = GetDecryptParam("ATag", "0");
            string viewing = GetDecryptParam("Viewing", "0");

            try
            {
                if (dID.Equals("") || dID.Equals("0") || aTag.Equals("")) return "!NULL";

                return ModDocViewing(CompanyID, int.Parse(dID), viewing, aTag);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModDocViewing('" + dID + "','" + aTag + "')", ex);
                return "ERROR";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재진행을 안하는 것
        /// 작성일자 : 2012-09-11
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_viewing"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public string ModDocViewing(int _companyID, int _documentID, string _viewing, string _approvalTag)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@Viewing", SqlDbType.Char, 1)
                                            ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = _viewing;
                parm[3].Value = _approvalTag;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_DocViewing");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                return "OK";
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("ModDocViewing(" + _companyID.ToString() + "," + _documentID.ToString() + "," + _viewing + "," + _approvalTag + ")", ex);
                return "ERROR";
            }
        }

        /// <summary>
        /// 결재취소 가능여부 체크
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public string GetIsCancelApproval(string _companyID, string _documentID, string _sequence, string _approvalTag)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@DocumentID", SqlDbType.Int),
                                                            new SqlParameter("@Sequence", SqlDbType.Int),
                                                            new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 5)
                };

                parms[0].Value = UserInfo.CompanyID;
                parms[1].Value = _documentID;
                parms[2].Value = _sequence;
                parms[3].Value = _approvalTag;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_IsCancelApproval");

                return SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms).ToString();

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("ModDocViewing(" + _companyID.ToString() + "," + _documentID.ToString() + "," + _sequence.ToString() + "," + _approvalTag + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 결재취소 - 병렬결재자 중 결재한 사람이 있는지 체크
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public string GetIsCancelZA(int _companyID, int _documentID, int _sequence, string _approvalTag)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@DocumentID", SqlDbType.Int),
                                                            new SqlParameter("@Sequence", SqlDbType.Int),
                                                            new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 5)
                };

                parms[0].Value = UserInfo.CompanyID;
                parms[1].Value = _documentID;
                parms[2].Value = _sequence;
                parms[3].Value = _approvalTag;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_IsCancelZA");

                return SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms).ToString();

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetIsCancelZA(" + _companyID.ToString() + "," + _documentID.ToString() + "," + _sequence.ToString() + "," + _approvalTag + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그처리하기 
        /// 작성일자 : 2012-09-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetLog(string p)
        {
            SetDecryptString(p);
            int dID = GetDecryptParam("DID", "0").ToNumber<int>();
            int seq = GetDecryptParam("Seq", "0").ToNumber<int>();
            string aTag = GetDecryptParam("ATag", "0");
            string gubun = GetDecryptParam("Gubun", "0");
            string decisionTag = GetDecryptParam("DecisionTag", "0");

            if (CompanyID <= 0 || UserID <= 0 || dID <= 0) return "!NULL";

            string remoteAddr = Context.Request.ServerVariables["REMOTE_ADDR"];

            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "INS_PROC_LOG");

                SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@DocumentID", SqlDbType.Int),
											   new SqlParameter("@Sequence", SqlDbType.Int),
                                               new SqlParameter("@ApprovalTag", SqlDbType.VarChar,20),
											   new SqlParameter("@Gubun", SqlDbType.Char, 1),
											   new SqlParameter("@DecisionTag", SqlDbType.VarChar, 20), 
											   new SqlParameter("@IP", SqlDbType.VarChar, 50)											   
										   };
                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                parm[2].Value = seq;
                parm[3].Value = aTag;
                parm[4].Value = gubun;
                parm[5].Value = decisionTag;
                parm[6].Value = remoteAddr;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetLog('" + dID + "','" + seq + "','" + aTag + "','" + gubun + "','" + decisionTag + "','" + remoteAddr + "')", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 승인 
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetApproveS(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string seq = GetDecryptParam("Seq", "0");
            string accTag = GetDecryptParam("AccTag", "");
            string aTag = GetDecryptParam("ATag", "");
            string opinion = GetDecryptParam("Opinion", "");
            string targetUID = GetDecryptParam("TargetUID", "0");
            string targetDID = GetDecryptParam("TargetDID", "0");
            string num2 = GetDecryptParam("Num2", "");
            string groupID = GetDecryptParam("GroupID", "0");

            try
            {
                if (dID.Equals("0") || seq.Equals("0") || groupID.Equals("0") || aTag.Equals("") || accTag.Equals("")) return "!NULL";
                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                bool rtnDept = false;

                if ((targetUID.ToNumber<int>().Equals(UserInfo.UserID)) && (!targetDID.ToNumber<int>().Equals(UserInfo.DeptID)))
                {
                    for (int i = 0; i < UserInfo.UserDepts.Length; i++)
                    {
                        if (UserInfo.UserDepts[i].ToString() == targetDID.ToString())
                        {
                            rtnDept = true;
                        }
                    }
                }
                else
                {
                    rtnDept = true;
                }

                SequenceInfo sequenceInfo = new SequenceInfo();
                sequenceInfo.CompanyID = CompanyID;
                sequenceInfo.DocumentID = dID.ToNumber<int>();
                sequenceInfo.Sequence = seq.ToNumber<int>();
                sequenceInfo.ApprovalTag = aTag;
                sequenceInfo.UserID = targetUID.ToNumber<int>();
                sequenceInfo.DeptID = (rtnDept) ? targetDID.ToNumber<int>() : UserInfo.DeptID;
                sequenceInfo.ApprovalType = "S";
                sequenceInfo.Opinion = Server.UrlDecode(opinion).Replace("\r\n", "<br>");

                if (targetUID.ToNumber<int>() != UserID)
                {
                    sequenceInfo.Proxy = "Y";
                    sequenceInfo.ProxyUserID = UserID;
                }
                else
                {
                    sequenceInfo.Proxy = "N";
                    sequenceInfo.ProxyUserID = 0;
                }

                // 1. 인장, 결재정보 , 첨언처리 (결재문서 내용 처리)
                string tmpCont = StampPermit(dID, aTag, Server.UrlDecode(opinion), sequenceInfo, accTag, num2, "S");
                if (tmpCont.Equals("FILENO"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "FILENO";
                }

                if (tmpCont.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }


                // 2. DB처리             
                string rvDb = ApproveS(sequenceInfo, tmpCont, accTag, int.Parse(groupID));
                if (rvDb.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 3. 완결처리 - 문서번호 처리
                string[] arrRvDb = rvDb.Split('^');
                if (arrRvDb[0] == "Y")
                {
                    tmpCont = tmpCont.Replace("id=NO", "id=NO value='" + arrRvDb[1] + "'");
                    tmpCont = tmpCont.Replace("id='NO'", "id=NO value='" + arrRvDb[1] + "'");
                    tmpCont = tmpCont.Replace("id=\"NO\"", "id=\"NO\" value='" + arrRvDb[1] + "'");

                    string rvDbUpdate = "";
                    rvDbUpdate = UpdateHTML(CompanyID, int.Parse(dID), int.Parse(groupID), accTag, tmpCont);
                    if (rvDbUpdate.Equals("!NULL"))
                    {
                        ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                        return "!NULL";
                    }

                    string useLinked = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "USELINKED");
                    if (useLinked == "Y")
                    {
                        // edm 폴더로 문서 이관처리                                    
                        Transfer TransferEDM = new Transfer();
                        if (!TransferEDM.ApprovalToEDM(CompanyID, int.Parse(groupID)))
                        {
                            return "!NULL";
                        }
                    }
                }

                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmApproval");
                if (dvApp.Count > 0)
                {
                    // 4. 알람  
                    BE.AccountInfo info = new AccountInfo();
                    info.CompanyID = UserInfo.CompanyID;
                    info.UserID = UserInfo.UserID;
                    info.Name = UserInfo.Name;
                    info.DeptName = UserInfo.DeptName;
                    info.TitleName = UserInfo.TitleName;
                    info.DutyName = UserInfo.DutyName;
                    info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
                    info.MobilePhone = UserInfo.MobilePhone;

                    info.Opinion = Server.UrlDecode(opinion);
                    info.ApprovalType = "S";
                    info.PreLang = PreLang;

                    List<BE.AccountInfo> targetList = null;

                    if (arrRvDb[0] == "Y")
                    {
                        targetList = GetAlarmTarget("COMPLETE", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(seq), accTag);
                        SendAlarm(int.Parse(dID), info, targetList, "C", dvApp);
                    }
                    else
                    {
                        if (!opinion.Equals(""))
                        {
                            targetList = GetAlarmTarget("OPINION", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(seq), accTag);
                            SendAlarm(int.Parse(dID), info, targetList, "O", dvApp);
                        }

                        targetList = GetAlarmTarget("APPROVE", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(seq), accTag);
                        SendAlarm(int.Parse(dID), info, targetList, "S", dvApp);
                    }
                }

                ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetApproveS()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전결
        /// 작성일자 : 2012-09-17
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetApproveA(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string seq = GetDecryptParam("Seq", "0");
            string accTag = GetDecryptParam("AccTag", "");
            string aTag = GetDecryptParam("ATag", "");
            string opinion = GetDecryptParam("Opinion", "");
            string targetUID = GetDecryptParam("TargetUID", "0");
            string targetDID = GetDecryptParam("TargetDID", "0");
            string num2 = GetDecryptParam("Num2", "");
            string groupID = GetDecryptParam("GroupID", "0");

            try
            {
                if (dID.Equals("0") || seq.Equals("0") || groupID.Equals("0") || aTag.Equals("") || accTag.Equals("")) return "!NULL";
                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                bool rtnDept = false;

                if ((targetUID.ToNumber<int>().Equals(UserInfo.UserID)) && (!targetDID.ToNumber<int>().Equals(UserInfo.DeptID)))
                {
                    for (int i = 0; i < UserInfo.UserDepts.Length; i++)
                    {
                        if (UserInfo.UserDepts[i].ToString() == targetDID.ToString())
                        {
                            rtnDept = true;
                        }
                    }
                }
                else
                {
                    rtnDept = true;
                }

                SequenceInfo sequenceInfo = new SequenceInfo();
                sequenceInfo.CompanyID = CompanyID;
                sequenceInfo.DocumentID = dID.ToNumber<int>();
                sequenceInfo.Sequence = seq.ToNumber<int>();
                sequenceInfo.ApprovalTag = aTag;
                sequenceInfo.UserID = targetUID.ToNumber<int>();
                sequenceInfo.DeptID = (rtnDept) ? targetDID.ToNumber<int>() : UserInfo.DeptID;
                sequenceInfo.ApprovalType = "A";
                sequenceInfo.Opinion = Server.UrlDecode(opinion).Replace("\r\n", "<br>");

                if (targetUID.ToNumber<int>() != UserID)
                {
                    sequenceInfo.Proxy = "Y";
                    sequenceInfo.ProxyUserID = UserID;
                }
                else
                {
                    sequenceInfo.Proxy = "N";
                    sequenceInfo.ProxyUserID = 0;
                }

                // 1. 인장, 결재정보 , 첨언처리 (결재문서 내용 처리)
                string tmpCont = StampPermit(dID, aTag, Server.UrlDecode(opinion), sequenceInfo, accTag, num2, "A");

                if (tmpCont.Equals("FILENO"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "FILENO";
                }

                if (tmpCont.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 2. DB처리             
                string rvDb = ApproveA(sequenceInfo, tmpCont, accTag, int.Parse(groupID));
                if (rvDb.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 3. 완결처리 - 문서번호 처리
                string[] arrRvDb = rvDb.Split('^');
                if (arrRvDb[0] == "Y")
                {
                    tmpCont = tmpCont.Replace("id=NO", "id=NO value='" + arrRvDb[1] + "'");
                    tmpCont = tmpCont.Replace("id='NO'", "id=NO value='" + arrRvDb[1] + "'");
                    tmpCont = tmpCont.Replace("id=\"NO\"", "id=\"NO\" value='" + arrRvDb[1] + "'");

                    string rvDbUpdate = "";
                    rvDbUpdate = UpdateHTML(CompanyID, int.Parse(dID), int.Parse(groupID), accTag, tmpCont);
                    if (rvDbUpdate.Equals("!NULL"))
                    {
                        ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                        return "!NULL";
                    }

                    string useLinked = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "USELINKED");
                    if (useLinked == "Y")
                    {
                        // edm 폴더로 문서 이관처리                                    
                        Transfer TransferEDM = new Transfer();
                        if (!TransferEDM.ApprovalToEDM(CompanyID, int.Parse(dID)))
                        {
                            return "!NULL";
                        }
                    }
                }

                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmApproval");
                if (dvApp.Count > 0)
                {
                    BE.AccountInfo info = new AccountInfo();
                    info.CompanyID = UserInfo.CompanyID;
                    info.UserID = UserInfo.UserID;
                    info.Name = UserInfo.Name;
                    info.DeptName = UserInfo.DeptName;
                    info.TitleName = UserInfo.TitleName;
                    info.DutyName = UserInfo.DutyName;
                    info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
                    info.MobilePhone = UserInfo.MobilePhone;

                    info.Opinion = Server.UrlDecode(opinion);
                    info.ApprovalType = "A";
                    info.PreLang = PreLang;

                    List<BE.AccountInfo> targetList = GetAlarmTarget("COMPLETE", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(seq), accTag);
                    SendAlarm(int.Parse(dID), info, targetList, "C", dvApp);
                }

                ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetApproveA()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 합의 & 합의 불가
        /// 작성일자 : 2012-09-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetApproveH(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string seq = GetDecryptParam("Seq", "0");
            string accTag = GetDecryptParam("AccTag", "");
            string aTag = GetDecryptParam("ATag", "");
            string opinion = GetDecryptParam("Opinion", "");
            string targetUID = GetDecryptParam("TargetUID", "0");
            string targetDID = GetDecryptParam("TargetDID", "0");
            string num2 = GetDecryptParam("Num2", "");
            string groupID = GetDecryptParam("GroupID", "0");
            string appGun = GetDecryptParam("AppGun", "");

            try
            {
                if (dID.Equals("0") || seq.Equals("0") || groupID.Equals("0") || aTag.Equals("") || accTag.Equals("")) return "!NULL";
                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                bool rtnDept = false;

                if ((targetUID.ToNumber<int>().Equals(UserInfo.UserID)) && (!targetDID.ToNumber<int>().Equals(UserInfo.DeptID)))
                {
                    for (int i = 0; i < UserInfo.UserDepts.Length; i++)
                    {
                        if (UserInfo.UserDepts[i].ToString() == targetDID.ToString())
                        {
                            rtnDept = true;
                        }
                    }
                }
                else
                {
                    rtnDept = true;
                }

                SequenceInfo sequenceInfo = new SequenceInfo();
                sequenceInfo.CompanyID = CompanyID;
                sequenceInfo.DocumentID = dID.ToNumber<int>();
                sequenceInfo.Sequence = seq.ToNumber<int>();
                sequenceInfo.ApprovalTag = aTag;
                sequenceInfo.UserID = targetUID.ToNumber<int>();
                sequenceInfo.DeptID = (rtnDept) ? targetDID.ToNumber<int>() : UserInfo.DeptID;
                sequenceInfo.ApprovalType = "S";
                sequenceInfo.Opinion = Server.UrlDecode(opinion).Replace("\r\n", "<br>");

                if (targetUID.ToNumber<int>() != UserID)
                {
                    sequenceInfo.Proxy = "Y";
                    sequenceInfo.ProxyUserID = UserID;
                }
                else
                {
                    sequenceInfo.Proxy = "N";
                    sequenceInfo.ProxyUserID = 0;
                }

                // 1. 인장, 결재정보 , 첨언처리 (결재문서 내용 처리)
                string tmpCont = StampPermit(dID, aTag, Server.UrlDecode(opinion), sequenceInfo, accTag, num2, appGun);

                if (tmpCont.Equals("FILENO"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "FILENO";
                }

                if (tmpCont.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 2. DB처리             
                string rvDb = ApproveH(sequenceInfo, tmpCont, accTag, int.Parse(groupID));
                if (rvDb.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 3. Sequences+ 1 이고 Myturn = 'Y' 인 경우에 알람 발송                
                if (rvDb == "0")
                {
                    // 알람-코드관리로 처리
                    DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmApproval");
                    if (dvApp.Count > 0)
                    {
                        BE.AccountInfo info = new AccountInfo();
                        info.CompanyID = UserInfo.CompanyID;
                        info.UserID = UserInfo.UserID;
                        info.Name = UserInfo.Name;
                        info.DeptName = UserInfo.DeptName;
                        info.TitleName = UserInfo.TitleName;
                        info.DutyName = UserInfo.DutyName;
                        info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
                        info.MobilePhone = UserInfo.MobilePhone;

                        info.Opinion = Server.UrlDecode(opinion);
                        info.ApprovalType = "S";
                        info.PreLang = PreLang;

                        List<BE.AccountInfo> targetList = null;

                        if (!opinion.Equals(""))
                        {
                            targetList = GetAlarmTarget("OPINION", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(seq), accTag);
                            SendAlarm(int.Parse(dID), info, targetList, "O", dvApp);
                        }

                        targetList = GetAlarmTarget("APPROVE", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(seq), accTag);
                        SendAlarm(int.Parse(dID), info, targetList, "S", dvApp);
                    }
                }

                ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetApproveH()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재취소
        /// 작성일자 : 2012-09-21
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetCancel(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string seq = GetDecryptParam("Seq", "0");
            string accTag = GetDecryptParam("AccTag", "");
            string aTag = GetDecryptParam("ATag", "");
            string groupID = GetDecryptParam("GroupID", "0");

            try
            {
                if (dID.Equals("0") || seq.Equals("0") || groupID.Equals("0") || aTag.Equals("") || accTag.Equals("")) return "!NULL";
                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                SequenceInfo sequenceInfo = new SequenceInfo();
                sequenceInfo.CompanyID = CompanyID;
                sequenceInfo.DocumentID = dID.ToNumber<int>();
                sequenceInfo.Sequence = seq.ToNumber<int>();
                sequenceInfo.ApprovalTag = aTag;
                sequenceInfo.Opinion = "";

                // 1. 인장, 결재정보 , 첨언처리 (결재문서 내용 처리)
                string tmpCont = StampCancel(dID, seq, aTag);
                if (tmpCont.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 2. DB처리                             
                string rvDb = Cancel(sequenceInfo, tmpCont, accTag, int.Parse(groupID));
                if (rvDb.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetCancel()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 결재취소
        /// 작성일자 : 2012-09-21
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetCancelApproval(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string seq = GetDecryptParam("Seq", "0");
            string aTag = GetDecryptParam("ATag", "");
            string accTag = GetDecryptParam("AccTag", "");
            string groupID = GetDecryptParam("GroupID", "");


            try
            {
                if (dID.Equals("0") || seq.Equals("0") || aTag.Equals("")) return "!NULL";

                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                //1. 결재취소 가능여부 확인. 
                string tmpIsCancelApproval = GetIsCancelApproval(CompanyID.ToString(), dID, seq, aTag);
                if (tmpIsCancelApproval.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                if (tmpIsCancelApproval.Equals("N"))
                {
                    //CancelApprovalCheck 다국어 추가!
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return tmpIsCancelApproval;
                }

                //2. 문서정보 가져온 후 인장처리
                string tmpCont = StampCancelApproval(dID, seq, aTag);
                if (tmpCont.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                //3. DB처리
                BE.SequenceInfo nextinfo = null;
                List<BE.SequenceInfo> listseq = GetSequenceInfo(CompanyID, dID.ToNumber<int>()).Where(x => x.Sequence == seq.ToNumber<int>() + 1).ToList<BE.SequenceInfo>();
                if (listseq.Count > 0)
                {
                    nextinfo = listseq[0];
                }

                string tmpState = "";
                if (aTag.Substring(0, 2) == "ZA")
                {
                    string tmpZAApproveYN = GetIsCancelZA(CompanyID, int.Parse(dID), int.Parse(seq), aTag);

                    //병렬사용자중에 결재 한 사람이 없을 때
                    if (seq.Equals("2") && tmpZAApproveYN == "Y")
                    {
                        tmpState = "W";
                    }
                    else
                    {
                        tmpState = "I";
                    }
                }
                else
                {
                    tmpState = (seq.Equals("2")) ? "W" : "I";
                }

                string rvDb = CancelApproval(CompanyID, dID.ToNumber<int>(), groupID.ToNumber<int>(), seq.ToNumber<int>(), aTag, nextinfo.MyTurn, nextinfo.ApprovalTag
                    , nextinfo.Sequence, accTag, tmpState, tmpCont);
                if (rvDb.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                //4. 알람처리
                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmApproval");
                if (dvApp.Count > 0)
                {
                    // 4. 알람  
                    BE.AccountInfo info = new AccountInfo();
                    info.CompanyID = UserInfo.CompanyID;
                    info.UserID = UserInfo.UserID;
                    info.Name = UserInfo.Name;
                    info.DeptName = UserInfo.DeptName;
                    info.TitleName = UserInfo.TitleName;
                    info.DutyName = UserInfo.DutyName;
                    info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
                    info.MobilePhone = UserInfo.MobilePhone;

                    info.Opinion = "";
                    info.ApprovalType =
                    info.PreLang = PreLang;

                    List<BE.AccountInfo> targetList = null;

                    targetList = GetAlarmTarget("APPROVEC", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(seq), accTag);
                    SendAlarm(int.Parse(dID), info, targetList, tmpState, dvApp);
                }

                ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);


                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetCancelApproval()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 반려
        /// 작성일자 : 2012-09-26
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetReturn(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string seq = GetDecryptParam("Seq", "0");
            string accTag = GetDecryptParam("AccTag", "");
            string aTag = GetDecryptParam("ATag", "");
            string opinion = GetDecryptParam("Opinion", "");
            string targetUID = GetDecryptParam("TargetUID", "0");
            string targetDID = GetDecryptParam("TargetDID", "0");
            string num2 = GetDecryptParam("Num2", "");
            string groupID = GetDecryptParam("GroupID", "0");
            string targetInfo = GetDecryptParam("TargetInfo", "");

            try
            {
                if (dID.Equals("0") || seq.Equals("0") || groupID.Equals("0") || aTag.Equals("") || accTag.Equals("")) return "!NULL";
                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                bool rtnDept = false;

                if ((targetUID.ToNumber<int>().Equals(UserInfo.UserID)) && (!targetDID.ToNumber<int>().Equals(UserInfo.DeptID)))
                {
                    for (int i = 0; i < UserInfo.UserDepts.Length; i++)
                    {
                        if (UserInfo.UserDepts[i].ToString() == targetDID.ToString())
                        {
                            rtnDept = true;
                        }
                    }
                }
                else
                {
                    rtnDept = true;
                }

                SequenceInfo sequenceInfo = new SequenceInfo();
                sequenceInfo.CompanyID = CompanyID;
                sequenceInfo.DocumentID = dID.ToNumber<int>();
                sequenceInfo.Sequence = seq.ToNumber<int>();
                sequenceInfo.ApprovalTag = aTag;
                sequenceInfo.UserID = targetUID.ToNumber<int>();
                sequenceInfo.DeptID = (rtnDept) ? targetDID.ToNumber<int>() : UserInfo.DeptID;
                sequenceInfo.ApprovalType = "R";
                sequenceInfo.Opinion = Server.UrlDecode(opinion).Replace("\r\n", "<br>");

                if (targetUID.ToNumber<int>() != UserID)
                {
                    sequenceInfo.Proxy = "Y";
                    sequenceInfo.ProxyUserID = UserID;
                }
                else
                {
                    sequenceInfo.Proxy = "N";
                    sequenceInfo.ProxyUserID = 0;
                }

                // 1. 인장, 결재정보 , 첨언처리 (결재문서 내용 처리)
                string tmpCont = StampPermit(dID, aTag, Server.UrlDecode(opinion), sequenceInfo, accTag, num2, "R");

                if (tmpCont.Equals("FILENO"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "FILENO";
                }

                if (tmpCont.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 2. DB처리             
                //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID
                string[] tmpTargetInfo = targetInfo.Split('-');

                // 반려대상 부서 확인
                string isTargetZD = "N";
                if (accTag.Equals("ZD"))
                {
                    isTargetZD = "Y";
                }
                else
                {
                    isTargetZD = (targetInfo.IndexOf(groupID + "-") == 0) ? "Y" : "N";
                }

                string rvDb = Return(sequenceInfo, tmpCont, accTag, int.Parse(groupID), tmpTargetInfo[0].ToNumber<int>(), tmpTargetInfo[1].ToNumber<int>(), tmpTargetInfo[2], isTargetZD);
                if (rvDb.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmApproval");
                if (dvApp.Count > 0)
                {
                    // 4. 알람  
                    BE.AccountInfo info = new AccountInfo();
                    info.CompanyID = UserInfo.CompanyID;
                    info.UserID = UserInfo.UserID;
                    info.Name = UserInfo.Name;
                    info.DeptName = UserInfo.DeptName;
                    info.TitleName = UserInfo.TitleName;
                    info.DutyName = UserInfo.DutyName;
                    info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
                    info.MobilePhone = UserInfo.MobilePhone;

                    info.Opinion = Server.UrlDecode(opinion);
                    info.ApprovalType = "R";
                    info.PreLang = PreLang;

                    List<BE.AccountInfo> targetList = GetAlarmTarget("REJECT", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(seq), accTag);

                    if (targetInfo.IndexOf("ZD0") > 0)
                    {
                        SendAlarm(int.Parse(dID), info, targetList, "RW", dvApp);   //상신자 반려
                    }
                    else
                    {
                        SendAlarm(int.Parse(dID), info, targetList, "RS", dvApp);   //중간결재자 반려
                    }
                }

                ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetReturn()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 의견추가
        /// 작성일자 : 2017-01-16
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetOpinion(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string accTag = GetDecryptParam("AccTag", "");
            string opinion = GetDecryptParam("Opinion", "");
            string groupID = GetDecryptParam("GroupID", "0");
            string gubun = GetDecryptParam("Gubun", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.SetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                if (dID.Equals("0") || groupID.Equals("0") || accTag.Equals("")) return "!NULL";
                if (UserInfo == null) return "!NULL";

                opinion = Server.UrlDecode(opinion).Replace("\r\n", "<br>");

                // 1. 첨언처리 (결재문서 내용 처리)   
                string tmpCont = OpinionCont(dID, opinion, gubun);
                if (tmpCont.Equals("!NULL")) return "!NULL";

                // 2. DB처리             
                string rvDb = AddOpinion(UserInfo.CompanyID, dID.ToNumber<int>(), groupID.ToNumber<int>(), accTag, tmpCont, opinion, UserInfo.UserID, gubun);
                if (rvDb.Equals("!NULL")) return "!NULL";

                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmApproval");
                if (dvApp.Count > 0)
                {
                    // 3. 알람  
                    BE.AccountInfo info = new AccountInfo();
                    info.CompanyID = UserInfo.CompanyID;
                    info.UserID = UserInfo.UserID;
                    info.Name = UserInfo.Name;
                    info.DeptName = UserInfo.DeptName;
                    info.TitleName = UserInfo.TitleName;
                    info.DutyName = UserInfo.DutyName;
                    info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
                    info.MobilePhone = UserInfo.MobilePhone;

                    info.Opinion = Server.UrlDecode(opinion);
                    info.ApprovalType = "S";
                    info.PreLang = PreLang;

                    List<BE.AccountInfo> targetList = null;

                    targetList = GetAlarmTarget("OPINIONAll", CompanyID, int.Parse(dID), int.Parse(groupID), 0, accTag);
                    SendAlarm(int.Parse(dID), info, targetList, "C", dvApp);

                }
                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetOpinion()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 상신보류, 보류해제
        /// 작성일자 : 2019-08-06
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetApproveHCancel(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string aTag = GetDecryptParam("ATag", "");
            string accTag = GetDecryptParam("AccTag", "");
            string stat = GetDecryptParam("Stat", "");
            string groupID = GetDecryptParam("GroupID", "");


            try
            {
                if (dID.Equals("0") || accTag.Equals("") || stat.Equals("") || groupID.Equals("0")) return "!NULL";

                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                string rvDb = ModApprovalState(CompanyID, dID.ToNumber<int>(), groupID.ToNumber<int>(), accTag, stat);
                if (rvDb.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetApproveHCancel()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 상신취소
        /// 작성일자 : 2019-08-06
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetApproveDel(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string aTag = GetDecryptParam("ATag", "");
            string accTag = GetDecryptParam("AccTag", "");
            string groupID = GetDecryptParam("GroupID", "");


            try
            {
                if (dID.Equals("0") || accTag.Equals("") || groupID.Equals("0")) return "!NULL";

                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                BE.AccountInfo info = new AccountInfo();
                info.CompanyID = UserInfo.CompanyID;
                info.UserID = UserInfo.UserID;
                info.Name = UserInfo.Name;
                info.DeptName = UserInfo.DeptName;
                info.TitleName = UserInfo.TitleName;
                info.DutyName = UserInfo.DutyName;
                info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
                info.MobilePhone = UserInfo.MobilePhone;

                info.Opinion = "";
                info.ApprovalType = "C";
                info.PreLang = PreLang;

                BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
                if (dpBll.DeleteDoc(CompanyID, groupID.ToNumber<int>(), dID.ToNumber<int>(), info, accTag.Substring(0, 2)))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "OK";
                }
                else
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetApproveDel()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 후열처리
        /// 작성일자 : 2019-08-07
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetAfterward(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string accTag = GetDecryptParam("AccTag", "");
            string aTag = GetDecryptParam("ATag", "");
            string groupID = GetDecryptParam("GroupID", "0");
            string NextSeq = GetDecryptParam("NextSeq", "0");
            string NextTag = GetDecryptParam("NextTag", "");
            string OrginUserID = GetDecryptParam("OrginUserID", "");
            string OrginDeptID = GetDecryptParam("OrginDeptID", "");
            string MsgReason = GetDecryptParam("MsgReason", "");
            string opinion = GetDecryptParam("Opinion", "");

            try
            {
                if (dID.Equals("0") || groupID.Equals("0") || aTag.Equals("") || NextSeq.Equals("0") || NextTag.Equals("")) return "!NULL";
                if (UserInfo == null)
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                SequenceInfo sequenceInfo = new SequenceInfo();
                sequenceInfo.CompanyID = CompanyID;
                sequenceInfo.DocumentID = dID.ToNumber<int>();
                sequenceInfo.Sequence = NextSeq.ToNumber<int>();
                sequenceInfo.ApprovalTag = NextTag;
                sequenceInfo.UserID = OrginUserID.ToNumber<int>();
                sequenceInfo.DeptID = OrginDeptID.ToNumber<int>();
                sequenceInfo.ApprovalType = "L";
                sequenceInfo.Opinion = Server.UrlDecode(opinion).Replace("\r\n", "<br>");

                if (OrginUserID.ToNumber<int>() != UserID)
                {
                    sequenceInfo.Proxy = "Y";
                    sequenceInfo.ProxyUserID = UserID;
                }
                else
                {
                    sequenceInfo.Proxy = "N";
                    sequenceInfo.ProxyUserID = 0;
                }

                // 1. 인장, 결재정보 , 첨언처리 (결재문서 내용 처리)
                string tmpCont = StampPermit(dID, NextTag, Server.UrlDecode(opinion), sequenceInfo, MsgReason);

                if (tmpCont.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 2. DB처리             
                string rvDb = ApproveL(sequenceInfo, tmpCont, accTag, int.Parse(groupID));
                if (rvDb.Equals("!NULL"))
                {
                    ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);
                    return "!NULL";
                }

                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmApproval");
                if (dvApp.Count > 0)
                {
                    // 4. 알람  
                    BE.AccountInfo info = new AccountInfo();
                    info.CompanyID = UserInfo.CompanyID;
                    info.UserID = UserInfo.UserID;
                    info.Name = UserInfo.Name;
                    info.DeptName = UserInfo.DeptName;
                    info.TitleName = UserInfo.TitleName;
                    info.DutyName = UserInfo.DutyName;
                    info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
                    info.MobilePhone = UserInfo.MobilePhone;

                    info.Opinion = Server.UrlDecode(opinion);
                    info.ApprovalType = "L";
                    info.PreLang = PreLang;

                    List<BE.AccountInfo> targetList = null;

                    if (!opinion.Equals(""))
                    {
                        targetList = GetAlarmTarget("OPINION", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(NextSeq), accTag);
                        SendAlarm(int.Parse(dID), info, targetList, "O", dvApp);
                    }

                    targetList = GetAlarmTarget("APPROVE", CompanyID, int.Parse(dID), int.Parse(groupID), int.Parse(NextSeq), accTag);
                    SendAlarm(int.Parse(dID), info, targetList, "S", dvApp);

                }

                ModDocViewing(CompanyID, int.Parse(dID), "N", aTag);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetAfterward()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨언&인장 처리 (승인, 전결, 합의, 합의불가,반려)
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <param name="_docID"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_opinion"></param>
        /// <param name="_sequenceInfo"></param>
        /// <param name="_accountTag"></param>
        /// <param name="_number2"></param>
        /// <param name="_appGun"></param>
        /// <returns></returns>
        public string StampPermit(string _docID, string _approvalTag, string _opinion, SequenceInfo _sequenceInfo, string _accountTag, string _number2, string _appGun)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.SetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                // 1. 인장 복사
                string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                string filePath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
                string designPath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_DESIGN");
                string FullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
                string SealSizeWidth = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "SIZEIMGSEAL").Split(',')[0];
                string SealSizeHeight = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "SIZEIMGSEAL").Split(',')[1];
                string sealFrom = String.Empty;

                if (_appGun == "HR")
                {
                    // 합의 불가                                                          
                    sealFrom = Server.MapPath(designPath + "/C/Img/Lang/" + PreLang + "/app_stamp_HR.gif");
                }
                else if (_appGun == "H")
                {
                    // 합의                    
                    sealFrom = Server.MapPath(designPath + "/C/Img/Lang/" + PreLang + "/app_stamp_H.gif");
                }
                else if (_appGun == "A")
                {
                    // 전결                    
                    sealFrom = Server.MapPath(designPath + "/C/Img/Lang/" + PreLang + "/app_stamp_A.gif");
                }
                else if (_appGun == "R")
                {
                    // 반려                    
                    sealFrom = Server.MapPath(designPath + "/C/Img/Lang/" + PreLang + "/app_stamp_R.gif");
                }
                else
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_USER");

                    if (_sequenceInfo.Proxy == "Y")
                    {
                        // 대결자일 경우 원결재자의 인장이 들어가도록
                        proc = string.Format(proc, CompanyID, _sequenceInfo.UserID);

                        DataSet dss = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserList", null);

                        string sealImage = "";

                        if (dss != null)
                        {
                            foreach (DataRow dr in dss.Tables[0].Rows)
                            {
                                sealImage = dr["SealImage"].ToString();
                            }
                        }

                        //  인장파일 복사
                        sealFrom = Server.MapPath(filePath + "/" + CompanyID.ToString() + "/Users/" + _sequenceInfo.UserID + "/Picture/" + sealImage);
                    }
                    else
                    {
                        proc = string.Format(proc, CompanyID, UserID);

                        DataSet dss = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserList", null);

                        string sealImage = "";

                        if (dss != null)
                        {
                            foreach (DataRow dr in dss.Tables[0].Rows)
                            {
                                sealImage = dr["SealImage"].ToString();
                            }
                        }

                        //  인장파일 복사
                        sealFrom = Server.MapPath(filePath + "/" + CompanyID.ToString() + "/Users/" + UserInfo.UserID + "/Picture/" + sealImage);
                    }
                }

                string sealTo = Server.MapPath(filePath + "/" + CompanyID.ToString() + "/Approval/Document/" + _number2 + "/" + _docID + "/" + _accountTag + "_seal" + _sequenceInfo.ApprovalTag + ".gif");

                if (!File.Exists(sealFrom))
                {
                    return "FILENO";
                }

                File.Copy(sealFrom, sealTo, true);

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                sql = string.Format(sql, CompanyID.ToString(), _docID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCont", null);
                string tmpCont = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(tmpCont);

                HtmlNode body = doc.DocumentNode.SelectSingleNode("//body");

                string tmpDeptName = UserInfo.DeptName;
                string tmpTitleName = UserInfo.TitleName;
                string tmpDutyName = UserInfo.DutyName;
                string tmpUserName = UserInfo.Name;

                if ((_sequenceInfo.UserID.Equals(UserInfo.UserID)) && (!_sequenceInfo.DeptID.Equals(UserInfo.DeptID)))
                {
                    List<BE.AccountInfo> ainfo = GetAccountInfo(CompanyID, _sequenceInfo.UserID, _sequenceInfo.DeptID);

                    foreach (BE.AccountInfo info in ainfo)
                    {
                        tmpDeptName = info.DeptName;
                        tmpTitleName = info.TitleName;
                        tmpDutyName = info.DutyName;
                    }
                }

                // 2. 첨언처리
                if (_opinion.Trim() != "")
                {
                    string tmpOpinion = GetOpinionTag(_opinion, UserInfo.Name, tmpTitleName, tmpDeptName);
                    if (doc.DocumentNode.SelectNodes("//div[@id='OPINION']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//div[@id='OPINION']"))
                        {
                            span.InnerHtml = span.InnerHtml.Insert(span.InnerHtml.Length, tmpOpinion);
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//div[@id='OPINIONVIEW']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//div[@id='OPINIONVIEW']"))
                        {
                            span.InnerHtml = resMng.GetString("TagDic_OPINIONVIEW");
                        }
                    }
                }

                // 3. 결재 정보
                string todayObj = "", timeObj = "", monthDayObj = "", timeExObj = "";
                string tmpProxy = "";
                string brTag = " ";

                this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.SetApproval).Assembly;	//어셈블리 선언            
                this.GetResourceManager();

                if (_sequenceInfo.Proxy == "Y")
                {
                    tmpProxy = resMng.GetString("ProxyS");

                    #region 원결재자정보
                    string sqlSeq = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ApprovalTag");
                    sqlSeq = string.Format(sqlSeq, CompanyID.ToString(), _docID, _sequenceInfo.Sequence, _sequenceInfo.ApprovalTag);
                    DataSet dsSeq = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlSeq, "dsSeq", null);

                    if (dsSeq.Tables[0].Rows.Count > 0)
                    {
                        tmpUserName = this.GetLangName(
                        dsSeq.Tables[0].Rows[0]["NameBase"].ToString(), dsSeq.Tables[0].Rows[0]["NameKo"].ToString(),
                        dsSeq.Tables[0].Rows[0]["NameEn"].ToString(), dsSeq.Tables[0].Rows[0]["NameJa"].ToString(),
                        dsSeq.Tables[0].Rows[0]["NameFr"].ToString(), dsSeq.Tables[0].Rows[0]["NameZhcn"].ToString(),
                        dsSeq.Tables[0].Rows[0]["NameZhtw"].ToString(), PreLang);

                        tmpDeptName = this.GetLangName(
                        dsSeq.Tables[0].Rows[0]["DeptBase"].ToString(), dsSeq.Tables[0].Rows[0]["DeptKo"].ToString(),
                        dsSeq.Tables[0].Rows[0]["DeptEn"].ToString(), dsSeq.Tables[0].Rows[0]["DeptJa"].ToString(),
                        dsSeq.Tables[0].Rows[0]["DeptFr"].ToString(), dsSeq.Tables[0].Rows[0]["DeptZhcn"].ToString(),
                        dsSeq.Tables[0].Rows[0]["DeptZhtw"].ToString(), PreLang);

                        tmpTitleName = this.GetLangName(
                        dsSeq.Tables[0].Rows[0]["TitleBase"].ToString(), dsSeq.Tables[0].Rows[0]["TitleKo"].ToString(),
                        dsSeq.Tables[0].Rows[0]["TitleEn"].ToString(), dsSeq.Tables[0].Rows[0]["TitleJa"].ToString(),
                        dsSeq.Tables[0].Rows[0]["TitleFr"].ToString(), dsSeq.Tables[0].Rows[0]["TitleZhcn"].ToString(),
                        dsSeq.Tables[0].Rows[0]["TitleZhtw"].ToString(), PreLang);

                        tmpDutyName = this.GetLangName(
                        dsSeq.Tables[0].Rows[0]["DutyBase"].ToString(), dsSeq.Tables[0].Rows[0]["DutyKo"].ToString(),
                        dsSeq.Tables[0].Rows[0]["DutyEn"].ToString(), dsSeq.Tables[0].Rows[0]["DutyJa"].ToString(),
                        dsSeq.Tables[0].Rows[0]["DutyFr"].ToString(), dsSeq.Tables[0].Rows[0]["DutyZhcn"].ToString(),
                        dsSeq.Tables[0].Rows[0]["DutyZhtw"].ToString(), PreLang);
                    }

                    #endregion
                }

                DateTime dt = System.DateTime.Now;
                todayObj = dt.ToShortDateString();
                timeObj = dt.ToShortDateString() + " " + dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0');
                monthDayObj = dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                timeExObj = dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0');


                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO0']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO0']"))
                    {
                        span.InnerHtml = todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO1']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO1']"))
                    {
                        span.InnerHtml = tmpProxy + tmpTitleName + brTag + tmpUserName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO2']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO2']"))
                    {
                        span.InnerHtml = tmpProxy + tmpTitleName + brTag + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO3']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO3']"))
                    {
                        span.InnerHtml = tmpProxy + tmpUserName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO4']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO4']"))
                    {
                        span.InnerHtml = tmpProxy + tmpTitleName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO5']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO5']"))
                    {
                        span.InnerHtml = tmpProxy + tmpDeptName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO6']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO6']"))
                    {
                        span.InnerHtml = tmpProxy + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO7']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO7']"))
                    {
                        span.InnerHtml = tmpProxy + tmpDeptName + brTag + tmpTitleName + brTag + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO8']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO8']"))
                    {
                        span.InnerHtml = timeObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO9']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO9']"))
                    {
                        span.InnerHtml = tmpProxy + tmpDutyName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO10']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO10']"))
                    {
                        span.InnerHtml = tmpProxy + tmpDutyName + brTag + tmpUserName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO11']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO11']"))
                    {
                        span.InnerHtml = tmpProxy + tmpDutyName + brTag + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO12']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO12']"))
                    {
                        span.InnerHtml = tmpProxy + tmpDeptName + brTag + tmpDutyName + brTag + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO13']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO13']"))
                    {
                        span.InnerHtml = tmpProxy + tmpUserName + brTag + timeObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO14']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO14']"))
                    {
                        span.InnerHtml = tmpProxy + monthDayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO15']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO15']"))
                    {
                        span.InnerHtml = tmpProxy + timeExObj;
                    }
                }

                // 4. 결재인장          
                string sealTarPath = "/DeskplusEIP/FileData/" + CompanyID.ToString() + "/Approval/Document/" + _number2 + "/" + _docID + "/" + _accountTag + "_seal" + _sequenceInfo.ApprovalTag + ".gif";
                foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "']"))
                {
                    span.InnerHtml = "<img src=\"" + httpType + FullDomain + sealTarPath + "\" width=" + SealSizeWidth + "px height=" + SealSizeHeight + "px>";
                }

                return body.ParentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("StampPermit(" + _docID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 인장 처리 (결재취소)
        /// 작성일자 : 2012-09-21
        /// </summary>
        /// <param name="_docID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public string StampCancel(string _docID, string _sequence, string _approvalTag)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                sql = string.Format(sql, CompanyID.ToString(), _docID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCont", null);
                string tmpCont = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(tmpCont);

                HtmlNode body = doc.DocumentNode.SelectSingleNode("//body");

                string sqlSeq = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SelectSeq");
                sqlSeq = string.Format(sqlSeq, CompanyID.ToString(), _docID, _sequence);
                DataSet dss = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlSeq, "dsSeq", null);

                //  1. 결재처리(결재정보 및 인장)
                foreach (DataRow dr in dss.Tables[0].Rows)
                {
                    string tmpApprovalTag = dr["ApprovalTag"].ToString();

                    if (((dr["Sequence"].ToString() == _sequence) && (tmpApprovalTag == _approvalTag)) || (dr["Sequence"].ToString().ToNumber<int>() > _sequence.ToNumber<int>()))
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "']"))
                        {
                            span.InnerHtml = dr["NameBase"].ToString();
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO0']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO0']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO1']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO1']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO2']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO2']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO3']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO3']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO4']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO4']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO5']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO5']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO6']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO6']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO7']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO7']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO8']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO8']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO9']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO9']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO10']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO10']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO11']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO11']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO12']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO12']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO13']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO13']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO14']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO14']"))
                            {
                                span.InnerHtml = "";
                            }
                        }

                        if (doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO15']") != null)
                        {
                            foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + tmpApprovalTag + "_INFO15']"))
                            {
                                span.InnerHtml = "";
                            }
                        }
                    }
                }

                return body.ParentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("StampCancel(" + _docID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 인장 처리 (결재취소-관계있는문서)
        /// </summary>
        /// <param name="_docID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public string StampCancelApproval(string _docID, string _sequence, string _approvalTag)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                sql = string.Format(sql, CompanyID.ToString(), _docID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCont", null);
                string tmpCont = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(tmpCont);

                HtmlNode body = doc.DocumentNode.SelectSingleNode("//body");


                #region Info처리
                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO0']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO0']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO1']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO1']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO2']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO2']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO3']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO3']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO4']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO4']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO5']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO5']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO6']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO6']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO7']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO7']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO8']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO8']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO9']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO9']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO10']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO10']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO11']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO11']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO12']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO12']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO13']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO13']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO14']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO14']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO15']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO15']"))
                    {
                        span.InnerHtml = "&nbsp;";
                    }
                }
                #endregion

                // 4. 결재인장          
                foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "']"))
                {
                    span.InnerHtml = UserInfo.NameBase;
                }



                return body.ParentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("StampCancel(" + _docID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨언&인장 처리 (후열)
        /// 작성일자 : 2019-08-07
        /// </summary>
        /// <param name="_docID"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_opinion"></param>
        /// <param name="_sequenceInfo"></param>
        /// <param name="_reason"></param>
        /// <returns></returns>
        public string StampPermit(string _docID, string _approvalTag, string _opinion, SequenceInfo _sequenceInfo, string _reason)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.SetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                // 1. 문서 가져오기
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                sql = string.Format(sql, CompanyID.ToString(), _docID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCont", null);
                string tmpCont = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(tmpCont);

                HtmlNode body = doc.DocumentNode.SelectSingleNode("//body");

                string tmpDeptName = UserInfo.DeptName;
                string tmpTitleName = UserInfo.TitleName;
                string tmpDutyName = UserInfo.DutyName;
                string tmpUserName = UserInfo.Name;

                if ((_sequenceInfo.UserID.Equals(UserInfo.UserID)) && (!_sequenceInfo.DeptID.Equals(UserInfo.DeptID)))
                {
                    List<BE.AccountInfo> ainfo = GetAccountInfo(CompanyID, _sequenceInfo.UserID, _sequenceInfo.DeptID);

                    foreach (BE.AccountInfo info in ainfo)
                    {
                        tmpDeptName = info.DeptName;
                        tmpTitleName = info.TitleName;
                        tmpDutyName = info.DutyName;
                    }
                }

                // 2. 첨언처리
                if (_opinion.Trim() != "")
                {
                    string tmpOpinion = GetOpinionTag(_opinion, UserInfo.Name, tmpTitleName, tmpDeptName);
                    if (doc.DocumentNode.SelectNodes("//div[@id='OPINION']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//div[@id='OPINION']"))
                        {
                            span.InnerHtml = span.InnerHtml.Insert(span.InnerHtml.Length, tmpOpinion);
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//div[@id='OPINIONVIEW']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//div[@id='OPINIONVIEW']"))
                        {
                            span.InnerHtml = resMng.GetString("TagDic_OPINIONVIEW");
                        }
                    }
                }

                // 3. 결재 정보
                string todayObj = "", timeObj = "", monthDayObj = "", timeExObj = "";
                string brTag = " ";

                DateTime dt = System.DateTime.Now;
                todayObj = dt.ToShortDateString();
                timeObj = dt.ToShortDateString() + " " + dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0');
                monthDayObj = dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                timeExObj = dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0');


                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO0']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO0']"))
                    {
                        span.InnerHtml = todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO1']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO1']"))
                    {
                        span.InnerHtml = tmpTitleName + brTag + tmpUserName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO2']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO2']"))
                    {
                        span.InnerHtml = tmpTitleName + brTag + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO3']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO3']"))
                    {
                        span.InnerHtml = tmpUserName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO4']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO4']"))
                    {
                        span.InnerHtml = tmpTitleName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO5']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO5']"))
                    {
                        span.InnerHtml = tmpDeptName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO6']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO6']"))
                    {
                        span.InnerHtml = tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO7']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO7']"))
                    {
                        span.InnerHtml = tmpDeptName + brTag + tmpTitleName + brTag + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO8']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO8']"))
                    {
                        span.InnerHtml = timeObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO9']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO9']"))
                    {
                        span.InnerHtml = tmpDutyName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO10']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO10']"))
                    {
                        span.InnerHtml = tmpDutyName + brTag + tmpUserName;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO11']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO11']"))
                    {
                        span.InnerHtml = tmpDutyName + brTag + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO12']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO12']"))
                    {
                        span.InnerHtml = tmpDeptName + brTag + tmpDutyName + brTag + tmpUserName + brTag + todayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO13']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO13']"))
                    {
                        span.InnerHtml = tmpUserName + brTag + timeObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO14']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO14']"))
                    {
                        span.InnerHtml = monthDayObj;
                    }
                }

                if (doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO15']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "_INFO15']"))
                    {
                        span.InnerHtml = timeExObj;
                    }
                }

                // 4. 결재인장          
                foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + _approvalTag + "']"))
                {
                    span.InnerHtml = _reason;
                }

                return body.ParentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("StampPermit 후열(" + _docID + ")", ex);
                return "!NULL";
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 의견추가
        /// 작성일자 : 2017-01-16
        /// </summary>
        /// <param name="_docID"></param>
        /// <param name="_opinion"></param>
        /// <returns></returns>
        public string OpinionCont(string _docID, string _opinion, string _gubun)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.SetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string sql = "";
                if (_gubun == "C")
                {
                    sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocCompleteInfo");
                }
                else
                {
                    sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                }

                sql = string.Format(sql, CompanyID.ToString(), _docID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCont", null);
                string tmpCont = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(tmpCont);

                HtmlNode body = doc.DocumentNode.SelectSingleNode("//body");

                string tmpOpinion = GetAddOpinionTag(_opinion, UserInfo.Name, UserInfo.TitleName, UserInfo.DeptName);
                if (doc.DocumentNode.SelectNodes("//div[@id='OPINION']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//div[@id='OPINION']"))
                    {
                        span.InnerHtml = span.InnerHtml.Insert(span.InnerHtml.Length, tmpOpinion);
                    }
                }

                if (doc.DocumentNode.SelectNodes("//div[@id='OPINIONVIEW']") != null)
                {
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//div[@id='OPINIONVIEW']"))
                    {
                        span.InnerHtml = resMng.GetString("TagDic_OPINIONVIEW");
                    }
                }

                return body.ParentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("OpinionCont(" + _docID + ")", ex);
                return "!NULL";
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨언 스타일 
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <param name="_opinion"></param>
        /// <param name="_name"></param>
        /// <param name="_titleName"></param>
        /// <param name="_deptName"></param>
        /// <returns></returns>
        public static string GetOpinionTag(string _opinion, string _name, string _titleName, string _deptName)
        {
            var objTit = _name + "/" + _titleName + "/" + _deptName;

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='app_opi'>");
            sb.Append("<span class='tit'>[" + objTit + "]</span>");
            sb.Append("<span class='date'>- " + getTimeByServer() + "</span>");
            sb.Append("<div class='cont'>" + _opinion + "</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 의견 추가 
        /// 작성일자 : 2017-01-16
        /// </summary>
        /// <param name="_opinion"></param>
        /// <param name="_name"></param>
        /// <param name="_titleName"></param>
        /// <param name="_deptName"></param>
        /// <returns></returns>
        public static string GetAddOpinionTag(string _opinion, string _name, string _titleName, string _deptName)
        {
            var objTit = _name + "/" + _titleName + "/" + _deptName;

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='app_opi'>");
            sb.Append("<span class='tit'>[" + objTit + "]</span>");
            sb.Append("<span class='date'>- " + getTimeByServer() + "</span>");
            sb.Append("<span class='opt'> <b>(*)</b></span>");
            sb.Append("<div class='cont'>" + _opinion + "</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public static string getTimeByServer()
        {
            DateTime dt = System.DateTime.Now;
            return dt.ToShortDateString() + " " + dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0');
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 문서 내용 DB처리 
        /// 작성일자 : 2012-09-07
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_accountTag"></param>
        /// <param name="_contentHTML"></param>
        /// <returns></returns>
        public string UpdateHTML(int _companyID, int _documentID, int _groupID, string _accountTag, string _contentHTML)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)  
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)  
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar,20)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)                                               
                };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;
                parms[2].Value = _groupID;
                parms[3].Value = _accountTag;
                parms[4].Value = _contentHTML;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_CONTENT");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return "OK";
            }
            catch (SqlException ex)
            {
                AppException.WriteException("UpdateHTML(" + _documentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 승인 처리 DB처리 
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <param name="_sequenceInfo"></param>
        /// <param name="_contentHTML"></param>
        /// <param name="_accountTag"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public string ApproveS(SequenceInfo _sequenceInfo, string _contentHTML, string _accountTag, int groupID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)
                                               ,new SqlParameter("@ApprovalType", SqlDbType.Char,1)
                                               ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@Sequence", SqlDbType.Int)
                                               ,new SqlParameter("@Proxy", SqlDbType.Char,1)
                                               ,new SqlParameter("@ProxyUserID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@Opinion", SqlDbType.NVarChar, 500)
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)       
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)
                };

                parms[0].Value = _sequenceInfo.CompanyID;
                parms[1].Value = _sequenceInfo.DocumentID;
                parms[2].Value = _sequenceInfo.UserID;
                parms[3].Value = _sequenceInfo.DeptID;
                parms[4].Value = _sequenceInfo.ApprovalType;
                parms[5].Value = _sequenceInfo.ApprovalTag;
                parms[6].Value = _sequenceInfo.Sequence;
                parms[7].Value = _sequenceInfo.Proxy;
                parms[8].Value = _sequenceInfo.ProxyUserID;
                parms[9].Value = _sequenceInfo.Opinion;
                parms[10].Value = _accountTag;
                parms[11].Value = _contentHTML;
                parms[12].Value = groupID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_APPROVES");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsApproveS", parms);

                return ds.Tables["dsApproveS"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("ApproveS(" + CompanyID.ToString() + "," + _sequenceInfo.DocumentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전결 처리 DB처리 
        /// 작성일자 : 2012-09-17
        /// </summary>
        /// <param name="_sequenceInfo"></param>
        /// <param name="_contentHTML"></param>
        /// <param name="_accountTag"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public string ApproveA(SequenceInfo _sequenceInfo, string _contentHTML, string _accountTag, int groupID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)
                                               ,new SqlParameter("@ApprovalType", SqlDbType.Char,1)
                                               ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@Sequence", SqlDbType.Int)
                                               ,new SqlParameter("@Proxy", SqlDbType.Char,1)
                                               ,new SqlParameter("@ProxyUserID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@Opinion", SqlDbType.NVarChar, 500)
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)       
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)
                };

                parms[0].Value = _sequenceInfo.CompanyID;
                parms[1].Value = _sequenceInfo.DocumentID;
                parms[2].Value = _sequenceInfo.UserID;
                parms[3].Value = _sequenceInfo.DeptID;
                parms[4].Value = _sequenceInfo.ApprovalType;
                parms[5].Value = _sequenceInfo.ApprovalTag;
                parms[6].Value = _sequenceInfo.Sequence;
                parms[7].Value = _sequenceInfo.Proxy;
                parms[8].Value = _sequenceInfo.ProxyUserID;
                parms[9].Value = _sequenceInfo.Opinion;
                parms[10].Value = _accountTag;
                parms[11].Value = _contentHTML;
                parms[12].Value = groupID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_APPROVEA");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsApproveA", parms);

                return ds.Tables["dsApproveA"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("ApproveA(" + CompanyID.ToString() + "," + _sequenceInfo.DocumentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 합의, 합의 불가 DB처리 
        /// 작성일자 : 2012-09-20
        /// </summary>
        /// <param name="_sequenceInfo"></param>
        /// <param name="_contentHTML"></param>
        /// <param name="_accountTag"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public string ApproveH(SequenceInfo _sequenceInfo, string _contentHTML, string _accountTag, int groupID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)
                                               ,new SqlParameter("@ApprovalType", SqlDbType.Char,1)
                                               ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@Sequence", SqlDbType.Int)
                                               ,new SqlParameter("@Proxy", SqlDbType.Char,1)
                                               ,new SqlParameter("@ProxyUserID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@Opinion", SqlDbType.NVarChar, 500)
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)       
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)
                };

                parms[0].Value = _sequenceInfo.CompanyID;
                parms[1].Value = _sequenceInfo.DocumentID;
                parms[2].Value = _sequenceInfo.UserID;
                parms[3].Value = _sequenceInfo.DeptID;
                parms[4].Value = _sequenceInfo.ApprovalType;
                parms[5].Value = _sequenceInfo.ApprovalTag;
                parms[6].Value = _sequenceInfo.Sequence;
                parms[7].Value = _sequenceInfo.Proxy;
                parms[8].Value = _sequenceInfo.ProxyUserID;
                parms[9].Value = _sequenceInfo.Opinion;
                parms[10].Value = _accountTag;
                parms[11].Value = _contentHTML;
                parms[12].Value = groupID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_APPROVEH");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsApproveH", parms);

                return ds.Tables["dsApproveH"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("ApproveH(" + CompanyID.ToString() + "," + _sequenceInfo.DocumentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재취소 DB처리 
        /// 작성일자 : 2012-09-21
        /// </summary>
        /// <param name="_sequenceInfo"></param>
        /// <param name="_contentHTML"></param>
        /// <param name="_approvalState"></param>
        /// <param name="_accountTag"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public string Cancel(SequenceInfo _sequenceInfo, string _contentHTML, string _accountTag, int _groupID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)                                              
                                               ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@Sequence", SqlDbType.Int)                                            
                                               ,new SqlParameter("@Opinion", SqlDbType.NVarChar, 500)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar, 20)  
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)
                };

                parms[0].Value = _sequenceInfo.CompanyID;
                parms[1].Value = _sequenceInfo.DocumentID;
                parms[2].Value = _sequenceInfo.ApprovalTag;
                parms[3].Value = _sequenceInfo.Sequence;
                parms[4].Value = _sequenceInfo.Opinion;
                parms[5].Value = _contentHTML;
                parms[6].Value = _accountTag;
                parms[7].Value = _groupID;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_CANCEL");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return "0";
            }
            catch (SqlException ex)
            {
                AppException.WriteException("Cancel(" + _sequenceInfo.DocumentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 반송 DB처리 
        /// 작성일자 : 2012-09-26
        /// </summary>
        /// <param name="_sequenceInfo"></param>
        /// <param name="_contentHTML"></param>
        /// <param name="_accountTag"></param>
        /// <param name="groupID"></param>
        /// <param name="_dID"></param>
        /// <param name="_targetSeq"></param>
        /// <param name="_targetApprovalTag"></param>
        /// <returns></returns>
        public string Return(SequenceInfo _sequenceInfo, string _contentHTML, string _accountTag, int groupID, int _dID, int _targetSeq, string _targetApprovalTag, string _isTargetZD)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)
                                               ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@Sequence", SqlDbType.Int)
                                               ,new SqlParameter("@Proxy", SqlDbType.Char,1)
                                               ,new SqlParameter("@ProxyUserID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@Opinion", SqlDbType.NVarChar, 500)
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)       
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)
                                               ,new SqlParameter("@TargetDID", SqlDbType.Int)
                                               ,new SqlParameter("@TargetSeq", SqlDbType.Int)
                                               ,new SqlParameter("@TargetApprovalTag", SqlDbType.VarChar,20)
                                               ,new SqlParameter("@IsTargetZD", SqlDbType.Char,1)
                                               
                };

                parms[0].Value = _sequenceInfo.CompanyID;
                parms[1].Value = _sequenceInfo.DocumentID;
                parms[2].Value = _sequenceInfo.UserID;
                parms[3].Value = _sequenceInfo.DeptID;
                parms[4].Value = _sequenceInfo.ApprovalTag;
                parms[5].Value = _sequenceInfo.Sequence;
                parms[6].Value = _sequenceInfo.Proxy;
                parms[7].Value = _sequenceInfo.ProxyUserID;
                parms[8].Value = _sequenceInfo.Opinion;
                parms[9].Value = _accountTag;
                parms[10].Value = _contentHTML;
                parms[11].Value = groupID;
                parms[12].Value = _dID;
                parms[13].Value = _targetSeq;
                parms[14].Value = _targetApprovalTag;
                parms[15].Value = _isTargetZD;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_RETURN");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return "0";
            }
            catch (SqlException ex)
            {
                AppException.WriteException("Return(" + _sequenceInfo.DocumentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 결재취소
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_nextMyturn"></param>
        /// <param name="_nextApprovalTag"></param>
        /// <param name="_nextSequence"></param>
        /// <returns></returns>
        public string CancelApproval(int _companyID, int _documentID, int _groupID, int _sequence, string _approvalTag, string _nextMyturn, string _nextApprovalTag, int _nextSequence
            , string _accountTag, string _approvalState, string _contentHTML)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)                                              
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)
                                               ,new SqlParameter("@Sequence", SqlDbType.Int)
                                               ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)                                            
                                               ,new SqlParameter("@NextMyturn", SqlDbType.Char, 1)
                                               ,new SqlParameter("@NextApprovalTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@NextSequence", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@ApprovalState", SqlDbType.Char, 1)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)                                               
                };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;
                parms[2].Value = _groupID;
                parms[3].Value = _sequence;
                parms[4].Value = _approvalTag;
                parms[5].Value = _nextMyturn;
                parms[6].Value = _nextApprovalTag;
                parms[7].Value = _nextSequence;
                parms[8].Value = _accountTag;
                parms[9].Value = _approvalState;
                parms[10].Value = _contentHTML;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_CancelApproval");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return "0";
            }
            catch (SqlException ex)
            {
                AppException.WriteException("CancelApproval(" + _documentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 후열처리 DB처리 
        /// 작성일자 : 2019-08-07
        /// </summary>
        /// <param name="_sequenceInfo"></param>
        /// <param name="_contentHTML"></param>
        /// <param name="_accountTag"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public string ApproveL(SequenceInfo _sequenceInfo, string _contentHTML, string _accountTag, int groupID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)
                                               ,new SqlParameter("@ApprovalType", SqlDbType.Char,1)
                                               ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@Sequence", SqlDbType.Int)
                                               ,new SqlParameter("@Proxy", SqlDbType.Char,1)
                                               ,new SqlParameter("@ProxyUserID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@Opinion", SqlDbType.NVarChar, 500)
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar, 20)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)       
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)
                };

                parms[0].Value = _sequenceInfo.CompanyID;
                parms[1].Value = _sequenceInfo.DocumentID;
                parms[2].Value = _sequenceInfo.UserID;
                parms[3].Value = _sequenceInfo.DeptID;
                parms[4].Value = _sequenceInfo.ApprovalType;
                parms[5].Value = _sequenceInfo.ApprovalTag;
                parms[6].Value = _sequenceInfo.Sequence;
                parms[7].Value = _sequenceInfo.Proxy;
                parms[8].Value = _sequenceInfo.ProxyUserID;
                parms[9].Value = _sequenceInfo.Opinion;
                parms[10].Value = _accountTag;
                parms[11].Value = _contentHTML;
                parms[12].Value = groupID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_APPROVEL");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsApproveL", parms);

                return ds.Tables["dsApproveL"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("ApproveS(" + CompanyID.ToString() + "," + _sequenceInfo.DocumentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재 의견 추가
        /// 작성일자 : 2017-01-17
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_accountTag"></param>
        /// <param name="_contentHTML"></param>
        /// <param name="_opinion"></param>
        /// <param name="_userID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public string AddOpinion(int _companyID, int _documentID, int _groupID, string _accountTag, string _contentHTML, string _opinion, int _userID, string _gubun)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)  
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)  
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar,20)
                                               ,new SqlParameter("@ContentHTML", SqlDbType.NText)                                               
                                               ,new SqlParameter("@Opinion", SqlDbType.NVarChar,4000)    
                                               ,new SqlParameter("@UserID", SqlDbType.Int)    
                };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;
                parms[2].Value = _groupID;
                parms[3].Value = _accountTag;
                parms[4].Value = _contentHTML;
                parms[5].Value = _opinion;
                parms[6].Value = _userID;

                string sqlQuery = "";

                if (_gubun == "C")
                {
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_DocOpinion");
                }
                else
                {
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPT_PROC_DocProcessOpinion");
                }

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return "OK";
            }
            catch (SqlException ex)
            {
                AppException.WriteException("AddOpinion(" + _documentID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 문서상태변경 (상신보류, 상신보류취소)
        /// 작성일자 : 2019-08-06
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_accountTag"></param>
        /// <param name="_approvalState"></param>
        /// <returns></returns>
        public string ModApprovalState(int _companyID, int _documentID, int _groupID, string _accountTag, string _approvalState)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@DocumentID", SqlDbType.Int)  
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)  
                                               ,new SqlParameter("@AccountTag", SqlDbType.VarChar,20)
                                               ,new SqlParameter("@ApprovalState", SqlDbType.Char,1)                                               
                };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;
                parms[2].Value = _groupID;
                parms[3].Value = _accountTag;
                parms[4].Value = _approvalState;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_AppState");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return "OK";
            }
            catch (SqlException ex)
            {
                AppException.WriteException("ApproveHCancel(" + _documentID + ")", ex);
                return "!NULL";
            }
        }


        #region 알람 - 알람대상

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 메신저 알람발송
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_section"></param>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        private List<BE.AccountInfo> GetAlarmTarget(string _section, int _companyID, int _documentID, int _groupID, int _sequence, string _accountTag)
        {
            if (_section.Equals("") || _companyID.Equals(0) || _documentID.Equals(0)) return null;

            try
            {
                List<BE.AccountInfo> info = null;

                switch (_section.ToUpper())
                {
                    case "OPINION":                         // 이전 기결재자
                        // 결재의견 알림
                        info = GetAlarmOpinion(_companyID, _documentID, _groupID, _sequence);
                        break;
                    case "APPROVE":                         // 다음결재자 (@Sequence + Myturn='Y')
                        // 승인(+상신)
                        info = GetAlarmApprove(_companyID, _documentID, _groupID, _sequence);
                        break;
                    case "REJECT":                          // 반려대상자
                        info = GetAlarmReject(_companyID, _documentID, _groupID);
                        // 반려
                        break;
                    case "COMPLETE":                        // 결재라인+참조+수신(ZP)
                        // 완결
                        info = GetAlarmComplete(_companyID, _documentID, _groupID, _accountTag, _sequence);
                        break;
                    case "OPINIONALL":
                        // 결재의견 추가
                        info = GetAlarmOpinionAll(_companyID, _documentID, _accountTag);
                        break;
                    case "APPROVEC":
                        // 결재의견 추가
                        info = GetAlarmApproveC(_companyID, _documentID, _groupID, _sequence);
                        break;
                }

                return info;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmTarget('" + _documentID + "')", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재의견 추가
        /// 작성일자 : 2017-01-17
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_accountTag"></param>
        /// <returns></returns>
        public List<BE.AccountInfo> GetAlarmOpinionAll(int _companyID, int _documentID, string _accountTag)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@AccountTag", SqlDbType.VarChar,20)
                    };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = UserInfo.UserID;
                parm[3].Value = _accountTag;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ALARMOPINIONALL");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAlarmOpinion", parm);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new AccountInfo();
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

                    // 개인알람정보추가 2017-04-14
                    info.PreferredLanguage = dr["PreLang"].ToString();
                    info.PreLang = dr["PreLang"].ToString();
                    info.AlarmApproval = dr["AlarmApproval"].ToString();
                    if (info.AlarmApproval.IndexOf("1-").Equals(0))
                    {
                        info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                        info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                        info.AlarmMobile = (info.AlarmApproval.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                        info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                    }
                    else
                    {
                        info.AlarmMessenger = "0";
                        info.AlarmMail = "0";
                        info.AlarmMobile = "0";
                        info.AlarmSMS = "0";
                    }

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmOpinionAll('" + _documentID + "')", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 의견첨언
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.AccountInfo> GetAlarmOpinion(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@GroupID", SqlDbType.Int)
                                            ,new SqlParameter("@Sequence", SqlDbType.Int)
                    };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = _groupID;
                parm[3].Value = _sequence;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ALARMOPINION");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAlarmOpinion", parm);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new AccountInfo();
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

                    // 개인알람정보추가 2017-04-14
                    info.PreferredLanguage = dr["PreLang"].ToString();
                    info.PreLang = dr["PreLang"].ToString();
                    info.AlarmApproval = dr["AlarmApproval"].ToString();
                    if (info.AlarmApproval.IndexOf("1-").Equals(0))
                    {
                        info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                        info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                        info.AlarmMobile = (info.AlarmApproval.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                        info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                    }
                    else
                    {
                        info.AlarmMessenger = "0";
                        info.AlarmMail = "0";
                        info.AlarmMobile = "0";
                        info.AlarmSMS = "0";
                    }

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmOpinion('" + _documentID + "')", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 다음결재자 (@Sequence + Myturn='Y')
        /// 작성일자 : 2012-09-10
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
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@GroupID", SqlDbType.Int)
                                            ,new SqlParameter("@Sequence", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = _groupID;
                parm[3].Value = _sequence;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ALARMAPPROVE");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAlarmApprove", parm);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new AccountInfo();
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
                        info.AlarmMobile = (info.AlarmApproval.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                        info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                    }
                    else
                    {
                        info.AlarmMessenger = "0";
                        info.AlarmMail = "0";
                        info.AlarmMobile = "0";
                        info.AlarmSMS = "0";
                    }

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmApprove('" + _documentID + "')", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 반려대상자
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public List<BE.AccountInfo> GetAlarmReject(int _companyID, int _documentID, int _groupID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@GroupID", SqlDbType.Int)                                            
                };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = _groupID;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ALARMREJECT");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAlarmReject", parm);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new AccountInfo();
                    info.AlarmType = dr["AlarmType"].ToString().ToNull();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.Name = dr["NameBase"].ToString();

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
                        info.AlarmMobile = (info.AlarmApproval.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                        info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                    }
                    else
                    {
                        info.AlarmMessenger = "0";
                        info.AlarmMail = "0";
                        info.AlarmMobile = "0";
                        info.AlarmSMS = "0";
                    }

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmReject('" + _documentID + "')", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재라인+참조+수신(ZP)
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_accountTag"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.AccountInfo> GetAlarmComplete(int _companyID, int _documentID, int _groupID, string _accountTag, int _sequence)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@GroupID", SqlDbType.Int)
                                            ,new SqlParameter("@AccountTag", SqlDbType.VarChar, 20)
                                            ,new SqlParameter("@Sequence", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = _groupID;
                parm[3].Value = _accountTag;
                parm[4].Value = _sequence;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ALARMCOMPLETE");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAlarmComplete", parm);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new AccountInfo();
                    info.AlarmType = dr["AlarmType"].ToString().ToNull();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.Name = dr["NameBase"].ToString();

                    if (!dr["MailID"].ToString().Equals("") && !dr["MailDomain"].ToString().Equals(""))
                    {
                        info.Mail = dr["MailID"].ToString() + "@" + dr["MailDomain"].ToString();
                    }
                    else
                    {
                        info.Mail = "";
                    }
                    info.MobilePhone = dr["MobilePhone"].ToString().ToNull();

                    // 개인알람정보추가 2017-04-14
                    info.PreferredLanguage = dr["PreLang"].ToString();
                    info.PreLang = dr["PreLang"].ToString();
                    info.AlarmApproval = dr["AlarmApproval"].ToString();
                    if (info.AlarmApproval.IndexOf("1-").Equals(0))
                    {
                        info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                        info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                        info.AlarmMobile = (info.AlarmApproval.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                        info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                    }
                    else
                    {
                        info.AlarmMessenger = "0";
                        info.AlarmMail = "0";
                        info.AlarmMobile = "0";
                        info.AlarmSMS = "0";
                    }

                    list.Add(info);
                }

                return list;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmComplete('" + _documentID + "')", ex);
                return null;
            }
        }

        /// <summary>
        /// 결재취소
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public List<BE.AccountInfo> GetAlarmApproveC(int _companyID, int _documentID, int _groupID, int _sequence)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@GroupID", SqlDbType.Int)
                                            ,new SqlParameter("@Sequence", SqlDbType.Int)
                    };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;
                parm[2].Value = _groupID;
                parm[3].Value = _sequence;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ALARMAPPROVECNEXT");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAlarmOpinion", parm);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new AccountInfo();
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

                    // 개인알람정보추가 2017-04-14
                    info.PreferredLanguage = dr["PreLang"].ToString();
                    info.PreLang = dr["PreLang"].ToString();
                    info.AlarmApproval = dr["AlarmApproval"].ToString();
                    if (info.AlarmApproval.IndexOf("1-").Equals(0))
                    {
                        info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                        info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                        info.AlarmMobile = (info.AlarmApproval.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                        info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                    }
                    else
                    {
                        info.AlarmMessenger = "0";
                        info.AlarmMail = "0";
                        info.AlarmMobile = "0";
                        info.AlarmSMS = "0";
                    }

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmOpinionAll('" + _documentID + "')", ex);
                return null;
            }
        }

        #endregion

        #region 알람 - 알람발송

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 알람
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_documentID"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_state"></param>
        private void SendAlarm(int _documentID, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _state, DataView _dv)
        {
            try
            {
                if (_sendUserInfo == null || _receiveUserlist == null || _dv == null) return;
                if (_dv.Count.Equals(0)) return;
                if (_receiveUserlist.Count.Equals(0)) return;

                string xmlPath = Server.MapPath("/DeskPlusMobile/XmlData/Confirm/" + PreLang + ".xml");
                XmlDocument contXml = new XmlDocument();
                contXml.Load(xmlPath);
                XmlNode rootNode = contXml.DocumentElement;
                XmlNode subNode = null;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SelectDoc");
                sql = string.Format(sql, CompanyID.ToString(), _documentID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsAlarm", null);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                BE.DocInfo docInfo = new DocInfo();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    docInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    docInfo.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                    docInfo.Number3 = dr["Number3"].ToString().ToNull();
                    docInfo.Subject = dr["Subject"].ToString().ToReverseXSS();
                    docInfo.DeptBase = dr["DeptBase"].ToString().ToNull();
                    docInfo.TitleBase = dr["TitleBase"].ToString().ToNull();
                    docInfo.DutyBase = dr["DutyBase"].ToString().ToNull();
                    docInfo.NameBase = dr["NameBase"].ToString().ToNull();
                    docInfo.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                    docInfo.UserID = dr["UserID"].ToString().ToNumber<int>();
                    docInfo.GroupID = dr["GroupID"].ToString().ToNumber<int>();
                    docInfo.AccountTag = dr["AccountTag"].ToString().ToNull();
                }

                foreach (DataRowView dr in _dv)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            subNode = rootNode.SelectSingleNode("/ALARM/MAIL");
                            SendMail(subNode, docInfo, _sendUserInfo, _receiveUserlist, _state, method);
                            break;
                        case "MESSENGER":
                            subNode = rootNode.SelectSingleNode("/ALARM/MESSENGER");
                            SendMessenger(subNode, docInfo, _sendUserInfo, _receiveUserlist, _state, method);
                            break;
                        case "SMS":
                            subNode = rootNode.SelectSingleNode("/ALARM/SMS");
                            SendSMS(subNode, docInfo, _sendUserInfo, _receiveUserlist, method);
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력
                subNode = rootNode.SelectSingleNode("/ALARM/MOBILE");
                SendMobile(subNode, docInfo, _sendUserInfo, _receiveUserlist, _state, "MOBILE");

                // 결재의견 알람                    
                if (!_sendUserInfo.Opinion.Equals("") && _state != "O")
                {
                    List<BE.AccountInfo> targetList = null;

                    targetList = GetAlarmTarget("OPINIONAll", CompanyID, docInfo.DocumentID, docInfo.GroupID, 0, docInfo.AccountTag);

                    //메신저알람 활성화일 경우 첨언 메일발송
                    foreach (DataRowView dr in _dv)
                    {
                        string method = dr["CodeID"].ToString().ToUpper();
                        switch (method)
                        {
                            case "MESSENGER":
                                subNode = rootNode.SelectSingleNode("/ALARM/OPINION");
                                SendMailOpinion(subNode, docInfo, _sendUserInfo, targetList, _state, "OPINION");
                                break;

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SendAlarm('" + _documentID + "')", ex);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 메일
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_urlSection"></param>
        private void SendMail(XmlNode _contNode, BE.DocInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMail(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;

            StringBuilder sbCC = new StringBuilder();
            int cnt = 0; string sendMail = "";
            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                if (info.AlarmMail.Equals("1"))
                {
                    if (info.AlarmType.Equals("CARBON"))
                    {
                        if (cnt.Equals(0))
                        {
                            sendMail = info.Mail;
                        }
                        else
                        {
                            sbCC.Append(info.Mail + ";");
                        }
                        cnt++;
                    }
                    else
                    {
                        // 참조 외, 메일처리
                        arrCont = GetAlarmCont(info.AlarmType, _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);      // 알람내용 가져오기
                        if (arrCont == null) return;

                        // 보낸사람 이름이 받는사람 언어로 나오게
                        string sql2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ACC_SelByUserID");
                        sql2 = string.Format(sql2, _dpInfo.CompanyID, _sendUserInfo.UserID);
                        DataSet dsSend = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql2, "dsSendUserInfo", null);

                        foreach (DataRow dr in dsSend.Tables[0].Rows)
                        {
                            //dr["DeptID"] : 인사부서
                            //UserInfo.DeptID : 결재자 부서
                            if (dr["DeptID"].ToString().ToNumber<int>() == UserInfo.DeptID)
                            {
                                string NameBase = this.GetLangName(
                                                dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                                dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                                dr["NameFr"].ToString().ToNull(), dr["NameZhCn"].ToString().ToNull(),
                                                dr["NameZhTw"].ToString().ToNull(), info.PreLang);

                                _sendUserInfo.Name = NameBase.ToNull();
                                break;
                            }
                        }

                        CreateMHT(_sendUserInfo.Mail, _sendUserInfo.Name, info.Mail, "", "", arrCont[0], arrCont[1]);
                    }
                }
            }

            // 참조의 경우, 묶음처리 (참조의 경우, 발송내용이 다름)
            if (!sbCC.ToString().TrimEnd(';').Equals("") || !sendMail.Equals(""))
            {
                // 알람내용 가져오기
                arrCont = GetAlarmCont("CARBON", _contNode, _dpInfo, _sendUserInfo, linkURL, _sendUserInfo.PreLang, _alarmMethod);
                if (arrCont == null) return;

                CreateMHT(_sendUserInfo.Mail, _sendUserInfo.Name, sendMail, sbCC.ToString().TrimEnd(';'), "", arrCont[0], arrCont[1]);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 메일
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_urlSection"></param>
        private void SendMailOpinion(XmlNode _contNode, BE.DocInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMail(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;

            StringBuilder sbCC = new StringBuilder();
            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                // 참조 외, 메일처리
                arrCont = GetAlarmCont(info.AlarmType, _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);      // 알람내용 가져오기
                if (arrCont == null) return;

                // 보낸사람 이름이 받는사람 언어로 나오게
                string sql2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ACC_SelByUserID");
                sql2 = string.Format(sql2, _dpInfo.CompanyID, _sendUserInfo.UserID);
                DataSet dsSend = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql2, "dsSendUserInfo", null);

                foreach (DataRow dr in dsSend.Tables[0].Rows)
                {
                    //dr["DeptID"] : 인사부서
                    //UserInfo.DeptID : 결재자 부서
                    if (dr["DeptID"].ToString().ToNumber<int>() == UserInfo.DeptID)
                    {
                        string NameBase = this.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhCn"].ToString().ToNull(),
                                        dr["NameZhTw"].ToString().ToNull(), info.PreLang);

                        _sendUserInfo.Name = NameBase.ToNull();
                        break;
                    }
                }

                CreateMHT(_sendUserInfo.Mail, _sendUserInfo.Name, info.Mail, "", "", arrCont[0], arrCont[1]);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 메신저
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_urlSection"></param>
        private void SendMessenger(XmlNode _contNode, BE.DocInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;
            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                if (info.AlarmMessenger.Equals("1"))
                {
                    linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);

                    // 결재대상(결재/수신결재)의 경우, 알람URL 재 처리
                    if (_urlSection.ToUpper().Equals("W") || _urlSection.ToUpper().Equals("S"))
                    {
                        if (info.AlarmType.ToUpper().Equals("RECEIVED"))
                        {
                            linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, "SR");
                        }
                        else
                        {
                            linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL);
                        }
                        if (linkURL.Equals("")) return;
                    }

                    if (_urlSection.ToUpper().Equals("C") && info.AlarmType.ToUpper().Equals("RECEIVED"))
                    {
                        linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, "SR");
                    }

                    // 알람내용 가져오기
                    arrCont = GetAlarmCont(info.AlarmType, _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);
                    if (arrCont == null) return;

                    Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                    alarmInfo.CompanyID = _dpInfo.CompanyID;
                    alarmInfo.SenderID = _sendUserInfo.UserID;
                    alarmInfo.SenderName = _sendUserInfo.Name;
                    alarmInfo.SenderMail = "";  //_sendUserInfo.Mail
                    alarmInfo.SenderPhone = ""; //_sendUserInfo.MobilePhone
                    alarmInfo.ReceiverID = info.UserID;
                    alarmInfo.ReceiverName = "";
                    alarmInfo.ReceiverMail = "";
                    alarmInfo.ReceiverPhone = "";
                    alarmInfo.Status = "";
                    alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.Subject = arrCont[0];
                    alarmInfo.Content = arrCont[1];
                    alarmInfo.TargetUrl = linkURL;
                    alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.WriteDate = System.DateTime.Now;
                    alarmInfo.AlarmType = 1;        // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                    alarmInfo.KindSubject = "Approval";
                    alarmInfo.ViewOption = "01";
                    alarmInfo.Store = 1;
                    alarmInfo.UserState = 0;

                    Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
                    alarm.Add(alarmInfo);
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : SMS
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        private void SendSMS(XmlNode _contNode, BE.DocInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _alarmMethod)
        {
        }

        /// <summary>
        /// 모바일(Push)
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_urlSection"></param>
        private void SendMobile(XmlNode _contNode, BE.DocInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;
            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID);

                // 결재대상(결재/수신결재)의 경우, 알람URL 재 처리
                if (_urlSection.ToUpper().Equals("W") || _urlSection.ToUpper().Equals("S"))
                {
                    if (info.AlarmType.ToUpper().Equals("RECEIVED"))
                    {
                        //_urlSection : SR 
                        linkURL = "";
                    }
                    else
                    {
                        linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL);
                    }
                    if (linkURL.Equals("")) return;
                }

                //중간결재자한테 반려
                if (_urlSection.ToUpper().Equals("RS"))
                {
                    linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL);
                }

                if (_urlSection.ToUpper().Equals("C") && info.AlarmType.ToUpper().Equals("RECEIVED"))
                {
                    //_urlSection : SR 
                    linkURL = "";
                }

                // 알람내용 가져오기
                arrCont = GetAlarmCont(info.AlarmType, _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);
                if (arrCont == null) return;

                Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                alarmInfo.CompanyID = _dpInfo.CompanyID;
                alarmInfo.SenderID = _sendUserInfo.UserID;
                alarmInfo.SenderName = _sendUserInfo.Name;
                alarmInfo.SenderMail = "";  //_sendUserInfo.Mail
                alarmInfo.SenderPhone = ""; //_sendUserInfo.MobilePhone
                alarmInfo.ReceiverID = info.UserID;
                alarmInfo.ReceiverName = "";
                alarmInfo.ReceiverMail = "";
                alarmInfo.ReceiverPhone = "";
                alarmInfo.Status = "";
                alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.Subject = arrCont[0];
                alarmInfo.Content = arrCont[1];
                alarmInfo.TargetUrl = "";
                alarmInfo.TargetMobileUrl = linkURL;
                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.WriteDate = System.DateTime.Now;
                alarmInfo.AlarmType = 4;        // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                alarmInfo.KindSubject = "Approval";
                alarmInfo.ViewOption = "01";
                alarmInfo.Store = 1;
                alarmInfo.UserState = 0;

                Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
                alarm.Add(alarmInfo);
            }
        }

        #endregion


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 알람내용(제목,내용)
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_alarmType"></param>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_linkURL"></param>
        /// <returns></returns>
        private string[] GetAlarmCont(string _alarmType, XmlNode _contNode, BE.DocInfo _dpInfo, BE.AccountInfo _sendUserInfo, string _linkURL, string _preLang, string _alarmMethod)
        {
            if (_preLang.Equals("")) _preLang = "ko";

            string xmlPath = Server.MapPath("/DeskPlusMobile/XmlData/Confirm/" + _preLang + ".xml");
            XmlDocument contXml = new XmlDocument();
            contXml.Load(xmlPath);
            XmlNode rootNode = contXml.DocumentElement;
            XmlNode subNode = null;

            switch (_alarmMethod)
            {
                case "OPINION":
                    subNode = rootNode.SelectSingleNode("/ALARM/OPINION");
                    break;

                case "MAIL":
                    subNode = rootNode.SelectSingleNode("/ALARM/MAIL");
                    break;

                case "MESSENGER":
                    subNode = rootNode.SelectSingleNode("/ALARM/MESSENGER");
                    break;

                case "SMS":
                    subNode = rootNode.SelectSingleNode("/ALARM/SMS");
                    break;

                case "MOBILE":
                    subNode = rootNode.SelectSingleNode("/ALARM/MOBILE");
                    break;
            }

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ACC_SelByUserID");
            sql = string.Format(sql, _dpInfo.CompanyID, _dpInfo.UserID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsUserInfo", null);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //dr["DeptID"] : 인사부서
                //_dpInfo.DeptID : 상신자 부서
                if (dr["DeptID"].ToString().ToNumber<int>() == _dpInfo.DeptID)
                {
                    string DeptBase = this.GetLangName(
                                        dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                        dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                        dr["DeptFr"].ToString().ToNull(), dr["DeptZhCn"].ToString().ToNull(),
                                        dr["DeptZhTw"].ToString().ToNull(), _preLang);

                    string TitleBase = this.GetLangName(
                                        dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                        dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                        dr["TitleFr"].ToString().ToNull(), dr["TitleZhCn"].ToString().ToNull(),
                                        dr["TitleZhTw"].ToString().ToNull(), _preLang);

                    string NameBase = this.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhCn"].ToString().ToNull(),
                                        dr["NameZhTw"].ToString().ToNull(), _preLang);

                    _dpInfo.DeptBase = DeptBase.ToNull();
                    _dpInfo.TitleBase = TitleBase.ToNull();
                    _dpInfo.NameBase = NameBase.ToNull();
                }
            }

            string sql2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ACC_SelByUserID");
            sql2 = string.Format(sql2, _dpInfo.CompanyID, _sendUserInfo.UserID);
            DataSet dsSend = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql2, "dsSendUserInfo", null);

            foreach (DataRow dr in dsSend.Tables[0].Rows)
            {
                //dr["DeptID"] : 인사부서
                //UserInfo.DeptID : 결재자 부서
                if (UserInfo.DeptID == 0) //상신일때
                {
                    _sendUserInfo.DeptName = _dpInfo.DeptBase.ToNull();
                    _sendUserInfo.TitleName = _dpInfo.TitleBase.ToNull();
                    _sendUserInfo.DutyName = _dpInfo.DutyBase.ToNull();
                    _sendUserInfo.Name = _dpInfo.NameBase.ToNull();
                }
                else
                {
                    if (dr["DeptID"].ToString().ToNumber<int>() == UserInfo.DeptID)
                    {
                        string DeptBase = this.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhCn"].ToString().ToNull(),
                                    dr["DeptZhTw"].ToString().ToNull(), _preLang);

                        string TitleBase = this.GetLangName(
                                        dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                        dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                        dr["TitleFr"].ToString().ToNull(), dr["TitleZhCn"].ToString().ToNull(),
                                        dr["TitleZhTw"].ToString().ToNull(), _preLang);

                        string DutyBase = this.GetLangName(
                                        dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                        dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                        dr["DutyFr"].ToString().ToNull(), dr["DutyZhCn"].ToString().ToNull(),
                                        dr["DutyZhTw"].ToString().ToNull(), _preLang);

                        string NameBase = this.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhCn"].ToString().ToNull(),
                                        dr["NameZhTw"].ToString().ToNull(), _preLang);

                        _sendUserInfo.DeptName = DeptBase.ToNull();
                        _sendUserInfo.TitleName = TitleBase.ToNull();
                        _sendUserInfo.DutyName = DutyBase.ToNull();
                        _sendUserInfo.Name = NameBase.ToNull();
                        break;
                    }
                }
            }


            if (_alarmType.Equals("") || subNode == null || _dpInfo == null || _sendUserInfo == null) return null;

            try
            {
                string[] arrRtnval = new string[2];

                string action = subNode.SelectSingleNode("SECTION/" + _alarmType).InnerText;
                arrRtnval[0] = subNode.SelectSingleNode("SUBJ/" + _alarmType).InnerText + _dpInfo.Subject;
                arrRtnval[1] = subNode.SelectSingleNode("CONTENT").InnerText;

                if (_alarmType == "OPINION")
                {
                    arrRtnval[0] = subNode.SelectSingleNode("SUBJ/" + _alarmType).InnerText + " [" + _dpInfo.Number3 + "] " + _dpInfo.Subject + " ( 상신자:" + _dpInfo.NameBase + " ) ";
                }

                arrRtnval[1] = arrRtnval[1].Replace("$ACTION$", action);                    //Message
                arrRtnval[1] = arrRtnval[1].Replace("$SUBJECT$", arrRtnval[0]);             //문서제목
                arrRtnval[1] = arrRtnval[1].Replace("$NUMBER3$", _dpInfo.Number3);          //양식명
                arrRtnval[1] = arrRtnval[1].Replace("$ACCDEPTNAME$", _dpInfo.DeptBase);     //상신자부서명  $ACCDEPTNAME$ / $ACCTITLENAME$ / $ACCUSERNAME$
                arrRtnval[1] = arrRtnval[1].Replace("$ACCTITLENAME$", _dpInfo.TitleBase);   //상신자직급명
                arrRtnval[1] = arrRtnval[1].Replace("$ACCUSERNAME$", _dpInfo.NameBase);     //상신자이름
                arrRtnval[1] = arrRtnval[1].Replace("$USERDEPTNAME$", _sendUserInfo.DeptName);      //결재자부서명
                arrRtnval[1] = arrRtnval[1].Replace("$USERTITLENAME$", _sendUserInfo.TitleName);    //결재자직급명
                arrRtnval[1] = arrRtnval[1].Replace("$USERDUTYNAME$", _sendUserInfo.DutyName);      //결재자직책명
                arrRtnval[1] = arrRtnval[1].Replace("$USERUSERNAME$", _sendUserInfo.Name);          //결재자이름
                arrRtnval[1] = arrRtnval[1].Replace("$OPINION$", _sendUserInfo.Opinion);            //결재자의견
                arrRtnval[1] = arrRtnval[1].Replace("$LINKURL$", _linkURL);                         //링크(문서보기)

                return arrRtnval;   // [0] : Subj / [1] : Content
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmCont()", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 대상URL정보가져오기(Mail)
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_urlSection"></param>
        /// <returns></returns>
        private string GetLinkURLMail(int _companyID, int _documentID, string _urlSection)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMail");
            targetURL = string.Format(targetURL, baseHttp, fullDomain);

            #region 파라미터 처리

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            if (targetURL.IndexOf("?") > 0)
            {
                string parms = targetURL.Split('?')[1];
                targetURL = targetURL.Split('?')[0];

                if (!parms.Equals("") && parms.IndexOf("=") > 0)
                {
                    if (parms.IndexOf("&") > 0)
                    {
                        string[] arrparms = parms.Split('&');          //xxxx.aspx?a=aaaaaaa&b=bbbbbb
                        foreach (string s in arrparms)
                        {
                            paraDic.Add(s.Split('=')[0], s.Split('=')[0]);
                        }
                    }
                    else
                    {
                        paraDic.Add(parms.Split('=')[0], parms.Split('=')[1]);
                    }
                }
            }

            paraDic.Add("companyID", _companyID.ToString());
            paraDic.Add("documentID", _documentID.ToString());

            #endregion

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 대상URL정보가져오기(Messenger)
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_urlSection"></param>
        /// <returns></returns>
        private string GetLinkURLMessenger(int _companyID, int _documentID, string _urlSection)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string targetURL = "";

            #region 상황별 url정보 가져오기
            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerW");
                    break;
                case "S":   //승인
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerS");
                    break;
                case "SR":    //승인(수신차례)
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerSR");
                    break;
                case "O":   //첨언,첨언추가
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerW");
                    break;
                case "C":   //완결
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerC");
                    break;
                case "RW":  //반려-상신자
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerRW");
                    break;
                case "RS":  //반려-중간결재자
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerRS");
                    break;
                default:    //기타(문서보기)
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerView");
                    break;
            }
            #endregion

            targetURL = string.Format(targetURL, baseHttp, fullDomain);

            #region 파라미터 처리

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            if (targetURL.IndexOf("?") > 0)
            {
                string parms = targetURL.Split('?')[1];
                targetURL = targetURL.Split('?')[0];

                if (!parms.Equals("") && parms.IndexOf("=") > 0)
                {
                    if (parms.IndexOf("&") > 0)
                    {
                        string[] arrparms = parms.Split('&');          //xxxx.aspx?a=aaaaaaa&b=bbbbbb
                        foreach (string s in arrparms)
                        {
                            paraDic.Add(s.Split('=')[0], s.Split('=')[0]);
                        }
                    }
                    else
                    {
                        paraDic.Add(parms.Split('=')[0], parms.Split('=')[1]);
                    }
                }
            }

            paraDic.Add("companyID", _companyID.ToString());
            paraDic.Add("documentID", _documentID.ToString());

            #endregion

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        private string GetLinkURLMessenger(int _companyID, int _documentID, string _urlSection, int _sequence, string _approvalTag, int _targetUserID, int _targetDeptID, string _defaultUrl)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return _defaultUrl;

            string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string targetURL = "";

            #region 상황별 url정보 가져오기
            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerW");
                    break;
                case "S":   //승인
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerS");
                    break;
                case "O":   //첨언,첨언추가
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerW");
                    break;
                case "C":   //완결
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerC");
                    break;
                case "RW":  //반려-상신자
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerRW");
                    break;
                case "RS":  //반려-중간결재자
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerRS");
                    break;
                default:    //기타(문서보기)
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerView");
                    break;
            }
            #endregion

            targetURL = string.Format(targetURL, baseHttp, fullDomain);

            #region 파라미터 처리

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            if (targetURL.IndexOf("?") > 0)
            {
                string parms = targetURL.Split('?')[1];
                targetURL = targetURL.Split('?')[0];

                if (!parms.Equals("") && parms.IndexOf("=") > 0)
                {
                    if (parms.IndexOf("&") > 0)
                    {
                        string[] arrparms = parms.Split('&');          //xxxx.aspx?a=aaaaaaa&b=bbbbbb
                        foreach (string s in arrparms)
                        {
                            paraDic.Add(s.Split('=')[0], s.Split('=')[0]);
                        }
                    }
                    else
                    {
                        paraDic.Add(parms.Split('=')[0], parms.Split('=')[1]);
                    }
                }
            }

            paraDic.Add("companyID", _companyID.ToString());
            paraDic.Add("documentID", _documentID.ToString());
            paraDic.Add("sequence", _sequence.ToString());
            paraDic.Add("approvalTag", _approvalTag);
            paraDic.Add("targetUserID", _targetUserID.ToString());
            paraDic.Add("targetDeptID", _targetDeptID.ToString());

            #endregion

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        private string GetLinkURLMobile(int _companyID, int _documentID)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("DID", _documentID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            string targetURL = "/DeskplusMobile/Approval/AlarmView.aspx?p=" + encparam;

            return targetURL;
        }

        private string GetLinkURLMobile(int _companyID, int _documentID, string _urlSection, int _sequence, string _approvalTag, int _targetUserID, int _targetDeptID, string _defaultUrl)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return _defaultUrl;

            string BPage = "";

            #region 상황별 url정보 가져오기
            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    BPage = "APP";
                    break;
                case "S":   //승인
                    BPage = "APP";
                    break;
                case "RS":   //반려-중간결재자
                    BPage = "APP";
                    break;
                default:    //기타(문서보기)
                    BPage = "";
                    break;
            }
            #endregion

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("DID", _documentID.ToString());
            paraDic.Add("Seq", _sequence.ToString());
            paraDic.Add("ATag", _approvalTag);
            paraDic.Add("TargetUID", _targetUserID.ToString());
            paraDic.Add("TargetDID", _targetDeptID.ToString());
            paraDic.Add("BPage", BPage.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            string targetURL = "/DeskplusMobile/Approval/AlarmView.aspx?p=" + encparam;

            return targetURL;
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 메일 발송
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_to"></param>
        /// <param name="_from"></param>
        /// <param name="_subject"></param>
        /// <param name="_contentHTML"></param>
        public void CreateMHT(string _sendMail, string _sendName, string _to, string _cc, string _bcc, string _subject, string _body)
        {
            MailDataInfo mailInfo = null;

            mailInfo = new MailDataInfo();
            mailInfo.From = _sendName + "<" + _sendMail + ">";
            //mailInfo.To.Add(_to, _to);

            //받는 사람
            foreach (string tmp in _to.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.To.Contains(tmp)) mailInfo.To.Add(tmp, tmp);
                }
            }

            //참조
            foreach (string tmp in _cc.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.Cc.Contains(tmp)) mailInfo.Cc.Add(tmp, tmp);
                }
            }

            //숨은 참조
            foreach (string tmp in _bcc.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.Bcc.Contains(tmp)) mailInfo.Bcc.Add(tmp, tmp);
                }
            }

            mailInfo.Subject = _subject;
            mailInfo.Contents = _body;

            Helper.AppException.WriteAssert(AppSettingType.SQLApproval, "Approval:CreateMHT:" + _to + "," + _sendMail + "," + _subject + "," + _body);

            Zeospace.DeskPlusMobile.Web.Helper.SendMail sendMail = new SendMail();
            sendMail.Send(mailInfo, false, false, "");
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : SMS처리
        /// 작성일자 : 2011-07-04
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendUserID"></param>
        /// <param name="_sendUserName"></param>
        /// <param name="_receiveName"></param>
        /// <param name="_replyPhone"></param>
        /// <param name="_receivePhone"></param>
        /// <param name="_subject"></param>
        /// <param name="_content"></param>
        /// <param name="_targetUrl"></param>
        public void SendSMS(int _companyID, int _sendUserID, string _sendUserName, string _receiveName, string _replyPhone, string _receivePhone, string _subject, string _content, string _targetUrl)
        {
            Zeospace.DeskPlusMobile.Web.SMS.SMSInfo smsInfo
                = new Zeospace.DeskPlusMobile.Web.SMS.SMSInfo();
            DateTime tmpDate = DateTime.Now;

            smsInfo.CompanyID = _companyID;
            smsInfo.InterfaceID = "";
            smsInfo.UserID = _sendUserID;
            smsInfo.Gubun = "0";
            smsInfo.SendPhone = _replyPhone.Replace("-", "");
            smsInfo.ReplyPhone1 = getPhone(_replyPhone, "First");
            smsInfo.ReplyPhone2 = getPhone(_replyPhone, "Middle");
            smsInfo.ReplyPhone3 = getPhone(_replyPhone, "Last");
            smsInfo.ReceivePhone = _receivePhone.Replace("-", ""); ;
            smsInfo.ReceivePhone1 = getPhone(_receivePhone, "First");
            smsInfo.ReceivePhone2 = getPhone(_receivePhone, "Middle");
            smsInfo.ReceivePhone3 = getPhone(_receivePhone, "Last");
            smsInfo.Status = "0";
            smsInfo.SendDate = tmpDate;
            smsInfo.SendTime = tmpDate.ToString("HHmmss");
            smsInfo.Subject = _subject;
            smsInfo.Content = _content;
            smsInfo.TargetUrl = _targetUrl;
            smsInfo.StartDate = tmpDate;
            smsInfo.EndDate = tmpDate;
            smsInfo.WriteDate = tmpDate;
            //smsInfo.SMSNoticeTemplateID = null;
            smsInfo.WriteUserID = _sendUserID;
            smsInfo.WriteUserName = _sendUserName;
            smsInfo.ReceiverName = _receiveName;

            Zeospace.DeskPlusMobile.Web.SMS.ControlSendSMS sendSMS
                = new Zeospace.DeskPlusMobile.Web.SMS.ControlSendSMS();
            sendSMS.Transfer(smsInfo);
        }

        /// <summary>
        /// 전화번호 각 필드값 가져오기
        /// </summary>
        /// <param name="_phone"></param>
        /// <param name="_position"></param>
        /// <returns></returns>
        public string getPhone(string _phone, string _position)
        {
            string rtnVal = "";
            string tmpVal = _phone.Replace("-", "");

            if (tmpVal.Trim().Length == 10)
            {
                switch (_position)
                {
                    case "First":
                        rtnVal = tmpVal.Trim().Substring(0, 3);
                        break;
                    case "Middle":
                        rtnVal = tmpVal.Trim().Substring(3, 3);
                        break;
                    case "Last":
                        rtnVal = tmpVal.Trim().Substring(6, 4);
                        break;
                }
            }
            else if (tmpVal.Trim().Length == 11)
            {
                switch (_position)
                {
                    case "First":
                        rtnVal = tmpVal.Trim().Substring(0, 3);
                        break;
                    case "Middle":
                        rtnVal = tmpVal.Trim().Substring(3, 4);
                        break;
                    case "Last":
                        rtnVal = tmpVal.Trim().Substring(7, 4);
                        break;
                }
            }
            else
            {
                switch (_position)
                {
                    case "First":
                        rtnVal = tmpVal.Trim().Substring(0, 3);
                        break;
                    case "Middle":
                        rtnVal = tmpVal.Trim().Substring(3, 3);
                        break;
                    case "Last":
                        rtnVal = tmpVal.Trim().Substring(6, tmpVal.Trim().Length - 6);
                        break;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 사용자 정보 가져오기
        /// 작성일자 : 2014-01-07
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public List<BE.AccountInfo> GetAccountInfo(int _companyID, int _userID, int _deptID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ACCOUNT");
                sql = string.Format(sql, _companyID, _userID, _deptID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new AccountInfo();
                    info.DeptName = this.GetLangName(
                                dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                dr["DeptZhtw"].ToString().ToNull(), PreLang);

                    info.TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);

                    info.DutyName = this.GetLangName(
                            dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                            dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                            dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                            dr["DutyZhtw"].ToString().ToNull(), PreLang);

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAccountInfo()", ex);
                return null;
            }
        }

        /// <summary>
        /// 결재라인 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.SequenceInfo> GetSequenceInfo(int _companyID, int _documentID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SelectSeqList");
                sql = string.Format(sql, _companyID, _documentID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                List<BE.SequenceInfo> list = new List<SequenceInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.SequenceInfo info = new SequenceInfo();

                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                    info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                    info.ApprovalTag = dr["ApprovalTag"].ToString();
                    info.MyTurn = dr["MyTurn"].ToString();
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();

                    info.UserName = this.GetLangName(
                                dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                dr["NameZhtw"].ToString().ToNull(), PreLang);

                    info.DeptName = this.GetLangName(
                                dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                dr["DeptZhtw"].ToString().ToNull(), PreLang);

                    info.TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAccountInfo()", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 참조문서 보기 체크
        /// 작성일자 : 2015-10-23
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCarboncopyViewingDate(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            try
            {
                if (dID.Equals("") || dID.Equals("0")) return false;

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                parm[2].Value = UserID;
                parm[3].Value = UserInfo.DeptID;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPD_PROC_CarbonCopyUser_ViewDate");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModCarboncopyViewingDate('" + dID + "')", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전달문서 보기 체크
        /// 작성일자 : 2015-10-23
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModForwardViewingDate(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            try
            {
                if (dID.Equals("") || dID.Equals("0")) return false;

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@ReceiveUserID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                parm[2].Value = UserID;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPD_PROC_ForwardUser_ViewDate");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModCarboncopyViewingDate('" + dID + "')", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 회람문서 보기 체크
        /// 작성일자 : 2015-10-26
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModRecvViewingDate(string p)
        {
            SetDecryptString(p);
            string cirDocID = GetDecryptParam("CirDocID", "0");

            try
            {
                if (cirDocID.Equals("") || cirDocID.Equals("0")) return false;

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@CirculationDocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = cirDocID;
                parm[2].Value = UserID;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "UPD_PROC_CircUser_ConfirmDate");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModRecvViewingDate('" + cirDocID + "')", ex);
                return false;
            }
        }

    }
}
