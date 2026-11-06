using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Organization
{
    public partial class Default : Base.BasePageAccount
    {
        protected string ChartJson { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            ChartJson = "{\"departments\":[],\"members\":[],\"error\":true}";

            // 회사 식별자는 URL이나 클라이언트 입력으로 받지 않는다.
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0) return;

            try
            {
                using (DataSet data = new DAL.OrganizationChart().GetChart(CurrentUserInfo.CompanyID))
                {
                    var chart = new
                    {
                        departments = data.Tables["Departments"].AsEnumerable().Select(row => new
                        {
                            id = Convert.ToInt32(row["DeptID"]),
                            parentId = row.IsNull("ParentDeptID") ? 0 : Convert.ToInt32(row["ParentDeptID"]),
                            name = LocalName(row, "Name"),
                            phone = row["Phone"].ToString()
                        }).ToArray(),
                        members = data.Tables["Members"].AsEnumerable().Select(row => new
                        {
                            id = Convert.ToInt32(row["UserID"]),
                            deptId = Convert.ToInt32(row["DeptID"]),
                            primary = row["BaseDept"].ToString() == "1",
                            name = LocalName(row, "Name"),
                            title = LocalName(row, "Title"),
                            duty = LocalName(row, "Duty"),
                            officePhone = row["OfficePhone"].ToString(),
                            extension = row["ExtensionOfficePhone"].ToString(),
                            mobile = row["MobilePhone"].ToString(),
                            photo = PhotoUrl(Convert.ToInt32(row["UserID"]), row["PhotoImage"].ToString()),
                            email = row["Email"].ToString()
                        }).ToArray(),
                        currentDeptId = CurrentUserInfo.DeptID,
                        error = false
                    };

                    var serializer = new JavaScriptSerializer { MaxJsonLength = 16 * 1024 * 1024 };
                    // application/json 블록을 조기에 닫을 수 없도록 HTML 특수문자를 이스케이프한다.
                    ChartJson = serializer.Serialize(chart).Replace("&", "\\u0026")
                        .Replace("<", "\\u003c").Replace(">", "\\u003e")
                        .Replace("\u2028", "\\u2028").Replace("\u2029", "\\u2029");
                }
            }
            catch (Exception ex)
            {
                AppException.WriteException("OrganizationChart : " + ex);
            }
        }

        private string LocalName(DataRow row, string prefix)
        {
            return GetLangName(row[prefix + "Base"].ToString(), row[prefix + "Ko"].ToString(),
                row[prefix + "En"].ToString(), row[prefix + "Ja"].ToString(),
                row[prefix + "Fr"].ToString(), row[prefix + "Zhcn"].ToString(),
                row[prefix + "Zhtw"].ToString(), PreLang);
        }

        private string PhotoUrl(int userID, string photo)
        {
            if (String.IsNullOrWhiteSpace(photo)) return String.Empty;
            string directory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskPlusEIP/FileData/");
            string path = directory.TrimEnd('/') + "/" + CurrentUserInfo.CompanyID + "/Users/" + userID + "/Picture/" + System.IO.Path.GetFileName(photo);
            if (!System.IO.File.Exists(Server.MapPath(path))) return String.Empty;
            var parameters = new Dictionary<string, string> { { "filePath", path } };
            string encrypted = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(parameters)));
            return ResolveUrl("~/Common/DownLoad/ImageView.aspx?p=") + HttpUtility.UrlEncode(encrypted);
        }
    }
}
