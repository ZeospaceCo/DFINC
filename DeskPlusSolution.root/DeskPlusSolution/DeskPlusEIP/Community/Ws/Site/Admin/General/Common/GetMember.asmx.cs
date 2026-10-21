using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Admin.General.Common
{
    /// <summary>
    /// GetMember의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetMember : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-30
        /// 작성목적 : 회원 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();

                int icnt = 0;

                DAL.User dal = new DAL.User();
                icnt = dal.GetPageCount(CurrentUserInfo.CompanyID, communityID);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-30
        /// 작성목적 : 회원 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();

                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.User dal = new DAL.User();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, communityID, setPageingStr[0], setPageingStr[1], order);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UserInfo info = new UserInfo();

                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Grade = dtr["Grade"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정
                            info.UserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 회원등급 다국어 설정
                            info.GubunName = this.GetLangName(
                                    dtr["GubunBase"].ToString().ToNull(), dtr["GubunKo"].ToString().ToNull(),
                                    dtr["GubunEn"].ToString().ToNull(), dtr["GubunJa"].ToString().ToNull(),
                                    dtr["GubunFr"].ToString().ToNull(), dtr["GubunZhcn"].ToString().ToNull(),
                                    dtr["GubunZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 부서 다국어 설정
                            info.Dept = GetLangName(dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(),
                                dtr["DeptEn"].ToString(),
                                dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(),
                                dtr["DeptZhcn"].ToString(),
                                dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();
                            #endregion

                            #region 직급 다국어 설정
                            info.Title = GetLangName(dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(),
                                dtr["TitleEn"].ToString(),
                                dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(),
                                dtr["TitleZhcn"].ToString(),
                                dtr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();
                            #endregion

                            info.MyInfo = info.UserID + "┐" + info.Dept + " " + info.Title + " " + info.UserName;

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

        #endregion

        #region MOD()

        
        #endregion
    }
}
