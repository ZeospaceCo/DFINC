using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.Task.Web.Home
{
    public partial class AdminMain : Base.BasePageTask
    {
        #region 변수설정
        public string Year = "";

        public int RecentPageNo = 0;
        public int ProcessPageNo = 0;
        public int DelayPageNo = 0;        
        public int ComPageNo = 0;
        public int ImpoPageNo = 0;
        #endregion

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Home.AdminMain).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-23
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            // 년도 가져오기
            ddlYear.Items.Clear();
            DAL.List dal = new DAL.List();
            var dtr = dal.GetYear(CurrentUserInfo.CompanyID);

            ddlYear.Items.Add(new ListItem(this.resMng.GetString("ListType0"), ""));

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ddlYear.Items.Add(new ListItem(dtr["Year"].ToString(), dtr["Year"].ToString()));
                    }
                }
            }

            DataSet ds = new DataSet();

            if (Year != "")
            {
                ddlYear.SelectedValue = Year;
                ds = dal.GetCnt(CurrentUserInfo.CompanyID, ddlYear.SelectedValue);
            }
            else
            {
                ds = dal.GetCnt(CurrentUserInfo.CompanyID);
            }          

            DataView dv = ds.Tables[0].DefaultView;
            rpList.DataSource = dv;
            rpList.DataBind();

            ChartMaxValue = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string name = this.GetLangName(
                            dr["ItemNameBase"].ToString().ToNull(), dr["ItemNameKo"].ToString().ToNull(),
                            dr["ItemNameEn"].ToString().ToNull(), dr["ItemNameJa"].ToString().ToNull(),
                            dr["ItemNameFr"].ToString().ToNull(), dr["ItemNameZhcn"].ToString().ToNull(),
                            dr["ItemNameZhtw"].ToString().ToNull(), PreLang);

                if (name.Length > 8)
                {
                    name = name.Substring(0, 8) + "..";
                }

                ChartTitle += "\"" + name + "\",";

                ChartValue += "[" + dr["I"].ToString() + "," + dr["D"].ToString() + "," + dr["C"].ToString() + "],";

                if (ChartMaxValue < dr["I"].ToString().ToNumber<int>())
                {
                    ChartMaxValue = dr["I"].ToString().ToNumber<int>();
                }

                if (ChartMaxValue < dr["D"].ToString().ToNumber<int>())
                {
                    ChartMaxValue = dr["D"].ToString().ToNumber<int>();
                }

                if (ChartMaxValue < dr["C"].ToString().ToNumber<int>())
                {
                    ChartMaxValue = dr["C"].ToString().ToNumber<int>();
                }

                CntTot += dr["Tot"].ToString().ToNumber<int>();
                CntI += dr["I"].ToString().ToNumber<int>();
                CntD += dr["D"].ToString().ToNumber<int>();
                CntC += dr["C"].ToString().ToNumber<int>();
            }

            ChartTitle = (ChartTitle != null) ? ChartTitle.TrimEnd(',') : "\"" + resMng.GetString("NoTask") + "\"";
            ChartValue = (ChartValue != null) ? ChartValue.TrimEnd(',') : "0";

            if (ChartMaxValue <= 25)
            {
                ChartMaxValue = 25;
                ChartIncre = 5;
            }
            else
            {
                //10 단위 올림
                ChartMaxValue = ((int)Math.Ceiling(ChartMaxValue / 10.0)) * 10;
                ChartIncre = ChartMaxValue / 5;
            }
        }

        public string GetItemName(string _base, string _ko, string _en, string _ja, string _fr, string _zhcn, string _zhtw)
        {
            string name = this.GetLangName(_base, _ko, _en, _ja, _fr, _zhcn, _zhtw, PreLang);
            return name;
        }
    
        #endregion

        #region (함수)

        /// <summary>
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                //cbtnSearch.Value = this.resMng.GetString("OrganBtnSearch");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-23
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            Year = GetDecryptParam("year", "");
            RecentPageNo = GetDecryptParam("recentPageNo", "0").ToNumber<int>();
            ProcessPageNo = GetDecryptParam("processPageNo", "0").ToNumber<int>();
            DelayPageNo = GetDecryptParam("delayPageNo", "0").ToNumber<int>();
            ComPageNo = GetDecryptParam("comPageNo", "0").ToNumber<int>();
            ImpoPageNo = GetDecryptParam("impoPageNo", "0").ToNumber<int>();
            
        }
      
        #endregion

        #region (프로퍼티)     
 
        /// <summary>
        /// </summary>
        public int CntTot
        {
            get { return GetViewStateInt("CntTot", "0"); }
            set { ViewState["CntTot"] = value; }
        }

        /// <summary>
        /// </summary>
        public int CntI
        {
            get { return GetViewStateInt("CntI", "0"); }
            set { ViewState["CntI"] = value; }
        }

        /// <summary>
        /// </summary>
        public int CntD
        {
            get { return GetViewStateInt("CntD", "0"); }
            set { ViewState["CntD"] = value; }
        }

        /// <summary>
        /// </summary>
        public int CntC
        {
            get { return GetViewStateInt("CntC", "0"); }
            set { ViewState["CntC"] = value; }
        }

        public string ChartTitle { get; set; }

        public string ChartValue { get; set; }

        public int ChartMaxValue { get; set; }

        public int ChartIncre { get; set; }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}