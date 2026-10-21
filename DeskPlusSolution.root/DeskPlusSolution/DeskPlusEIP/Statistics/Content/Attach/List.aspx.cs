using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Statistics.Web.Content.Attach
{
    public partial class List : Base.BasePageStatistics
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string TabMenu = "";
        //public string Order = string.Empty;       // sort 
        //public string SearchTerm = string.Empty;  //검색기간구분자
        //public string SearchTermValue = string.Empty;   //검색기간 시작,종료값 
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Statistics.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Statistics.Web.Content.Attach.List).Assembly;	//어셈블리 선언
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

            RecordPerPage = "10";//GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMON, "ListCount", "10");//페이지리스트수

            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();
            GetTotalCount();
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 언어 설정하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            //"".Replace("\r\n", "<BR>")
            if (!this.resMng.Equals(null))
            {                
                cbtnSearch.Text = this.resMng.GetString("Search");   // "검색";
                cbtnExcel.Text = this.resMng.GetString("StatExcel");


                #region 연도 바인딩
                ddlSearchYear.Items.Clear();
                DAL.Attach Attach = new DAL.Attach();
                DataTableReader dtr = Attach.GetYear(CurrentUserInfo.CompanyID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            ddlSearchYear.Items.Add(new ListItem(dtr["WriteYear"].ToString(), dtr["WriteYear"].ToString()));
                        }
                    }
                }

                if (ddlSearchYear.Items.Count == 0)
                {
                    ddlSearchYear.Items.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
                }
                ddlSearchYear.SelectedValue = DateTime.Now.Year.ToString();
                #endregion

                ddlSearchField.Items.Clear();
                this.ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ModuleNM"), "GroupGB"));    //UserName

                this.ddlSearchValue.Items.Add(new ListItem("전체", ""));
                this.ddlSearchValue.Items.Add(new ListItem("웹하드", "WebHard"));
                this.ddlSearchValue.Items.Add(new ListItem("업무일지", "Report"));
                this.ddlSearchValue.Items.Add(new ListItem("일정관리", "Pims"));
                this.ddlSearchValue.Items.Add(new ListItem("게시판", "Board"));
                this.ddlSearchValue.Items.Add(new ListItem("업무협업", "Coop"));
                this.ddlSearchValue.Items.Add(new ListItem("전자결재", "Approval"));
                this.ddlSearchValue.Items.Add(new ListItem("자원관리", "ResourceManager"));

            }
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
            Order = (Order.Length == 0) ? "userID  " : Order;
        }

        /// <summary>
        /// 상단 전체 토탈 카운트를 가져온다.
        /// </summary>
        private void GetTotalCount()
        {
            BE.AttachInfo AttachInfo = new BE.AttachInfo();
            DAL.Attach Attach = new DAL.Attach();

            AttachInfo.CompanyID = CurrentUserInfo.CompanyID;
            AttachInfo.TargetYear = ddlSearchYear.SelectedValue.ToString();

            DataTableReader dtr = Attach.GetTotalTopList(AttachInfo);

            // 가벼운 사유라 할지라도... 1Row 가져오는데. 이 방식은.. seo 
            // ds 나 dt 를 해도 상관 없지 않을까..
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lblFileDownLoadTotal.Text = dtr["FileDownTotalCount"].ToString().ToNumber<int>().ToString("#,##0");
                        lblFileLengthTotal.Text = GetSize(dtr["FileLengthTotal"].ToString(), 0).ToNumber<int>().ToString("#,##0") + " MB";
                    }
                }
            }

        }

        /// </summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 :해당 카테고리 총 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// <summary>
        private void GetListCount()
        {

            BE.AttachInfo AttachInfo = new BE.AttachInfo();
            DAL.Attach Attach = new DAL.Attach();

            AttachInfo.CompanyID = CurrentUserInfo.CompanyID;
            AttachInfo.TargetYear = ddlSearchYear.SelectedValue.ToString();

            //string srhField = ddlSearchField.SelectedValue;
            //string srhValue = ctbSearch.Text;

            switch (TabMenu.ToUpper())
            {
                case "ATTACHM":
                    TotalCount = Attach.GetPageMCount(AttachInfo).ToString();
                    break;
                case "ATTACHU":
                    TotalCount = Attach.GetPageUCount(AttachInfo).ToString();
                    break;
                case "ATTACHD":
                    TotalCount = Attach.GetPageDCount(AttachInfo).ToString();
                    break;
                case "ATTACHF":
                    TotalCount = Attach.GetPageFCount(AttachInfo).ToString();
                    break;
                case "ATTACHMFL":
                    TotalCount = Attach.GetPageMFLCount(AttachInfo).ToString();
                    break;
                default:
                    TotalCount = Attach.GetPageMCount(AttachInfo).ToString();
                    break;
            }            
        }
        #endregion

        #region (함수)

        /// <summary>
        /// byte를 megabyte로 변경
        /// </summary>
        /// <param name="pSize"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public string GetSize(string pSize, int pi)
        {

            if (pSize == "")
                return "0";

            double dSize = double.Parse(pSize);
            string returnStr = string.Empty;

            dSize /= 1024.0;
            dSize /= 1024.0;

            if (pi == 1)
                returnStr = string.Format("{0:F1}", dSize);
            else if (pi == 2)
                returnStr = string.Format("{0:F2}", dSize);
            else if (pi == 3)
                returnStr = string.Format("{0:F3}", dSize);
            else
                returnStr = string.Format("{0}", Convert.ToInt32(dSize));

            return returnStr;
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
