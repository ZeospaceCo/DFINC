using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Report.BE;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class View : Base.BasePageReport
    {
        #region 변수설정

        private string eventArgs = String.Empty;
        private int writeUserID;
        public string FlagCheck = string.Empty;
        public string OpenClose = string.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.View).Assembly;	//어셈블리 선언

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
            LoadList();

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
                cbtnShortReply.Text = resMng.GetString("AddShort");
                cbtnComplate.Text = resMng.GetString("WorkConfirm");
                cbtnComplateCancel.Text = resMng.GetString("WorkCancel");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnMod.Text = resMng.GetString("Modify");
                cbtnPrint.Text = resMng.GetString("Print");

                cbtnCancel.Text = (System.IO.Path.GetFileName(Request.Path).ToUpper() == "VIEWPOPUP.ASPX") ? resMng.GetString("Close") : resMng.GetString("List");
            }
        }

        /// <summary>
        /// 보고받는 대상자 이름
        /// </summary>
        private void GetShareAdminName()
        {
            DAL.ShareUser biz = new DAL.ShareUser();
            DataTableReader dtr = biz.GetShareUserTargerUserInfo(CurrentUserInfo.CompanyID, writeUserID);

            string name = string.Empty;
            string title = string.Empty;
            int adminUserID = 0;

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
                        adminUserID = dtr["TargetUserID"].ToString().ToNumber<int>();
                    }
                }
            }

            lblName.Text = name + " " + title;
        }

        /// <summary>
        /// 일지 내용가져오기
        /// </summary>
        private void LoadList()
        {
            DAL.List biz = new DAL.List();
            List<ListInfo> list = biz.GetList(CurrentUserInfo.CompanyID, ListID.ToString());

            //접근 권한 체크
            if (GetAuthAccess(list) == false)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            foreach (ListInfo listInfo in list)
            {
                writeUserID = listInfo.WriteUserID;
                // 구분(공개여부: 1:공개, 2:비공개)
                //rbOpen.Checked = listInfo.IsClose == "1" ? true : false;
                //rbClose.Checked = listInfo.IsClose == "2" ? true : false;

                OpenClose = GetResource("WorkClose");
                if (listInfo.IsClose == "1") OpenClose = GetResource("WorkOpen");

                // 보고내용(금주)
                lblThisContent.Text = listInfo.NowJob.Replace("\r\n", "<br>").Replace(" ", "&nbsp;");

                // 보고내용(차주)
                lblNextContent.Text = listInfo.NextJob.Replace("\r\n", "<br>").Replace(" ", "&nbsp;");

                // 이슈
                lblIssueContent.Text = listInfo.Issue.Replace("\r\n", "<br>").Replace(" ", "&nbsp;");

                string sDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(listInfo.StartDate, PreLangReal)).ToShortDateString();
                string eDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(listInfo.EndDate, PreLangReal)).ToShortDateString();

                lblThisWeek.Text = GetResource("ThisWork") + " <br />( " + sDate + " ~ " + eDate + " )";

                string nsDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(listInfo.StartDate, PreLangReal)).AddDays(7).ToShortDateString();
                string neDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(listInfo.EndDate, PreLangReal)).AddDays(7).ToShortDateString();

                lblNextWeek.Text = GetResource("NextWork") + " <br />( " + nsDate + " ~ " + neDate + " )";

                // 관리자 확인여부(1:저장, 2:요청, 3확인)
                FlagCheck = listInfo.Flag;

                //상태(0:-,1:저장;2:요청;3:확인)
                if (listInfo.Flag == "2" || listInfo.Flag == "3")
                {
                    string name = this.GetLangName(
                                        listInfo.CompNameBase.ToString().ToNull(), listInfo.CompNameKo.ToString().ToNull(),
                                        listInfo.CompNameEn.ToString().ToNull(), listInfo.CompNameJa.ToString().ToNull(),
                                        listInfo.CompNameFr.ToString().ToNull(), listInfo.CompNameZhcn.ToString().ToNull(),
                                        listInfo.CompNameZhtw.ToString().ToNull(), PreLang);

                    string title = this.GetLangName(
                                    listInfo.CompTitleBase.ToString().ToNull(), listInfo.CompTitleKo.ToString().ToNull(),
                                    listInfo.CompTitleEn.ToString().ToNull(), listInfo.CompTitleJa.ToString().ToNull(),
                                    listInfo.CompTitleFr.ToString().ToNull(), listInfo.CompTitleZhcn.ToString().ToNull(),
                                    listInfo.CompTitleZhtw.ToString().ToNull(), PreLang);

                    lblName.Text = title + " " + name;

                    //승인자 일 경우, 승인버튼 보이기
                    if (listInfo.CompUserID == CurrentUserInfo.UserID && ListType == "ConfirmList")
                    {
                        cbtnComplateCancel.Visible = true;
                        cbtnComplate.Visible = true;
                    }

                    if (writeUserID == CurrentUserInfo.UserID)
                    {
                        //승인자가 [확인]을 하면, 나의 글이라도 수정이 불가능하다.
                        if (listInfo.Flag != "3")
                        {
                            cbtnDel.Visible = true;
                            cbtnMod.Visible = true;
                        }
                    }
                }
                else if (listInfo.Flag == "0" || listInfo.Flag == "1")
                {
                    if (writeUserID == CurrentUserInfo.UserID)
                    {
                        cbtnDel.Visible = true;
                        cbtnMod.Visible = true;
                    }
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
            bool access = false;

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleReport = CurrentUserInfo.IsInRole("report");//report admin

            //관리자 및 일지 관리자는 접근 가능
            if (isRoleAdmin || isRoleReport)
            {
                access = true;
                return access;
            }

            foreach (ListInfo info in _list)
            {
                //나의 글 확인
                if (info.WriteUserID == CurrentUserInfo.UserID)
                {
                    access = true;
                    return access;
                }

                if (info.IsClose == "1") //공개글
                {
                    //부서 체크
                    if (info.DeptID == CurrentUserInfo.DeptID)
                    {
                        access = true;
                        return access;
                    }
                }

                //공유자 체크
                DAL.ShareUser dal = new DAL.ShareUser();
                DataTableReader dtr = dal.GetShareUserList(CurrentUserInfo.CompanyID, info.WriteUserID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            //비공개글 (본인하고 보고담당자만 볼 수 있다.)
                            if (info.IsClose == "2")
                            {
                                //보고대상자 (0:아니오;1:예)
                                if (CurrentUserInfo.UserID == dtr["TargetUserID"].ToString().ToNumber<int>() &&
                                    dtr["UseComplete"].ToString() == "1")
                                {
                                    access = true;
                                    break;
                                }
                            }
                            else if (info.IsClose == "1") //공개글
                            {
                                if (CurrentUserInfo.UserID == dtr["TargetUserID"].ToString().ToNumber<int>())
                                {
                                    access = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                return access;
            }
            return access;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        private void Del()
        {
            BE.ListInfo info = new ListInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ListID = ListID;
            info.WriteUserID = CurrentUserInfo.UserID;

            BLL.List biz = new BLL.List();
            if (biz.Del(info))
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

                paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskplusEIP/Report/Content/PrivateList.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        #endregion Method (함수)



        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string args = eventArgs;
                switch (args)
                {
                    case "DEL":  //수정
                        Del();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
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