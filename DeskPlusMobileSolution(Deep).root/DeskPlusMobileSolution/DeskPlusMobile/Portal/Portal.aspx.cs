using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Data.SqlClient;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Portal
{
    public partial class Portal : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string BGPortalImage = "";
        public string AprREP = "";
        public string AprAPP = "";
        public string AppREL = "";
        public string AppPRO = "";
        public string TaskRE = "";
        public string TaskTO = "";
        public string TaskDE = "";

        public string SelDate = string.Empty;
        public string NowHour = string.Empty;
        public string NowMinute = string.Empty;

        public string bWS = "0";
        public string WSTime = string.Empty;
        public string bWC = "0";
        public string WCTime = string.Empty;
        public string WCTimeHour = "0"; //퇴근시간
        public string WCTimeMinute = "00"; //퇴근시간

        public DateTime bWSTime; //정규 출근시간
        public DateTime bWCTime; //정규 퇴근시간
        public string bWSTimeHour = "0"; //정규 출근시간
        public string bWCTimeHour = "0"; //정규 퇴근시간
        public string bWSTimeMinute = "00"; //정규 출근시간
        public string bWCTimeMinute = "00"; //정규 퇴근시간
        public string isHoli = "0"; //휴일 여부

        public int BadgeCnt = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Portal).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadData();
            }
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-11-23
        /// </summary>
        public void LoadData()
        {
            //SetMsgRegID();
            LoadUser();            
            LoadAuth();
            LoadPortal();

            if (noticeCheck == "Y")
            {
                LoadBoard();
            }

            LoadMoreJob();
            LoadPortalImage();
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2018-12-18
        /// 작성목적 : 메인 설정  
        /// </summary>
        public void LoadPortal()
        {
            int Top = 0;
            int Bottom = 0;

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_Portal");
            sql = string.Format(sql, UserInfo.CompanyID);
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, null))
            {
                while (sdr.Read())
                {
                    if (sdr["Orders"].ToString() == "1")
                    {
                        Top = sdr["GubunID"].ToString().ToNumber<int>();
                    }

                    if (sdr["Orders"].ToString() == "2")
                    {
                        Bottom = sdr["GubunID"].ToString().ToNumber<int>();
                    }
               }
            }

            //포탈문구
            if ((Top == 1) || (Bottom == 1))
            {
                string sqlSlogan = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_Company_SelByID");
                sqlSlogan = string.Format(sqlSlogan, UserInfo.CompanyID);
                using (SqlDataReader sdrS = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlSlogan, null))
                {
                    while (sdrS.Read())
                    {
                        if (Top == 1)
                        {
                            TopPortal = "<li class='slogan'>" + sdrS["Slogan"].ToString().ToReverseXSS().Replace("\n", "<br>") + "</li>";
                        }

                        if (Bottom == 1)
                        {
                            BottomPortal = "<li class='slogan'>" + sdrS["Slogan"].ToString().ToReverseXSS().Replace("\n", "<br>") + "</li>";
                        }
                    }
                }
            }

            //게시판
            if ((Top == 2) || (Bottom == 2))
            {
                if (noticeCheck == "Y")
                {
                    string sqlBoard = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_SYSCode");
                    sqlBoard = String.Format(sqlBoard, CompanyID, "MobileBoard", "NoticeBoard");
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlBoard, "NoticeBoard", null);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        NoticeCID = dr["InterfaceCode"].ToString();
                    }

                    if (NoticeCID != "")
                    {
                        GetBoardList();

                        if (Top == 2)
                        {
                            TopPortal = "<li class='contentblock'><ul>" + NoticeBoardList + "</ul></li>";
                        }

                        if (Bottom == 2)
                        {
                            BottomPortal = "<li class='contentblock'><ul>" + NoticeBoardList + "</ul></li>";
                        }
                    }                  
                }
            }

            //전자결재 - 결재현황
            if ((Top == 3) || (Bottom == 3))
            {
                if (approvalCheck == "Y")
                {
                    GetApprovalCnt();

                    string ApprovalList = @"<li class='contentblock'><ul><li class='title'>" + resMng.GetString("ApprovalStaus") + @"</li><li class='Task-bt'><div>
                                      <button type='button' class='btn-lg br-white' style='width:40%; cursor:pointer' id='AprREP'>" + resMng.GetString("AprREP") + @"<br>(" + AprREP + @")</button>&nbsp;
                                      <button type='button' class='btn-lg br-white' style='width:40%; cursor:pointer' id='AprAPP'>" + resMng.GetString("AprAPP") + @"<br>(" + AprAPP + @")</button><br />
                                      <button type='button' class='btn-lg br-white' style='width:40%; cursor:pointer' id='AppREL'>" + resMng.GetString("AppREL") + @"<br>(" + AppREL + @")</button>&nbsp;
                                      <button type='button' class='btn-lg bg-end br-white' style='width:40%; cursor:pointer' id='AppPRO'>" + resMng.GetString("AppPRO") + @"<br>(" + AppPRO + @")</button>
                                      </div></li></ul></li>";

                    if (Top == 3)
                    {
                        TopPortal = ApprovalList;
                    }

                    if (Bottom == 3)
                    {
                        BottomPortal = ApprovalList;
                    }
                }
            }

            if ((Top != 3) && (Bottom != 3))
            {
                if (approvalCheck == "Y")
                {
                    GetBadgeCnt();
                }
            }

            //업무협업
            if ((Top == 4) || (Bottom == 4))
            {
                if (taskCheck == "Y")
                {
                    GetTaskCnt();

                    string TaskList = @"<li class='contentblock'><ul><li class='title'>" + resMng.GetString("TaskStaus") + @"</li><li class='Task-bt'><div>
                                      <button type='button' class='btn-lg br-white' style='width:40%; cursor:pointer' id='TaskRE'>" + resMng.GetString("TaskRE") + @"<br>" + TaskRE + @"</button>&nbsp;
                                      <button type='button' class='btn-lg br-white' style='width:40%; cursor:pointer' id='TaskTO'>" + resMng.GetString("TaskTO") + @"<br>" + TaskTO + @"</button><br />
                                      <button type='button' class='btn-lg br-white' style='width:40%; cursor:pointer' id='TaskDE'>" + resMng.GetString("TaskDE") + @"<br>" + TaskDE + @"</button>&nbsp;
                                      <button type='button' class='btn-lg bg-end br-white' style='width:40%; cursor:pointer' id='TaskRC'>" + resMng.GetString("TaskRC") + @"<br>Update</button>
                                      </div></li></ul></li>";

                    if (Top == 4)
                    {
                        TopPortal = TaskList;
                    }

                    if (Bottom == 4)
                    {
                        BottomPortal = TaskList;
                    }
                }
            }

            //근태
            if ((Top == 5) || (Bottom == 5))
            {
                if (attendCheck == "Y")
                {
                    GetAttendList();

                    if (Top == 5)
                    {
                        TopPortal = "<li class='contentblock'><ul>" + AttendList + "</ul></li>";
                    }

                    if (Bottom == 5)
                    {
                        BottomPortal = "<li class='contentblock'><ul>" + AttendList + "</ul></li>";
                    }
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2018-12-20
        /// 작성목적 : 결재현황 카운트 가져오기 
        /// </summary>
        public void GetApprovalCnt()
        {
            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_CNT");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;


                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, "dsList", parm);

                DataRow[] dr = ds.Tables["dsList"].Select("CAT = 'AL'");
                BadgeCnt = dr.Length;

                if (dr.Length > 0)
                {
                    AprAPP = dr.Length.ToString("#,###");                    
                }
                else
                {
                    AprAPP = "0";
                }

                dr = ds.Tables["dsList"].Select("CAT = 'FL'");
                if (dr.Length > 0)
                {
                    AprREP = dr.Length.ToString("#,###");
                }
                else
                {
                    AprREP = "0";
                }

                dr = ds.Tables["dsList"].Select("CAT = 'RL'");
                if (dr.Length > 0)
                {
                    AppREL = dr.Length.ToString("#,###");
                }
                else
                {
                    AppREL = "0";
                }

                dr = ds.Tables["dsList"].Select("CAT = 'PL'");
                BadgeCnt += dr.Length;
                if (dr.Length > 0)
                {
                    AppPRO = dr.Length.ToString("#,###");                    
                }
                else
                {
                    AppPRO = "0";
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetApprovalCnt()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-06
        /// 작성목적 : badge 카운트 가져오기 
        /// </summary>
        public void GetBadgeCnt()
        {
            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_MENUCNT");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;


                DataSet dsApp = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, "dsList", parm);

                DataRow[] dr = dsApp.Tables["dsList"].Select("CAT = 'AL'");
                BadgeCnt = dr.Length;

                dr = dsApp.Tables["dsList"].Select("CAT = 'FL'");
                BadgeCnt += dr.Length;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBadgeCnt()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2018-12-20
        /// 작성목적 : 업무협업 카운트 가져오기 
        /// </summary>
        public void GetTaskCnt()
        {
            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_PROC_CNT");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;


                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, "dsList", parm);

                int tot = 0;
                int totcnt = 0;

                // 요청한 협업 
                DataRow[] dr = ds.Tables["dsList"].Select("CAT = 'REQ'");
                TaskRE = "(" + dr[0]["CNT"].ToString() + ")";

                // 요청 받은 협업
                dr = ds.Tables["dsList"].Select("CAT = 'REC'"); // 전체갯수
                tot = dr[0]["CNT"].ToString().ToNumber<int>();

                dr = ds.Tables["dsList"].Select("CAT = 'RECT'"); //읽은 갯수
                totcnt = dr[0]["CNT"].ToString().ToNumber<int>();

                if (totcnt < 0) totcnt = 0;
                TaskTO = "(" + totcnt + "/" + tot + ")";

                //지연된 업무
                dr = ds.Tables["dsList"].Select("CAT = 'DEL'");
                TaskDE = "(" + dr[0]["CNT"].ToString() + ")";

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetTaskCnt()", ex);
                throw;
            }
        }
        
        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-04-20
        /// 작성목적 : 게시판 정보  
        /// </summary>
        public void LoadBoard()
        {
            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_SYSCode");
                sqlQuery = String.Format(sqlQuery, CompanyID, "MobileBoard", "MainBoard");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "UserBoard", null);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string[] arrCID = dr["InterfaceCode"].ToString().Split(',');

                    MainCID = arrCID[0];
                    PCatID = arrCID[1];
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoadBoard()", ex);
                throw;
            }
        }

        /// <summary>
        /// 근태관리 리스트
        /// </summary>
        private void GetAttendList()
        {
            try
            {
                SelDate = TargetYear.ToString();

                if (TargetMonth.ToString().Length < 2)
                {
                    SelDate = SelDate + "-0" + TargetMonth.ToString();
                }
                else
                {
                    SelDate = SelDate + "-" + TargetMonth.ToString();
                }

                if (TargetDay.ToString().Length < 2)
                {
                    SelDate = SelDate + "-0" + TargetDay.ToString();
                }
                else
                {
                    SelDate = SelDate + "-" + TargetDay.ToString();
                }

                NowHour = TargetHour.ToString();
                NowMinute = TargetMinute.ToString();

                Zeospace.DeskPlusMobile.Web.HR.Ws.Data.HR dal = new HR.Ws.Data.HR();

                DataTableReader dtr = dal.GetAttendance(CompanyID.ToNumber<int>(), UserID.ToNumber<int>(), SelDate);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            bWS = dtr["bWS"].ToString();
                            if (dtr["WSTime"].ToString() != "") //출근시간
                            {
                                string hour = Convert.ToDateTime(dtr["WSTime"].ToString()).Hour.ToString();
                                string minute = (Convert.ToDateTime(dtr["WSTime"].ToString()).Minute < 10) ? "0" + Convert.ToDateTime(dtr["WSTime"].ToString()).Minute.ToString() : Convert.ToDateTime(dtr["WSTime"].ToString()).Minute.ToString();
                                WSTime = hour + ":" + minute;
                            }
                            bWC = dtr["bWC"].ToString();
                            if (dtr["WCTime"].ToString() != "") //퇴근시간
                            {
                                string hour = Convert.ToDateTime(dtr["WCTime"].ToString()).Hour.ToString();
                                string minute = (Convert.ToDateTime(dtr["WCTime"].ToString()).Minute < 10) ? "0" + Convert.ToDateTime(dtr["WCTime"].ToString()).Minute.ToString() : Convert.ToDateTime(dtr["WCTime"].ToString()).Minute.ToString();
                                WCTime = hour + ":" + minute;

                                WCTimeHour = hour;
                                WCTimeMinute = minute;
                            }
                        }
                    }
                }

                // 휴일 여부
                ArrayList getCal = GetCalendarHoli(DateTime.Now.ToString("yyyy"));
                DateTime sDate = DateTime.Parse(SelDate);

                if (getCal.Contains(SelDate) || sDate.DayOfWeek == DayOfWeek.Saturday || sDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    isHoli = "1";
                }
                else
                {
                    isHoli = "0";
                }

                // 오늘 정규 출, 퇴근시간 가져오기
                DataTableReader dtrT = dal.GetAttendanceTime(CompanyID.ToNumber<int>(), UserID.ToNumber<int>(), SelDate, isHoli);
                if (dtrT != null)
                {
                    if (dtrT.HasRows)
                    {
                        while (dtrT.Read())
                        {
                            if (dtrT["bWSTime"].ToString() != "") //출근시간
                            {
                                bWSTime = Convert.ToDateTime(dtrT["bWSTime"]);
                                bWSTimeHour = bWSTime.Hour.ToString();
                                bWSTimeMinute = (bWSTime.Minute < 10) ? "0" + bWSTime.Minute.ToString() : bWSTime.Minute.ToString();
                            }

                            if (dtrT["bWCTime"].ToString() != "") //퇴근시간
                            {
                                bWCTime = Convert.ToDateTime(dtrT["bWCTime"]);
                                bWCTimeHour = bWCTime.Hour.ToString();
                                bWCTimeMinute = (bWCTime.Minute < 10) ? "0" + bWCTime.Minute.ToString() : bWCTime.Minute.ToString();
                            }
                        }
                    }
                }

                AttendList += "<li class=\"title\">" + resMng.GetString("Menu_Attendance") + "</li>";
                AttendList += "<li class=\"atten-bt\">";
                AttendList += "<div>";
                AttendList += "<button type=\"button\" class=\"btn-lg btn-primary br-white\" style=\"width:40%\" onclick=\"Portal.GotoWork()\" id=\"GotoWork\">" + resMng.GetString("Attendance_GotoWork") + "</button>";
                AttendList += "&nbsp;<button type=\"button\" class=\"btn-lg btn-danger br-white\" style=\"width:40%\" onclick=\"Portal.OffWork()\" id=\"HROffWork\">" + resMng.GetString("Attendance_OffWork") + "</button>";
                AttendList += "</div>";
                AttendList += "</li>";
                AttendList += "<li class=\"date\">";
                AttendList += "<div>";
                AttendList += "▶&nbsp;" + resMng.GetString("Attendance_GotoWork") + "&nbsp;: <span id=\"attendanceGotoWork\"></span><br />";
                AttendList += "<span id=\"spOffWork\" style=\"display: none;\">▶&nbsp;" + resMng.GetString("Attendance_OffWork") + "&nbsp;: <span id=\"attendanceOffWork\"></span></span>";
                AttendList += "<span id=\"trEarlyTime\" style=\"display: none;\">▶&nbsp;" + resMng.GetString("Attendance_Early") + "&nbsp;: <span id=\"spEarlyTime\"></span></span>";
                AttendList += "</div>";
                AttendList += "</li>";                
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAttendList()", ex);
                throw;
            }
        }

        /// <summary>
        /// 공지 게시판 리스트
        /// </summary>
        private void GetBoardList()
        {
            try
            {
                int boardComID = Board.Ws.BLL.Category.GetCompanyCategoryInfo(NoticeCID.ToNumber<int>());
                Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category bll = new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category();
                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.CategoryInfo be = bll.GetCategoryOptionEtc(boardComID, Convert.ToInt32(NoticeCID), Convert.ToInt32(UserID), UserInfo.DeptID);

                if (be.Auth != "")
                {
                    Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List listBiz = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();

                    string isAuthority = "";
                    string targetCategory = "";
                    //보기용이라면~
                    if (be.OptionUseView.Equals("1"))
                    {
                        isAuthority = bll.GetCategoryAuthority(boardComID, NoticeCID.ToNumber<int>(), UserID.ToNumber<int>(), UserInfo.DeptID, "1", false);
                        //해당 카테고리 게시판을 볼 수 있는지 확인
                        if (isAuthority != "")
                        {
                            targetCategory = NoticeCID;
                        }
                    }
                    else
                    {
                        //일반 카테고리라면..
                        isAuthority = bll.GetCategoryAuthority(boardComID, NoticeCID.ToNumber<int>(), UserID.ToNumber<int>(), UserInfo.DeptID, "1", false);
                        if (isAuthority != "") targetCategory = NoticeCID;
                    }

                    DataSet dsList = listBiz.GetBoardList(boardComID, targetCategory, "1", "2", be.OptionUseDept, UserInfo.DeptID.ToString(), UserID.ToNumber<int>(), "");

                    if (dsList != null)
                    {
                        string tmpCatName = string.Empty;
                        string tmpDate = string.Empty;

                        tmpCatName = this.GetLangName(
                            be.NameBase.ToNull(), be.NameKo.ToNull(),
                            be.NameEn.ToNull(), be.NameJa.ToNull(),
                            be.NameFr.ToNull(), be.NameZhcn.ToNull(),
                            be.NameZhtw.ToNull(), PreLang);

                        NoticeBoardList += "<li class='title'>" + tmpCatName + "</li>";

                        foreach (DataRow dr in dsList.Tables[0].Rows)
                        {
                            DateTime dt;
                            tmpDate = string.Empty;
                            if (DateTime.TryParse(dr["WriteDate"].ToString(), out dt))
                                tmpDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                tmpDate = string.Empty;

                            NoticeBoardList += "<li><a style='cursor:pointer;' onclick=\"Portal.viewBoard('" + dr["CategoryID"].ToString() + "','" + dr["ListID"].ToString() + "')\">" + dr["Title"].ToString() + "</a><span class='date'>" + tmpDate + "</span></li>";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBoardList()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-11-23
        /// 작성목적 : regID 값이 없으면 알람을 받지 못한다는 문구처리
        /// </summary>
        private void SetMsgRegID()
        {
            // 안드로이드 App으로 접속했는데 regID 값을 받지 못할 경우
            if ((UUID != "") && (MobileType == "Android") && (RegID == ""))
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_MDM_List");
                sql = string.Format(sql, CompanyID.ToString(), UserID, UUID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsEnvInfo", null);

                string msgYN = "N";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["MailAlarm"].ToString() == "Y")
                    {
                        //msgYN = "Y";
                    }

                    if (dr["EtcAlarm"].ToString() != "")
                    {
                        //msgYN = "Y";
                    }
                }

                if (msgYN == "Y")
                {
                    //msg.Visible = true;
                }
                else
                {
                    //msg.Visible = false;
                }

            }
            else
            {
                //msg.Visible = false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadAuth()
        {
            mpMail.Visible = false;
            mpPims.Visible = false;
            mpNotice.Visible = false;
            mpApproval.Visible = false;

            if (UserInfo.MenuRoles != null)
            {
                for (int i = 0; i < UserInfo.MenuRoles.Length; i++)
                {
                    if (UserInfo.MenuRoles[i] == "1")
                    {
                        mpMail.Visible = true;
                        mailCheck = "Y";
                    }

                    if (UserInfo.MenuRoles[i] == "2")
                    {
                        mpApproval.Visible = true;
                        approvalCheck = "Y";
                    }

                    if (UserInfo.MenuRoles[i] == "3")
                    {
                        mpNotice.Visible = true;
                        noticeCheck = "Y";
                    }

                    if (UserInfo.MenuRoles[i] == "4")
                    {
                        mpPims.Visible = true;
                        pimsCheck = "Y";
                    }

                    if (UserInfo.MenuRoles[i] == "10")
                    {
                        taskCheck = "Y";
                    }

                    if (UserInfo.MenuRoles[i] == "12")
                    {
                        attendCheck = "Y";
                    }

                    
                }
            }
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2016-08-29
        /// 작성목적 : 모바일 겸직 표시
        /// </summary>
        private void LoadMoreJob()
        {
            try
            {
                string sqlQueryUserInfo = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_ACCOUNTDEPT");
                sqlQueryUserInfo = String.Format(sqlQueryUserInfo, CompanyID, UserID);

                DataSet dsUserInfo = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQueryUserInfo, "UserUserInfo", null);

                if (dsUserInfo.Tables[0].Rows.Count == 1)
                {
                    UserInfoName = UserInfo.Name + " " + UserInfo.TitleName;

                    if (UserInfoName.Length > 20)
                    {
                        UserInfoName = UserInfoName.Substring(0, 18) + "..";
                    }

                    spanUserInfo.Visible = true;
                    selUserInfo.Visible = false;
                }
                else
                {
                    ddlUserInfo.Items.Clear();

                    string itemText = "", itemValue = "";
                    string selVal = "";

                    foreach (DataRow dr in dsUserInfo.Tables[0].Rows)
                    {
                        //itemText = dr["DeptBase"].ToString() + " " + dr["NameBase"].ToString() + " " + dr["TitleBase"].ToString();

                        string tmpDept = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);

                        itemText = (tmpDept.Length > 20) ? tmpDept.Substring(0, 20) + ".." : tmpDept;

                        itemValue = dr["CompanyID"].ToString() + "~" + dr["UserID"].ToString() + "~" + dr["DeptID"].ToString() + "~" + dr["CodeTitleID"].ToString() + "~" + dr["CodeDutyID"].ToString() + "~" + dr["LogInID"].ToString() + "~" + dr["Password"].ToString();

                        ddlUserInfo.Items.Add(new ListItem(itemText, itemValue));

                        if (dr["BaseDept"].ToString() == "1")
                        {
                            selVal = itemValue;
                        }
                    }

                    ddlUserInfo.SelectedValue = selVal;
                    ddlUserInfo.Visible = true;
                    spanUserInfo.Visible = false;
                    selUserInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoadMoreJob()", ex);
                ddlUserInfo.Visible = false;
            }
        }


        private void LoadUser()
        {
            Account.Data.User dal = new Account.Data.User();
            Account.BE.UserInfo info = dal.GetUser(CompanyID.ToNumber<int>(), UserID.ToNumber<int>());

            string photoName = info.PhotoImage;
            string BaseImgPath = "";
            string imgTag = "";
            string encParams = "";

            Dictionary<string, string> paraDic = null;
            string filePath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");

            if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
            {
                BaseImgPath = filePath + "/{0}/Users/{1}/Picture/" + photoName;
                BaseImgPath = string.Format(BaseImgPath, CompanyID, info.UserID);
            }
            else
            {
                BaseImgPath = "/DeskPlusMobile/Content/ko/Base/Img/DStyle/myimgdefault.gif";
            }

            if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
            {
                #region 스크린샷
                paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", BaseImgPath);
                encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                BaseImgPath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;
                #endregion

                imgTag = "<img src='{0}' />";
                imgTag = string.Format(imgTag, BaseImgPath);
            }

            PhotoImage = imgTag;           
        }

        private void LoadPortalImage()
        {
            string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "BaseFileDirectory");
            string ciBGPortalImage = baseFileDirectory + UserInfo.CompanyID + "/mainback.jpg";

            //회사별 이미지가 있다면?
            if (System.IO.File.Exists(Server.MapPath(ciBGPortalImage)))
            {
                BGPortalImage = ciBGPortalImage + "?t=" + System.Guid.NewGuid().ToString();
            }
            else
            {
                //회사별 이미지가 없다면, 기본 이미지 
                BGPortalImage = "/DeskPlusEIP/Design/C/Img/DStyle/mainback1.jpg";
            }
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 암호화 문자열 반환하기
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                return Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        public ArrayList GetCalendarHoli(string _year)
        {
            string CacheName = "HolidayList_";

            HttpContext context = HttpContext.Current;

            List<object> listobj2 = new List<object>();

            if (context.Cache[CacheName + CompanyID] == null)
            {
                SetCacheHolidayList(CompanyID.ToNumber<int>(), _year);
            }

            listobj2 = (List<object>)context.Cache[CacheName + CompanyID];

            ArrayList Holidayobj = new ArrayList();
            string tempDate = string.Empty;

            foreach (Pims.Ws.BE.HolidayRInfo _be in listobj2)
            {
                string SolarLunar = _be.SolarLunar;                               //양음력구분
                string HOLIDAY = _be.HoliDay;                                     //일자
                string TITLE = _be.Title;                                         //제목
                string HolidayBetween = _be.HolidayBetween;                       //연휴구분
                int HolidayBetweenDay = _be.HolidayBetweenDay;                    //연휴일자
                string IsReplace = _be.IsReplace;                                 //대체휴가구분
                int IsReplaceDay = 0;                                             //대체휴가일자
                string IsHolidayKind = _be.IsHolidayKind;                         //공휴일구분    

                DateTime sDate = DateTime.Parse(HOLIDAY);

                //양력음력구분
                if (SolarLunar != "0")
                {
                    CalendarHelper calHelper = new CalendarHelper();
                    sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                }

                //대체휴일 확인
                if (IsReplace == "1")
                {
                    for (int i = 0; i <= HolidayBetweenDay; i++)
                    {
                        if (HolidayBetween == "1" || HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                if (i > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }

                        if (HolidayBetween == "2" || HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }
                    }
                }

                //대체휴일 + 토 확인
                if (IsReplace == "2")
                {
                    for (int i = 0; i <= HolidayBetweenDay; i++)
                    {
                        if (HolidayBetween == "0")
                        {
                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }
                            else if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }

                        if (HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }
                    }
                }

                //일자 저장
                Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                //비교 데이터 저장
                if (IsHolidayKind == "0")
                {
                    tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                }

                if (HolidayBetween == "1")
                {
                    //이전일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                        }

                        if (IsHolidayKind == "0")
                        {
                            tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
                else if (HolidayBetween == "2")
                {
                    //다음일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        }

                        if (IsHolidayKind == "0")
                        {
                            tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
                else if (HolidayBetween == "3")
                {
                    //이전다음일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                        }

                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                        }

                        if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        }

                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }

                if (IsReplaceDay > 0)
                {
                    int IsReplaceDayStart = HolidayBetweenDay + 1;
                    int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                    for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                    {
                        Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                    }
                }
            }

            return Holidayobj;
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 휴일 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public void SetCacheHolidayList(int _companyID, string _UserSelectedYear)
        {
            try
            {
                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj = new List<object>();

                if (context.Cache[CacheName + _companyID] == null)
                {
                    SqlParameter[] parm = new SqlParameter[]{
                                new SqlParameter("@CompanyID", SqlDbType.Int)
                            ,new SqlParameter("@SelectYear", SqlDbType.Int)
                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = _UserSelectedYear.ToNumber<int>();

                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_HolidayR_SelByYear");
                    sql = string.Format(sql, _companyID, _UserSelectedYear.ToNumber<int>());
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsYear", parm);

                    foreach (DataRow dtr in ds.Tables[0].Rows)
                    {

                        Pims.Ws.BE.HolidayRInfo _data = new Pims.Ws.BE.HolidayRInfo();

                        _data.SolarLunar = dtr["SolarLunar"].ToString();                               //양음력구분
                        _data.HoliDay = dtr["HOLIDAY"].ToString();                                     //일자
                        _data.Title = dtr["TITLE"].ToString();                                         //제목
                        _data.HolidayBetween = dtr["HolidayBetween"].ToString();                       //연휴구분
                        _data.HolidayBetweenDay = dtr["HolidayBetweenDay"].ToString().ToNumber<int>(); //연휴일자
                        _data.IsReplace = dtr["IsReplace"].ToString();                                 //대체휴가구분
                        _data.IsReplaceDay = 0;                                                        //대체휴가일자
                        _data.IsHolidayKind = dtr["IsHolidayKind"].ToString();                         //공휴일구분        

                        listobj.Add(_data);
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheName + _companyID, listobj, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetCacheHolidayList : " + ex);
                throw;
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        public string UserInfoName { get; set; }
        public string Today { get; set; }
        public string MainCID { get; set; }
        public string PCatID { get; set; }
        public string mailCheck { get; set; }
        public string approvalCheck { get; set; }
        public string noticeCheck { get; set; }
        public string pimsCheck { get; set; }
        public string taskCheck { get; set; }
        public string attendCheck { get; set; }
        public string NoticeCID { get; set; }
        public string NoticeBoardList { get; set; }
        public string AttendList { get; set; }
        public string PhotoImage { get; set; }
        public string TopPortal { get; set; }
        public string BottomPortal { get; set; }

        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }
        public int TargetHour
        {
            get { return (ViewState["targetHour"] == null) ? System.DateTime.Now.Hour : Convert.ToInt32(ViewState["targetHour"].ToString()); }
            set { ViewState["targetHour"] = value; }
        }        
        public int TargetMinute
        {
            get { return (ViewState["targetMinute"] == null) ? System.DateTime.Now.Minute : Convert.ToInt32(ViewState["targetMinute"].ToString()); }
            set { ViewState["targetMinute"] = value; }
        }
        #endregion
    }
}