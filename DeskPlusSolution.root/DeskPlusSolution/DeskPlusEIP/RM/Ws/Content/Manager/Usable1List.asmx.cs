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
using System.Collections;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Ws.Content.Manager
{
    /// <summary>
    /// Usable1List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Usable1List : Base.BaseWSRM
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
        /// 작성일자 : 2015-12-17
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Content.Manager.Usable1List).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string usable = GetDecryptParam("usable", "0");
                string order = GetDecryptParam("order", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string adminYN = GetDecryptParam("adminYN", "N");
                adminYN = "N"; //자원의 주 담당자만 보이게 수정

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "week", true) == 0 || string.Compare(searchTerm, "month", true) == 0)
                {
                    if (string.Compare(searchTerm, "week", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (string.Compare(searchTerm, "month", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "period", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion 날짜

                DAL.Reverse dal = new DAL.Reverse();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, adminYN, usable, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.RM.BE.ReserveInfo info = new BE.ReserveInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                            info.ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.Subject = dtr["Subject"].ToString();
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();

                            info.ReserveDate = Convert.ToDateTime(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            info.ReserveDate += " ~ " + Convert.ToDateTime(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            //예약자 이름
                            info.WriteUserName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                            DateTime dt;

                            if (DateTime.TryParse(dtr["ModifyDate"].ToString(), out dt))
                                info.ModifyDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.ModifyDateStr = string.Empty;


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
        /// 작성일자 : 2015-12-17
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
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string usable = GetDecryptParam("usable", "0");
                string adminYN = GetDecryptParam("adminYN", "N");
                adminYN = "N"; //자원의 주 담당자만 보이게 수정

                int icnt = 0;

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "week", true) == 0 || string.Compare(searchTerm, "month", true) == 0)
                {
                    if (string.Compare(searchTerm, "week", true) == 0)
                        getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                    else if (string.Compare(searchTerm, "month", true) == 0)
                        getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "period", true) == 0)
                {
                    startDate = DateHelper.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelper.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                DAL.Reverse dal = new DAL.Reverse();
                icnt = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, adminYN, usable, startDate, endDate, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-22
        /// 작성목적 : 첨부파일 정보가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            SetDecryptString(p);

            int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

            string response = "0";

            DAL.Attach dal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttach(CurrentUserInfo.CompanyID, 0, reservationID, "0");

            string totalFileInfo = "";
            string encFilePath = "";

            foreach (var _attachInfo in attachInfo)
            {
                encFilePath = (_attachInfo.FileGUID != "") ? _attachInfo.FilePath + _attachInfo.FileGUID : _attachInfo.FilePath + _attachInfo.FileName;

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", encFilePath);
                paraDic.Add("fileName", _attachInfo.FileName);
                encFilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                totalFileInfo += _attachInfo.FileName + ":" + encFilePath + "|"; // Key < 파일경로파일명
            }

            totalFileInfo = totalFileInfo.TrimEnd('|');

            if (totalFileInfo.Length > 0)
            {
                response = totalFileInfo.Replace("&", "&amp;");		// 첨부파일 정보 가져오기
            }

            return response;
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-17
        /// 작성목적 : 폴더별 사용자 정보 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCategoryUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string categoryID = GetDecryptParam("categoryID", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "B");

                BE.CategoryUserInfo info = new BE.CategoryUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = categoryID.ToNumber<int>();
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.Types = types;

                DAL.CategoryUser dal = new DAL.CategoryUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-17
        /// 작성목적 : 승인 / 거부 처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModUsable(string p)
        {
            SetDecryptString(p);
            string usable = GetDecryptParam("usable", "1");
            string tmpValue = GetDecryptParam("tmpValue", "");

            tmpValue = tmpValue.TrimEnd(',');
            string[] ArrValue = tmpValue.Split(',');

            string mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            RM.BLL.Reserve biz = new BLL.Reserve();
            DAL.Reverse dal = new DAL.Reverse();

            for (int i = 0; i < ArrValue.Length; i++)
            {
                if (!dal.Mod(CurrentUserInfo.CompanyID, ArrValue[i].ToNumber<int>(), usable))
                {
                    return false;
                }
                //알람발송
                SendAlarm(ArrValue[i].ToNumber<int>(), usable);
            }

            return true;
        }

        #endregion

        /// <summary>
        /// 승인or거부 알람발송
        /// </summary>
        /// <param name="_reservationID"></param>
        /// <param name="_usable"></param>
        private void SendAlarm(int _reservationID, string _usable)
        {
            DAL.Reverse dal = new DAL.Reverse();
            BLL.Reserve bll = new BLL.Reserve();
            string Section = (_usable == "1") ? "APPROVE" : "REJECT";


            //예약정보 가져오기
            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, _reservationID);
            BE.ReserveInfo info;
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info = new BE.ReserveInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                        info.ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        info.WriteUserName = dtr["WriteUserName"].ToString();
                        info.ModifyUserID = CurrentUserInfo.UserID;
                        info.ModifyUserName = CurrentUserInfo.Name;
                        info.Usable = _usable;
                        info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                        info.Subject = dtr["Subject"].ToString().ToReverseXSS();
                        info.PreLang = CurrentUserInfo.PreferredLanguage;
                        info.Mail = CurrentUserInfo.Name + "<" + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain + ">";
                        info.Title = dtr["Title"].ToString();

                        bll.SendAlarm(info, info.ParentReservationID, info.StartDate, Section);
                    }
                }
            }
        }
    }
}
