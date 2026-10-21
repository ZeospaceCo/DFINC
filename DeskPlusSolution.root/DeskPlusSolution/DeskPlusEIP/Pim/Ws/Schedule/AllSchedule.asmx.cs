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
    public class AllSchedule : Base.BaseWSPim
    {

        public AllSchedule()
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Schedule.AllSchedule).Assembly;	//어셈블리 선언

            #endregion

            this.GetResourceManager();
        }

        #region Get
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAllUserScheduleList(string p)
        {
            List<object> listobj = new List<object>();

            int _UserID = 0;
            int _CompanyID = 0;


            string _UserList = string.Empty;
            string _btitle = string.Empty;
            string _pname = string.Empty;
            string _OpenType = string.Empty;
            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;

            SetDecryptString(p);

            _UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _OpenType = GetDecryptParam("OpenType", "");
            _UserList = GetDecryptParam("UserList", "");


            //////////////////
            Hashtable htUser = new Hashtable();
             

            if (_UserList.Length > 0)
            {
                string[] aruser = _UserList.Split(',');
                
                string _UserListTemp = string.Empty;

                for (int f = 0; f < aruser.Length; f++)
                {
                    if (aruser[f].Length > 0)
                    {
                        if (aruser[f].Substring(0, 1) == "P")
                        {
                            if (htUser[aruser[f].ToString()] == null)
                            {
                                if (htUser[aruser[f].ToString().Replace("P", "")] == null)
                                {
                                    htUser.Add(aruser[f].ToString().Replace("P", ""), "0");
                                    _UserListTemp += aruser[f].ToString().Replace("P", "") + ",";
                                }
                            }
                        }
                        else if (aruser[f].Substring(0, 1) == "M")
                        {
                        }
                        else
                        {
                            if (htUser[aruser[f].ToString()] == null)
                            {
                                htUser.Add(aruser[f].ToString(), "0");
                                _UserListTemp += aruser[f].ToString() + ",";
                            }
                        }
                    }
                }

                _UserList = _UserListTemp;
            }



            switch (GetDecryptParam("StartDate", "").ToString())
            {

                case "0":
                    //_StartDate = _StartDate = DateTime.Parse("1901-01-01");DateTime.Now.ToString("yyyy-MM-dd");
                    _StartDate = DateTime.Parse( DateTime.Now.ToString("yyyy-MM-dd")); //_StartDate = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01"));
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                case "WEEK":
                    _StartDate = DateTime.Now.AddDays(-7);
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                case "MON":
                    _StartDate = DateTime.Now.AddMonths(-1);
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                default:
                    _StartDate = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _EndDate = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 23:59")));
                    break;
            }

            if (_StartDate == _EndDate)
            {
                _EndDate = DateTime.Parse(_StartDate.ToString("yyyy-MM-dd 23:59")); 
                     
            }



            _btitle = GetDecryptParam("title", "").ToString().Trim();
            _pname = GetDecryptParam("name", "").ToString().Trim();

            DAL.Schedule biz = new DAL.Schedule();
            DataTable dt = new DataTable();
            DataTableReader dtr = new DataTableReader(dt);

            dtr = biz.GetAllUserListSearch(_UserID, _CompanyID, _StartDate, _EndDate, _OpenType, _UserList, _btitle, _pname, CurrentUserInfo.DeptID);

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
                        data.ToDoID = dtr["ToDoID"].ToString().ToNumber<int>();
                        data.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();

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
                        if (dtr["Kind"].ToString() == "8")
                        {
                            data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString();// +"~" + DateTime.Parse(dtr["EndTime"].ToString()).ToString("yyyy-MM-dd");

                            switch (dtr["AlertType"].ToString())
                            {
                                case "W":
                                    data.stringENDTime =  GetResource("priority1");//"시작안함";
                                    break;
                                case "I":
                                    data.stringENDTime = GetResource("priority2");//"진행중";
                                    break;
                                case "E":
                                    data.stringENDTime = GetResource("priority3");//"완료";
                                    break;
                                case "C":
                                    data.stringENDTime =GetResource("priority4");// "취소";
                                    break;
                                case "T":
                                    data.stringENDTime = GetResource("priority5");//"위임";
                                    break;
                                case "D":
                                    data.stringENDTime = GetResource("priority6");//"연기";
                                    break;
                            }
                            //data.stringENDTime
                        }
                        else
                        {
                            data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString();
                            data.stringENDTime = DateTime.Parse(dtr["StartTime"].ToString()).ToString("HH:mm") + "~" + DateTime.Parse(dtr["ENDTime"].ToString()).ToString("HH:mm");

                            // data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm") + "~<br/>" + DateTime.Parse(dtr["EndTime"].ToString()).ToString("yyyy-MM-dd  HH:mm");
                        } 

                        data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        //종류
                        data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();

                        data.OpenType = dtr["kind"].ToString();

                        //분류색설정
                        data.StatusColors = dtr["Colors"].ToString();

                        switch (dtr["kind"].ToString())
                        {
                            case "-1"://나의일정
                                data.KindDesk =GetResource("MYSCHEDULE");// "나의일정";
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
                            case "8"://전체일정
                                data.KindDesk = GetResource("TODO");//"할일";
                                break;
                        }

                        if (htUser[data.UserID.ToString()] == null)
                        {
                            htUser.Add(data.UserID.ToString(), "0");
                        }

                        int ucnt = (htUser[data.UserID.ToString()].ToString().ToNumber<int>() + 1);
                        htUser[data.UserID.ToString()] = ucnt;
                        data.Row = ucnt;
                        

                        listobj.Add(data);
                    }
                }
            }


            List<object> listRt = new List<object>();
            foreach (BE.ScheduleInfo _d in listobj)
            {
                if (_d.Row == 1)
                {
                    _d.Row = htUser[_d.UserID.ToString()].ToString().ToNumber<int>();
                }
                else
                {
                    _d.Row = 0;
                }

                listRt.Add(_d);
            }

            return listRt;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUserScheduleListCnt(string p)
        {
            int iRt = 0;

            int _UserID = 0;
            int _CompanyID = 0;


            string _UserList = string.Empty;
            string _btitle = string.Empty;
            string _OpenType = string.Empty;
            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;

            SetDecryptString(p);

            _UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _OpenType = GetDecryptParam("OpenType", "");
            _UserList = GetDecryptParam("UserList", "");

            if (_UserList.Length > 0)
            {
                string[] arUser = _UserList.Split(',');
                _UserList = string.Empty;

                for (int i = 0; i < arUser.Length; i++)
                {
                    if (arUser[i].Length > 0)
                    {
                        string[] arFor = arUser[i].Split('$');
                        _UserList += arFor[1] + ",";
                    }

                }
            }
            else
            {
                return iRt;
            }
            switch (GetDecryptParam("StartDate", "").ToString())
            {

                case "0":
                    _StartDate = _StartDate = DateTime.Parse("1901-01-01");
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                case "W":
                    _StartDate = DateTime.Now.AddDays(-7);
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                case "M":
                    _StartDate = DateTime.Now.AddMonths(-1);
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                default:
                    _StartDate = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _EndDate = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 23:59")));
                    break;
            }
            _btitle = GetDecryptParam("title", "").ToString().Trim();

            DAL.Schedule biz = new DAL.Schedule();
            iRt = biz.GetUserListSearchCnt(_UserID, _CompanyID, _StartDate, _EndDate, _OpenType, _UserList, _btitle);

            return iRt;
        }

        #endregion

        #region Set
        #endregion
    }
}
