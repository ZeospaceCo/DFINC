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
using Zeospace.DeskPlus.Common.DAL;
using System.Text;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Static.LogIn
{
    public partial class ExcelDown : Base.BasePageEIP
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string Order = string.Empty;       // sort 
        public string SearchTerm = string.Empty;  //검색기간구분자
        public string SearchTermValue = string.Empty;   //검색기간 시작,종료값 
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수  
        public string SearchField = string.Empty;   // 검색구분
        public string SearchValue = string.Empty;   // 검색 필드
        public DataTableReader dtr = null;

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
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Static.LogIn.ExcelDown).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
                LoadExcel();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            LoadParam();
            GetList();
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 엑셀 로드
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadExcel()
        {
            string today = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Day.ToString().PadLeft(2, '0');

            StringBuilder sb = new StringBuilder();

            sb.Append(@"
                <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
                <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
                </head><body>");

            sb.Append("<table style='BORDER-RIGHT-WIDTH: 0px; WIDTH: 100%; BORDER-COLLAPSE: collapse; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px' id='tableHeader' border='1' cellSpacing='0' cellPadding='0'>");
            sb.Append(LoadLogInExcelHeader());
            sb.Append(LoadLogInExcelContent());
            sb.Append("</table>");

            sb.Append("</body></html>");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + today + "_DownLoadAccessLogList.xls");
            Response.ContentType = "application/vnd.ms-excel";


            Response.Write(sb.ToString());
            Response.End();
            
        }


        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 엑셀 헤더
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        private string LoadLogInExcelHeader()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr>");
            sb.Append("<td align='center'>" + GetResource("DeptName") + "</td>");
            sb.Append("<td align='center'>" + GetResource("TitleName") + "</td>");
            sb.Append("<td align='center'>" + GetResource("UserName") + "</td>");
            sb.Append("<td align='center'>" + GetResource("IPAddress") + "</td>");
            sb.Append("<td align='center'>" + GetResource("StartDate") + "</td>");
            sb.Append("<td align='center'>" + GetResource("EndDate") + "</td>");

            sb.Append("</tr>");
            return sb.ToString();
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 엑셀 다운로드 내용
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        private string LoadLogInExcelContent()
        {
            StringBuilder sb = new StringBuilder();
            string userName = string.Empty;     // 이름
            string deptName = string.Empty;     // 부서명
            string titleName = string.Empty;    // 직급명
            string ipAddress = string.Empty;    // IP

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        #region 이름 다국어 설정
                        userName = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 부서 다국어 설정
                        deptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직급 다국어 설정
                        titleName = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region IP세팅

                        ipAddress = dtr["IPAddress"].ToString();

                        if (ipAddress.Trim().Length == 12)
                        {
                            string IP1 = string.Empty;
                            string IP2 = string.Empty;
                            string IP3 = string.Empty;
                            string IP4 = string.Empty;

                            IP1 = ipAddress.Substring(0, 3).ToString();
                            IP2 = ipAddress.Substring(3, 3).ToString();
                            IP3 = ipAddress.Substring(6, 3).ToString();
                            IP4 = ipAddress.Substring(9, 3).ToString();

                            ipAddress = IP1 + "." + IP2 + "." + IP3 + "." + IP4;
                        }

                        #endregion

                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + deptName + "</td>");
                        sb.Append("<td align='center'>" + titleName + "</td>");
                        sb.Append("<td align='center'>" + userName + "</td>");
                        sb.Append("<td align='center'>" + ipAddress + "</td>");
                        sb.Append("<td align='center'>" + dtr["StartTime"].ToString() + "</td>");
                        sb.Append("<td align='center'>" + dtr["EndTime"].ToString() + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr><td colspan='6' align='center'>" + GetResource("SearchNoResultData") + "</td></tr>");
                }
            }
            return sb.ToString();
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

            if (!this.resMng.Equals(null))
            {

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
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = (Order.Length == 0) ? "AccessLogID DESC " : Order;
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 접속통계 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void GetList()
        {
            #region 날짜

            string startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
            string endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            //string getDate = "";

            /*
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
            */

            #endregion

            #region IP값 세팅

            if (SearchField == "IPAddress")
            {
                /*
                string[] remoteAddrStr = SearchValue.Split('.');
                string remoteAddr = SearchValue == "" ? "" : UtilHelper.GetIPAddress(remoteAddrStr);
                SearchValue = remoteAddr;
                */
                SearchValue = SearchValue.Replace(".", "");
            }

            #endregion

            Zeospace.DeskPlus.Common.DAL.AccessLog biz = new AccessLog();
            TotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue).ToString();
            dtr = biz.GetLogPageList(CurrentUserInfo.CompanyID, TotalCount, "0",startDate, endDate,SearchField, SearchValue, Order);
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