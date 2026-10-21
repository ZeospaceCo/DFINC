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
    public partial class AdminManage : Base.BasePageETC
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
        public string ActiveUsers = string.Empty;      //재직, 퇴사
        public string SearchTermValue = string.Empty;  // 휴가기간 검색 날짜
        public string CompanyName = string.Empty;   // 소속
        public string DeptName = string.Empty;         // 부서
        public string TitleName = string.Empty;        // 직급
        public string Gubun = string.Empty;            // 구분
        public string Order = string.Empty; //sort
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; // 리스트 개수
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Holiday.AdminManage).Assembly;	//어셈블리 선언
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
        /// 작성자 : shine
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadGubunSetting();

            // 재직상태
            RblActive.SelectedValue = ActiveUsers;
            // 부서
            ctbDeptName.Text = DeptName;
            // 직급
            ctbTitleName.Text = TitleName;
            // 사번.이름
            ddlSearchField.SelectedValue = SearchField;
            // 검색어
            ctbSearch.Text = SearchValue;
            // 구분
            ddlGubun.SelectedValue = Gubun;


            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "HolidayManage", CurrentUserInfo.UserID, "Admin");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage;

            GetDateType();
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자  : shine
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("EmpNo"), "EmployeeNumber"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Name"), "ManyName"));

                RblActive.Items.Clear();
                RblActive.Items.Add(new ListItem(this.resMng.GetString("Work"), "1"));            //재직
                RblActive.Items.Add(new ListItem(this.resMng.GetString("Retire"), "0"));            //퇴사 

                RblActive.Items[0].Selected = true;

                cbtnExcel.Text = this.resMng.GetString("ExecelDown");
                cbtnSearch.Text = this.resMng.GetString("Search");
            }
        }

        /// <summary>
        /// 작성자  : shine
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            ActiveUsers = GetDecryptParam("activeUsers", "1");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            CompanyName = GetDecryptParam("companyName", "");
            DeptName = GetDecryptParam("deptName", "");
            TitleName = GetDecryptParam("titleName", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Gubun = GetDecryptParam("gubun", "");
            Order = (Order.Length == 0) ? "UsedID DESC, StartDate DESC " : Order;

            RecordPerPage = GetDecryptParam("recordPerPage", "5");
        }

        private void LoadGubunSetting()
        {
            DAL.HoliCat biz = new DAL.HoliCat();
            
            DataSet ds = biz.GetCategory(CurrentUserInfo.CompanyID);

            ddlGubun.Items.Clear();

            ddlGubun.Items.Add(new ListItem(GetResource("ListType0"), "0"));

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ddlGubun.Items.Add(new ListItem(dr["CatName"].ToString(), dr["CatID"].ToString()));
                }
            }
        }

        /// <summary>  
        /// 작성자  : shine
        /// 휴가 기간 가져오기
        /// </summary>
        private void GetDateType()
        {
            string startDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString().PadLeft(2,'0') + "-" + "01";
            string lastDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

            SearchTermValue = DateFormat(startDate) + "," + DateFormat(lastDate);
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
            switch (PreLang)
            {
                case "ko":
                    //"yy-mm-dd";
                    ParsingDate = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
                    break;
                case "en":
                    //"mm/dd/yy";
                    ParsingDate = arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
                    break;
                case "en-gb":
                    //"mm/dd/yy";
                    ParsingDate = arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
                    break;
                case "ja":
                    //"yy/mm/dd";
                    ParsingDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
                    break;
                case "fr":
                    //"dd/mm/yy";
                    ParsingDate = arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0];
                    break;
                case "zh-cn":
                    //"yy/mm/dd";
                    ParsingDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
                    break;
                case "zh-tw":
                    //"yy-mm-dd";
                    ParsingDate = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
                    break;
            }

            return ParsingDate;
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        protected void lbtnexcel_Click(object sender, EventArgs e)
        {
            StringBuilder sbData = new StringBuilder();

            Zeospace.DeskPlus.ETC.Web.Ws.Holiday.AdminManage ws = new Ws.Holiday.AdminManage();
            List<Ws.Holiday.PageList> list = ws.GetPageList(hdexcel.Value.ToString());

            sbData.Append("<tr>");
            sbData.Append("<td align='center'>" + GetResource("Number") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("DeptName") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("EmpNo") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("UserName") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("TitleName") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("StartDate") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("StartHalf") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("EndDate") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("StartHalf") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("Period") + "</td>");
            sbData.Append("<td align='center'>" + GetResource("Gubun") + "</td>");
            sbData.Append("</tr>");

            if (list != null && list.Count > 0)
            {
                foreach (Ws.Holiday.PageList _data in list)
                {
                    sbData.Append("<tr>");
                    sbData.Append("<td align='center'>" + _data.ROW + "</td>");
                    sbData.Append("<td align='center'>" + _data.DeptName + "</td>");
                    sbData.Append("<td align='center'>" + _data.EmployeeNumber + "</td>");
                    sbData.Append("<td align='center'>" + _data.Name + "</td>");
                    sbData.Append("<td align='center'>" + _data.TitleName + "</td>");
                    sbData.Append("<td align='center'>" + _data.StartDate + "</td>");
                    sbData.Append("<td align='center'>" + _data.StartHalf + "</td>");
                    sbData.Append("<td align='center'>" + _data.EndDate + "</td>");
                    sbData.Append("<td align='center'>" + _data.EndHalf + "</td>");
                    sbData.Append("<td align='center'>" + _data.Duration + "</td>");
                    sbData.Append("<td align='center'>" + _data.Gubun + "</td>");
                    sbData.Append("</tr>");
                }
            }
            else
            {
                sbData.Append("<tr>");
                sbData.Append("<td colspan='11' align='center'>" + GetResource("NoContent") + "</td>");
                sbData.Append("</tr>");
            }

            string strFileName = GetResource("HoliList") + ".xls";
            strFileName = Server.UrlEncode(strFileName.Replace(" ", ""));

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