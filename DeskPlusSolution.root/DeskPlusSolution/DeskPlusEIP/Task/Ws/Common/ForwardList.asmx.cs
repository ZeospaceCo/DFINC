using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;
namespace Zeospace.DeskPlus.Task.Web.Ws.Common
{
    /// <summary>
    /// ForwardList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ForwardList : Base.BaseWSTask
    {
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 전달현황 페이징
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
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 전달현황 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListViewer(string p)
        {

            string listID = string.Empty;
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;

            SetDecryptString(p);

            listID = GetDecryptParam("listID", "0");
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            ObjList lst = new ObjList();
            DAL.ListViewer dal = new DAL.ListViewer();
            DataTableReader dtr = dal.GetForwardList(CurrentUserInfo.CompanyID, listID.ToString().ToNumber<int>(), setPageingStr[0].ToString().ToNumber<int>(), setPageingStr[1].ToString().ToNumber<int>());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ListViewerInfo info = new ListViewerInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                        info.SendName = GetLangName(dtr["SendNameBase"].ToString().ToNull(), dtr["SendNameKo"].ToString().ToNull(), dtr["SendNameEn"].ToString().ToNull(),
                                                    dtr["SendNameJa"].ToString().ToNull(), dtr["SendNameFr"].ToString().ToNull(),
                                                    dtr["SendNameZhcn"].ToString().ToNull(), dtr["SendNameZhtw"].ToString().ToNull(), PreLang);

                        info.SendDeptName = GetLangName(dtr["SendDeptBase"].ToString().ToNull(), dtr["SendDeptKo"].ToString().ToNull(),dtr["SendDeptEn"].ToString().ToNull(),
                                                        dtr["SendDeptJa"].ToString().ToNull(),dtr["SendDeptFr"].ToString().ToNull(), dtr["SendDeptZhcn"].ToString().ToNull(),
                                                        dtr["SendDeptZhtw"].ToString().ToNull(), PreLang);

                        info.SendTitleName = GetLangName(dtr["SendTitleBase"].ToString().ToNull(), dtr["SendTitleKo"].ToString().ToNull(), dtr["SendTitleEn"].ToString().ToNull(),
                            dtr["SendTitleJa"].ToString().ToNull(), dtr["SendTitleFr"].ToString().ToNull(), dtr["SendTitleZhcn"].ToString().ToNull(),
                            dtr["SendTitleZhtw"].ToString().ToNull(), PreLang);


                        info.NameBase = GetLangName(dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(), dtr["NameFr"].ToString().ToNull(),
                                        dtr["NameZhcn"].ToString().ToNull(), dtr["NameZhtw"].ToString().ToNull(), PreLang);

                        info.DeptName = this.GetLangName(dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                            dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                            dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                            dtr["DeptZhtw"].ToString().ToNull(), PreLang);


                        info.TitleName = this.GetLangName(dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                               dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(), dtr["TitleFr"].ToString().ToNull(),
                               dtr["TitleZhcn"].ToString().ToNull(), dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                        info.ChechYN = dtr["Viewing"].ToString();
                       
                        DateTime dt;

                        if (DateTime.TryParse(dtr["SendWriteDate"].ToString(), out dt))
                            info.SendDateYMD = DateHelpers.GetCultureDate(dt, PreLang).Substring(0, 10);
                        else
                            info.SendDateYMD = string.Empty;

                        

                        DateTime dt2;

                        if (DateTime.TryParse(dtr["ViewingDate"].ToString(), out dt2))
                            info.ViewingDate = DateHelpers.GetCultureDate(dt2, PreLang).Substring(0, 10);
                        else
                            info.ViewingDate = string.Empty;



                        lst.Add(info);
                    }
                }
            }

            return lst;
        }
    }
}
