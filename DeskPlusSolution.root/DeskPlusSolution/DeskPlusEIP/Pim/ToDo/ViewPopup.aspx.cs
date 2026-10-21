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

namespace Zeospace.DeskPlus.Pim.Web.ToDo
{
    public partial class ViewPopup : Base.BasePagePim
    {

        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        public int TodoID = 0;
        public string bPage = string.Empty;



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


        public string AttachLimitSize = string.Empty; //첨부파일 제한용량
        public string Contents = string.Empty; //내용
        #endregion

        #region Page Process (페이지 처리)


        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.ToDo.ViewPopup).Assembly;	//어셈블리 선언

            #endregion

            if (!IsPostBack)
            {
                #region 초기화

                SetLanguage();
                SettingControl();
                #endregion


                #region  LOADDATA

                LoadData();

                #endregion


            }
        }

        #endregion

        #region Method (함수)


        private void SettingControl()
        {
            #region  정리1
            ListItem lv = new ListItem();
            lv.Value = "W";
            lv.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_NotBegin.gif'/>" + GetResource("priority1");
            ListItem lv1 = new ListItem();
            lv1.Value = "I";
            lv1.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Going.gif'/>" + GetResource("priority2");
            ListItem lv2 = new ListItem();
            lv2.Value = "E";
            lv2.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Done.gif'/>" + GetResource("priority3");
            ListItem lv3 = new ListItem();
            lv3.Value = "C";
            lv3.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Cancel.gif'/>" + GetResource("priority4");
            ListItem lv4 = new ListItem();
            lv4.Value = "T";
            lv4.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Pass.gif'/>" + GetResource("priority5");
            ListItem lv5 = new ListItem();
            lv5.Value = "D";
            lv5.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Postpone.gif'/>" + GetResource("priority6");

            rbtnlStatus.Items.Add(lv);
            rbtnlStatus.Items.Add(lv1);
            rbtnlStatus.Items.Add(lv2);
            rbtnlStatus.Items.Add(lv3);
            rbtnlStatus.Items.Add(lv4);
            rbtnlStatus.Items.Add(lv5);


            #endregion
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnPrint.Text = GetResource("PRINT");
                cbtnClose.Text = GetResource("Close");
                cbtnShortReply.Text = GetResource("View_AddShortReply");
            }
        }


        private void LoadParam()
        {


            TodoID = GetDecryptParam("TodoID", "0").ToNumber<int>();
            bPage = GetDecryptParam("Page", "");
            //Date = GetDecryptParam("Date", "");
            PageUrl = GetDecryptParam("Page", "");
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

        private void LoadData()
        {
            LoadParam();
            if (TodoID == 0)
                return;

            DAL.ToDo dal = new DAL.ToDo();
            DataTableReader dtr = dal.GetTodoID(CompanyID, TodoID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //진척도
                        rbtnlStatus.SelectedValue = dtr["Status"].ToString();


                        //중요도
                        lblPriority.Text = dtr["Priority"].ToString();
                        switch (dtr["Priority"].ToString())
                        {
                            case "1":
                                lblPriority.Text = GetResource("TOP");
                                break;
                            case "2":
                                lblPriority.Text = GetResource("MIDDLE");
                                break;
                            case "3":
                                lblPriority.Text = GetResource("BOTTOM");
                                break;
                        }

                        //공유여부
                        //lblshare



                        switch (dtr["opentype"].ToString())
                        {
                            case "0":
                                lblshare.Text = GetResource("TodoShare1").ToString();
                                break;
                            case "3":
                                lblshare.Text = GetResource("TodoShare2").ToString();
                                break;
                            case "1":
                                lblshare.Text = GetResource("TodoShare3").ToString();
                                break;
                            case "2":
                                lblshare.Text = GetResource("TodoShare4").ToString();
                                break;
                        }

                        //제목
                        lbltitle.Text = dtr["Title"].ToString();//.ToReverseXSS();
                        //내용
                        lblContent.Text = dtr["Content"].ToString();

                        //시작
                        string strTime = string.Empty;


                        //strTime = DateTime.Parse(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd");
                        strTime = DateTime.Parse(dtr["StartDate"].ToString()).ToShortDateString();

                        //strTime +="~ "+ DateTime.Parse(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                        lbltime.Text = strTime;

                        //일정에반영

                        if (dtr["ScheduleYN"].ToString().Equals("Y"))
                        {
                            lblSchedule.Text = GetResource("Ebem");

                        }
                        else
                        {
                            lblSchedule.Text = GetResource("UnPimEbem");

                        }

                    }
                }
            }
        }



        #endregion      

        #region Property (프로퍼티)
        #endregion
    }
}