using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Statistics.Web.Content.Attach
{
    public partial class ExcelDown : Base.BasePageStatistics
    {
        #region 변수설정
        public int totCnt;      //총카운트
        public string TabMenu = "";    // 양식별(Form), 부서별(Dept), 개인별(User)
        public string TargetYear = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Statistics.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Statistics.Web.Content.Attach.ExcelDown).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            LoadExcelData(TabMenu, SearchField, SearchValue);
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            //totCnt = GetDecryptParam("totCnt", "0").ToNumber<int>();
            Order = (Order.Length == 0) ? "GroupGB  " : Order;
            TabMenu = GetDecryptParam("tabMenu", "ATTACHM");
            TargetYear = GetDecryptParam("targetYear", DateTime.Now.Year.ToString());
            SearchField = GetDecryptParam("searchFiled", "");
            SearchValue = GetDecryptParam("searchValue", "");

        }


        /// <summary>
        /// 데이타가져오기
        /// </summary>
        /// <param name="_tabMenu"></param>
        /// <param name="_sDate"></param>
        /// <param name="_eDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_totCnt"></param>
        private void LoadExcelData(string _tabMenu, string _searchField, string _searchValue)
        {
            DataSet dsH = GetListH(_tabMenu, _searchField, _searchValue, TargetYear);
            DataSet ds = GetList(_tabMenu, _searchField, _searchValue, TargetYear);
            if (ds != null)
            {

                StringBuilder sb = new StringBuilder();

                string fileNM = string.Empty;
                string titleNM = string.Empty;
                string searchGubun = string.Empty;
                string subGubun = string.Empty;
                string headerTitle = string.Empty;

                switch (_tabMenu.ToUpper())
                {
                    case "ATTACHM":
                        fileNM = "ModuleStats";
                        titleNM = "*" + this.GetResource("MolduleDownStats");
                        searchGubun = this.GetResource("ModuleNM");
                        subGubun = "Module";
                        headerTitle = this.GetResource("TotalDownLoadCount");
                        break;
                    case "ATTACHU":
                        fileNM = "UserStats";
                        titleNM = "*" + this.GetResource("UserDownStats");
                        searchGubun = this.GetResource("UserNM");
                        subGubun = "User";
                        headerTitle = this.GetResource("TotalDownLoadCount");
                        break;
                    case "ATTACHD":
                        fileNM = "DeptDownStats";
                        titleNM = "*" + this.GetResource("DeptDownStats");
                        searchGubun = this.GetResource("DeptName");
                        subGubun = "Dept";
                        headerTitle = this.GetResource("TotalDownLoadCount");
                        break;
                    case "ATTACHF":
                        fileNM = "FileStats";
                        titleNM = "*" + this.GetResource("FileDownStats");
                        searchGubun = this.GetResource("FileNM");
                        subGubun = "File";
                        headerTitle = this.GetResource("TotalDownLoadCount");
                        break;
                    case "ATTACHMFL":
                        fileNM = "MFileLengthStats";
                        titleNM = "*" + this.GetResource("MFileLengthStats");
                        searchGubun = this.GetResource("ModuleNM");
                        subGubun = "Module";
                        headerTitle = this.GetResource("TotalFileLength");
                        break;
                    default:
                        fileNM = "ModuleStats";
                        titleNM = "*" + this.GetResource("MolduleDownStats");
                        searchGubun = this.GetResource("ModuleNM");
                        subGubun = "Module";
                        headerTitle = this.GetResource("TotalDownLoadCount");
                        break;
                }


                #region 타이틀

                sb.Append("<table border='1' cellspacing='0' class='table_stat'>");
                sb.Append("<caption></caption>");
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th>");
                sb.Append(searchGubun);
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("1Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("2Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("3Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("4Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("5Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("6Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("7Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("8Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("9Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("10Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("11Month"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(this.GetResource("12Month"));
                sb.Append("</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                // 헤더소계 값
                sb.Append("<tr>");
                sb.Append("<th>");
                sb.Append(headerTitle);
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth1"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth2"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth3"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth4"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth5"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth6"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth7"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth8"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth9"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth10"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth11"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("<th>");
                sb.Append(dsH.Tables[0].Rows[0]["CntMonth12"].ToString().ToNumber<int>().ToString("#,##0"));
                sb.Append("</th>");
                sb.Append("</tr>");
                #endregion

                #region 내용

                string subTxt = string.Empty;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    switch (subGubun)
                    {
                        case "Module":
                            subTxt = GetTagInfo(ds.Tables[0].Rows[i]["SearchGubun"].ToString());
                            break;
                        case "User":
                            subTxt = this.GetLangName(
                                    ds.Tables[0].Rows[i]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameKo"].ToString().ToNull(),
                                    ds.Tables[0].Rows[i]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameJa"].ToString().ToNull(),
                                    ds.Tables[0].Rows[i]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameZhcn"].ToString().ToNull(),
                                    ds.Tables[0].Rows[i]["NameZhtw"].ToString().ToNull(), PreLang);
                            break;
                        case "Dept":
                            subTxt = this.GetLangName(
                                    ds.Tables[0].Rows[i]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptKo"].ToString().ToNull(),
                                    ds.Tables[0].Rows[i]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptJa"].ToString().ToNull(),
                                    ds.Tables[0].Rows[i]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptZhcn"].ToString().ToNull(),
                                    ds.Tables[0].Rows[i]["DeptZhtw"].ToString().ToNull(), PreLang);
                            break;
                        case "File":
                            subTxt = ds.Tables[0].Rows[i]["SearchGubun"].ToString();
                            break;
                    }

                    sb.Append("<tr>");
                    sb.Append("<th>");
                    sb.Append(subTxt);
                    sb.Append("</th>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth1"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth2"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth3"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth4"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth5"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth6"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth7"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth8"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth9"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth10"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth11"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(ds.Tables[0].Rows[i]["CntMonth12"].ToString().ToNumber<int>().ToString("#,##0"));
                    sb.Append("</td>");
                    sb.Append("</tr>");


                }
                #endregion

                #region 하단
                sb.Append("</tbody>");
                sb.Append("</table>");
                #endregion

                ltCont.Text = sb.ToString();

                Response.ClearHeaders();
                string today = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString().PadLeft(2, '0');
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + today + "_" + fileNM + ".xls\"");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.UnicodeEncoding.UTF8;

                Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
                                <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
                                </head><body>");

                Response.Write(titleNM);
                Response.Write(Environment.NewLine);
                Response.Write(sb.ToString());
                Response.Write("</body></html>");

                Response.Flush();
                Response.Close();
                Response.End();
            }
        }


        /// <summary>
        /// 헤더가져오기
        /// </summary>
        /// <param name="_tabMenu"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_totCnt"></param>
        /// <returns></returns>
        private DataSet GetListH(string _tabMenu, string _searchField, string _searchValue, string _targetYear)
        {
            DataSet ds;
            BE.AttachInfo AttachInfo = new BE.AttachInfo();
            DAL.Attach Attach = new DAL.Attach();

            AttachInfo.CompanyID = CurrentUserInfo.CompanyID;
            AttachInfo.TargetYear = _targetYear;
            AttachInfo.SearchStartDate = "";
            AttachInfo.SearchEndDate = "";
            AttachInfo.SearchField = _searchField;
            AttachInfo.SearchValue = _searchValue;
            AttachInfo.PageSize = 0;
            AttachInfo.PageSizePageNo = 0;
            AttachInfo.Order = Order;

            switch (_tabMenu.ToUpper())
            {
                case "ATTACHM":
                    ds = Attach.GetTotalMListExcel(AttachInfo);
                    break;
                case "ATTACHU":
                    ds = Attach.GetTotalUListExcel(AttachInfo);
                    break;
                case "ATTACHD":
                    ds = Attach.GetTotalDListExcel(AttachInfo);
                    break;
                case "ATTACHF":
                    ds = Attach.GetTotalFListExcel(AttachInfo);
                    break;
                case "ATTACHMFL":
                    ds = Attach.GetTotalMFLListExcel(AttachInfo);
                    break;
                default:
                    ds = Attach.GetTotalMListExcel(AttachInfo);
                    break;
            }
            return ds;
        }


        /// <summary>
        /// 리스트가져오기
        /// </summary>
        /// <param name="_tabMenu"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_totCnt"></param>
        /// <returns></returns>
        private DataSet GetList(string _tabMenu, string _searchField, string _searchValue, string _targetYear)
        {
            DataSet ds;
            BE.AttachInfo AttachInfo = new BE.AttachInfo();
            DAL.Attach Attach = new DAL.Attach();

            AttachInfo.CompanyID = CurrentUserInfo.CompanyID;
            AttachInfo.TargetYear = _targetYear;
            AttachInfo.SearchStartDate = "";
            AttachInfo.SearchEndDate = "";
            AttachInfo.SearchField = _searchField;
            AttachInfo.SearchValue = _searchValue;
            AttachInfo.PageSize = 0;
            AttachInfo.PageSizePageNo = 0;
            AttachInfo.Order = Order;

            switch (_tabMenu.ToUpper())
            {
                case "ATTACHM":
                    ds = Attach.GetExcelMList(AttachInfo);
                    break;
                case "ATTACHU":
                    ds = Attach.GetExcelUList(AttachInfo);
                    break;
                case "ATTACHD":
                    ds = Attach.GetExcelDList(AttachInfo);
                    break;
                case "ATTACHF":
                    ds = Attach.GetExcelFList(AttachInfo);
                    break;
                case "ATTACHMFL":
                    ds = Attach.GetExcelMFLList(AttachInfo);
                    break;
                default:
                    ds = Attach.GetExcelMList(AttachInfo);
                    break;
            }
            return ds;
        }

        private string GetTagInfo(string _approvalTag)
        {

            string rtnval = "";
            switch (_approvalTag)
            {
                case "Approval":
                    rtnval = this.GetResource("Approval");
                    break;
                case "Opinion":
                    rtnval = this.GetResource("ModuleNM");
                    break;
                case "WebHard":
                    rtnval = this.GetResource("WebHard");
                    break;
                case "ResourceManager":
                    rtnval = this.GetResource("ResourceManager");
                    break;
                case "PMS":
                    rtnval = this.GetResource("PMS");
                    break;
                case "Pims":
                    rtnval = this.GetResource("Pims");
                    break;
                case "Community":
                    rtnval = this.GetResource("Community");
                    break;
                case "Board":
                    rtnval = this.GetResource("Board");
                    break;
                case "Coop":
                    rtnval = this.GetResource("Coop");
                    break;
                default:
                    rtnval = this.GetResource("Etc") + "(" + _approvalTag + ")";
                    break;
            }
            return rtnval;
        }


        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}