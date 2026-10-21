using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Report.BE;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class Mod : Base.BasePageReport
    {
        #region 변수설정

        public string NowStartDate = string.Empty;//금주업무 시작일
        public string NextStartDate = string.Empty; //다음주 업무 시작일

        public string AttachLimitSize = string.Empty; //첨부파일 제한용량
        private string eventArgs = String.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.Mod).Assembly;	//어셈블리 선언

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

            LoadList();
            LoadAttach();
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
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
                cbtnRequest.Text = resMng.GetString("ConfirmRequest");
                cbtnReport.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("BtnCancel");
                cbtnSwfFileAdd.Text = resMng.GetString("AddFile");
                cbtnBringNow.Text = resMng.GetString("BringNowJobSche");
                cbtnBringNext.Text = resMng.GetString("BringNextJobSche");
                cbtnBringReporter.Text = resMng.GetString("GetReportList");
            }
        }

        /// <summary>
        /// 보고받는 대상자 이름
        /// _flag : 상태(0:-,1:저장;2:요청;3:확인)
        /// </summary>
        private void GetShareAdminName()
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
        /// 작성자  : 심재운
        /// 리스트
        /// </summary>
        private void LoadList()
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            DAL.List dl = new DAL.List();
            List<ListInfo> list = dl.GetList(CurrentUserInfo.CompanyID, ListID.ToString());

            //접근 권한 체크
            if (GetAuthAccess(list) == false)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            foreach (ListInfo info in list)
            {
                // 구분(공개여부: 1:공개, 2:비공개)
                rbOpen.Checked = info.IsClose == "1" ? true : false;
                rbClose.Checked = info.IsClose == "2" ? true : false;

                // 보고내용(금주)
                //string tmpContent = Regex.Replace(info.NowJob, "</body>\r\n</html>", "</body></html>", RegexOptions.IgnoreCase);
                //info.NowJob = Regex.Replace(tmpContent, "</body></html>", "</body></html>", RegexOptions.IgnoreCase);
                tbthisWeek.Value = info.NowJob.ToReverseXSS();

                // 보고내용(차주)
                //tmpContent = Regex.Replace(info.NextJob, "</body>\r\n</html>", "</body></html>", RegexOptions.IgnoreCase);
                //info.NextJob = Regex.Replace(tmpContent, "</body></html>", "</body></html>", RegexOptions.IgnoreCase);
                tbNextWeek.Value = info.NextJob.ToReverseXSS();

                // 이슈
                //tmpContent = Regex.Replace(info.Issue, "</body>\r\n</html>", "</body></html>", RegexOptions.IgnoreCase);
                //info.Issue = Regex.Replace(tmpContent, "</body></html>", "</body></html>", RegexOptions.IgnoreCase);
                tbIssue.Value = info.Issue.ToReverseXSS();

                string sDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(info.StartDate, PreLangReal)).ToShortDateString();
                string eDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(info.EndDate, PreLangReal)).ToShortDateString();

                lblThisWeek.Text = GetResource("ThisWork") + " <br />( " + sDate + " ~ " + eDate + " )";

                string nsDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(info.StartDate, PreLangReal)).AddDays(7).ToShortDateString();
                string neDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(info.EndDate, PreLangReal)).AddDays(7).ToShortDateString();

                lblNextWeek.Text = GetResource("NextWork") + " <br />( " + nsDate + " ~ " + neDate + " )";

                NowStartDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(info.StartDate, PreLangReal)).ToString("yyyyMMdd");
                NextStartDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(info.StartDate, PreLangReal)).AddDays(7).ToString("yyyyMMdd");

                //상태(0:-,1:저장;2:요청;3:확인)
                if (info.Flag == "2" || info.Flag == "3")
                {
                    string name = this.GetLangName(
                                        info.CompNameBase.ToString().ToNull(), info.CompNameKo.ToString().ToNull(),
                                        info.CompNameEn.ToString().ToNull(), info.CompNameJa.ToString().ToNull(),
                                        info.CompNameFr.ToString().ToNull(), info.CompNameZhcn.ToString().ToNull(),
                                        info.CompNameZhtw.ToString().ToNull(), PreLang);

                    string title = this.GetLangName(
                                    info.CompTitleBase.ToString().ToNull(), info.CompTitleKo.ToString().ToNull(),
                                    info.CompTitleEn.ToString().ToNull(), info.CompTitleJa.ToString().ToNull(),
                                    info.CompTitleFr.ToString().ToNull(), info.CompTitleZhcn.ToString().ToNull(),
                                    info.CompTitleZhtw.ToString().ToNull(), PreLang);

                    lblName.Text = title + " " + name;
                }
                else
                {
                    GetShareAdminName();
                }
            }
        }

        /// <summary>
        /// 접근 권한 체크
        /// </summary>
        /// <param name="_list">The _list.</param>
        /// <returns></returns>
        private bool GetAuthAccess(List<ListInfo> _list)
        {
            cbtnRequest.Visible = false;
            cbtnReport.Visible = false;
            bool access = false;
            foreach (ListInfo info in _list)
            {
                if (info.WriteUserID == CurrentUserInfo.UserID)
                {
                    access = true;
                    cbtnRequest.Visible = true;
                    cbtnReport.Visible = true;
                }
            }
            return access;
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
        /// 메서드명   :
        /// 작성목적   : 게시물 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <param name="_flag">The _flag.</param>
        /// <returns></returns>
        private bool Modify(string _flag)
        {
            DAL.List dl = new DAL.List();
            List<ListInfo> list = dl.GetList(CurrentUserInfo.CompanyID, ListID.ToString());

            foreach (var info in list)
            {
                info.NowJob = tbthisWeek.Value;
                info.NextJob = tbNextWeek.Value;
                info.Issue = tbIssue.Value;
                info.Flag = _flag;
                info.IsClose = rbOpen.Checked == true ? "1" : "2";
                info.FolderID = 0;

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

                //알람 발송 글자
                string smg = dept + " " + name + " " + GetResource("DoConfirmRequest");

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
                info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

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
                string args = eventArgs;

                if (Modify(args))
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
                    paraDic.Add("listID", ListID.ToString());

                    paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                    paraDic.Add("alertMsg", GetResource("GoList"));
                    paraDic.Add("goURL", "/DeskplusEIP/Report/Content/View.aspx");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
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