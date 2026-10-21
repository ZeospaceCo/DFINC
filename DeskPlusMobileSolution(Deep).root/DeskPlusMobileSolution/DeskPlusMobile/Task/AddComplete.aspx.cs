using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Task
{
    public partial class AddComplete : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string bPage = "";
        public int pageNo = 1;
        public int listID = 0, groupID = 0;
        public string status = "";
        public string searchValue = "";
        public string startCalMonth = "";
        public string endCalMonth = "";
        public string bType = "";
        public string alarm;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Task.AddComplete).Assembly;	//어셈블리 선언
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

                LoadParam();

                LoadData();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            bPage = GetDecryptParam("BPage", "");
            bType = GetDecryptParam("BType", "");
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            listID = GetDecryptParam("ListID", "0").ToNumber<int>();
            groupID = GetDecryptParam("GroupID", "0").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            status = GetDecryptParam("Status", "");
            alarm = GetDecryptParam("Alarm", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-14
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-14
        /// 작성목적 : 완료정보 가져오기
        /// </summary>
        private void LoadData()
        {
            Zeospace.DeskPlusMobile.Web.Task.Ws.Data.List dal = new Ws.Data.List();
            DataSet ds = dal.GetList(CompanyID, listID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tbStartDate.Value = DateHelpers.GetCultureDateChar8(dr["StartDate"].ToString(), PreLangReal);
                    tbEndDate.Value = DateHelpers.GetCultureDateChar8(dr["EndDate"].ToString(), PreLangReal);

                    startCalMonth = dr["StartDate"].ToString().Substring(0, 4) + "-" + dr["StartDate"].ToString().Substring(4, 2);
                    endCalMonth = dr["EndDate"].ToString().Substring(0, 4) + "-" + dr["EndDate"].ToString().Substring(4, 2);
                }
            }
        }

        public string Today { get; set; }
    }
}