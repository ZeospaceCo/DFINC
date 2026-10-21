using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.MDM.BE;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.MDM.Web.Ws.Auth
{
    /// <summary>
    /// ListAdd의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListReject : Base.BaseWSMDM
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
        /// 작성일자 : 2013-01-16
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
                string activeMobie = GetDecryptParam("activeMobie", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string order = GetDecryptParam("order", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
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

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                DAL.List dal = new DAL.List();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, activeMobie,
                    setPageingStr[0], setPageingStr[1], searchField, searchValue, startDate, endDate, order, true);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.UUID = dtr["UUID"].ToString();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.ActiveMobile = dtr["ActiveMobile"].ToString();
                            info.MobileNumber = dtr["MobileNumber"].ToString();
                            info.DeviceName = dtr["DeviceName"].ToString();
                            info.MobileType = dtr["MobileType"].ToString();

                            info.DeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(),
                                                               dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(),
                                                               dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);

                            info.TitleName = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                                                                dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                                                                dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);

                            info.Name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                            DateTime dt;

                            if (DateTime.TryParse(dtr["RequestDate"].ToString(), out dt))
                                info.RequestDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                info.RequestDateStr = string.Empty;

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
        /// 작성일자 : 2013-01-16        
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
                string activeMobie = GetDecryptParam("activeMobie", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                int icnt = 0;

                DAL.List dal = new DAL.List();
                icnt = dal.GetPageListCount(CurrentUserInfo.CompanyID, activeMobie, searchField, searchValue, startDate, endDate, true);

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 승인요청 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try
            {
                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "0");                

                DAL.List dal = new DAL.List();
                dal.Del(CurrentUserInfo.CompanyID, listID);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-01-07
        /// 작성목적 : 승인 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ListApprove(string p)
        {
            try
            {
                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "0");

                DAL.List dal = new DAL.List();

                bool rtn = false;

                foreach (string list in listID.Split(','))
                {
                    rtn = dal.ModApprove(CurrentUserInfo.CompanyID, list.ToNumber<int>(), CurrentUserInfo.UserID, CurrentUserInfo.Name);
                }

                // 메신저 알람 처리 
                if (rtn)
                {
                    DataTableReader dtr1 = dal.GetUserList(CurrentUserInfo.CompanyID, listID);

                    if (dtr1 != null)
                    {
                        if (dtr1.HasRows)
                        {
                            while (dtr1.Read())
                            {
                                SendAlarm(dtr1["UserID"].ToString().ToNumber<int>(), dtr1["DeviceName"].ToString(), "1");
                            }
                        }
                    }
                }

                return rtn;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }


        #endregion

        #region 알람

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-05
        /// 작성목적 : 메신저 알람
        /// </summary>
        /// <param name="_receiverID"></param>
        /// <param name="_deviceName"></param>
        /// <param name="_activeMobile"></param>
        /// <returns></returns>
        public bool SendAlarm(int _receiverID, string _deviceName, string _activeMobile)
        {
            this.ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Ws.Auth.ListAdd).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Zeospace.DeskPlus.Common.BLL.Alarm();
            Zeospace.DeskPlus.Common.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlus.Common.BE.AlarmInfo();
            alarmInfo.CompanyID = CurrentUserInfo.CompanyID;
            alarmInfo.SenderID = CurrentUserInfo.UserID;
            alarmInfo.SenderName = CurrentUserInfo.Name;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = resMng.GetString("MDM");
            alarmInfo.Content = AlarmMessengerTemplate(_deviceName, _activeMobile, PreLang);

            alarmInfo.TargetUrl = "";
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "MDM";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-05
        /// 작성목적 : 알람 내용 템플릿
        /// </summary>
        /// <param name="_deviceName"></param>
        /// <param name="_activeMobile"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        private string AlarmMessengerTemplate(string _deviceName, string _activeMobile, string _preLang)
        {
            string contHtml = "";

            try
            {
                XmlDocument confirmXml = null;
                string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/MDM/App_Data/" + _preLang + "/Messenger.xml");
                confirmXml = new XmlDocument();
                confirmXml.Load(xmlPath);
                XmlNode tmpXmlNode = confirmXml.DocumentElement;

                contHtml = (tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML1") != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML1").InnerText) : "";
                contHtml = contHtml.Replace("$DEVICENAME$", _deviceName);          //기기종류

            }
            catch { }

            return contHtml;
        }

        #endregion 
    }
}
