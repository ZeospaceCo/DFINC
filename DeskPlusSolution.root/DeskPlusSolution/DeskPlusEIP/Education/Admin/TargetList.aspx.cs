using System;
using System.Data;
using System.Globalization;
using System.Web;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class TargetList : Base.BasePageEducationAdmin
    {
        public bool HasEducation { get; private set; }
        public int TotalCount { get; private set; }
        public int RecordPerPage = 5;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (!AccessAuth)
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;
                form1.Visible = false;
                return;
            }

            int listID;
            if (!Int32.TryParse(GetDecryptParam("listID", ""), NumberStyles.None,
                CultureInfo.InvariantCulture, out listID) || listID <= 0)
            {
                ShowListError();
                return;
            }
            ListID = listID;

            using (DataTableReader dtr = new DAL.List().GetList(CurrentUserInfo.CompanyID,
                ListID.ToString(CultureInfo.InvariantCulture)))
            {
                if (dtr == null || !dtr.Read())
                {
                    ShowListError();
                    return;
                }
            }

            int pageSize;
            if (Int32.TryParse(GetDecryptParam("recordPerPage", "5"), NumberStyles.None,
                CultureInfo.InvariantCulture, out pageSize)
                && (pageSize == 5 || pageSize == 10 || pageSize == 20 || pageSize == 30))
                RecordPerPage = pageSize;

            ddlListCount.SelectedValue = RecordPerPage.ToString(CultureInfo.InvariantCulture);
            TotalCount = new DAL.TOUser().GetTargetListCount(CurrentUserInfo.CompanyID, ListID);
            HasEducation = true;
            phTargetList.Visible = true;
        }

        private void ShowListError()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            ltlMessage.Text = "조회할 수 없는 내역입니다.";
        }
    }
}
