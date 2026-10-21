using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.Web.Board
{
    public partial class View : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        #region 변수설정

        public string PageListUrl;
        public int PageNo;

        public int BoardID;
        public int GroupID;
        public int ListOrder;
        public int Depth;

        private string eventArgs = String.Empty;

        #endregion 변수설정

        #endregion Declaration (컨트롤 선언)

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Board.View).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }

            #region 관련프로젝트 처리

            if (Authority == 1) cbtnAdd.Visible = false;
            if (Authority == 1) cbtnMod.Visible = false;
            if (Authority == 1) cbtnDel.Visible = false;

            #endregion 관련프로젝트 처리
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            //자료 뿌려주기
            LoadDataContent();

            //짧은 답글 리스트
            LoadShortReply();

            if (System.IO.Path.GetFileName(Request.Path).ToUpper() == "VIEWPOPUP.ASPX")
            {
                cbtnAdd.Visible = false;
                cbtnReply.Visible = false;
                cbtnMod.Visible = false;
                cbtnDel.Visible = false;
            }
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Writing");
                cbtnReply.Text = resMng.GetString("Reply");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnDel.Text = resMng.GetString("Unit_Register_Delete");

                cbtnPrint.Text = resMng.GetString("Print");
                cbtnListView.Text = resMng.GetString("View_ReadList");
                cbtnShortReply.Text = resMng.GetString("AddShortReply");

                ltName.Text = resMng.GetString("Board_List_Name");
                ltTitle.Text = resMng.GetString("Board_List_Title");
                ltWriteDate.Text = resMng.GetString("Board_List_DateTime");
                ltReadCount.Text = resMng.GetString("Board_List_Viewer");

                cbtnCancel.Text = (System.IO.Path.GetFileName(Request.Path).ToUpper() == "VIEWPOPUP.ASPX") ? resMng.GetString("Close") : resMng.GetString("List");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            SearchField = Server.UrlDecode(GetDecryptParam("searchField", ""));
            SearchValue = Server.UrlDecode(GetDecryptParam("searchValue", ""));
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");

            PageListUrl = GetDecryptParam("pageListUrl", "JList.aspx");
            PageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();

            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
            ListOrder = GetDecryptParam("listOrder", "0").ToNumber<int>();
            Depth = GetDecryptParam("depth", "0").ToNumber<int>();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 자료 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        private void LoadDataContent()
        {
            #region 첨부파일 가져오기

            StringBuilder sbShowImage = new StringBuilder();
            DAL.BoardAttach attachfile = new Zeospace.DeskPlus.PMS.DAL.BoardAttach();
            List<BE.BoardAttachInfo> attachList = attachfile.GetAttachFile(CurrentUserInfo.CompanyID, ProjectID, MenuID, BoardID);

            foreach (BE.BoardAttachInfo info in attachList)
            {
                if (info.FileType.ToUpper() == ".GIF"
                    || info.FileType.ToUpper() == ".JPG"
                    || info.FileType.ToUpper() == ".PNG"
                    || info.FileType.ToUpper() == ".BMP"
                    || info.FileType.ToUpper() == ".EMF")
                {
                    if (System.IO.File.Exists(Server.MapPath(info.FilePath + info.FileGUID)))
                    {
                        #region 이미지 경로 암호화

                        string images = info.FilePath + info.FileGUID;
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", images);
                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                        #endregion 이미지 경로 암호화

                        sbShowImage.Append(string.Format("<br><br><br><img id='{0}' class='shrink-me' src='{1}' alt='{2}' title='{2}' ><br>[{2}]",
                            info.AttachFileID, images, info.FileName));
                    }
                }
            }

            #endregion 첨부파일 가져오기

            DAL.Board list = new Zeospace.DeskPlus.PMS.DAL.Board();

            //1. 내용 가져오기
            var al = list.GetBoard(CurrentUserInfo.CompanyID, ProjectID, MenuID, BoardID);
            foreach (var listInfo in al)
            {
                if (listInfo != null)
                {
                    lbTitle.Text = listInfo.Title;

                    string tmpContent = Regex.Replace(listInfo.Contents, "</body>\r\n</html>", "</body></html>", RegexOptions.IgnoreCase);
                    listInfo.Contents = Regex.Replace(tmpContent, "</body></html>", sbShowImage.ToString() + "</body></html>", RegexOptions.IgnoreCase);
                    FormContent = listInfo.Contents;
                    pnContent.Controls.Add(new LiteralControl(listInfo.Contents));

                    #region 작성일자

                    lbWriteDate.Text = listInfo.WriteDate.ToShortDateString();

                    #endregion 작성일자

                    lbReadCount.Text = listInfo.ReadCount.ToString();

                    int writeUserID = listInfo.WriteUserID;

                    #region 수정버튼 활성화 여부

                    cbtnMod.IsDisabled = (writeUserID == CurrentUserInfo.UserID) ? "0" : "1"; //수정모드 비활성화
                    cbtnDel.IsDisabled = (writeUserID == CurrentUserInfo.UserID) ? "0" : "1"; //삭제버튼 활성화

                    #endregion 수정버튼 활성화 여부

                    #region 이름

                    //this.hlName.NavigateUrl = "javascript:DeskPlus.Common.viewAccountInfo('" + CompanyID + "','" + writeUserID + "');";
                    this.hlName.NavigateUrl = "javascript:AccountInfo('" + writeUserID + "');";

                    hlName.Text = GetWriteUserName(
                        this.GetLangName(
                                    listInfo.NameBase,
                                    listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                    listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang));

                    #endregion 이름

                    //조회테이블에 읽기 명단 저장시키기
                    SaveListViewers();
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
        /// <returns></returns>
        private string GetWriteUserName(string _writeUserName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _writeUserName);
            return _writeUserName;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 짧은 답글 리스트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        private void LoadShortReply()
        {
            DAL.BoardShortReply bll = new Zeospace.DeskPlus.PMS.DAL.BoardShortReply();
            DataTableReader dtr = bll.GetBoardShortReply(CurrentUserInfo.CompanyID, ProjectID, MenuID, BoardID);

            List<BE.BoardShortReplyInfo> alList = new List<BoardShortReplyInfo>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.BoardShortReplyInfo info = new BoardShortReplyInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                        info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                        info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();
                        info.BoardShortReplyID = dtr["BoardShortReplyID"].ToString().ToNumber<int>();
                        info.Contents = dtr["Contents"].ToString();
                        info.NameBase = dtr["NameBase"].ToString();
                        info.NameKo = dtr["NameKo"].ToString();
                        info.NameEn = dtr["NameEn"].ToString();
                        info.NameJa = dtr["NameJa"].ToString();
                        info.NameFr = dtr["NameFr"].ToString();
                        info.NameZhcn = dtr["NameZhcn"].ToString();
                        info.NameZhtw = dtr["NameZhtw"].ToString();
                        info.ManyName = dtr["ManyName"].ToString();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        alList.Add(info);
                    }
                }
            }

            ltlShortReplyCount.Text = GetResource("Board_List_ShortReply") + "(" + alList.Count + ")";
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 페이지 이동
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        /// <param name="_url">The _url.</param>
        /// <param name="_mode">The _mode.</param>
        private string GoPageUrl(string _url, string _mode)
        {
            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("searchField", Server.UrlEncode(SearchField));
            paraDic.Add("searchValue", Server.UrlEncode(SearchValue));
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);

            paraDic.Add("pageNo", PageNo.ToString());
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("rqProjectID", RqProjectID.ToString());
            paraDic.Add("pageListUrl", PageListUrl.ToString());

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            return _url + "?p=" + encparm;

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CLOSE_WND", "<script language=\"javascript\">self.location = '" + urlWrapper.GetParameter() + "';</script>");
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 조회자 읽기 명단에 저장시키기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        private void SaveListViewers()
        {
            DAL.BoardViewer bll = new Zeospace.DeskPlus.PMS.DAL.BoardViewer();
            BE.BoardViewerInfo viewInfo = new BoardViewerInfo();

            viewInfo.CompanyID = CurrentUserInfo.CompanyID;
            viewInfo.ProjectID = ProjectID;
            viewInfo.MenuID = MenuID;
            viewInfo.BoardID = BoardID;
            viewInfo.CRUDDate = System.DateTime.Now;
            viewInfo.CRUDType = "R";
            viewInfo.WriteUserID = CurrentUserInfo.UserID;

            bll.Add(viewInfo);
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메일 쓰기 창 띄우기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        /// <returns></returns>
        //public string GetForm()
        //{
        //    Zeospace.DeskPlus.Mail.BLL.MailFunction mailFunction = new Zeospace.DeskPlus.Mail.BLL.MailFunction();
        //    return mailFunction.GetForm();
        //}

        #endregion Method (함수)

        #region Property (프로퍼티)

        #region viewstate

        /// <summary>
        /// Gets or sets the content of the form.
        /// </summary>
        /// <value>The content of the form.</value>
        public string FormContent
        {
            get
            {
                return GetViewStateString("FormContent", "");
            }
            set
            {
                ViewState["FormContent"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the img URL.
        /// </summary>
        /// <value>The img URL.</value>
        public string ImgUrl
        {
            get
            {
                return GetViewStateString("ImgUrl", "");
            }
            set
            {
                ViewState["ImgUrl"] = value;
            }
        }

        /// <summary>
        /// 첨부파일 경로
        /// </summary>
        /// <value>The files info.</value>
        public string AttachFilesInfo
        {
            get
            {
                return GetViewStateString("AttachFilesInfo", "");
            }
        }

        /// <summary>
        /// 첨부파일 개수
        /// </summary>
        /// <value>The attach files count.</value>
        public string AttachFilesCount
        {
            get
            {
                return GetViewStateString("AttachFilesCount", "");
            }
        }

        /// <summary>
        /// Gets the go URL.
        /// </summary>
        /// <value>The go URL.</value>
        public string GoUrl
        {
            get
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("pageNo", PageNo.ToString());
                paraDic.Add("pageListUrl", PageListUrl.ToString());
                paraDic.Add("searchField", Server.UrlEncode(SearchField));
                paraDic.Add("searchValue", Server.UrlEncode(SearchValue));
                paraDic.Add("projectID", ProjectID.ToString());
                paraDic.Add("menuID", MenuID.ToString());
                paraDic.Add("menuType", MenuType.ToString());

                string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                return PageListUrl + "?p=" + encparm;
            }
        }

        /// <summary>
        /// Gets the board list view URL.
        /// </summary>
        /// <value>The board list view URL.</value>
        public string BoardListViewUrl
        {
            get
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("projectID", ProjectID.ToString());
                paraDic.Add("menuID", MenuID.ToString());
                paraDic.Add("menuType", MenuType.ToString());
                paraDic.Add("projectBoardListID", BoardID.ToString());

                string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                return PageListUrl + "?p=" + encparm;
            }
        }

        #endregion viewstate

        #endregion Property (프로퍼티)

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
                    case "MOD":  //수정
                        string scripts = @"<script type=""text/javascript"">location = '" + GoPageUrl("Add.aspx", "edit") + "'</script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);

                        break;

                    case "DEL":  //삭제
                        BE.BoardInfo be = new BoardInfo();
                        be.CompanyID = CurrentUserInfo.CompanyID;
                        be.ProjectID = ProjectID;
                        be.MenuID = MenuID;
                        be.BoardID = BoardID;
                        be.WriteUserID = CurrentUserInfo.UserID;

                        BLL.Board bll = new Zeospace.DeskPlus.PMS.BLL.Board();

                        if (bll.Del(be))
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
                        }
                        else
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
                        }

                        //GoPage(PageListUrl, Gubun);
                        Response.Redirect(GoPageUrl(PageListUrl, ""), false);
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
        /// 뒤로 가기 : CBSs the back_ click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void cbsBack_Click(object sender, System.EventArgs e)
        {
            LoadParam();

            string scripts = @"<script type=""text/javascript"">location = '" + GoPageUrl(PageListUrl, "") + "'</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);
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