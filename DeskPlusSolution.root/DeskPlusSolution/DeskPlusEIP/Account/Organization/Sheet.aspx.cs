using System;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;

namespace Zeospace.DeskPlus.Account.Web.Organization
{
    public partial class Sheet : Base.BasePageAccount
    {
        protected string SheetJson { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            SheetJson = "{\"error\":true}";
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0) return;
            try
            {
                bool canEdit = OrganizationSheetLayout.CanEdit(CurrentUserInfo);
                SheetJson = OrganizationSheetLayout.SafeJson(new
                {
                    error = false, canUpload = canEdit,
                    token = canEdit ? OrganizationSheetLayout.GetToken(Context) : "",
                    endpoint = ResolveUrl("~/Account/Organization/Upload.ashx"),
                    state = OrganizationSheetLayout.Load(CurrentUserInfo.CompanyID, Server.MapPath("~/Account/App_Data/OrganizationSheetTemplate.json"))
                });
            }
            catch (Exception ex) { AppException.WriteException("OrganizationSheet.Load : " + ex); }
        }
    }
}
