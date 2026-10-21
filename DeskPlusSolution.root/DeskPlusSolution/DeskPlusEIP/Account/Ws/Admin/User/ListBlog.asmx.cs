using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User
{
    /// <summary>
    /// ListBlog의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class ListBlog : Base.BaseWSAccount
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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 인사 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelUser(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                string userList = GetDecryptParam("userList", "");

                if (companyID > 0 && userList.Length > 0)
                {
                    Zeospace.DeskPlus.Account.DAL.UserMailDomain bizMail = new DAL.UserMailDomain();

                    DAL.User dal = new DAL.User();

                    string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");

                    if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                    {
                        #region 제타 메일삭제/ GW사용자삭제

                        Zeospace.DeskPlus.Common.BLL.MailZetta65 zmail65 = new DeskPlus.Common.BLL.MailZetta65();

                        foreach (string user in userList.Split(','))
                        {
                            #region 제타 메일삭제/ GW사용자삭제

                            var mailList = bizMail.GetUserMailDomain(companyID, user.ToNumber<int>(), false);

                            //제타 6.5 일 경우 삭제 api 호출
                            if (mailProduct.ToUpper() == "ZETTA65")
                            {
                                Zeospace.DeskPlus.Common.BLL.MailZettaFactory zettaBiz = new DeskPlus.Common.BLL.MailZettaFactory();
                                foreach (var mailInfo in mailList) zettaBiz.DelUser(mailInfo.MailID + "@" + mailInfo.MailDomain);
                            }

                            dal.Del(companyID, user.ToNumber<int>()); //사용자 삭제 후

                            #endregion 제타 메일삭제/ GW사용자삭제
                        }

                        #endregion 제타 메일삭제/ GW사용자삭제
                    }
                    else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                    {
                        #region DEEP 메일삭제 / GW사용자삭제

                        Zeospace.DeskPlus.Common.BLL.MailDeepFactory deepBiz = new DeskPlus.Common.BLL.MailDeepFactory();

                        foreach (string user in userList.Split(','))
                        {
                            #region DEEP 메일삭제/ GW사용자삭제

                            var mailList = bizMail.GetUserMailDomain(companyID, user.ToNumber<int>(), false);

                            foreach (var mailInfo in mailList) deepBiz.DelUser(companyID, mailInfo.MailDomain, mailInfo.MailID, mailInfo.MailPassword);

                            dal.Del(companyID, user.ToNumber<int>()); //사용자 삭제 후

                            #endregion DEEP 메일삭제/ GW사용자삭제
                        }

                        #endregion DEEP 메일삭제 / GW사용자삭제
                    }
                    else
                    {
                        foreach (string user in userList.Split(','))
                        {
                            dal.Del(companyID, user.ToNumber<int>()); //사용자 삭제 후
                        }
                    }

                    Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                    //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                    bizMenu.ClearCacheMenuUser(companyID);
                    //사용자별 메뉴 정보 - 캐쉬설정
                    bizMenu.SetCacheMenuListUser(companyID);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelUser : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 인사 활성화
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
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                string userList = GetDecryptParam("userList", "");
                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", ""); //메일 제품 체크

                if (companyID > 0 && userList.Length > 0)
                {
                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory bizMail = new DeskPlus.Common.BLL.MailDeepFactory();
                    Zeospace.DeskPlus.Common.BLL.MailZetta65 zmail65 = new DeskPlus.Common.BLL.MailZetta65();

                    DAL.User dal = new DAL.User();

                    foreach (string user in userList.Split(','))
                    {
                        dal.ModActive(companyID, user.ToNumber<int>());

                        DataTableReader dtr = dal.GetActiveUser(companyID, user.ToNumber<int>());

                        #region 딥 메일 modify shimpark

                        if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                        {
                            bool rtnval = false;

                            if (dtr != null)
                            {
                                if (dtr.HasRows)
                                {
                                    while (dtr.Read())
                                    {
                                        //true :계정부재 , false :계정존재
                                        if (!bizMail.IsNonExistUser(companyID, dtr["MailDomain"].ToString(), dtr["MailID"].ToString()))
                                        {
                                            bool isActive = (dtr["ActiveUser"].ToString().Equals("1")) ? true : false;

                                            rtnval = bizMail.ModUser(companyID,
                                                dtr["MailDomain"].ToString(),
                                                dtr["MailID"].ToString(),
                                                dtr["MailPassword"].ToString(), "",
                                                dtr["NameBase"].ToString(), "", "",
                                                dtr["MailDesign"].ToString(), isActive);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion 딥 메일 modify shimpark

                        #region 제타 메일 modify shimpark

                        if (mailProduct.ToUpper() == "ZETTA65") //제타메일6.5 사용자라면
                        {
                            if (dtr != null)
                            {
                                if (dtr.HasRows)
                                {
                                    while (dtr.Read())
                                    {
                                        if (dtr["ActiveUser"].ToString() == "1") //그룹웨어에서 활성화 했을 경우에는 제타사용자도 활성화
                                        {
                                            zmail65.ApprovalUser(dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString());
                                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("이사용자는 활성화 되었습니다.. 사용자= " + dtr["Loginid"].ToString(), AppSettingType.MAIL);
                                        }
                                        else //비 활성화 했을때는 비활성화 API 호출
                                        {
                                            zmail65.SuspendUser(dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString());
                                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("이사용자는 비활성화 되었습니다. 사용자= " + dtr["Loginid"].ToString(), AppSettingType.MAIL);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion 제타 메일 modify shimpark
                    }

                    Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                    //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                    bizMenu.ClearCacheMenuUser(companyID);
                    //사용자별 메뉴 정보 - 캐쉬설정
                    bizMenu.SetCacheMenuListUser(companyID);
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
        /// 용도 : 호봉 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModUserDeptOrder(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                string userDeptOrderList = GetDecryptParam("userDeptOrderList", "");
                userDeptOrderList = userDeptOrderList.TrimEnd(',');

                if (companyID > 0 && userDeptOrderList.Length > 0)
                {
                    DAL.User dal = new DAL.User();
                    //obj.attr('userID') + "_" + obj.attr('deptID') + "_" + obj.val() + ","
                    foreach (string userDept in userDeptOrderList.Split(','))
                    {
                        if (userDept.Length > 0)
                        {
                            dal.ModUserDeptOrder(companyID,
                                userDept.Split('_')[0].ToNumber<int>(),
                                userDept.Split('_')[1].ToNumber<int>(),
                                userDept.Split('_')[2].ToNumber<int>());
                        }
                    }

                    Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                    //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                    bizMenu.ClearCacheMenuUser(companyID);
                    //사용자별 메뉴 정보 - 캐쉬설정
                    bizMenu.SetCacheMenuListUser(companyID);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModUserDeptOrder : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 회사별 라이센스 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCompanyLicenseCount(string p)
        {
            try
            {
                SetDecryptString(p);

                int icnt = 0;

                var dal = new Zeospace.DeskPlus.Common.DAL.Company();
                var alList = dal.GetCompany(CurrentUserInfo.CompanyID, true);
                foreach (var info in alList)
                {
                    icnt = info.LicenseCnt;
                }

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCompanyLicenseCount : " + ex.Message);
                return 0;
            }
        }
    }
}