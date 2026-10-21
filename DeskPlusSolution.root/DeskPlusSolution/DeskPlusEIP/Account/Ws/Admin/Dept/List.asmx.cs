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
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.Dept
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 사용자 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string deptID = GetDecryptParam("deptID", "0");
                string codeID = GetDecryptParam("codeID", "0");
                string activeUser = GetDecryptParam("activeUser", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("currentPage", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                if (order.Length == 0) order = "CompanyID, TitleCodeOrder, DutyCodeOrder, UserDeptOrder desc";

                //검색은 전체에서 검색한다.
                //deptID = (searchValue.Length > 0) ? "0" : deptID;               

                string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                string[] dateValue = DateHelper.GetSearchTerm(searchTerm, searchTermValue, PreLangReal);

                DAL.User dal = new DAL.User();

                DataTableReader dtr = dal.GetPageList(companyID.ToNumber<int>(),
                    deptID, codeID, (activeUser == "1") ? true : false,
                    setPageingStr[0], setPageingStr[1], searchField, searchValue, order, dateValue[0], dateValue[1]);

                BLL.User biz = new BLL.User();
                return biz.GetPageList(PreLang, dtr);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 사용자 카운트 가져오기
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
                string companyID = GetDecryptParam("companyID", "0");
                string deptID = GetDecryptParam("deptID", "0");
                string codeID = GetDecryptParam("codeID", "0");
                string activeUser = GetDecryptParam("activeUser", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int icnt = 0;

                string[] dateValue = DateHelper.GetSearchTerm(searchTerm, searchTermValue, PreLangReal);

                DAL.User biz = new DAL.User();
                icnt = biz.GetPageListCount(companyID.ToNumber<int>(),
                    deptID, codeID, (activeUser == "1") ? true : false, searchField, searchValue, dateValue[0], dateValue[1]);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetListCount : " + ex.Message);
                return 0;
            }
        }
    }
}
