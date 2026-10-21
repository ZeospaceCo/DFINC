using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using InfoSoftGlobal;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Admin.Statistic
{
    public partial class LineChart : Base.BasePageBoardAdmin
    {
        private Dictionary<int, int> Dic = new Dictionary<int, int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Statistic.LineChart).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                string yy = GetDecryptParam("year", System.DateTime.Now.Year.ToString());

                DAL.Statistic dal = new DAL.Statistic();
                DataTableReader dtr = dal.GetStatYYMM(CurrentUserInfo.CompanyID, yy);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Dic.Add(dtr["MM"].ToString().ToNumber<int>(), dtr["Cnt"].ToString().ToNumber<int>());
                        }
                    }
                }

                //FusionChart(yy);

                ListChart(yy);

                LoadYear(yy);

                ChartNwagon(yy);
            }
        }

        /// <summary>
        /// 막대그래프
        /// </summary>
        /// <param name="yy">The yy.</param>
        private void ChartNwagon(string _yy)
        {
            StringBuilder sbTitle = new StringBuilder();
            string chartValue = ""; //차트값

            bool isOk = false;

            for (int i = 1; i <= 12; i++)
            {
                isOk = false;

                foreach (var o in Dic)
                {
                    if (o.Key == i)
                    {
                        sbTitle.Append(@"""" + _yy + "-" + i.ToString() + @"""").Append(",");
                        chartValue += String.Format("{0},", o.Value);

                        if (o.Value > CntTot) { CntTot = o.Value; }

                        isOk = true;
                        break;
                    }
                }

                if (isOk == false)
                {
                    sbTitle.Append(@"""" + _yy + "-" + i.ToString() + @"""").Append(",");
                    chartValue += String.Format("{0},", 0);
                }
            }

            ChartTitle = sbTitle.ToString().TrimEnd(','); //x축 값
            if (chartValue != "") ChartValue = chartValue.TrimEnd(','); //데이터값            

            if (CntTot > 0)
            {
                #region 10 줄라인으로 증가값을 만든다. (shimpark)

                double iVal = 0.1;
                iVal = CntTot / 10.0;
                if (iVal < 1) iVal = 1;
                iVal = Math.Round(iVal);

                #endregion 10 줄라인으로 증가값을 만든다. (shimpark)
                
                ChartIncrement = Convert.ToInt32(iVal); //차트 증가값
            }
        }

        /// <summary>
        /// 년도 바인딩
        /// </summary>
        /// <param name="yy">The yy.</param>
        private void LoadYear(string yy)
        {
            ddlYear.Items.Clear();

            //이전 10년치
            for (int i = System.DateTime.Now.Year - 10; i < System.DateTime.Now.Year; i++) ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlYear.Items.Add(new ListItem(System.DateTime.Now.Year.ToString(), System.DateTime.Now.Year.ToString()));
            //이후 10년치
            for (int i = System.DateTime.Now.Year + 1; i < System.DateTime.Now.Year + 10; i++) ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));

            ddlYear.SelectedValue = yy;
        }

        /// <summary>
        /// 표 그리기
        /// </summary>
        /// <param name="yy">The yy.</param>
        private void ListChart(string yy)
        {
            string tmp = "";
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i <= 12; i++)
            {
                tmp = string.Format(@"<tr><td scope=""row"">{0}</td><td>0</td></tr>", yy + "-" + i.ToString());

                foreach (var o in Dic)
                {
                    if (o.Key == i)
                    {
                        tmp = string.Format(@"<tr><td>{0}</td><td>{1}</td></tr>", yy + "-" + o.Key.ToString(), o.Value);
                        break;
                    }
                }

                sb.Append(tmp);
            }

            ltlConent.Text = sb.ToString();
        }

        /// <summary>
        /// 퓨전 막대 그래프 그리기
        /// </summary>
        /// <param name="_yy">The _yy.</param>
        private void FusionChart(string _yy)
        {
            StringBuilder xmlData = new StringBuilder();

            /*
             * subCaption : 상단제목; subCaption : 상단제목 바로 아래 제목; numberSuffix : 왼쪽 숫자 옆 기재 (예 : 개수)
             */
            xmlData.Append(String.Format("<chart caption='{0}' subCaption='{1}' pieSliceDepth='30' showBorder='1' formatNumberScale='0' numberSuffix=' {2}' >",
                _yy + "년 월별 현황", "", ""));            

            for (int i = 1; i <= 12; i++)
            {
                bool isval = false;
                foreach (var o in Dic)
                {
                    if (o.Key == i)
                    {
                        xmlData.AppendFormat("<set label='{0}' value='{1}' link='JavaScript:updateChart({2})' />", _yy + "-" + i.ToString(), o.Value, "");
                        isval = true;
                        break;
                    }
                }

                if (!isval)
                    xmlData.AppendFormat("<set label='{0}' value='{1}' link='JavaScript:updateChart({2})' />", _yy + "-" + i.ToString(), "0", "");
            }

            xmlData.Append("</chart>");
            //ltlChart.Text = FusionCharts.RenderChart("/DeskPlusEIP/Util/Component/FusionCharts/Column3D.swf", "", xmlData.ToString(), "FactorySum", "800", "600", false, true);
        }

        /// <summary>
        /// </summary>
        public string ChartTitle
        {
            get { return GetViewStateString("ChartTitle", ""); }
            set { ViewState["ChartTitle"] = value; }
        }

        /// <summary>
        /// </summary>
        public int CntTot
        {
            get { return GetViewStateInt("CntTot", "0"); }
            set { ViewState["CntTot"] = value; }
        }

        /// <summary>
        /// 챠트 값
        /// </summary>
        /// <value>
        /// The chart value.
        /// </value>
        public string ChartValue
        {
            get { return GetViewStateString("ChartValue", ""); }
            set { ViewState["ChartValue"] = value; }
        }

        /// <summary>
        /// 차트 증가값
        /// </summary>
        /// <value>
        /// The chart increment.
        /// </value>
        public int ChartIncrement
        {
            get { return GetViewStateInt("ChartIncrement", "1"); }
            set { ViewState["ChartIncrement"] = value; }
        }
    }
}