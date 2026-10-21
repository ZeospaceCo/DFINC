using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Content
{
    /// <summary>
    /// ListWeek의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListWeek : Base.BaseWSRM
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Content.ListWeek).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string tmpdate = GetDecryptParam("tmpdate", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchUsable = GetDecryptParam("searchUsable", ""); 

                DAL.List dalList = new DAL.List();
                DataSet dss = dalList.GetWeekList(CurrentUserInfo.CompanyID, categoryID);

                DAL.Reverse dal = new DAL.Reverse();
                DataSet ds = dal.GetWeekList(CurrentUserInfo.CompanyID, categoryID, tmpdate, searchField, searchValue, searchUsable);

                List<object> listobj = new List<object>();
                                
                for (int i = 0; i < 7; i++)
                {
                    int row = 0;                    
                    string weekDay = String.Empty;
                    DateTime weekDate = Convert.ToDateTime(tmpdate).AddDays(i);
                    //string tmpWeekName = weekDate.Month.ToString().PadLeft(2, '0') + "." + weekDate.Day.ToString().PadLeft(2, '0') + " " + WeekDayName(i);
                    string tmpWeekName = DateHelpers.GetCultureDate(weekDate, CurrentUserInfo.PreferredLanguage).Substring(0, 10) + " " + WeekDayName(i);
                    string tmpWeekDay = weekDate.Year + "-" + weekDate.Month.ToString().PadLeft(2, '0') + "-" + weekDate.Day.ToString().PadLeft(2, '0');

                    foreach (DataRow drr in dss.Tables[0].Rows)
                    {                       
                        if (ds != null)
                        {
                            DataRow[] drs = ds.Tables[0].Select("ListID='" + drr["ListID"].ToString() + "'");
                            
                            if (drs.Length > 0)
                            {
                                int ResCheck = 0;
                                
                                foreach (DataRow dr in drs)
                                {
                                    DateTime sDate = Convert.ToDateTime(dr["StartDate"]);
                                    DateTime eDate = Convert.ToDateTime(dr["EndDate"]);

                                    #region 예약 자원이 있을 때
                                    if (CheckTime(weekDate, sDate, eDate))
                                    {
                                        ResCheck += 1;
                                        BE.ReserveInfo info = new BE.ReserveInfo();
                                        row += row + 1;
                                        info.Row = row;
                                        info.WeekName = tmpWeekName;
                                        info.WeekDay = tmpWeekDay;
                                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                                        info.ReservationID = dr["ReservationID"].ToString().ToNumber<int>();
                                        info.ParentReservationID = dr["ParentReservationID"].ToString().ToNumber<int>();
                                        info.ListID = dr["ListID"].ToString().ToNumber<int>();                                        
                                        info.Subject = dr["Subject"].ToString();
                                        info.ReplyCount = dr["ReplyCount"].ToString().ToNumber<int>(); 
                                        info.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                                        info.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                                        int startHour = Convert.ToDateTime(dr["StartDate"].ToString()).Hour;
                                        int endHour = Convert.ToDateTime(dr["EndDate"].ToString()).Hour;                                       
                                        info.StartTimeGap = ((startHour - 6) > 13) ? 14 : (startHour - 6);
                                        info.EndTimeGap = ((20 - endHour) > 13) ? 14 : (21 - endHour);
                                        info.StartMinute = Convert.ToDateTime(dr["StartDate"].ToString()).Minute;
                                        info.EndMinute = Convert.ToDateTime(dr["EndDate"].ToString()).Minute;
                                        info.StartHour = startHour;
                                        info.EndHour = (info.EndMinute > 0) ? endHour - 1 : endHour - 2;

                                        int gap = (dr["DifferMinite"].ToString().ToNumber<int>() / 30);

                                        if ((startHour < 7) && (endHour <= 19))
                                        {
                                            int startgap = 7 - startHour;
                                            gap = gap - (startgap * 2);

                                            if (info.StartMinute > 0)
                                            {
                                                gap = gap + 1;
                                            }

                                            if (info.EndMinute > 0)
                                            {
                                                gap = gap - 1;
                                            }
                                            info.DifferMinite = gap / 2;
                                        }
                                        else if ((startHour >= 7) && (endHour > 19))
                                        {
                                            int endgap = endHour - 20;
                                            gap = gap - (endgap * 2);
                                            if (info.StartMinute > 0)
                                            {
                                                gap = gap - 1;
                                            }
                                            info.DifferMinite = gap / 2;
                                        }
                                        else if ((startHour >= 7) && (endHour <= 19))
                                        {
                                            info.DifferMinite = gap / 2;
                                            if (info.StartMinute > 0)
                                            {
                                                if (info.EndMinute > 0)
                                                {
                                                    info.DifferMinite = (gap / 2) - 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            info.DifferMinite = 13;
                                        }


                                        if (startHour < 7)
                                        {
                                            info.StartMinute = 0;
                                            info.StartTimeGap = 0;
                                        }

                                        if (endHour > 19)
                                        {
                                            if ((endHour == 20) && (info.EndMinute == 0))
                                            {

                                            }
                                            else
                                            {
                                                info.EndMinute = 0;
                                                info.EndTimeGap = 0;
                                            }
                                        }

                                        if (endHour < 7)
                                        {
                                            info.EndMinute = 0;
                                            info.DifferMinite = 0;
                                            info.EndHour = 6;
                                        }

                                        if (startHour > 19)
                                        {
                                            info.StartMinute = 0;
                                        }
                                                

                                        if (info.EndMinute > 0)
                                        {
                                            if (info.EndHour != 19)
                                            {
                                                info.EndTimeGap = info.EndTimeGap - 1;
                                            }
                                        }

                                        DateTime atmpdate = Convert.ToDateTime(weekDate);
                                        // 다른날짜까지 일정이 이어져 있을 경우                                
                                        if (CheckTime2(atmpdate, info.EndDate))
                                        {
                                            if (CheckTime3(atmpdate, info.StartDate))
                                            {
                                                info.DifferMinite = 13;
                                                info.StartTimeGap = 0;
                                                info.StartMinute = 0;
                                                info.EndTimeGap = 0;
                                                info.EndMinute = 0;
                                            }
                                            else
                                            {
                                                if (Convert.ToDateTime(dr["StartDate"].ToString()).Minute > 0)
                                                {
                                                    info.DifferMinite = 19 - startHour;
                                                }
                                                else
                                                {
                                                    info.DifferMinite = 20 - startHour;
                                                }
                                                info.EndTimeGap = 0;
                                                info.EndMinute = 0;
                                            }
                                        }
                                        else
                                        {
                                            // 이전날짜까지 일정이 이어져 있을 경우 
                                            if (CheckTime3(atmpdate, info.StartDate))
                                            {
                                                info.DifferMinite = endHour - 7;
                                                info.StartTimeGap = 0;
                                                info.StartMinute = 0;
                                            }
                                        }

                                        info.DifferMinite = (info.DifferMinite > 13) ? 13 : info.DifferMinite;

                                        info.Usable = dr["Usable"].ToString();
                                        info.WriteUserID = dr["WriteUserID"].ToString().ToNumber<int>();

                                        //예약자 이름
                                        info.WriteUserName = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(),
                                                                            dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                                                                            dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                                        // 미승인이고 등록자가 본인일 경우 승인요청 버튼 보이게 처리
                                        if ((info.Usable == "0") && (CurrentUserInfo.UserID == info.WriteUserID))
                                        {
                                            info.UsableStr = resMng.GetString("RequestSend");
                                        }
                                        else
                                        {
                                            info.UsableStr = "";
                                        }


                                        info.Title = dr["Title"].ToString();
                                        info.ManagerUserID = dr["ManagerUserID"].ToString().ToNumber<int>();

                                        //자원 담당자 이름
                                        string[] arrManyName = dr["MManyName"].ToString().Split('|');
                                        string tmpManagerName = "";
                                        if (arrManyName.Length == 7)
                                        {
                                            tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                                   arrManyName[2], arrManyName[3], arrManyName[4],
                                                                                   arrManyName[5], arrManyName[6], PreLang);
                                        }

                                        info.ManagerName = resMng.GetString("ResourcesUser") + " : " + tmpManagerName;

                                        info.Authority = dr["Authority"].ToString();

                                        if (dr["Authority"].ToString() == "1")
                                        {
                                            /*
                                            // 관리자 권한 
                                            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
                                            bool isRoleRM = CurrentUserInfo.IsInRole("resource");//resource admin

                                            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleRM)
                                            {
                                                info.Admin = "Y";
                                            }
                                            else
                                            {
                                            */
                                                if (info.ManagerUserID == CurrentUserInfo.UserID)
                                                {
                                                    info.Admin = "Y";
                                                }
                                                else
                                                {
                                                    info.Admin = "N";
                                                }
                                            //}
                                        }
                                        else
                                        {
                                            info.Admin = "N";
                                        }

                                        info.IsGuard = dr["IsGuard"].ToString();
                                        //보안 예약인 경우 해당 예약에 대해서 권한체크
                                        if (info.IsGuard == "1")
                                        {
                                            //관리자 권한 체크                                            
                                            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
                                            bool isRoleRM = CurrentUserInfo.IsInRole("resource");//resource admin

                                            if (info.WriteUserID == CurrentUserInfo.UserID || CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleRM || (info.Admin == "Y"))
                                            {
                                                info.Auth = 1;
                                            }
                                            else
                                            {
                                                info.Auth = CheckAuth(info.ReservationID);
                                                if (info.Auth <= 0)
                                                {
                                                    //보안예약인데 권한이 없을경우.                                                    
                                                    info.Subject = resMng.GetString("SecurityReservation");
                                                    info.WriteUserName = resMng.GetString("SecurityReservation");
                                                }
                                            }

                                        }
                                        else
                                        {
                                            info.Auth = 1;
                                        }

                                        listobj.Add(info);
                                    }
                                    #endregion
                                }

                                if ((ResCheck == 0) && (searchValue == ""))
                                {
                                    #region 예약 자원이 없을 때

                                    BE.ReserveInfo info = new BE.ReserveInfo();
                                    row += row + 1;
                                    info.Row = row;
                                    info.ListID = drr["ListID"].ToString().ToNumber<int>();
                                    info.WeekName = tmpWeekName;
                                    info.WeekDay = tmpWeekDay;
                                    info.Subject = resMng.GetString("NoResource");
                                    info.StartTimeGap = 0;
                                    info.EndTimeGap = 0;
                                    info.StartMinute = 0;
                                    info.EndMinute = 0;
                                    info.DifferMinite = 13;
                                    info.Usable = "3";
                                    info.WriteUserID = 0;
                                    info.WriteUserName = String.Empty;
                                    info.UsableStr = "";
                                    info.Title = drr["Title"].ToString();
                                    info.ReplyCount = 0;
                                    info.ManagerUserID = drr["ManagerUserID"].ToString().ToNumber<int>();
                                    info.Authority = drr["Authority"].ToString();

                                    //자원 담당자 이름
                                    string[] arrManyName = drr["ManyName"].ToString().Split('|');
                                    string tmpManagerName = "";
                                    if (arrManyName.Length == 7)
                                    {
                                        tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                               arrManyName[2], arrManyName[3], arrManyName[4],
                                                                               arrManyName[5], arrManyName[6], PreLang);
                                    }

                                    info.ManagerName = resMng.GetString("ResourcesUser") + " : " + tmpManagerName;
                                    info.Admin = "N";

                                    listobj.Add(info);

                                    #endregion
                                }                                
                            }
                            else
                            {
                                if (searchValue == "")
                                {
                                    #region 예약 자원이 없을 때

                                    BE.ReserveInfo info = new BE.ReserveInfo();
                                    row += row + 1;
                                    info.Row = row;
                                    info.ListID = drr["ListID"].ToString().ToNumber<int>();
                                    info.WeekName = tmpWeekName;
                                    info.WeekDay = tmpWeekDay;
                                    info.Subject = resMng.GetString("NoResource");
                                    info.StartTimeGap = 0;
                                    info.EndTimeGap = 0;
                                    info.StartMinute = 0;
                                    info.EndMinute = 0;
                                    info.DifferMinite = 13;
                                    info.Usable = "3";
                                    info.WriteUserID = 0;
                                    info.WriteUserName = String.Empty;
                                    info.UsableStr = "";
                                    info.Title = drr["Title"].ToString();
                                    info.ReplyCount = 0;
                                    info.ManagerUserID = drr["ManagerUserID"].ToString().ToNumber<int>();
                                    info.Authority = drr["Authority"].ToString();

                                    //자원 담당자 이름
                                    string[] arrManyName = drr["ManyName"].ToString().Split('|');
                                    string tmpManagerName = "";
                                    if (arrManyName.Length == 7)
                                    {
                                        tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                               arrManyName[2], arrManyName[3], arrManyName[4],
                                                                               arrManyName[5], arrManyName[6], PreLang);
                                    }

                                    info.ManagerName = resMng.GetString("ResourcesUser") + " : " + tmpManagerName;
                                    info.Admin = "N";

                                    listobj.Add(info);

                                    #endregion
                                }
                            }
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
        /// 작성일자 : 2012-02-27
        /// 작성목적 : 요일 정보 가져오기
        /// </summary>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        private string WeekDayName(int _gubun)
        {
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Content.ListWeek).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            string rtnVal = "";
            switch (_gubun)
            {
                case 0: rtnVal = "(" + resMng.GetString("Sun") + ")"; break;
                case 1: rtnVal = "(" + resMng.GetString("Mon") + ")"; break;
                case 2: rtnVal = "(" + resMng.GetString("Tue") + ")"; break;
                case 3: rtnVal = "(" + resMng.GetString("Wed") + ")"; break;
                case 4: rtnVal = "(" + resMng.GetString("Thu") + ")"; break;
                case 5: rtnVal = "(" + resMng.GetString("Fri") + ")"; break;
                case 6: rtnVal = "(" + resMng.GetString("Sat") + ")"; break;
            };

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_nowDate"></param>
        /// <param name="_sDate"></param>
        /// <param name="_eDate"></param>
        /// <returns></returns>
        private bool CheckTime(DateTime _tmpDate, DateTime _sDate, DateTime _eDate)
        {
            bool rtn = false;

            string tmpDate = _tmpDate.Year + _tmpDate.Month.ToString().PadLeft(2, '0') + _tmpDate.Day.ToString().PadLeft(2, '0');
            string sDate = _sDate.Year + _sDate.Month.ToString().PadLeft(2, '0') + _sDate.Day.ToString().PadLeft(2, '0');
            string eDate = _eDate.Year + _eDate.Month.ToString().PadLeft(2, '0') + _eDate.Day.ToString().PadLeft(2, '0');

            if(sDate.ToNumber<int>() <= tmpDate.ToNumber<int>())
            {
                if (eDate.ToNumber<int>() >= tmpDate.ToNumber<int>())
                {
                    rtn = true;
                }
            }            

            return rtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_nowDate"></param>
        /// <param name="_sDate"></param>
        /// <param name="_eDate"></param>
        /// <returns></returns>
        private bool CheckTime2(DateTime _tmpDate, DateTime _eDate)
        {
            bool rtn = false;

            string tmpDate = _tmpDate.Year + _tmpDate.Month.ToString().PadLeft(2, '0') + _tmpDate.Day.ToString().PadLeft(2, '0');
            string eDate = _eDate.Year + _eDate.Month.ToString().PadLeft(2, '0') + _eDate.Day.ToString().PadLeft(2, '0');

            if (eDate.ToNumber<int>() > tmpDate.ToNumber<int>())
            {
                rtn = true;
            }

            return rtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_nowDate"></param>
        /// <param name="_sDate"></param>
        /// <param name="_sDate"></param>
        /// <returns></returns>
        private bool CheckTime3(DateTime _tmpDate, DateTime _sDate)
        {
            bool rtn = false;

            string tmpDate = _tmpDate.Year + _tmpDate.Month.ToString().PadLeft(2, '0') + _tmpDate.Day.ToString().PadLeft(2, '0');
            string sDate = _sDate.Year + _sDate.Month.ToString().PadLeft(2, '0') + _sDate.Day.ToString().PadLeft(2, '0');

            if (sDate.ToNumber<int>() < tmpDate.ToNumber<int>())
            {
                rtn = true;
            }

            return rtn;
        }

		
        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-24
        /// 작성목적 : 승인 / 거부 처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int ModUsable(string p)
        {
            SetDecryptString(p);
            string usable = GetDecryptParam("usable", "0");//1 : 승인; 2: 거부
            int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();            
            
            bool rtnVal = false;
            DAL.Reverse dal = new DAL.Reverse();
            rtnVal = dal.Mod(CurrentUserInfo.CompanyID, reservationID, usable);

            if(rtnVal)
            {
                //알람발송
                SendAlarm(reservationID, usable);

                // 대기자원 카운트 가져오기
                DAL.Reverse Rdal = new DAL.Reverse();
                return Rdal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "N", "0", "", "", "", "");
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-09
        /// 작성목적 : 범례 상태값 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddModUsable(string p)
        {
            try
            {
                SetDecryptString(p);
                string usable = GetDecryptParam("usable", "");

                BE.UsableInfo info = new BE.UsableInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.Usable = usable;

                DAL.Usable dal = new DAL.Usable();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-14
        /// 작성목적 : 승인 요청 알람
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SendRequest(string p)
        {
            try
            {
                SetDecryptString(p);
                int reservationID = GetDecryptParam("reservationID", "").ToNumber<int>();

                DAL.Reverse dal = new DAL.Reverse();
                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, reservationID);

                BE.ReserveInfo info = new BE.ReserveInfo();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.Subject = dtr["Subject"].ToString().ToReverseXSS();
                            info.WriteUserID = CurrentUserInfo.UserID;
                            info.WriteUserName = CurrentUserInfo.Name;
                            info.ModifyUserID = CurrentUserInfo.UserID;
                            info.ModifyUserName = CurrentUserInfo.Name;
                            info.PreLang = PreLang;
                            info.SMSLoginID = CurrentUserInfo.LogInID;
                            info.Mail = CurrentUserInfo.Name + "<" + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain + ">";
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.Usable = "0";
                            info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                            info.Title = dtr["Title"].ToString();
                        }
                    }
                }

                BLL.Reserve bll = new BLL.Reserve();
                bll.SendAlarm(info, reservationID, info.StartDate, "REQUEST");

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion 


        /// <summary>
        /// 보안 예약일 경우 해당 예약의 권한이 있는지 체크
        /// </summary>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        private int CheckAuth(int _reservationID)
        {
            int rtnVal = 0;
            DAL.ShareUser dal = new DAL.ShareUser();
            rtnVal = dal.GetShareUserCntByID(CurrentUserInfo.CompanyID, _reservationID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);

            return rtnVal;
        }

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
