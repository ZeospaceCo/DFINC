using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Education.Web.Content
{
    public partial class PopUp : Base.BasePageEducation
    {
        public string CookieName = String.Empty;
        public string ContentUrl = "about:blank";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            phPopupActions.Visible = false;
            CookieName = String.Empty;
            ContentUrl = "about:blank";

            string popID = GetParm("popid", "");
            if (CurrentUserInfo == null || popID.Length > 64
                || !Regex.IsMatch(popID, @"\AEDU_[0-9]+_[0-9]+_[0-9]+\z"))
            {
                SetNotFound();
                return;
            }

            string[] parts = popID.Split('_');
            int companyID;
            int userID;
            int listID;
            if (!Int32.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out companyID)
                || !Int32.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out userID)
                || !Int32.TryParse(parts[3], NumberStyles.None, CultureInfo.InvariantCulture, out listID)
                || companyID <= 0 || userID <= 0 || listID <= 0
                || companyID != CurrentUserInfo.CompanyID || userID != CurrentUserInfo.UserID)
            {
                SetNotFound();
                return;
            }

            DateTime koreaNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time"));
            bool eligible = false;
            using (DataTableReader reader = new DAL.List().GetList(companyID, userID,
                koreaNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture), listID))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        int eligibleListID;
                        if (Int32.TryParse(reader["ListID"].ToString(), NumberStyles.None,
                            CultureInfo.InvariantCulture, out eligibleListID) && eligibleListID == listID)
                        {
                            eligible = true;
                            break;
                        }
                    }
                }
            }
            if (!eligible)
            {
                SetNotFound();
                return;
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("listID", listID.ToString(CultureInfo.InvariantCulture));
            parameters.Add("isPopup", "Y");
            string encrypted = GetEncryptParam(Utility.GetParameterDic(parameters));
            if (String.IsNullOrEmpty(encrypted) || encrypted == "!NULL")
            {
                SetNotFound();
                return;
            }

            ContentUrl = "/DeskPlusEIP/Education/Content/View.aspx?p=" + HttpUtility.UrlEncode(encrypted);
            CookieName = String.Format(CultureInfo.InvariantCulture, "EDU_{0}_{1}_{2}", companyID, userID, listID);
            phPopupActions.Visible = true;
        }

        private void SetNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
        }
    }
}
