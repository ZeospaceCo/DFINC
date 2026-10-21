using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// Opinion의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MySpace : Base.BaseWSPortlet
    {
        /// <summary>
        /// 메모 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();

            try
            {
                //if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                //{
                //    if (roleList.Length > 0)
                //    {
                //        List<BE.TabRoleInfo> al = new List<BE.TabRoleInfo>();

                //        foreach (string _authValue in roleList.Split(','))
                //        {
                //            BE.TabRoleInfo info = new BE.TabRoleInfo();
                //            info.CompanyID = CurrentUserInfo.CompanyID;
                //            info.TabID = tabID;
                //            info.RoleID = _authValue.ToNumber<int>();

                //            al.Add(info);
                //        }

                //        BLL.TabRole biz = new BLL.TabRole();
                //        biz.AddTx(al);

                //        return true;
                //    }
                //}

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddTabRole : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 잔여용량 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<MySpaceInfo> GetContent(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            try
            {
                MySpaceInfo info = new MySpaceInfo();
                info.MailPercent = "100%";
                info.WebHardPercent = "50%";
                info.BoardPercent = "100%";
                info.ApprovalPercent = "100%";

                info.MailSpace = "1200MB";
                info.WebHardSpace = "1200MB";
                info.BoardSpace = "1200MB";
                info.ApprovalSpace = "1200MB";

                info.MailTotSpace = "150GB";
                info.WebHardTotSpace = "500GB";
                info.BoardTotSpace = "50GB";
                info.ApprovalTotSpace = "0.9GB";

                List<MySpaceInfo> al = new List<MySpaceInfo>();
                al.Add(info);
                return al;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetContent : " + ex);
                throw;
            }
        }

    }

    /// <summary>
    /// 잔여용량
    /// </summary>
    [Serializable]   
    public class MySpaceInfo
    {
        public MySpaceInfo() { }
        
        public string MailPercent { get; set; }
        public string WebHardPercent { get; set; }
        public string BoardPercent { get; set; }
        public string ApprovalPercent { get; set; }

        public string MailSpace { get; set; }
        public string WebHardSpace { get; set; }
        public string BoardSpace { get; set; }
        public string ApprovalSpace { get; set; }

        public string MailTotSpace { get; set; }
        public string WebHardTotSpace { get; set; }
        public string BoardTotSpace { get; set; }
        public string ApprovalTotSpace { get; set; }
    }
}
