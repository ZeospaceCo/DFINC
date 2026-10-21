using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// AuthList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class AuthList : Base.BaseWSReport
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
                //if (authList.Length == 0) return false;

                List<BE.ShareUserInfo> alList = new List<BE.ShareUserInfo>();

                //50$1,10$0

                if (authList.Length > 0)
                {
                    int targetUserID;
                    string useComplete = string.Empty;

                    foreach (var temp in authList.Split(','))
                    {
                        //50$1
                        targetUserID = temp.Split('$')[0].ToNumber<int>();
                        useComplete = temp.Split('$')[1];

                        BE.ShareUserInfo info = new BE.ShareUserInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = CurrentUserInfo.UserID;
                        info.TargetUserID = targetUserID;
                        info.UseComplete = useComplete;

                        alList.Add(info);
                    }

                    BLL.ShareUser biz = new BLL.ShareUser();
                    return biz.Mod(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, alList);
                }
                else
                {
                    DAL.ShareUser dalAuth = new DAL.ShareUser();
                    return dalAuth.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
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