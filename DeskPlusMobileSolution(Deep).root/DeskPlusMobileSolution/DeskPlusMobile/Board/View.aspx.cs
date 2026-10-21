using System;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Board
{
    public partial class View : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정

        private int companyID;
        public int categoryID;
        public int listID;
        public int pageNo;
        public string optionShowImage;
        public string optionShowView;
        public string optionUseDept;
        public string optionUseShortReply;
        public string optionUseReply;
        public string optionShortReplyGood; //댓글 추천/비추천 기능을 사용합니다.
        public string optionShortReplyTop; //추천 댓글을 최상위에 보여주기
        public string searchValue;
        public int mainCID;
        public string pCatID;
        public string optionDownload = "0";

        #endregion 변수설정

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Board.View).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadParam();
                LoadData();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            categoryID = GetDecryptParam("CID", "10").ToNumber<int>();

            //통합게시판 회사아이디
            companyID = Ws.BLL.Category.GetCompanyCategoryInfo(categoryID);
            //companyID = UserInfo.CompanyID;

            listID = GetDecryptParam("ID", "10").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            mainCID = GetDecryptParam("MainCID", "").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            pCatID = GetDecryptParam("PCatID", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 섬네일 이미지 가로,세로 비율값 얻어내기
        /// </summary>
        /// <param name="_sourcePath">The _source path.</param>
        /// <param name="_width">The _width.</param>
        /// <param name="_height">The _height.</param>
        /// <returns></returns>
        public string GetThumnailSize(string _sourcePath, int _width, int _height)
        {
            Bitmap sourceImage = new Bitmap(_sourcePath);
            ImageFormat sourceFormat = sourceImage.RawFormat;

            decimal ratio = 0;
            int newWidth = 0;
            int newHeight = 0;

            if (sourceImage.Width < _width && sourceImage.Height < _height)
            {
                newWidth = sourceImage.Width;
                newHeight = sourceImage.Height;
            }
            else
            {
                if (sourceImage.Width > sourceImage.Height)
                {
                    ratio = (decimal)_width / sourceImage.Width;
                    newWidth = _width;
                    newHeight = (int)(sourceImage.Height * ratio);
                }
                else
                {
                    ratio = (decimal)_height / sourceImage.Height;
                    newHeight = _height;
                    newWidth = (int)(sourceImage.Width * ratio);
                }
            }

            sourceImage.Dispose();

            return newWidth + "," + newHeight;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            dvAdd.Visible = false;

            Ws.BLL.Category cbiz = new Ws.BLL.Category();
            DataSet ds = cbiz.GetCategory(companyID, categoryID);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                optionShowImage = dr["OptionShowImage"].ToString();
                optionShowView = dr["OptionShowViewer"].ToString();
                optionUseDept = dr["OptionUseDept"].ToString();
                optionUseShortReply = dr["OptionUseShortReply"].ToString();
                optionUseReply = dr["OptionUseReply"].ToString();
                optionShortReplyGood = dr["OptionShortReplyGood"].ToString(); //댓글 추천/비추천 기능을 사용합니다.
                optionShortReplyTop = dr["OptionShortReplyTop"].ToString(); //추천 댓글을 최상위에 보여주기

                CategoryTitle = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                         dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
            }

            if (optionUseDept == "1")
            {
                dvAdd.Visible = true;//추가
                if (optionUseShortReply == "0") dvAdd.Visible = false;//댓글 활성화 확인
            }
            else
            {
                NameValueCollection nvcAuth = new NameValueCollection();
                Ws.BLL.Category bizCat = new Ws.BLL.Category();

                //읽기 이상 존재하는지 체크
                nvcAuth = bizCat.GetMyAuthCategory(companyID, UserInfo.UserID, UserInfo.DeptID, categoryID, optionUseDept, false, "1,2,3,4", true, false);

                //2 : 쓰기 , 4 : 쓰기 + 댓글쓰기
                foreach (string s in nvcAuth.AllKeys)
                {
                    //댓글 권한자(3,4 는 댓글 추가 가능)
                    if (nvcAuth[s].IndexOf("3") != -1 || nvcAuth[s].IndexOf("4") != -1)
                    {
                        if (optionUseShortReply == "1") dvAdd.Visible = true;//댓글추가
                    }
                }
            }

            //첨부파일 가져오기
            string imgUrl = "";

            Zeospace.DeskPlusMobile.Web.Board.Ws.Data.AttachFiles bizAttachFiles =
            new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.AttachFiles();

            var docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "DocStreamYN");
            DataSet dsDSList = bizAttachFiles.GetDSFiles(companyID, categoryID, listID, "BOA_ATTACH");

            var dtDsList = dsDSList.Tables[0].Rows;
            if (dtDsList.Count > 0 && docStreamYN == "Y")
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"<li class='contentblock'><div class='title flxedA'>");
                sb.Append(resMng.GetString("Attach"));

                foreach (DataRow dr in dtDsList)
                {
                    string fileID = dr["FileID"].ToString();
                    string fileName = dr["FileName"].ToString();
                    string filePath = dr["FilePath"].ToString().Replace("/DeskPlusEIP/FileData/", FilePath + "/");
                    string fileGUID = dr["FileGUID"].ToString();
                    string fileType = dr["FileType"].ToString();
                    string fileLength = dr["FileLength"].ToString().ToNumber<int>().ToString("#,###");
                    if (fileID.Length > 0)
                    {
                        if (fileName.IndexOf("'") > -1) fileName = fileName.Replace("'", "");

                        sb.Append(@"<div><a class='linkDiv' href='#2' onclick=View.goDS('" + fileID + "')>" + fileName + "</a>");
                        sb.Append(@"<span class='iste'>&nbsp;" + fileLength + " byte</span></div>");
                    }
                    else
                    {
                        #region 이미지 다운로드

                        if (fileName.IndexOf("'") > -1) fileName = fileName.Replace("'", "");

                        sb.Append(@"<div><a class='linkDiv' href='#2' onclick=View.goAttach('" + filePath + fileGUID + "','" + fileName.Replace(" ", "") + "')>" + fileName + "</a>");
                        sb.Append(@"<span class='iste'>&nbsp;" + fileLength + " byte</span></div>");

                        #endregion 이미지 다운로드
                    }

                    #region 이미지 미리보기

                    if (optionShowImage == "1")
                    {
                        //첨부파일이 존재하면 이미지 다운로드 및 보기를 처리 한다.
                        if (File.Exists(Server.MapPath(filePath + fileGUID)) == true)
                        {
                            if (
                                string.Compare(fileType, ".GIF", true) == 0
                                || string.Compare(fileType, ".JPG", true) == 0
                                || string.Compare(fileType, ".PNG", true) == 0
                                || string.Compare(fileType, ".BMP", true) == 0
                                || string.Compare(fileType, ".EMF", true) == 0
                            )
                            {
                                //스마트용 이미지 존재하지 않으면 생성.
                                if (System.IO.File.Exists(Server.MapPath(filePath + "Smart_" + fileGUID)) == false)
                                {
                                    #region 350 사이즈 이미지 (스마트폰)

                                    //최대 가로, 세로 350
                                    string imgSizeSmall350 = GetThumnailSize(Server.MapPath(filePath + fileGUID), 350, 350);
                                    Bitmap imgFile350 = new Bitmap(Server.MapPath(filePath + fileGUID));
                                    Bitmap thumbnail350 = new Bitmap(imgFile350, imgSizeSmall350.Split(',')[0].ToNumber<int>(), imgSizeSmall350.Split(',')[1].ToNumber<int>());
                                    //작은 이미지 Smart_ 만들기
                                    thumbnail350.Save(Server.MapPath(filePath + "Smart_" + fileGUID), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    imgFile350.Dispose();
                                    thumbnail350.Dispose();

                                    #endregion 350 사이즈 이미지 (스마트폰)
                                }

                                imgUrl += "<br><img src=\"" + filePath + "Smart_" + fileGUID + "\"><br>[" + fileName + "]<br><br>";
                            }
                        }
                    }

                    #endregion 이미지 미리보기
                }

                sb.Append(@"</div></li>");

                AttachList = sb.ToString();

            }
            else
            {
                DataSet dsAttachList = bizAttachFiles.GetAttachFiles(companyID, categoryID, listID);

                if (dsAttachList != null)
                {
                    if (dsAttachList.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append(@"<li class='contentblock'><div class='title flxedA'>");
                        sb.Append(resMng.GetString("Attach"));

                        foreach (DataRow dr in dsAttachList.Tables[0].Rows)
                        {
                            string fileName = dr["FileName"].ToString();
                            string filePath = dr["FilePath"].ToString().Replace("/DeskPlusEIP/FileData/", FilePath + "/");
                            string fileGUID = dr["FileGUID"].ToString();
                            string fileType = dr["FileType"].ToString();
                            string fileLength = dr["FileLength"].ToString().ToNumber<int>().ToString("#,###");

                            #region 이미지 다운로드

                            if (fileName.IndexOf("'") > -1) fileName = fileName.Replace("'", "");

                            sb.Append(@"<div><a class='linkDiv' onclick=View.goAttach('" + filePath + fileGUID + "','" + fileName.Replace(" ", "") + "')>" + fileName + "</a>");
                            sb.Append(@"<span class='iste'>&nbsp;" + fileLength + " byte</span></div>");

                            #endregion 이미지 다운로드

                            #region 이미지 미리보기

                            if (optionShowImage == "1")
                            {
                                //첨부파일이 존재하면 이미지 다운로드 및 보기를 처리 한다.
                                if (File.Exists(Server.MapPath(filePath + fileGUID)) == true)
                                {
                                    if (
                                        string.Compare(fileType, ".GIF", true) == 0
                                        || string.Compare(fileType, ".JPG", true) == 0
                                        || string.Compare(fileType, ".PNG", true) == 0
                                        || string.Compare(fileType, ".BMP", true) == 0
                                        || string.Compare(fileType, ".EMF", true) == 0
                                    )
                                    {
                                        //스마트용 이미지 존재하지 않으면 생성.
                                        if (System.IO.File.Exists(Server.MapPath(filePath + "Smart_" + fileGUID)) == false)
                                        {
                                            #region 350 사이즈 이미지 (스마트폰)

                                            //최대 가로, 세로 350
                                            string imgSizeSmall350 = GetThumnailSize(Server.MapPath(filePath + fileGUID), 350, 350);
                                            Bitmap imgFile350 = new Bitmap(Server.MapPath(filePath + fileGUID));
                                            Bitmap thumbnail350 = new Bitmap(imgFile350, imgSizeSmall350.Split(',')[0].ToNumber<int>(), imgSizeSmall350.Split(',')[1].ToNumber<int>());
                                            //작은 이미지 Smart_ 만들기
                                            thumbnail350.Save(Server.MapPath(filePath + "Smart_" + fileGUID), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            imgFile350.Dispose();
                                            thumbnail350.Dispose();

                                            #endregion 350 사이즈 이미지 (스마트폰)
                                        }

                                        imgUrl += "<br><img src=\"" + filePath + "Smart_" + fileGUID + "\"><br>[" + fileName + "]<br><br>";
                                    }
                                }
                            }

                            #endregion 이미지 미리보기
                        }

                        sb.Append(@"</div></li>");

                        AttachList = sb.ToString();
                    }
                }
            }

            //if (optionShowImage == "1" && imgUrl.Length > 0) imgUrl = "<br><br>---- preview the atteched image ---- <br>" + imgUrl;
            if (optionShowImage == "1" && imgUrl.Length > 0) imgUrl = "<br><br>" + imgUrl;
            ImgUrl = imgUrl.ToString();

            // 게시판 정보 가져오기
            Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List bizList =
                new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();

            DataRow drList = bizList.GetBoardList(companyID, categoryID, listID);

            if (drList != null)
            {
                if (drList["OptionAnonymity"].ToString() == "1") //익명
                {
                    string AnonymityName = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "AnonymityName");
                    WriteUserInfo = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                }
                else
                {
                    string name = this.GetLangName(drList["NameBase"].ToString(), drList["NameKo"].ToString(), drList["NameEn"].ToString(),
                                               drList["NameJa"].ToString(), drList["NameFr"].ToString(), drList["NameZhcn"].ToString(), drList["NameZhtw"].ToString(), PreLang);
                    string titleName = this.GetLangName(drList["TitleBase"].ToString(), drList["TitleKo"].ToString(), drList["TitleEn"].ToString(),
                                    drList["TitleJa"].ToString(), drList["TitleFr"].ToString(), drList["TitleZhcn"].ToString(), drList["TitleZhtw"].ToString(), PreLang);
                    string deptName = this.GetLangName(drList["DeptBase"].ToString(), drList["DeptKo"].ToString(), drList["DeptEn"].ToString(),
                                    drList["DeptJa"].ToString(), drList["DeptFr"].ToString(), drList["DeptZhcn"].ToString(), drList["DeptZhtw"].ToString(), PreLang);

                    WriteUserInfo = deptName + " " + titleName + " " + name;
                }

                WriteDate = drList["WriteDate"].ToString();
                ReadCount = drList["ReadCount"].ToString();

                string title = GetTitle(drList["Depth"].ToString().ToNumber<int>(), drList["Title"].ToString());

                //말머리
                string headerName = this.GetLangName(
                        drList["HeaderBase"].ToString().ToNull(), drList["HeaderKo"].ToString().ToNull(),
                        drList["HeaderEn"].ToString().ToNull(), drList["HeaderJa"].ToString().ToNull(),
                        drList["HeaderFr"].ToString().ToNull(), drList["HeaderZhcn"].ToString().ToNull(),
                        drList["HeaderZhtw"].ToString().ToNull(), PreLang);
                headerName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

                title = (headerName.Length > 0) ? headerName + " " + title : title;
                Subject = title;

                #region 본문내용

                string formContent = drList["Content"].ToString().ToEditorXSS();

                /*
                formContent = formContent.Replace("</BODY>", ImgUrl + "</BODY>").Replace("</body>", ImgUrl + "</body>");

                formContent = Server.HtmlDecode(formContent).Replace("<%", "&lt;%");
                formContent = Regex.Replace(formContent, "<script", "<xscript", RegexOptions.IgnoreCase);
                formContent = Regex.Replace(formContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
                formContent = Regex.Replace(formContent, "<form", "<xform", RegexOptions.IgnoreCase);
                formContent = Regex.Replace(formContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
                formContent = Regex.Replace(formContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
                formContent = Regex.Replace(formContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

                //본문안의 Style 제거
                formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);
                */

                //By Freeman 20110721  본문의 Body 안의 내용만 추가 함
                HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                hdoc.LoadHtml(formContent);
                var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                if (bodyNode != null)
                {
                    Content = bodyNode.InnerHtml;
                }
                else
                {
                    Content = formContent;
                }

                #endregion 본문내용

                //조회테이블에 읽기 명단 저장시키기
                SaveListViewers();
            }

            Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List biz = new
                    Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();

            //string next = biz.GetListIDBeforeNext(companyID, mainCID, listID, "Down", UserInfo.UserID, UserInfo.DeptID, searchValue);
            //string before = biz.GetListIDBeforeNext(companyID, mainCID, listID, "Top", UserInfo.UserID, UserInfo.DeptID, searchValue);

            string next = biz.GetListIDBeforeNext(companyID, pCatID.ToNumber<int>(), listID, "Down", UserInfo.UserID, UserInfo.DeptID, searchValue);
            string before = biz.GetListIDBeforeNext(companyID, pCatID.ToNumber<int>(), listID, "Top", UserInfo.UserID, UserInfo.DeptID, searchValue);

            if (next != "0")
            {
                NextCatID = Convert.ToInt32(next.Split(',')[0]);
                NextListID = Convert.ToInt32(next.Split(',')[1]);
            }

            if (before != "0")
            {
                BeforeCatID = Convert.ToInt32(before.Split(',')[0]);
                BeforeListID = Convert.ToInt32(before.Split(',')[1]);
            }

            Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ListEtc etc = new Ws.Data.ListEtc();
            DataSet EtcDs = etc.GetListEtc(companyID, listID);

            if (EtcDs != null)
            {
                if (EtcDs.Tables[0].Rows.Count > 0)
                {
                    if (EtcDs.Tables[0].Rows[0]["Etc8"].ToString() == "1") //최초 글 작성시 외부 다운로드 금지가 체크되어있다면
                    {
                        if (!ChkRole(UserInfo.RolesList) && !CheckIP()) //다운권한도 없고, IP영역도 다르면 다운로드 불가
                        {
                            optionDownload = "1"; //1이면 다운로드 불가
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 회사 전체 Role중에 외부 다운받을 수 있는 RoleID가 본인이 속한 RoleID 에 있는지 체크
        /// </summary>
        /// <param name="arrayList"></param>
        /// <returns></returns>
        private bool ChkRole(ArrayList arrayList)
        {
            Zeospace.DeskPlusMobile.Web.Account.Data.User dalUser = new Account.Data.User();
            DataSet ds = dalUser.GetRole(companyID); //회사 전체 Role가져오기
            bool chk = false; //false는 다운로드 불가능

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["RoleCode"].ToString() == "AllowBoardDownUser") //회사 권한중에 외부다운로드 가능 권한이 있다면
                        {
                            for (int j = 0; j < arrayList.Count; j++) //사용자의 권한중에 해당 권한이 존재 하는지 확인
                            {
                                if (ds.Tables[0].Rows[i]["RoleID"].ToString() == arrayList[j].ToString())
                                {
                                    chk = true;
                                }
                            }
                        }
                    }
                }
            }
            return chk;
        }

        /// <summary>
        /// 게시판 제목
        /// </summary>
        /// <param name="_depth"></param>
        /// <param name="_title"></param>
        /// <returns></returns>
        private string GetTitle(int _depth, string _title)
        {
            string title = _title;
            string arrowURl = "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_arrow.gif' title='' />";

            string space = "";
            for (int i = 0; i < _depth; i++) space += "&nbsp;&nbsp;";

            //답변글 일 경우
            if (_depth > 0) title = arrowURl + " " + title;
            if (space.Length > 0) title = space + title;

            return title;
        }

        /// <summary>
        /// 조회현황을 위하여.
        /// </summary>
        /// <param name="_readCount"></param>
        private void SaveListViewers()
        {
            Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ListViewer dal = new
                    Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ListViewer();

            dal.Add(companyID, categoryID, listID, UserInfo.UserID);
        }

        /// <summary>
        /// IP체크
        /// </summary>
        /// <returns></returns>
        private bool CheckIP()
        {
            Application.Lock();
            Application.Remove("SysCode" + companyID);
            Application.UnLock();
            IPList allowIPList = new IPList();
            DataView dv = XmlDataBaseHelper.GetSysCode(companyID, "AllowBoardDownLoadIP");

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

        public string WriteUserInfo { get; set; }

        public string WriteDate { get; set; }

        public string ReadCount { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public string ShortList { get; set; }

        public int BeforeCatID { get; set; }

        public int NextCatID { get; set; }

        public int BeforeListID { get; set; }

        public int NextListID { get; set; }

        public string CategoryTitle { get; set; }

        public string AttachList { get; set; }

        public string ImgUrl
        {
            get
            {
                object o = ViewState["ImgUrl"];
                if (o == null)
                    return "";
                else
                    return ViewState["ImgUrl"].ToString();
            }
            set
            {
                ViewState["ImgUrl"] = value;
            }
        }

        public string Today { get; set; }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : SSL 사용 유무      
        /// </summary>
        public string httpType
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : 도메인 정보        
        /// </summary>
        public string MobileDOMAIN
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");
            }
        }
    }

    #region IPCheck
    public class IPList
    {
        private ArrayList ipRangeList = new ArrayList();
        private SortedList maskList = new SortedList();
        private ArrayList usedList = new ArrayList();

        public IPList()
        {
            // IP 마스크 목록을 초기화하고 ipRangeList에 IPArrayList를 만듬
            uint mask = 0x00000000;
            for (int level = 1; level < 33; level++)
            {
                mask = (mask >> 1) | 0x80000000;
                maskList.Add(mask, level);
                ipRangeList.Add(new IPArrayList(mask));
            }
        }

        // System.Net.IPAddress 를 사용 할수 있게 uint 로 IP주소를 분석
        private uint parseIP(string IPNumber)
        {
            uint res = 0;
            string[] elements = IPNumber.Split(new Char[] { '.' });
            if (elements.Length == 4)
            {
                res = (uint)Convert.ToInt32(elements[0]) << 24;
                res += (uint)Convert.ToInt32(elements[1]) << 16;
                res += (uint)Convert.ToInt32(elements[2]) << 8;
                res += (uint)Convert.ToInt32(elements[3]);
            }
            return res;
        }

        /// <summary>
        /// IP 리스트에 IP 추가, ex. 10.1.1.1
        /// </summary>
        public void Add(string ipNumber)
        {
            this.Add(parseIP(ipNumber));
        }

        /// <summary>
        /// IP 리스트에 IP 추가 ex. 0x0A010101
        /// </summary>
        public void Add(uint ip)
        {
            ((IPArrayList)ipRangeList[31]).Add(ip);
            if (!usedList.Contains((int)31))
            {
                usedList.Add((int)31);
                usedList.Sort();
            }
        }

        /// <summary>
        /// 지정 범위의 마스크를 사용하여 IP 번호를 추가
        /// ex. 172.16.0.0 255.255.0.0 => 172.16.0.0 - 172.16.255.255
        /// </summary>
        public void Add(string ipNumber, string mask)
        {
            this.Add(parseIP(ipNumber), parseIP(mask));
        }

        /// <summary>
        /// 지정 범위의 마스크를 사용하여 IP 번호를 추가
        /// ex. 0xAC1000 0xFFFF0000 => 172.16.0.0 - 172.16.255.255
        /// </summary>
        public void Add(uint ip, uint umask)
        {
            object Level = maskList[umask];
            if (Level != null)
            {
                ip = ip & umask;
                ((IPArrayList)ipRangeList[(int)Level - 1]).Add(ip);
                if (!usedList.Contains((int)Level - 1))
                {
                    usedList.Add((int)Level - 1);
                    usedList.Sort();
                }
            }
        }

        /// <summary>
        /// 지정 범위의 마스크를 사용하여 IP 번호를 추가
        /// ex. 192.168.1.0/24 => 192.168.1.0 - 192.168.1.255
        /// </summary>
        public void Add(string ipNumber, int maskLevel)
        {
            this.Add(parseIP(ipNumber), (uint)maskList.GetKey(maskList.IndexOfValue(maskLevel)));
        }

        /// <summary>
        /// IP 범위를 추가  범위 확인하여 정상 IP / 마스크 블록 으로 변환
        /// </summary>
        public void AddRange(string fromIP, string toIP)
        {
            this.AddRange(parseIP(fromIP), parseIP(toIP));
        }

        /// <summary>
        /// IP 범위를 추가  범위 확인하여 정상 IP / 마스크 블록 으로 변환
        /// </summary>
        public void AddRange(uint fromIP, uint toIP)
        {
            if (fromIP > toIP)
            {
                uint tempIP = fromIP;
                fromIP = toIP;
                toIP = tempIP;
            }
            if (fromIP == toIP)
            {
                this.Add(fromIP);
            }
            else
            {
                uint diff = toIP - fromIP;
                int diffLevel = 1;
                uint range = 0x80000000;
                if (diff < 256)
                {
                    diffLevel = 24;
                    range = 0x00000100;
                }
                while (range > diff)
                {
                    range = range >> 1;
                    diffLevel++;
                }
                uint mask = (uint)maskList.GetKey(maskList.IndexOfValue(diffLevel));
                uint minIP = fromIP & mask;
                if (minIP < fromIP) minIP += range;
                if (minIP > fromIP)
                {
                    this.AddRange(fromIP, minIP - 1);
                    fromIP = minIP;
                }
                if (fromIP == toIP)
                {
                    this.Add(fromIP);
                }
                else
                {
                    if ((minIP + (range - 1)) <= toIP)
                    {
                        this.Add(minIP, mask);
                        fromIP = minIP + range;
                    }
                    if (fromIP == toIP)
                    {
                        this.Add(toIP);
                    }
                    else
                    {
                        if (fromIP < toIP) this.AddRange(fromIP, toIP);
                    }
                }
            }
        }

        /// <summary>
        /// IP 리스트에 해당 IP가 잇는지 확인  ex. 10.0.0.1
        /// </summary>
        public bool CheckNumber(string ipNumber)
        {
            return this.CheckNumber(parseIP(ipNumber)); ;
        }

        /// <summary>
        /// IP 리스트에 해당 IP가 잇는지 확인  ex) 0x0A000001
        /// </summary>
        public bool CheckNumber(uint ip)
        {
            bool found = false;
            int i = 0;
            while (!found && i < usedList.Count)
            {
                found = ((IPArrayList)ipRangeList[(int)usedList[i]]).Check(ip);
                i++;
            }
            return found;
        }

        /// <summary>
        /// 리스트 초기화
        /// </summary>
        public void Clear()
        {
            foreach (int i in usedList)
            {
                ((IPArrayList)ipRangeList[i]).Clear();
            }
            usedList.Clear();
        }

        /// <summary>
        /// 모든 IP 범위 리스트 포멧
        /// </summary>
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            foreach (int i in usedList)
            {
                buffer.Append("\r\nRange with mask of ").Append(i + 1).Append("\r\n");
                buffer.Append(((IPArrayList)ipRangeList[i]).ToString());
            }
            return buffer.ToString();
        }


    }

    /// <summary>
    /// 동일한 IP 마스크를 저장하기 위한 클래스 
    /// </summary>
    internal class IPArrayList
    {
        private bool isSorted = false;
        private ArrayList ipNumList = new ArrayList();
        private uint ipmask;

        /// <summary>
        /// 생성자
        /// </summary>
        public IPArrayList(uint mask)
        {
            ipmask = mask;
        }

        /// <summary>
        /// 추가
        /// </summary>
        public void Add(uint IPNum)
        {
            isSorted = false;
            ipNumList.Add(IPNum & ipmask);
        }

        /// <summary>
        /// IP 번호 목록이 포함된 범위 내인지 체크
        /// </summary>
        public bool Check(uint IPNum)
        {
            bool found = false;
            if (ipNumList.Count > 0)
            {
                if (!isSorted)
                {
                    ipNumList.Sort();
                    isSorted = true;
                }
                IPNum = IPNum & ipmask;
                if (ipNumList.BinarySearch(IPNum) >= 0) found = true;
            }
            return found;
        }

        /// <summary>
        /// 리스트 삭제
        /// </summary>
        public void Clear()
        {
            ipNumList.Clear();
            isSorted = false;
        }

        /// <summary>
        /// The ToString is overriden to generate a list of the IP numbers
        /// </summary>
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (uint ipnum in ipNumList)
            {
                if (buf.Length > 0) buf.Append("\r\n");
                buf.Append(((int)ipnum & 0xFF000000) >> 24).Append('.');
                buf.Append(((int)ipnum & 0x00FF0000) >> 16).Append('.');
                buf.Append(((int)ipnum & 0x0000FF00) >> 8).Append('.');
                buf.Append(((int)ipnum & 0x000000FF));
            }
            return buf.ToString();
        }

        /// <summary>
        /// The IP mask for this list of IP numbers
        /// </summary>
        public uint Mask
        {
            get
            {
                return ipmask;
            }
        }
    }
    #endregion
}