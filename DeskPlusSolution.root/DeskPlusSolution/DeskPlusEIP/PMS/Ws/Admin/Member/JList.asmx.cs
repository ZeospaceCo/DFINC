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

namespace Zeospace.DeskPlus.PMS.Web.Ws.Admin.Member
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
                int status = GetDecryptParam("status", "0").ToNumber<int>(); 

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                BE.ProjectUserInfo listInfo = new Zeospace.DeskPlus.PMS.BE.ProjectUserInfo();
                DAL.ProjectUser list = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = projectID;
                listInfo.Status = status;
                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();
                listInfo.SearchStartDate = "";
                listInfo.SearchEndDate = "";

                DataTableReader dtr = list.GetPageList(listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ProjectUserInfo info = new ProjectUserInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                            info.ProjectUserID = dtr["ProjectUserID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.NameBase = dtr["NameBase"].ToString();
                            info.NameKo = dtr["NameKo"].ToString();
                            info.NameEn = dtr["NameEn"].ToString();
                            info.NameJa = dtr["NameJa"].ToString();
                            info.NameFr = dtr["NameFr"].ToString();
                            info.NameZhcn = dtr["NameZhcn"].ToString();
                            info.NameZhtw = dtr["NameZhtw"].ToString();
                            info.ManyName = dtr["ManyName"].ToString();
                            info.Status = dtr["Status"].ToString().ToNumber<int>();
                            info.StatusStr = RecStatus(info.Status);
                            
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();                            
                            

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
                            
                            if (DateTime.TryParse(dtr["RegisterDate"].ToString(), out dt))
                                info.RegisterDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.RegisterDateStr = string.Empty;
                            
                            if (DateTime.TryParse(dtr["VisitedDate"].ToString(), out dt))
                                info.VisitedDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.VisitedDateStr = string.Empty;
                            

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
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Ws.Admin.Member.JList).Assembly;	//어셈블리 선언

            if (_status == 0) return "";
            string response = "";

            switch (_status)
            {
                case 1:
                    response = GetResource("Admin_Menu");
                    break;
                case 2:
                    response = GetResource("Participant");
                    break;
            }


            return response;
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
                int status = GetDecryptParam("status", "0").ToNumber<int>();         

                BE.ProjectUserInfo listInfo = new Zeospace.DeskPlus.PMS.BE.ProjectUserInfo();
                DAL.ProjectUser list = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = projectID;
                listInfo.Status = status;
                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";
                listInfo.SearchEndDate = "";

                int cnt = list.GetPageCount(listInfo);

                return cnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion        



        /// <summary>
        /// 리스트 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelList(string p)
        {
            try
            {
                SetDecryptString(p);

                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                string projectUserList = GetDecryptParam("projectUserList", "");

                if (projectID > 0 && projectUserList.Length > 0)
                {
                    BLL.ProjectUser bll = new Zeospace.DeskPlus.PMS.BLL.ProjectUser();
                    //1 : 관리자; 2: 참여자
                    //참여자만 삭제한다.
                    foreach (string list in projectUserList.Split(','))
                    {                        
                        BE.ProjectUserInfo be = new ProjectUserInfo();
                        be.CompanyID = CurrentUserInfo.CompanyID;
                        be.ProjectID = projectID;
                        be.ProjectUserID = list.Split('_')[0].ToNumber<int>();

                        bll.Del(be);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 리스트 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            try
            {
                SetDecryptString(p);

                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                string projectUserList = GetDecryptParam("projectUserList", "");
                projectUserList = projectUserList.TrimEnd(',');

                DAL.ProjectUser bll = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();

                BE.ProjectUserInfo be = new ProjectUserInfo();
                be.CompanyID = CurrentUserInfo.CompanyID;
                be.ProjectID = projectID;

                foreach (string _str in projectUserList.Split(','))
                {
                    be.ProjectUserID = 0;
                    be.UserID = _str.ToNumber<int>();
                    be.Status = 2; //참여자
                    be.RegisterDate = System.DateTime.Now;
                    be.VisitedDate = System.DateTime.Now;
                    be.WriteUserID = CurrentUserInfo.UserID;

                    bll.Add(be);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

    }
}
