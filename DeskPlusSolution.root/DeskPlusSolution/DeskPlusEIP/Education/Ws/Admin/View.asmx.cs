using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Education.Web.Ws.Admin
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSEducation
    {
        #region 수정 가능 여부

        /// <summary>
        /// 0: 조회 불가, 1: 수정 가능. 완료자가 있어도 수정은 허용합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int CanModifyEducation(string p)
        {
            if (!String.Equals(Context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                throw new HttpException(405, "POST 요청만 사용할 수 있습니다.");
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                    && !CurrentUserInfo.IsInRole("Education")))
                throw new HttpException(403, "수정할 권한이 없습니다.");

            SetDecryptString(p);
            int listID;
            if (!Int32.TryParse(GetDecryptParam("listID", ""), NumberStyles.None,
                CultureInfo.InvariantCulture, out listID) || listID <= 0)
                throw new HttpException(400, "수정할 정보가 올바르지 않습니다.");

            int modifyState = new DAL.List().GetModifyState(CurrentUserInfo.CompanyID, listID);
            return modifyState == 1 || modifyState == 2 ? 1 : 0;
        }

        #endregion 수정 가능 여부

        #region 완료 취소

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public BE.CompletionCancelResult CancelComplete(string p)
        {
            Base.CompletionCancelRequest.Validate(Context.Request);
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                    && !CurrentUserInfo.IsInRole("Education")))
                throw new HttpException(403, "완료를 취소할 권한이 없습니다.");

            SetDecryptString(p);
            int listID = Base.CompletionCancelRequest.ReadPositiveID(GetDecryptParam("listID", ""),
                "완료 취소할 정보가 올바르지 않습니다.");
            int userID = Base.CompletionCancelRequest.ReadPositiveID(GetDecryptParam("userID", ""),
                "완료 취소할 대상자 정보가 올바르지 않습니다.");
            return new BLL.TOAttach().CancelComplete(CurrentUserInfo.CompanyID, listID, userID);
        }

        #endregion 완료 취소

        #region 삭제

        /// <summary>
        /// 0: 조회 불가, 1: 삭제 가능, 2: 완료자가 있어 삭제 불가.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int CanDeleteEducation(string p)
        {
            if (!String.Equals(Context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                throw new HttpException(405, "POST 요청만 사용할 수 있습니다.");
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                    && !CurrentUserInfo.IsInRole("Education")))
                throw new HttpException(403, "삭제할 권한이 없습니다.");

            SetDecryptString(p);
            int listID;
            if (!Int32.TryParse(GetDecryptParam("listID", ""), NumberStyles.None,
                CultureInfo.InvariantCulture, out listID) || listID <= 0)
                throw new HttpException(400, "삭제할 정보가 올바르지 않습니다.");

            return new DAL.List().GetModifyState(CurrentUserInfo.CompanyID, listID);
        }

        /// <summary>
        /// 완료자가 없는 현재 회사의 내역과 관련 등록 정보를 삭제합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelList(string p)
        {
            if (!String.Equals(Context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                throw new HttpException(405, "POST 요청만 사용할 수 있습니다.");

            // 화면의 버튼 표시 여부와 관계없이 서버에서 관리자 권한을 확인합니다.
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                    && !CurrentUserInfo.IsInRole("Education")))
                throw new HttpException(403, "삭제할 권한이 없습니다.");

            SetDecryptString(p);
            int listID;
            if (!Int32.TryParse(GetDecryptParam("listID", ""), NumberStyles.None,
                CultureInfo.InvariantCulture, out listID) || listID <= 0)
                throw new HttpException(400, "삭제할 정보가 올바르지 않습니다.");

            DAL.List list = new DAL.List();
            int deleteState = list.GetModifyState(CurrentUserInfo.CompanyID, listID);
            if (deleteState == 0) throw new HttpException(404, "조회할 수 없는 내역입니다.");
            if (deleteState == 2)
                throw new HttpException(409, "완료자가 있는 내역은 삭제할 수 없습니다.");

            // 실제 삭제에서도 같은 트랜잭션 안에서 완료 여부를 재확인합니다.
            bool deleted = new BLL.List().Del(CurrentUserInfo.CompanyID, listID);
            if (!deleted && list.GetModifyState(CurrentUserInfo.CompanyID, listID) == 2)
                throw new HttpException(409, "완료자가 있는 내역은 삭제할 수 없습니다.");
            return deleted;
        }

        #endregion 삭제

        #region 완료 요청 알람

        /// <summary>
        /// 현재 보고 있는 미완료 대상자에게 메신저 알람을 요청합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SendAlarm(string p)
        {
            int userID;
            BE.ListInfo info = LoadAlarmInfo(p, false, out userID);
            using (DataTableReader reader = new DAL.TOUser().GetAlarmTargets(info.CompanyID, info.ListID, userID))
            {
                if (reader == null || !reader.Read()) return false;
            }

            return new BLL.Alarm().SendCompletionRequest(info, userID);
        }

        /// <summary>
        /// 현재 진행 중인 미완료 대상자에게만 메신저 알람을 요청합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public AlarmSendResult SendAllAlarm(string p)
        {
            return SendStatusAlarms(p, "2");
        }

        /// <summary>
        /// 종료일이 지난 미완료(지연) 대상자에게만 메신저 알람을 요청합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public AlarmSendResult SendDelayedAlarm(string p)
        {
            return SendStatusAlarms(p, "4");
        }

        private AlarmSendResult SendStatusAlarms(string p, string progressStatus)
        {
            int userID;
            BE.ListInfo info = LoadAlarmInfo(p, true, out userID);
            List<int> receivers = new List<int>();
            using (DataTableReader reader = new DAL.TOUser().GetAlarmTargets(info.CompanyID, info.ListID, 0, progressStatus))
            {
                if (reader != null)
                {
                    while (reader.Read()) receivers.Add(reader["UserID"].ToString().ToNumber<int>());
                }
            }

            AlarmSendResult result = new AlarmSendResult();
            result.RecipientCount = receivers.Count;
            BLL.Alarm biz = new BLL.Alarm();
            foreach (int receiverID in receivers)
            {
                try
                {
                    if (biz.SendStatusCompletionRequest(info, receiverID, progressStatus)) result.SuccessCount++;
                    else result.FailureCount++;
                }
                catch (Exception ex)
                {
                    result.FailureCount++;
                    try
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                            "Education.SendStatusAlarms (" + progressStatus + ", " + info.ListID.ToString(CultureInfo.InvariantCulture)
                            + ", " + receiverID.ToString(CultureInfo.InvariantCulture) + ")", ex);
                    }
                    catch (Exception) { }
                }
            }
            return result;
        }

        /// <summary>
        /// 현재 진행 중인 미완료 대상자별로 메일 발송 대기 EML 파일을 생성합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public MailSendResult SendIncompleteMail(string p)
        {
            return QueueStatusMails(p, "2");
        }

        /// <summary>
        /// 지연 상태의 미완료 대상자별로 메일 발송 대기 EML 파일을 생성합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public MailSendResult SendDelayedMail(string p)
        {
            return QueueStatusMails(p, "4");
        }

        private MailSendResult QueueStatusMails(string p, string progressStatus)
        {
            int userID;
            BE.ListInfo info = null;
            List<KeyValuePair<int, string>> recipients = new List<KeyValuePair<int, string>>();
            MailSendResult result = new MailSendResult();
            bool batchRequested = false;
            try
            {
                // 권한, 동일 출처 POST/JSON, 회사별 및 일괄 처리 문맥을 서버에서 확인합니다.
                info = LoadAlarmInfo(p, true, out userID);
                HashSet<int> seen = new HashSet<int>();
                using (DataTableReader reader = new DAL.TOUser().GetMailTargets(info.CompanyID, info.ListID, progressStatus))
                {
                    if (reader == null) throw new InvalidOperationException("Mail target query did not return a reader.");
                    while (reader.Read())
                    {
                        int recipientID = reader["UserID"].ToString().ToNumber<int>();
                        if (recipientID <= 0) throw new InvalidOperationException("Invalid mail recipient ID.");
                        if (!seen.Add(recipientID)) continue;
                        string recipientMail;
                        // 잘못된 주소도 포함하여 한 명이라도 준비할 수 없으면 전체 요청을 취소합니다.
                        BLL.StatusMail.TryGetAddress(reader["MailID"].ToString(), reader["MailDomain"].ToString(), out recipientMail);
                        recipients.Add(new KeyValuePair<int, string>(recipientID, recipientMail));
                    }
                }

                result.RecipientCount = recipients.Count;
                if (recipients.Count == 0)
                {
                    result.Outcome = "NoRecipients";
                    return result;
                }

                string senderMail;
                BLL.StatusMail.TryGetAddress(CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, out senderMail);
                batchRequested = true;
                BLL.StatusMailBatchOutcome outcome = new BLL.StatusMail().QueueCompletionRequests(
                    info, senderMail, recipients, progressStatus);
                if (outcome == BLL.StatusMailBatchOutcome.Sent)
                {
                    result.Outcome = "Sent";
                    result.SuccessCount = recipients.Count;
                    return result;
                }

                // 파일 준비 실패/회수 완료와 업체 수집으로 회수 여부가 불명확한 경우를 구분합니다.
                result.Outcome = outcome == BLL.StatusMailBatchOutcome.RolledBack ? "RolledBack" : "Unknown";
                result.FailureCount = recipients.Count;
                WriteStatusMailFailure(info, progressStatus, result.Outcome, recipients.Count, null);
            }
            catch (Exception error)
            {
                // 권한 부족/잘못된 요청은 기존 HTTP 오류를 유지하고, 조회 장애는 준비 실패로 기록합니다.
                if (info == null && error is HttpException) throw;
                result.RecipientCount = recipients.Count;
                result.FailureCount = recipients.Count;
                result.Outcome = batchRequested ? "Unknown" : "RolledBack";
                WriteStatusMailFailure(info, progressStatus, result.Outcome, recipients.Count, error);
            }
            return result;
        }

        private static void WriteStatusMailFailure(BE.ListInfo info, string progressStatus, string outcome,
            int recipientCount, Exception error)
        {
            try
            {
                // 공통 ExceptionLog 경로를 사용하며 주소/본문/링크/예외 원문은 기록하지 않습니다.
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(String.Format(
                    CultureInfo.InvariantCulture,
                    "Education.QueueStatusMails CompanyID={0}, ListID={1}, ActorUserID={2}, Status={3}, Outcome={4}, RecipientCount={5}, ErrorType={6}",
                    info == null ? 0 : info.CompanyID, info == null ? 0 : info.ListID,
                    info == null ? 0 : info.WriteUserID, progressStatus, outcome, recipientCount,
                    error == null ? "" : error.GetType().Name));
            }
            catch (Exception) { }
        }

        private BE.ListInfo LoadAlarmInfo(string p, bool allTargets, out int userID)
        {
            Base.CompletionCancelRequest.Validate(Context.Request);
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                    && !CurrentUserInfo.IsInRole("Education")))
                throw new HttpException(403, "알람을 발송할 권한이 없습니다.");

            SetDecryptString(p);
            int listID;
            if (!Int32.TryParse(GetDecryptParam("listID", ""), NumberStyles.None,
                CultureInfo.InvariantCulture, out listID) || listID <= 0)
                throw new HttpException(400, "알람을 발송할 정보가 올바르지 않습니다.");
            userID = 0;
            if (allTargets && GetDecryptParam("userID", "") != "" && GetDecryptParam("userID", "") != "0")
                throw new HttpException(400, "사용자별 보기에서는 일괄 알람을 발송할 수 없습니다.");
            if (!allTargets && (!Int32.TryParse(GetDecryptParam("userID", ""), NumberStyles.None,
                CultureInfo.InvariantCulture, out userID) || userID <= 0))
                throw new HttpException(400, "알람을 받을 사용자 정보가 올바르지 않습니다.");

            BE.ListInfo info = new BE.ListInfo();
            using (DataTableReader reader = new DAL.List().GetList(CurrentUserInfo.CompanyID,
                listID.ToString(CultureInfo.InvariantCulture)))
            {
                if (reader == null || !reader.Read())
                    throw new HttpException(404, "조회할 수 없는 내역입니다.");
                info.ItemID = reader["ItemID"].ToString();
                info.Title = reader["Title"].ToString();
                info.StartDate = reader["StartDate"].ToString();
                info.EndDate = reader["EndDate"].ToString();
            }
            foreach (DataRowView code in AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem"))
            {
                if (code["CodeID"].ToString() != info.ItemID) continue;
                info.ItemName = GetLangName(code["NameBase"].ToString(), code["NameKo"].ToString(),
                    code["NameEn"].ToString(), code["NameJa"].ToString(), code["NameFr"].ToString(),
                    code["NameZhcn"].ToString(), code["NameZhtw"].ToString(), PreLang);
                break;
            }
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ListID = listID;
            info.WriteUserID = CurrentUserInfo.UserID;
            info.SenderName = CurrentUserInfo.Name;
            return info;
        }

        #endregion 완료 요청 알람

        #region 공지첨부

        /// <summary>
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetAttachFileList(string p)
        {
            try
            {
                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "0");

                ObjList listobj = new ObjList();

                DAL.Attach dal = new DAL.Attach();
                List<BE.AttachInfo> attachInfo = dal.GetAttachList(CurrentUserInfo.CompanyID, listID.ToNumber<int>());

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    BE.AttachInfo info = new BE.AttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.ListID = _attachInfo.ListID;
                    info.AttachID = _attachInfo.AttachID;

                    info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    info.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : info.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", info.FilePath + info.FileGUID);
                    paraDic.Add("fileName", info.FileName);
                    info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;
                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 공지첨부
    }

    public class AlarmSendResult
    {
        public int RecipientCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
    }

    public class MailSendResult
    {
        public string Outcome { get; set; }
        public int RecipientCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
    }
}
