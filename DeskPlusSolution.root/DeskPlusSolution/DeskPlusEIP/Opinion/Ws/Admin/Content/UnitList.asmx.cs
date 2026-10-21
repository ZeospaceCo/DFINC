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
    /// UnitList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class UnitList : Base.BaseWSOpinion
    {
        #region GET

        /// <summary>
        /// 질문리스트가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetUnitList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content.UnitList).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            // totCnt , pageSize , pageNo , sKey , sVal , order
            string totCnt = GetDecryptParam("totCnt", "0");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageNo = GetDecryptParam("pageNo", "0");
            string section = GetDecryptParam("section", "0");
            string sKey = GetDecryptParam("sKey", "");
            string sVal = GetDecryptParam("sVal", "");
            string sPeriod = GetDecryptParam("sPeriod", "");
            string sPeriodVal = GetDecryptParam("sPeriodVal", "");
            string order = GetDecryptParam("order", "");

            DAL.Unit ubiz = new DAL.Unit();
            DataTableReader dtr = ubiz.GetUnitList(CurrentUserInfo.CompanyID, section, pageSize, pageNo, sKey, sVal, sPeriod, sPeriodVal, order);

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.UnitInfo uInfo = new BE.UnitInfo();

                        uInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        uInfo.UnitID = dtr["UnitID"].ToString().ToNumber<int>();
                        uInfo.Section = dtr["Section"].ToString();
                        uInfo.SectionStr = (uInfo.Section.Equals("0")) ? resMng.GetString("Question") : resMng.GetString("Example");
                        uInfo.Title = dtr["Title"].ToString();
                        uInfo.Type = dtr["Type"].ToString();
                        switch (uInfo.Type.ToUpper())
                        {
                            case "A":
                                uInfo.TypeStr = resMng.GetString("Unit_SelectA");
                                break;
                            case "B":
                                uInfo.TypeStr = resMng.GetString("Unit_SelectB");
                                break;
                            case "C":
                                uInfo.TypeStr = resMng.GetString("Unit_PointA");
                                break;
                            case "D":
                                uInfo.TypeStr = resMng.GetString("Unit_PointB");
                                break;
                            case "E":
                                uInfo.TypeStr = resMng.GetString("Unit_TypeE");
                                break;
                            default:
                                uInfo.TypeStr = resMng.GetString("JTF16");
                                break;
                        }
                        uInfo.IsReq = dtr["IsReq"].ToString();
                        uInfo.IsReqStr = (uInfo.IsReq.Equals("1")) ? resMng.GetString("TitReq") : resMng.GetString("TitNoReq"); //필수, 비필수
                        uInfo.Point = dtr["Point"].ToString().ToNumber<int>();
                        uInfo.IsActive = dtr["IsActive"].ToString();
                        uInfo.IsActiveStr = (uInfo.IsActive.Equals("1")) ? resMng.GetString("Unit_Active") : resMng.GetString("Unit_ActiveNon");  //활성화, 비활성화

                        DateTime dt;
                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            uInfo.WriteDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                        else
                            uInfo.WriteDateStr = string.Empty;

                        uInfo.ItemCnt = dtr["ItemCnt"].ToString();
                        uInfo.QuesUnitCnt = dtr["QuesUnitCnt"].ToString();

                        uobj.Add(uInfo);
                    }
                }
            }

            return uobj;
        }

        /// <summary>
        /// 질문개수가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUnitListCnt(string p)
        {
            SetDecryptString(p);

            string section = GetDecryptParam("section", "0");
            string sKey = GetDecryptParam("sKey", "");
            string sVal = GetDecryptParam("sVal", "");
            string sPeriod = GetDecryptParam("sPeriod", "");
            string sPeriodVal = GetDecryptParam("sPeriodVal", "");

            DAL.Unit ubiz = new DAL.Unit();
            int listCnt = ubiz.GetUnitList(CurrentUserInfo.CompanyID, section, sKey, sVal, sPeriod, sPeriodVal);
            return listCnt;
        }

        #endregion

        #region SET

        /// <summary>
        /// 질문삭제(다중)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelUnitList(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            BLL.Unit uBll = new BLL.Unit();
            foreach (string id in arrID)
            {
                if (!id.Equals(""))
                {
                    uBll.Del(CurrentUserInfo.CompanyID, id.Split(':')[0].ToNumber<int>(), Convert.ToDateTime(id.Split(':')[1]));
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
        public bool UpdActiveUnit(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            BLL.Unit uBll = new BLL.Unit();
            foreach (string id in arrID)
            {
                if (id.Split('-').Length.Equals(2))
                {
                    uBll.UpdActive(CurrentUserInfo.CompanyID, id.Split('-')[0].ToNumber<int>(), id.Split('-')[1]);
                }
            }
            return true;
        }
        #endregion
    }
}
