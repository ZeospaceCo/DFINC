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

namespace Zeospace.DeskPlus.Opinion.Web.Ws.Folder
{
    /// <summary>
    /// GetUnitList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetUnitList : Base.BaseWSOpinion
    {
        #region GET

        /// <summary>
        /// 특정 설문의 미사용 질문리스트가져오기(설문수정)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetUnitListByQuesID(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Folder.GetUnitList).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion
            SetDecryptString(p);

            // totCnt , pageSize , pageNo , sKey , sVal , order
            string totCnt = GetDecryptParam("totCnt", "0");
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();
            string sKey = GetDecryptParam("sKey", "");
            string sVal = GetDecryptParam("sVal", "");
            string order = GetDecryptParam("order", "");

            DAL.Unit ubiz = new DAL.Unit();
            DataTableReader dtr = ubiz.GetUnitListByQuesID(CurrentUserInfo.CompanyID, quesID, pageSize, pageNo, sKey, sVal, order);

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
                        uInfo.IsReqStr = (uInfo.IsReq.Equals("1")) ? resMng.GetString("TitReq") : resMng.GetString("TitNoReq");
                        uInfo.Point = dtr["Point"].ToString().ToNumber<int>();
                        uInfo.IsActive = dtr["IsActive"].ToString();
                        uInfo.IsActiveStr = (uInfo.IsActive.Equals("1")) ? resMng.GetString("Unit_Active") : resMng.GetString("Unit_ActiveNon"); 

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
        /// 특정 설문의 미사용 질문개수가져오기(설문수정)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUnitListByQuesIDCnt(string p)
        {
            SetDecryptString(p);

            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            string sKey = GetDecryptParam("sKey", "");
            string sVal = GetDecryptParam("sVal", "");

            DAL.Unit ubiz = new DAL.Unit();
            int listCnt = ubiz.GetUnitListByQuesID(CurrentUserInfo.CompanyID, quesID, sKey, sVal);
            return listCnt;
        }

        #endregion

        #region SET

        #endregion
    }
}
