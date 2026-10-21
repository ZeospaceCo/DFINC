using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Pim.DAL;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{

    public partial class ScheduleMain : Base.BasePagePim
    { 
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID = 0;//
        public int UserID = 0;//
        public string pageinfo = "";
        public string SelDate = "";
        
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ScheduleMain).Assembly;	//어셈블리 선언
            #endregion

            #region 초기 데이타
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                SettingControl();
                LoadData();
            }
            CbtnDefault.Visible = false;
        }

        #endregion

        #region Method (함수)

        

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //관리자 버튼

            CbtnAdmin.Visible = false;
            #region admin 관리자 체크
            CbtnAdmin.Visible = (AccessAuth) ? true : false;
            #endregion 

        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            //CatID = GetDecryptParam("catID", "0").ToNumber<int>();
             
            pageinfo = GetParm("p", "");
            SelDate = GetDecryptParam("Date", "");
            

            if (PageScope.Length == 0)
            {
                //string selpage = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "DefaultView", "");
                string selpage = "";
                Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();
                DataTableReader dr = env.GetPimTabMenu(CompanyID, UserID);

                if (dr != null)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (dr["PimTabMenu"].ToString() == "")
                            {
                                selpage = "day";
                            }
                            else
                            {
                                selpage = dr["PimTabMenu"].ToString();
                            }
                        }
                    }
                }

                switch (selpage)
                {
                    case "day":
                        selpage = "ListDay.aspx";
                        break;
                    case "week":
                        selpage = "ListWeek.aspx";
                        break;
                    case "month":
                        selpage = "ListMonth.aspx";
                        break;
                    default:
                        break;
                }
                if (SelDate != "") //메인 포틀릿에서 넘어온 값이 있으면 무조건 일간페이지
                {
                    selpage = "ListDay.aspx";
                }

                PageScope = selpage;// AppSettingHelper.GetAppSetting(AppSettingType.PIM, "DefaultView", "");
            }

            if (Opentype == "")
            {
                Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();
                DataTableReader dr = env.GetPimCheckMenu(CompanyID, UserID);

                if (dr != null)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Opentype = dr["PimCheckMenu"].ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnAdmin.Text = GetResource("ADMIN");
                CbtnAnniv.Text = GetResource("AnnivControl");
                CbtnShareObj.Text = GetResource("ShareObj");
                CbtnAllShare.Text = GetResource("AllShareView");
                CbtnUserSche.Text = GetResource("AllShareView");

                cbtnLinkPim.Text = GetResource("OutSync");
                CbtnDefault.Text = GetResource("BasicSetting");
                cbtnToDoAdd.Text = GetResource("TodoAdd");
                cbtnAdd.Text = GetResource("AddSchedule");
                CbtnPrint.Text = GetResource("PRINT");
                 

                CbtnAllShare.IsDisabled = "";
            }
        }


        private void SettingControl()
        { 

            //ddlSearchKind
            ListItem lt1 = new ListItem(GetResource("SEARCH1"), "0");
            ListItem lt2 = new ListItem(GetResource("SEARCH2"), "WEEK");
            ListItem lt3 = new ListItem(GetResource("SEARCH3"), "MON");
            ListItem lt4 = new ListItem(GetResource("SEARCH4"), "U");

            ddlSearchKind.Items.Add(lt1);
            ddlSearchKind.Items.Add(lt2);
            ddlSearchKind.Items.Add(lt3);
            ddlSearchKind.Items.Add(lt4);

            ListItem ls2 = new ListItem(GetResource("Title"), "Title");
            ListItem ls3 = new ListItem(GetResource("Name"), "Name");
            ddlValue.Items.Add(ls2);
            ddlValue.Items.Add(ls3);

        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }        
  
     
}