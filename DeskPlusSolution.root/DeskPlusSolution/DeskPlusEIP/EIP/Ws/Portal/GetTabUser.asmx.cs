using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Portal
{
    /// <summary>
    /// Menu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetTabUser : Base.BaseWSEIP
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 탭 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModTab(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int tabID = GetDecryptParam("tabID", "0").ToNumber<int>();
                string name = GetDecryptParam("name", "");
                string multiName = GetDecryptParam("multiName", "");
                int layout = GetDecryptParam("layout", "0").ToNumber<int>();
                string activeTab = GetDecryptParam("activeTab", "");

                if (tabID > 0 && name.Length > 0)
                {
                    BE.TabInfo info = new BE.TabInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.NameBase = name;
                    info.TabID = tabID;

                    if (multiName != "" && multiName.IndexOf(",") != -1)
                    {
                        info.NameKo = multiName.Split(',')[0];
                        info.NameEn = multiName.Split(',')[1];
                        info.NameJa = multiName.Split(',')[2];
                        info.NameFr = multiName.Split(',')[3];
                        info.NameZhcn = multiName.Split(',')[4];
                        info.NameZhtw = multiName.Split(',')[5];
                    }

                    info.TabOrder = 0;
                    info.TabLayout = layout;

                    info.ActiveTab = activeTab;
                    info.IsModify = "0"; //수정불가
                    info.IsAdmin = "0"; //사용자용
                    info.WriteUserID = CurrentUserInfo.UserID;

                    DAL.Tab dal = new DAL.Tab();
                    return dal.Mod(info);
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModTab", ex);
                return false;
            }
        }
    }
}
