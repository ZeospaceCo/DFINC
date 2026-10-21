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

namespace Zeospace.DeskPlus.ETC.Web.Holiday.Popup
{
    public partial class ViewAdmin : Base.BasePageETC
    {
        #region 변수설정
        public int UserID = 0;
        public string Name = "";
        public string StartDate = "";
        public string EndDate = "";
        public string TitleDate = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Holiday.Popup.ViewAdmin).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
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
            SetLanguage();

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Holiday", CurrentUserInfo.UserID, "ViewAdmin");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자  : 김형석
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            StartDate = GetDecryptParam("startDate", "");
            EndDate = GetDecryptParam("endDate", "");
            TitleDate = GetDecryptParam("titleDate", "");
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            Name = GetDecryptParam("name", "");
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");

            hdstartdate.Value = StartDate;
            hdenddate.Value = EndDate;
            hdname.Value = Name;
        }

        /// <summary>
        /// 작성자  : 김형석
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                ltlHeader.Text = Name + " " + this.resMng.GetString("AdminViewTitle") + " ( " + TitleDate + " )";
                cbtnExcel.Text = resMng.GetString("ExecelDown");
                cbtnClose.Text = resMng.GetString("Closed");
            }
        }

        /// <summary>
        /// 작성자   : 김형석
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.HoliUsed dal = new DAL.HoliUsed();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, UserID, StartDate, EndDate).ToString();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        protected void lbtnexcel_Click(object sender, EventArgs e)
        {
            StringBuilder sbData = new StringBuilder();

            Zeospace.DeskPlus.ETC.Web.Ws.Holiday.Popup.ViewAdmin ws = new Ws.Holiday.Popup.ViewAdmin();
            List<object> list = ws.GetAllHoliUserList(hdexcel.Value.ToString());
            this.GetResourceManager();
            
            sbData.Append("<tr>");
            sbData.Append("<td class='center'>" + GetResource("StartDate") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("StartHalfExcel") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("EndDate") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("EndHalfExcel") + "</td>");
            sbData.Append("<td>" + GetResource("Duration") + "</td>");
            sbData.Append("<td class='center'>" + GetResource("Gubun") + "</td>");     
            sbData.Append("</tr>");

            foreach (BE.HoliUsedInfo _data in list)
            {
                sbData.Append("<tr>");
                sbData.Append("<td class='center'>" + _data.StartDate + "</td>");
                sbData.Append("<td class='center'>" + _data.StartHalf + "</td>");
                sbData.Append("<td class='center'>" + _data.EndDate + "</td>");
                sbData.Append("<td class='center'>" + _data.EndHalf + "</td>");
                sbData.Append("<td class='center'>" + _data.Duration + "</td>");
                sbData.Append("<td class='center'>" + _data.Gubun + "</td>");            
                sbData.Append("</tr>");
            }

            string strFileName = hdstartdate.Value + "~" + hdenddate.Value + "_" + GetResource("UserListTitle") + "_" + hdname.Value + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
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