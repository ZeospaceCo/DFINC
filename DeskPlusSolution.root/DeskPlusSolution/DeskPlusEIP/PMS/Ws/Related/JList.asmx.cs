using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.PMS.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;


namespace Zeospace.DeskPlus.PMS.Web.Ws.Related
{
    /// <summary>
    /// JList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class JList : Base.BaseWSPMS
    {

        #region 리스트

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
        /// 작성자 : 심재운
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string currentPage = GetDecryptParam("currentPage", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                string order = GetDecryptParam("order", "");
                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();                

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.PMS.DAL.RelationLink list = new Zeospace.DeskPlus.PMS.DAL.RelationLink();
                BE.PagingInfo listInfo = new Zeospace.DeskPlus.PMS.BE.PagingInfo();

                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();
                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;

                DataTableReader dtr = list.GetPageList(CurrentUserInfo.CompanyID, projectID, listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.RelationLinkInfo info = new RelationLinkInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                            info.ParentProjectID = dtr["ParentProjectID"].ToString().ToNumber<int>();
                            //진행률
                            info.Process = dtr["Process"].ToString().ToNumber<int>();
                            info.RelationLinkID = dtr["RelationLinkID"].ToString().ToNumber<int>();
                            //상태값
                            info.StatusStr = this.RecStatus(dtr["Status"].ToString().ToNumber<int>());
                            //작성자 아이디
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.IsApproval = dtr["RelIsApproval"].ToString().ToNumber<int>();

                            //연관프로젝트
                            info.SubjectBase = this.GetLangName(
                                    dtr["SubjectBase"].ToString().ToNull(), dtr["SubjectKo"].ToString().ToNull(),
                                    dtr["SubjectEn"].ToString().ToNull(), dtr["SubjectJa"].ToString().ToNull(),
                                    dtr["SubjectFr"].ToString().ToNull(), dtr["SubjectZhcn"].ToString().ToNull(),
                                    dtr["SubjectZhtw"].ToString().ToNull(), PreLang);

                            #region 개설자 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptBase = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyBase = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            DateTime dt;

                            //계획시작일
                            if (DateTime.TryParse(dtr["PlanStartDate"].ToString(), out dt))
                                info.PlanStartDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.PlanStartDateStr = string.Empty;

                            //계획완료일
                            if (DateTime.TryParse(dtr["PlanEndDate"].ToString(), out dt))
                                info.PlanEndDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.PlanEndDateStr = string.Empty;

                            // 승인
                            info.Apprval = RecApprval(info.IsApproval.ToString());

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
        /// 상태값 추출하기
        /// </summary>
        /// <param name="_status"></param>
        /// <returns></returns>
        private string RecStatus(int _status)
        {
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Ws.Related.JList).Assembly;	//어셈블리 선언

            if (_status == 0) return "";
            string response = "";

            switch (_status)
            {
                case 1:
                    response = GetResource("List_Status1");
                    break;
                case 2:
                    response = GetResource("List_Status2");
                    break;
                case 3:
                    response = GetResource("List_Status3");
                    break;
                case 4:
                    response = GetResource("List_Status4");
                    break;
                case 5:
                    response = GetResource("List_Status5");
                    break;
            }


            return response;
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 승인여부
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-02-04
        /// </summary>
        /// <param name="_IsApproval">The _ is approval.</param>
        /// <returns></returns>
        public string RecApprval(string _IsApproval)
        {
            if (_IsApproval == "1")
            {
                return GetResource("Project_ProjectInfo_Approval");
            }
            else if (_IsApproval == "2")
            {
                return GetResource("ListLinked_Reject");
            }
            else
            {
                return GetResource("List_Status1");
            }
        }


        /// <summary>
        /// 사용자 정보 카운트 가져오기
        /// 작성자 : 심재운
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

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();

                BE.PagingInfo listInfo = new Zeospace.DeskPlus.PMS.BE.PagingInfo();
                DAL.RelationLink list = new Zeospace.DeskPlus.PMS.DAL.RelationLink();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;

                int cnt = list.GetPageCount(CurrentUserInfo.CompanyID, projectID, listInfo);

                return cnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion        
    }
}
