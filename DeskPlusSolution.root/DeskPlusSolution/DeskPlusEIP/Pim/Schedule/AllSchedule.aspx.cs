using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;


namespace Zeospace.DeskPlus.Pim.Web.Schedule
{

    public partial class AllSchedule : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        public string TotalCount = "0";

        public string Searchkey = "";
        public string SearchValue = "";
        public int CurrentPage = 1;
        public string Order = "";
        public string Starttime = "";
        public string Endtime = "";
        public string UserList = "";

        public string Subject = "";
        public string name = "";
        public string SelectDateS = "";
        public string SelectDateE = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.AllSchedule).Assembly;	//어셈블리 선언
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

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (UserList.Length > 0)
                return;
                
            //GetListCount();
            DAL.OpenUser obiz = new DAL.OpenUser();

            DataTableReader odtr = obiz.GetOpenUserAll(CompanyID, UserID, CurrentUserInfo.DeptID);
            ///DataTableReader odtr = obiz.GetOpenUser(CompanyID, UserID);
            
            UserList += UserID.ToString() + ",";
            if (odtr != null)
            {
                if (odtr.HasRows)
                {
                    while (odtr.Read())
                    {
                        if (odtr["OpenUserID"].ToString().ToNumber<int>() > 0)
                        {
                            UserList += "P" + odtr["UserID"].ToString() + ",";
                        }
                        else
                        {
                            UserList += "M" + odtr["UserID"].ToString() + ",";
                        }
                         
                    }
                }
            }

            
            
                hdShareTemp.Value = UserList;
                hdShareList.Text = UserList;
            
            //

        }

        private void LoadParam()
        {
            Subject = GetDecryptParam("title", "");
            name = GetDecryptParam("name", "");
            Starttime = GetDecryptParam("StartDate", "");
            Endtime = GetDecryptParam("EndDate", "");
            UserList = GetDecryptParam("UserList", "");

            hdShareTemp.Value = UserList;
            hdShareList.Text = UserList;

            SelectDateS = GetDecryptParam("Date", "");

            if (SelectDateS.Length > 0)
            {
                SelectDateE = DateTime.Parse(SelectDateS).AddDays(1).ToShortDateString();
            }
        }

        #endregion

        #region (함수)

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
                CbtnMySchedule.Text = GetResource("MYSCHEDULE");
                CbtnTarget.Text = GetResource("InqueryObjSel");
                
            }
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


            ListItem lk1 = new ListItem(GetResource("Name"), "Name");
            ListItem lk2 = new ListItem(GetResource("Title"), "Title");

            ddlkey.Items.Add(lk2);
            ddlkey.Items.Add(lk1);

            
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
                Zeospace.DeskPlus.Pim.Web.Ws.Schedule.AllSchedule ws = new Ws.Schedule.AllSchedule();
                List<object> list = ws.GetAllUserScheduleList(hdexcel.Value.ToString());
                this.GetResourceManager();
                string[] arCulName = { GetResource("Name"), GetResource("KIND"), GetResource("DATE2"), GetResource("Title") };
                string[] arVal = new string[arCulName.Length];
                System.Data.DataTable dt = BLL.ExcelHelper.AddStringCulumns(arCulName);

                foreach (BE.ScheduleInfo _data in list)
                {
                    arVal[0] = _data.UserName;
                    arVal[1] = _data.KindDesk;
                    arVal[2] = _data.stringStartTime;
                    arVal[3] = _data.Title;
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