using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Admin.General
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-30
        /// 작성목적 : 커뮤니티 시샵 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();

                List<object> listobj = new List<object>();

                DAL.User dal = new DAL.User();
                DataTableReader dtr = dal.GetGradeList(CurrentUserInfo.CompanyID, communityID, 2);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UserInfo info = new UserInfo();

                            #region 다국어 이름
                            info.UserName = GetLangName(dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(),
                            dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(),
                            dtr["NameZhcn"].ToString(),
                            dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                            #endregion

                            #region 다국어 직급
                            info.Title = GetLangName(dtr["TitleBase"].ToString(),
                            dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(),
                            dtr["TitleJa"].ToString(),
                            dtr["TitleFr"].ToString(),
                            dtr["TitleZhcn"].ToString(),
                            dtr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();
                            #endregion

                            #region 다국어 부서
                            info.Dept = GetLangName(dtr["DeptBase"].ToString(),
                            dtr["DeptKo"].ToString(),
                            dtr["DeptEn"].ToString(),
                            dtr["DeptJa"].ToString(),
                            dtr["DeptFr"].ToString(),
                            dtr["DeptZhcn"].ToString(),
                            dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();
                            #endregion

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

        #region MOD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-30
        /// 작성목적 : 커뮤니티 시샵 변경
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModGrade(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();  // 변경 할 커뮤니티 지기
                int adminUserID = 0;        // 기존 커뮤니티 지기

                #region 기존 커뮤니티 지기 정보 가져오기
                DAL.User dal = new DAL.User();

                DataTableReader dtrManager = dal.GetGradeList(CurrentUserInfo.CompanyID, communityID, 2);

                if (dtrManager != null)
                {
                    if (dtrManager.HasRows)
                    {
                        while (dtrManager.Read())
                        {
                            adminUserID = dtrManager["UserID"].ToString().ToNumber<int>();
                        }
                    }
                }
                #endregion

                DAL.UserGroupMem dalUserGroupMem = new DAL.UserGroupMem();
                if (userID > 0 && adminUserID > 0)
                {                    
                    // 1. 새로운 시샵으로 수정
                    dalUserGroupMem.Mod(CurrentUserInfo.CompanyID, communityID, userID, 2);
                    // 2. 기존 시샵은 우수회원으로 수정
                    dalUserGroupMem.Mod(CurrentUserInfo.CompanyID, communityID, adminUserID, 5);
                }
                else
                {
                    //기존 시샵이 없을 경우(퇴사하여 탈퇴한 경우)
                    dalUserGroupMem.Mod(CurrentUserInfo.CompanyID, communityID, userID, 2);
                }
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
