using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;

namespace Zeospace.DeskPlus.Education.BLL
{
    public class Alarm : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 알람

        /// <summary>
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public void SendAlarm(BE.ListInfo _info, int _receiverID, string _isType)
        {
            SendMessenger(_info, _receiverID, CreateRequestAlarmTemplate(_info, _isType, LoadAlarmTemplate(), false));
        }

        /// <summary>
        /// 관리자 요청으로 한 명의 교육 대상자에게 완료 요청 알람을 접수합니다.
        /// </summary>
        public bool SendCompletionRequest(BE.ListInfo _info, int _receiverID)
        {
            DateTime koreaNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time"));
            AlarmInfo alarmInfo = CreateCompletionRequestAlarm(_info, _receiverID, koreaNow);
            return alarmInfo != null && new Common.BLL.Alarm().Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 관리자가 진행/지연 상태별로 요청한 교육 완료 알람을 접수합니다.
        /// </summary>
        public bool SendStatusCompletionRequest(BE.ListInfo _info, int _receiverID, string progressStatus)
        {
            AlarmInfo alarmInfo = CreateStatusCompletionRequestAlarm(_info, _receiverID, progressStatus, DateTime.Now);
            return alarmInfo != null && new Common.BLL.Alarm().Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        private AlarmInfo CreateStatusCompletionRequestAlarm(BE.ListInfo _info, int _receiverID,
            string progressStatus, DateTime writeDate)
        {
            if (!IsValidCompletionRequest(_info, _receiverID)) return null;
            return CreateCompletionRequestAlarmCore(_info, _receiverID, writeDate,
                GetStatusCompletionSubjectPrefix(_info, progressStatus, 50));
        }

        internal static string GetStatusCompletionSubjectPrefix(BE.ListInfo info, string progressStatus,
            int maxItemLength)
        {
            if (progressStatus != "2" && progressStatus != "4")
                throw new ArgumentException("발송 진행상태가 올바르지 않습니다.", "progressStatus");

            return GetSubjectPrefix(info, progressStatus == "2" ? "미완료" : "지연", maxItemLength);
        }

        internal static string GetAlarmItemName(BE.ListInfo info)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (String.IsNullOrWhiteSpace(info.ItemName))
            {
                // 예약 알람처럼 분류 코드가 없는 경로도 기존 교육 조회를 이용합니다.
                if (String.IsNullOrWhiteSpace(info.ItemID) && info.CompanyID > 0 && info.ListID > 0)
                {
                    using (DataTableReader reader = new DAL.List().GetList(info.CompanyID,
                        info.ListID.ToString(System.Globalization.CultureInfo.InvariantCulture)))
                    {
                        if (reader == null || !reader.Read())
                            throw new InvalidOperationException("알람 분류 정보를 확인할 수 없습니다.");
                        info.ItemID = reader["ItemID"].ToString();
                    }
                }

                if (info.CompanyID > 0 && !String.IsNullOrWhiteSpace(info.ItemID))
                {
                    string language = String.IsNullOrWhiteSpace(info.PreLang) ? "ko" : info.PreLang;
                    foreach (DataRowView code in AppSettingHelper.GetSysCode(info.CompanyID, "EducationItem"))
                    {
                        if (code["CodeID"].ToString() != info.ItemID) continue;
                        info.ItemName = UtilHelper.GetLangName(code["NameBase"].ToString(), code["NameKo"].ToString(),
                            code["NameEn"].ToString(), code["NameJa"].ToString(), code["NameFr"].ToString(),
                            code["NameZhcn"].ToString(), code["NameZhtw"].ToString(), language);
                        break;
                    }
                }
                // 삭제된 분류는 화면과 동일하게 분류 코드로 표시하며 특정 이름으로 대체하지 않습니다.
                if (String.IsNullOrWhiteSpace(info.ItemName)) info.ItemName = info.ItemID;
            }

            StringBuilder normalized = new StringBuilder();
            foreach (char character in (info.ItemName ?? String.Empty).ToReverseXSS())
                normalized.Append(Char.IsControl(character) ? ' ' : character);
            string itemName = normalized.ToString().Trim();
            if (itemName.Length == 0)
                throw new InvalidOperationException("알람 분류 정보를 확인할 수 없습니다.");
            return itemName;
        }

        private static string GetSubjectPrefix(BE.ListInfo info, string modifier, int maxItemLength)
        {
            return "[" + TruncateAlarmText(GetAlarmItemName(info), maxItemLength)
                + (String.IsNullOrEmpty(modifier) ? "" : "(" + modifier + ")") + "] ";
        }

        private AlarmInfo CreateCompletionRequestAlarm(BE.ListInfo _info, int _receiverID, DateTime writeDate)
        {
            if (!IsValidCompletionRequest(_info, _receiverID)) return null;
            DateTime endDate;
            string status = DateTime.TryParseExact(_info.EndDate, new string[] { "yyyyMMdd", "yyyy-MM-dd" },
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                out endDate) && endDate.Date < writeDate.Date ? "4" : "2";
            return CreateStatusCompletionRequestAlarm(_info, _receiverID, status, writeDate);
        }

        private static bool IsValidCompletionRequest(BE.ListInfo info, int receiverID)
        {
            return info != null && info.CompanyID > 0 && info.ListID > 0
                && info.WriteUserID > 0 && receiverID > 0;
        }

        private AlarmInfo CreateCompletionRequestAlarmCore(BE.ListInfo _info, int _receiverID,
            DateTime writeDate, string subjectPrefix)
        {
            if (!IsValidCompletionRequest(_info, _receiverID)) return null;

            string title = (_info.Title ?? String.Empty).ToReverseXSS();
            string contentPrefix = "과정명: ";
            string contentSuffix = "<br />해당 과정을 완료해 주시기 바랍니다.";
            string subject = subjectPrefix + TruncateAlarmText(title, 70 - subjectPrefix.Length);
            string content = contentPrefix
                + EncodeAlarmTitle(title, 1000 - contentPrefix.Length - contentSuffix.Length)
                + contentSuffix;

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("listID", _info.ListID.ToString(System.Globalization.CultureInfo.InvariantCulture));
            string encrypted = Utility.EncryptString(HttpUtility.UrlEncode(Utility.GetParameterDic(parameters)));
            if (String.IsNullOrEmpty(encrypted) || encrypted == "!NULL")
                return null;
            string targetUrl = baseHttp + fullDomain + "/DeskPlusEIP/Education/Content/ViewPopupTotal.aspx?p="
                + HttpUtility.UrlEncode(encrypted);

            DateTime sendDate = writeDate.AddMinutes(1);
            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.WriteUserID;
            alarmInfo.SenderName = _info.SenderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = sendDate;
            alarmInfo.Subject = subject;
            alarmInfo.Content = content;
            alarmInfo.TargetUrl = targetUrl;
            alarmInfo.StartDate = sendDate;
            alarmInfo.EndDate = sendDate;
            alarmInfo.WriteDate = writeDate;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "EDUCATION";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return alarmInfo;
        }

        /// <summary>
        /// 한국 기준 오늘의 종료일 전 알람과, 종료일이 지나지 않은 실패 기록을 Teams로 재시도합니다.
        /// 회사는 1로 고정하고 실행일은 호출자의 입력을 받지 않습니다.
        /// </summary>
        public BE.ScheduledAlarmResult RunScheduledReminders()
        {
            DateTime koreaNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time"));
            DateTime today = koreaNow.Date;
            DAL.ScheduledAlarm dal = new DAL.ScheduledAlarm();
            BE.ScheduledAlarmResult result = new BE.ScheduledAlarmResult();

            // Teams 호출 전에 발송 대상을 DB에 기록하여 동시 실행의 중복 발송을 막습니다.
            DataTable targets = new DataTable();
            using (DataTableReader reader = dal.GetTargets(today)) targets.Load(reader);
            Common.BLL.Alarm teams = new Common.BLL.Alarm();
            Dictionary<int, string> itemNames = new Dictionary<int, string>();
            result.TargetCount = targets.Rows.Count;
            foreach (DataRow target in targets.Rows)
            {
                int listID = Convert.ToInt32(target["ListID"]);
                int userID = Convert.ToInt32(target["UserID"]);
                DateTime alarmDate = DateTime.ParseExact(target["AlarmDate"].ToString(), "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture);
                string failureCode = "PREPARE_FAILED";
                bool deliveryStarted = false;
                bool canRetry = true;
                try
                {
                    BE.ListInfo info = new BE.ListInfo();
                    info.CompanyID = 1;
                    info.ListID = listID;
                    info.PreLang = "ko";
                    info.Title = target["Title"].ToString();
                    info.EndDate = target["EndDate"].ToString();
                    info.AlarmDay = Convert.ToInt32(target["AlarmDay"]);
                    failureCode = "CLAIM_FAILED";
                    canRetry = false;
                    DAL.ScheduledAlarm.DeliveryClaim claim = dal.TryStartTeamsDelivery(listID, userID, alarmDate,
                        today, info.Title, info.EndDate, info.AlarmDay);
                    if (claim == DAL.ScheduledAlarm.DeliveryClaim.Skip)
                    {
                        result.SkippedCount++;
                        continue;
                    }
                    if (claim == DAL.ScheduledAlarm.DeliveryClaim.Pending)
                    {
                        result.FailedCount++;
                        LogScheduledAlarmFailure(listID, userID, "TEAMS_PENDING");
                        continue;
                    }

                    deliveryStarted = true;
                    canRetry = true;
                    failureCode = "PREPARE_FAILED";
                    info.WriteUserID = Convert.ToInt32(target["WriteUserID"]);
                    info.SenderName = TruncateAlarmText(target["SenderName"].ToString().ToReverseXSS(), 50);
                    string itemName;
                    if (itemNames.TryGetValue(listID, out itemName)) info.ItemName = itemName;
                    else
                    {
                        GetAlarmItemName(info);
                        itemNames.Add(listID, info.ItemName);
                    }
                    AlarmInfo alarm = CreateCompletionRequestAlarm(info, userID, koreaNow);
                    if (!teams.PrepareForQueue(alarm))
                        throw new InvalidOperationException("Alarm preparation failed.");

                    // 기존 Teams 알림과 같은 자동 로그인 링크를 사용합니다.
                    failureCode = "TEAMS_UNCONFIRMED";
                    canRetry = false;
                    bool deliveryUncertain;
                    if (!Common.BLL.TeamsMSG.SendAlarm(alarm, out deliveryUncertain))
                    {
                        canRetry = !deliveryUncertain;
                        if (canRetry)
                            failureCode = "TEAMS_FAILED";
                        throw new InvalidOperationException("Teams delivery failed.");
                    }

                    failureCode = "HISTORY_FAILED";
                    dal.CompleteTeamsDelivery(listID, userID, alarmDate);
                    result.SentCount++;
                }
                catch (Exception)
                {
                    result.FailedCount++;
                    if (canRetry)
                    {
                        try
                        {
                            dal.RecordFailure(listID, userID, alarmDate, today,
                                failureCode == "TEAMS_FAILED" ? "MESSAGE_FAILED" : "PREPARE_FAILED", deliveryStarted);
                        }
                        catch (Exception) { LogScheduledAlarmFailure(listID, userID, "HISTORY_FAILED"); }
                    }
                    LogScheduledAlarmFailure(listID, userID, failureCode);
                }
            }

            // 발송 대상이 없거나 일부 발송에 실패해도 오래된 이력 정리는 별도로 실행합니다.
            // 정리 실패로 이미 발송한 알람의 결과를 잃지 않도록 실패 항목을 구분합니다.
            try { result.CleanedHistoryCount = dal.CleanupHistory(today); }
            catch (Exception)
            {
                result.HistoryCleanupFailed = true;
                LogScheduledAlarmFailure(0, 0, "HISTORY_CLEANUP_FAILED");
            }
            return result;
        }

        private static void LogScheduledAlarmFailure(int listID, int userID, string code)
        {
            try
            {
                // 예외 원문과 로그인 연결 URL에는 비밀번호가 포함될 수 있어 기록하지 않습니다.
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                    String.Format(System.Globalization.CultureInfo.InvariantCulture,
                        "Education.ScheduledAlarm CompanyID=1, ListID={0}, UserID={1}, Code={2}", listID, userID, code));
            }
            catch (Exception) { }
        }

        private static string TruncateAlarmText(string text, int maxLength)
        {
            if (text.Length <= maxLength) return text;
            if (maxLength > 0 && Char.IsHighSurrogate(text[maxLength - 1])
                && Char.IsLowSurrogate(text[maxLength]))
                maxLength--;
            return text.Substring(0, maxLength);
        }

        private static string EncodeAlarmTitle(string title, int maxEncodedLength)
        {
            // 원문 단위로 자른 뒤 인코딩하여 HTML 엔티티나 서로게이트 쌍이 잘리지 않게 합니다.
            int minimum = 0;
            int maximum = Math.Min(title.Length, maxEncodedLength);
            while (minimum < maximum)
            {
                int middle = minimum + (maximum - minimum + 1) / 2;
                string encoded = HttpUtility.HtmlEncode(TruncateAlarmText(title, middle));
                if (encoded.Length <= maxEncodedLength)
                    minimum = middle;
                else
                    maximum = middle - 1;
            }
            return HttpUtility.HtmlEncode(TruncateAlarmText(title, minimum));
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="arrAlarm"></param>
        /// <returns></returns>
        private bool SendMessenger(BE.ListInfo _info, int _receiverID, string[] arrAlarm)
        {
            if (arrAlarm == null) return false;

            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.WriteUserID;
            alarmInfo.SenderName = _info.SenderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = "{0}{1}/DeskPlusEIP/Education/Content/ViewPopupTotal.aspx";

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _info.ListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "EDUCATION";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        #endregion

        #region 요청알람

        /// <summary>
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AlarmRequest(BE.ListInfo _info)
        {
            return AlarmRequest(_info, false);
        }

        internal bool AlarmRequest(BE.ListInfo _info, bool isReannouncement)
        {
            return AlarmRequest(_info, isReannouncement, null);
        }

        internal bool AlarmRequest(BE.ListInfo _info, bool isReannouncement, ISet<int> receiverIDs)
        {
            if (_info == null) return false;
            if (receiverIDs != null && receiverIDs.Count == 0) return true;

            try
            {
                DAL.ListTO toDal = new DAL.ListTO();
                bool allSucceeded = true;

                using (DataTableReader dtrTO = toDal.GetListAlarm(_info.CompanyID, _info.ListID.ToString()))
                {
                    if (dtrTO != null && dtrTO.HasRows)
                    {
                        XmlDocument templateXml = LoadAlarmTemplate();
                        Dictionary<string, string[]> templates = new Dictionary<string, string[]>();

                        while (dtrTO.Read())
                        {
                            string receiverID = "";

                            try
                            {
                                receiverID = dtrTO["UserID"].ToString();
                                int receiverUserID = receiverID.ToNumber<int>();
                                if (receiverIDs != null && !receiverIDs.Contains(receiverUserID)) continue;
                                string isType = dtrTO["IsType"].ToString();
                                string[] arrAlarm;
                                if (!templates.TryGetValue(isType, out arrAlarm))
                                {
                                    arrAlarm = CreateRequestAlarmTemplate(_info, isType, templateXml, isReannouncement);
                                    templates.Add(isType, arrAlarm);
                                }

                                if (!SendMessenger(_info, receiverUserID, arrAlarm))
                                {
                                    allSucceeded = false;
                                    LogAlarmFailure(_info, receiverID, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                allSucceeded = false;
                                LogAlarmFailure(_info, receiverID, ex);
                            }
                        }
                    }
                }

                return allSucceeded;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AlarmRequest (" + _info.ListID.ToString() + ") : " + ex);
                return false;
            }
        }

        private void LogAlarmFailure(BE.ListInfo _info, string receiverID, Exception exception)
        {
            try
            {
                string message = "AlarmRequest failed. CompanyID=" + _info.CompanyID.ToString()
                    + ", ListID=" + _info.ListID.ToString() + ", UserID=" + receiverID;

                if (exception == null)
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(message);
                else
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(message, exception);
            }
            catch (Exception)
            {
                // 로그 기록 실패도 나머지 수신자의 알람 처리를 중단하지 않도록 합니다.
            }
        }

        #endregion

        #region 알람 내용 템플릿

        /// <summary>
        /// 알람 처리마다 XML 파일을 한 번 읽고 같은 처리 내에서 재사용합니다.
        /// </summary>
        private XmlDocument LoadAlarmTemplate()
        {
            XmlDocument templateXml = new XmlDocument();
            string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Education/App_Data/Ko.xml");
            templateXml.Load(xmlPath);
            return templateXml;
        }

        private string[] CreateRequestAlarmTemplate(BE.ListInfo info, string isType,
            XmlDocument templateXml, bool isReannouncement)
        {
            try
            {
                XmlNode tmpXmlNode = templateXml.DocumentElement;
                string prefix = GetSubjectPrefix(info, isReannouncement ? "재공지" : "", 50);
                string title = (info.Title ?? String.Empty).ToReverseXSS();
                string[] arrRtnval = new string[2];
                string subjectPrefix = tmpXmlNode.SelectSingleNode("/ALARM/MESSENGER/" + isType).InnerText
                    .Replace("$PREFIX$", prefix);
                arrRtnval[0] = subjectPrefix + TruncateAlarmText(title, 70 - subjectPrefix.Length);
                XmlNode contentNode = tmpXmlNode.SelectSingleNode("/ALARM/MESSENGER/ContentHTML" + isType);
                arrRtnval[1] = contentNode == null ? "" : HttpUtility.HtmlDecode(contentNode.InnerText);
                arrRtnval[1] = Regex.Replace(arrRtnval[1], @"\$(PREFIX|SUBJECT)\$", delegate(Match match)
                {
                    return HttpUtility.HtmlEncode(match.Groups[1].Value == "PREFIX" ? prefix.TrimEnd() : title);
                });
                return arrRtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Eductaion.AlarmTemplate : " + ex);
                throw;
            }
        }

        #endregion 
    }
}
