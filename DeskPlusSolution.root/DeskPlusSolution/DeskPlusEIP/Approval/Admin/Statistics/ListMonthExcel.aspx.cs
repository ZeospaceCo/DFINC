using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Statistics
{
    public partial class ListMonthExcel : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public int totCnt;      //총카운트
        public string TabMenu = "";    // 양식별(Form), 부서별(Dept), 개인별(User)
        public string sDate = "", eDate = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Statistics.ListMonthExcel).Assembly;	//어셈블리 선언
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

            LoadExcelData(TabMenu, sDate, eDate, SearchField, SearchValue, totCnt);
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
            BaseParam();

            totCnt = GetDecryptParam("totCnt", "0").ToNumber<int>();
            TabMenu = GetDecryptParam("tabMenu", "Form");

            int years = GetDecryptParam("years", DateTime.Today.Year.ToString()).ToNumber<int>();
            if (years > 1900)
            {
                sDate = years.ToString() + "-01-01";         // 2012-01-01
                eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            }
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
        private void LoadExcelData(string _tabMenu, string _sDate, string _eDate, string _searchField, string _searchValue, int _totCnt)
        {
            DataSet ds = GetList(_tabMenu, _sDate, _eDate, _searchField, _searchValue, _totCnt);
            if (ds != null)
            {
                DataTable dtMain = ds.Tables[0];    // 양식별
                DataTable dtSub = ds.Tables[1];     // 년월건수

                List<BE.StatisticsInfo> uobj = this.GetItems(dtMain, dtSub, false);

                StringBuilder sb = new StringBuilder();

                #region 타이틀
                sb.Append(@"
                <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
                <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
                </head><body>");

                sb.Append("<table border='1' cellspacing='0' class='table_stat'>");
                sb.Append("<caption></caption>");
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='2' scope='row'><span class='spStatMonth0'>");
                sb.Append(this.GetResource("StatMonth0"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth1'>");
                sb.Append(this.GetResource("StatMonth1"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth2'>");
                sb.Append(this.GetResource("StatMonth2"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth3'>");
                sb.Append(this.GetResource("StatMonth3"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth4'>");
                sb.Append(this.GetResource("StatMonth4"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth5'>");
                sb.Append(this.GetResource("StatMonth5"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth6'>");
                sb.Append(this.GetResource("StatMonth6"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth7'>");
                sb.Append(this.GetResource("StatMonth7"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth8'>");
                sb.Append(this.GetResource("StatMonth8"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth9'>");
                sb.Append(this.GetResource("StatMonth9"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth10'>");
                sb.Append(this.GetResource("StatMonth10"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth11'>");
                sb.Append(this.GetResource("StatMonth11"));
                sb.Append("</span></th>");
                sb.Append("<th><span class='spStatMonth12'>");
                sb.Append(this.GetResource("StatMonth12"));
                sb.Append("</span></th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                #endregion

                #region 내용
                foreach (BE.StatisticsInfo o in uobj)
                {
                    #region 완료
                    sb.Append("<tr>");
                    sb.Append("<th rowspan='5' scope='rowgroup' style='width:150px;'>");
                    sb.Append(o.NameBase);
                    sb.Append("<br />");
                    sb.Append("<span class='txt_point'>(<span class='spStatTot'>");
                    sb.Append(o.Cnt.ToString());
                    sb.Append("</span>");
                    sb.Append("<span class='spStatUnit'>");
                    sb.Append(this.GetResource("StatUnit"));
                    sb.Append("</span>)</span>");
                    sb.Append("</th>");
                    sb.Append("<th scope='row' style='width:50px;'><span class='spStatC'>");
                    sb.Append(this.GetResource("StatC"));
                    sb.Append("</span></th>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].January.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].February.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].March.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].April.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].May.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].June.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].July.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].Auguest.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].September.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].October.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].November.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[0].December.ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    #endregion

                    #region 진행
                    sb.Append("<tr>");
                    sb.Append("<th scope='row'><span class='spStatW'>");
                    sb.Append(this.GetResource("StatW"));
                    sb.Append("</span></th>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].January.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].February.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].March.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].April.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].May.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].June.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].July.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].Auguest.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].September.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].October.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].November.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[1].December.ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    #endregion

                    #region 반려

                    sb.Append("<tr>");
                    sb.Append("<th scope='row'><span class='spStatR'>");
                    sb.Append(this.GetResource("StatR"));
                    sb.Append("</span></th>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].January.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].February.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].March.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].April.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].May.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].June.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].July.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].Auguest.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].September.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].October.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].November.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[2].December.ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    #endregion

                    #region 보류

                    sb.Append("<tr>");
                    sb.Append("<th scope='row'><span class='spStatD'>");
                    sb.Append(this.GetResource("StatD"));
                    sb.Append("</span></th>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].January.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].February.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].March.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].April.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].May.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].June.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].July.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].Auguest.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].September.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].October.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].November.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[3].December.ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    #endregion

                    #region 합계

                    sb.Append("<tr>");
                    sb.Append("<th scope='row' class='sum'><span class='spStatSum'>");
                    sb.Append(this.GetResource("StatSum"));
                    sb.Append("</span></th>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].January.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].February.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].March.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].April.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].May.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].June.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].July.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].Auguest.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].September.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].October.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].November.ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(o.ArrMonthInfo[4].December.ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    #endregion

                }
                #endregion

                #region 하단
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</body></html>");
                #endregion

                ltCont.Text = sb.ToString();

                Response.ClearHeaders();
                string today = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString().PadLeft(2, '0');
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + today + "_Statistics.xls\"");
                //Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentEncoding = System.Text.UnicodeEncoding.UTF8;
                // Response.Write(this.GetResource("StatSub"));
                //Response.Write(Environment.NewLine);

                Response.Write(sb.ToString());
                //Response.Flush();
                //Response.Close();
                Response.End();
            }
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
        private DataSet GetList(string _tabMenu, string _sDate, string _eDate, string _searchField, string _searchValue, int _totCnt)
        {
            DataSet ds;
            DAL.Statistics dal = new DAL.Statistics();
            switch (_tabMenu.ToUpper())
            {
                case "FORM":
                    ds = dal.GetMonthFormList(CurrentUserInfo.CompanyID, _totCnt, 0, _sDate, _eDate, _searchField, _searchValue);
                    break;
                case "DEPT":
                    ds = dal.GetMonthDeptList(CurrentUserInfo.CompanyID, _totCnt, 0, _sDate, _eDate, _searchField, _searchValue);
                    break;
                case "USER":
                    ds = dal.GetMonthUserList(CurrentUserInfo.CompanyID, _totCnt, 0, _sDate, _eDate, _searchField, _searchValue);
                    break;
                default:
                    ds = dal.GetMonthFormList(CurrentUserInfo.CompanyID, _totCnt, 0, _sDate, _eDate, _searchField, _searchValue);
                    break;
            }
            return ds;
        }

        /// <summary>
        /// 공통-월별처리
        /// </summary>
        /// <param name="_statelist"></param>
        /// <returns></returns>
        private List<BE.MonthInfo> SetMonthInfo(List<BE.StateInfo> _statelist)
        {
            if (_statelist == null) return null;

            List<BE.MonthInfo> arrlist = new List<BE.MonthInfo>();
            BE.MonthInfo cInfo = new BE.MonthInfo();
            BE.MonthInfo wInfo = new BE.MonthInfo();
            BE.MonthInfo rInfo = new BE.MonthInfo();
            BE.MonthInfo dInfo = new BE.MonthInfo();
            BE.MonthInfo sumInfo = new BE.MonthInfo();

            foreach (BE.StateInfo info in _statelist)
            {
                #region 월 체크

                /* 
                   기안(진행) [0] - W,I,P
                   승인(완료) [1] - C
                   반려 [2] - R
                   보류 [3] - D,H 
                 */


                if (info.Month.Equals(1))
                {
                    cInfo.January = info.C;
                    wInfo.January = info.W + info.I + info.P;
                    rInfo.January = info.R;
                    dInfo.January = info.D + info.H;
                }
                if (info.Month.Equals(2))
                {
                    cInfo.February = info.C;
                    wInfo.February = info.W + info.I + info.P;
                    rInfo.February = info.R;
                    dInfo.February = info.D + info.H;
                }
                if (info.Month.Equals(3))
                {
                    cInfo.March = info.C;
                    wInfo.March = info.W + info.I + info.P;
                    rInfo.March = info.R;
                    dInfo.March = info.D + info.H;
                }
                if (info.Month.Equals(4))
                {
                    cInfo.April = info.C;
                    wInfo.April = info.W + info.I + info.P;
                    rInfo.April = info.R;
                    dInfo.April = info.D + info.H;
                }
                if (info.Month.Equals(5))
                {
                    cInfo.May = info.C;
                    wInfo.May = info.W + info.I + info.P;
                    rInfo.May = info.R;
                    dInfo.May = info.D + info.H;
                }
                if (info.Month.Equals(6))
                {
                    cInfo.June = info.C;
                    wInfo.June = info.W + info.I + info.P;
                    rInfo.June = info.R;
                    dInfo.June = info.D + info.H;
                }
                if (info.Month.Equals(7))
                {
                    cInfo.July = info.C;
                    wInfo.July = info.W + info.I + info.P;
                    rInfo.July = info.R;
                    dInfo.July = info.D + info.H;
                }
                if (info.Month.Equals(8))
                {
                    cInfo.Auguest = info.C;
                    wInfo.Auguest = info.W + info.I + info.P;
                    rInfo.Auguest = info.R;
                    dInfo.Auguest = info.D + info.H;
                }
                if (info.Month.Equals(9))
                {
                    cInfo.September = info.C;
                    wInfo.September = info.W + info.I + info.P;
                    rInfo.September = info.R;
                    dInfo.September = info.D + info.H;
                }
                if (info.Month.Equals(10))
                {
                    cInfo.October = info.C;
                    wInfo.October = info.W + info.I + info.P;
                    rInfo.October = info.R;
                    dInfo.October = info.D + info.H;
                }
                if (info.Month.Equals(11))
                {
                    cInfo.November = info.C;
                    wInfo.November = info.W + info.I + info.P;
                    rInfo.November = info.R;
                    dInfo.November = info.D + info.H;
                }
                if (info.Month.Equals(12))
                {
                    cInfo.December = info.C;
                    wInfo.December = info.W + info.I + info.P;
                    rInfo.December = info.R;
                    dInfo.December = info.D + info.H;
                }
                #endregion
            }

            sumInfo.January = cInfo.January + wInfo.January + rInfo.January + dInfo.January;
            sumInfo.February = cInfo.February + wInfo.February + rInfo.February + dInfo.February;
            sumInfo.March = cInfo.March + wInfo.March + rInfo.March + dInfo.March;
            sumInfo.April = cInfo.April + wInfo.April + rInfo.April + dInfo.April;
            sumInfo.May = cInfo.May + wInfo.May + rInfo.May + dInfo.May;
            sumInfo.June = cInfo.June + wInfo.June + rInfo.June + dInfo.June;
            sumInfo.July = cInfo.July + wInfo.July + rInfo.July + dInfo.July;
            sumInfo.Auguest = cInfo.Auguest + wInfo.Auguest + rInfo.Auguest + dInfo.Auguest;
            sumInfo.September = cInfo.September + wInfo.September + rInfo.September + dInfo.September;
            sumInfo.October = cInfo.October + wInfo.October + rInfo.October + dInfo.October;
            sumInfo.November = cInfo.November + wInfo.November + rInfo.November + dInfo.November;
            sumInfo.December = cInfo.December + wInfo.December + rInfo.December + dInfo.December;

            arrlist.Add(cInfo);
            arrlist.Add(wInfo);
            arrlist.Add(rInfo);
            arrlist.Add(dInfo);
            arrlist.Add(sumInfo);

            return arrlist;
        }

        /// <summary>
        /// 공통-월별현황리스트
        /// </summary>
        /// <param name="_dtMain"></param>
        /// <param name="_dtSub"></param>
        /// <returns></returns>
        private List<BE.StatisticsInfo> GetItems(DataTable _dtMain, DataTable _dtSub, bool _isUser)
        {
            if (_dtMain == null || _dtSub == null) return null;
            //if (_dtMain.Rows.Count.Equals(0) || _dtSub.Rows.Count.Equals(0)) return null;

            // 서브리스트 처리
            List<BE.StateInfo> listStat = new List<BE.StateInfo>();
            foreach (DataRow dr in _dtSub.Rows)
            {
                BE.StateInfo info = new BE.StateInfo();
                info.KeyID = dr["KeyID"].ToString().ToNumber<int>();
                info.Years = dr["Years"].ToString().ToNumber<int>();
                info.Month = dr["Months"].ToString().ToNumber<int>();
                info.C = dr["C"].ToString().ToNumber<int>();
                info.W = dr["W"].ToString().ToNumber<int>();
                info.I = dr["I"].ToString().ToNumber<int>();
                info.R = dr["R"].ToString().ToNumber<int>();
                info.H = dr["H"].ToString().ToNumber<int>();
                info.D = dr["D"].ToString().ToNumber<int>();
                info.P = dr["P"].ToString().ToNumber<int>();

                listStat.Add(info);
            }

            List<BE.StatisticsInfo> uobj = new List<BE.StatisticsInfo>();
            foreach (DataRow dr in _dtMain.Rows)
            {
                BE.StatisticsInfo oInfo = new BE.StatisticsInfo();

                oInfo.Row = dr["RowID"].ToString();
                oInfo.KeyID = dr["KeyID"].ToString().ToNumber<int>();
                oInfo.Cnt = dr["Cnt"].ToString().ToNumber<int>();
                oInfo.NameBase = (dr["NameBase"].ToString().Equals("")) ? dr["NameBaseOrg"].ToString() : GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                oInfo.UserID = (_isUser) ? oInfo.KeyID : 0;

                List<BE.StateInfo> subInfo = listStat.Where(x => x.KeyID.Equals(oInfo.KeyID)).ToList<BE.StateInfo>();
                oInfo.ArrMonthInfo = SetMonthInfo(subInfo);    //상태&월별처리

                uobj.Add(oInfo);
            }
            return uobj;
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}