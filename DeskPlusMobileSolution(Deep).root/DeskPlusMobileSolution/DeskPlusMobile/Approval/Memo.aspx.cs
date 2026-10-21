using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class Memo : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string bPage = "";
        public int pageNo = 0;
        public string dID = "";
        public string searchValue = "";
        public int keyID = 0;
        public int cirDocID = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Memo).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadData();
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            bPage = GetDecryptParam("BPage", "");
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            dID = GetDecryptParam("DID", "0");
            searchValue = GetDecryptParam("Sv", "");
            keyID = GetDecryptParam("KeyID", "0").ToNumber<int>();
            cirDocID = GetDecryptParam("CirDocID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-23
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
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-23
        /// </summary>
        /// <param name="_dID"></param>
        public void LoadData()
        {
            LoadParam();

            string proc = "";

            if (bPage == "Forward")
            {
                proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ForwardDoc");
                proc = string.Format(proc, CompanyID, dID, keyID);


            }
            else
            {
                proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_CirDoc");
                proc = string.Format(proc, CompanyID, cirDocID);
            }

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetMemo", null);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    memo.Value = dr["Memo"].ToString().ToReverseXSS();
                }
            }
        }

        public string Today { get; set; }

    }
}