using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.IO;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Zeospace.DeskPlus.Portlet.Web.Base;
using System.Web.Caching;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Portal
{
    /// <summary>
    /// Main의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Main : Base.BaseWSEIP
    {        

        #region 추가/수정/삭제

        /// <summary>
        /// 탭별 포틀릿 정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderPortlet(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int tabID = GetDecryptParam("tabID", "0").ToNumber<int>();
            string columnID = GetDecryptParam("columnID", "");
            string portletList = GetDecryptParam("portletList", "");

            try
            {
                if (tabID > 0 && columnID.Length > 0 && portletList.Length > 0)
                {
                    DAL.TabPortlet dal = new DAL.TabPortlet();
                    dal.ModrowSequence(CurrentUserInfo.CompanyID, tabID, columnID, portletList);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModOrderPortlet : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 탭별 포틀릿 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelTabPortlet(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int tabPortletID = GetDecryptParam("tabPortletID", "0").ToNumber<int>();

            try
            {
                if (tabPortletID > 0)
                {
                    DAL.TabPortlet dal = new DAL.TabPortlet();
                    dal.Del(CurrentUserInfo.CompanyID, tabPortletID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelTabPortlet : " + ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 탭포틀릿별 사용자 포틀릿설정값 추가 및 수정하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddTabPortletUser(string p)
        {
            try
            {
                SetDecryptString(p);

                int tabPortletID = GetDecryptParam("tabPortletID", "0").ToString().ToNumber<int>();
                int tabID = GetDecryptParam("tabID", "0").ToString().ToNumber<int>();
                int portletID = GetDecryptParam("portletID", "0").ToString().ToNumber<int>();
                string isMinimize = GetDecryptParam("isMinimize", "0").ToString(); // 0 : 최대화; 1 : 최소화
                int pageSize = GetDecryptParam("pageSize", "0").ToString().ToNumber<int>();//페이지사이즈
                string etcParam = GetDecryptParam("etcParam", "");

                Zeospace.DeskPlus.EIP.BE.TabPortletUserInfo info = new EIP.BE.TabPortletUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.TabPortletID = tabPortletID;
                info.TabID = tabID;
                info.PortletID = portletID;
                info.IsMinimize = isMinimize;
                info.PageSize = pageSize; //0 보다 크면 UPDATE YES ; 0 이면 INSERT 5; UPDATE NO
                info.EtcParam = etcParam;

                Zeospace.DeskPlus.EIP.DAL.TabPortletUser dal = new EIP.DAL.TabPortletUser();
                return dal.AddMod(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 시작탭 설정하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModBaseTab(string p)
        {
            try
            {
                SetDecryptString(p);
                
                int tabID = GetDecryptParam("tabID", "0").ToString().ToNumber<int>();
                if (tabID > 0)
                {
                    Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Account.DAL.UserEnvironment();
                    return dal.ModBaseTab(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tabID);
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 박미란
        /// 포틀릿별 개인들의 설정정보 저장하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModTabPortletUser(string p)
        {
            SetDecryptString(p);

            int TabPortletID = GetDecryptParam("tabPortletID", "").ToNumber<int>();
            int TabID = GetDecryptParam("tabID", "").ToNumber<int>();
            int PortletID = GetDecryptParam("portletID", "").ToNumber<int>();
            int PageSize = GetDecryptParam("pageSize", "").ToNumber<int>();
            string etcParam = GetDecryptParam("etcParam", "");

            try
            {
                Zeospace.DeskPlus.EIP.BE.TabPortletUserInfo info = new EIP.BE.TabPortletUserInfo();

                info.TabPortletID = TabPortletID;
                info.TabID = TabID;
                info.PortletID = PortletID;
                info.PageSize = PageSize;
                info.UserID = CurrentUserInfo.UserID;
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.IsMinimize = "0";
                info.EtcParam = etcParam;

                Zeospace.DeskPlus.EIP.DAL.TabPortletUser biz = new EIP.DAL.TabPortletUser();

                biz.AddMod(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModTabPortletUser : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 이미지 영역 감추기 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModImage(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            string imageYN = GetDecryptParam("imageYN", "");

            try
            {

                Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Account.DAL.UserEnvironment();
                dal.ModImage(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, imageYN);

                return true;
 
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModImage : " + ex);
                throw;
            }
        }

        #endregion

        #region 가져오기

        /// <summary>
        /// 탭별 포틀릿 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.TabPortletInfo> GetTabPortlet(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int tabID = GetDecryptParam("tabID", "0").ToNumber<int>();
            if (tabID == 0) return null;
            string roleUserList = string.Join(",", CurrentUserInfo.RoleIDs);

            try
            {
                List<BE.TabPortletInfo> alList = new List<TabPortletInfo>();

                DAL.TabPortlet dal = new DAL.TabPortlet();
                List<TabPortletInfo> al = dal.GetTabPortlet(CurrentUserInfo.CompanyID, tabID, CurrentUserInfo.UserID, roleUserList);
                DAL.MenuDataLink dalMenu = new DAL.MenuDataLink();
                BLL.MenuDataLink bllMenu = new BLL.MenuDataLink();
                List<BE.LDAPInfo> alLdap = dalMenu.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                foreach(BE.TabPortletInfo info in al)
                {
                    string paramValue = info.ParamValue.ToNull();
                    //id$Mail$OL^pass$UserPassword$OL^go$dec$OL
                    if (paramValue.Length > 0)
                    {
                        info.ParamValue = "";

                        foreach (string tmpStr in paramValue.Split('^'))
                        {
                            //id$Mail$OL
                            if (tmpStr.Split('$')[2] == "OL")
                            {
                                //Mail
                                string ldapValue = bllMenu.GetLinkValue(alLdap[0], tmpStr.Split('$')[1]);
                                info.ParamValue += tmpStr.Split('$')[0] + "$" + ldapValue + "$" + tmpStr.Split('$')[2] + "^";

                            }
                            else //OU
                            {
                                info.ParamValue += tmpStr.Split('$')[0] + "$" + tmpStr.Split('$')[1] + "$" + tmpStr.Split('$')[2] + "^";
                            }
                        }

                        info.ParamValue = info.ParamValue.TrimEnd('^');
                    }

                    alList.Add(info);
                }

                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTabPortlet : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 포틀릿 만들기 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string LoadPortlet(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            string uiURL = GetDecryptParam("uiURL", "");

            try
            {
                return RenderPartialToString(uiURL, p);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Main[LoadPortlet] : " + ex);
            }

            return "";
        }

        #endregion

        #region 일반함수

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-02-21
        /// 작성목적 : RSS Data 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRss(string p)
        {
            SetDecryptString(p);
            int firstListCount = GetDecryptParam("firstListCount", "0").ToNumber<int>();
            int lastListCount = GetDecryptParam("lastListCount", "4").ToNumber<int>();
            string rssURL = GetDecryptParam("dataURL", "");
            List<object> cacheOutput = null;
            double rssCacheTime = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "RSSCacheTime", "600").ToDouble<double>();
            List<object> listobj = new List<object>();
            int feedCount = 0;

            if (rssCacheTime > 0)
            {
                cacheOutput = (List<object>)Context.Cache[rssURL];

                if (cacheOutput == null) //Cache를 사용하면서 null일 경우 
                {
                    cacheOutput = GetRssItems(rssURL);
                    
                    Context.Cache.Insert(rssURL, cacheOutput, null, DateTime.Now.AddSeconds(rssCacheTime), Cache.NoSlidingExpiration);
                }

                feedCount = cacheOutput.Count - 1;
                if (feedCount < lastListCount) lastListCount = feedCount;

                for (int i = firstListCount; i <= lastListCount; i++)
                {
                    listobj.Add(cacheOutput[i]);
                }
            }
            else //실시간요청이면 Feed items에서 바로 리턴
            {
                try
                {
                    if (String.IsNullOrEmpty(rssURL)) return listobj;

                    RssFeed feed = RssReader.GetFeed(rssURL);

                    feedCount = feed.Items.Count - 1;

                    if (feedCount < lastListCount) lastListCount = feedCount;

                    for (int i = firstListCount; i <= lastListCount; i++)
                    {
                        listobj.Add(feed.Items[i]);
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetRss] " + rssURL + " : " + ex.Message);
                    throw;
                }
            }

            return listobj;
        }

        /// <summary>
        /// 전체 RSSItems를 가져온다
        /// </summary>
        /// <param name="rssURL"></param>
        /// <returns></returns>
        private List<object> GetRssItems(string rssURL)
        {
            try
            {
                if (String.IsNullOrEmpty(rssURL)) return null;

                RssFeed feed = RssReader.GetFeed(rssURL);
                List<object> listobj = new List<object>();
                for (int i = 0; i < feed.Items.Count; i++)
                {
                    listobj.Add(feed.Items[i]);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetRssItems] " + rssURL + " : " + ex.Message);
                throw;
            }
        }

        

        /// <summary>
        /// 작성자 : 심재운
        /// 작성일자 : 2012-02-21
        /// 작성목적 : RSS 2.0 Data 가져오기(NHN 기사들)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRss20(string p)
        {
            SetDecryptString(p);
            int firstListCount = GetDecryptParam("firstListCount", "0").ToNumber<int>();
            int lastListCount = GetDecryptParam("lastListCount", "4").ToNumber<int>();
            string rssURL = GetDecryptParam("dataURL", "");

            try
            {
                List<object> listobj = new List<object>();
                if (String.IsNullOrEmpty(rssURL)) return listobj;
                
                List<Rss20HelperInfo> al = Rss20Helper.GetRss20Reader(rssURL);

                int feedCount = al.Count-1;                

                if (feedCount < lastListCount) lastListCount = feedCount;

                for (int i = firstListCount; i <= lastListCount; i++)
                {
                    al[i].Pubdate = Convert.ToDateTime(al[i].Pubdate).ToShortDateString() + "," + Convert.ToDateTime(al[i].Pubdate).ToShortTimeString();
                    listobj.Add(al[i]);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetRss20] " + rssURL + " : " + ex.Message);
                throw;
            }
        }
        


        /// <summary>
        /// 작성자 : 심재운
        /// 작성일자 : 2012-02-21
        /// 작성목적 : SyndicationFeed 방식으로 rss 읽기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRss20Syndi(string p)
        {
            SetDecryptString(p);
            int listCount = GetDecryptParam("listCount", "5").ToNumber<int>();
            string rssURL = GetDecryptParam("dataURL", "");

            try
            {
                List<object> listobj = new List<object>();

                if (String.IsNullOrEmpty(rssURL)) return listobj;

                List<Rss20HelperInfo> al = Rss20Helper.GetRss20SyndiReader(rssURL);

                if (al.Count < listCount) listCount = al.Count;


                for (int i = 0; i < listCount; i++)
                {
                    listobj.Add(al[i]);
                }

                //foreach (RssItem rssItem in feed.Items)
                //{
                //    listobj.Add(rssItem);
                //}

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetRss20Syndi] " + rssURL + " : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-02-21
        /// 작성목적 : SP Data 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSP(string p)
        {
            SetDecryptString(p);
            int listCount = GetDecryptParam("listCount", "5").ToNumber<int>();
            string rssURL = GetDecryptParam("dataURL", "");

            try
            {
                List<object> listobj = new List<object>();

                if (String.IsNullOrEmpty(rssURL)) return listobj;

                RssFeed feed = RssReader.GetFeed(rssURL);


                for (int i = 0; i < listCount; i++)
                {
                    listobj.Add(feed.Items[i]);
                }

                //foreach (RssItem rssItem in feed.Items)
                //{
                //    listobj.Add(rssItem);
                //}

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetRss] " + rssURL + " : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : Load ascx
        /// </summary>
        /// <param name="_controlName"></param>
        /// <param name="_p"></param>
        /// <returns></returns>
        private string RenderPartialToString(string _controlName, string _p)
        {
            if (string.IsNullOrEmpty(_controlName)) return "";

            string result = null;
            StringWriter writer = new StringWriter();
            try
            {
                Page page = new Page();
                BaseControlPortlet c = (BaseControlPortlet)page.LoadControl(_controlName);
                c.P = _p;           //호출 파라미터 추가
                page.Controls.Add(c);

                HttpContext.Current.Server.Execute(page, writer, false);
                result = writer.ToString();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Main[RenderPartialToString] : " + ex);
            }
            finally
            {
                writer.Close();
            }

            return result;
        }

        #endregion
    }
}
