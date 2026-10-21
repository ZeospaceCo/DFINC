using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Site.Main
{
    public partial class Menu : Base.BasePageCommunity
    {
        #region 변수설정
        public int CommunityID = 0;
        public int Grade = 0;
        public string AdminCheck = "N";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Main.Menu).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                LoadData();                
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-10
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
            AdminCheck = GetDecryptParam("adminCheck", "N");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-10
        /// 작성목적 : 초기화 
        /// </summary>
        private void LoadData()
        {
            LoadCommunityInfo();
            LoadMenu();
            LoadMyCommunityList();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-10
        /// 작성목적 : 커뮤니티 정보 (개발이 더 필요..)
        /// </summary>
        private void LoadCommunityInfo()
        {
            DAL.Com dal = new DAL.Com();
            DataTableReader dtr = dal.GetComunifyInfo(CurrentUserInfo.CompanyID, CommunityID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbCommunityName.Text = dtr["Name"].ToString().ToReverseXSS();

                        string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                        string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                        string goDirectURL = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "AlarmTargetURL", "");
                        goDirectURL = string.Format(goDirectURL, baseHttp, fullDomain);

                        goDirectURL = goDirectURL + "&t=gubun|" + dtr["GubunGUID"].ToString();

                        lbGoUrl.Text = goDirectURL;
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-10
        /// 작성목적 : 자신이 가입한 커뮤니티 정보 가져오기
        /// </summary>
        private void LoadMyCommunityList()
        {
            DAL.User dal = new DAL.User();
            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            ddlMyCommunity.Items.Clear();
            ddlMyCommunity.Items.Add(new ListItem(resMng.GetString("GoMyCommunity"), "0"));

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ddlMyCommunity.Items.Add(new ListItem(dtr["Name"].ToString().ToReverseXSS(), dtr["CommunityID"].ToString() + "^" + dtr["Grade"].ToString()));
                    }
                }
            }
        }

        
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 상위 메뉴 구성
        /// </summary>
        private void LoadMenu()
        {
            DAL.Menu dal = new DAL.Menu();

            DataSet ds = dal.GetMenuList(CurrentUserInfo.CompanyID, CommunityID);
            DataView dv = ds.Tables[0].DefaultView;
            
            this.dlTopMenu.DataSource = dv;
            this.dlTopMenu.DataBind();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-10
        /// 작성목적 : 상위 메뉴 구성 - 서브
        /// </summary>
        /// <param name="_menuID"></param>
        /// <param name="_type"></param>
        /// <param name="_nameBase"></param>
        /// <param name="_nameKo"></param>
        /// <param name="_nameEn"></param>
        /// <param name="_nameJa"></param>
        /// <param name="_nameFr"></param>
        /// <param name="_nameZhcn"></param>
        /// <param name="_nameZhtw"></param>
        /// <returns></returns>
        public string MakeMenu(string _menuID, string _type, string _nameBase, string _nameKo, string _nameEn, string _nameJa, string _nameFr, string _nameZhcn, string _nameZhtw)
        {
            DAL.MenuAuth dal = new DAL.MenuAuth();            
            DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, _menuID.ToNumber<int>());

            int ReadAuth = 9;
            int BoaCount = 0;
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();
                        BoaCount = dtr["BoaCount"].ToString().ToNumber<int>();  
                    }
                }
            }

            string tmpString = "";
            string imageName = "";

            if (_type == "1")
            {
                _type = "Board";
                imageName = "cm_cat_noti.gif";
            }
            else if (_type == "2")
            {
                _type = "Board";
                imageName = "cm_cat_board.gif";
            }
            else if (_type == "3")
            {
                _type = "Board";
                imageName = "cm_cat_data.gif";
            }
            else if (_type == "4")
            {
                _type = "Album";
                imageName = "cm_cat_album.gif";
            }
            else if (_type == "7")
            {
                _type = "Schedule";
                imageName = "cm_cat_calendar.gif";
            }            
            else
            {
                imageName = "cm_cat_nocate.gif";
            }

            string tmpName = this.GetLangName(_nameBase, _nameKo, _nameEn, _nameJa, _nameFr, _nameZhcn, _nameZhtw, PreLang).ToReverseXSS();

            if (BoaCount > 0)
            {                
                tmpName = tmpName + " <img src='" + BaseImgPath + "/btn/i_new.gif' alt='' style='padding:0px 0px 2px 0px;' /></span>";
                
            }

            if (Grade <= ReadAuth)
            {
                if (_type == "5")
                {
                    tmpString = "<li><div class='cm_barA' style='clear:both;width:110pt''></div></li>";
                }
                else if (_type == "6")
                {
                    tmpString = "<li><span class='tit'><b>[" + tmpName + "]</b></span></li>";
                }
                else
                {
                    tmpString = "<li><img src='" + BaseImgPath + "/Community/" + imageName + "' alt='' /><a onclick=\"Menu.GoListPage('" + _menuID + "', '" + _type + "')\"><span class='tit'>" + tmpName + "</span></a></li>";
                }
            }
            else
            {
                if (_type == "5")
                {
                    tmpString = "<li><div class='cm_barA' style='clear:both;width:110pt''></div></li>";
                }
                else if (_type == "6")
                {
                    tmpString = "<li><span class='tit'><b>[" + tmpName + "]</b></span></li>";
                }
                else
                {
                    tmpString = "<li><img src='" + BaseImgPath + "/Community/" + imageName + "' alt='' /><a onclick=\"Menu.NoAuth()\"><span class='tit'>" + tmpName + "</span></a></li>";
                }
            }
            return tmpString;

        }

        #endregion

        #region Property (프로퍼티)

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
    }
}