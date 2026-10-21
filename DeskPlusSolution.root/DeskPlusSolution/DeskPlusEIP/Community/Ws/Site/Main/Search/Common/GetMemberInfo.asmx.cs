using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Main.Search.Common
{
    /// <summary>
    /// GetMemberInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetMemberInfo : Base.BaseWSCommunity
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-12
        /// 작성목적 : 자기 소개 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UserMod(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                string description = GetDecryptParam("description", "");

                BE.UserInfo info = new BE.UserInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.UserID = CurrentUserInfo.UserID;  
                info.Description = description;

                DAL.User dal = new DAL.User();
                return dal.Mod(info);                 
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-12
        /// 작성목적 : 회원 탈퇴
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UserDrop(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int status = GetDecryptParam("status", "0").ToNumber<int>();

                DAL.User dal = new DAL.User();
                return dal.ModStatus(CurrentUserInfo.CompanyID, communityID, CurrentUserInfo.UserID.ToString(), status);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        
    }
}
