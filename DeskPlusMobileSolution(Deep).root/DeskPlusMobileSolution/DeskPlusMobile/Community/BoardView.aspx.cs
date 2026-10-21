using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using HtmlAgilityPack;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Community
{
    public partial class BoardView : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        private int companyID;
        public int communityID;
        public int menuID;
        public int boardID;
        public int pageNo = 1;
        public string searchValue;
        public bool readAuth = false;
        public bool writeAuth = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Community.BoardView).Assembly;	//어셈블리 선언
            #endregion

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
                GetMenuAuth();

                if (readAuth)
                {
                    LoadData();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadParam()
        {
            companyID = UserInfo.CompanyID;
            communityID = GetDecryptParam("CommunityID", "0").ToNumber<int>();
            menuID = GetDecryptParam("MenuID", "0").ToNumber<int>();
            boardID = GetDecryptParam("BoardID", "0").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-10
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
        /// 
        /// </summary>
        private void LoadData()
        {
            // 1. 첨부파일 가져오기
            string imgUrl = "";

            Zeospace.DeskPlusMobile.Web.Community.Ws.Data.BoaAttach bizAttachFiles = new Ws.Data.BoaAttach();

            var docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "DocStreamYN");
            DataSet dsDSList = bizAttachFiles.GetDSFiles(companyID, communityID, menuID, boardID, "CMN_ATTACH");

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
                    string fileLength = dr["FileLength"].ToString().ToNumber<int>().ToString("#,###");
                    string url = (fileGUID != "") ? filePath + fileGUID : filePath + fileName;  // 실제 저장된 파일

                    if (fileID.Length > 0)
                    {
                        if (fileName.IndexOf("'") > -1)
                        {
                            fileName = fileName.Replace("'", "");
                        }

                        sb.Append(@"<div style='word-wrap:break-word;'><a class='linkDiv' href='#2' onclick=BoardView.goDS('" + fileID + "')>" + fileName + "</a>");
                        sb.Append(@"<span class='iste'>&nbsp;" + fileLength + " byte</span></div>");
                    }
                    else
                    {
                        if (fileName.IndexOf("'") > -1)
                        {
                            fileName = fileName.Replace("'", "");
                        }

                        sb.Append(@"<div style='word-wrap:break-word;'><a class='linkDiv' href='#2' onclick=BoardView.goAttach('" + url + "','" + fileName.Replace(" ", "") + "')>" + fileName + "</a>");
                        sb.Append(@"<span class='iste'>&nbsp;" + fileLength + " byte</span></div>");
                    }
                }

                sb.Append(@"</div></li>");

                AttachList = sb.ToString();
            }
            else
            {
                DataSet ds = bizAttachFiles.GetAttachFiles(companyID, communityID, menuID, boardID);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append(@"<li class='contentblock'><div class='title flxedA'>");
                        sb.Append(resMng.GetString("Attach"));

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string fileName = dr["FileName"].ToString();
                            string filePath = dr["FilePath"].ToString().Replace("/DeskPlusEIP/FileData/", FilePath + "/");
                            string fileGUID = dr["FileGUID"].ToString();
                            string fileLength = dr["FileLength"].ToString().ToNumber<int>().ToString("#,###");
                            string url = (fileGUID != "") ? filePath + fileGUID : filePath + fileName;  // 실제 저장된 파일

                            if (fileName.IndexOf("'") > -1)
                            {
                                fileName = fileName.Replace("'", "");
                            }

                            sb.Append(@"<div style='word-wrap:break-word;'><a class='linkDiv' href='#2' onclick=BoardView.goAttach('" + url + "','" + fileName.Replace(" ", "") + "')>" + fileName + "</a>");
                            sb.Append(@"<span class='iste'>&nbsp;" + fileLength + " byte</span></div>");
                        }

                        sb.Append(@"</div></li>");

                        AttachList = sb.ToString();
                    }
                }
            }

            ImgUrl = imgUrl.ToString();

            // 2. 게시판 정보 가져오기
            Ws.Data.Boa bizBoard = new Ws.Data.Boa();
            DataSet dsBoard = bizBoard.GetBoardList(companyID, communityID, menuID, boardID);

            if (dsBoard != null)
            {
                foreach (DataRow dr in dsBoard.Tables[0].Rows)
                {
                    string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                               dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                    string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                    dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                    string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                    dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);


                    WriteUserInfo = deptName + " " + titleName + " " + name;

                    if (WriteUserInfo.Trim() == "")
                    {
                        WriteUserInfo = dr["WriteUserName"].ToString();
                    }

                    WriteDate = dr["WriteDate"].ToString();
                    WriteUserID = dr["WriteUserID"].ToString().ToNumber<int>();
                    Subject = dr["Title"].ToString();
                    ReadCount = dr["ReadCount"].ToString();
                    RecomCount = dr["RecomCount"].ToString();

                    #region 본문내용
                    string formContent = dr["Content"].ToString();

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

                    #endregion
                }
            }

            // 3. 조회자 추가 
            int CheckPoint = SaveListViewers();

            // 4. 포인트 추가
            if (CheckPoint == 0)
            {
                AddPoint();
            }

            // 5. 댓글
            Ws.Data.BoaShortReply bizReply = new Ws.Data.BoaShortReply();
            DataSet dsReply = bizReply.GetShortReply(companyID, communityID, menuID, boardID);

            if (dsReply != null)
            {
                if (dsReply.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(@"<li class='contentblock bg-white'><div class='title'>");
                    sb.Append(@"<div  class='rc'>");
                    foreach (DataRow dr in dsReply.Tables[0].Rows)
                    {
                        string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                               dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                        string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                        dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                        string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                        dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);


                        string userInfo = deptName + " " + titleName + " " + name;

                        if (userInfo.Trim() == "")
                        {
                            userInfo = dr["UserName"].ToString();
                        }

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dr["WriteDate"].ToString());
                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                        string showNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                        #endregion

                        sb.Append(@"<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>");
                        sb.Append(@"<tr><td align=""center"">&nbsp;</td><td class='lif'>");
                        sb.Append(userInfo + "&nbsp;| " + dr["WriteDate"].ToString());
                        sb.Append("<br />");
                        sb.Append(dr["Content"].ToString() + " " + showNew);
                        sb.Append("</td>");
                        sb.Append(@"<td>&nbsp;</td></tr><tr>");
                        sb.Append(@"<td height=""1"" colspan=""3"" align=""center"" bgcolor=""#CCCCCC""></td></tr></table>");

                    }

                    sb.Append(@"</div>");
                    sb.Append(@"</div></li>");

                    ShortList = sb.ToString();
                }
            }
            dsReply = null;

            // 6. 다음글, 이전글 정보 가져오기
            string next = bizBoard.GetBoardIDBeforeNext(companyID, communityID, menuID, boardID, "DOWN", searchValue);
            string before = bizBoard.GetBoardIDBeforeNext(companyID, communityID, menuID, boardID, "TOP", searchValue);

            if (next != "0")
            {
                NextBoardID = next.ToNumber<int>();
            }

            if (before != "0")
            {
                BeforeBoardID = before.ToNumber<int>();
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-12
        /// 작성목적 : 조회수 추가
        /// </summary>
        /// <returns></returns>
        private int SaveListViewers()
        {
            if (companyID > 0 && communityID > 0 && menuID > 0 && boardID > 0)
            {
                Ws.Data.BoaView bizList = new Ws.Data.BoaView();
                Ws.BE.BoaViewInfo info = new Ws.BE.BoaViewInfo();

                info.CompanyID = companyID;
                info.CommunityID = communityID;
                info.MenuID = menuID;
                info.BoardID = boardID;
                info.UserID = UserInfo.UserID;
                info.UserName = UserInfo.Name;

                return bizList.Add(info);
            }

            return -1;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-12
        /// 작성목적 : 포인트 추가 (1.글쓰기 2.조회 3.추천 4. 댓글 5.답변 6.방문)
        /// </summary>
        private void AddPoint()
        {
            Ws.BE.PointInfo info = new Ws.BE.PointInfo();

            info.CompanyID = companyID;
            info.CommunityID = communityID;
            info.UserID = UserInfo.UserID;
            info.Type = 2;
            info.Point = 0;

            Ws.Data.Point point = new Ws.Data.Point();
            point.AddPoint(info, menuID);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-12
        /// 작성목적 : 쓰기 권한 체크
        /// </summary>
        private void GetMenuAuth()
        {
            Ws.Data.MenuAuth dal = new Ws.Data.MenuAuth();
            DataSet ds = dal.GetAuth(UserInfo.CompanyID, communityID, menuID, UserInfo.UserID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["ReadAuth"].ToString().ToNumber<int>() >= dr["Grade"].ToString().ToNumber<int>())
                    {
                        readAuth = true;
                    }

                    if (dr["WriteAuth"].ToString().ToNumber<int>() >= dr["Grade"].ToString().ToNumber<int>())
                    {
                        writeAuth = true;
                    }
                }
            }
        }

        public string WriteUserInfo { get; set; }
        public string WriteDate { get; set; }
        public string ReadCount { get; set; }
        public string RecomCount { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string ShortList { get; set; }
        public int WriteUserID { get; set; }
        public int BeforeBoardID { get; set; }
        public int NextBoardID { get; set; }
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
}