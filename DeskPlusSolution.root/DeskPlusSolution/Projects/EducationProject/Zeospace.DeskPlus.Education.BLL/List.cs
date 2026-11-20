using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Education.BE;
using System.Web;
using System.Transactions;
using System.IO;
using System.Globalization;
using System.Data;

namespace Zeospace.DeskPlus.Education.BLL
{
    public class List : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region DEL()

        /// <summary>
        /// 완료자가 없는 교육만 전체 삭제하며 DB 기록만 처리하고 실제 문서는 그대로 둡니다.
        /// </summary>
        public bool Del(int _companyID, int _listID)
        {
            if (_companyID <= 0 || _listID <= 0) return false;

            return new DAL.List().Del(_companyID, _listID);
        }

        #endregion

        #region 요청하기

        /// <summary>
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public int Add(BE.ListInfo _listinfo)
        {
            if (!ValidateTargets(_listinfo))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                    "Education.List.Add invalid target data.");
                return -1;
            }

            string alarmValidationMessage = GetRegistrationAlarmValidationMessage(
                _listinfo.StartDate, _listinfo.EndDate, _listinfo.AlarmDay, GetRegistrationToday());
            if (!String.IsNullOrEmpty(alarmValidationMessage))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                    "Education.List.Add invalid reminder date.");
                return -1;
            }

            try
            {
                int listID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블
                    DAL.List dal = new DAL.List();
                    listID = dal.Add(_listinfo);

                    if (listID <= 0)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                            "Education.List.Add invalid listID: " + listID.ToString());
                        return -1;
                    }

                    _listinfo.ListID = listID;

                    DAL.ListTO toDal = new DAL.ListTO();
                    toDal.Add(_listinfo.CompanyID, listID, _listinfo.ToUserList, _listinfo.WriteUserID);

                    DAL.TOUser userDal = new DAL.TOUser();
                    int userCount = userDal.Add(_listinfo.CompanyID, listID, _listinfo.WriteUserID);
                    if (userCount <= 0)
                        throw new InvalidOperationException("No active education targets were found.");

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, listID, listID, _listinfo.HiddenSpoolPath, "");
                        }
                    }
                    #endregion 첨부파일 추가          
                  
                    ts.Complete();
                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmRequest(_listinfo);
                #endregion

                return listID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        #endregion

        #region 수정하기

        /// <summary>
        /// 원본 등록 정보와 기존 대상자의 수료 정보를 보존하며 교육을 수정합니다.
        /// 일반 첨부파일은 선택한 DB 기록만 제거하고 실제 파일은 그대로 둡니다.
        /// </summary>
        public bool Modify(BE.ListInfo _listinfo)
        {
            string failureMessage;
            return Modify(_listinfo, out failureMessage);
        }

        public bool Modify(BE.ListInfo _listinfo, out string failureMessage)
        {
            failureMessage = String.Empty;
            if (!ValidateModify(_listinfo) || !ValidateTargets(_listinfo)) return false;

            try
            {
                bool sendAlarm;
                HashSet<int> alarmReceiverIDs = null;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.List dal = new DAL.List();
                    using (DataSet before = dal.GetModificationAlarmSnapshot(_listinfo.CompanyID, _listinfo.ListID))
                    {
                        if (before.Tables.Count != 4 || before.Tables[0].Rows.Count != 1) return false;
                        if (!IsReminderScheduleUnchanged(before.Tables[0].Rows[0], _listinfo)
                            && !String.IsNullOrEmpty(GetRegistrationAlarmValidationMessage(
                                _listinfo.StartDate, _listinfo.EndDate, _listinfo.AlarmDay, GetRegistrationToday())))
                            return false;
                        int failureReason;
                        if (!dal.Modify(_listinfo, out failureReason))
                        {
                            failureMessage = GetModificationFailureMessage(failureReason);
                            return false;
                        }

                        if (!String.IsNullOrEmpty(_listinfo.HiddenSpoolPath)
                            && !new BLL.Attach().AddForModify(_listinfo.CompanyID, _listinfo.ListID, _listinfo.HiddenSpoolPath))
                            throw new InvalidOperationException("Education modification attachments were not saved.");

                        using (DataSet after = dal.GetModificationAlarmSnapshot(_listinfo.CompanyID, _listinfo.ListID))
                        {
                            sendAlarm = HasModificationAlarmChanges(before, after, _listinfo.OriginalAttachIDs);
                            if (sendAlarm)
                                TryGetTargetChangeAlarmRecipients(before, after, _listinfo.OriginalAttachIDs, out alarmReceiverIDs);
                        }
                    }
                    ts.Complete();
                }

                // 팝업설정/알람 일수만 바뀐 경우에는 저장만 하고 재알림을 발송하지 않습니다.
                // 대상자만 변경했다면 신규 대상자에게 등록 알람을 보내며, 제외만 했을 때는 발송하지 않습니다.
                // 그 외 변경은 기존과 같이 전체 대상자에게 알립니다.
                if (sendAlarm && (alarmReceiverIDs == null || alarmReceiverIDs.Count > 0))
                    new BLL.Alarm().AlarmRequest(_listinfo, isReannouncement: alarmReceiverIDs == null,
                        receiverIDs: alarmReceiverIDs);
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Modify", ex);
                return false;
            }
        }

        private static string GetModificationFailureMessage(int failureReason)
        {
            switch (failureReason)
            {
                case 1:
                    return "완료한 사용자는 대상자에서 제외할 수 없습니다. 해당 사용자 또는 소속 부서를 대상자에 포함해 주세요.";
                case 2:
                    return "관리자 확인이 완료된 사용자는 대상자에서 제외할 수 없습니다. 해당 사용자 또는 소속 부서를 대상자에 포함해 주세요.";
                case 3:
                    return "첨부파일이 등록된 사용자는 대상자에서 제외할 수 없습니다. 해당 사용자 또는 소속 부서를 대상자에 포함해 주세요.";
                default:
                    return String.Empty;
            }
        }

        private static bool HasModificationAlarmChanges(DataSet before, DataSet after, string originalAttachIDs)
        {
            if (before == null || after == null || before.Tables.Count != 4 || after.Tables.Count != 4
                || before.Tables[0].Rows.Count != 1 || after.Tables[0].Rows.Count != 1)
                throw new InvalidOperationException("Education modification comparison data is invalid.");

            for (int tableIndex = 0; tableIndex < before.Tables.Count; tableIndex++)
            {
                DataTable oldTable = before.Tables[tableIndex];
                DataTable newTable = after.Tables[tableIndex];
                if (HasModificationTableChanges(oldTable, newTable, tableIndex == 0)) return true;
            }

            // 기존 파일 삭제는 별도 요청에서 즉시 처리되므로 수정 화면을 열었을 때의 목록도 비교합니다.
            // 이 값은 클라이언트의 알림 여부가 아니라 서버 ViewState에 저장된 최초 AttachID 목록입니다.
            return HasOriginalAttachmentChanges(after.Tables[3], originalAttachIDs);
        }

        private static bool HasModificationTableChanges(DataTable before, DataTable after, bool ignoreAlarmSettings)
        {
            if (before.Columns.Count != after.Columns.Count)
                throw new InvalidOperationException("Education modification comparison columns are invalid.");
            if (before.Rows.Count != after.Rows.Count) return true;
            for (int columnIndex = 0; columnIndex < before.Columns.Count; columnIndex++)
            {
                string column = before.Columns[columnIndex].ColumnName;
                if (column != after.Columns[columnIndex].ColumnName)
                    throw new InvalidOperationException("Education modification comparison columns are invalid.");
                if (ignoreAlarmSettings && (column == "PopUpYN" || column == "AlarmDay")) continue;
                for (int rowIndex = 0; rowIndex < before.Rows.Count; rowIndex++)
                    if (!String.Equals(NormalizeModificationValue(before.Rows[rowIndex][columnIndex]),
                        NormalizeModificationValue(after.Rows[rowIndex][columnIndex]), StringComparison.Ordinal))
                        return true;
            }
            return false;
        }

        private static bool HasOriginalAttachmentChanges(DataTable attachments, string originalAttachIDs)
        {
            if (originalAttachIDs != null)
            {
                HashSet<int> originalAttachments = new HashSet<int>();
                if (originalAttachIDs.Length > 0)
                {
                    foreach (string value in originalAttachIDs.Split(','))
                    {
                        int attachID;
                        if (!Int32.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out attachID)
                            || attachID <= 0 || !originalAttachments.Add(attachID)) return true;
                    }
                }
                HashSet<int> currentAttachments = new HashSet<int>();
                foreach (DataRow attachment in attachments.Rows)
                    currentAttachments.Add(Convert.ToInt32(attachment["AttachID"], CultureInfo.InvariantCulture));
                if (!originalAttachments.SetEquals(currentAttachments)) return true;
            }
            return false;
        }

        private static bool TryGetTargetChangeAlarmRecipients(DataSet before, DataSet after,
            string originalAttachIDs, out HashSet<int> receiverIDs)
        {
            receiverIDs = null;
            if (HasModificationTableChanges(before.Tables[0], after.Tables[0], false)
                || HasModificationTableChanges(before.Tables[3], after.Tables[3], false)
                || HasOriginalAttachmentChanges(after.Tables[3], originalAttachIDs))
                return false;

            HashSet<int> oldUsers = GetModificationUserIDs(before.Tables[2]);
            HashSet<int> newUsers = GetModificationUserIDs(after.Tables[2]);
            newUsers.ExceptWith(oldUsers);
            receiverIDs = newUsers;
            return true;
        }

        private static HashSet<int> GetModificationUserIDs(DataTable users)
        {
            HashSet<int> userIDs = new HashSet<int>();
            foreach (DataRow user in users.Rows)
                userIDs.Add(Convert.ToInt32(user["UserID"], CultureInfo.InvariantCulture));
            return userIDs;
        }

        private static bool IsReminderScheduleUnchanged(DataRow original, BE.ListInfo info)
        {
            return String.Equals(Convert.ToString(original["StartDate"], CultureInfo.InvariantCulture), info.StartDate, StringComparison.Ordinal)
                && String.Equals(Convert.ToString(original["EndDate"], CultureInfo.InvariantCulture), info.EndDate, StringComparison.Ordinal)
                && Convert.ToInt32(original["AlarmDay"], CultureInfo.InvariantCulture) == info.AlarmDay;
        }

        private static string NormalizeModificationValue(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture).Replace("\r\n", "\n").Replace("\r", "\n");
        }

        private bool ValidateModify(BE.ListInfo listInfo)
        {
            if (listInfo == null || listInfo.CompanyID <= 0 || listInfo.ListID <= 0 || listInfo.WriteUserID <= 0
                || String.IsNullOrWhiteSpace(listInfo.Title) || listInfo.Title.ToXSS().Length > 100
                || String.IsNullOrWhiteSpace(listInfo.ItemID) || listInfo.ItemID.ToXSS().Length > 10
                || (listInfo.PopUpYN != "Y" && listInfo.PopUpYN != "N")
                || !BE.ListInfo.IsSupportedAlarmDay(listInfo.AlarmDay))
                return false;

            DateTime startDate;
            DateTime endDate;
            if (!DateTime.TryParseExact(listInfo.StartDate, "yyyyMMdd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out startDate)
                || !DateTime.TryParseExact(listInfo.EndDate, "yyyyMMdd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out endDate) || startDate > endDate)
                return false;

            // 특정 일수 상한은 두지 않되 SQL datetime으로 계산할 수 없는 날짜는 저장하지 않습니다.
            if (listInfo.AlarmDay > 0
                && (endDate - System.Data.SqlTypes.SqlDateTime.MinValue.Value.Date).Days < listInfo.AlarmDay)
                return false;

            if (!String.IsNullOrEmpty(listInfo.ToUserList) && listInfo.ToUserList.Split(',').Length > 1000)
                return false;

            listInfo.DeleteAttachIDs = (listInfo.DeleteAttachIDs ?? String.Empty).Trim();
            if (listInfo.DeleteAttachIDs.Length == 0) return true;

            string[] attachments = listInfo.DeleteAttachIDs.Split(',');
            if (attachments.Length > 1000) return false;
            HashSet<int> selectedAttachments = new HashSet<int>();
            System.Collections.Generic.List<string> normalizedAttachments = new System.Collections.Generic.List<string>();
            foreach (string attachment in attachments)
            {
                int attachID;
                if (!Int32.TryParse(attachment.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out attachID)
                    || attachID <= 0 || !selectedAttachments.Add(attachID))
                    return false;
                normalizedAttachments.Add(attachID.ToString(CultureInfo.InvariantCulture));
            }
            listInfo.DeleteAttachIDs = String.Join(",", normalizedAttachments.ToArray());
            return true;
        }

        #endregion

        #region 등록/수정 알람 검증

        public static DateTime GetRegistrationToday()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time")).Date;
        }

        public static string GetRegistrationAlarmValidationMessage(string startDate, string endDate,
            int alarmDay, DateTime today)
        {
            const string invalidAlarmMessage = "알람 예정일은 종료일 기준 유효한 일자만 선택 가능합니다.";
            const string sameDayAlarmMessage = "알람 예정일은 당일로 선택할 수 없습니다. 해당 교육 내 미완료알람발송 기능을 이용 바랍니다.";
            if (!BE.ListInfo.IsSupportedAlarmDay(alarmDay))
                return invalidAlarmMessage;
            if (alarmDay == 0) return String.Empty;

            DateTime endDateValue;
            if (!DateTime.TryParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out endDateValue))
                return invalidAlarmMessage;

            DateTime startDateValue;
            if (!DateTime.TryParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out startDateValue))
                return invalidAlarmMessage;
            if (startDateValue > endDateValue)
                return invalidAlarmMessage;

            int remainingDays = (endDateValue - today.Date).Days;
            if (remainingDays == alarmDay)
                return sameDayAlarmMessage;

            // 날짜를 직접 차감하지 않아 큰 알람 일수에도 날짜 범위 초과가 발생하지 않습니다.
            if ((endDateValue - startDateValue).Days < alarmDay)
                return invalidAlarmMessage;

            // 종료일 - 알람 일수가 내일 이후인지 날짜 차이로 확인합니다. 당일과 과거는 허용하지 않습니다.
            if (remainingDays < alarmDay)
                return invalidAlarmMessage;

            return String.Empty;
        }

        #endregion

        #region 대상 검증

        /// <summary>
        /// 선택한 사용자와 부서의 대상 문자열 형식을 검증합니다.
        /// </summary>
        private bool ValidateTargets(BE.ListInfo listInfo)
        {
            if (listInfo == null) return false;

            string targetValue = (listInfo.ToUserList ?? string.Empty).Trim();

            if (targetValue.Length == 0) return false;

            string[] targets = targetValue.Split(',');
            System.Collections.Generic.List<string> normalizedTargets = new System.Collections.Generic.List<string>();
            HashSet<string> duplicateCheck = new HashSet<string>(StringComparer.Ordinal);

            foreach (string target in targets)
            {
                string[] values = target.Trim().Split('_');
                int ownerID;
                int deptID;

                if (values.Length != 3
                    || !int.TryParse(values[1], out ownerID)
                    || !int.TryParse(values[2], out deptID))
                    return false;

                bool isMember = values[0] == "M" && ownerID > 0 && deptID > 0;
                bool isGroup = values[0] == "G" && ownerID == 0 && deptID > 0;
                if (!isMember && !isGroup) return false;

                string normalizedTarget = values[0] + "_" + ownerID.ToString() + "_" + deptID.ToString();
                if (!duplicateCheck.Add(normalizedTarget)) return false;

                normalizedTargets.Add(normalizedTarget);
            }

            listInfo.ToUserList = string.Join(",", normalizedTargets.ToArray());
            return normalizedTargets.Count > 0;
        }

        #endregion
    }
}
