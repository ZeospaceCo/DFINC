using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Admin.Holiday
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPim
    {
        public List()
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Admin.Holiday.List).Assembly;	//어셈블리 선언
            #endregion
            this.GetResourceManager();
        }

        #region Get

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetHolyDay(string p)
        {
            string companyID = string.Empty;
            string startDate = string.Empty;
            string sType = string.Empty;

            SetDecryptString(p);

            startDate = GetDecryptParam("StarDate", DateTime.Now.ToString("yyyy-MM-dd"));
            sType = GetDecryptParam("SearchKey", "G,M,K,N,A");

            List<object> listobj = new List<object>();

            DAL.Schedule biz = new DAL.Schedule();

            BE.ScheduleInfo info = new ScheduleInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.StartTime = DateTime.Parse(startDate);
            info.Type = ConvertComma(sType);
            

            DataTableReader dtr = biz.GetSheduleHolyDayTable(info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo data = new ScheduleInfo();
                        string username = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                               , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        data.UserName = username;
                        data.stringStartTime =DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString();//_be.StartTime.ToShortDateString();
                        data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();//_be.ScheduleID;
                        data.Type = dtr["Type"].ToString();// _be.Type;
                        data.Title = dtr["Title"].ToString();// _be.Title;
                        data.OpenType = dtr["OpenType"].ToString();// _be.OpenType;
                        data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();// _be.DisplayID;

                        switch (data.Type)
                        {
                            case "G": //공유일정
                                data.KindDesk = GetResource("holy3");//"공유일정";
                                break;
                            case "M": //공유 기념일
                                data.KindDesk = GetResource("holy4");// "공유기념일";
                                break;
                            case "K": //공유 절기
                                data.KindDesk = GetResource("holy5");// "공유절기";
                                break;
                            case "N": //전체 공유 국가 공휴일
                                data.KindDesk = GetResource("holy6");//"국가공휴일";
                                break;
                            case "A":   //전체 공유 국가 기념일
                                data.KindDesk = GetResource("holy7");//"국가기념일";
                                break;
                        }

                        if (sType.IndexOf(data.Type) > -1)
                        {
                            listobj.Add(data);
                        }

                    }
                }
            }            
           
            return listobj;
        }


        #endregion


        #region Set
        #endregion

        #region util


        private string ConvertLength(string sMon)
        {
            string[] arTemp = sMon.Split('-');

            if (arTemp[0].Length == 1)
            {
                arTemp[0] = "0" + arTemp[0];
            }
            if (arTemp[1].Length == 1)
            {
                arTemp[1] = "0" + arTemp[1];
            }

            return arTemp[0] + "-" + arTemp[1];
        }


        private string ConvertComma(string sValue)
        {
            string rtMsg = string.Empty;

            string[] arV = sValue.Split(',');


            for (int i = 0; i < arV.Length; i++)
            {
                rtMsg += "'" + arV[i] + "'";
                if (i != (arV.Length - 1))
                {
                    rtMsg += ",";
                }
            }


            return rtMsg;
        }

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="iMaxLength"></param>
        /// <param name="strTail"></param>
        /// <returns></returns>
        public string MaxTextCut(string strSource, int iMaxLength, string strTail)
        {
            string strRt = string.Empty;
            strRt = strSource.ToReverseXSS();
            if (strSource.ToReverseXSS().Length > iMaxLength)
            {
                strRt = strSource.ToReverseXSS().Substring(0, iMaxLength) + strTail;
            }

            return strRt.ToXSS();
        }

        #endregion
    }

}
