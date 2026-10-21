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
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ToDo : Base.BaseWSPim
    {

        public ToDo()
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.ToDo.Add).Assembly;	//어셈블리 선언
            #endregion

            this.GetResourceManager();
        }

        #region Get
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetQuickTodoList(string p)
        {
            List<object> listobj = new List<object>();
            string companyID = string.Empty;
            string userid = string.Empty;
            string sOpenType = string.Empty;
            string sDtae = string.Empty;
            SetDecryptString(p);

            companyID = GetDecryptParam("companyID", CurrentUserInfo.CompanyID.ToString());
            userid = GetDecryptParam("UserID", CurrentUserInfo.UserID.ToString());
            if (userid == "0")
            {
                userid = CurrentUserInfo.UserID.ToString();
            }
            sOpenType = GetDecryptParam("OpenType", "0");
            sDtae = GetDecryptParam("Date", DateTime.Now.ToString("T_yyyyMMdd"));
            if (sDtae.Length > 2)
            {
                if (sDtae.Substring(0, 2) == "T_")
                {
                    sDtae = sDtae.Replace("T_", "");
                    sDtae = sDtae.Substring(0, 4) + "-" + sDtae.Substring(4, 2) + "-" + sDtae.Substring(6, 2);
                }

            }
            SetDecryptString(p);
            string[] arOpentype = sOpenType.Split(',');
            for (int o = 0; o < arOpentype.Length; o++)
            {

                if (arOpentype[o].Length > 0)
                {
                    BE.ToDoInfo _info = new ToDoInfo();
                    DAL.ToDo _biz = new DAL.ToDo();

                    _info.CompanyID = companyID.ToNumber<int>();
                    _info.UserID = userid.ToNumber<int>();
                    _info.DeptID = CurrentUserInfo.DeptID;
                    _info.StartDate = DateTime.Parse(sDtae);

                    DataTable dt = new DataTable();
                    DataTableReader dtr = new DataTableReader(dt);

                    switch (arOpentype[o])
                    {
                        case "-1":  //나의
                            dtr = _biz.GetTodo_Mine(_info);
                            break;
                        case "0":  //개인
                            dtr = _biz.GetTodo_Person(_info);
                            break;
                        case "3":  //부서
                            dtr = _biz.GetTodo_Dept(_info);
                            break;
                        case "1":  //공유
                            dtr = _biz.GetTodo_Share(_info);
                            break;
                        case "2":  //전체
                            dtr = _biz.GetTodo_All(_info);
                            break;
                    }

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                BE.ToDoInfo data = new ToDoInfo();

                                data.ToDoID = dtr["ToDoID"].ToString().ToNumber<int>();

                                string username = "[" + GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                               , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang) + "]";

                                if (dtr["UserID"].ToString().ToNumber<int>() == CurrentUserInfo.UserID)
                                {
                                    username = "";
                                }

                                //data.Title = username + MaxTextCut(dtr["Title"].ToString(), 25, "...");
                                data.Title = username + dtr["Title"].ToString();
                                data.StartDate = DateTime.Parse(dtr["StartDate"].ToString());
                                data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                                switch (dtr["OpenType"].ToString())
                                {
                                    case "-1":
                                        data.Type = GetResource("TYPE1");//"나의";
                                        break;
                                    case "0":
                                        data.Type = GetResource("TYPE2");//"개인";
                                        break;
                                    case "3":
                                        data.Type = GetResource("TYPE3");//"부서";
                                        break;
                                    case "1":
                                        data.Type = GetResource("TYPE4");//"공유";
                                        break;
                                    case "2":
                                        data.Type = GetResource("TYPE5");//"전체";
                                        break;

                                }

                                switch (dtr["Priority"].ToString())
                                {
                                    case "0":
                                        data.Priority = "";
                                        break;
                                    case "1":
                                        data.Priority = GetResource("TOPAbb");//"상";
                                        break;
                                    case "2":
                                        data.Priority = GetResource("MIDDLEAbb");//"중";
                                        break;
                                    case "3":
                                        data.Priority = GetResource("BOTTOMAbb");//"하";
                                        break;
                                }
                                data.Status = dtr["Status"].ToString();

                                data.UserName = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                     dtr["NameEn"].ToString(), dtr["NameJa"].ToString()
                                   , dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                   PreLang);

                                data.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();

                                listobj.Add(data);
                            }
                        }
                    }

                }
            }

            return listobj;
        }
        #endregion

        #region Set
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddTodo(string p)
        {
            SetDecryptString(p);
            BE.ToDoInfo info = new ToDoInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Type = GetDecryptParam("Type", "");
            info.Status = GetDecryptParam("Status", "");
            info.Priority = GetDecryptParam("Priority", "0"); ;
            info.Title = GetDecryptParam("Title", "");
            info.Content = GetDecryptParam("Content", "");
            info.StartDate = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd 00:00")));

            info.EndDate = DateTime.Parse(GetDecryptParam("EndDate", info.StartDate.ToString("yyyy-MM-dd 23:59")));
            info.ScheduleYN = GetDecryptParam("ScheduleYN", "N");
            info.IsOpen = "Y";
            info.UserID = CurrentUserInfo.UserID;
            info.ReservationID = GetDecryptParam("ScheduleYN", "0").ToNumber<int>();
            info.OpenType = GetDecryptParam("OpenType", "1").ToNumber<int>();
            info.DeptID = CurrentUserInfo.DeptID;
            DAL.ToDo biz = new DAL.ToDo();

            int scheduleid = biz.TodoAdd(info);



            return 1;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelTodo(string p)
        {
            SetDecryptString(p);
            int companyID = 0;
            int todoID = 0;

            companyID = GetDecryptParam("CompnayID", "0").ToNumber<int>();
            todoID = GetDecryptParam("TodoID", "0").ToNumber<int>();

            DAL.ToDo dal = new DAL.ToDo();

            return dal.DelTodo(companyID, todoID);


        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModTodo(string p)
        {
            SetDecryptString(p);
            int companyID = 0;
            int todoID = 0;
            int scope = 0;
            companyID = GetDecryptParam("CompnayID", "0").ToNumber<int>();
            todoID = GetDecryptParam("TodoID", "0").ToNumber<int>();
            scope = GetDecryptParam("SCOPE", "0").ToNumber<int>();

            BE.ToDoInfo info = new BE.ToDoInfo();
            DAL.ToDo dal = new DAL.ToDo();
            DataTableReader dtr = dal.GetTodoID(companyID, todoID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info.CompanyID = companyID;
                        info.Type = dtr["TYPE"].ToString();
                        info.Status = dtr["Status"].ToString();
                        info.Priority = dtr["Priority"].ToString();
                        info.Content = dtr["Content"].ToString();
                        info.StartDate = DateTime.Parse(dtr["StartDate"].ToString());
                        info.EndDate = DateTime.Parse(dtr["EndDate"].ToString());
                        info.ScheduleYN = dtr["ScheduleYN"].ToString();
                        info.IsOpen = dtr["IsOpen"].ToString();
                        info.UserID = CurrentUserInfo.UserID;
                        info.ReservationID = int.Parse(dtr["ReservationID"].ToString());
                        info.ToDoID = todoID;
                        info.DeptID = CurrentUserInfo.DeptID;

                        if (GetDecryptParam("OpenType", "0") == "-1")
                        {
                            info.OpenType = dtr["OpenType"].ToString().ToNumber<int>();
                        }
                        else
                        {
                            info.OpenType = GetDecryptParam("OpenType", "0").ToNumber<int>();
                        }

                        //제목
                        if (GetDecryptParam("Title", "").Trim().Length == 0)
                        {
                            info.Title = dtr["title"].ToString().ToReverseXSS();
                        }
                        else
                        {
                            info.Title = GetDecryptParam("Title", "").Trim().ToReverseXSS();
                        }

                        //중요도
                        if (GetDecryptParam("Priority", "").Trim().Length == 0)
                        {
                            info.Priority = dtr["Priority"].ToString();
                        }
                        else
                        {
                            info.Priority = GetDecryptParam("Priority", "").Trim();
                        }

                        //진척도
                        if (GetDecryptParam("Status", "").Trim().Length == 0)
                        {
                            info.Status = dtr["Status"].ToString();
                        }
                        else
                        {
                            info.Status = GetDecryptParam("Status", "").Trim();
                        }

                        //일정
                        switch (scope)
                        {
                            case 1://내일할일

                                info.StartDate = info.StartDate.AddDays(1);
                                info.EndDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 23:59"));
                                break;
                            case 2://특정일까지 할일
                                info.EndDate = DateTime.Parse(GetDecryptParam("DateInfo", "").Trim());
                                break;
                            case 3://특정일에 할일
                                info.StartDate = DateTime.Parse(GetDecryptParam("DateInfo", "").Trim());
                                info.EndDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 23:59"));
                                break;
                            case 4:
                            case 5:
                                info.Status = "D";
                                break;

                        }

                        //info.Status = "D";
                        //dal.ModTodo(info);
                        if (scope == 0  )
                        {
                            dal.ModTodo(info);
                        }
                        else if (scope == 3)
                        {
                            dal.TodoAdd(info);
                        }
                        else if (scope == 1)
                        {
                            dal.TodoAdd(info);
                        }
                        else if (scope == 2)
                        {
                            DateTime d = info.StartDate;
                            DateTime e = info.EndDate;
                            while (true)
                            {
                                d = d.AddDays(1);
                                info.StartDate = d;
                                info.EndDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 23:59"));
                                dal.TodoAdd(info);
                                if (d >= e)
                                {
                                    break;
                                }
                            }


                        }
                        else if (scope == 5) //특정일연기
                        {
                            info.Status = "D";
                            dal.ModTodo(info);

                            info.StartDate = DateTime.Parse(GetDecryptParam("DateInfo", "").Trim());
                            info.EndDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 23:59"));
                            info.Status = "W";
                            dal.TodoAdd(info);
                        }
                        else if (scope == 4) //내일로연기
                        {
                            info.Status = "D";
                            dal.ModTodo(info);

                            info.StartDate = info.StartDate.AddDays(1);
                            info.EndDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 23:59"));
                            info.Status = "W";
                            dal.TodoAdd(info);
                        }
                        else if (scope == 7) //일정에 반영
                        {


                            string[] arTime = GetDecryptParam("DateInfo", "").Trim().Split('|');

                            Pim.BE.ScheduleInfo pinfo = new Pim.BE.ScheduleInfo();
                            pinfo.CompanyID = CurrentUserInfo.CompanyID;
                            pinfo.ToDoID = 0;
                            pinfo.Type = "I";
                            pinfo.Title = info.Title;
                            pinfo.Content = info.Content;
                            pinfo.StartTime = info.StartDate;
                            pinfo.EndTime = info.StartDate;
                            pinfo.Repeat = "N";
                            pinfo.Alert = "N";
                            pinfo.AlertType = "";
                            pinfo.AlertTime = 0;
                            pinfo.AlertTargetAddress = "";
                            pinfo.IsOpen = "Y";
                            pinfo.UserID = CurrentUserInfo.UserID;
                            pinfo.DeptID = CurrentUserInfo.DeptID;
                            pinfo.OpenType = info.OpenType.ToString();
                            pinfo.Lunar = "N";
                            pinfo.StatusID = 0;//없음

                            Pim.DAL.Schedule pimsch = new Pim.DAL.Schedule();
                            int pSchedule = pimsch.Add(pinfo);
                            Pim.BE.DisplayInfo dinfo = new Pim.BE.DisplayInfo();
                            dinfo.CompanyID = pinfo.CompanyID;
                            dinfo.UserID = pinfo.UserID;
                            dinfo.StartDate = DateTime.Parse(pinfo.StartTime.ToShortDateString() + " " + arTime[0]);
                            dinfo.EndDate = DateTime.Parse(pinfo.StartTime.ToShortDateString() + " " + arTime[1]);
                            dinfo.ScheduleID = pSchedule;
                            Pim.DAL.Display bizdis = new Pim.DAL.Display();
                            bizdis.AddDisplay(dinfo);
                        }


                    }
                }
            }
            //제목
            //중요도
            //진척도
            //일정
            //일정선택값

            return true;

        }

        #endregion

        #region utill
        public string MaxTextCut(string strSource, int iMaxLength, string strTail)
        {
            string strRt = string.Empty;
            strRt = strSource.ToReverseXSS();
            if (strSource.ToReverseXSS().Length > iMaxLength)
            {
                strRt = strSource.ToReverseXSS().Substring(0, iMaxLength) + strTail;
            }

            return strRt.ToXSS();
        }


        #endregion
    }
}
