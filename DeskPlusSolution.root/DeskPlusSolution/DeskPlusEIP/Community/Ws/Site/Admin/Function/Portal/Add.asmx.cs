using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Admin.Function.Portal
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 사용중인 메뉴 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetUsingList(string p)
        {
            try 
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();

                DAL.Menu dal = new DAL.Menu();
                DataTableReader dtr = dal.GetUsingMenuList(CurrentUserInfo.CompanyID, communityID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.MenuInfo info = new MenuInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();

                            #region 다국메뉴명
                            info.MenuName = GetLangName(dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(),
                                dtr["NameEn"].ToString(),
                                dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(),
                                dtr["NameZhcn"].ToString(),
                                dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
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

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 사용중인 메뉴 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShowList(string p)
        {
            try
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();

                DAL.Menu dal = new DAL.Menu();
                DataTableReader dtr = dal.GetShowMenuList(CurrentUserInfo.CompanyID, communityID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.MenuInfo info = new MenuInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();

                            #region 다국메뉴명
                            info.MenuName = GetLangName(dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(),
                                dtr["NameEn"].ToString(),
                                dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(),
                                dtr["NameZhcn"].ToString(),
                                dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
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

        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 메뉴 추가(시작화면에 보여줄 메뉴로 추가)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MenuAdd(string p)
        {
            try 
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                DAL.MenuDisplay dal = new DAL.MenuDisplay();
                BE.MenuDisplayInfo info = new MenuDisplayInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.MenuID = menuID;

                dal.Add(info);
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 메뉴 추가(시작화면에 보여줄 메뉴로 추가)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MenuDel(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                DAL.MenuDisplay dal = new DAL.MenuDisplay();
                dal.Del(CurrentUserInfo.CompanyID, communityID, menuID);
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
