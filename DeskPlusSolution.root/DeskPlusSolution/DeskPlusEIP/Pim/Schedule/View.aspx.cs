using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Pim.BE;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class View : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        public int ScheduleID = 0;
        public int DisplayID = 0;
        public int CurrentPage = 0;        
        public string PageUrl = string.Empty;
        
        private string eventArgs = String.Empty;

        //검색어
        public string s_Distance = string.Empty;
        public string s_inquryPerson = string.Empty;
        public string s_DatePeriod = string.Empty;
        public string s_SearchKind = string.Empty;
        public string s_SerachValue = string.Empty;
        public string s_StartDate = string.Empty;
        public string s_EndDate = string.Empty;
        public string s_UserID = string.Empty;

        public string s_PageSize = string.Empty;
        public string s_CurrentPage = string.Empty;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;
            CbtnEdit.Visible=false;
            cbtnDel.Visible = false;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.View).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion


        #region Method (함수)

        private void GetRepeat(int schedulid, int companyid)
        {
            DAL.RepeatSchedule biz = new DAL.RepeatSchedule();
            List<BE.RepeatScheduleInfo> data = biz.GetRepeatSchedule(companyid, schedulid);
            BE.RepeatScheduleInfo info = new BE.RepeatScheduleInfo();

            foreach (BE.RepeatScheduleInfo _be in data)
            {
                hdRepeatChk.Text = "I";
            }
        }


        /// <summary>
        /// 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            CbtnEdit.Text = GetResource("Edit");
            cbtnDel.Text = GetResource("Del");
            CbtnPrint.Text = GetResource("PRINT");
            CbtnCancel.Text = GetResource("Catalog");
            cbtnShortReply.Text = GetResource("View_AddShortReply");
        }

        private void LoadData()
        {
            LoadParam();

            if (DisplayID == 0)
                return;
             
            #region 권한체크
            #endregion

            #region 내용가져오기

            BE.ScheduleInfo info = new BE.ScheduleInfo();
            info.CompanyID = this.CompanyID;
            info.ScheduleID = this.ScheduleID;
            info.DisplayID = this.DisplayID;
            DAL.Schedule dal = new DAL.Schedule();

            DataTableReader dtr = dal.GetSheduleDataViewID(info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (dtr["UserID"].ToString().ToNumber<int>() == CurrentUserInfo.UserID)
                        {
                            CbtnEdit.Visible = true;
                            cbtnDel.Visible = true;
                        }
                        //제목
                        lblTitle.Text = dtr["Title"].ToString();//.ToReverseXSS();
                        ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        
                        //기념일
                        lblDateStr.Text = DateTime.Parse(dtr["StartDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["StartDate"].ToString()).ToShortTimeString() + " ~ " +
                             DateTime.Parse(dtr["EndDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["EndDate"].ToString()).ToShortTimeString();

                        //내용
                        lblContent.Text = dtr["Content"].ToString();
                        
                        //작성자
                        lblWriter.Text = GetLangName(dtr["NameBase"].ToString(),dtr["NameKo"].ToString(),dtr["NameEn"].ToString()
                            ,dtr["NameJa"].ToString(),dtr["NameFr"].ToString(),dtr["NameZhcn"].ToString(),dtr["NameZhtw"].ToString(),PreLang);
                         
                        //게시범위
                        switch (dtr["OpenType"].ToString())
                        {
                            case "-1":
                                lblshare.Text = GetResource("pim1");
                                break;
                            case "0":
                                lblshare.Text = GetResource("pim1");
                                break;
                            case "3":
                                lblshare.Text = GetResource("pim2");
                                break;
                            case "1":
                                lblshare.Text = GetResource("pim3");
                                break;
                            case "2":
                                lblshare.Text = GetResource("pim4");
                                break;

                        }
                        GetShareUser(ScheduleID, CompanyID, dtr["UserID"].ToString().ToNumber<int>());

                        GetRepeat(ScheduleID, CompanyID);

                        GetStatus(dtr["StatusID"].ToString().ToNumber<int>());
                    }
                }
            }


            #endregion


        }


        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="_statusID">The _status ID.</param>
        private void GetStatus(int _statusID)
        {
            DAL.Status dal = new DAL.Status();
            List<StatusInfo> alList = dal.GetList(CurrentUserInfo.CompanyID, _statusID);

            foreach (BE.StatusInfo info in alList)
            {
                this.lblStatus.Text = this.GetLangName(
                                    info.NameBase.ToNull(), info.NameKo.ToNull(),
                                    info.NameEn.ToNull(), info.NameJa.ToNull(),
                                    info.NameFr.ToNull(), info.NameZhcn.ToNull(),
                                    info.NameZhtw.ToNull(), PreLang);

                lblStatus.Attributes.Add("style", "color:" + info.Colors);
            }
        }


        private void GetShareUser(int schedulid, int companyid, int userid)
        {
            DAL.ScheShareList biz = new DAL.ScheShareList();            
            DataTableReader dtr = biz.GetScheShareListTable(companyid, schedulid, userid);

            string rtHtml = "<ul id='divShare' class='table_typeT ul_typeT'>";

            bool bchk = false;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        bchk = true;
                        string name = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        string title = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                            dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang
                            );

                        if (title.Length > 0)
                        {
                            title = "(" + title + ")";
                        }

                        rtHtml += "<li>";
                        rtHtml += "<span>" + name + "" + title + "</span>";                        
                        rtHtml += "</li>";
                    }
                }
            }

            if (!bchk) rtHtml += "<li></li>";

            rtHtml += "</ul>";
            lblshareuser.Text = rtHtml;

            /*
            string rtHtml = "<table cellspacing='0' class='table_typeT'>";
            rtHtml += "<caption></caption>";
            rtHtml += "<colgroup><col  /></colgroup>";
            rtHtml += "<tr><th>" + GetResource("Name") + "</th></tr>";
            bool bchk = false;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        bchk = true;
                        string name = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        
                        string title = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                            dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang
                            );

                        if (title.Length > 0)
                        {
                            title = "("+title+")";
                        }

                        rtHtml += "<tr><td class='center'>" + name +""+title +"</td></tr>";                
                    }
                }
            }

            if(!bchk)
                rtHtml += "<tr><td class='center'>" + GetResource("TextEmpty") + "</td></tr>";
            

            rtHtml += "</table>";
            lblshareuser.Text = rtHtml;
            */

        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            PageUrl = GetDecryptParam("Page", "");
            Date = GetDecryptParam("Date", "");
            DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
            

            s_Distance = GetDecryptParam("s_Distance", "");
            s_inquryPerson = GetDecryptParam("s_inquryPerson", "");
            s_DatePeriod = GetDecryptParam("s_DatePeriod", "");
            s_SearchKind = GetDecryptParam("s_SearchKind", "");
            s_SerachValue = GetDecryptParam("s_SerachValue", "");
            s_StartDate = GetDecryptParam("s_StartDate", "");
            s_EndDate = GetDecryptParam("s_EndDate", "");
            s_UserID = GetDecryptParam("s_UserID", "");

            s_PageSize = GetDecryptParam("s_PageSize", "");
            s_CurrentPage = GetDecryptParam("s_CurrentPage", "");


            if (PageUrl == "AllSchedule.aspx")//전체공유일정일경우
            {
                s_StartDate = GetDecryptParam("StartDate", "");
                s_EndDate = GetDecryptParam("EndDate", "");
                s_UserID = GetDecryptParam("UserList", "");
                s_SearchKind = GetDecryptParam("title", "");
                s_SerachValue = GetDecryptParam("name", "");
            }
        }


         


        #endregion


        #region Event Handler (이벤트 핸들러)


        protected void lbtnDel_Click(object sender, EventArgs e)
        {
          //  DeleteContent();
        }

        #endregion 

    }
}