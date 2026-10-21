using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.HR.Web.Content
{
    public partial class ListPrivateMod : Base.BasePageHR
    {
        #region 변수설정
        public string FolderStatusID = "";
        public string DeptID = string.Empty; //DeptID^PathID^DeptBase^DeptState
        public string SearchType = string.Empty;    //버튼검색 or 선택날짜검색 구분
        public string AttendanceType = string.Empty; //근태검색 구분
        public string TotalCount = string.Empty;    //총카운트

        public string WeekStartDate = string.Empty;
        public string WeekEndDate = string.Empty;

        public string Gubun = string.Empty;
        public string Year = string.Empty;
        public string Quarter = string.Empty;
        public string Month = string.Empty;

        public string DeptState = string.Empty; //활성화부서, 비활성화부서 구분 값
        public int RootLevels = 0;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.ListPrivateMod).Assembly;	    //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }


        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 권한 체크
            if (!AccessAuth && !CurrentUserInfo.IsInRole("HRAttendance"))
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }
            #endregion

            LoadParam();

            GetDeptList();

            SetDeptInfo();

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                FolderStatusID, CurrentUserInfo.UserID, "User");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }

            #endregion

            #region admin 관리자 체크
            cbtnAdmin.Visible = (AccessAuth) ? true : false;
            #endregion
            
            ddlListCount.SelectedValue = RecordPerPage.ToString();

            GetDateType();
            GetListCount();

            ltlHeader.Text = GetResource("ListPrivateMod");
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 언어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = this.resMng.GetString("Administrator"); // 관리자
                cbtnExcel.Text = resMng.GetString("ExecelDown");        //엑셀
                cbtnSearch.Text = this.resMng.GetString("Search");      //검색 
                cbtnDown.Text = "▼" + resMng.GetString("DownView");

                ddlActiveDept.Items.Clear();
                ddlActiveDept.Items.Add(new ListItem(this.resMng.GetString("CurrentDept"), "1")); //현재부서
                ddlActiveDept.Items.Add(new ListItem(this.resMng.GetString("PreviousDept"), "0")); //이전부서

                ddlGubun.Items.Clear();
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("ThisWeek"), "Week"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Month"), "Month"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Quarter"), "Quarter"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Year"), "Year"));

                ddlAttendanceType.Items.Clear();
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("ListType0"), "0"));                //전체
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_Tardy"), "1")); //지각
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_Early"), "2")); //조퇴
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Over_Work"), "3")); //연장근무
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Holi"), "4")); //휴가

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Name"), "NameBase")); //이름
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID")); //아이디
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TitleName"), "TitleBase")); //직급

                NowYear = DateTime.Now.Year.ToString();
                NowMonth = DateTime.Now.Month.ToString();

                ddlMonth.SelectedValue = NowMonth;

                int month = NowMonth.ToNumber<int>();
                if (month >= 1 && month <= 3)
                {
                    ddlQuarter.SelectedValue = "1";
                }
                if (month >= 4 && month <= 6)
                {
                    ddlQuarter.SelectedValue = "2";
                }
                if (month >= 7 && month <= 9)
                {
                    ddlQuarter.SelectedValue = "3";
                }
                if (month >= 10 && month <= 12)
                {
                    ddlQuarter.SelectedValue = "4";
                }

                NowQuarter = ddlQuarter.SelectedValue;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            StartDate = GetDecryptParam("startDate", "");   //시작일자
            EndDate = GetDecryptParam("endDate", "");   //종료일자
            SearchType = GetDecryptParam("searchType", "");   //버튼검색 or 선택날짜검색 구분     
            Gubun = GetDecryptParam("gubun", "");
            Year = GetDecryptParam("year", System.DateTime.Now.Year.ToString());
            Quarter = GetDecryptParam("quarter", "");
            Month = GetDecryptParam("month", "");

            DeptID = GetDecryptParam("deptID", "^^^").Replace("&", "＆");    //조회 대상부서 DeptID^PathID^DeptBase^DeptState
            DeptState = GetDecryptParam("deptState", "1"); //활성화부서, 비활성화부서 구분 값
            AttendanceType = GetDecryptParam("attendanceType", "");   //근태검색 구분

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlActiveDept.SelectedValue = DeptState;
            ddlAttendanceType.SelectedValue = AttendanceType;

            if (Gubun != "")
            {
                ddlGubun.SelectedValue = Gubun;
            }
            if (Quarter != "")
            {
                ddlQuarter.SelectedValue = Quarter;
            }
            if (Month != "")
            {
                ddlMonth.SelectedValue = Month;
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-11-27
        /// 작성목적 : 권한부서 가져오기
        /// </summary>
        private void GetDeptList()
        {
            DAL.AttendanceRole role = new DAL.AttendanceRole();
            DataSet ds;
            if (ddlActiveDept.SelectedValue == "1")
            {
                ds = role.GetActiveDept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "dsDept");
            }
            else
            {
                ds = role.GetDept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "dsDept");
            }

            ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["DeptID"], ds.Tables[0].Columns["ParentDeptID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow.IsNull("ParentDeptID"))
                {
                    string userYN = "N";

                    if (ddlActiveDept.SelectedValue == "1")
                    {
                        Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                        var userList = dal.GetUserListByDept(CurrentUserInfo.CompanyID, dbRow["DeptID"].ToString(), true);

                        if (userList.Count > 0)
                        {
                            userYN = "Y";
                        }
                    }
                    else
                    {
                        /*
                        DAL.Closing dal = new DAL.Closing();
                        DataSet dsUser = dal.GetUserListByDept(CurrentUserInfo.CompanyID, dbRow["DeptID"].ToString().ToNumber<int>(), dbRow["PathID"].ToString(), dbRow["NameBase"].ToString());

                        foreach (DataRow row in dsUser.Tables[0].Rows)
                        {
                            if (row["Cnt"].ToString().ToNumber<int>() > 0)
                            {
                                userYN = "Y";
                            }
                        }
                        */

                        // 속도 문제로 사용자가 없는 부서도 바인딩
                        userYN = "Y";
                    }

                    // 사용자가 있는 부서만 바인딩
                    if (userYN == "Y")
                    {
                        string deptName = this.GetLangName(
                        dbRow["NameBase"].ToString(),
                        dbRow["NameKo"].ToString(), dbRow["NameEn"].ToString(), dbRow["NameJa"].ToString(),
                        dbRow["NameFr"].ToString(), dbRow["NameZhCn"].ToString(), dbRow["NameZhTw"].ToString(), PreLang).ToReverseXSS().Replace("&", "＆");

                        if (PreLang == "ko")
                        {
                            deptName = dbRow["NameBase"].ToString();
                        }

                        RootLevels = dbRow["Levels"].ToString().ToNumber<int>();

                        if (ddlActiveDept.SelectedValue == "1")
                        {
                            if (dbRow["DeptState"].ToString() == "1")
                            {
                                if (!ddlDeptList.Items.Contains(new ListItem(deptName, dbRow["DeptID"].ToString() + "^" + dbRow["PathID"].ToString() + "^" + dbRow["NameBase"].ToString().Replace("&", "＆") + "^" + dbRow["DeptState"].ToString())))
                                {
                                    ddlDeptList.Items.Add(new ListItem(deptName, dbRow["DeptID"].ToString() + "^" + dbRow["PathID"].ToString() + "^" + dbRow["NameBase"].ToString().Replace("&", "＆") + "^" + dbRow["DeptState"].ToString()));
                                }
                            }

                            //하위 바인딩(datarow)
                            PopulateSubNode(dbRow);
                        }
                        else
                        {
                            if (dbRow["DeptState"].ToString() != "1")
                            {
                                string PathNameBase = dbRow["PathNameBase"].ToString().Substring(1, (dbRow["PathNameBase"].ToString().Length - 1)).Replace("&", "＆");
                                if (!ddlDeptList.Items.Contains(new ListItem(PathNameBase, dbRow["DeptID"].ToString() + "^" + dbRow["PathID"].ToString() + "^" + dbRow["NameBase"].ToString().Replace("&", "＆") + "^" + dbRow["DeptState"].ToString())))
                                {
                                    ddlDeptList.Items.Add(new ListItem(PathNameBase, dbRow["DeptID"].ToString() + "^" + dbRow["PathID"].ToString() + "^" + dbRow["NameBase"].ToString().Replace("&", "＆") + "^" + dbRow["DeptState"].ToString()));
                                }
                            }

                            //하위 바인딩(datarow)
                            PopulateSubNode(dbRow);
                        }
                    }
                    else
                    {
                        //하위 바인딩(datarow)
                        PopulateSubNode(dbRow);
                    }
                    
                }
            }

            // 이전부서일때
            if (ddlActiveDept.SelectedValue == "0")
            {
                foreach (ListItem item in ddlDeptList.Items)
                {
                    item.Attributes.Add("style", "color:red");
                }
            }

            if (DeptID != "" && DeptID != "^^^")
            {
                ddlDeptList.SelectedValue = DeptID;
            }

            // 부서 상태값 설정
            SetStateDept();
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2019-01-15
        /// 작성목적 : 부서 상태값 설정
        /// </summary>
        private void SetStateDept()
        {
            string selDate = string.Empty;

            if (ddlDeptList.SelectedValue == "") return;

            int deptID = ddlDeptList.SelectedValue.Split('^')[0].ToNumber<int>();
            string pathID = ddlDeptList.SelectedValue.Split('^')[1];
            string nameBase = ddlDeptList.SelectedValue.Split('^')[2];
            string DeptState = ddlDeptList.SelectedValue.Split('^')[3]; // -2, -1: 삭제된부서, 0: 부서명변경된부서, 1: 현재부서

            ltState.Text = "";

            // 조회기간 가져오기
            selDate = GetSelDate(deptID, pathID, nameBase, DeptState);

            if (ddlDeptList.SelectedValue.Split('^')[3].ToNumber<int>() < 0) // 삭제된 부서
            {
                ltState.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("State") + "</span>: " + this.resMng.GetString("Del") + " / <span style='font-weight:bold'>" + this.resMng.GetString("SelDate") + "</span>: " + selDate + "</span>";
            }
            else if (ddlDeptList.SelectedValue.Split('^')[3].ToNumber<int>() == 0) // 부서명이 변경된 부서
            {
                Account.DAL.Dept dal = new Account.DAL.Dept();
                List<Account.BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, ddlDeptList.SelectedValue.Split('^')[0], true);

                string name = string.Empty;
                foreach (Account.BE.DeptInfo deptInfo in al)
                {
                    name = deptInfo.NameBase;
                }

                ltState.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("State") + "</span>: " + this.resMng.GetString("DeptNameChange") + " / <span style='font-weight:bold'>" + this.resMng.GetString("Now") + "</span>: " + name + " / <span style='font-weight:bold'>" + this.resMng.GetString("SelDate") + "</span>: " + selDate + "</span>";
            }
            else
            {
                string closingDeptName = "";

                DAL.Closing dal = new DAL.Closing();
                DataSet ds = dal.GetClosingDeptInfo(CurrentUserInfo.CompanyID, deptID, pathID);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (nameBase.Replace("＆", "&") != dr["NameBase"].ToString())
                    {
                        closingDeptName = dr["NameBase"].ToString();
                    }
                }

                // 부서명이 바뀐 부서이면 이전부서명 표시
                if (closingDeptName != "")
                {
                    ltState.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("State") + "</span>: " + this.resMng.GetString("DeptNameChange") + " / <span style='font-weight:bold'>" + this.resMng.GetString("Previous") + "</span>: " + closingDeptName + " / <span style='font-weight:bold'>" + this.resMng.GetString("SelDate") + "</span>: " + selDate + "</span>";
                }
                else
                {
                    ltState.Text = "<span style='vertical-align: middle;'><span style='font-weight:bold'>" + this.resMng.GetString("SelDate") + "</span>: " + selDate + "</span>";
                }
            }
        }

        /// <summary>
        /// 부서 조회기간 가져오기
        /// </summary>
        /// <param name="_deptID"></param>
        /// <param name="_pathID"></param>
        /// <param name="_nameBase"></param>
        private string GetSelDate(int _deptID, string _pathID, string _nameBase, string _active)
        {
            string minDate = string.Empty;
            string maxDate = string.Empty;

            DAL.Closing dal = new DAL.Closing();
            DataSet ds = dal.GetClosingDate(CurrentUserInfo.CompanyID, 0, _deptID, _pathID, _nameBase);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow[0].ToString() != "")
                {
                    minDate = dbRow[0].ToString().Split('^')[0];
                    maxDate = dbRow[0].ToString().Split('^')[1];
                }
            }

            string selDate = string.Empty;

            if (_active == "1")
            {
                if (minDate == "") // 백업된 데이터에 없으면 이번주로 표시
                {
                    selDate = SetDate(GetDateOfMonday(DateTime.Now).ToString("yyyy-MM-dd"));
                    selDate += "~";
                    selDate += SetDate(GetDateOfSunday(DateTime.Now).ToString("yyyy-MM-dd"));
                }
                else
                {
                    DateTime startDate = Convert.ToDateTime(minDate);
                    selDate = SetDate(GetDateOfMonday(startDate).ToString("yyyy-MM-dd"));

                    selDate += "~";

                    string userYN = "N";
                    Zeospace.DeskPlus.Account.DAL.User uDal = new Account.DAL.User();
                    var userList = uDal.GetUserListByDept(CurrentUserInfo.CompanyID, _deptID.ToString(), true);

                    if (userList.Count > 0)
                    {
                        userYN = "Y";
                    }

                    if (userYN == "Y") // 현재 부서에 유저가 있으면 이번주까지 표시
                    {
                        selDate += SetDate(GetDateOfSunday(DateTime.Now).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        DateTime endDate = Convert.ToDateTime(maxDate);
                        selDate += SetDate(GetDateOfSunday(endDate).ToString("yyyy-MM-dd"));
                    }
                }
            }
            else
            {
                if (minDate != "")
                {
                    DateTime startDate = Convert.ToDateTime(minDate);
                    selDate = SetDate(GetDateOfMonday(startDate).ToString("yyyy-MM-dd"));

                    selDate += "~";

                    DateTime endDate = Convert.ToDateTime(maxDate);
                    selDate += SetDate(GetDateOfSunday(endDate).ToString("yyyy-MM-dd"));
                }
            }

            return selDate;
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-11-27
        /// 작성목적 : 하위 바인딩
        /// </summary>
        private void PopulateSubNode(DataRow _dbRow)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("DeptRelation"))
            {
                string userYN = "N";

                if (ddlActiveDept.SelectedValue == "1")
                {
                    Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                    var userList = dal.GetUserListByDept(CurrentUserInfo.CompanyID, childRow["DeptID"].ToString(), true);

                    if (userList.Count > 0)
                    {
                        userYN = "Y";
                    }
                }
                else
                {
                    /*
                    DAL.Closing dal = new DAL.Closing();
                    DataSet dsUser = dal.GetUserListByDept(CurrentUserInfo.CompanyID, childRow["DeptID"].ToString().ToNumber<int>(), childRow["PathID"].ToString(), childRow["NameBase"].ToString());

                    foreach (DataRow row in dsUser.Tables[0].Rows)
                    {
                        if (row["Cnt"].ToString().ToNumber<int>() > 0)
                        {
                            userYN = "Y";
                        }
                    }
                    */

                    // 속도 문제로 사용자가 없는 부서도 바인딩
                    userYN = "Y";
                }

                // 사용자가 있는 부서만 바인딩
                if (userYN == "Y")
                {
                    string deptName = this.GetLangName(
                        childRow["NameBase"].ToString(),
                        childRow["NameKo"].ToString(), childRow["NameEn"].ToString(), childRow["NameJa"].ToString(),
                        childRow["NameFr"].ToString(), childRow["NameZhCn"].ToString(), childRow["NameZhTw"].ToString(), PreLang).ToReverseXSS().Replace("&", "＆");

                    if (PreLang == "ko")
                    {
                        deptName = childRow["NameBase"].ToString();
                    }

                    if (ddlActiveDept.SelectedValue == "1")
                    {
                        if (childRow["DeptState"].ToString() == "1")
                        {
                            string gap = "";
                            for (int i = 0; i < childRow["Levels"].ToString().ToNumber<int>() - RootLevels; i++)
                            {
                                gap = gap + "　";
                            }

                            deptName = gap + deptName;

                            if (!ddlDeptList.Items.Contains(new ListItem(deptName, childRow["DeptID"].ToString() + "^" + childRow["PathID"].ToString() + "^" + childRow["NameBase"].ToString().Replace("&", "＆") + "^" + childRow["DeptState"].ToString())))
                            {
                                ddlDeptList.Items.Add(new ListItem(deptName, childRow["DeptID"].ToString() + "^" + childRow["PathID"].ToString() + "^" + childRow["NameBase"].ToString().Replace("&", "＆") + "^" + childRow["DeptState"].ToString()));
                            }
                        }

                        //하위 바인딩(datarow)
                        PopulateSubNode(childRow);
                    }
                    else
                    {
                        if (childRow["DeptState"].ToString() != "1")
                        {
                            string PathNameBase = childRow["PathNameBase"].ToString().Substring(1, (childRow["PathNameBase"].ToString().Length - 1)).Replace("&", "＆");
                            if (!ddlDeptList.Items.Contains(new ListItem(PathNameBase, childRow["DeptID"].ToString() + "^" + childRow["PathID"].ToString() + "^" + childRow["NameBase"].ToString().Replace("&", "＆") + "^" + childRow["DeptState"].ToString())))
                            {
                                ddlDeptList.Items.Add(new ListItem(PathNameBase, childRow["DeptID"].ToString() + "^" + childRow["PathID"].ToString() + "^" + childRow["NameBase"].ToString().Replace("&", "＆") + "^" + childRow["DeptState"].ToString()));
                            }
                        }

                        //하위 바인딩(datarow)
                        PopulateSubNode(childRow);
                    }
                }
                else
                {
                    //하위 바인딩(datarow)
                    PopulateSubNode(childRow);
                }
                
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 
        /// </summary>
        private void SetDeptInfo()
        {
            DeptID = ddlDeptList.SelectedValue;
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 총 카운트
        /// </summary>
        private void GetListCount()
        {
            if (DeptID == "" || DeptID == "^^^") return;

            #region 백업 ID 가져오기

            int minclosingID = -1;
            int maxclosingID = -1;

            DAL.Closing dal = new DAL.Closing();
            List<BE.ClosingInfo> startlist = dal.GetClosing(CurrentUserInfo.CompanyID, StartDate, "W");

            foreach (ClosingInfo info in startlist)
            {
                minclosingID = info.ClosingID;
            }

            // 백업 ID가 없으면 검색 날짜가 이번주 이상인지 확인
            if (minclosingID < 1)
            {
                string thisDate = GetDateOfMonday(DateTime.Now).ToString("yyyy-MM-dd");

                DateTime T1 = Convert.ToDateTime(StartDate);
                DateTime T2 = Convert.ToDateTime(thisDate);

                int result = DateTime.Compare(T1, T2);

                // T1이 더 크면 endDate가 이번주 이상이다.
                if (result >= 0)
                {
                    minclosingID = -1;
                }
                else
                {
                    minclosingID = 0; // 백업되기 전의 기간은 minclosingID를 0으로 처리
                }
            }

            List<BE.ClosingInfo> endlist = dal.GetClosing(CurrentUserInfo.CompanyID, EndDate, "W");

            foreach (ClosingInfo info in endlist)
            {
                maxclosingID = info.ClosingID;
            }

            // 백업 ID가 없으면 검색 날짜가 이번주 이상인지 확인
            if (maxclosingID < 1)
            {
                string thisDate = GetDateOfMonday(DateTime.Now).ToString("yyyy-MM-dd");

                DateTime T1 = Convert.ToDateTime(EndDate);
                DateTime T2 = Convert.ToDateTime(thisDate);

                int result = DateTime.Compare(T1, T2);

                // T1이 더 크면 endDate가 이번주 이상이다.
                if (result >= 0)
                {
                    maxclosingID = -1;
                }
                else
                {
                    maxclosingID = 0; // 백업되기 전의 기간은 maxclosingID를 0으로 처리
                }
            }

            #endregion

            DAL.Attendance attdal = new DAL.Attendance();
            TotalCount = attdal.GetAttendanceListCount(CurrentUserInfo.CompanyID, DeptID.Split('^')[0].ToNumber<int>(), DeptID.Split('^')[1], StartDate, EndDate, AttendanceType, SearchField, SearchValue, 1, minclosingID, maxclosingID).ToString();
        }

        /// <summary>  
        /// 작성자 : kkzz52
        /// 작성일자 : 2017-11-21
        /// 작성목적 : 날짜설정
        /// </summary>
        private void GetDateType()
        {
            if (StartDate == "" && EndDate == "")
            {
                DateTime today = DateTime.Now;
                StartDate = SetDate(GetDateOfMonday(today).ToString("yyyy-MM-dd")); // 특정주의 월요일
                EndDate = SetDate(GetDateOfSunday(today).ToString("yyyy-MM-dd")); // 특정주의 일요일
            }
            else
            {
                StartDate = SetDate(StartDate);
                EndDate = SetDate(EndDate);
            }

            // 주간
            DateTime sourceDate = DateTime.Now;
            WeekStartDate = SetDate(GetDateOfMonday(sourceDate).ToString("yyyy-MM-dd")); // 특정주의 월요일
            WeekEndDate = SetDate(GetDateOfSunday(sourceDate).ToString("yyyy-MM-dd")); // 특정주의 일요일
        }

        /// <summary>
        /// 특정주의 월요일
        /// </summary>
        /// <param name="sourceDate"></param>
        /// <returns></returns>
        public DateTime GetDateOfMonday(DateTime sourceDate)
        {
            int today = (int)sourceDate.DayOfWeek;
            return sourceDate.AddDays((double)((today == 0) ? -6 : 1 - today));
        }

        /// <summary>
        /// 특정주의 일요일
        /// </summary>
        /// <param name="sourceDate"></param>
        /// <returns></returns>
        public DateTime GetDateOfSunday(DateTime sourceDate)
        {
            int today = (int)sourceDate.DayOfWeek;
            return sourceDate.AddDays((double)((today == 0) ? 0 : 7 - today));
        }

        /// <summary>
        /// 날짜 변환
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string SetDate(string _date)
        {
            string tmpVal = String.Empty;
            DateTime dt = Convert.ToDateTime(_date);
            switch (CurrentUserInfo.PreferredLanguage)
            {
                case "ko":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "en":
                    tmpVal = dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "en-gb":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "ja":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "fr":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "zh-cn":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "zh-tw":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;
            }

            return tmpVal;
        }
        #endregion

        #region (프로퍼티)

        /// <summary>
        /// 근태시작일
        /// </summary>
        public string StartDate
        {
            get { return GetViewStateString("StartDate", ""); }
            set { ViewState["StartDate"] = value; }
        }

        /// <summary>
        /// 근태종료일
        /// </summary>
        public string EndDate
        {
            get { return GetViewStateString("EndDate", ""); }
            set { ViewState["EndDate"] = value; }
        }

        /// <summary>
        /// 이번년도
        /// </summary>
        public string NowYear
        {
            get { return GetViewStateString("NowYear", ""); }
            set { ViewState["NowYear"] = value; }
        }

        /// <summary>
        /// 이번달
        /// </summary>
        public string NowMonth
        {
            get { return GetViewStateString("NowMonth", ""); }
            set { ViewState["NowMonth"] = value; }
        }

        /// <summary>
        /// 이번분기
        /// </summary>
        public string NowQuarter
        {
            get { return GetViewStateString("NowQuarter", ""); }
            set { ViewState["NowQuarter"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 엑셀 다운로드
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        protected void lbtnexcel_Click(object sender, EventArgs e)
        {
            StartDate = hdStartDate.Value;
            EndDate = hdEndDate.Value;

            /*
            if (hdSearchType.Value == "" || hdSearchType.Value == "Week")
            {
                StartDate = GetDateOfMonday(Convert.ToDateTime(StartDate)).ToString("yyyy-MM-dd"); // 특정주의 월요일
                EndDate = GetDateOfSunday(Convert.ToDateTime(EndDate)).ToString("yyyy-MM-dd"); // 특정주의 일요일
            }
            */

            StartDate = SetDate(StartDate);
            EndDate = SetDate(EndDate);

            StringBuilder sbData = new StringBuilder();

            Zeospace.DeskPlus.Approval.DAL.Document dal = new Approval.DAL.Document();

            Zeospace.DeskPlus.HR.Web.Ws.Content.ListPrivateMod ws = new Ws.Content.ListPrivateMod();
            List<object> list = ws.GetPrivateModList(hdexcel.Value.ToString());
            this.GetResourceManager();

            sbData.Append("<tr>");
            sbData.Append("<td align=center colspan=17>" + ltlHeader.Text + "</td>");
            sbData.Append("</tr>");
            sbData.Append("<tr>");
            sbData.Append("<td colspan=17>" + ddlDeptList.SelectedItem.Text.Replace("　", "") + "</td>");
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center rowspan=2 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Name") + "</td>"); //이름
            sbData.Append("<td align=center rowspan=2 style='background: #eaeaea; font-weight: bold;'>" + GetResource("TitleName2") + "</td>"); //직급
            sbData.Append("<td align=center rowspan=2 style='background: #eaeaea; font-weight: bold;'>" + GetResource("CalcDate") + "</td>"); //일자
            sbData.Append("<td align=center colspan=6 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Normal_Work") + "</td>"); //기본근무
            sbData.Append("<td align=center colspan=2 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Over_Work") + "</td>");  //연장근무
            sbData.Append("<td align=center colspan=5 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Holi_HR") + "</td>"); //근태정보
            sbData.Append("<td align=center rowspan=2 style='background: #eaeaea; font-weight: bold;'>" + GetResource("Reason") + "</td>"); //비고(사유)
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("WS_Reg") + "</td>");    //출근등록            
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Location") + "</td>");   //위치 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("WC_Reg") + "</td>");    //퇴근등록 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Location") + "</td>");    //위치 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("StandardReg") + "</td>"); //등록기준 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("WorkTime") + "</td>");    //근무시간
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("WorkTime") + "</td>");    //근무시간
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("DocumentID") + "</td>");    //결재승인 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Attendance_Tardy") + "</td>");    //지각 
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Attendance_Early") + "</td>");    //조퇴
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Holi") + "</td>");    //휴가
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("Etc") + "</td>");    //기타
            sbData.Append("<td align=center style='background: #eaeaea; font-weight: bold;'>" + GetResource("AllWorkTime") + "</td>");    //총근무시간
            sbData.Append("</tr>");

            foreach (BE.AttendanceInfo ListPrivate in list)
            {
                sbData.Append("<tr>");

                if (ListPrivate.Sunday == "1")
                {
                    sbData.Append("<td align='center'>");
                    sbData.Append(ListPrivate.UserName);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center'>");
                    sbData.Append(ListPrivate.TitleName);
                    sbData.Append("</td>");

                    sbData.Append("<td align='center'>");
                    if (ListPrivate.bSysHoli == "1")
                    {
                        sbData.Append("<FONT COLOR='red'>" + ListPrivate.CalcDate + ListPrivate.DayWeek + "</FONT>");
                    }
                    else
                    {
                        sbData.Append(ListPrivate.CalcDate + ListPrivate.DayWeek);
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bLate == "1")
                    {
                        sbData.Append("<span style='color: Red'>" + ListPrivate.WSTime + "</span>");
                    }
                    else
                    {
                        sbData.Append(ListPrivate.WSTime);
                    }
                    sbData.Append("</td>");

                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3;'>" + ListPrivate.WSConn + "</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bEarly == "1")
                    {
                        sbData.Append("<span style='color: Red'>" + ListPrivate.WCTime + "</span>");
                    }
                    else
                    {
                        sbData.Append(ListPrivate.WCTime);
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3;'>" + ListPrivate.WCConn + "</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3;'>" + ListPrivate.TimeGap + "</td>");
                    sbData.Append("<td align='center' style='mso-number-format: \\@'>" + ListPrivate.WorkTime + "</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bOver == "1")
                    {
                        sbData.Append(ListPrivate.OverTime);
                    }
                    else
                    {
                        sbData.Append("&nbsp;");
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center'>");
                    if (ListPrivate.bOver == "1")
                    {
                        if (ListPrivate.PRDocumentID.ToNumber<int>() > 0)
                        {
                            List<Zeospace.DeskPlus.Approval.BE.DocumentInfo> doclist = dal.GetDocList(CurrentUserInfo.CompanyID, ListPrivate.PRDocumentID.ToNumber<int>());
                            foreach (Zeospace.DeskPlus.Approval.BE.DocumentInfo info in doclist)
                            {
                                sbData.Append(info.DocNumber);
                            }
                        }
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bLate == "1")
                    {
                        sbData.Append("<span style='color: Red'>" + ListPrivate.bLateMinute + "</span>");
                    }
                    else
                    {
                        sbData.Append("&nbsp;");
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bEarly == "1")
                    {
                        sbData.Append("<span style='color: Red'>" + ListPrivate.bEarlyMinute + "</span>");
                    }
                    else
                    {
                        sbData.Append("&nbsp;");
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.bHoli + "</td>");
                    sbData.Append("<td align='center' style='border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.bEtc + "</td>");
                    sbData.Append("<td align='center' style='mso-number-format: \\@'>" + ListPrivate.AllWorkTime + "</td>");
                    sbData.Append("<td>");
                    if (ListPrivate.bHoliAll == "1") sbData.Append(ListPrivate.HoliGubun + ": " + ListPrivate.HoliGubun2);
                    if (ListPrivate.bLate == "1") sbData.Append(ListPrivate.bLateGubun + ": " + ListPrivate.LateRemark);
                    if (ListPrivate.bEarly == "1") sbData.Append(ListPrivate.bEarlyGubun + ": " + ListPrivate.EarlyRemark);
                    if (ListPrivate.bOver == "1") sbData.Append(ListPrivate.bOverGubun + ": " + ListPrivate.OverRemark);
                    sbData.Append("</td>");
                }
                else
                {
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3;'>");
                    sbData.Append(ListPrivate.UserName);
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3;'>");
                    sbData.Append(ListPrivate.TitleName);
                    sbData.Append("</td>");

                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3;'>");
                    if (ListPrivate.bSysHoli == "1")
                    {
                        sbData.Append("<FONT COLOR='red'>" + ListPrivate.CalcDate + ListPrivate.DayWeek + "</FONT>");
                    }
                    else
                    {
                        sbData.Append(ListPrivate.CalcDate + ListPrivate.DayWeek);
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bLate == "1")
                    {
                        sbData.Append("<span style='color: Red'>" + ListPrivate.WSTime + "</span>");
                    }
                    else
                    {
                        sbData.Append(ListPrivate.WSTime);
                    }
                    sbData.Append("</td>");

                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>" + ListPrivate.WSConn + "</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bEarly == "1")
                    {
                        sbData.Append("<span style='color: Red'>" + ListPrivate.WCTime + "</span>");
                    }
                    else
                    {
                        sbData.Append(ListPrivate.WCTime);
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>" + ListPrivate.WCConn + "</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>" + ListPrivate.TimeGap + "</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.WorkTime + "</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bOver == "1")
                    {
                        sbData.Append(ListPrivate.OverTime);
                    }
                    else
                    {
                        sbData.Append("&nbsp;");
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3;'>");
                    if (ListPrivate.bOver == "1")
                    {
                        if (ListPrivate.PRDocumentID.ToNumber<int>() > 0)
                        {
                            List<Zeospace.DeskPlus.Approval.BE.DocumentInfo> doclist = dal.GetDocList(CurrentUserInfo.CompanyID, ListPrivate.PRDocumentID.ToNumber<int>());
                            foreach (Zeospace.DeskPlus.Approval.BE.DocumentInfo info in doclist)
                            {
                                sbData.Append(info.DocNumber);
                            }
                        }
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bLate == "1")
                    {
                        sbData.Append("<span style='color: Red'>" + ListPrivate.bLateMinute + "</span>");
                    }
                    else
                    {
                        sbData.Append("&nbsp;");
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                    if (ListPrivate.bEarly == "1")
                    {
                        sbData.Append("<span style='color: Red'>" + ListPrivate.bEarlyMinute + "</span>");
                    }
                    else
                    {
                        sbData.Append("&nbsp;");
                    }
                    sbData.Append("</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.bHoli + "</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.bEtc + "</td>");
                    sbData.Append("<td align='center' style='border-bottom: 1px dotted #C3C3C3; mso-number-format: \\@'>" + ListPrivate.AllWorkTime + "</td>");
                    sbData.Append("<td style='border-bottom: 1px dotted #C3C3C3;'>");
                    if (ListPrivate.bHoliAll == "1") sbData.Append(ListPrivate.HoliGubun + ": " + ListPrivate.HoliGubun2);
                    if (ListPrivate.bLate == "1") sbData.Append(ListPrivate.bLateGubun + ": " + ListPrivate.LateRemark);
                    if (ListPrivate.bEarly == "1") sbData.Append(ListPrivate.bEarlyGubun + ": " + ListPrivate.EarlyRemark);
                    if (ListPrivate.bOver == "1") sbData.Append(ListPrivate.bOverGubun + ": " + ListPrivate.OverRemark);
                    sbData.Append("</td>");
                }

                sbData.Append("</tr>");
            }

            string strFileName = ltlHeader.Text + "_" + ddlDeptList.SelectedItem.Text.Replace("　", "") + "_" + StartDate.Replace("/", "").Replace("-", "") + "_" + EndDate.Replace("/", "").Replace("-", "") + ".xls";
            strFileName = Server.UrlEncode(strFileName.Replace(" ", "")).Replace("+", "%20");

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
            <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
            </head><body>");

            sb.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0' style='font-size: 10pt'>");
            sb.Append(sbData);
            sb.Append("</table>");

            sb.Append("</body></html>");

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(sb.ToString());
            Response.End();
        }

        #endregion
    }
}