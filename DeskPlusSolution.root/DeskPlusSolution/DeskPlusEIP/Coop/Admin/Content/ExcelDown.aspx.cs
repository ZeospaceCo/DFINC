using System;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.Web.Admin.Content
{
    public partial class ExcelDown : Base.BasePageCoopAdmin
    {
        #region 변수설정

        public string ItemID = string.Empty;
        public string ItemSubID = string.Empty;
        public string SearchStatus = string.Empty;
        public string SearchField = string.Empty;
        public string SearchValue = string.Empty;
        public string SearchDateValue = string.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Admin.Content.ExcelDown).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자  : shimpark
        /// 작성일자 : 2014-08-30
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
        /// 작성자  : shimpark
        /// 작성일자 : 2014-08-30
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
            ItemID = GetDecryptParam("itemID", "");
            ItemSubID = GetDecryptParam("itemSubID", "");
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchDateValue = GetDecryptParam("searchDateValue", "");
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetList();
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자  : shimpark
        /// 작성일자 : 2014-08-30
        /// 작성목적 : 첨부파일 다운로드 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void GetList()
        {
            #region 날짜

            string startDate = "";
            string endDate = "";

            if ((SearchField == "RequestDate" || SearchField == "StartDate" || SearchField == "EndDate") && SearchDateValue.Length > 0)
            {
                startDate = DateHelper.GetCultureDate(SearchDateValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchDateValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion 날짜

            StringBuilder sb = new StringBuilder();

            #region 엑셀 상단 기술

            sb.Append(@"
                <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
                <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
                </head><body>");

            sb.Append("<table style='BORDER-RIGHT-WIDTH: 0px; WIDTH: 100%; BORDER-COLLAPSE: collapse; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px' id='tableHeader' border='1' cellSpacing='0' cellPadding='0'>");
            sb.Append("<tr>");
            sb.Append("<td align='center'>" + GetResource("DocNumber") + "</td>"); //문서번호
            sb.Append("<td align='center'>" + GetResource("Title") + "</td>"); //제목
            sb.Append("<td align='center'>" + GetResource("State") + "</td>"); //상태
            sb.Append("<td align='center'>" + GetResource("Receiver") + "</td>"); //수신자
            sb.Append("<td align='center'>" + GetResource("Requester") + "</td>"); //요청자
            sb.Append("<td align='center'>" + GetResource("First") + GetResource("Group") + "</td>"); //1차분류
            sb.Append("<td align='center'>" + GetResource("Second") + GetResource("Group") + "</td>"); //2차분류
            sb.Append("<td align='center'>" + GetResource("RequestDate") + "</td>"); //요청일자
            sb.Append("<td align='center'>" + GetResource("EndDate") + "</td>"); //종료(예정)일
            sb.Append("<td align='center'>D-Day</td>"); //d-day
            sb.Append("</tr>");

            #endregion 엑셀 상단 기술

            DAL.List biz = new DAL.List();

            DataTableReader dtr = biz.GetAdminPageList(CurrentUserInfo.CompanyID, "30", "0",
                startDate, endDate, SearchStatus, SearchField, SearchValue, "", ItemID.ToNumber<int>(), ItemSubID.ToNumber<int>());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + dtr["CoopNumber"].ToString() + "</td>");
                        sb.Append("<td align='center'>" + dtr["Title"].ToString() + "</td>");
                        sb.Append("<td align='center'>" + GetStatus(dtr["StatusLast"].ToString()) + "</td>");

                        sb.Append("<td align='center'>" + this.GetLangName(
                                dtr["TONameBase"].ToString().ToNull(), dtr["TONameKo"].ToString().ToNull(),
                                dtr["TONameEn"].ToString().ToNull(), dtr["TONameJa"].ToString().ToNull(),
                                dtr["TONameFr"].ToString().ToNull(), dtr["TONameZhcn"].ToString().ToNull(),
                                dtr["TONameZhtw"].ToString().ToNull(), PreLang));
                        if (dtr["TOCnt"].ToString().ToNumber<int>() > 1) sb.Append("+");
                        sb.Append("</td>");

                        sb.Append("<td align='center'>" + this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang) + "</td>");

                        sb.Append("<td align='center'>" + this.GetLangName(
                                dtr["ItemNameBase"].ToString().ToNull(), dtr["ItemNameKo"].ToString().ToNull(),
                                dtr["ItemNameEn"].ToString().ToNull(), dtr["ItemNameJa"].ToString().ToNull(),
                                dtr["ItemNameFr"].ToString().ToNull(), dtr["ItemNameZhcn"].ToString().ToNull(),
                                dtr["ItemNameZhtw"].ToString().ToNull(), PreLang) + "</td>");

                        sb.Append("<td align='center'>" + this.GetLangName(
                                dtr["ItemSubNameBase"].ToString().ToNull(), dtr["ItemSubNameKo"].ToString().ToNull(),
                                dtr["ItemSubNameEn"].ToString().ToNull(), dtr["ItemSubNameJa"].ToString().ToNull(),
                                dtr["ItemSubNameFr"].ToString().ToNull(), dtr["ItemSubNameZhcn"].ToString().ToNull(),
                                dtr["ItemSubNameZhtw"].ToString().ToNull(), PreLang) + "</td>");

                        sb.Append("<td align='center'>" + DateHelper.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal) + "</td>");
                        sb.Append("<td align='center'>" + DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal) + "</td>");
                        sb.Append("<td align='center'>" + dtr["ExcessDay"].ToString() + "</td>");
                        sb.Append("</tr>");
                    }
                }
            }

            sb.Append("</table>");
            sb.Append("</body></html>");

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=OTICS_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(sb.ToString());
            Response.End();
        }
    }
}