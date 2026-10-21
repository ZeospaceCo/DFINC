using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Admin.Statistic
{
    public partial class View : Base.BasePageBoardAdmin
    {
        #region 변수설정

        private string eventArgs = String.Empty;
        public int WriteUserID = 0;
        public int ListID = 0;

        private string OptionShowImage = string.Empty;//첨부된 이미지를 본문에 보여줍니다.
        private string OptionUseReply = string.Empty;//답변글을 사용합니다.
        private string OptionUseShortReply = string.Empty;//댓글을 사용합니다.
        private string OptionShowViewer = string.Empty;//조회자 체크여부
        private string OptionUseDept = string.Empty;//부서별 게시판 여부
        private string OptionVote = string.Empty;//평가기능 사용여부
        private string OptionAlarm = string.Empty;//알람 사용여부

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.View).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnRead.Text = this.resMng.GetString("View_ReadList");
                cbtnRate.Text = this.resMng.GetString("Rate");
                cbtnCancel.Text = this.resMng.GetString("List");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CatID = GetDecryptParam("catID", "0").ToNumber<int>();
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchGroup = GetDecryptParam("searchGroup", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CategoryID == 0 && ListID == 0) return;

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            GetCategory();

            #region 내용 보이기

            int writeUserID = 0;
            string optionModify = ""; //타인수정가능

            DAL.Lists dalList = new DAL.Lists();

            List<ListInfo> list = dalList.GetList(CurrentUserInfo.CompanyID, CategoryID, ListID);

            foreach (ListInfo listinfo in list)
            {
                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명");

                if (AnonymityName == listinfo.WriteUserName) //익명
                {
                    WriteUserID = 0;
                    lbWriteInfo.Text = AnonymityName;
                }
                else
                {
                    WriteUserID = listinfo.WriteUserID;
                    lbWriteInfo.Text = GetWriteUserName(
                        listinfo.WriteUserName,
                        listinfo.DeptName,
                        listinfo.TitleName,
                        listinfo.DutyName);
                }

                lbWriteDate.Text = listinfo.WriteDateDT.ToShortDateString() + " " + listinfo.WriteDateDT.ToShortTimeString();//등록일
                lbReadCount.Text = listinfo.ReadCount.ToString();//조회수
                lbContent.Text = listinfo.Content;//내용

                writeUserID = listinfo.WriteUserID;
                optionModify = listinfo.OptionModify; //타인수정가능
                lbTitle.Text = listinfo.Title;

                #region ETC 반영하기

                DAL.ListEtc dalEtc = new DAL.ListEtc();
                List<BE.ListEtcInfo> al = dalEtc.GetList(CurrentUserInfo.CompanyID, CategoryID, ListID);

                if (al.Count > 0)
                {
                    foreach (BE.ListEtcInfo info in al)
                    {
                        if (info.Etc2.ToNull().Length > 0)
                        {
                            string[] etcStr = info.Etc2.Split(',');
                            StringBuilder sb = new StringBuilder();
                            if (etcStr[2].ToString() != "0") sb.Append("<b>");
                            if (etcStr[0].ToString() != "") sb.Append("<font color='" + etcStr[0].ToString() + "'>");
                            if (etcStr[1].ToString() != "") sb.Append("<font size='" + etcStr[1].ToString() + "'>");
                            sb.Append(listinfo.Title.Trim());
                            if (etcStr[0].ToString() != "") sb.Append("</font>");
                            if (etcStr[1].ToString() != "") sb.Append("</font>");
                            if (etcStr[2].ToString() != "0") sb.Append("</b>");

                            lbTitle.Text = sb.ToString();
                        }
                    }
                }

                #endregion ETC 반영하기
            }

            #endregion 내용 보이기

            #region 버튼 활성화 여부

            cbtnRate.Visible = (OptionVote == "0") ? false : true;
            pnReple.Visible = (OptionUseShortReply == "1") ? true : false;

            #endregion 버튼 활성화 여부

            #region 조회자 추가

            BE.ListViewersInfo listViewInfo = new BE.ListViewersInfo();
            listViewInfo.CompanyID = BoaComID;
            listViewInfo.CategoryID = CategoryID;
            listViewInfo.ListID = ListID;
            listViewInfo.UserID = CurrentUserInfo.UserID;
            listViewInfo.TitleName = CurrentUserInfo.TitleName;
            //listViewInfo.DeptName = CurrentUserInfo.DeptName;
            //listViewInfo.DutyName = CurrentUserInfo.DutyName;
            //listViewInfo.UserName = CurrentUserInfo.Name;
            DAL.ListViewers bizListView = new DAL.ListViewers();
            bizListView.Add(listViewInfo);

            //조회자 현황 미선택시
            if (OptionShowViewer != "1") cbtnRead.Visible = false;

            #endregion 조회자 추가
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 카테고리 옵션 적용
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-05-09
        /// </summary>
        private void GetCategory()
        {
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(CurrentUserInfo.CompanyID, CategoryID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OptionShowImage = dr["OptionShowImage"].ToString();//첨부된 이미지를 본문에 보여줍니다.
                    OptionUseReply = dr["OptionUseReply"].ToString();//답변글을 사용합니다.
                    OptionUseShortReply = dr["OptionUseShortReply"].ToString();//댓글을 사용합니다.
                    OptionShowViewer = dr["OptionShowViewer"].ToString();//조회자 체크여부
                    OptionUseDept = dr["OptionUseDept"].ToString();//부서별 게시판 여부
                    OptionVote = dr["OptionVote"].ToString();//평가기능 사용여부
                    OptionListType = dr["OptionListType"].ToString();//게시판 리스트 형식
                    OptionAlarm = dr["OptionAlarm"].ToString();//알람 사용여부
                }
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 작성자 이름설정하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_titleName">Name of the _title.</param>
        /// <param name="_dutyName">Name of the _duty.</param>
        /// <returns></returns>
        private string GetWriteUserName(string _writeUserName, string _deptName, string _titleName, string _dutyName)
        {
            bool isUse = false;
            string nameFieldFormat = _writeUserName;

            string applyCategory = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameFieldCategory", "0");

            if (applyCategory.IndexOf(",") == -1)
            {
                if (applyCategory == "0" || applyCategory == CategoryID.ToString()) isUse = true;
            }
            else
            {
                string[] applyCategoryStr = applyCategory.Split(',');
                if (applyCategoryStr.Length > 0)
                {
                    for (int i = 0; i < applyCategoryStr.Length; i++)
                    {
                        if (applyCategoryStr[i].ToString() == CategoryID.ToString()) isUse = true;
                    }
                }
            }

            if (isUse)
            {
                nameFieldFormat = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameField", "WriteName");
                nameFieldFormat = (nameFieldFormat.IndexOf("WriteName") != -1) ? nameFieldFormat.Replace("WriteName", _writeUserName) : nameFieldFormat;
                nameFieldFormat = (nameFieldFormat.IndexOf("DeptName") != -1) ? nameFieldFormat.Replace("DeptName", _deptName) : nameFieldFormat;
                nameFieldFormat = (nameFieldFormat.IndexOf("TitleName") != -1) ? nameFieldFormat.Replace("TitleName", _titleName) : nameFieldFormat;
                nameFieldFormat = (nameFieldFormat.IndexOf("DutyName") != -1) ? nameFieldFormat.Replace("DutyName", _dutyName) : nameFieldFormat;
            }

            //이름 보이기 (이름:WriteName, 부서:Dept, 직급:Title, 직책:Duty)
            return nameFieldFormat;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        private void Del()
        {
            BE.ListInfo info = new BE.ListInfo();
            info.CompanyID = BoaComID;
            info.CategoryID = CategoryID;
            info.ListID = ListID;
            info.WriteUserID = CurrentUserInfo.UserID;

            BLL.Lists biz = new BLL.Lists();
            biz.Del(info);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("catID", CatID.ToString());
            paraDic.Add("categoryID", CategoryID.ToString());
            paraDic.Add("listID", ListID.ToString());
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchGroup", SearchGroup);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
            paraDic.Add("alertMsg", this.resMng.GetString("GoToList"));
            paraDic.Add("goURL", GoPage);

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        #endregion (함수)

        #region (프로퍼티)

        /// <summary>
        /// //게시판 리스트 형식
        /// </summary>
        public string OptionListType
        {
            get { return GetViewStateString("OptionListType", ""); }
            set { ViewState["OptionListType"] = value; }
        }

        /// <summary>
        /// 페이지 이동
        /// </summary>
        public string GoPage
        {
            get
            {
                string goPage = "/DeskplusEIP/Board/Admin/Content/List.aspx";
                if (OptionListType == "1") goPage = "/DeskplusEIP/Board/Admin/Content/GalleryList.aspx";
                if (OptionListType == "2") goPage = "/DeskplusEIP/Board/Admin/Content/BlogList.aspx";
                return goPage;
            }
        }

        #endregion (프로퍼티)

        #region (이벤트핸들러)

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
                //통합게시판 회사아이디
                BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);
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

        #endregion (이벤트핸들러)
    }
}