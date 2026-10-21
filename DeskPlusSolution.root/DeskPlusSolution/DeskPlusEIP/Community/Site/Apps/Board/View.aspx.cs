using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text.RegularExpressions;
using System.Text;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Board
{
    public partial class View : Base.BasePageCommunity
    {
        #region 변수설정
        public int CurrentPage = 0;
        public int CommunityID = 0;
        public int MenuID = 0;
        public int BoardID = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort 
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종료값
        private string eventArgs = String.Empty;
        public string ListType = String.Empty;
        public int Grade = 10;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Board.View).Assembly;	//어셈블리 선언
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
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CommunityID == 0 && BoardID == 0) return;

            #region 권한 체크

            if (Grade == 2)
            {
                cbtnIssue.Visible = true;
                cbtnMod.Visible = true;
                cbtnDel.Visible = true;
            }
            else
            {
                cbtnMod.Visible = false;
                cbtnDel.Visible = false;

                DAL.MenuAuth dal = new DAL.MenuAuth();
                DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, MenuID);

                int WriteAuth = 9;
                int ReadAuth = 9;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            WriteAuth = dtr["WriteAuth"].ToString().ToNumber<int>();
                            ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();
                        }
                    }
                }

                if (Grade <= ReadAuth)
                {
                    if (Grade <= WriteAuth)
                    {
                        cbtnAdd.Visible = true;
                        cbtnReply.Visible = true;
                        cbtnShortReply.Visible = true;
                    }
                    else
                    {
                        cbtnAdd.Visible = false;
                        cbtnReply.Visible = false;
                        cbtnShortReply.Visible = false;
                    }
                }
                else
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }

            #endregion            

            LoadList();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Write");
                cbtnReply.Text = resMng.GetString("Answer");
                cbtnListView.Text = resMng.GetString("ViewStatus");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnCancel.Text = resMng.GetString("List");
                cbtnShortReply.Text = resMng.GetString("AddShortReply");
                cbtnIssue.Text = resMng.GetString("Issue");
                cbtnRecom.Text = resMng.GetString("Recommend");
                cbtnShowImage.Text = this.resMng.GetString("ShowImage");
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            RecordPerPage = GetDecryptParam("recordPerPage", "10");
            ListType = GetDecryptParam("listType", "");
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();        
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 게시물 내용 
        /// </summary>
        private void LoadList()
        {
            #region 내용            
            DAL.Boa dal = new DAL.Boa();

            // 게시물 작성자 아이디 
            int writeUserID = 0;

            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, CommunityID, MenuID, BoardID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BoardIDs = dtr["BoardIDs"].ToString().ToNumber<int>();

                        // 제목
                        lbTitle.Text = dtr["Title"].ToString();

                        // 등록자
                        lbWriteInfo.Text = GetLangName(dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(),
                            dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(),
                            dtr["NameZhcn"].ToString(),
                            dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        if (lbWriteInfo.Text.Trim() == "")
                        {
                            lbWriteInfo.Text = dtr["WriteUserName"].ToString();
                        }

                        // 등록일
                        lbWriteDate.Text = Convert.ToDateTime(dtr["WriteDate"].ToString()).ToShortDateString() + " " +
                            Convert.ToDateTime(dtr["WriteDate"].ToString()).ToShortTimeString();

                        // 조회수
                        lbReadCount.Text = dtr["ReadCount"].ToString();

                        // 추천수 
                        lbRecomCount.Text = dtr["RecomCount"].ToString();

                        // 본문정보
                        StringBuilder sbShowImage = new StringBuilder();
                        if (ListType == "Album")
                        {
                            DAL.BoaAttach dalAttach = new DAL.BoaAttach();
                            List<BE.BoaAttachInfo> alAttach = dalAttach.GetAttachFileLists(CurrentUserInfo.CompanyID, CommunityID, MenuID, BoardID);
                            foreach (BE.BoaAttachInfo attinfo in alAttach)
                            {
                                if (attinfo.FileType.ToUpper() == ".GIF"
                                    || attinfo.FileType.ToUpper() == ".JPG"
                                    || attinfo.FileType.ToUpper() == ".PNG"
                                    || attinfo.FileType.ToUpper() == ".BMP"
                                    || attinfo.FileType.ToUpper() == ".TIF"
                                    || attinfo.FileType.ToUpper() == ".JPEG"
                                    || attinfo.FileType.ToUpper() == ".PJPEG"
                                    || attinfo.FileType.ToUpper() == ".PJPG")
                                {
                                    if (System.IO.File.Exists(Server.MapPath(attinfo.FilePath + attinfo.FileGUID)))
                                    {
                                        #region 이미지 경로 암호화
                                        string images = attinfo.FilePath + attinfo.FileGUID;
                                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                        paraDic.Add("filePath", images);
                                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                        images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                        #endregion

                                        sbShowImage.Append(string.Format("<br><br><br><img id='{0}' class='shrink-me' src='{1}' alt='{2}' title='{2}' ><br>[{2}]",
                                            attinfo.AttachFileID, images, attinfo.FileName));
                                    }
                                }
                            }
                        }

                        string tmpContent = Regex.Replace(dtr["Content"].ToString(), "</body>\r\n</html>", "</body></html>", RegexOptions.IgnoreCase);
                        tmpContent = Regex.Replace(tmpContent, "</body></html>", sbShowImage.ToString() + "</body></html>", RegexOptions.IgnoreCase);
                        lbContent.Text = tmpContent;
                        

                        writeUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                    }
                }
            }
            #endregion

            #region 추천버튼
            if (writeUserID != CurrentUserInfo.UserID)
            {
                cbtnRecom.Visible = true;
            }
            else
            {
                cbtnReply.Visible = true;
                cbtnMod.Visible = true;
                cbtnDel.Visible = true;
            }

            #endregion

            #region 조회자 추가
            BE.BoaViewInfo info = new BoaViewInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CommunityID = CommunityID;
            info.MenuID = MenuID;
            info.BoardID = BoardID;
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;

            DAL.BoaView dalView = new DAL.BoaView();
            int CheckPoint = dalView.Add(info);

            #endregion

            #region 포인트 추가
            if ((CheckPoint == 0) && Grade != 10)
            {
                AddPoint();
            }
            #endregion

            #region 권한

            if (ListType == "Album")
            {
                cbtnReply.Visible = false;
                cbtnIssue.Visible = false;
            }

            #endregion 
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 201-05-24
        /// 작성목적 : 포인트 추가
        /// </summary>
        private void AddPoint()
        {
            // 1.글쓰기 2.조회 3.추천 4.짧은답글 5.답변 6.방문

            BE.PointInfo info = new PointInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CommunityID = CommunityID;
            info.UserID = CurrentUserInfo.UserID;
            info.Type = 2;
            info.Point = 0;

            DAL.Point dal = new DAL.Point();
            dal.Add(info, MenuID);
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 게시물 삭제
        /// </summary>
        private void Del()
        {
            BE.BoaInfo info = new BoaInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CommunityID = CommunityID;
            info.MenuID = MenuID;
            info.BoardID = BoardID;
            info.WriteUserID = CurrentUserInfo.UserID;

            BLL.Boa bll = new BLL.Boa();

            if (BoardID == BoardIDs)
            {
                bll.DelAll(info);
            }
            else
            {
                bll.Del(info);
            }

            string url = "List.aspx";

            if (ListType == "Album")
            {
                url = "ListGallery.aspx";
            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("communityID", CommunityID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("boardID", BoardID.ToString());
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);
            paraDic.Add("listType", ListType);
            paraDic.Add("grade", Grade.ToString());
            paraDic.Add("recordPerPage", RecordPerPage);

            paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
            paraDic.Add("alertMsg", GetResource("GoList"));
            paraDic.Add("goURL", "/DeskPlusEIP/Community/Site/Apps/Board/" + url);

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }
        #endregion

        #region (프로퍼티)


        /// <summary>
        /// BoardIDs
        /// </summary>
        public int BoardIDs
        {
            get { return GetViewStateInt("BoardIDs", "0"); }
            set { ViewState["BoardIDs"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

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
        #endregion

    }
}