using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Report.BE;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class RegisterInform : Base.BasePageReport
    {
        #region 변수설정

        public string NowStartDate = string.Empty;//금주업무 시작일
        public string NextStartDate = string.Empty; //다음주 업무 시작일
        public string StartDate = string.Empty;
        public string EndDate = string.Empty;

        public string AttachLimitSize = string.Empty; //첨부파일 제한용량
        private string eventArgs = String.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.RegisterInform).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region (함수)

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     :
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            #region 금주 및 차주 업무 현황

            string sDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(StartDate, PreLangReal)).ToShortDateString();
            string eDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(EndDate, PreLangReal)).ToShortDateString();

            NowStartDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(StartDate, PreLangReal)).ToString("yyyyMMdd");

            lblThisWeek.Text = GetResource("ThisWork") + " <br />( " + sDate + " ~ " + eDate + " )";

            string nsDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(StartDate, PreLangReal)).AddDays(7).ToShortDateString();
            string neDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(EndDate, PreLangReal)).AddDays(7).ToShortDateString();

            NextStartDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(StartDate, PreLangReal)).AddDays(7).ToString("yyyyMMdd");

            lblNextWeek.Text = GetResource("NextWork") + " <br />( " + nsDate + " ~ " + neDate + " )";

            #endregion 금주 및 차주 업무 현황

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

            if (FolderID > 0)
            {
                LoadList();
                LoadAttach();
            }
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
            StartDate = GetDecryptParam("startDate", "");
            EndDate = GetDecryptParam("endDate", "");
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
                cbtnReport.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("BtnCancel");
                //cbtnSwfFileAdd.Text = resMng.GetString("AddFile");
                cbtnBringNow.Text = resMng.GetString("BringNowJobSche");
                cbtnBringNext.Text = resMng.GetString("BringNextJobSche");
                cbtnBringReporter.Text = resMng.GetString("GetReportList");
                cbtnPT.Text = resMng.GetString("ViewPT");
                cbtnShortReply.Text = resMng.GetString("AddShort");

            }
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 리스트
        /// </summary>
        private void LoadList()
        {
            ListID = 0;

            DAL.List dl = new DAL.List();
            List<ListInfo> list = dl.GetListByFolder(CurrentUserInfo.CompanyID, FolderID, NowStartDate);

            foreach (ListInfo info in list)
            {
                // 구분(공개여부: 1:공개, 2:비공개)
                rbOpen.Checked = info.IsClose == "1" ? true : false;
                rbClose.Checked = info.IsClose == "2" ? true : false;

                // 보고내용(금주)
                tbthisWeek.Value = info.NowJob.ToReverseXSS();

                // 보고내용(차주)
                tbNextWeek.Value = info.NextJob.ToReverseXSS();

                // 이슈
                tbIssue.Value = info.Issue.ToReverseXSS();

                ListID = info.ListID; //리스트 아이디 추가
            }
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 첨부파일 정보 가져오기
        /// </summary>
        private void LoadAttach()
        {
            DAL.Attach biz = new DAL.Attach();
            List<BE.AttachInfo> al = biz.GetAttachLists(CurrentUserInfo.CompanyID, ListID);

            string saveFileList = "";

            if (al.Count > 0)
            {
                foreach (BE.AttachInfo info in al)
                {
                    saveFileList += "S>" + info.AttachFileID + ">" + info.FilePath + ">" + info.FileName + ">" + info.FileGUID + "|";
                }
            }

            //fileList = fileList.TrimEnd('|');
            saveFileList = saveFileList.TrimEnd('|');

            //tbHiddenFileList.Text = fileList;					//첨부파일 정보를 저장하며 리스트박스와 동기화
            tbHiddenFileList.Text = saveFileList;					//첨부파일 정보를 저장하며 리스트박스와 동기화
            tbHiddenSaveFileList.Text = saveFileList;			//서버 파일 삭제시 비교로 사용 될 원본 파일 리스트(FileID 포함)
        }

        /// <summary>
        /// 추가하기
        /// </summary>
        /// <returns></returns>
        private int Addition()
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
            info.Flag = "4";
            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteUserName = CurrentUserInfo.Name;
            info.IsClose = rbOpen.Checked == true ? "1" : "2";
            info.DeptID = CurrentUserInfo.DeptID;
            info.FolderID = ddlFolder.SelectedValue.ToNumber<int>();

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            //알람 발송 글자
            string smg = "[" + CurrentUserInfo.DeptName + " " + CurrentUserInfo.Name + "]" + " 보고서 작성하였습니다.";

            return bll.Add(info, smg, PreLang);
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시물 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            DAL.List dl = new DAL.List();
            List<ListInfo> list = dl.GetList(CurrentUserInfo.CompanyID, ListID.ToString());

            foreach (var info in list)
            {
                info.NowJob = tbthisWeek.Value;
                info.NextJob = tbNextWeek.Value;
                info.Issue = tbIssue.Value;
                info.Flag = "4";
                info.IsClose = rbOpen.Checked == true ? "1" : "2";
                info.FolderID = ddlFolder.SelectedValue.ToNumber<int>();

                info.WriteUserID = CurrentUserInfo.UserID;
                info.WriteUserName = CurrentUserInfo.Name;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.WriteDateDT = FileHelper.GetYearMonth();

                //작성자
                string name = this.GetLangName(
                                        info.NameBase.ToString().ToNull(), info.NameKo.ToString().ToNull(),
                                        info.NameEn.ToString().ToNull(), info.NameJa.ToString().ToNull(),
                                        info.NameFr.ToString().ToNull(), info.NameZhcn.ToString().ToNull(),
                                        info.NameZhtw.ToString().ToNull(), PreLang);

                string dept = this.GetLangName(
                                        info.DeptBase.ToString().ToNull(), info.DeptKo.ToString().ToNull(),
                                        info.DeptEn.ToString().ToNull(), info.DeptJa.ToString().ToNull(),
                                        info.DeptFr.ToString().ToNull(), info.DeptZhcn.ToString().ToNull(),
                                        info.DeptZhtw.ToString().ToNull(), PreLang);
                

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
                info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

                //알람 발송 글자
                string smg = "[" + CurrentUserInfo.DeptName + " " + CurrentUserInfo.Name + "]" + " 보고서 작성하였습니다.";

                BLL.List bll = new BLL.List();
                bll.Mod(info, smg, PreLang);
            }

            return true;
        }

        #endregion (함수)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시물버튼 클릭
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

                BE.ShortReplyInfo info = new BE.ShortReplyInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;

                if (ListID == 0)
                {
                    ListID = Addition();
                    info.Content = "[" + CurrentUserInfo.DeptName + " " + CurrentUserInfo.Name + "]" + System.DateTime.Now.ToLongTimeString() + " " + GetResource("Registered");
                }
                else
                {
                    Modify();
                    info.Content = "[" + CurrentUserInfo.DeptName + " " + CurrentUserInfo.Name + "]" + System.DateTime.Now.ToLongTimeString() + " " + GetResource("Modifyed");
                }

                info.ListID = ListID;
                info.UserID = CurrentUserInfo.UserID;
                info.UserName = CurrentUserInfo.Name;

                BLL.ShortReply dal = new BLL.ShortReply();
                //pretitlt 없으면 알람은 발송되지 않는다.(로그용)
                dal.Add(info, CurrentUserInfo.PreferredLanguage, "");

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("years", Years.ToString());
                paraDic.Add("months", Months.ToString());
                paraDic.Add("targetUserID", TargetUserID.ToString());
                paraDic.Add("targetDeptID", TargetDeptID.ToString());
                paraDic.Add("weeks", Weeks.ToString());
                paraDic.Add("flag", "4");
                paraDic.Add("searchField", SearchField.ToString());
                paraDic.Add("searchValue", SearchValue.ToString());
                paraDic.Add("listType", ListType.ToString());
                paraDic.Add("listID", ListID.ToString());
                paraDic.Add("folderID", FolderID.ToString());
                paraDic.Add("startDate", StartDate);
                paraDic.Add("endDate", EndDate);

                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskplusEIP/Report/Content/RegisterInform.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
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

        #region Property (프로퍼티)

        public string Gubun { get; set; }

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        #endregion Property (프로퍼티)
    }
}