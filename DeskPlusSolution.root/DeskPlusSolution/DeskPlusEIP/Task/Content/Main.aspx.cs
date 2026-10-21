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

namespace Zeospace.DeskPlus.Task.Web.Content
{
    public partial class Main : Base.BasePageTask
    {
        #region 변수설정
        public string Year = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Content.Main).Assembly;	//어셈블리 선언
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

        #endregion

        #region (함수)

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            Year = GetDecryptParam("year", System.DateTime.Now.Year.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            // 년도 가져오기
            ddlYear.Items.Clear();            
            DAL.List dal = new DAL.List();
            var dtr = dal.GetYear(CurrentUserInfo.CompanyID);

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

            if (ddlYear.Items.Count == 0)
            {
                ddlYear.Items.Add(new ListItem(System.DateTime.Now.Year.ToString(), System.DateTime.Now.Year.ToString()));
            }

            ddlYear.SelectedValue = Year;

            // 차트 그리기
            StringBuilder sbX = new StringBuilder();
            StringBuilder sbY = new StringBuilder();
            DataSet ds = dal.GetCnt(CurrentUserInfo.CompanyID, ddlYear.SelectedValue);

            DataView dv = ds.Tables[0].DefaultView;
            rpList.DataSource = dv;
            rpList.DataBind();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string name = this.GetLangName(
                            dr["ItemNameBase"].ToString().ToNull(), dr["ItemNameKo"].ToString().ToNull(),
                            dr["ItemNameEn"].ToString().ToNull(), dr["ItemNameJa"].ToString().ToNull(),
                            dr["ItemNameFr"].ToString().ToNull(), dr["ItemNameZhcn"].ToString().ToNull(),
                            dr["ItemNameZhtw"].ToString().ToNull(), PreLang);


                sbX.Append(name);
                sbX.Append("|");

                sbY.Append(dr["Tot"].ToString());
                sbY.Append(",");
                sbY.Append(dr["I"].ToString());
                sbY.Append(",");
                sbY.Append(dr["D"].ToString());
                sbY.Append(",");
                sbY.Append(dr["C"].ToString());
                sbY.Append("|");

                if (TopCnt < dr["I"].ToString().ToNumber<int>())
                {
                    TopCnt = dr["I"].ToString().ToNumber<int>();
                }

                if (TopCnt < dr["D"].ToString().ToNumber<int>())
                {
                    TopCnt = dr["D"].ToString().ToNumber<int>();
                }

                if (TopCnt < dr["C"].ToString().ToNumber<int>())
                {
                    TopCnt = dr["C"].ToString().ToNumber<int>();
                }

                CntTot += dr["Tot"].ToString().ToNumber<int>();
                CntI += dr["I"].ToString().ToNumber<int>();
                CntD += dr["D"].ToString().ToNumber<int>();
                CntC += dr["C"].ToString().ToNumber<int>();
                CntBeforeI += dr["BeforeI"].ToString().ToNumber<int>();
                CntBeforeD += dr["BeforeD"].ToString().ToNumber<int>();
            }

            Graph1 = sbX.ToString().TrimEnd('|');
            Graph2 = sbY.ToString().TrimEnd('|');

            TopCnt = (TopCnt / 10) * 10 + 10;
        }

        public string GetItemName(string _base, string _ko, string _en, string _ja , string _fr, string _zhcn, string _zhtw)
        {
            string name = this.GetLangName(_base, _ko, _en, _ja, _fr, _zhcn,_zhtw, PreLang);
            return name;
        }

        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// </summary>
        public string Graph1
        {
            get { return GetViewStateString("Graph1", ""); }
            set { ViewState["Graph1"] = value; }
        }

        /// <summary>
        /// </summary>
        public string Graph2
        {
            get { return GetViewStateString("Graph2", ""); }
            set { ViewState["Graph2"] = value; }
        }

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

        /// <summary>
        /// </summary>
        public int CntBeforeI
        {
            get { return GetViewStateInt("CntBeforeI", "0"); }
            set { ViewState["CntBeforeI"] = value; }
        }

        /// <summary>
        /// </summary>
        public int CntBeforeD
        {
            get { return GetViewStateInt("CntBeforeD", "0"); }
            set { ViewState["CntBeforeD"] = value; }
        }

        /// <summary>
        /// </summary>
        public int TopCnt
        {
            get { return GetViewStateInt("TopCnt", "0"); }
            set { ViewState["TopCnt"] = value; }
        }

        #endregion Property (프로퍼티)

        #region (이벤트핸들러)
        #endregion
    }
}