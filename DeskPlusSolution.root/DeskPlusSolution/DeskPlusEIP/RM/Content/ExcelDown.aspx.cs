using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class ExcelDown : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수설정
        public string Order = string.Empty;       // sort 
        public string SearchTerm = string.Empty;  //검색기간구분자
        public string SearchTermValue = string.Empty;   //검색기간 시작,종료값 
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수  
        public string SearchField = string.Empty;   // 검색구분
        public string SearchValue = string.Empty;   // 검색 필드
        public string AdminYN = String.Empty;
        public string Usable = String.Empty;
        public DataTableReader dtr = null;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.ExcelDown).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
                LoadExcel();
            }

        }

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            LoadParam();
            GetList();
        }

        /// <summary>
        /// 작성자  : maniaysw
        /// 작성일자 : 2017-01-24
        /// 작성목적 : 엑셀 로드
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadExcel()
        {
            string today = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Day.ToString().PadLeft(2, '0');
            string fileName = "";

            if (Usable == "0")
            {
                fileName = HttpUtility.UrlEncode(GetResource("Usable0Reserve"),System.Text.Encoding.UTF8).Replace("+","%20");
            }
            else if (Usable == "1")
            {
                fileName = HttpUtility.UrlEncode(GetResource("Usable1Reserve"), System.Text.Encoding.UTF8).Replace("+", "%20");
            }
            else if (Usable == "2")
            {
                fileName = HttpUtility.UrlEncode(GetResource("Usable2Reserve"), System.Text.Encoding.UTF8).Replace("+", "%20");
            }
            else //Usable 4 값이면 나의 예약
            {
                fileName = HttpUtility.UrlEncode(GetResource("MyReserve"), System.Text.Encoding.UTF8).Replace("+", "%20"); 
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(@"
                <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
                <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
                </head><body>");

            sb.Append("<table style='BORDER-RIGHT-WIDTH: 0px; WIDTH: 100%; BORDER-COLLAPSE: collapse; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px' id='tableHeader' border='1' cellSpacing='0' cellPadding='0'>");
            sb.Append(LoadAttachExcelHeader());
            sb.Append(LoadAttachExcelContent());
            sb.Append("</table>");

            sb.Append("</body></html>");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "_" + today + ".xls");
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(sb.ToString());
            Response.End();

        }


        /// <summary>
        /// 작성자  : maniaysw
        /// 작성일자 : 2017-01-24
        /// 작성목적 : 엑셀 헤더
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        private string LoadAttachExcelHeader()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr>");
            if (Usable == "4") //나의예약일 경우 승인헤더 추가
            {
                sb.Append("<td align='center'>" + GetResource("Approved") + "</td>");
            }
            sb.Append("<td align='center'>" + GetResource("ResourcesTitle") + "</td>");
            sb.Append("<td align='center'>" + GetResource("ReserveDate") + "</td>");
            sb.Append("<td align='center'>" + GetResource("Subject") + "</td>");
            sb.Append("<td align='center'>" + GetResource("Subscriber") + "</td>");
            sb.Append("<td align='center'>" + GetResource("ModifyDate") + "</td>");
            sb.Append("</tr>");
            return sb.ToString();
        }

        /// <summary>
        /// 작성자  : maniaysw
        /// 작성일자 : 2017-01-24
        /// 작성목적 : 엑셀 다운로드 내용
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        private string LoadAttachExcelContent()
        {
            StringBuilder sb = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        sb.Append("<tr>");
                        if (Usable == "4")
                        {
                            if (dtr["Usable"].ToString() == "0") ///DB Usable 값이 0이면 승인대기
                            {
                                sb.Append("<td align='center'>" +GetResource("Usable0") + "</td>");
                            }
                            else if (dtr["Usable"].ToString() == "1")
                            {
                                 sb.Append("<td align='center'>" +GetResource("Usable1") + "</td>");
                            }
                            else if (dtr["Usable"].ToString() == "2")
                            {
                                sb.Append("<td align='center'>" + GetResource("Usable2") + "</td>");
                            }

                        }
                        sb.Append("<td align='center'>" + dtr["Title"].ToString() + "</td>");
                        sb.Append("<td align='center'>" + Convert.ToDateTime(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm") + " ~ " + Convert.ToDateTime(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm") + "</td>");
                        sb.Append("<td align='center'>" + dtr["Subject"].ToString() + "</td>");
                        sb.Append("<td align='center'>" + dtr["WriteUserName"].ToString() + "</td>");
                        sb.Append("<td align='center'>" + Convert.ToDateTime(dtr["WriteDate"].ToString()).ToString("yyyy-MM-dd HH:mm") + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr><td colspan='5' align='center'>" + GetResource("SearchNoResultData") + "</td></tr>");
                }
            }


            return sb.ToString();
        }
        /// <summary>
        /// 작성자  : maniaysw
        /// 작성일자 : 2017-01-24
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
        /// 작성자  : maniaysw
        /// 작성일자 : 2017-01-24
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Usable = GetDecryptParam("usable", "0");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "StartDate desc" : Order;
            AdminYN = GetDecryptParam("adminYN", "N");            
        }

        /// <summary>
        /// 작성자  : maniaysw
        /// 작성일자 : 2017-01-24
        /// 작성목적 : 첨부파일 다운로드 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void GetList()
        {
            #region 날짜

            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (SearchTerm == "week" || SearchTerm == "month")
            {
                if (SearchTerm == "week")
                    getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                else if (SearchTerm == "month")
                    getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm == "period")
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion

            DAL.Reverse dal = new DAL.Reverse();
            if (Usable == "0" || Usable == "1" || Usable == "2")
            {
                TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, AdminYN, Usable, startDate, endDate, SearchField, SearchValue).ToString();
                dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, AdminYN, Usable, TotalCount, "0", startDate, endDate, SearchField, SearchValue, Order);
            }
            else //Usable 0,1,2 가 아니면 나의 예약
            {
                TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,startDate, endDate, SearchField, SearchValue).ToString();
                dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,TotalCount, "0", startDate, endDate, SearchField, SearchValue, Order);
            }
            

        }

        #endregion
    }
}