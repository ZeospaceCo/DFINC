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
    public partial class ListCompany : Base.BasePageHR
    {
        #region 변수설정
        public string FolderStatusID;
        public int UserID = 0;
        public int DeptID = 0;
        public string SearchType = string.Empty;    //버튼검색 or 선택날짜검색 구분
        public string TotalCount = string.Empty;    //총카운트

        public string WeekStartDate = string.Empty;
        public string WeekEndDate = string.Empty;

        public string Gubun = string.Empty;
        public string Year = string.Empty;
        public string Quarter = string.Empty;
        public string Month = string.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.ListCompany).Assembly;	    //어셈블리 선언
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
            if (!AccessAuth && !CurrentUserInfo.IsInRole("HRDept"))
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }
            #endregion

            LoadParam();

            GetDeptList();

            GetDateType();

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

            ddlListCount.SelectedValue = RecordPerPage.ToString();            

            if (FolderStatusID.ToNull().Length > 0)
            {
                DAL.FolderStatus dal = new DAL.FolderStatus();

                var alList = dal.GetFolderStatusName(CurrentUserInfo.CompanyID, FolderStatusID, true);
                foreach (var listinfo in alList)
                {
                    #region 전체경로

                    string name = this.GetLangName(
                            listinfo.NameBase,
                            listinfo.NameKo,
                            listinfo.NameEn, listinfo.NameJa,
                            listinfo.NameFr, listinfo.NameZhcn,
                            listinfo.NameZhtw, PreLang);

                    ltlHeader.Text = name;

                    #endregion
                }
            }
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

                ddlGubun.Items.Clear();
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("ThisWeek"), "Week"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Month"), "Month"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Quarter"), "Quarter"));
                ddlGubun.Items.Add(new ListItem(this.resMng.GetString("Year"), "Year"));

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
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();    //조회 대상부서
            SearchType = GetDecryptParam("searchType", "");   //버튼검색 or 선택날짜검색 구분

            Gubun = GetDecryptParam("gubun", "");
            Year = GetDecryptParam("year", System.DateTime.Now.Year.ToString());
            Quarter = GetDecryptParam("quarter", "");
            Month = GetDecryptParam("month", "");
            string isIN = GetDecryptParam("isIN", "0");

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

            if (isIN == "1")
            {
                chkIsIN.Checked = true;
            }
            else
            {
                chkIsIN.Checked = false;
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-11-27
        /// 작성목적 : 권한부서 가져오기
        /// </summary>
        private void GetDeptList()
        {
            DAL.Role role = new DAL.Role();
            DataSet ds = role.GetRootDept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "dsDept");

            ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["DeptID"], ds.Tables[0].Columns["ParentDeptID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow.IsNull("ParentDeptID"))
                {
                    string deptName = this.GetLangName(
                    dbRow["NameBase"].ToString(),
                    dbRow["NameKo"].ToString(), dbRow["NameEn"].ToString(), dbRow["NameJa"].ToString(),
                    dbRow["NameFr"].ToString(), dbRow["NameZhCn"].ToString(), dbRow["NameZhTw"].ToString(), PreLang).ToReverseXSS().Replace("&", "＆");

                    if (PreLang == "ko")
                    {
                        deptName = dbRow["NameBase"].ToString();
                    }

                    ddlDeptList.Items.Add(new ListItem(deptName, dbRow["DeptID"].ToString()));

                    //하위 바인딩(datarow)
                    PopulateSubNode(dbRow);
                }
            }

            foreach (ListItem item in ddlDeptList.Items)
            {
                Account.DAL.Dept dal = new Account.DAL.Dept();
                List<Account.BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, item.Value, true);

                if (al.Count == 0)
                {
                    item.Attributes.Add("style", "color:red");
                }
            }

            if (DeptID != 0)
            {
                ddlDeptList.SelectedValue = DeptID.ToString();
            }
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
                string deptName = this.GetLangName(
                    childRow["NameBase"].ToString(),
                    childRow["NameKo"].ToString(), childRow["NameEn"].ToString(), childRow["NameJa"].ToString(),
                    childRow["NameFr"].ToString(), childRow["NameZhCn"].ToString(), childRow["NameZhTw"].ToString(), PreLang).ToReverseXSS().Replace("&", "＆");

                if (PreLang == "ko")
                {
                    deptName = childRow["NameBase"].ToString();
                }

                ddlDeptList.Items.Add(new ListItem(deptName, childRow["DeptID"].ToString()));

                //하위 바인딩(datarow)
                PopulateSubNode(childRow);
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 
        /// </summary>
        private void SetDeptInfo()
        {
            #region admin 관리자 체크
            cbtnAdmin.Visible = (AccessAuth) ? true : false;
            #endregion

            DeptID = ddlDeptList.SelectedValue.ToNumber<int>();
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

            Zeospace.DeskPlus.HR.Web.Ws.Content.ListCompany ws = new Ws.Content.ListCompany();
            List<object> list = ws.GetCompanyList(hdexcel.Value.ToString());
            this.GetResourceManager();

            sbData.Append("<tr>");
            sbData.Append("<td align=center colspan=12>" + ltlHeader.Text + "</td>");
            sbData.Append("</tr>");
            sbData.Append("<tr>");
            sbData.Append("<td colspan=12>" + ddlDeptList.SelectedItem.Text + "</td>");
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center rowspan=2 style='font-weight: bold; background: #eaeaea'>" + GetResource("DeptName") + "</td>");   //부서명
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("PersonCount") + "</td>");    //인원(명)
            sbData.Append("<td align=center colspan=2 style='font-weight: bold; background: #eaeaea'>" + GetResource("WorkTime") + "</td>"); //근무시간
            sbData.Append("<td align=center colspan=3 style='font-weight: bold; background: #eaeaea'>" + GetResource("52Time") + "</td>"); //주 52시간 기준
            sbData.Append("<td align=center colspan=4 style='font-weight: bold; background: #eaeaea'>" + GetResource("Holi_HR") + "</td>");    //근태정보
            sbData.Append("<td align=center rowspan=2 style='font-weight: bold; background: #eaeaea'>" + GetResource("HoliUserInfo") + "</td>");    //연차 사용자 정보 (휴가/반차/기타)
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("PersonCount2") + "</td>"); //(명)
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("BaseWork") + "</td>"); //기본근무
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("Over_Work") + "</td>"); //연장근무
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("StandardTime") + "</td>"); //기준시간
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("WorkSum") + "</td>"); //근무합산
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("StandardTimePer") + "</td>"); //요율
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("Attendance_Tardy") + "</td>"); //지각
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("Attendance_Early") + "</td>"); //조퇴
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("HoliType") + "</td>"); //연차
            sbData.Append("<td align=center style='font-weight: bold; background: #eaeaea'>" + GetResource("Etc") + "</td>");  //기타
            sbData.Append("</tr>");

            foreach (BE.AttendanceInfo ListCompany in list)
            {
                sbData.Append("<tr>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3;'>");
                if (ListCompany.ParentDeptName != "")
                {
                    sbData.Append("<span style='font-size: 11px'>" + ListCompany.ParentDeptName + "</span> / ");
                    if (ListCompany.DeptState == "1")
                    {
                        sbData.Append(ListCompany.DeptName);
                    }
                    else
                    {
                        sbData.Append("<span style='color: Red'>" + ListCompany.DeptName + "</span>");
                    }
                }
                else
                {
                    if (ListCompany.DeptState == "1")
                    {
                        sbData.Append(ListCompany.DeptName);
                    }
                    else
                    {
                        sbData.Append("<span style='color: Red'>" + ListCompany.DeptName + "</span>");
                    }
                }
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3;'>");
                sbData.Append(ListCompany.UserSUM);
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@' title='휴가시간: -" + ListCompany.HoliTime + "'>");
                sbData.Append(ListCompany.WorkTime);
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                sbData.Append(ListCompany.OverTime);
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                sbData.Append(ListCompany.StandardTime);
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                sbData.Append(ListCompany.AllWorkTime);
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; mso-number-format: \\@'>");
                sbData.Append(ListCompany.StandardTimePer + "%");
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>");
                if (ListCompany.bLate == "0")
                {
                    sbData.Append(ListCompany.bLate);
                }
                else
                {
                    sbData.Append("<span style='color: Red'>" + ListCompany.bLate + "</span>");
                }
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>");
                if (ListCompany.bEarly == "0")
                {
                    sbData.Append(ListCompany.bEarly);
                }
                else
                {
                    sbData.Append("<span style='color: Red'>" + ListCompany.bEarly + "</span>");
                }
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>");
                sbData.Append(ListCompany.bHoliCnt);
                sbData.Append("</td>");
                sbData.Append("<td align=center style='border-bottom: 1px dotted #C3C3C3; border-right: 1px dotted #C3C3C3;'>");
                sbData.Append(ListCompany.bEtcCnt);
                sbData.Append("</td>");
                sbData.Append("<td style='border-bottom: 1px dotted #C3C3C3;'>");
                if (ListCompany.bHoliAll == "1")
                {
                    sbData.Append(ListCompany.HoliRemark);
                }
                else
                {
                    sbData.Append("&nbsp;");
                }
                sbData.Append("</td>");
                sbData.Append("</tr>");
            }

            string strFileName = ltlHeader.Text + "_" + ddlDeptList.SelectedItem.Text + "_" + StartDate.Replace("/", "").Replace("-", "") + "_" + EndDate.Replace("/", "").Replace("-", "") + ".xls";
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