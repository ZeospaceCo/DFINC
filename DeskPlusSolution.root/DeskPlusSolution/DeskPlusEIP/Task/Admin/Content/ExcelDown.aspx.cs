using System;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Task.Web.Admin.Content
{
    public partial class ExcelDown : Base.BasePageTaskAdmin
    {
        #region 변수설정
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

            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Content.ExcelDown).Assembly;	//어셈블리 선언

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
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
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
            sb.Append("<td align='center'>" + GetResource("IsGubun") + "</td>"); //구분
            sb.Append("<td align='center'>" + GetResource("Title") + "</td>"); //제목
            sb.Append("<td align='center'>" + GetResource("State") + "</td>"); //상태            
            sb.Append("<td align='center'>" + GetResource("Requester") + "</td>"); //요청자
            sb.Append("<td align='center'>" + GetResource("Category") + "</td>"); //대분류
            sb.Append("<td align='center'>" + GetResource("SubCategory") + "</td>"); //소분류
            sb.Append("<td align='center'>" + GetResource("RequestDate") + "</td>"); //요청일자
            sb.Append("<td align='center'>" + GetResource("EndDate") + "</td>"); //종료(예정)일
            sb.Append("<td align='center'>D-Day</td>"); //d-day
            sb.Append("</tr>");

            #endregion 엑셀 상단 기술

            DAL.List biz = new DAL.List();

            DataTableReader dtr = biz.GetAdminList(CurrentUserInfo.CompanyID, startDate, endDate, SearchStatus,
                SearchField, SearchValue, ItemID, ItemSubID, IsGubun);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + dtr["TaskNumber"].ToString() + "</td>");

                        if (dtr["IsGubun"].ToString() == "1")
                        {
                            sb.Append("<td align='center'>" + GetResource("ShortIsGubun1") + "</td>");
                        }
                        else
                        {
                            sb.Append("<td align='center'>" + GetResource("ShortIsGubun2") + "</td>");
                        }
                        sb.Append("<td align='center'>" + dtr["Title"].ToString() + "</td>");
                        sb.Append("<td align='center'>" + GetStatus(dtr["StatusLast"].ToString()) + "</td>");

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

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

    }
}