using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.HR.Web.Content.Popup
{
    public partial class ViewPrivateEtc : Base.BasePageHR
    {
        #region Declaration (컨트롤 선언)

        public string CalcDate = string.Empty;
        public string DayWeek = string.Empty;

        public string HUDocumentID = string.Empty;
        public string PRDocumentID = string.Empty;

        public string UserName = string.Empty;
        public string DeptName = string.Empty;
        public string TitleName = string.Empty;

        public string bHoliAll = string.Empty;
        public string bLate = string.Empty;
        public string bEarly = string.Empty;
        public string bOver = string.Empty;

        public string HoliGubun = string.Empty;
        public string bLateGubun = string.Empty;
        public string bEarlyGubun = string.Empty;
        public string bOverGubun = string.Empty;

        public string HoliGubun2 = string.Empty;
        public string LateRemark = string.Empty;
        public string EarlyRemark = string.Empty;
        public string OverRemark = string.Empty;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";		        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.Popup.ViewPrivateEtc).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnClose.Text = resMng.GetString("BtnClose");
            }
        }

        private void LoadData()
        {
            LoadParam();

            ltlHeader.Text = CalcDate + " " + DayWeek + " - " + DeptName + " " + TitleName + " " + UserName;

            ltHoliGubun.Text = HoliGubun.Replace(" / ", "");
            ltHoliGubun2.Text = "<div style=\"word-break: break-all;\">" + HoliGubun2 + "</div>";
            ltHUDocumentID.Text = "<span onclick=\"ViewPrivateEtc.DocView('" + HUDocumentID + "');return false;\" style=\"cursor:pointer\">";
			ltHUDocumentID.Text += "<img alt=\"\" src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span>";

            ltbOverGubun.Text = bOverGubun.Replace(" / ", "");
            ltOverRemark.Text = "<div style=\"word-break: break-all;\">" + OverRemark + "</div>";
            ltPRDocumentID.Text = "<span onclick=\"ViewPrivateEtc.DocView('" + PRDocumentID + "');return false;\" style=\"cursor:pointer\">";
            ltPRDocumentID.Text += "<img alt=\"\" src=\"/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif\" /></span>";

            ltbLateGubun.Text = bLateGubun.Replace(" / ", "");
            ltLateRemark.Text = "<div style=\"word-break: break-all;\">" + LateRemark + "</div>";

            ltbEarlyGubun.Text = bEarlyGubun.Replace(" / ", "");
            ltEarlyRemark.Text = "<div style=\"word-break: break-all;\">" + EarlyRemark + "</div>";
        }

        private void LoadParam()
        {
            CalcDate = GetDecryptParam("calcDate", "");
            DayWeek = GetDecryptParam("dayWeek", "");

            HUDocumentID = GetDecryptParam("HUDocumentID", "");
            PRDocumentID = GetDecryptParam("PRDocumentID", "");

            UserName = GetDecryptParam("userName", "");
            DeptName = GetDecryptParam("deptName", "");
            TitleName = GetDecryptParam("titleName", "");

            bHoliAll = GetDecryptParam("bHoliAll", "");
            bLate = GetDecryptParam("bLate", "");
            bEarly = GetDecryptParam("bEarly", "");
            bOver = GetDecryptParam("bOver", "");

            HoliGubun = GetDecryptParam("HoliGubun", "");
            bLateGubun = GetDecryptParam("bLateGubun", "");
            bEarlyGubun = GetDecryptParam("bEarlyGubun", "");
            bOverGubun = GetDecryptParam("bOverGubun", "");

            HoliGubun2 = GetDecryptParam("HoliGubun2", "");
            LateRemark = GetDecryptParam("LateRemark", "");
            EarlyRemark = GetDecryptParam("EarlyRemark", "");
            OverRemark = GetDecryptParam("OverRemark", "");
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}