using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text;

namespace Zeospace.DeskPlus.Community.Web.Site.Main
{
    public partial class Content : Base.BasePageCommunity
    {
        #region 변수설정  
        public int CommunityID = 0;
        public int Grade = 0;
        private int  Check = 0;
        private string MenuList = string.Empty;     // 시작화면에 보여질 메뉴ID
        private string MenuName = string.Empty;     //  시작화면에 보여질 메뉴이름
        
        DataTableReader dtrBoard = null;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Main.Content).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadComDetail();
            LoadAuth();

            // 시작화면에 보일 메뉴의 권한이 있을 때
            if (Check > 0) LoadBoard();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 커뮤니티 상세정보 가져오기
        /// </summary>
        private void LoadComDetail()
        {
            DAL.Com dal = new DAL.Com();
            DataTableReader dtr = dal.GetComEtc(CurrentUserInfo.CompanyID, CommunityID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbContent.Text = dtr["AdminDescription"].ToString().ToEditorXSS();

                        DateTime dt;

                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            ComInfo1.InnerText = "Since " + DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            ComInfo1.InnerText = string.Empty;
                                                
                        ComInfo2.InnerText = GetResource("Welcome2") + " : " + dtr["UserCount"].ToString();
                        ComInfo3.InnerText = GetResource("Welcome3") + " : " + dtr["VisitCount"].ToString();
                    }
                }
            }                       
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-14
        /// 작성목적 : 시작화면에 보여줄 메뉴 가져오기
        /// </summary>
        private void LoadBoard()
        {
            StringBuilder sb = new StringBuilder();
            DAL.Boa dal = new DAL.Boa();

            string[] arrMenu = MenuList.Split('┐');     // 메뉴ID^읽기권한(1:권한있음, 권한없음:0)
            
            for (int i = 0; i < Check; i++)
            {
                sb.Append("<div class='cont'>");
                sb.Append("<h2>" + MenuName.Split('┐')[i] + "</h2>");   // 메뉴 이름
                sb.Append("<table class='table_typeA'>");
                sb.Append(LoadBoardHeader());

                // 메뉴 리스트 가져오기
                dtrBoard = dal.GetBoardList(CurrentUserInfo.CompanyID, CommunityID, arrMenu[i].ToNumber<int>());
                sb.Append(LoadBoardContent());                

                sb.Append("</table>");
                sb.Append("</div>");
            }
            
            plList.Controls.Add(new LiteralControl(sb.ToString()));
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-14
        /// 작성목적 : 게시판 헤더
        /// </summary>
        /// <returns></returns>
        private string LoadBoardHeader()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<caption></caption>");
            sb.Append("<colgroup>");
            sb.Append("<col width='60px' />");
            sb.Append("<col />");
            sb.Append("<col width='60px' />");
            sb.Append("<col width='60px' />");
            sb.Append("<col width='100px' />");
            sb.Append("<col width='100px' />");
            sb.Append("</colgroup>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>" + GetResource("Number") + "</th>");
            sb.Append("<th>" + GetResource("Title") + "</th>");
            sb.Append("<th>" + GetResource("AttachFile") + "</th>");
            sb.Append("<th>" + GetResource("ViewCount") + "</th>");
            sb.Append("<th>" + GetResource("Registrant") + "</th>");
            sb.Append("<th>" + GetResource("RegDate") + "</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            return sb.ToString();
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-14
        /// 작성목적 : 게시판 내용
        /// </summary>
        /// <returns></returns>
        private string LoadBoardContent()
        {
            StringBuilder sb = new StringBuilder();

            if (dtrBoard != null)
            {
                if (dtrBoard.HasRows)
                {
                    while (dtrBoard.Read())
                    {
                        BE.BoaInfo info = new BE.BoaInfo();
                        info.ROW = dtrBoard["ROW"].ToString();
                        info.MenuID = dtrBoard["MenuID"].ToString().ToNumber<int>();
                        info.BoardID = dtrBoard["BoardID"].ToString().ToNumber<int>();
                        info.WriteUserID = dtrBoard["WriteUserID"].ToString().ToNumber<int>();
                        info.Depth = dtrBoard["Depth"].ToString().ToNumber<int>();

                        info.Attach = dtrBoard["AttachCount"].ToString().ToNumber<int>();
                        info.Title = dtrBoard["Title"].ToString();
                        info.ReadCount = dtrBoard["ReadCount"].ToString();
                        info.ReplyCount = dtrBoard["ReplyCount"].ToString();

                        #region 등록자
                        info.WriteUserName = GetLangName(
                                    dtrBoard["NameBase"].ToString().ToNull(), dtrBoard["NameKo"].ToString().ToNull(),
                                    dtrBoard["NameEn"].ToString().ToNull(), dtrBoard["NameJa"].ToString().ToNull(),
                                    dtrBoard["NameFr"].ToString().ToNull(), dtrBoard["NameZhcn"].ToString().ToNull(),
                                    dtrBoard["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                        if (info.WriteUserName.Trim() == "")
                        {
                            info.WriteUserName = dtrBoard["WriteUserName"].ToString();
                        }
                        #endregion

                        #region 등록일
                        DateTime dt;
                        if (DateTime.TryParse(dtrBoard["WriteDate"].ToString(), out dt))
                            info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.WriteDate = "";
                        #endregion

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dtrBoard["WriteDate"].ToString());
                        DateTime todayDate = System.DateTime.Now;
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                        info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                        #endregion

                        #region 답글일 경우 제목에 화살표 추가

                        string space = "";
                        for (int i = 0; i < info.Depth; i++) space += "&nbsp;&nbsp;";

                        if (info.Depth > 0) info.Title = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />" + " " + info.Title;
                        if (space.Length > 0) info.Title = space + info.Title;

                        #endregion

                        sb.Append("<tr>");
                        sb.Append("<td style='width:50px;text-align:center;'>" + info.ROW + "</td>");
                        sb.Append("<td><a onclick=\"Content.View('" + CommunityID + "', '" + info.MenuID + "','" + info.BoardID + "');\" return false; style='cursor:pointer'>" + info.Title + "</a>");
                        if (info.ReplyCount.ToNumber<int>() > 0)
                        {
                            //sb.Append("&nbsp;<img src='" + BaseImgPath + "/Icon/icon_comment.gif' action='' style='cursor:pointer' CommunityID='" + CommunityID + "' MenuID='" + info.MenuID + "' BoardID ='" + info.BoardID + "' />");
                            sb.Append("&nbsp;<a onclick=\"Content.PopupShort('" + CommunityID + "','" + info.MenuID + "','" + info.BoardID + "')\" ><span class=\"icon_Comment_view\">(" + info.ReplyCount + ")</span></a>");
                        }
                        if (info.ShowNew == "Y")
                            sb.Append(" <img src='" + BaseImgPath + "/btn/i_new.gif' /></td>");
                        else
                            sb.Append("</td>");

                        if (info.Attach > 0)
                        {
                            //sb.Append("<td style='width:60px;text-align:center;'><img src='" + BaseImgPath + "/Icon/attach.gif' action='attach' style='cursor:pointer' CommunityID='" + CommunityID + "' MenuID='" + info.MenuID + "' BoardID ='" + info.BoardID + "' /></td>");
                            sb.Append("<td style='width:60px;text-align:center;'><i class='fas fa-arrow-alt-circle-down' action='attach' style='cursor:pointer' CommunityID='" + CommunityID + "' MenuID='" + info.MenuID + "' BoardID ='" + info.BoardID + "'></i></td>");
                        }
                        else
                        {
                            sb.Append("<td></td>");
                        }

                        sb.Append("<td style='width:60px;text-align:center;'><a onclick=\"Content.ReadList('" + CommunityID + "','" + info.MenuID + "','" + info.BoardID + "')\" >" + info.ReadCount + "</a></td>");
                        sb.Append("<td style='width:120px;text-align:center;'><a onclick=\"AccountInfo('" + info.WriteUserID + "');\" return false; style='cursor:pointer'>" + info.WriteUserName + "</a></td>");
                        sb.Append("<td style='width:100px;text-align:center;'>" + info.WriteDate + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan='6' class='center'>" + GetResource("NoContent") + "</td>");
                    sb.Append("</tr>");
                }
            }
            else   //권한 없을 때
            {
                sb.Append("<tr>");
                sb.Append("<td colspan='6' class='center'>" + GetResource("NoContent") + "</td>");
                sb.Append("</tr>");
            }
            return sb.ToString();
        }


        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-15
        /// 작성목적 : Type=1,2,3 중 사용자의 메뉴별 권한 가져오기
        /// </summary>
        private void LoadAuth()
        {
            DAL.User dal = new DAL.User();
            DataTableReader dtrUser = dal.GetMyAuth(CurrentUserInfo.CompanyID, CommunityID, Grade);

            if (dtrUser != null)
            {
                if (dtrUser.HasRows)
                {
                    while (dtrUser.Read())
                    {
                        MenuList += dtrUser["MenuID"].ToString() + "┐";
                        MenuName += this.GetLangName(
                                    dtrUser["NameBase"].ToString().ToNull(), dtrUser["NameKo"].ToString().ToNull(),
                                    dtrUser["NameEn"].ToString().ToNull(), dtrUser["NameJa"].ToString().ToNull(),
                                    dtrUser["NameFr"].ToString().ToNull(), dtrUser["NameZhcn"].ToString().ToNull(),
                                    dtrUser["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS() + "┐";
                        Check++;
                    }
                }
            }

            MenuList = MenuList.TrimEnd('┐');
            MenuName = MenuName.TrimEnd('┐');
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                userInfo.InnerText = CurrentUserInfo.Name + GetResource("Welcome1");                
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)

        /// <summary>
        /// 커뮤니티 CSS 파일 
        /// </summary>
        public string CommunityPath
        {
            get
            {                
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string ComPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Community/" + CommunityID;
                return ComPath;
            }
        }


        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}