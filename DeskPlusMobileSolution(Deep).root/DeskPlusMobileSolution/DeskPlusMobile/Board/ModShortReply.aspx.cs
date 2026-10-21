using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;


namespace Zeospace.DeskPlusMobile.Web.Board
{
    public partial class ModShortReply : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int pageNo = 0;
        public int companyID = 0;
        public int cid = 0;
        public int id = 0;
        public string subject = "";
        public string searchValue;
        public string pCatID;
        public int shortReplyID = 0;
        public string alarm;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Board.ModShortReply).Assembly;	    //어셈블리 선언
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
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            cid = GetDecryptParam("CID", "10").ToNumber<int>();
            id = GetDecryptParam("ID", "10").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            pCatID = GetDecryptParam("PCatID", "");
            shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
            alarm = GetDecryptParam("Alarm", "");
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2012-03-23
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
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply biz
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply();

            DataSet dsReplyID = biz.GetShortReplyListID(companyID, cid, id, shortReplyID);
            if (dsReplyID != null)
            {
                foreach (DataRow dr in dsReplyID.Tables[0].Rows)
                {
                    memo.Value = dr["Content"].ToString().ToReverseXSS();
                }
            }
        }

        public string Today { get; set; }
    }
}