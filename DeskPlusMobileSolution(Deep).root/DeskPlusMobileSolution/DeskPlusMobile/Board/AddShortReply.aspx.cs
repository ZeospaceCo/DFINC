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
    public partial class AddShortReply : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
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
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Board.AddShortReply).Assembly;	    //어셈블리 선언
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
                StringBuilder sb = new StringBuilder();

                foreach (DataRow dr in dsReplyID.Tables[0].Rows)
                {
                    string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                                   dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                    string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                    dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                    string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                    dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                    string userInfo = deptName + " " + titleName + " " + name;

                    sb.Append("<div>");
                    sb.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td>");

                    // header
                    sb.Append("<div style=\"height: 20px; margin: 0 0 5px 0\">");
                    sb.Append("<span>" + userInfo + "</span>");
                    sb.Append("</div>");
                    // content
                    sb.Append("<div style=\"width: 100%\">");
                    sb.Append(@"<table cellpadding='0' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append(dr["Content"].ToString().Replace("\n", "<br>"));
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    // footer 1
                    sb.Append("<div style=\"margin-top: 5px;\">");
                    sb.Append("<span>");
                    sb.Append(dr["InputDate"].ToString());
                    sb.Append("</span>");
                    sb.Append("</div>");

                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");

                    // 입력부분
                    sb.Append("<div>");
                    sb.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style=\"vertical-align: top; width:25px;\">");
                    sb.Append("<img src=\"/DeskPlusMobile/Design/C/Img/Board/review.png\" style=\"vertical-align: middle;\" />");
                    sb.Append("</td>");

                    sb.Append("<td>");

                    // header
                    string tmpDeptName = "";
                    string tmpTitleName = "";
                    string tmpUserName = "";
                    List<Zeospace.DeskPlusMobile.Web.Account.BE.UserDeptInfo> ainfo = GetAccountInfo(UserInfo.CompanyID, UserInfo.UserID, UserInfo.DeptID);

                    foreach (Zeospace.DeskPlusMobile.Web.Account.BE.UserDeptInfo info in ainfo)
                    {
                        tmpDeptName = info.DeptName;
                        tmpTitleName = info.TitleName;
                        tmpUserName = info.UserName;
                    }

                    sb.Append("<div style=\"height: 20px; margin: 0 0 5px 0\">");
                    sb.Append("<span>" + tmpDeptName + " " + tmpTitleName + " " + tmpUserName + "</span>");
                    sb.Append("</div>");

                    sb.Append("<textarea id=\"memo\" class='form-control' style='border: 1px solid rgb(210, 210, 210);  margin: 5px auto; width: 100%; height: 10em; color: gray; vertical-align: top;' maxlength='200'></textarea>");

                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                }

                ShortList += sb.ToString();
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 사용자 정보 가져오기
        /// 작성일자 : 2014-01-07
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public List<Zeospace.DeskPlusMobile.Web.Account.BE.UserDeptInfo> GetAccountInfo(int _companyID, int _userID, int _deptID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ACCOUNT");
                sql = string.Format(sql, _companyID, _userID, _deptID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                List<Zeospace.DeskPlusMobile.Web.Account.BE.UserDeptInfo> list = new List<Zeospace.DeskPlusMobile.Web.Account.BE.UserDeptInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Zeospace.DeskPlusMobile.Web.Account.BE.UserDeptInfo info = new Zeospace.DeskPlusMobile.Web.Account.BE.UserDeptInfo();
                    info.DeptName = this.GetLangName(
                                dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                dr["DeptZhtw"].ToString().ToNull(), PreLang);

                    info.TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);

                    info.UserName = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAccountInfo()", ex);
                return null;
            }
        }

        public string Today { get; set; }

        public string ShortList { get; set; }
    }
}