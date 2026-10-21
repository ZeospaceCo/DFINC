using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class ViewImpo : Board.Web.Base.BasePageBoard
    {
        #region 변수설정

        private string eventArgs = String.Empty;
        public int WriteUserID = 0;
        public int ListID = 0;

        private string OptionShowImage = string.Empty;//첨부된 이미지를 본문에 보여줍니다.
        private string OptionUseReply = string.Empty;//답변글을 사용합니다.
        private string OptionUseShortReply = string.Empty;//댓글을 사용합니다.
        public string OptionShortReplyGood = string.Empty;//댓글 추천/비추천 기능을 사용합니다.
        public string OptionShortReplyTop = string.Empty;//추천 댓글을 최상위에 보여주기
        private string OptionShowViewer = string.Empty;//조회자 체크여부
        private string OptionUseDept = string.Empty;//부서별 게시판 여부
        private string OptionVote = string.Empty;//평가기능 사용여부
        public string OptionListType = string.Empty;//리스트 형식
        private string OptionAlarm = string.Empty;//알람 사용여부
        public string OptionAnonymity = string.Empty;//게시물 익명 여부
        public string Popup = string.Empty;// 팝업 여부
        public string CategoryTitle = ""; //카테고리이름
        public string OptionDownload = "0"; //외부다운로드 가능여부 (0가능,1 불가)

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
                cbtnDel.Text = this.resMng.GetString("ImpoDel");
                cbtnRead.Text = this.resMng.GetString("View_ReadList");
                cbtnRate.Text = this.resMng.GetString("Rate");
                cbtnPrint.Text = this.resMng.GetString("Print");
                cbtnCancel.Text = this.resMng.GetString("List");
                cbtnShortReply.Text = this.resMng.GetString("View_AddShortReply");
                cbtnShowImage.Text = this.resMng.GetString("ShowImage");
                cbtnMail.Text = this.resMng.GetString("SendEmail");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            Popup = GetDecryptParam("popup", "");
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
            LoadList();
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시물 내용
        /// </summary>
        private void LoadList()
        {
            #region 내용

            BLL.Category bizCat = new BLL.Category();
            NameValueCollection nvcAuth = new NameValueCollection();

            //읽기 이상 존재하는지 체크
            nvcAuth = bizCat.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                CurrentUserInfo.DeptID, CategoryID, OptionUseDept, false, BE.BoardAuth.Read, true, false);

            if (nvcAuth.AllKeys.Length == 0)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            int writeUserID = 0;
            string optionModify = ""; //타인수정가능

            DAL.Lists dalList = new DAL.Lists();

            List<ListInfo> list = dalList.GetList(BoaComID, CategoryID, ListID);

            foreach (ListInfo listinfo in list)
            {
                if (listinfo.OptionAnonymity == "1") //익명
                {
                    WriteUserID = 0;
                    string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명,NoName");
                    lbWriteInfo.Text = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
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

                StringBuilder sbShowImage = new StringBuilder();
                if (OptionShowImage == "1")
                {
                    Zeospace.DeskPlus.Board.DAL.AttachFiles dalAttach = new DAL.AttachFiles();
                    List<BE.AttachFileInfo> alAttach = dalAttach.GetAttachFileLists(BoaComID, CategoryID, ListID);
                    foreach (BE.AttachFileInfo info in alAttach)
                    {
                        if (
                            //info.FileType.ToUpper() == ".GIF"
                            //|| info.FileType.ToUpper() == ".JPG"
                            //|| info.FileType.ToUpper() == ".PNG"
                            //|| info.FileType.ToUpper() == ".BMP"
                            //|| info.FileType.ToUpper() == ".EMF"
                            string.Compare(info.FileType, ".GIF", true) == 0
                            || string.Compare(info.FileType, ".JPG", true) == 0
                            || string.Compare(info.FileType, ".PNG", true) == 0
                            || string.Compare(info.FileType, ".BMP", true) == 0
                            || string.Compare(info.FileType, ".EMF", true) == 0
                            )
                        {
                            if (System.IO.File.Exists(Server.MapPath(info.FilePath + info.FileGUID)))
                            {
                                string smallImage = info.FilePath + "Small_" + info.FileGUID;

                                if (System.IO.File.Exists(Server.MapPath(smallImage)) == false)
                                {
                                    #region 600 사이즈 이미지 (미리보기)

                                    //최대 가로, 세로 600
                                    string imgSizeSmall600 = UtilHelper.GetThumnailSize(Server.MapPath(info.FilePath + info.FileGUID), 600, 600);
                                    Bitmap imgFile600 = new Bitmap(Server.MapPath(info.FilePath + info.FileGUID));
                                    Bitmap thumbnail600 = new Bitmap(imgFile600, imgSizeSmall600.Split(',')[0].ToNumber<int>(), imgSizeSmall600.Split(',')[1].ToNumber<int>());
                                    //작은 이미지 Small_ 만들기
                                    thumbnail600.Save(Server.MapPath(info.FilePath + "Small_" + info.FileGUID), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    imgFile600.Dispose();
                                    thumbnail600.Dispose();

                                    #endregion 600 사이즈 이미지 (미리보기)
                                }

                                if (System.IO.File.Exists(Server.MapPath(smallImage)) == true)
                                {
                                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                    paraDic.Add("filePath", smallImage);
                                    string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                    smallImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                                    sbShowImage.Append(string.Format("<br><br><br><img id='{0}' class='shrink-me' src='{1}' alt='{2}' title='{2}' ><br>[{2}]",
                                        info.AttachFileID, smallImage, info.FileName));
                                }
                            }
                        }
                    }
                }

                string tmpContent = Regex.Replace(listinfo.Content, "</body>\r\n</html>", "</body></html>", RegexOptions.IgnoreCase);
                listinfo.Content = Regex.Replace(tmpContent, "</body></html>", sbShowImage.ToString() + "</body></html>", RegexOptions.IgnoreCase);
                lbContent.Text = listinfo.Content;

                writeUserID = listinfo.WriteUserID;
                optionModify = listinfo.OptionModify; //타인수정가능
                listinfo.Title = listinfo.Title;

                OptionAnonymity = listinfo.OptionAnonymity; //게시물 익명 여부

                #region ETC 반영하기

                DAL.ListEtc dalEtc = new DAL.ListEtc();
                List<BE.ListEtcInfo> al = dalEtc.GetList(BoaComID, CategoryID, ListID);

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

                            listinfo.Title = sb.ToString();
                        }
                        //외부 다운로드 체크여부
                        if (info.Etc8.ToString() == "1")
                        {
                            if (!CurrentUserInfo.IsInRole("AllowBoardDownUser") && !CheckIP()) //게시판 첨부다운 허용role이 없고, IP대역도 다르다면 다운불가 메시지 출력
                            {
                                OptionDownload = "1";
                            }

                        }
                    }
                }

                #endregion ETC 반영하기

                #region 말머리

                string headerName = this.GetLangName(
                    listinfo.HeaderBase,
                    listinfo.HeaderKo, listinfo.HeaderEn, listinfo.HeaderJa,
                    listinfo.HeaderFr, listinfo.HeaderZhcn, listinfo.HeaderZhtw, PreLang);

                lbTitle.Text = (headerName.Length > 0) ? "[" + headerName + "] " + listinfo.Title : listinfo.Title;

                #endregion 말머리
            }

            #endregion 내용

            #region 버튼 활성화 여부

            cbtnShortReply.Visible = false;
            cbtnRate.Visible = (OptionVote == "0") ? false : true;
            //pnReple.Visible = (OptionUseShortReply == "1") ? true : false;
            liReply.Visible = (OptionUseShortReply == "1") ? true : false;
            liReplyList.Visible = (OptionUseShortReply == "1") ? true : false;

            //부서별게시판은 권한없이 쓰기 가능
            if (OptionUseDept == "1")
            {
                cbtnShortReply.Visible = true;//댓글
                if (OptionUseShortReply == "0") cbtnShortReply.Visible = false;//댓글 활성화 확인
            }
            else
            {
                NameValueCollection nvc = new NameValueCollection();

                //2 : 쓰기 , 4 : 쓰기 + 댓글쓰기
                foreach (string s in nvcAuth.AllKeys)
                {
                    //댓글 권한자(3,4 는 댓글 추가 가능)
                    if (nvcAuth[s].IndexOf("3") != -1 || nvcAuth[s].IndexOf("4") != -1)
                    {
                        if (OptionUseShortReply == "1") cbtnShortReply.Visible = true;//댓글추가
                    }
                }
            }

            //팝업 용이라면?
            //if (Popup == "1")
            //{
            //    cbtnDel.Visible = false;
            //    cbtnMod.Visible = false;
            //}

            #endregion 버튼 활성화 여부

            #region 조회자 추가

            BE.ListViewersInfo listViewInfo = new BE.ListViewersInfo();
            listViewInfo.CompanyID = BoaComID;
            listViewInfo.CategoryID = CategoryID;
            listViewInfo.ListID = ListID;
            listViewInfo.UserID = CurrentUserInfo.UserID;
            listViewInfo.TitleName = CurrentUserInfo.TitleName;
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
            DataSet ds = dal.GetCategory(BoaComID, CategoryID);

            //BLL.Category biz = new BLL.Category();
            //CategoryTitle = biz.GetPathName(ds, PreLang); //sitemap 경로 반환

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OptionShowImage = dr["OptionShowImage"].ToString();//첨부된 이미지를 본문에 보여줍니다.
                    OptionUseReply = dr["OptionUseReply"].ToString();//답변글을 사용합니다.
                    OptionUseShortReply = dr["OptionUseShortReply"].ToString();//댓글을 사용합니다.
                    OptionShortReplyGood = dr["OptionShortReplyGood"].ToString();//댓글 추천/비추천 기능을 사용합니다.
                    OptionShortReplyTop = dr["OptionShortReplyTop"].ToString();//추천 댓글을 최상위에 보여주기
                    OptionShowViewer = dr["OptionShowViewer"].ToString();//조회자 체크여부
                    OptionUseDept = dr["OptionUseDept"].ToString();//부서별 게시판 여부
                    OptionVote = dr["OptionVote"].ToString();//평가기능 사용여부
                    OptionListType = dr["OptionListType"].ToString();//게시판 리스트 형식
                    OptionAlarm = dr["OptionAlarm"].ToString();//알람 사용여부

                    CategoryTitle = this.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                        dr["NameZhtw"].ToString().ToNull(), PreLang);
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
            GetCategory();

            BE.ImpoUserInfo info = new ImpoUserInfo();
            info.CompanyID = BoaComID;
            info.UserID = CurrentUserInfo.UserID;
            info.ListID = ListID;

            DAL.ImpoUser dal = new DAL.ImpoUser();
            dal.Del(info);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("catID", CatID.ToString());
            paraDic.Add("categoryID", CategoryID.ToString());
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchGroup", SearchGroup);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            paraDic.Add("alertType", AlertMsgEnum.Alert_Success.ToString());
            paraDic.Add("alertMsg", GetResource("GoToList"));
            paraDic.Add("goURL", GoPage);

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        /// <summary>
        /// IP체크
        /// </summary>
        /// <returns></returns>
        private bool CheckIP()
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.IPList allowIPList = new IPList();
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AllowBoardDownLoadIP");

            string ip = "";
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ActiveCode"].ToString() == "1")
                {
                    ip = dv[i]["InterfaceCode"].ToString();
                    string[] ipArr = ip.Split('-');

                    if (ipArr.Length == 1)
                    {
                        allowIPList.Add(ipArr[0]);
                    }
                    else
                    {
                        allowIPList.AddRange(ipArr[0], ipArr[1]);
                    }
                }
            }

            if (allowIPList.CheckNumber(Request.UserHostAddress))
            {
                return true;      // 접속 IP가 사내 IP 대역에 있음. 
            }
            else
            {
                return false;
            }
        }

        #endregion (함수)

        #region (프로퍼티)

        /// <summary>
        /// 페이지 이동
        /// </summary>
        public string GoPage
        {
            get
            {
                BLL.Lists biz = new BLL.Lists();
                return "/DeskplusEIP/Board/Content/" + biz.GoListPageImpo(BoaComID, CurrentUserInfo.UserID);
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
                    case "DEL":  //해제
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