using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.AuthEmail
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

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public  List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                List<object> listobj = new List<object>();

                string companyID = GetDecryptParam("companyID", "0");
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

                DAL.PasswordHistory dal = new DAL.PasswordHistory();

                DataTableReader dtr = dal.GetPageList(companyID.ToNumber<int>(),setPageingStr[0], setPageingStr[1], searchField, searchValue, order, dateValue[0], dateValue[1]);
                
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            PasswordHistroyInfo info = new PasswordHistroyInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.SendUserID = dtr["SendUserID"].ToString().ToNumber<int>();                           

                            info.SendUserName = UtilHelper.GetLangName(
                                    dtr["SendUserNameBase"].ToString(), dtr["SendUserNameKo"].ToString(), dtr["SendUserNameEn"].ToString(),
                                    dtr["SendUserNameJa"].ToString(), dtr["SendUserNameFr"].ToString(), dtr["SendUserNameZhcn"].ToString(),
                                    dtr["SendUserNameZhtw"].ToString(), PreLang).ToReverseXSS();
                            if (info.SendUserName.ToString() == "") //보내는사람 UserID가 UserInfo 테이블에 삭제되었으면
                            {
                                info.SendUserName = dtr["SendUserName"].ToString(); //DB상에 있는 이름을 가져온다
                            }                           

                            info.SendUserEmail = dtr["SendUserEmail"].ToString();
                            info.SendTitleBase = dtr["SendTitleBase"].ToString();
                            info.ReceiveUserID = dtr["ReceiveUserID"].ToString().ToNumber<int>();


                            info.ReceiveUserName = UtilHelper.GetLangName(
                                    dtr["ReceiveUserNameBase"].ToString(), dtr["ReceiveUserNameKo"].ToString(), dtr["ReceiveUserNameEn"].ToString(),
                                    dtr["ReceiveUserNameJa"].ToString(), dtr["ReceiveUserNameFr"].ToString(), dtr["ReceiveUserNameZhcn"].ToString(),
                                    dtr["ReceiveUserNameZhtw"].ToString(), PreLang).ToReverseXSS();

                            if (info.ReceiveUserName.ToString() == "") //수신자 UserID가 UserInfo 테이블에 삭제되었으면
                            {
                                info.ReceiveUserName = dtr["ReceiveUserName"].ToString(); //DB상에 이름을 가져온다. 
                            }
                            
                            info.ReceiveUserEmail = dtr["ReceiveUserEmail"].ToString();
                            info.ReceiveTitleBase = dtr["ReceiveTitleBase"].ToString();
                            info.SendTime = Convert.ToDateTime(dtr["SendTime"].ToString()).ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 검색 카운트 가져오기
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
                string companyID = GetDecryptParam("companyID", "0");
                
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int icnt = 0;

                string[] dateValue = DateHelper.GetSearchTerm(searchTerm, searchTermValue, PreLangReal);

                DAL.PasswordHistory biz = new DAL.PasswordHistory();
                icnt = biz.GetPageCnt(companyID.ToNumber<int>() , dateValue[0], dateValue[1], searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetListCount : " + ex.Message);
                return 0;
            }
        }

    }

    public class PasswordHistroyInfo
    {
        public int CompanyID { get; set; }
        public int HistoryID { get; set; }
        public int SendUserID { get; set; }
        public string SendUserName { get; set; }
        public string SendUserNameBase { get; set; }
        public string SendUserNameKo { get; set; }
        public string SendUserNameEn { get; set; }
        public string SendUserNameJa { get; set; }
        public string SendUserNameFr { get; set; }
        public string SendUserNameZhcn { get; set; }
        public string SendUserNameZhtw { get; set; } 

        public string SendUserEmail { get; set; }
        public string SendTitleBase { get; set; }

        public int ReceiveUserID { get; set; }
        public string ReceiveUserName { get; set; }
        public string ReceiveUserNameBase { get; set; }
        public string ReceiveUserNameKo { get; set; }
        public string ReceiveUserNameEn { get; set; }
        public string ReceiveUserNameJa { get; set; }
        public string ReceiveUserNameFr { get; set; }
        public string ReceiveUserNameZhcn { get; set; }
        public string ReceiveUserNameZhtw { get; set; } 

        public string ReceiveUserEmail { get; set; }
        public string ReceiveTitleBase { get; set; }
        
        public string SendTime { get; set; }

    }
}
