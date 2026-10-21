using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data.SqlClient;
using System.Configuration;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base;
using ObjList = System.Collections.Generic.List<System.Object>;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Content.Admin
{
    /// <summary>
    /// AuthList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AuthList : Base.BaseWSProposal
    {

        /// <summary>
        /// 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            try
            {
                SetDecryptString(p);

                string authList = GetDecryptParam("authList", "");
                int degree = GetDecryptParam("degree", "1").ToNumber<int>();
                string status = GetDecryptParam("status", "A");

                List<BE.JudgeInfo> alList = new List<BE.JudgeInfo>();

                if (authList.Length > 0)
                {
                    foreach (var temp in authList.Split('^'))
                    {
                        BE.JudgeInfo info = new BE.JudgeInfo();

                        info.UserID = temp.ToNumber<int>();
                        info.Degree = degree;
                        info.Status = status;

                        alList.Add(info);
                    }

                    BLL.Judge biz = new BLL.Judge();
                    return biz.Mod(CurrentUserInfo.CompanyID, alList);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
