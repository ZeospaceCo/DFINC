using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class Register : Base.BasePageReport
    {
        #region 변수설정

        private string StartDate = string.Empty;
        private string EndDate = string.Empty;

        public string NowStartDate = string.Empty;//금주업무 시작일
        public string NextStartDate = string.Empty; //다음주 업무 시작일

        private string eventArgs = String.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.Register).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
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

            GetAdminName();
            string sDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(StartDate, PreLangReal)).ToShortDateString();
            string eDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(EndDate, PreLangReal)).ToShortDateString();

            NowStartDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(StartDate, PreLangReal)).ToString("yyyyMMdd");

            lblThisWeek.Text = GetResource("ThisWork") + " <br />( " + sDate + " ~ " + eDate + " )";

            string nsDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(StartDate, PreLangReal)).AddDays(7).ToShortDateString();
            string neDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(EndDate, PreLangReal)).AddDays(7).ToShortDateString();

            NextStartDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(StartDate, PreLangReal)).AddDays(7).ToString("yyyyMMdd");

            lblNextWeek.Text = GetResource("NextWork") + " <br />( " + nsDate + " ~ " + neDate + " )";
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
            StartDate = GetDecryptParam("startDate", "");
            EndDate = GetDecryptParam("endDate", "");

            if (StartDate.Length == 0) StartDate = System.DateTime.Now.StartOfWeek().AddDays(1).ToString("yyyyMMdd");//금주 월요일
            if (EndDate.Length == 0) EndDate = System.DateTime.Now.EndOfWeek().AddDays(-1).ToString("yyyyMMdd");//금주 금요일
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
                cbtnBringNow.Text = resMng.GetString("BringNowJobSche");
                cbtnBringNext.Text = resMng.GetString("BringNextJobSche"); ;
                cbtnRequest.Text = resMng.GetString("ConfirmRequest");
                cbtnReport.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("BtnCancel");
                cbtnSwfFileAdd.Text = resMng.GetString("AddFile");
                cbtnBringReporter.Text = resMng.GetString("GetReportList");
            }
        }

        /// <summary>
        /// 보고받는 대상자 이름
        /// </summary>
        private void GetAdminName()
        {
            DAL.ShareUser biz = new DAL.ShareUser();
            DataTableReader dtr = biz.GetShareUserTargerUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            string name = string.Empty;
            string title = string.Empty;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        name = this.GetLangName(
                                        dtr["TargetNameBase"].ToString().ToNull(), dtr["TargetNameKo"].ToString().ToNull(),
                                        dtr["TargetNameEn"].ToString().ToNull(), dtr["TargetNameJa"].ToString().ToNull(),
                                        dtr["TargetNameFr"].ToString().ToNull(), dtr["TargetNameZhcn"].ToString().ToNull(),
                                        dtr["TargetNameZhtw"].ToString().ToNull(), PreLang);

                        title = this.GetLangName(
                                        dtr["TargetTitleBase"].ToString().ToNull(), dtr["TargetTitleKo"].ToString().ToNull(),
                                        dtr["TargetTitleEn"].ToString().ToNull(), dtr["TargetTitleJa"].ToString().ToNull(),
                                        dtr["TargetTitleFr"].ToString().ToNull(), dtr["TargetTitleZhcn"].ToString().ToNull(),
                                        dtr["TargetTitleZhtw"].ToString().ToNull(), PreLang);
                    }
                }
            }

            lblName.Text = name + " " + title;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 보고저장
        /// 작성자     : daniel
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void WriteAdd(string _flag)
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            int listID = Addition(_flag);

            if (listID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("years", Years.ToString());
                paraDic.Add("months", Months.ToString());
                paraDic.Add("targetUserID", TargetUserID.ToString());
                paraDic.Add("targetDeptID", TargetDeptID.ToString());
                paraDic.Add("weeks", Weeks.ToString());
                paraDic.Add("flag", Flag.ToString());
                paraDic.Add("searchField", SearchField.ToString());
                paraDic.Add("searchValue", SearchValue.ToString());
                paraDic.Add("listType", ListType.ToString());
                paraDic.Add("listID", listID.ToString());

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Report/Content/View.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        private int Addition(string _flag)
        {
            Zeospace.DeskPlus.Report.BE.ListInfo info = new BE.ListInfo();
            BLL.List bll = new BLL.List();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.YYYYMM = Months.ToString().Length == 1 ? Years.ToString() + "0" + Months.ToString() : Years.ToString() + Months.ToString();
            info.StartDate = StartDate;
            info.EndDate = EndDate;
            info.NowJob = tbthisWeek.Value;
            info.NextJob = tbNextWeek.Value;
            info.Issue = tbIssue.Value;
            info.Flag = _flag;
            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteUserName = CurrentUserInfo.Name;
            info.IsClose = rbOpen.Checked == true ? "1" : "2";
            info.DeptID = CurrentUserInfo.DeptID;
            info.FolderID = 0;

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            //알람 발송 글자
            string smg = CurrentUserInfo.DeptName + " " + CurrentUserInfo.Name + " " + GetResource("DoConfirmRequest");

            return bll.Add(info, smg, PreLang);
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        #endregion Property (프로퍼티)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 보고
        /// 작성자     : daniel
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();
                string args = eventArgs;
                WriteAdd(args);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
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