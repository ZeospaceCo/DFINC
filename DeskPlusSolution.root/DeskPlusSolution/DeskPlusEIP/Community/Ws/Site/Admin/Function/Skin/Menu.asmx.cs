using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Admin.Function.Skin
{
    /// <summary>
    /// Menu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Menu : Base.BaseWSCommunity
    {

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 메뉴 스타일 수정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModStyle(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                string titleColor = GetDecryptParam("titleColor", "");
                string titleImage = GetDecryptParam("titleImage", "");
                string menuColor1 = GetDecryptParam("menuColor1", "");
                string menuColor2 = GetDecryptParam("menuColor2", "");
                string menuColor3 = GetDecryptParam("menuColor3", "");
                string menuBlock1TextClass = GetDecryptParam("menuBlock1TextClass", "");
                string menuBlock2TextClass = GetDecryptParam("menuBlock2TextClass", "");

                BE.ComDetailInfo info = new BE.ComDetailInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.TitleImage = titleImage;
                info.TitleTextColor = titleColor;
                info.MenuColor1 = menuColor1;
                info.MenuColor2 = menuColor2;
                info.MenuColor3 = menuColor3;
                info.MenuBlock1TextClass = menuBlock1TextClass;
                info.MenuBlock2TextClass = menuBlock2TextClass;

                BLL.ComDetail bll = new BLL.ComDetail();

                return bll.Mod(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion 

    }
}
