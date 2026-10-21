using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.Pim.Web.Anniversary
{
    public partial class List : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        
        public string TotalCount = "0";

        public int CompanyID = 0;
        public int UserID = 0;

        public string Searchkey = "";
        public string SearchValue = "";
        public int CurrentPage = 1;
        public string Order = "";
        public string Starttime = "";
        public string Endtime = "";
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
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Anniversary.List).Assembly;	//어셈블리 선언
            #endregion

            UserID = CurrentUserInfo.UserID;
            CompanyID = CurrentUserInfo.CompanyID;

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                SettingControl();
            }

            
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            //GetListCount();
        }

        private void LoadParam()
        {
            
            Searchkey = GetDecryptParam("Searchkey", "");
            SearchValue = GetDecryptParam("SearchValue", "");
            CurrentPage = GetDecryptParam("CurrentPage", "1").ToNumber<int>(); ;
            Starttime = GetDecryptParam("Starttime", "");
            Endtime = GetDecryptParam("Endtime", "");
            Order = GetDecryptParam("order", "");
        }

        #endregion

        #region (함수)

        private void SettingControl()
        {

            //divCategory
            //일정
            //할일
            ListItem ltholy1 = new ListItem(GetResource("ALL"), "0,2");
            ListItem ltholy2 = new ListItem(GetResource("schedule2"), "0");
            ListItem ltholy3 = new ListItem(GetResource("AllAnniversary"), "2");



            ddlCategory.Items.Add(ltholy1);
            ddlCategory.Items.Add(ltholy2);
            ddlCategory.Items.Add(ltholy3);


            ListItem lt1 = new ListItem(GetResource("Title"), "Title");
            ListItem lt2 = new ListItem(GetResource("Period"), "WriteDate");

            ddlSearchType.Items.Add(lt1);
            ddlSearchType.Items.Add(lt2);

        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnMySchedule.Text = GetResource("MYSCHEDULE");
                cbtnExcel.Text = GetResource("ExecelDown");
                cbtnSearch.Text = GetResource("SEARCHTEXT");
                cbtnAdd.Text = GetResource("ADD");
            }
        }
        



        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)



        protected void lbtnexcel_Click(object sender, EventArgs e)
        {
            try
            {

                DataGrid dg = new DataGrid();
                Ws.Anniversary.List ws = new Ws.Anniversary.List();
                //Ws.GetPim ws = new Ws.GetPim();
                List<object> list = ws.GetAnniversaryInfoList(hdexcel.Value.ToString());
                this.GetResourceManager();
                string[] arCulName = { GetResource("KIND"), GetResource("Title"), GetResource("marknameperiod"), GetResource("WRITER") }; //구분 제목 기간 작성자
                string[] arVal = new string[arCulName.Length];
                System.Data.DataTable dt = BLL.ExcelHelper.AddStringCulumns(arCulName);

                foreach (BE.DayInfo _data in list)
                {
                    arVal[0] = _data.CategoryDesc;
                    arVal[1] = _data.Title;
                    arVal[2] = _data.DateInfo;
                    arVal[3] = _data.WriteName;
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
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnexcel_Click", ex);
            }
        }


        #endregion
    }
}