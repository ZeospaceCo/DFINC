using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;


namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class ListTotal : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        public string TotalCount = "0";

        public string Searchkey = "";
        public string SearchValue = "";
        public int CurrentPage = 1;
        public string Order = "";
        public string Starttime = "";
        public string Endtime = "";
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ListTotal).Assembly;	//어셈블리 선언
            #endregion

            #region 기본정보세팅
            UserID = CurrentUserInfo.UserID;
            CompanyID = CurrentUserInfo.CompanyID;
            #endregion

             

            if (!IsPostBack)
            {
                LoadParam();               

                SetLanguage();
                SettingControl();
                LoadData();
            }


        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            //GetListCount();
        }

        private void LoadParam()
        {

            Searchkey = GetDecryptParam("SearchKind", "");
            SearchValue = GetDecryptParam("SerachValue", "");            
            Starttime = GetDecryptParam("s_StartDate", "");
            Endtime = GetDecryptParam("s_EndDate", "");
            
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnExcel.Text = GetResource("ExecelDown");

                CbtnMySchedule.Text = GetResource("MYSCHEDULE");
                CbtnTarget.Text = GetResource("InqueryObjSel");
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
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
            

        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)





        #endregion
    }
}