using System;
using System.Data;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.Web.Base
{
    internal static class AdminConfirmation
    {
        internal static BE.AdminListInfo Read(IDataRecord reader, string language)
        {
            BE.AdminListInfo info = new BE.AdminListInfo();
            info.CompanyID = Convert.ToInt32(reader["CompanyID"], CultureInfo.InvariantCulture);
            info.ListID = Convert.ToInt32(reader["ListID"], CultureInfo.InvariantCulture);
            info.UserID = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
            Apply(info, reader, language);
            return info;
        }

        // 목록, 사용자별 상세, 저장 응답, 엑셀에서 같은 확인 정보를 표시합니다.
        internal static void Apply(BE.AdminListInfo info, IDataRecord reader, string language)
        {
            info.IsConfirmed = String.Equals(reader["ConfirmYN"].ToString(), "Y", StringComparison.Ordinal);
            info.ConfirmUserID = 0;
            info.ConfirmUserName = "";
            info.ConfirmDateStr = "";
            if (!info.IsConfirmed) return;

            int actorID;
            if (Int32.TryParse(reader["ConfirmUserID"].ToString(), NumberStyles.None,
                CultureInfo.InvariantCulture, out actorID) && actorID > 0)
            {
                info.ConfirmUserID = actorID;
                info.ConfirmUserName = UtilHelper.GetLangName(reader["ConfirmNameBase"].ToString(),
                    reader["ConfirmNameKo"].ToString(), reader["ConfirmNameEn"].ToString(),
                    reader["ConfirmNameJa"].ToString(), reader["ConfirmNameFr"].ToString(),
                    reader["ConfirmNameZhcn"].ToString(), reader["ConfirmNameZhtw"].ToString(), language).ToReverseXSS();
            }
            if (reader["ConfirmDate"] is DateTime)
                info.ConfirmDateStr = ((DateTime)reader["ConfirmDate"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}
