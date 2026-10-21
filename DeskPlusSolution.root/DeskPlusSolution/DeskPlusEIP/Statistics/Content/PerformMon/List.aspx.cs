using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Statistics.Web.Content.PerformMon
{
    public partial class List : Base.BasePageStatistics
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string TabMenu = String.Empty;
        public string SelMachineName = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Statistics.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Statistics.Web.Content.PerformMon.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            LoadParam();
            LoadMenu();

            //RecordPerPage = "10";//GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMON, "ListCount", "10");//페이지리스트수

            //ddlListCount.SelectedValue = RecordPerPage;

            //GetListCount();

            //// 최상단 리스트를 가져온다.
            //GetTotalCount();
        }

        private void LoadMenu()
        {
            DAL.PerformMon dal = new DAL.PerformMon();
            DataSet ds = dal.GetMachineName();

            string machineName = String.Empty;
            string tabMenu = String.Empty;
            int isFirst = 1;
            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {

                machineName = dbRow["MachineName"].ToString();
                
                tabMenu = "<li";

                if (ds.Tables[0].Rows.Count == isFirst)
                {
                    tabMenu += " class=\"last\"";
                }
                
                tabMenu += "><a href='#'";
                if (isFirst == 1)
                {
                    tabMenu += " class=\"tab_on\" ";
                    SelMachineName = machineName;
                };
                tabMenu += " onclick=\"List.SetTab('" + machineName + "')\">" + machineName + "</a></li>";

                TabMenu += tabMenu;

                isFirst += 1;
            }
        }

        /// <summary>
        /// 작성자  : 
        /// 작성일자 : 
        /// 작성목적 : 언어 설정하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {


            }
        }

        /// <summary>
        /// 작성자  : 
        /// 작성일자 : 
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {

        }

        #endregion

        #region (함수)
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}