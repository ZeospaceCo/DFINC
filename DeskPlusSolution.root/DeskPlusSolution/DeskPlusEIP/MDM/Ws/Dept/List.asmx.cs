using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.MDM.Web.Ws.Dept
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSMDM
    {
        #region GET()

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
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string deptID = GetDecryptParam("deptID", "0");                
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");               
                string order = GetDecryptParam("order", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.List dal = new DAL.List();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, deptID, setPageingStr[0], setPageingStr[1], searchField, searchValue, order, true);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            //info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();                            
                            //info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.LoginID = dtr["LoginID"].ToString();
                            //info.UUID = dtr["UUID"].ToString();
                            //info.MobileNumber = dtr["MobileNumber"].ToString();

                            info.Name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                            info.DeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(),
                                                               dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(),
                                                               dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);

                            info.TitleName = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                                                                dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                                                                dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);

                            info.DutyName = this.GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(),
                                                                dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(),
                                                                dtr["DutyZhcn"].ToString(), dtr["DutyZhtw"].ToString(), PreLang);
                            
                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                            info.ROW = listNum.ToString();

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
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17        
        /// 작성목적 : 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string deptID = GetDecryptParam("deptID", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int icnt = 0;

                DAL.List dal = new DAL.List();
                icnt = dal.GetPageListCount(CurrentUserInfo.CompanyID, deptID, searchField, searchValue, true);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 

        #region ADD()
        
        #endregion
        
        #region DEL()

        #endregion
        
        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-18
        /// 작성목적 : 접속불가 처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ListUpd(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "0");
                string accessRight = GetDecryptParam("accessRight", "0");

                DAL.List dal = new DAL.List();
                dal.ModUser(CurrentUserInfo.CompanyID, userID, accessRight, CurrentUserInfo.UserID, CurrentUserInfo.Name);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion 
    }
}
