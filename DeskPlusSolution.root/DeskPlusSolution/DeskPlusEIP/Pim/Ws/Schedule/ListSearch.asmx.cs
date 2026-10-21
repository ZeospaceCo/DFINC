using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListSearch : Base.BaseWSPim
    {
        public ListSearch()
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Schedule.ListSearch).Assembly;	//어셈블리 선언

            #endregion

            this.GetResourceManager();
        }

        #region Get
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTotalList(string p)
        {
            List<object> listobj = new List<object>();

            int _UserID = 0;
            int _CompanyID = 0;
            int _pagesize = 0;
            int _pagesizepageno = 0;
            string _UserList = string.Empty;
            string _btitle = string.Empty;
            string _CurrentUser = string.Empty;

            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;
            SetDecryptString(p);

            _UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _CurrentUser = GetDecryptParam("SelectUser", "P");

            switch (GetDecryptParam("StartDate", "").ToString())
            {
                case "0":
                    _StartDate =DateTime.Now.AddMonths(-6);// _StartDate = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01"));
                    
                    _EndDate = DateTime.Now.AddDays(1);
                    break;
                case "W":
                    _StartDate = DateTime.Now.AddDays(-7);
                    _EndDate = DateTime.Now;
                    break;
                case "M":
                    _StartDate = DateTime.Now.AddMonths(-1);
                    _EndDate = DateTime.Now;
                    break;
                default:
                    _StartDate = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _EndDate = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                    break;
            }
            _StartDate = DateTime.Parse(_StartDate.ToString("yyyy-MM-dd"));
            _EndDate = DateTime.Parse(_EndDate.ToString("yyyy-MM-dd"));

            _pagesize = GetDecryptParam("PageSize", "0").ToNumber<int>();
            _pagesizepageno = GetDecryptParam("PageSizePageNo", "0").ToNumber<int>();

            int iPage = 0;
            int iRecordPage = 0;

            if (_pagesizepageno == 1)
            {
                iPage = 0;
                iRecordPage = _pagesize;
            }
            else
            {
                iRecordPage = (_pagesizepageno) * _pagesize;
                iPage = ((_pagesizepageno - 1) * _pagesize) + 1;

            }

            _btitle = GetDecryptParam("title", "").ToString().Trim().ToXSS();


            DAL.Schedule biz = new DAL.Schedule();
            DataTable dt = new DataTable();
            DataTableReader dtr = new DataTableReader(dt);
            if (_CurrentUser == "P")
            {
                if (_UserList.Length == 0)
                    dtr = biz.GetTotalSearchList(iRecordPage, iPage, _UserID, _CompanyID, _StartDate, _EndDate, _btitle,CurrentUserInfo.DeptID);
                else
                    dtr = biz.GetTotalSearchListUserList(iRecordPage, iPage, _UserID, _CompanyID, _StartDate, _EndDate, _UserList);
            }
            else
            {
                dtr = biz.GetTotalSearchListCurrent(iRecordPage, iPage, CurrentUserInfo.UserID, _CompanyID, _StartDate, _EndDate, _btitle, _CurrentUser.ToNumber<int>(), CurrentUserInfo.DeptID);
            }

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo data = new ScheduleInfo();

                        data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        data.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();

                    
                        string _name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                           dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                           dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                           );
                        string _title = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                            dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang
                            );
                        

                        data.UserName = _name + "(" + _title + ")";
                        data.Title = dtr["Title"].ToString();

                        
                        data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        //종류
                        data.OpenType = dtr["OpenType"].ToString();

                        switch (data.OpenType)
                        {
                            case "-1"://나의일정
                                data.KindDesk = GetResource("MYSCHEDULE");// "나의일정";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToLongDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToLongDateString();
                                break;
                            case "0"://개인일정
                                data.KindDesk = GetResource("pim1");//"개인일정";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToLongDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToLongDateString();
                                break;
                            case "3"://부서일정
                                data.KindDesk = GetResource("pim2");//"부서일정";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToLongDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToLongDateString();
                                break;
                            case "1"://공유일정
                                data.KindDesk = GetResource("pim3");//"공유일정";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToLongDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToLongDateString();
                                break;
                            case "2"://전체일정
                                data.KindDesk = GetResource("pim4");//"전체일정";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToLongDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToLongDateString();
                                break;
                            case "8"://전체일정
                                data.KindDesk = GetResource("TODO");//"할일";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString();
                                break;
                        }


                        listobj.Add(data);
                    }
                }
            }


            return listobj;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetOpenUserComboList(string p)
        {
            List<object> listobj = new List<object>();
            try
            {
                SetDecryptString(p);

                DAL.OpenUser biz = new OpenUser();
                DataTableReader dtr = biz.GetOpenUserCombo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);


                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.OpenUserInfo _be = new OpenUserInfo();
                            _be.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            _be.Name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                           dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                           dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                           );

                            listobj.Add(_be);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTodoList", ex);
                return listobj;
            }
            return listobj;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetTotalListCnt(string p)
        {
            int iRt = 0;
            int _UserID = 0;
            int _CompanyID = 0;
            string _title = string.Empty;
            string _CurrentUser = string.Empty;

            string _UserList = string.Empty;

            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;
            SetDecryptString(p);

            _UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();

            _CurrentUser = GetDecryptParam("SelectUser", "P");

            switch (GetDecryptParam("StartDate", "").ToString())
            {
                case "0":
                    _StartDate =DateTime.Now.AddMonths(-6);// _StartDate = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01"));                    
                    _EndDate = DateTime.Now.AddDays(1);
                    break;
                case "W":
                    _StartDate = DateTime.Now.AddDays(-7);
                    _EndDate = DateTime.Now;
                    break;
                case "M":
                    _StartDate = DateTime.Now.AddMonths(-1);
                    _EndDate = DateTime.Now;
                    break;
                default:
                    _StartDate = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _EndDate = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                    break;
            }

            _StartDate = DateTime.Parse(_StartDate.ToString("yyyy-MM-dd"));
            _EndDate = DateTime.Parse(_EndDate.ToString("yyyy-MM-dd"));

            _title = GetDecryptParam("title", "").ToString();

            DAL.Schedule biz = new DAL.Schedule();
            DataTable dt = new DataTable();
            DataTableReader dtr = new DataTableReader(dt);

            if (_CurrentUser == "P")
            {
                if (_UserList.Length == 0)
                    iRt = biz.GetTotalSearchListCnt(_UserID, _CompanyID, _StartDate, _EndDate, _title,CurrentUserInfo.DeptID);
                else
                    iRt = biz.GetTotalSearchListUserListCnt(_UserID, _CompanyID, _StartDate, _EndDate, _UserList);
            }
            else
            {
                iRt = biz.GetTotalSearchListCntCurrent(CurrentUserInfo.UserID, _CompanyID, _StartDate, _EndDate, _title, _CurrentUser.ToNumber<int>(), CurrentUserInfo.DeptID);
            }

            return iRt;
        }



        #region 전체 검색

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetALLList(string p)
        {
            List<object> listobj = new List<object>();

            
            int _pagesize = 0;
            int _pagesizepageno = 0;

            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;
            SetDecryptString(p);

            BE.ScheduleInfo _info = new ScheduleInfo();


            _info.CompanyID = CurrentUserInfo.CompanyID;
            _info.UserID = CurrentUserInfo.UserID;

            switch (GetDecryptParam("StartDate", "").ToString())
            {
                case "0":  //전체
                    _info.StartTime = DateTime.Parse("2000-01-01");
                    _info.EndTime = DateTime.Now.AddDays(1);
                    break;
                case "W":
                    _info.StartTime = DateTime.Now.AddDays(-7);
                    _info.EndTime = DateTime.Now;
                    break;
                case "M":
                    _info.StartTime = DateTime.Now.AddMonths(-1);
                    _info.EndTime = DateTime.Now;
                    break;
                default:
                    _info.StartTime = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _info.EndTime = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                    break;
            }

            _pagesize = GetDecryptParam("PageSize", "0").ToNumber<int>();
            _pagesizepageno = GetDecryptParam("PageSizePageNo", "0").ToNumber<int>();

            int iPage = 0;
            int iRecordPage = 0;

            if (_pagesizepageno == 1)
            {
                iPage = 0;
                iRecordPage = _pagesize;
            }
            else
            {
                iRecordPage = (_pagesizepageno) * _pagesize;
                iPage = ((_pagesizepageno - 1) * _pagesize) + 1;
            }

            _info.Pagesize = iPage;
            _info.PageSizePageNo = iRecordPage;


            string SearchKind = GetDecryptParam("SearchKind", "").ToString().Trim();

            if (SearchKind == "Title")
            {
                //제목
                _info.Title = GetDecryptParam("SearchValue", "").ToString().Trim();
            }
            else if (SearchKind == "Name")
            {
                //이름
                _info.UserName = GetDecryptParam("SearchValue", "").ToString().Trim();
            }

            _info.DeptID = CurrentUserInfo.DeptID;

            DAL.Schedule dal = new DAL.Schedule();

            DataTableReader dtr = dal.GetAllSearch(_info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo data = new ScheduleInfo();
                        data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        data.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();

                        string _name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                         dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                         dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                         );
                        string _title = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                            dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang
                            );

                        data.UserName = _name + "(" + _title + ")";
                        data.Title = dtr["Title"].ToString();

                        //DateTime.Parse(dtr["StartDate"].ToString()).ToDateTimeLongString()



                        data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString();
                        data.stringENDTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortTimeString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortTimeString();

                        data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        //종류
                        data.OpenType = dtr["kind"].ToString();

                        switch (data.OpenType)
                        {
                            case "-1"://나의일정
                                
                                data.KindDesk = GetResource("MYSCHEDULE");// "나의일정";
                                break;
                            case "0"://개인일정
                                
                                data.KindDesk = GetResource("pim1");//"개인일정";
                                break;
                            case "3"://부서일정
                                
                                data.KindDesk = GetResource("pim2");//"부서일정";
                                break;
                            case "1"://공유일정

                                data.KindDesk = GetResource("pim3");//"공유일정";

                                break;
                            case "2"://전체일정
                                data.KindDesk = GetResource("pim4");//"전체일정";
                                break;
                            case "8"://할일
                                data.KindDesk = GetResource("TODO");//"할일";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString();
                                data.stringENDTime = "";
                               // data.KindDesk = dtr["Priority"].ToString();
                                switch (dtr["AlertType"].ToString())
                                {
                                    case "W":
                                        data.stringENDTime = GetResource("priority1");//"시작안함";
                                        break;
                                    case "I":
                                        data.stringENDTime = GetResource("priority2");//"진행중";
                                        break;
                                    case "E":
                                        data.stringENDTime = GetResource("priority3");//"완료";
                                        break;
                                    case "C":
                                        data.stringENDTime = GetResource("priority4");// "취소";
                                        break;
                                    case "T":
                                        data.stringENDTime = GetResource("priority5");//"위임";
                                        break;
                                    case "D":
                                        data.stringENDTime = GetResource("priority6");//"연기";
                                        break;
                                }
                                break;
                        }

                        listobj.Add(data);
                    }
                }
            }
            return listobj;
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetALLListCnt(string p)
        {
            List<object> listobj = new List<object>();
             
            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;
            SetDecryptString(p);

            BE.ScheduleInfo _info = new ScheduleInfo();


            _info.CompanyID = CurrentUserInfo.CompanyID;
            _info.UserID = CurrentUserInfo.UserID;

            switch (GetDecryptParam("StartDate", "").ToString())
            {
                case "0":  //전체
                    _info.StartTime = DateTime.Parse("2000-01-01");
                    _info.EndTime = DateTime.Now.AddDays(1);
                    break;
                case "W":
                    _info.StartTime = DateTime.Now.AddDays(-7);
                    _info.EndTime = DateTime.Now;
                    break;
                case "M":
                    _info.StartTime = DateTime.Now.AddMonths(-1);
                    _info.EndTime = DateTime.Now;
                    break;
                default:
                    _info.StartTime = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _info.EndTime = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                    break;
            }


           string SearchKind =  GetDecryptParam("SearchKind", "").ToString().Trim();

           if (SearchKind == "Title")
           {
               //제목
               _info.Title = GetDecryptParam("SearchValue", "").ToString().Trim();
           }
           else if (SearchKind == "Name")
           {
               //이름
               _info.UserName = GetDecryptParam("SearchValue", "").ToString().Trim();
           }
            _info.DeptID = CurrentUserInfo.DeptID;

            DAL.Schedule dal = new DAL.Schedule();

            int irt  = dal.GetAllSearchCnt(_info);

           
            return irt;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSearchList(string p)
        {
            List<object> listobj = new List<object>();

            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;
            SetDecryptString(p);

            BE.ScheduleInfo _info = new ScheduleInfo();

            _info.CompanyID = CurrentUserInfo.CompanyID;
            _info.UserID = CurrentUserInfo.UserID;

            switch (GetDecryptParam("StartDate", "").ToString())
            {
                case "0":  //전체
                    _info.StartTime = DateTime.Parse("2000-01-01");
                    _info.EndTime = DateTime.Now.AddDays(1);
                    break;
                case "W":
                    _info.StartTime = DateTime.Now.AddDays(-7);
                    _info.EndTime = DateTime.Now;
                    break;
                case "M":
                    _info.StartTime = DateTime.Now.AddMonths(-1);
                    _info.EndTime = DateTime.Now;
                    break;
                default:
                    _info.StartTime = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _info.EndTime = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                    break;
            }

            string SearchKind = GetDecryptParam("SearchKind", "").ToString().Trim();

            if (SearchKind == "Title")
            {
                //제목
                _info.Title = GetDecryptParam("SearchValue", "").ToString().Trim();
            }
            else if (SearchKind == "Name")
            {
                //이름
                _info.UserName = GetDecryptParam("SearchValue", "").ToString().Trim();
            }

            _info.DeptID = CurrentUserInfo.DeptID;

            DAL.Schedule dal = new DAL.Schedule();

            DataTableReader dtr = dal.GetSearchExcel(_info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo data = new ScheduleInfo();
                        data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        data.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();

                        string _name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                         dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                         dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                         );
                        string _title = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                            dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang
                            );

                        data.UserName = _name + "(" + _title + ")";
                        data.Title = dtr["Title"].ToString();


                        data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString();
                        data.stringENDTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortTimeString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortTimeString();

                        data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        //종류
                        data.OpenType = dtr["kind"].ToString();

                        switch (data.OpenType)
                        {
                            case "-1"://나의일정

                                data.KindDesk = GetResource("MYSCHEDULE");// "나의일정";
                                break;
                            case "0"://개인일정

                                data.KindDesk = GetResource("pim1");//"개인일정";
                                break;
                            case "3"://부서일정

                                data.KindDesk = GetResource("pim2");//"부서일정";
                                break;
                            case "1"://공유일정

                                data.KindDesk = GetResource("pim3");//"공유일정";

                                break;
                            case "2"://전체일정
                                data.KindDesk = GetResource("pim4");//"전체일정";
                                break;
                            case "8"://할일
                                data.KindDesk = GetResource("TODO");//"할일";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString();
                                data.stringENDTime = "";
                                // data.KindDesk = dtr["Priority"].ToString();
                                switch (dtr["AlertType"].ToString())
                                {
                                    case "W":
                                        data.stringENDTime = GetResource("priority1");//"시작안함";
                                        break;
                                    case "I":
                                        data.stringENDTime = GetResource("priority2");//"진행중";
                                        break;
                                    case "E":
                                        data.stringENDTime = GetResource("priority3");//"완료";
                                        break;
                                    case "C":
                                        data.stringENDTime = GetResource("priority4");// "취소";
                                        break;
                                    case "T":
                                        data.stringENDTime = GetResource("priority5");//"위임";
                                        break;
                                    case "D":
                                        data.stringENDTime = GetResource("priority6");//"연기";
                                        break;
                                }
                                break;
                        }

                        listobj.Add(data);
                    }
                }
            }
            return listobj;
        }
        #endregion 



        #endregion

        #region Set
        #endregion
    }
}
