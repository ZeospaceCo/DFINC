using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

using System.IO;
using System.Data;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{

    public partial class ListExcel : Base.BasePagePim
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ListExcel).Assembly;	//어셈블리 선언
            #endregion

            #region 기본정보세팅
            UserID = CurrentUserInfo.UserID;
            CompanyID = CurrentUserInfo.CompanyID;
            #endregion

            if (!IsPostBack)
            {
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
            LoadExcel();
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            SearchKind = GetDecryptParam("SearchKind", "");
            SerachValue = GetDecryptParam("SerachValue", "");
            StartDate = GetDecryptParam("StartDate", "");
            EndDate = GetDecryptParam("EndDate", "");
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        private void LoadExcel()
        {
            DataGrid dg = new DataGrid();

            //Zeospace.DeskPlus.Pim.Web.Ws.Schedule.ListSearch ws = new Ws.Schedule.ListSearch();
            //List<object> list = ws.GetALLList(hdexcel.Value.ToString());
            List<object> list = GetSearchList();
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

        public List<object> GetSearchList()
        {
            List<object> listobj = new List<object>();

            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;

            BE.ScheduleInfo _info = new BE.ScheduleInfo();

            _info.CompanyID = CurrentUserInfo.CompanyID;
            _info.UserID = CurrentUserInfo.UserID;

            switch (GetDecryptParam("StartDate", "").ToString())
            {
                case "0":  //전체
                    _info.StartTime = DateTime.Parse("2000-01-01");
                    _info.EndTime = DateTime.Now.AddDays(1);
                    break;
                case "W":
                    _info.StartTime = DateTime.Now.AddDays(-7);
                    _info.EndTime = DateTime.Now;
                    break;
                case "M":
                    _info.StartTime = DateTime.Now.AddMonths(-1);
                    _info.EndTime = DateTime.Now;
                    break;
                default:
                    _info.StartTime = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _info.EndTime = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                    break;
            }

            string SearchKind = GetDecryptParam("SearchKind", "").ToString().Trim();

            if (SearchKind == "Title")
            {
                //제목
                _info.Title = GetDecryptParam("SearchValue", "").ToString().Trim();
            }
            else if (SearchKind == "Name")
            {
                //이름
                _info.UserName = GetDecryptParam("SearchValue", "").ToString().Trim();
            }

            _info.DeptID = CurrentUserInfo.DeptID;

            DAL.Schedule dal = new DAL.Schedule();

            DataTableReader dtr = dal.GetSearchExcel(_info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo data = new BE.ScheduleInfo();
                        data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        data.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();

                        string _name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                         dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                         dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                         );
                        string _title = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                            dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang
                            );

                        data.UserName = _name + "(" + _title + ")";
                        data.Title = dtr["Title"].ToString();


                        data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString();
                        data.stringENDTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortTimeString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortTimeString();

                        data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        //종류
                        data.OpenType = dtr["kind"].ToString();

                        switch (data.OpenType)
                        {
                            case "-1"://나의일정

                                data.KindDesk = GetResource("MYSCHEDULE");// "나의일정";
                                break;
                            case "0"://개인일정

                                data.KindDesk = GetResource("pim1");//"개인일정";
                                break;
                            case "3"://부서일정

                                data.KindDesk = GetResource("pim2");//"부서일정";
                                break;
                            case "1"://공유일정

                                data.KindDesk = GetResource("pim3");//"공유일정";

                                break;
                            case "2"://전체일정
                                data.KindDesk = GetResource("pim4");//"전체일정";
                                break;
                            case "8"://할일
                                data.KindDesk = GetResource("TODO");//"할일";
                                data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString();
                                data.stringENDTime = "";
                                // data.KindDesk = dtr["Priority"].ToString();
                                switch (dtr["AlertType"].ToString())
                                {
                                    case "W":
                                        data.stringENDTime = GetResource("priority1");//"시작안함";
                                        break;
                                    case "I":
                                        data.stringENDTime = GetResource("priority2");//"진행중";
                                        break;
                                    case "E":
                                        data.stringENDTime = GetResource("priority3");//"완료";
                                        break;
                                    case "C":
                                        data.stringENDTime = GetResource("priority4");// "취소";
                                        break;
                                    case "T":
                                        data.stringENDTime = GetResource("priority5");//"위임";
                                        break;
                                    case "D":
                                        data.stringENDTime = GetResource("priority6");//"연기";
                                        break;
                                }
                                break;
                        }

                        listobj.Add(data);
                    }
                }
            }
            return listobj;
        }
        #endregion

        #region Property (프로퍼티)

        #endregion
    }

}