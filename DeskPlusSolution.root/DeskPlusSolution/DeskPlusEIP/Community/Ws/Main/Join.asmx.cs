using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Main
{
    /// <summary>
    /// Join의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Join : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 퀴즈형 커뮤니티가입 시 답변 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAnswer(string p)
        {
            try 
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();

                DAL.Com dal = new DAL.Com();
                DataTableReader dtr = dal.GetComunifyInfo(CurrentUserInfo.CompanyID, communityID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ComInfo info = new ComInfo();
                            info.Answer = dtr["Answer"].ToString().ToReverseXSS();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-10
        /// 작성목적 : 비공개 커뮤니티가입하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddUserApply(string p)
        {
            try 
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                string description = GetDecryptParam("description", "");
                string communityName = GetDecryptParam("communityName", "");
                int type = GetDecryptParam("type", "0").ToNumber<int>();
                int joinType = GetDecryptParam("joinType", "1").ToNumber<int>();

                BE.UserApplyInfo info = new UserApplyInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.UserID = CurrentUserInfo.UserID;
                info.UserName = CurrentUserInfo.Name;
                info.Description = description;

                string usermail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                BLL.UserApply bll = new BLL.UserApply();
                bll.Add(info, communityName, PreLang, CurrentUserInfo.LogInID, type, joinType, usermail);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion
    }
}
