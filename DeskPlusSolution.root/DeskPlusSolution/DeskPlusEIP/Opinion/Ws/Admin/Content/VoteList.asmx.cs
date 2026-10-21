using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using System.IO;

namespace Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content
{
    /// <summary>
    /// VoteList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class VoteList : Base.BaseWSOpinion
    {
        #region GET

        /// <summary>
        /// 항목별 투표현황리스트
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetVotePageList(string p)
        {
            SetDecryptString(p);

            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int unitID = GetDecryptParam("unitID", "0").ToNumber<int>();
            int unitItemID = GetDecryptParam("unitItemID", "0").ToNumber<int>();
            string unitItemCont = GetDecryptParam("unitItemCont", "");
            string type = GetDecryptParam("type", "");
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();

            DAL.Vote vDAL = new DAL.Vote();
            DataTableReader dtr;

            if (type == "")
            {
                dtr = vDAL.GetVotePageListAdmin(CurrentUserInfo.CompanyID, quesID, unitID, unitItemID, pageSize, pageNo);
            }
            else if (type == "D")
            {
                dtr = vDAL.GetVotePageListAdminD(CurrentUserInfo.CompanyID, quesID, unitID, unitItemID, unitItemCont, pageSize, pageNo);
            }
            else
            {
                dtr = vDAL.GetVotePageListAdminC(CurrentUserInfo.CompanyID, quesID, unitID, unitItemCont, pageSize, pageNo);
            }

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.VoteInfo vInfo = new BE.VoteInfo();
                        vInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        vInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        vInfo.NameBase = this.GetLangName(dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);
                        vInfo.DeptBase = this.GetLangName(dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                        dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                        dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                        dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                        vInfo.TitleBase = this.GetLangName(dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                        dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                        dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                        dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        vInfo.DutyBase = this.GetLangName(dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                        dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                        dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                        dtr["DutyZhtw"].ToString().ToNull(), PreLang);

                        vInfo.UnitItemCont = dtr["Cont"].ToString();

                        DateTime dt;
                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            vInfo.WriteDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                        else
                            vInfo.WriteDateStr = string.Empty;

                        uobj.Add(vInfo);
                    }
                }
            }

            return uobj;
        }

        /// <summary>
        /// 항목별 투표현황리스트(건수)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetVotePageCnt(string p)
        {
            SetDecryptString(p);

            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int unitID = GetDecryptParam("unitID", "0").ToNumber<int>();
            int unitItemID = GetDecryptParam("unitItemID", "0").ToNumber<int>();
            string unitItemCont = GetDecryptParam("unitItemCont", "");
            string type = GetDecryptParam("type", "");

            DAL.Vote vDAL = new DAL.Vote();
            int listCnt = 0;
            if (type == "")
            {
                listCnt = vDAL.GetVotePageListAdmin(CurrentUserInfo.CompanyID, quesID, unitID, unitItemID);
            }
            else if (type == "D")
            {
                listCnt = vDAL.GetVotePageListAdminD(CurrentUserInfo.CompanyID, quesID, unitID, unitItemID, unitItemCont);
            }
            else
            {
                listCnt = vDAL.GetVotePageListAdminC(CurrentUserInfo.CompanyID, quesID, unitID, unitItemCont);
            }
            return listCnt;
        }

        #endregion
    }
}
