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

namespace Zeospace.DeskPlus.Education.BLL
{
    public class List : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
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
                    if (_listinfo.AllTargetYN == "Y")
                    {
                        int selectionCount = toDal.AddAll(_listinfo.CompanyID, listID, _listinfo.WriteUserID);
                        if (selectionCount <= 0)
                            throw new InvalidOperationException("All-user selection was not saved.");
                    }
                    else
                    {
                        toDal.Add(_listinfo.CompanyID, listID, _listinfo.ToUserList, _listinfo.WriteUserID);
                    }

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

        /// <summary>
        /// 전체 대상과 개인/부서 대상을 혼합할 수 없도록 하고 대상 문자열 형식을 검증합니다.
        /// </summary>
        private bool ValidateTargets(BE.ListInfo listInfo)
        {
            if (listInfo == null) return false;

            string allTargetYN = (listInfo.AllTargetYN ?? "N").Trim().ToUpperInvariant();
            string targetValue = (listInfo.ToUserList ?? string.Empty).Trim();

            if (allTargetYN != "Y" && allTargetYN != "N") return false;

            if (allTargetYN == "Y")
            {
                // 이전 UI의 G_0_0 전체 표식은 허용합니다. 전체선택은 AddAll에서 A행으로 별도 저장하므로
                // 개인/부서 문자열인 ToUserList에는 표식을 혼합하지 않고 빈 값으로 정규화합니다.
                if (targetValue.Length > 0 && targetValue != "G_0_0") return false;

                listInfo.AllTargetYN = "Y";
                listInfo.ToUserList = string.Empty;
                return true;
            }

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

            listInfo.AllTargetYN = "N";
            listInfo.ToUserList = string.Join(",", normalizedTargets.ToArray());
            return normalizedTargets.Count > 0;
        }

        #endregion
    }
}
