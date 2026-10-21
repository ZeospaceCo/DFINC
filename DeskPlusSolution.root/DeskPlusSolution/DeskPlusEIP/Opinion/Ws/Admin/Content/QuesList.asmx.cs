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
    /// QuesList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class QuesList : Base.BaseWSOpinion
    {

        #region GET

        /// <summary>
        /// 설문리스트가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetQuesList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content.QuesList).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            string section = GetDecryptParam("section", "0");
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();
            string sKey = GetDecryptParam("sKey", "");
            string sVal = GetDecryptParam("sVal", "");
            string sPeriod = GetDecryptParam("sPeriod", "");
            string sPeriodVal = GetDecryptParam("sPeriodVal", "");
            string order = GetDecryptParam("order", "");

            DAL.Ques ubiz = new DAL.Ques();
            DataTableReader dtr = ubiz.GetQuesList(CurrentUserInfo.CompanyID, section, pageSize, pageNo, sKey, sVal, sPeriod, sPeriodVal, order);

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.QuesInfo qInfo = new BE.QuesInfo();
                        qInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        qInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        qInfo.Title = dtr["Title"].ToString();
                        qInfo.IsActive = dtr["IsActive"].ToString();
                        qInfo.IsActiveStr = (qInfo.IsActive.Equals("1")) ? resMng.GetString("Unit_Active") : resMng.GetString("Unit_ActiveNon");
                        qInfo.VoteCnt = dtr["VoteCnt"].ToString().ToNumber<int>();
                        qInfo.UnitCnt = dtr["UnitCnt"].ToString().ToNumber<int>();

                        DateTime sdt;
                        if (DateTime.TryParse(dtr["StartDate"].ToString(), out sdt))
                            qInfo.StartDateStr = DateHelpers.GetCultureDate(sdt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                        else
                            qInfo.StartDateStr = string.Empty;

                        DateTime edt;
                        if (DateTime.TryParse(dtr["EndDate"].ToString(), out edt))
                            qInfo.EndDateStr = DateHelpers.GetCultureDate(edt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                        else
                            qInfo.EndDateStr = string.Empty;

                        //info.WriteUserName = this.GetLangName(
                        //                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                        //                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                        //                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                        //                dtr["NameZhtw"].ToString().ToNull(), PreLang);

                        uobj.Add(qInfo);
                    }
                }
            }

            return uobj;
        }

        /// <summary>
        /// 설문개수가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetQuesListCnt(string p)
        {
            SetDecryptString(p);

            string section = GetDecryptParam("section", "0");
            string sKey = GetDecryptParam("sKey", "");
            string sVal = GetDecryptParam("sVal", "");
            string sPeriod = GetDecryptParam("sPeriod", "");
            string sPeriodVal = GetDecryptParam("sPeriodVal", "");


            DAL.Ques ubiz = new DAL.Ques();
            int listCnt = ubiz.GetQuesList(CurrentUserInfo.CompanyID, section, sKey, sVal, sPeriod, sPeriodVal);
            return listCnt;
        }


        #endregion

        #region SET

        /// <summary>
        /// 설문삭제(다중)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelQuesList(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            BLL.Ques qBll = new BLL.Ques();
            foreach (string id in arrID)
            {
                if (!id.Equals(""))
                {
                    qBll.Del(CurrentUserInfo.CompanyID, id.ToNumber<int>());
                }
            }
            return true;
        }

        /// <summary>
        /// 활성화/비활성화
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UpdActiveQues(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            BLL.Ques qBll = new BLL.Ques();
            foreach (string id in arrID)
            {
                if (id.Split('-').Length.Equals(2))
                {
                    qBll.UpdActive(CurrentUserInfo.CompanyID, id.Split('-')[0].ToNumber<int>(), id.Split('-')[1]);
                }
            }
            return true;
        }

        #endregion

    }
}
