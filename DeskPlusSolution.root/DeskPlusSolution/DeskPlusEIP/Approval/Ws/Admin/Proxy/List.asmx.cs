using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.Proxy
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {
        /// <summary>
        /// 작성자 : 이문호
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
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
        /// 작성자 : 이문호
        /// 대결자 페이징 총개수
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageListCnt(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");
                string deptID = GetDecryptParam("deptID", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyy-MM-dd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyy-MM-dd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyy-MM-dd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyy-MM-dd", PreLangReal);
                }

                if (startDate != "")
                {
                    startDate = startDate + " 00:00:00";
                }

                if (endDate != "")
                {
                    endDate = endDate + " 23:59:59";
                }

                #endregion

                DAL.Proxy list = new DAL.Proxy();
                return list.GetPageListCountAdmin(companyID.ToNumber<int>(), userID, deptID, startDate, endDate, searchField, searchValue);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageListCnt : " + ex);
                return -1;
            }

        }


        /// <summary>
        /// 작성자 : 이문호
        /// 대결자 페이징 리스트 가져오기
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
                string userID = GetDecryptParam("userID", "0");
                string deptID = GetDecryptParam("deptID", "0");                
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0"); //페이지에 보여지는 목록수
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0"); //페이지 넘버
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                HttpContext context = HttpContext.Current;
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyy-MM-dd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyy-MM-dd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyy-MM-dd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyy-MM-dd", PreLangReal);
                }

                if (startDate != "")
                {
                    startDate = startDate + " 00:00:00";
                }

                if (endDate != "")
                {
                    endDate = endDate + " 23:59:59";
                }

                #endregion

                DAL.Proxy biz = new DAL.Proxy();

                DataTableReader dtr = biz.GetPageListAdmin(companyID.ToNumber<int>(), userID, deptID, startDate, endDate, searchField, searchValue, setPageingStr[0], setPageingStr[1]);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.ProxyInfo info = new BE.ProxyInfo();

                            //info.ApprovalUserSeq = dtr["ROW"].ToString().ToNumber<int>();
                            info.ProxyID = dtr["ProxyID"].ToString().ToNumber<int>();
                            info.ProxyUserID = dtr["ProxyUserID"].ToString().ToNumber<int>();
                            info.ProxyDeptID = dtr["ProxyDeptID"].ToString().ToNumber<int>();

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            info.BaseDept = dtr["BaseDept0"].ToString().ToNumber<int>();

                            #region 직급 다국어 설정
                            info.TitleName = this.GetLangName(
                                    dtr["TitleBase0"].ToString().ToNull(), dtr["TitleKo0"].ToString().ToNull(),
                                    dtr["TitleEn0"].ToString().ToNull(), dtr["TitleJa0"].ToString().ToNull(),
                                    dtr["TitleFr0"].ToString().ToNull(), dtr["TitleZhcn0"].ToString().ToNull(),
                                    dtr["TitleZhtw0"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase0"].ToString().ToNull(), dtr["NameKo0"].ToString().ToNull(),
                                    dtr["NameEn0"].ToString().ToNull(), dtr["NameJa0"].ToString().ToNull(),
                                    dtr["NameFr0"].ToString().ToNull(), dtr["NameZhcn0"].ToString().ToNull(),
                                    dtr["NameZhtw0"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase0"].ToString().ToNull(), dtr["DeptKo0"].ToString().ToNull(),
                                    dtr["DeptEn0"].ToString().ToNull(), dtr["DeptJa0"].ToString().ToNull(),
                                    dtr["DeptFr0"].ToString().ToNull(), dtr["DeptZhcn0"].ToString().ToNull(),
                                    dtr["DeptZhtw0"].ToString().ToNull(), PreLang);
                            #endregion

                            info.ProxyBaseDept = dtr["BaseDept"].ToString().ToNumber<int>();
                            info.ProxyMail = dtr["Mail"].ToString();
                            info.ProxyMobilePhone = dtr["MobilePhone"].ToString();
                            info.ProxyOfficePhone = dtr["OfficePhone"].ToString();
                            info.ProxyState = dtr["IsProxy"].ToString();

                            #region 사진 이미지 설정
                            string ProxyPhotoName = dtr["PhotoImage"].ToString();
                            info.ProxyPhotoName = "";
                            string imgPath = "";
                            string imgTag = "";
                            string encParams = "";
                            Dictionary<string, string> paraDic = null;

                            if (!string.IsNullOrEmpty(ProxyPhotoName) && ProxyPhotoName.Length > 0)
                            {
                                imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + ProxyPhotoName;
                                imgPath = string.Format(imgPath, CurrentUserInfo.CompanyID, info.ProxyUserID);

                                if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                                {
                                    #region 스크린샷
                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("filePath", imgPath);
                                    encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                    imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                    #endregion

                                    imgTag = "<img src='{0}' width='52' height='52' />";
                                    imgTag = string.Format(imgTag, imgPath);
                                    info.ProxyPhotoName = imgTag;
                                }
                            }

                            if (info.ProxyPhotoName.Length == 0)
                            {
                                #region 스크린샷
                                paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", baseImageDirectory + "C/Img/Icon/ico_noimg.gif");
                                encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                #endregion

                                info.ProxyPhotoName = "<img src='" + imgPath + "' width='52' height='52' alt='' title='' />";
                            }

                            #endregion

                            #region 직급 다국어 설정
                            info.ProxyTitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.ProxyUserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.ProxyDeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.ProxyDateStr = DateTime.Parse(dtr["ProxyStartDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["ProxyStartDate"].ToString()).ToShortTimeString() + " ~ " +
                                    DateTime.Parse(dtr["ProxyEndDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["ProxyEndDate"].ToString()).ToShortTimeString();

                            info.AllDay = dtr["AllDay"].ToString();

                            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AbsenceType");
                            dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + dtr["ProxyType"].ToString() + "'";

                            foreach (DataRowView dr in dv)
                            {
                                info.ProxyType = this.GetLangName(
                                    dr["NameBase"].ToString(),
                                    dr["NameKo"].ToString(),
                                    dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                    dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                                    dr["NameZhTw"].ToString(), PreLang);
                            }

                            info.IsCall = dtr["IsCall"].ToString();
                            info.Gubun = dtr["Gubun"].ToString();

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
        /// 작성자 : 이문호
        /// 대결자 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelProxyUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);
            string proxyIDs = GetDecryptParam("proxyID", "0");

            try
            {
                if (proxyIDs.Length > 0)
                {
                    Account.DAL.User userdal = new Account.DAL.User();
                    Zeospace.DeskPlus.Approval.BLL.Proxy pBll = new BLL.Proxy();
                    Zeospace.DeskPlus.Approval.DAL.Proxy dal = new DAL.Proxy();

                    BE.UserInfo userinfo = new BE.UserInfo();

                    foreach (string proxyID in proxyIDs.Split(','))
                    {
                        string proxyUserID = "";
                        string proxyDeptID = "";
                        string proxyDateStr = "";
                        string proxyContent = "";
                        string proxyTypeStr = "";
                        string isCall = "";
                        DataTableReader dtr = dal.GetProxyInfoByProxyID(CurrentUserInfo.CompanyID, proxyID.ToNumber<int>());
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    List<Account.BE.UserInfo> list = userdal.GetUserInfo(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["UserID"].ToString().ToNumber<int>(), true, false);
                                    foreach (Account.BE.UserInfo uinfo in list)
                                    {
                                        if (dtr["DeptID"].ToString().ToNumber<int>() == uinfo.DeptID)
                                        {
                                            userinfo.CompanyID = uinfo.CompanyID;
                                            userinfo.LogInID = uinfo.LogInID;
                                            userinfo.UserID = uinfo.UserID;
                                            userinfo.DeptID = uinfo.DeptID;
                                            userinfo.MailID = uinfo.MailID;
                                            userinfo.MailDomain = uinfo.MailDomain;
                                            userinfo.MobilePhone = uinfo.MobilePhone;
                                            userinfo.PreferredLanguage = uinfo.PreferredLanguage;

                                            #region 부서명 다국어 처리
                                            userinfo.DeptName = UtilHelper.GetLangName(
                                                    uinfo.DeptBase.ToNull(), uinfo.DeptKo.ToNull(),
                                                    uinfo.DeptEn.ToNull(), uinfo.DeptJa.ToNull(),
                                                    uinfo.DeptFr.ToNull(), uinfo.DeptZhCn.ToNull(),
                                                    uinfo.DeptZhTw.ToNull(), uinfo.PreferredLanguage);
                                            #endregion

                                            #region 직급명 다국어 처리
                                            userinfo.TitleName = UtilHelper.GetLangName(
                                                            uinfo.TitleBase.ToNull(), uinfo.TitleKo.ToNull(),
                                                            uinfo.TitleEn.ToNull(), uinfo.TitleJa.ToNull(),
                                                            uinfo.TitleFr.ToNull(), uinfo.TitleZhCn.ToNull(),
                                                            uinfo.TitleZhTw.ToNull(), uinfo.PreferredLanguage);
                                            #endregion

                                            #region 사용자명 다국어 처리
                                            userinfo.Name = UtilHelper.GetLangName(
                                                            uinfo.NameBase.ToNull(), uinfo.NameKo.ToNull(),
                                                            uinfo.NameEn.ToNull(), uinfo.NameJa.ToNull(),
                                                            uinfo.NameFr.ToNull(), uinfo.NameZhCn.ToNull(),
                                                            uinfo.NameZhTw.ToNull(), uinfo.PreferredLanguage);
                                            #endregion
                                        }
                                    }

                                    proxyUserID = dtr["proxyUserID"].ToString();
                                    proxyDeptID = dtr["proxyDeptID"].ToString();

                                    proxyContent = dtr["ProxyContent"].ToString();

                                    string proxyPreferredLanguage = "";
                                    List<Account.BE.UserInfo> ulist = userdal.GetUserInfo(CurrentUserInfo.CompanyID, proxyUserID.ToNumber<int>(), true, true);
                                    foreach (Account.BE.UserInfo pinfo in ulist)
                                    {
                                        proxyPreferredLanguage = pinfo.PreferredLanguage;
                                    }

                                    if (proxyPreferredLanguage == "")
                                    {
                                        proxyPreferredLanguage = "ko";
                                    }

                                    DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AbsenceType");
                                    dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + dtr["ProxyType"].ToString() +"'";

                                    foreach (DataRowView dr in dv)
                                    {
                                        proxyTypeStr = this.GetLangName(
                                            dr["NameBase"].ToString(),
                                            dr["NameKo"].ToString(),
                                            dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                            dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                                            dr["NameZhTw"].ToString(), proxyPreferredLanguage);
                                    }

                                    proxyDateStr = DateHelper.GetCultureDate(DateTime.Parse(dtr["ProxyStartDate"].ToString()), proxyPreferredLanguage, true) + " ~ " + DateHelper.GetCultureDate(DateTime.Parse(dtr["ProxyEndDate"].ToString()), proxyPreferredLanguage, true);
                                    
                                    isCall = dtr["IsCall"].ToString();
                                }
                            }
                        }

                        BE.ProxyInfo info = new BE.ProxyInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ProxyID = proxyID.ToNumber<int>();
                        info.ProxyUserID = proxyUserID.ToNumber<int>();
                        info.ProxyDeptID = proxyDeptID.ToNumber<int>();
                        info.ProxyContent = proxyContent;
                        info.ProxyTypeStr = proxyTypeStr;
                        info.ProxyDateStr = proxyDateStr;
                        info.IsCall = isCall;

                        pBll.Del(userinfo, info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelProxyUser : " + ex);
                throw;
            }
        }
    }
}
