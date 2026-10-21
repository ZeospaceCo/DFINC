using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;


namespace Zeospace.DeskPlus.Account.Web.Ws.MyInfo
{
    /// <summary>
    /// GetUser의 요약 설명입니다.
    /// </summary>
    public class GetUser : IHttpHandler
    {
        public class NameValue
        {
            public string name { get; set; }
            public int id { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string loginID = context.User.Identity.Name;
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth.UserInfo
                userInfo = Authenticator.GetInstance().AuthenticatorInfo(loginID);

            

            string prefix = context.Request.QueryString["q"];
            if (prefix.Length > 0) prefix = context.Server.UrlDecode(prefix);
            context.Response.ContentType = "application/json";
            var obj = new List<NameValue>();            

            DAL.User dal = new DAL.User();
            var totCont = dal.GetPageListCount(userInfo.CompanyID,
                    "0", "0", true, "ManyName", prefix, "", "");

            DataTableReader dtr = dal.GetPageList(userInfo.CompanyID,
                    "0", "0", true,
                    totCont.ToString(), "0", "ManyName", prefix, "", "", "");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var nameBase = UtilHelper.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), "ko");

                        obj.Add(new NameValue { id = dtr["UserID"].ToString().ToNumber<int>(),
                                                name = nameBase
                        });
                    }
                }
            }
            
            var filtered = obj.Where(p => p.id > 0).GroupBy(p => p.id).Select(grp => grp.First()).ToList();

            //var filtered = (from c in obj
            //                //where c.Name.IndexOf(prefix) == 0
            //                select c).Distinct().ToList();

            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            string data = jss.Serialize(filtered);
            context.Response.Write(data);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}