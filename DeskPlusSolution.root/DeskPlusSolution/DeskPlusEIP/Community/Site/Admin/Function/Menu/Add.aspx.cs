using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Menu
{
    public partial class Add : Base.BasePageCommunitySysop
    {
        #region 변수설정
        public int CommunityID;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Menu.Add).Assembly;	//어셈블리 선언
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
            SetBasic();
            SetAuth();
            LoadMenu();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 기본 메뉴 구성 
        /// </summary>
        private void SetBasic()
        {
            // 메뉴 구성
            ddlBasicMenu.Items.Clear();
            DataView dvMenu = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "CMNMenu");

            for (int i = 0; i < dvMenu.Count; i++)
            {
                string name = this.GetLangName(dvMenu[i]["NameBase"].ToString(), dvMenu[i]["NameKo"].ToString(), dvMenu[i]["NameEn"].ToString(), dvMenu[i]["NameJa"].ToString(),
                                                dvMenu[i]["NameFr"].ToString(), dvMenu[i]["NameZhcn"].ToString(), dvMenu[i]["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                ddlBasicMenu.Items.Add(new ListItem(name, dvMenu[i]["InterfaceCode"].ToString()));
            }

            // 중요도
            ddlBasicWeight.Items.Clear();
            ddlWeight.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                ddlBasicWeight.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlWeight.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 권한설정
        /// </summary>
        private void SetAuth()
        {
            // 메뉴 권한설정
            ddlBasicWriteAuth.Items.Clear();
            ddlBasicReadAuth.Items.Clear();
            ddlWriteAuth.Items.Clear();
            ddlReadAuth.Items.Clear();

            ddlBasicWriteAuth.Items.Add(new ListItem(GetResource("AuthorityNot"), "10"));
            ddlBasicReadAuth.Items.Add(new ListItem(GetResource("AuthorityNot"), "10"));
            ddlWriteAuth.Items.Add(new ListItem(GetResource("AuthorityNot"), "10"));
            ddlReadAuth.Items.Add(new ListItem(GetResource("AuthorityNot"), "10"));

            DataView dvMenu = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "CMNGroup");

            for (int i = 0; i < dvMenu.Count; i++)
            {
                string name = this.GetLangName(dvMenu[i]["NameBase"].ToString(), dvMenu[i]["NameKo"].ToString(), dvMenu[i]["NameEn"].ToString(), dvMenu[i]["NameJa"].ToString(),
                                                dvMenu[i]["NameFr"].ToString(), dvMenu[i]["NameZhcn"].ToString(), dvMenu[i]["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                ddlBasicWriteAuth.Items.Add(new ListItem(name, dvMenu[i]["InterfaceCode"].ToString()));
                ddlBasicReadAuth.Items.Add(new ListItem(name, dvMenu[i]["InterfaceCode"].ToString()));
                ddlWriteAuth.Items.Add(new ListItem(name, dvMenu[i]["InterfaceCode"].ToString()));
                ddlReadAuth.Items.Add(new ListItem(name, dvMenu[i]["InterfaceCode"].ToString()));
            }

            ddlBasicWriteAuth.SelectedIndex = 6;
            ddlBasicReadAuth.SelectedIndex = 6;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 해당 커뮤니티의 메뉴리스트 가져오기
        /// </summary>
        private void LoadMenu()
        {
            DAL.Menu dal = new DAL.Menu();
            DataSet ds = dal.GetMenuList(CurrentUserInfo.CompanyID, CommunityID);

            this.lbxMenu.Items.Clear();

            if (ds != null)
            {
                string Mname = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Mname = this.GetLangName(
                         dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(), dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(), dr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                    this.lbxMenu.Items.Add(new ListItem(Mname, dr["MenuID"].ToString() + "^" + dr["Sequence"].ToString()));

                    if (dr["Type"].ToString() == "7")
                    {
                        ScheduleYN = "Y";
                    }

                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Add");       //"추가";
                cbtnMod.Text = resMng.GetString("Mod");       //"수정";
                cbtnDel.Text = resMng.GetString("Del");       //"삭제";
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
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {

        }

        #endregion

        #region (프로퍼티)

        /// <summary>
        /// 일정관리 유무
        /// </summary>
        public string ScheduleYN
        {
            get { return GetViewStateString("ScheduleYN", ""); }
            set { ViewState["ScheduleYN"] = value; }
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}