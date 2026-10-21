using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.MDM.Web.Device
{
    public partial class DeviceInfo : Base.BasePageMDMAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int DeptID = 0;
        public int UserID = 0;
        public int ListID = 0;
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort 
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string ListPage = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Device.DeviceInfo).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2013-01-18
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.List dal = new DAL.List();
            DataTableReader dr = dal.GetListUUID(CurrentUserInfo.CompanyID, UserID);            

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
                if (ListID != 0)
                {
                    ddlUUID.SelectedValue = ListID.ToString();

                }
                else
                {
                    ListID = ddlUUID.Items[0].Value.ToNumber<int>();
                }

                DataTableReader dtr = dal.GetListInfo(CurrentUserInfo.CompanyID, UserID, DeptID, ListID);

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


                            ltDeptName.Text = deptName;
                            ltTitle.Text = titleName;
                            ltName.Text = name;
                            rblAccessRight.SelectedValue = dtr["AccessRight"].ToString();
                            ltOfficePhone.Text = dtr["OfficePhone"].ToString();
                            CTBMobilePhone.Text = dtr["MobileNumber"].ToString();
                            CTBMobileType.InnerText = dtr["MobileType"].ToString();
                            CTBDeviceName.Text = dtr["DeviceName"].ToString();
                            CTBRequestDate.InnerText = requestDateStr;
                            CTBApproveDate.InnerText = approveDateStr;
                            ddlLoginType.SelectedValue = dtr["LoginType"].ToString();
                            rblActiveMobile.SelectedValue = dtr["ActiveMobile"].ToString();
                            ctbDesc.Text = dtr["Memo"].ToString();
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
            else
            {
                DAL.Env dalEnv = new DAL.Env();
                DataTableReader dtr = dalEnv.GetUserInfo(CurrentUserInfo.CompanyID, UserID, DeptID);

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

                            ltDeptName.Text = deptName;
                            ltTitle.Text = titleName;
                            ltName.Text = name;
                            rblAccessRight.SelectedValue = dtr["AccessRight"].ToString();
                        }
                    }
                }
            }

            DAL.UserEnv dalUserEnv = new DAL.UserEnv();
            DataTableReader drUserEnv = dalUserEnv.GetUserEnvInfo(CurrentUserInfo.CompanyID, UserID);

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
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {                
                cbtnDel.Text = resMng.GetString("Del");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnCancel.Text = this.resMng.GetString("Cancel");

                rblAccessRight.Items.Clear();
                rblAccessRight.Items.Add(new ListItem(this.resMng.GetString("AccessRight1"), "0"));
                rblAccessRight.Items.Add(new ListItem(this.resMng.GetString("AccessRight2"), "1"));
                rblAccessRight.Items.Add(new ListItem(this.resMng.GetString("AccessRight3"), "2"));   

                ddlLoginType.Items.Clear();
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType1"), "0"));
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType2"), "1"));
                ddlLoginType.Items.Add(new ListItem(this.resMng.GetString("LoginType3"), "2"));

                rblActiveMobile.Items.Clear();
                rblActiveMobile.Items.Add(new ListItem(this.resMng.GetString("ActiveMobile1"), "1"));
                rblActiveMobile.Items.Add(new ListItem(this.resMng.GetString("ActiveMobile2"), "2"));

                ddlMailAlarm.Items.Clear();
                ddlMailAlarm.Items.Add(new ListItem("OFF", "N"));
                ddlMailAlarm.Items.Add(new ListItem("ON" , "Y"));                

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();            
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            ListPage = GetDecryptParam("listPage", "");
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();          
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        private void ModConfig()
        {

        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            ModConfig();
        }

        #endregion
    }
}