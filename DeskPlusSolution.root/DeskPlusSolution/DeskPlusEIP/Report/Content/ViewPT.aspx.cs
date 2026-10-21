using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Report.BE;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class ViewPT : Base.BasePageReport
    {
        #region 변수설정

        private string eventArgs = String.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.ViewPT).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     :
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (ListID > 0)
            {
                DAL.List dalList = new DAL.List();
                var alList = dalList.GetList(CurrentUserInfo.CompanyID, ListID.ToString());
                foreach (var list in alList)
                {
                    StartDate = list.StartDate;
                    FolderID = list.FolderID;
                }
            }

            #region 보고체계 지정

            int i = 0;

            DAL.Folder fDal = new DAL.Folder();
            var dtr = fDal.GetFolderByInform(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);

            ddlFolder.Items.Clear();
            ddlFolder.Items.Add(new ListItem("-------", ""));
            ddlFolder.AppendDataBoundItems = true;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (i == 0 && FolderID == 0) FolderID = dtr["FolderID"].ToString().ToNumber<int>();
                        i += 1;

                        ddlFolder.Items.Add(new ListItem(this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS(),
                                        dtr["FolderID"].ToString()));
                    }

                    ddlFolder.SelectedValue = FolderID.ToString();
                }
            }

            #endregion 보고체계 지정

            LoadList();

            //금주-다음주 타이틀 표기
            LoadTime(StartDate);

            //조회 테이블 저장
            DAL.ListViewer dal = new DAL.ListViewer();
            dal.Add(CurrentUserInfo.CompanyID, ListID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
            StartDate = GetDecryptParam("startDate", "");
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
                cbtnCancel.Text = resMng.GetString("Close");
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnShortReply.Text = resMng.GetString("AddShort");
            }
        }

        /// <summary>
        /// Loads the time.
        /// </summary>
        /// <param name="_startDate">The _start date.</param>
        /// <remarks></remarks>
        private void LoadTime(string _startDate)
        {
            #region 이전주 가져오기

            string beforeStartDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(_startDate, PreLangReal)).AddDays(-7).ToString("yyyyMMdd");
            BeforeStartDate = beforeStartDate;

            #endregion 이전주 가져오기

            #region 다음 주 가져오기

            string nextStartDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(_startDate, PreLangReal)).AddDays(7).ToString("yyyyMMdd");
            NextStartDate = nextStartDate;

            #endregion 다음 주 가져오기

            StartDate = _startDate;

            string sDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(_startDate, PreLangReal)).ToShortDateString();//월
            string eDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(_startDate, PreLangReal)).AddDays(4).ToShortDateString();//금
            lblThisWeek.Text = GetResource("ThisWork") + " <br />( " + sDate + " ~ " + eDate + " )";

            string nsDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(_startDate, PreLangReal)).AddDays(7).ToShortDateString();
            string neDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(_startDate, PreLangReal)).AddDays(7).ToShortDateString();
            lblNextWeek.Text = GetResource("NextWork") + " <br />( " + nsDate + " ~ " + neDate + " )";

            TimeList.InnerText = sDate;
            EtimeList.InnerText = eDate;
        }

        /// <summary>
        /// 일지 내용가져오기
        /// </summary>
        private void LoadList()
        {
            DAL.List dl = new DAL.List();
            List<ListInfo> listNow = dl.GetListByFolder(CurrentUserInfo.CompanyID, FolderID, StartDate);

            foreach (ListInfo info in listNow)
            {
                lblThisContent.Text = info.NowJob.Replace("\r\n", "<br>").Replace(" ", "&nbsp;");
                lblNextContent.Text = info.NextJob.Replace("\r\n", "<br>").Replace(" ", "&nbsp;");
                lblIssueContent.Text = info.Issue.Replace("\r\n", "<br>").Replace(" ", "&nbsp;");
                ListID = info.ListID; //리스트 아이디 추가
            }
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// 전 주 시작일자
        /// </summary>
        /// <value>The before list ID.</value>
        /// <remarks></remarks>
        public string BeforeStartDate
        {
            get { return GetViewStateString("BeforeStartDate", ""); }
            set { ViewState["BeforeStartDate"] = value; }
        }

        /// <summary>
        /// 다음주 시작일자
        /// </summary>
        /// <value>The next list ID.</value>
        /// <remarks></remarks>
        public string NextStartDate
        {
            get { return GetViewStateString("NextStartDate", ""); }
            set { ViewState["NextStartDate"] = value; }
        }

        /// <summary>
        /// 금주 시작일자
        /// </summary>
        /// <value>The next list ID.</value>
        /// <remarks></remarks>
        public string StartDate
        {
            get { return GetViewStateString("StartDate", ""); }
            set { ViewState["StartDate"] = value; }
        }

        #endregion Property (프로퍼티)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the Click event of the lbtnEvent control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lbtnClick_Click(object sender, EventArgs e)
        {
            try
            {
                string args = eventArgs;

                ListID = 0;
                lblThisContent.Text = "";
                lblNextContent.Text = "";
                lblIssueContent.Text = "";

                FolderID = ddlFolder.SelectedValue.ToNumber<int>();

                switch (args)
                {
                    case "Before": //이전 주 검색
                        StartDate = BeforeStartDate;
                        LoadList();
                        LoadTime(BeforeStartDate);
                        break;

                    case "Next": //다음주 검색
                        StartDate = NextStartDate;
                        LoadList();
                        LoadTime(NextStartDate);
                        break;

                    case "Folder": //지점 검색
                        LoadList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnClick_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}