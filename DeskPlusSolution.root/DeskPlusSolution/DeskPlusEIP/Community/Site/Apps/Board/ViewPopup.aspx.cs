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
    public partial class ViewPopup : Base.BasePageCommunity
    {
        #region Declaration (컨트롤 선언)

        #endregion

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
        public string AlarmPopup = "N";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Board.ViewPopup).Assembly;	//어셈블리 선언
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
        /// 작성자  : shine
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnListView.Text = this.resMng.GetString("ViewStatus");
                cbtnPrint.Text = this.resMng.GetString("Print");
                cbtnCancel.Text = this.resMng.GetString("Close");
                cbtnShortReply.Text = this.resMng.GetString("AddShortReply");
                cbtnShowImage.Text = this.resMng.GetString("ShowImage");
            }
        }

        /// <summary>
        /// 작성목적   : 내용가져오기
        /// 작성자     : shine
        /// 최초작성일 : 2012-07-01
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CommunityID == 0 && BoardID == 0) return;

            #region 권한 체크

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

            // 알람으로 해당 페이지 왔을 때 
            if (AlarmPopup == "Y")
            {
                DAL.User userdal = new DAL.User();
                DataTableReader dtrUser = userdal.GetUserDetailInfo(CurrentUserInfo.CompanyID, CommunityID, CurrentUserInfo.UserID);

                if (dtrUser != null)
                {
                    if (dtrUser.HasRows)
                    {
                        while (dtrUser.Read())
                        {
                            Grade = dtrUser["Grade"].ToString().ToNumber<int>();
                        }
                    }
                }
            }

            if (Grade <= ReadAuth)
            {
                if (Grade <= WriteAuth)
                {
                    cbtnShortReply.Visible = true;
                }
                else
                {
                    cbtnShortReply.Visible = false;
                }
            }
            else
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            #endregion            

            LoadList();
        }

        /// <summary>
        /// 작성목적   : 게시물 내용
        /// 작성자     : shine
        /// 최초작성일 : 2012-07-01
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
        }
        #endregion

        #region (함수)

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

        #endregion

        #region (프로퍼티)

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
            AlarmPopup = GetDecryptParam("alarmPopup", "N");
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}