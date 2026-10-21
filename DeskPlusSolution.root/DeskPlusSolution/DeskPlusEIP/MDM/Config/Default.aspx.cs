using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Zeospace.DeskPlus.MDM.Web.Config
{
    public partial class Default : Base.BasePageMDMAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Config.Default).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-16
        /// 작성목적 : 환경설정 초기값 설정
        /// </summary>
        private void LoadData()
        {
            DAL.Env dal = new DAL.Env();
            DataTableReader dtr = dal.GetEnvInfo(CurrentUserInfo.CompanyID);

            List<object> listobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ddlLoginType.SelectedValue = dtr["LoginType"].ToString();
                        rblAuth.SelectedValue = dtr["AccessRight"].ToString();
                        ddlAppApprove.SelectedValue = dtr["Approve"].ToString();

                        ddlMailAlarm.SelectedValue = dtr["MailAlarm"].ToString();

                        if (dtr["EtcAlarm"].ToString().IndexOf("Approval") > -1)
                        {
                            ddlApprovalAlarm.SelectedValue = "Y";
                        }

                        if (dtr["EtcAlarm"].ToString().IndexOf("Board") > -1)
                        {
                            ddlBoardAlarm.SelectedValue = "Y";
                        }

                        if (dtr["EtcAlarm"].ToString().IndexOf("Pim") > -1)
                        {
                            ddlPimsAlarm.SelectedValue = "Y";
                        }

                        if (dtr["EtcAlarm"].ToString().IndexOf("TASK") > -1)
                        {
                            ddlTaskAlarm.SelectedValue = "Y";
                        }

                        if (dtr["EtcAlarm"].ToString().IndexOf("Report") > -1)
                        {
                            ddlWorkAlarm.SelectedValue = "Y";
                        }

                        if (dtr["EtcAlarm"].ToString().IndexOf("Community") > -1)
                        {
                            ddlCommunityAlarm.SelectedValue = "Y";
                        }

                        if (dtr["EtcAlarm"].ToString().IndexOf("ResourceManager") > -1)
                        {
                            ddlResourceAlarm.SelectedValue = "Y";
                        }

                        if (dtr["EtcAlarm"].ToString().IndexOf("SNS") > -1)
                        {
                            ddlSNSAlarm.SelectedValue = "Y";
                        }

                        if (dtr["EtcAlarm"].ToString().IndexOf("Notice") > -1)
                        {
                            ddlNoticeAlarm.SelectedValue = "Y";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : 2013-01-15
        /// 작성일자 : famo23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");       //"확인";

                ddlLoginType.Items.Clear();
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType1"), "0"));
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType2"), "1"));
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType3"), "2"));

                rblAuth.Items.Clear();
                rblAuth.Items.Add(new ListItem(this.resMng.GetString("AccessRight1"), "0"));
                rblAuth.Items.Add(new ListItem(this.resMng.GetString("AccessRight2"), "1"));

                rblAuth.Items[0].Selected = true;

                ddlMailAlarm.Items.Clear();
                ddlMailAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlMailAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlApprovalAlarm.Items.Clear();
                ddlApprovalAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlApprovalAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlBoardAlarm.Items.Clear();
                ddlBoardAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlBoardAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlPimsAlarm.Items.Clear();
                ddlPimsAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlPimsAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlTaskAlarm.Items.Clear();
                ddlTaskAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlTaskAlarm.Items.Add(new ListItem("ON", "Y"));            

                ddlWorkAlarm.Items.Clear();
                ddlWorkAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlWorkAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlCommunityAlarm.Items.Clear();
                ddlCommunityAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlCommunityAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlResourceAlarm.Items.Clear();
                ddlResourceAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlResourceAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlSNSAlarm.Items.Clear();
                ddlSNSAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlSNSAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlNoticeAlarm.Items.Clear();
                ddlNoticeAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlNoticeAlarm.Items.Add(new ListItem("ON", "Y"));

                ddlAppApprove.Items.Clear();
                ddlAppApprove.Items.Add(new ListItem(this.resMng.GetString("AppApprove1"), "0"));
                ddlAppApprove.Items.Add(new ListItem(this.resMng.GetString("AppApprove2"), "1"));
            }
        }

        #endregion

        #region (함수)
        #endregion

        #region (속성)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}