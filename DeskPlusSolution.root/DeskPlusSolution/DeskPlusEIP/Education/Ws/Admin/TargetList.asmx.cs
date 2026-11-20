using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.Web.Ws.Admin
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class TargetList : Base.BaseWSEducation
    {
        /// <summary>
        /// 서버의 전체 건수로 출력 건수와 시작 위치를 계산합니다.
        /// </summary>
        private string SetPaging(int totalCount, int pageSize, int pageNo)
        {
            if (pageSize != 5 && pageSize != 10 && pageSize != 20 && pageSize != 30)
                pageSize = 5;
            if (totalCount <= 0) return "0,0";

            int pageCount = ((totalCount - 1) / pageSize) + 1;
            int pageIndex = Math.Min(Math.Max(pageNo, 1), pageCount) - 1;
            int pageOffset = pageSize * pageIndex;
            int outputCount = Math.Min(pageSize, totalCount - pageOffset);
            return outputCount.ToString(CultureInfo.InvariantCulture) + ","
                + pageOffset.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 해당 전체 대상자를 페이지별로 조회합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            CheckAccess();
            SetDecryptString(p);
            int listID = GetListID();
            int pageSize;
            if (!Int32.TryParse(GetDecryptParam("pageSize", "5"), NumberStyles.None,
                CultureInfo.InvariantCulture, out pageSize)
                || (pageSize != 5 && pageSize != 10 && pageSize != 20 && pageSize != 30))
                pageSize = 5;
            int pageNo;
            if (!Int32.TryParse(GetDecryptParam("pageSizePageNo", "1"), NumberStyles.None,
                CultureInfo.InvariantCulture, out pageNo) || pageNo < 1)
                pageNo = 1;

            DAL.TOUser dal = new DAL.TOUser();
            int totalCount = dal.GetTargetListCount(CurrentUserInfo.CompanyID, listID);
            string[] paging = SetPaging(totalCount, pageSize, pageNo).Split(',');
            int outputCount = Int32.Parse(paging[0], CultureInfo.InvariantCulture);
            int pageOffset = Int32.Parse(paging[1], CultureInfo.InvariantCulture);
            List<object> users = new List<object>();
            if (outputCount == 0) return users;

            using (DataTableReader dtr = dal.GetTargetPageList(CurrentUserInfo.CompanyID,
                listID, outputCount, pageOffset))
            {
                if (dtr != null)
                {
                    while (dtr.Read())
                    {
                        BE.TargetListInfo info = new BE.TargetListInfo();
                        info.WorkplaceName = GetUserName(dtr, "Workplace");
                        info.DeptName = GetUserName(dtr, "Dept");
                        info.TitleName = GetUserName(dtr, "Title");
                        info.UserName = GetUserName(dtr, "Name");
                        if (String.IsNullOrWhiteSpace(info.UserName))
                            info.UserName = "사용자 " + dtr["UserID"].ToString();
                        switch (dtr["ProgressStatus"].ToString())
                        {
                            case "1": info.StatusStr = "대기"; break;
                            case "2": info.StatusStr = "진행"; break;
                            case "3": info.StatusStr = "완료"; break;
                            case "4": info.StatusStr = "지연"; break;
                            default: info.StatusStr = ""; break;
                        }
                        info.CompleteDateStr = GetCompleteDate(dtr["CompleteDate"]);
                        users.Add(info);
                    }
                }
            }
            return users;
        }

        private void CheckAccess()
        {
            if (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                && !CurrentUserInfo.IsInRole("Education"))
                throw new HttpException(403, "조회 권한이 없습니다.");
        }

        private int GetListID()
        {
            int listID;
            if (!Int32.TryParse(GetDecryptParam("listID", ""), NumberStyles.None,
                CultureInfo.InvariantCulture, out listID) || listID <= 0)
                throw new HttpException(404, "조회할 수 없는 내역입니다.");

            using (DataTableReader dtr = new DAL.List().GetList(CurrentUserInfo.CompanyID,
                listID.ToString(CultureInfo.InvariantCulture)))
            {
                if (dtr == null || !dtr.Read())
                    throw new HttpException(404, "조회할 수 없는 내역입니다.");
            }
            return listID;
        }

        private string GetUserName(DataTableReader dtr, string field)
        {
            return GetLangName(dtr[field + "Base"].ToString(), dtr[field + "Ko"].ToString(),
                dtr[field + "En"].ToString(), dtr[field + "Ja"].ToString(),
                dtr[field + "Fr"].ToString(), dtr[field + "Zhcn"].ToString(),
                dtr[field + "Zhtw"].ToString(), PreLang).ToReverseXSS();
        }

        private static string GetCompleteDate(object value)
        {
            if (value == null || value == DBNull.Value) return "";
            if (value is DateTime)
                return ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            DateTime completeDate;
            string date = Convert.ToString(value, CultureInfo.InvariantCulture).Trim();
            if (DateTime.TryParseExact(date, new[] { "yyyyMMdd", "yyyy-MM-dd" },
                CultureInfo.InvariantCulture, DateTimeStyles.None, out completeDate))
                return completeDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return "";
        }
    }
}
