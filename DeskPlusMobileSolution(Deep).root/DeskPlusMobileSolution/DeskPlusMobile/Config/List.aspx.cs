using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Config
{
    public partial class List : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Config.List).Assembly;	//어셈블리 선언
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

                LoadUserEnv();
                LoadEnv();
                LoadAuth();
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                ddlLoginType.Items.Clear();
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType1"), "0"));
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType2"), "1"));
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType3"), "2"));

                ddlMailList.Items.Clear();
                ddlMailList.Items.Add(new ListItem("OFF", "N"));
                ddlMailList.Items.Add(new ListItem("ON", "Y"));

                ddlMailAlarm.Items.Clear();
                ddlMailAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlMailAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlApprovalList.Items.Clear();
                ddlApprovalList.Items.Add(new ListItem("OFF", "N"));
                ddlApprovalList.Items.Add(new ListItem("ON", "Y"));

                ddlApprovalAlarm.Items.Clear();
                ddlApprovalAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlApprovalAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlBoardList.Items.Clear();
                ddlBoardList.Items.Add(new ListItem("OFF", "N"));
                ddlBoardList.Items.Add(new ListItem("ON", "Y"));

                ddlBoardAlarm.Items.Clear();
                ddlBoardAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlBoardAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlPimsList.Items.Clear();
                ddlPimsList.Items.Add(new ListItem("OFF", "N"));
                ddlPimsList.Items.Add(new ListItem("ON", "Y"));

                ddlPimsAlarm.Items.Clear();
                ddlPimsAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlPimsAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlTaskList.Items.Clear();
                ddlTaskList.Items.Add(new ListItem("OFF", "N"));
                ddlTaskList.Items.Add(new ListItem("ON", "Y"));

                ddlTaskAlarm.Items.Clear();
                ddlTaskAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlTaskAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlWorkList.Items.Clear();
                ddlWorkList.Items.Add(new ListItem("OFF", "N"));
                ddlWorkList.Items.Add(new ListItem("ON", "Y"));

                ddlWorkAlarm.Items.Clear();
                ddlWorkAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlWorkAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlCommunityList.Items.Clear();
                ddlCommunityList.Items.Add(new ListItem("OFF", "N"));
                ddlCommunityList.Items.Add(new ListItem("ON", "Y"));

                ddlCommunityAlarm.Items.Clear();
                ddlCommunityAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlCommunityAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlResourceList.Items.Clear();
                ddlResourceList.Items.Add(new ListItem("OFF", "N"));
                ddlResourceList.Items.Add(new ListItem("ON", "Y"));

                ddlResourceAlarm.Items.Clear();
                ddlResourceAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlResourceAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlSNSList.Items.Clear();
                ddlSNSList.Items.Add(new ListItem("OFF", "N"));
                ddlSNSList.Items.Add(new ListItem("ON", "Y"));

                ddlSNSAlarm.Items.Clear();
                ddlSNSAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlSNSAlarm.Items.Add(new ListItem("Push-ON", "Y"));

                ddlNoticeAlarm.Items.Clear();
                ddlNoticeAlarm.Items.Add(new ListItem("Push-OFF", "N"));
                ddlNoticeAlarm.Items.Add(new ListItem("Push-ON", "Y"));
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-10-21
        /// 작성목적 : 알림함 설정 정보 가져오기
        /// </summary>
        public void LoadUserEnv()
        {
            try
            {
                ddlMailList.SelectedValue = "Y";
                ddlApprovalList.SelectedValue = "Y";
                ddlBoardList.SelectedValue = "Y";
                ddlPimsList.SelectedValue = "Y";
                ddlTaskList.SelectedValue = "Y";
                ddlWorkList.SelectedValue = "Y";
                ddlCommunityList.SelectedValue = "Y";
                ddlResourceList.SelectedValue = "Y";
                ddlSNSList.SelectedValue = "Y";

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_MDM_UserEnv");
                sql = string.Format(sql, CompanyID.ToString(), UserID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsUserEnv", null);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ddlMailList.SelectedValue = dr["AlarmMail"].ToString();
                            ddlApprovalList.SelectedValue = dr["AlarmApproval"].ToString();
                            ddlBoardList.SelectedValue = dr["AlarmBoard"].ToString();
                            ddlPimsList.SelectedValue = dr["AlarmPims"].ToString();
                            ddlTaskList.SelectedValue = dr["AlarmTask"].ToString();
                            ddlWorkList.SelectedValue = dr["AlarmWork"].ToString();
                            ddlCommunityList.SelectedValue = dr["AlarmCommunity"].ToString();
                            ddlResourceList.SelectedValue = dr["AlarmRM"].ToString();
                            ddlSNSList.SelectedValue = dr["AlarmSNS"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoadUserEnv()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-02-08
        /// 작성목적 : 환경 정보 가져오기
        /// </summary>
        public void LoadEnv()
        {
            try
            {
                if (!String.IsNullOrEmpty(UUID))
                {
                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_MDM_List");
                    sql = string.Format(sql, CompanyID.ToString(), UserID, UUID);
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsEnvInfo", null);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ddlLoginType.SelectedValue = dr["LoginType"].ToString();
                        ddlMailAlarm.SelectedValue = dr["MailAlarm"].ToString();

                        if (dr["EtcAlarm"].ToString().IndexOf("Approval") > -1)
                        {
                            ddlApprovalAlarm.SelectedValue = "Y";
                        }

                        if (dr["EtcAlarm"].ToString().IndexOf("Board") > -1)
                        {
                            ddlBoardAlarm.SelectedValue = "Y";
                        }

                        if (dr["EtcAlarm"].ToString().IndexOf("Pim") > -1)
                        {
                            ddlPimsAlarm.SelectedValue = "Y";
                        }

                        if (dr["EtcAlarm"].ToString().IndexOf("TASK") > -1)
                        {
                            ddlTaskAlarm.SelectedValue = "Y";
                        }

                        if (dr["EtcAlarm"].ToString().IndexOf("Report") > -1)
                        {
                            ddlWorkAlarm.SelectedValue = "Y";
                        }

                        if (dr["EtcAlarm"].ToString().IndexOf("Community") > -1)
                        {
                            ddlCommunityAlarm.SelectedValue = "Y";
                        }

                        if (dr["EtcAlarm"].ToString().IndexOf("ResourceManager") > -1)
                        {
                            ddlResourceAlarm.SelectedValue = "Y";
                        }

                        if (dr["EtcAlarm"].ToString().IndexOf("SNS") > -1)
                        {
                            ddlSNSAlarm.SelectedValue = "Y";
                        }

                        if (dr["EtcAlarm"].ToString().IndexOf("Notice") > -1)
                        {
                            ddlNoticeAlarm.SelectedValue = "Y";
                        }

                        if (dr["AlarmTimeUse"].ToString() == "1")
                        {
                            cbAlarmTimeUse.Checked = true;

                            var alarmStart = dr["AlarmStart"].ToString().Split(':');
                            var alarmEnd = dr["AlarmEnd"].ToString().Split(':');
                            if (alarmStart.Length > 0 && alarmEnd.Length > 0)
                            {
                                alarmStartHour.SelectedValue = alarmStart[0];
                                alarmStartMin.SelectedValue = alarmStart[1];
                                alarmEndHour.SelectedValue = alarmEnd[0];
                                alarmEndMin.SelectedValue = alarmStart[1];
                            }
                        }
                    }

                    if (ddlMailList.SelectedValue == "N")
                    {
                        ddlMailAlarm.SelectedValue = "N";
                        ddlMailAlarm.Enabled = false;
                    }

                    if (ddlApprovalList.SelectedValue == "N")
                    {
                        ddlApprovalAlarm.SelectedValue = "N";
                        ddlApprovalAlarm.Enabled = false;
                    }

                    if (ddlBoardList.SelectedValue == "N")
                    {
                        ddlBoardAlarm.SelectedValue = "N";
                        ddlBoardAlarm.Enabled = false;
                    }

                    if (ddlPimsList.SelectedValue == "N")
                    {
                        ddlPimsAlarm.SelectedValue = "N";
                        ddlPimsAlarm.Enabled = false;
                    }

                    if (ddlTaskList.SelectedValue == "N")
                    {
                        ddlTaskAlarm.SelectedValue = "N";
                        ddlTaskAlarm.Enabled = false;
                    }

                    if (ddlWorkList.SelectedValue == "N")
                    {
                        ddlWorkAlarm.SelectedValue = "N";
                        ddlWorkAlarm.Enabled = false;
                    }

                    if (ddlCommunityList.SelectedValue == "N")
                    {
                        ddlCommunityAlarm.SelectedValue = "N";
                        ddlCommunityAlarm.Enabled = false;
                    }

                    if (ddlResourceList.SelectedValue == "N")
                    {
                        ddlResourceAlarm.SelectedValue = "N";
                        ddlResourceAlarm.Enabled = false;
                    }

                    if (ddlSNSList.SelectedValue == "N")
                    {
                        ddlSNSAlarm.SelectedValue = "N";
                        ddlSNSAlarm.Enabled = false;
                    }
                }
                else
                {
                    ddlMailAlarm.Enabled = false;
                    ddlApprovalAlarm.Enabled = false;
                    ddlBoardAlarm.Enabled = false;
                    ddlPimsAlarm.Enabled = false;
                    ddlTaskAlarm.Enabled = false;
                    ddlWorkAlarm.Enabled = false;
                    ddlCommunityAlarm.Enabled = false;
                    ddlResourceAlarm.Enabled = false;
                    ddlSNSAlarm.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoadEnv()", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadAuth()
        {
            sMail.Visible = false;
            ddlMailAlarm.Visible = false;
            ddlMailList.Visible = false;

            divApprove.Visible = false;
            sApproval.Visible = false;
            ddlApprovalAlarm.Visible = false;
            ddlApprovalList.Visible = false;

            sBoard.Visible = false;
            ddlBoardAlarm.Visible = false;
            ddlBoardList.Visible = false;

            divPims.Visible = false;
            sPim.Visible = false;
            ddlPimsAlarm.Visible = false;
            ddlPimsList.Visible = false;

            sTask.Visible = false;
            ddlTaskAlarm.Visible = false;
            ddlTaskList.Visible = false;

            sCommunity.Visible = false;
            ddlCommunityAlarm.Visible = false;
            ddlCommunityList.Visible = false;

            sResource.Visible = false;
            ddlResourceAlarm.Visible = false;
            ddlResourceList.Visible = false;

            sSNS.Visible = false;
            ddlSNSAlarm.Visible = false;
            ddlSNSList.Visible = false;

            if (UserInfo.MenuRoles != null)
            {
                for (int i = 0; i < UserInfo.MenuRoles.Length; i++)
                {
                    if (UserInfo.MenuRoles[i] == "1")
                    {
                        sMail.Visible = true;
                        ddlMailAlarm.Visible = true;
                        ddlMailList.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "2")
                    {
                        divApprove.Visible = true;
                        sApproval.Visible = true;
                        ddlApprovalAlarm.Visible = true;
                        ddlApprovalList.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "3")
                    {
                        sBoard.Visible = true;
                        ddlBoardAlarm.Visible = true;
                        ddlBoardList.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "4")
                    {
                        pimsCheck = "Y";
                        divPims.Visible = true;
                        sPim.Visible = true;
                        ddlPimsAlarm.Visible = true;
                        ddlPimsList.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "7")
                    {
                        sCommunity.Visible = true;
                        ddlCommunityAlarm.Visible = true;
                        ddlCommunityList.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "8")
                    {
                        sSNS.Visible = true;
                        ddlSNSAlarm.Visible = true;
                        ddlSNSList.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "10")
                    {
                        sTask.Visible = true;
                        ddlTaskAlarm.Visible = true;
                        ddlTaskList.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "11")
                    {
                        sResource.Visible = true;
                        ddlResourceAlarm.Visible = true;
                        ddlResourceList.Visible = true;
                    }
                }
            }
        }

        public string Today { get; set; }
        public string pimsCheck { get; set; }
    }
}