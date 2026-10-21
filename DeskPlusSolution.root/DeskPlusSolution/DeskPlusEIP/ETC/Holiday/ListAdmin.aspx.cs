using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.ETC.Web.Holiday
{
    public partial class ListAdmin : Base.BasePageETC
    {
        #region 변수설정        
        public string StartYear = string.Empty; //휴가기간 시작 년도  
        public string StartMonth = string.Empty; //휴가기간 시작 월 
        public string EndYear = string.Empty; //휴가기간 종료 년도
        public string EndMonth = string.Empty; //휴가기간 종료 월
        public string StartDate = string.Empty; //휴가기간 시작 년월
        public string EndDate = string.Empty; //휴가기간 종료 년월
        
        public int CurrentPage = 0;                
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어       
        public string Order = string.Empty; //sort
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; // 리스트 개수
        public string MenuID = string.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Holiday.ListAdmin).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }


        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 김형석
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            // 근태관리에서 링크될때
            if (MenuID == "HoliAdmin")
            {
                ltlHeader.Text = this.resMng.GetString("HoliAdmin"); // 연차관리
            }

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Holiday", CurrentUserInfo.UserID, "Admin");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage;

            GetDateType();

            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자  : 김형석
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {                
                cbtnSearch.Text = this.resMng.GetString("Search");
                cbtnPost.Text = this.resMng.GetString("Save");
                ltlHeader.Text = this.resMng.GetString("AdminListTitle");                
                cbtnExcel.Text = resMng.GetString("ExecelDown");
                cbtnExcelUpload.Text = resMng.GetString("ExcelUpload");
                cbtnSample.Text = resMng.GetString("SampleDown");

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "ManyName")); //성명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID")); //로그인ID
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "ManyDeptName")); //부서명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TitleName"), "ManyTitleName")); //직급명
            }
        }

        /// <summary>
        /// 작성자  : 김형석
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();            
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = (Order.Length == 0) ? "TitleCodeOrder asc, DutyCodeOrder asc, UserDeptOrder desc , DeptOrder asc, WriteDate desc" : Order;  
                        
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            MenuID = GetDecryptParam("menuID", "");
        }

        /// <summary>
        /// 작성자   : 김형석
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.HoliAdmin dal = new DAL.HoliAdmin();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, SearchField, SearchValue).ToString();
        }        

        /// <summary>  
        /// 작성자  : 김형석
        /// 휴가 기간 가져오기
        /// </summary>
        private void GetDateType()
        {            
            string date = AppSettingHelper.GetAppSetting(AppSettingType.ETC, "HolidayDateType", "");

            if (date.Split(',').Length == 4)
            {                
                StartYear = date.Split(',')[0];
                StartMonth = (date.Split(',')[1].ToNumber<int>() < 10) ? "0" + date.Split(',')[1] : date.Split(',')[1];
                EndYear = date.Split(',')[2];
                EndMonth = (date.Split(',')[3].ToNumber<int>() < 10) ? "0" + date.Split(',')[3] : date.Split(',')[3];
            }
            else
            {                
                StartYear = Convert.ToString(DateTime.Now.Year);
                StartMonth = "01";
                EndYear = Convert.ToString(DateTime.Now.Year);
                EndMonth = "12";
            }           
            
            StringBuilder sb = new StringBuilder();
            string selData = string.Empty;

            string NowYear = DateTime.Now.Year.ToString();
            string NowMonth = (Convert.ToInt32(DateTime.Now.Month.ToString()) < 10) ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            string NowYM = NowYear + NowMonth;

            for (int i = StartYear.ToNumber<int>(); i <= DateTime.Now.Year + 5; i++)
            {           
                sb.Append(DateFormat(i.ToString() + "-" + StartMonth));
                sb.Append(" ~ ");
                sb.Append(DateFormat(EndYear + "-" + EndMonth));

                ddlYearField.Items.Add(new ListItem(sb.ToString(), i.ToString() + StartMonth + "," + EndYear + EndMonth));
                
                if ((NowYM.ToNumber<int>() >= (i.ToString() + StartMonth).ToNumber<int>()) && (NowYM.ToNumber<int>() <= (EndYear + EndMonth).ToNumber<int>()))
                {
                    selData = i.ToString() + StartMonth + "," + EndYear + EndMonth;

                    StartDate = i.ToString() + StartMonth;
                    EndDate = EndYear + EndMonth;
                }
                                
                EndYear = (EndYear.ToNumber<int>() + 1).ToString();
                
                sb.Clear();
            }

            ddlYearField.SelectedValue = selData;
        }

        /// <summary>
        /// 다국어 날짜 세팅
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string DateFormat(string _date)
        {
            string[] arrDate = _date.Split('-');
            string ParsingDate = "";
            switch (PreLang) {
                case "ko":
                    //"yy-mm";
                    ParsingDate = arrDate[0] + "-" + arrDate[1];            
                    break;
                case "en":
                    //"mm/yy";
                    ParsingDate = arrDate[1] + "/" + arrDate[0];
                    break;
                case "en-gb":
                    //"mm/yy";
                    ParsingDate = arrDate[1] + "/" + arrDate[0];
                    break;
                case "ja":
                    //"yy/mm";
                    ParsingDate = arrDate[0] + "/" + arrDate[1];
                    break;
                case "fr":
                    //"mm/yy";
                    ParsingDate = arrDate[1] + "/" + arrDate[0];
                    break;
                case "zh-cn":
                    //"yy/mm";
                    ParsingDate = arrDate[0] + "/" + arrDate[1];
                    break;
                case "zh-tw":            
                    //"yy-mm";
                    ParsingDate = arrDate[0] + "-" + arrDate[1];
                    break;
            }

            return ParsingDate;
        }

        #endregion

        #region (프로퍼티)
        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        protected void lbtnexcel_Click(object sender, EventArgs e)
        {
            StringBuilder sbData = new StringBuilder();

            Zeospace.DeskPlus.ETC.Web.Ws.Holiday.ListAdmin ws = new Ws.Holiday.ListAdmin();
            List<object> list = ws.GetAllHoliAdminUserList(hdexcel.Value.ToString());
            this.GetResourceManager();

            string[] holiTitle1 = new string[3];

            //연차 Title
            holiTitle1[0] = ddlYearField.SelectedItem + " " + GetResource("HoliType1") + " " + GetResource("HoliAll");
            holiTitle1[1] = ddlYearField.SelectedItem + " " + GetResource("HoliType1") + " " + GetResource("HoliUsed");
            holiTitle1[2] = ddlYearField.SelectedItem + " " + GetResource("HoliType1") + " " + GetResource("HoliReceive");

            sbData.Append("<tr>");
            sbData.Append("<td class='center'>" + GetResource("Number") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("DeptName") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("UserName") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("TitleName") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("DutyName") + "</td>");
            sbData.Append("<td class='center'>" + holiTitle1[0] + "</td>");
            sbData.Append("<td class='center'>" + holiTitle1[1] + "</td>");
            sbData.Append("<td class='center'>" + holiTitle1[2] + "</td>");
            sbData.Append("</tr>");

            foreach (BE.HoliAdminInfo _data in list)
            {
                sbData.Append("<tr>");
                sbData.Append("<td class='center'>" + _data.ROW + "</td>");
                sbData.Append("<td class='center'>" + _data.DeptName + "</td>");
                sbData.Append("<td class='center'>" + _data.WriteUserName + "</td>");
                sbData.Append("<td class='center'>" + _data.TitleName + "</td>");
                sbData.Append("<td class='center'>" + _data.DutyName + "</td>");
                sbData.Append("<td class='center'>" + _data.All1 + "</td>");
                sbData.Append("<td class='center'>" + _data.Used1 + "</td>");
                sbData.Append("<td class='center'>" + _data.Jan1 + "</td>");
                sbData.Append("</tr>");
            }

            string strFileName = ddlYearField.SelectedValue.Split(',')[0] + "~" + ddlYearField.SelectedValue.Split(',')[1] + "_" + GetResource("UserListTitle") + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            strFileName = Server.UrlEncode(strFileName.Replace(" ", "")).Replace("+", "%20");
                                                
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
            <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
            </head><body>");

            sb.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0'>");
            sb.Append(sbData);
            sb.Append("</table>");

            sb.Append("</body></html>");

            Response.Clear();
            Response.Buffer = true;            
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(sb.ToString());   
            Response.End();
        }

        #endregion
    }
}