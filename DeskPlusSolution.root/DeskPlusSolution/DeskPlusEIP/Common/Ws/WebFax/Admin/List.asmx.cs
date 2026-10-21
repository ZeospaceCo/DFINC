using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Common.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;

namespace Zeospace.DeskPlus.Common.Web.Ws.WebFax.Admin 
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSCommon
    {

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
        public List<BE.FaxManagerInfo> GetPageList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.Ws.WebFax.Admin.List).Assembly;	//어셈블리 선언


                SetDecryptString(p);
                
                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");//pagesize
                string currentPage = GetDecryptParam("currentPage", "0");//pageno
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");                
                string order = GetDecryptParam("order", "");
                string gubun = GetDecryptParam("gubun", "");
                string active = GetDecryptParam("active", "");

                bool isActive = (active == "1") ? true : false;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.FaxManager dal = new DAL.FaxManager();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID,
                    setPageingStr[0], setPageingStr[1],
                    searchKey, searchValue, isActive, gubun, order);

                List<BE.FaxManagerInfo> listobj = new List<FaxManagerInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.FaxManagerInfo info = new FaxManagerInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.FaxID = dtr["FaxID"].ToString().ToNumber<int>();
                            info.NameBase = dtr["NameBase"].ToString();
                            info.NameKo = dtr["NameKo"].ToString();
                            info.NameEn = dtr["NameEn"].ToString();
                            info.NameJa = dtr["NameJa"].ToString();
                            info.NameFr = dtr["NameFr"].ToString();
                            info.NameZhcn = dtr["NameZhcn"].ToString();
                            info.NameZhtw = dtr["NameZhtw"].ToString();

                            info.NameBase = this.GetLangName(
                                    info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

                            info.FaxNumber = dtr["FaxNumber"].ToString();
                            info.BranchID = dtr["BranchID"].ToString().ToNumber<int>();
                            info.Gubun = dtr["Gubun"].ToString();
                            info.IsActive = dtr["IsActive"].ToString().ToNumber<int>();
                            info.ActiveName = (info.IsActive == 1) ? GetResource("Alert_Yes") : GetResource("Alert_No");
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            info.BranchNameBase = dtr["BranchNameBase"].ToString();
                            info.BranchNameKo = dtr["BranchNameKo"].ToString();
                            info.BranchNameEn = dtr["BranchNameEn"].ToString();
                            info.BranchNameJa = dtr["BranchNameJa"].ToString();
                            info.BranchNameFr = dtr["BranchNameFr"].ToString();
                            info.BranchNameZhcn = dtr["BranchNameZhcn"].ToString();
                            info.BranchNameZhtw = dtr["BranchNameZhtw"].ToString();

                            info.BranchNameBase = this.GetLangName(
                                    info.BranchNameBase, info.BranchNameKo, info.BranchNameEn, info.BranchNameJa,
                                    info.BranchNameFr, info.BranchNameZhcn, info.BranchNameZhtw, PreLang);

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
        /// 작성자 : 심재운
        /// 총개수 가져오기
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
                
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");                
                string gubun = GetDecryptParam("gubun", "");
                string active = GetDecryptParam("active", "");

                bool isActive = (active == "1") ? true : false;

                DAL.FaxManager dal = new DAL.FaxManager();

                int cnt = dal.GetPageCount(CurrentUserInfo.CompanyID,                    
                    searchKey, searchValue, isActive, gubun);


                return cnt;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }



        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 활성화 여부
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModActive(string p)
        {
            try
            {
                SetDecryptString(p);
                string faxList = GetDecryptParam("faxList", "");

                if (faxList.Length > 0)
                {
                    DAL.FaxManager dal = new DAL.FaxManager();
                    foreach (string faxID in faxList.Split(','))
                    {
                        if (faxID.Length > 0) dal.ModActive(CurrentUserInfo.CompanyID, faxID.ToNumber<int>());
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModActive : " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 활성화 여부
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try
            {
                SetDecryptString(p);
                string faxList = GetDecryptParam("faxList", "");

                if (faxList.Length > 0)
                {
                    DAL.FaxManager dal = new DAL.FaxManager();
                    foreach (string faxID in faxList.Split(','))
                    {
                        if (faxID.Length > 0) dal.Del(CurrentUserInfo.CompanyID, faxID.ToNumber<int>());
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex.Message);
                return false;
            }
        }

    }
}
