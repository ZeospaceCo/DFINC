using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Account.Web.MyInfo
{
    public partial class Mobile : Base.BasePageAccount
    {
        #region 변수설정
        public string ActiveMobile = String.Empty;
        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.Mobile).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-07
        /// </summary>
        private void LoadData()
        {
            Zeospace.DeskPlus.MDM.DAL.List dal = new MDM.DAL.List();
            DataTableReader dr = dal.GetListUUIDAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            ddlUUID.Items.Clear();

            if (dr != null)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["UUID"].ToString() != "0")
                        {
                            ddlUUID.Items.Add(new ListItem(dr["UUID"].ToString(), dr["ListID"].ToString()));
                        }
                    }
                }
            }

            if (ddlUUID.Items.Count > 0)
            {
                int listID = ddlUUID.Items[0].Value.ToNumber<int>();

                DataTableReader dtr = dal.GetListInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, listID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            string deptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(),
                                                                   dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(),
                                                                   dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);

                            string titleName = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                                                                dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                                                                dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);

                            string name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                            DateTime dt;

                            string requestDateStr = "";

                            if (DateTime.TryParse(dtr["RequestDate"].ToString(), out dt))
                                requestDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                requestDateStr = string.Empty;

                            string approveDateStr = "";

                            if (DateTime.TryParse(dtr["ApproveDate"].ToString(), out dt))
                                approveDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                approveDateStr = string.Empty;


                            CTBMobilePhone.Text = dtr["MobileNumber"].ToString();
                            CTBMobileType.InnerText = dtr["MobileType"].ToString();
                            CTBDeviceName.Text = dtr["DeviceName"].ToString();
                            CTBRequestDate.InnerText = requestDateStr;                            

                            ActiveMobile = dtr["ActiveMobile"].ToString();
                            if (ActiveMobile == "0")
                            {
                                CTBActiveMobile.InnerText = resMng.GetString("ActiveMobile0");
                            }
                            else if (ActiveMobile == "1")
                            {
                                CTBActiveMobile.InnerText = resMng.GetString("ActiveMobile1");

                                CTBApproveDate.InnerText = approveDateStr;
                                ddlLoginType.SelectedValue = dtr["LoginType"].ToString();
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
                            else if (ActiveMobile == "2")
                            {
                                CTBActiveMobile.InnerText = resMng.GetString("ActiveMobile2");
                            }
                            else 
                            {
                                CTBActiveMobile.InnerText = resMng.GetString("ActiveMobile3");
                            }

                            ctbDesc.Text = dtr["Memo"].ToString();                            
                        }
                    }
                }
            }

            Zeospace.DeskPlus.MDM.DAL.UserEnv dalUserEnv = new MDM.DAL.UserEnv();
            DataTableReader drUserEnv = dalUserEnv.GetUserEnvInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (drUserEnv != null)
            {
                if (drUserEnv.HasRows)
                {
                    while (drUserEnv.Read())
                    {
                        if (drUserEnv["AlarmMail"].ToString() == "N")
                        {
                            ddlMailAlarm.Enabled = false;
                            ddlMailAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }

                        if (drUserEnv["AlarmApproval"].ToString() == "N")
                        {
                            ddlApprovalAlarm.Enabled = false;
                            ddlApprovalAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }

                        if (drUserEnv["AlarmBoard"].ToString() == "N")
                        {
                            ddlBoardAlarm.Enabled = false;
                            ddlBoardAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }

                        if (drUserEnv["AlarmPims"].ToString() == "N")
                        {
                            ddlPimsAlarm.Enabled = false;
                            ddlPimsAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }

                        if (drUserEnv["AlarmTask"].ToString() == "N")
                        {
                            ddlTaskAlarm.Enabled = false;
                            ddlTaskAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }

                        if (drUserEnv["AlarmWork"].ToString() == "N")
                        {
                            ddlWorkAlarm.Enabled = false;
                            ddlWorkAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }

                        if (drUserEnv["AlarmCommunity"].ToString() == "N")
                        {
                            ddlCommunityAlarm.Enabled = false;
                            ddlCommunityAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }

                        if (drUserEnv["AlarmRM"].ToString() == "N")
                        {
                            ddlResourceAlarm.Enabled = false;
                            ddlResourceAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }

                        if (drUserEnv["AlarmSNS"].ToString() == "N")
                        {
                            ddlSNSAlarm.Enabled = false;
                            ddlSNSAlarm.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");                       // "기본정보";              
                cbtnSave.Text = resMng.GetString("Modify");                                        //"저장";
                cbtnDel.Text = resMng.GetString("Admin_CompanyManager_btnDel");                    // "삭제";
                cbtnList.Text = resMng.GetString("BtnClose");

                ddlLoginType.Items.Clear();
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType1"), "0"));
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType2"), "1"));
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType3"), "2"));

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

            }
        }

        #endregion Method (함수)

        #region Event Handler (이벤트 핸들러)                
        #endregion Event Handler (이벤트 핸들러)

        #region Property (프로퍼티)
        #endregion Property (프로퍼티)
    }
}