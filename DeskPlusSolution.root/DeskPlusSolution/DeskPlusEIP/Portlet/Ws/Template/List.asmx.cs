using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Template
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPortlet
    {

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRss(string p)
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
    }
}
