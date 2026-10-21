using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Pim.Web.GoogleCalendar.v3.CalendarIntergrator;
using Zeospace.DeskPlus.Pim.Web.GoogleCalendar.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Auth.OAuth2.Flows;
using System.Threading.Tasks;
using Google.Apis.Util.Store;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Zeospace.DeskPlus.Pim.Web.GoogleCalendar.Helper;

namespace Zeospace.DeskPlus.Pim.Web.Schedule.SyncCalendar
{
    public partial class Default : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        private string googleAccount = string.Empty;
        private string code = string.Empty;
        private string error = string.Empty;
        private string action = string.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.SyncCalendar.Default).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                LoadData();

                GetLatestSyncTime();
            }
        }

        protected void Page_Init(object s, EventArgs e)
        {
            var clear = Request["clear"];
            if (clear != null)
            {
                Session["GoogleAccount"] = null;
                Session["CalIntegrator"] = null;
                Response.Redirect(Request.Url.LocalPath, true);
            }
        }

        protected void RenderException(Exception ex)
        {
            while (ex != null)
            {
                dvLatestSyncTime.Controls.Add(new HtmlGenericControl() { InnerText = "• " + ex.Message });
                ex = ex.InnerException;
            }
        }
        #endregion

        #region Method (함수)


        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (String.IsNullOrEmpty(code) && String.IsNullOrEmpty(error) && String.IsNullOrEmpty(action))
            {
                GetGoogleAccount();
            }

            AuthGoogle();
        }

        /// <summary>
        /// 작성목적   : 구글 계정 정보 가져오기
        /// 작성자     : 
        /// </summary>
        private void GetGoogleAccount()
        {
            #region 사용자 정보 사용
            //구글 계정 가져오기
            DAL.GoogleCalendar dal = new DAL.GoogleCalendar();

            DataTableReader dtr = dal.GetGoogleAccount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ctbGoogle.Text = dtr["Google"].ToString();
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// 작성목적   : 동기화 시간 가져오기
        /// 작성자     : 
        /// </summary>
        private void GetLatestSyncTime()
        {
            #region 사용자 정보 사용
            //구글 계정 가져오기
            DAL.GoogleCalendar dal = new DAL.GoogleCalendar();

            DataTableReader dtr = dal.GetLatestSynTime(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (dtr["WriteDate"] != DBNull.Value)
                        {
                            DateTime dt = (DateTime)dtr["WriteDate"];
                            dvLatestSyncTime.Controls.Add(new HtmlGenericControl() { InnerText = dt.ToDateTimeLongString() + " [" + dtr["Cnt"].ToString() + "] " });
                        }
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 작성목적   : 구글 인증
        /// 작성자     : 
        /// </summary>
        private void AuthGoogle()
        {
            googleAccount = (!string.IsNullOrEmpty(ctbGoogle.Text)) ? ctbGoogle.Text : null;

            if (googleAccount != null)
            {
                Session["GoogleAccount"] = googleAccount;
            }
            else
            {
                this.trSelCal.Visible = false;
                this.trSelFilter.Visible = false;
                this.cbtnSignIn.Visible = true;
                this.cbtnSignOut.Visible = false;
                ctbGoogle.Text = "";
                ctbGoogle.Enabled = true;
                ddlCalendarsList.Items.Clear();
            }

            if (googleAccount != null || code != null || error != null || action != null)
            {

                CalIntegrator calInt = new CalV3Integrator();

                var result = calInt.Authorize(Request, Session["GoogleAccount"] as string);

                if (result is AuthorizationCodeWebApp.AuthResult)
                {
                    var authResult = result as AuthorizationCodeWebApp.AuthResult;

                    Response.Redirect(authResult.RedirectUri);
                }
                else if (result is string && (result as string) == "OK")
                {
                    Session["CalIntegrator"] = calInt;
                    
                    cbtnSignIn.Visible = false;
                    cbtnSignOut.Visible = true;
                    ctbGoogle.Text = Session["GoogleAccount"] as string;
                    ctbGoogle.Enabled = false;
                    this.trSelCal.Visible = true;
                    this.trSelFilter.Visible = true;
                    LoadCalendarList(); // 캘린더 선택 기능
                    LoadSearchFilter(); // 캘린더 필터 선택 기능
                }
                else if (result is Task<string>)
                {
                    DAL.GoogleCalendar dal = new DAL.GoogleCalendar();
                    dal.AddGoogleAccount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, Session["GoogleAccount"] as string);
                    //var task = result as Task<string>;
                    //권한 부여를 위해서 다시 로그인.
                    Response.Redirect(Request.Url.AbsolutePath + "?action=check");
                }
                else if (result is UserRejectException)
                {
                    //초대 거절
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UserRejectException", new Exception("사용자가 초대 거절", result as UserRejectException));
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "GoogleAccErr", GetResource("GoogleUserRejectException"), "");
                    //dvLatestSyncTime.Controls.Add(new HtmlGenericControl() { InnerText = "• UserRejectException" });
                }
                else if (result is Exception)
                {
                    // 오류 발생
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(result.ToString());
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "GoogleAccErr", GetResource("GoogleAccErr"), "");
                    //dvLatestSyncTime.Controls.Add(new HtmlGenericControl() { InnerText = "• " + GetResource("GoogleAccErr") });
                    SignOut();
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "GoogleAccErr", GetResource("GoogleAccErr"), "");
                    SignOut();
                }
            }
        }

        /// <summary>
        /// 작성목적   : 캘린더 검색 필터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadSearchFilter()
        {
            ddlSearchFilter.Items.Clear();

            ddlSearchFilter.Items.Add(new ListItem(GetResource("SEARCH1"), "0"));
            ddlSearchFilter.Items.Add(new ListItem(GetResource("SEARCH2"), "WEEK"));
            ddlSearchFilter.Items.Add(new ListItem(GetResource("SEARCH3"), "MON"));
            ddlSearchFilter.Items.Add(new ListItem(GetResource("SEARCH4"), "U"));

            ddlSearchFilter.SelectedValue = "WEEK";
        }


        /// <summary>
        /// 작성목적   : 캘린더 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadCalendarList()
        {
            CalIntegrator Integrator = Session["CalIntegrator"] as CalIntegrator;

            var result = Integrator.GetCalendarList();
            if (result is Exception)
                RenderException(result as Exception);
            else
            {
                ddlCalendarsList.Items.Clear();

                var calList = result as CalendarList;

                calList.Items.ToList().ForEach(cal =>
                {
                    ddlCalendarsList.Items.Add(new ListItem(cal.Summary, cal.Id));
                });
            }
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            code = Request["code"];
            error = Request["error"];
            action = Request["action"];
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = GetResource("Close");
                cbtnSignIn.Text = GetResource("GoogleSignIn");
                cbtnSignOut.Text = GetResource("GoogleSignOut");
                cbtnGetCalendar.Text = GetResource("GetContent");
            }
        }


        /// <summary>
        /// 작성목적   : 구글 계정 로그 아웃
        /// 작성자     : 
        /// </summary>
        private void SignOut()
        {
            Session["CalIntegrator"] = null;
            Session["GoogleAccount"] = null;
            this.trSelCal.Visible = false;
            this.trSelFilter.Visible = false;
            this.cbtnSignIn.Visible = true;
            this.cbtnSignOut.Visible = false;
            ctbGoogle.Text = "";
            ctbGoogle.Enabled = true;
            ddlCalendarsList.Items.Clear();
        }
        #endregion

        #region Event Handler (이벤트 핸들러)
        /// <summary>
        /// 구글 계정 로그인
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSignIn_Click(object sender, EventArgs e)
        {
            this.trSelCal.Visible = true;
            this.trSelFilter.Visible = true;
            this.cbtnSignIn.Visible = false;
            this.cbtnSignOut.Visible = true;
            AuthGoogle();
        }

        /// <summary>
        /// 구글 계정 로그아웃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSignOut_Click(object sender, EventArgs e)
        {
            SignOut();
        }        

        /// <summary>
        /// 캘린더 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnGetCalendar_Click(object sender, EventArgs e)
        {
            var Integrator = Session["CalIntegrator"] as CalIntegrator;
            var selCalendarId = ddlCalendarsList.SelectedValue;
            var selFilter = ddlSearchFilter.SelectedValue;
            string googleAccount = Session["GoogleAccount"] as string;

            // 점검
            if (Integrator == null || string.IsNullOrEmpty(googleAccount) || string.IsNullOrEmpty(selCalendarId))
            {
                SignOut();
                return;
            }

            
            // 달력을 얻으려고
            var result = Integrator.GetCalendar(selCalendarId);
            if (result is Exception)
                RenderException(result as Exception);

            // Filter
            DateTime dt = DateTime.Now;
            switch (selFilter)
            {
                case "0": //전체
                    dt = DateTime.Parse("2001-01-01");
                    break;
                case "WEEK": //주
                    dt = dt.AddDays(-7);
                    break;
                case "MON": //월
                    dt = dt.AddMonths(-1);
                    break;
                case "U": //기간
                    dt = DateTime.Parse(this.ctbDate.Text);
                    break;
            }


            CalendarListEntry CalendarEntry = result as CalendarListEntry;
            result = Integrator.GetEvents(selCalendarId, dt);
            List<Event> CalendarEvents = result as List<Event>;

            List<BE.GoogleCalendarInfo> listobj = new List<BE.GoogleCalendarInfo>();


            foreach (Event _be in CalendarEvents)
            {
                //_be.Summary 제목
                //_be.description      내용
                //_be.StartTime 시작
                //_be.EndTime;  종료
                //_be.Location 장소
                
                //OutputScheduleLog(_be.Title + "|" + _be.UID);
                BE.GoogleCalendarInfo _info = new BE.GoogleCalendarInfo();
                _info.CompanyID = CurrentUserInfo.CompanyID;
                _info.UserID = CurrentUserInfo.UserID;
                _info.Title = _be.Summary.ToXSS();    //제목
                _info.Location = _be.Location.ToXSS();  //장소 


                if (_be.Start.DateTime == null && _be.End.DateTime == null)
                {
                    _info.StartTime = Convert.ToDateTime(_be.Start.Date);
                    _info.EndTime = Convert.ToDateTime(_be.End.Date);
                }
                else
                {
                    _info.StartTime = Convert.ToDateTime(_be.Start.DateTime); //시작시간

                    if (_be.Start.DateTime == _be.End.DateTime)
                        _info.EndTime = Convert.ToDateTime(_be.End.DateTime).AddHours(1);    //종료시간
                    else
                        _info.EndTime = Convert.ToDateTime(_be.End.DateTime);    //종료시간
                }

                if (String.IsNullOrEmpty(_be.Description))
                {
                    _info.Content = "";
                }
                else
                {
                    if (_be.Description.Length > 1000)
                        _info.Content = _be.Description.Substring(0, 1000).ToXSS(); //내용
                    else
                        _info.Content = _be.Description.ToXSS();
                }

                _info.Uid = _be.ICalUID;
                _info.DeptID = CurrentUserInfo.DeptID;
                listobj.Add(_info);
            }

            BLL.GoogleCalendar bll = new BLL.GoogleCalendar();
            if (bll.GoogleCalendarBulk(listobj))
            {
                GetLatestSyncTime();
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "SuccessSync", GetResource("SuccessSync"), "");
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "GoogleAccErr", GetResource("GoogleAccErr"), "");
            }
        }
        #endregion



        #region Property (프로퍼티)

        #endregion


    }



}