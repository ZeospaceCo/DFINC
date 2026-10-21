using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

using System.IO;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    
    public partial class ListSearch  : Base.BasePagePim
    { 
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정

        public string Distance = string.Empty;
        public string inquryPerson = string.Empty;
        public string DatePeriod = string.Empty;
        public string SearchKind = string.Empty;
        public string SerachValue = string.Empty;
        public string StartDate = string.Empty;
        public string EndDate = string.Empty;

        public int UserID = 0;
        public int CompanyID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ListSearch).Assembly;	//어셈블리 선언
            #endregion

            #region 기본정보세팅
            UserID = CurrentUserInfo.UserID;
            CompanyID = CurrentUserInfo.CompanyID;
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                SettingControl();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)


        private void ExcelDownLoad()
        {
            //Ws.GetPim ws = new Ws.GetPim();
            //ws.
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlSearchKind.SelectedValue = DatePeriod;
            ddlValue.SelectedValue = SearchKind;
            ctbSearch.Value = SerachValue;
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void SettingControl()
        {

            //ddlSearchKind
             
            ListItem lt1 = new ListItem(GetResource("SEARCH1"), "0");
            ListItem lt2 = new ListItem(GetResource("SEARCH2"), "WEEK");
            ListItem lt3 = new ListItem(GetResource("SEARCH3"), "MON");
            ListItem lt4 = new ListItem(GetResource("SEARCH4"), "U");

            ddlSearchKind.Items.Add(lt1);
            ddlSearchKind.Items.Add(lt2);
            ddlSearchKind.Items.Add(lt3);
            ddlSearchKind.Items.Add(lt4);

            ListItem ls2 = new ListItem(GetResource("Title"), "Title");
            ListItem ls3 = new ListItem(GetResource("Name"), "Name");
            ddlValue.Items.Add(ls2);
            ddlValue.Items.Add(ls3);

        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {

            Distance = GetDecryptParam("Distance", "");
            inquryPerson = GetDecryptParam("inquryPerson", "");
            DatePeriod = GetDecryptParam("DatePeriod", "");
            SearchKind = GetDecryptParam("SearchKind", "");
            SerachValue = GetDecryptParam("SerachValue", "");
            StartDate = GetDecryptParam("StartDate", "");
            EndDate = GetDecryptParam("EndDate", "");

            
              
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnExcel.Text = GetResource("ExecelDown");
                CbtnSchedule.Text = GetResource("MYSCHEDULE");

                //cbtnCancel.Text = GetResource("Cancel");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        protected void lbtnexcel_Click(object sender, EventArgs e)
        {
            DataGrid dg = new DataGrid();

            Zeospace.DeskPlus.Pim.Web.Ws.Schedule.ListSearch ws = new Ws.Schedule.ListSearch();
            //List<object> list = ws.GetALLList(hdexcel.Value.ToString());
            List<object> list = ws.GetSearchList(hdexcel.Value.ToString());
            this.GetResourceManager();
            string[] arCulName = { GetResource("Name"), GetResource("KIND"), GetResource("DATE2"), GetResource("Time"), GetResource("Title") };
            string[] arVal = new string[arCulName.Length];
            System.Data.DataTable dt = BLL.ExcelHelper.AddStringCulumns(arCulName);

            foreach (BE.ScheduleInfo _data in list)
            {
                arVal[0] = _data.UserName;
                arVal[1] = _data.KindDesk;
                arVal[2] = _data.stringStartTime;
                arVal[3] = _data.stringENDTime;
                arVal[4] = _data.Title;
                BLL.ExcelHelper.AddDataRow(ref dt, arCulName, arVal);
            }

            dg.DataSource = dt;
            dg.DataBind();

            string strFileName = DateTime.Now.ToString("yyyyMMddHHmm");
            strFileName += ".xls";


            Request.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            Response.HeaderEncoding = System.Text.Encoding.GetEncoding("euc-kr");

            new Zeospace.DeskPlus.Component.Excel.DataGridExcelExporter(dg, Page).Export(strFileName);

            GC.Collect();


            /*
            string strFileName = DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
  
            Response.Clear();
            Response.Buffer = true;

 
            Response.AddHeader("Content-Disposition", "attachment;fileName=" + strFileName);            
            Response.ContentType = "application/vnd.ms-excel;charset=utf-8";
            Response.Charset = "";
            
            DataGrid dg = new DataGrid();

            Zeospace.DeskPlus.Pim.Web.Ws.Schedule.ListSearch ws = new Ws.Schedule.ListSearch();
            List<object> list = ws.GetSearchList(hdexcel.Value.ToString());
            this.GetResourceManager();
            string[] arCulName = { GetResource("Name"), GetResource("KIND"), GetResource("DATE2"), GetResource("Time"), GetResource("Title") };
            string[] arVal = new string[arCulName.Length];
            System.Data.DataTable dt = BLL.ExcelHelper.AddStringCulumns(arCulName);

            foreach (BE.ScheduleInfo _data in list)
            {
                arVal[0] = _data.UserName;
                arVal[1] = _data.KindDesk;
                arVal[2] = _data.stringStartTime;
                arVal[3] = _data.stringENDTime;
                arVal[4] = _data.Title;
                BLL.ExcelHelper.AddDataRow(ref dt, arCulName, arVal);
            }

            dg.DataSource = dt;
            dg.DataBind();

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

            FrontDecorator(htmlWriter);
            dg.RenderControl(htmlWriter);
            RearDecorator(htmlWriter);

            String convStr = stringWriter.ToString();
   
            convStr = System.Text.RegularExpressions.Regex.Replace(convStr, "border=\"0\"", "border=\"1\"");
            convStr = System.Text.RegularExpressions.Regex.Replace(convStr, "&nbsp;", " ");

            Response.Write(convStr);
            Response.End();
            */
        }

        /// <summary>
        /// Renders the html text before the datagrid.
        /// </summary>
        /// <param name="writer">A HtmlTextWriter to write html to output stream</param>
        protected virtual void FrontDecorator(HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("HTML");
            writer.WriteFullBeginTag("Head");
            writer.RenderBeginTag(HtmlTextWriterTag.Style);
            writer.Write("td{mso-number-format:\"\\@\";}");
            writer.RenderEndTag();
            writer.WriteEndTag("Head");
            writer.WriteFullBeginTag("Body");
        }

        /// <summary>
        /// Renders the html text after the datagrid.
        /// </summary>
        /// <param name="writer">A HtmlTextWriter to write html to output stream</param>
        protected virtual void RearDecorator(HtmlTextWriter writer)
        {
            writer.WriteEndTag("Body");
            writer.WriteEndTag("HTML");
        }
        #endregion

        #region Property (프로퍼티)

        #endregion
    }        
  
}