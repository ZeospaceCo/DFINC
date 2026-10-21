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

namespace Zeospace.DeskPlus.ETC.Web.Work
{
    public partial class ListUser : Base.BasePageETC
    {
        #region 변수설정
        public int UserID = 0;
        public string SearchTermValue = string.Empty;  // 야,특근 검색 날짜
        public string StartYear = string.Empty; //야,특근 시작 년도  
        public string StartMonth = string.Empty; //야,특근 시작 월 
        public string EndYear = string.Empty; //야,특근 종료 년도
        public string EndMonth = string.Empty; //야,특근 종료 월
        public string StartDate = string.Empty; //야,특근 시작 년월
        public string EndDate = string.Empty; //야,특근 종료 년월
        public string Gubun = string.Empty;            // 구분
        public string Order = string.Empty;            // sort

        public int CurrentPage = 0;
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; // 리스트 개수
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Work.ListUser).Assembly;	//어셈블리 선언
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
        /// 작성자 : 김지현
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadGubunSetting();

            // 구분
            ddlGubun.SelectedValue = Gubun;

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,"Pay", CurrentUserInfo.UserID, "User");

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
        /// 작성자  : 김지현
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnExcel.Text = this.resMng.GetString("ExecelDown");
                ltlHeader.Text = this.resMng.GetString("UserWorkList");
                cbtnSearch.Text = this.resMng.GetString("Search");
            }
        }

        /// <summary>
        /// 작성자  : 김지현
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            UserID = GetDecryptParam("userID", CurrentUserInfo.UserID.ToString()).ToNumber<int>();

            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            Gubun = GetDecryptParam("gubun", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");

            Order = (Order.Length == 0) ? "ReportID DESC, StartDate DESC " : Order;

            RecordPerPage = GetDecryptParam("recordPerPage", "5");

        }

        private void LoadGubunSetting()
        {
            DAL.WorkCat biz = new DAL.WorkCat();

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
        /// 작성자   : 김지현
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.WorkReport dal = new DAL.WorkReport();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, StartDate, EndDate, Gubun.ToNumber<int>()).ToString();

        }

        /// <summary>  
        /// 작성자  : 김지현
        /// 휴가 기간 가져오기
        /// </summary>
        private void GetDateType()
        {
            string startDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + "01";
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

            Zeospace.DeskPlus.ETC.Web.Ws.Work.ListUser ws = new Ws.Work.ListUser();
            List<Ws.Work.PageList> list = ws.GetPageList(hdexcel.Value.ToString());

            sbData.Append("<tr>");
            sbData.Append("<td align='center'>" + GetResource("Number") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("Gubun") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("CustomerName") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("StartDate") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("EndDate") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("Duration") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("Time") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("WorkList") + "</td>");
            sbData.Append("</tr>");

            if (list != null && list.Count > 0)
            {
                foreach (Ws.Work.PageList _data in list)
                {
                    sbData.Append("<tr>");
                    sbData.Append("<td align='center'>" + _data.ROW + "</td>");
                    sbData.Append("<td class='center'>" + _data.Gubun + "</td>");
                    sbData.Append("<td class='center'>" + _data.CustomerName + "</td>");
                    sbData.Append("<td class='center'>" + _data.StartDate + "</td>");
                    sbData.Append("<td class='center'>" + _data.EndDate + "</td>");
                    sbData.Append("<td class='center'>" + _data.Duration + "</td>");
                    sbData.Append("<td class='center'>" + _data.Hour + "</td>");
                    sbData.Append("<td class='center'>" + _data.Reason + "</td>");
                    sbData.Append("</tr>");
                }
            }
            else
            {
                sbData.Append("<tr>");
                sbData.Append("<td colspan='8' align='center'>" + GetResource("NoContent") + "</td>");
                sbData.Append("</tr>");
            }

            string strFileName = GetResource("WorkList") + ".xls";
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