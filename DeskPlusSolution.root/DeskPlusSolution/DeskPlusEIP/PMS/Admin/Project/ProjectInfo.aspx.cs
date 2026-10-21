using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Zeospace.DeskPlus.PMS.Web.Admin.Project
{
    public partial class ProjectInfo : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)        
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Admin.Project.ProjectInfo).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();                
            }

            //cbtnOk.Icon = BaseImgPath + "/Pms/Button/but_ok.gif";
            //cbtnFolder.Icon = BaseImgPath + "/Approval/Button/but_folder_man.gif";
            //cbtnDeptUserTree.Icon = BaseImgPath + "/Account/Button/but_search_simple.gif";
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 초기 데이터 가져오기
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-22
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id     
            UserID = CurrentUserInfo.UserID;
            #endregion

            //LoadXmlDataForStatus();
            LoadProject();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Mod");
                cbtnDeptUserTree.Text = resMng.GetString("Search");
                cbtnFolder.Text = resMng.GetString("FolderManage");

                ltlCharger.Text = resMng.GetString("Unit_Register_ltlCharger");
                ltlDelayDay.Text = resMng.GetString("ProjectInfo_DelayDays");
                ltlEndDate.Text = resMng.GetString("ProjectInfo_EDate"); ;
                ltlFolder.Text = resMng.GetString("List_ltlFolder");
                ltlInjecDay.Text = resMng.GetString("ProjectInfo_InDays"); ;
                ltlIsOpen.Text = resMng.GetString("Project_ProjectInfo_IsOpen");
                ltlPlanEndDate.Text = resMng.GetString("ProjectInfo_PlanEDate");
                ltlPlanStartDate.Text = resMng.GetString("ProjectInfo_PlanSDate");
                ltlWholeDay.Text = resMng.GetString("ProjectInfo_TotalDays"); ;
                ltlWriteDate.Text = resMng.GetString("Project_ProjectInfo_Date");
                ltlWriteUserName.Text = resMng.GetString("ProjectList_Header2");
                ltlTitle.Text = resMng.GetString("List_ProjectName");
                ltlStartDate.Text = resMng.GetString("ProjectInfo_SDate");
                ltlProcess.Text = resMng.GetString("ProjectList_Header6");
                ltlStatus.Text = resMng.GetString("Unit_Register_ltlStatus");
                ltlRestDay.Text = resMng.GetString("ProjectInfo_RemainDays");

                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status1"), "1"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status2"), "2"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status3"), "3"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status4"), "4"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status5"), "5"));
            }
        }

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 프로젝트 정보 가져오기
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-22
        /// </summary>
        private void LoadProject()
        {
            DAL.Project bll = new Zeospace.DeskPlus.PMS.DAL.Project();            

            var alList = bll.GetProject(CompanyID, ProjectID);
            foreach (var be in alList)
            {
                if (be != null)
                {
                    if (be.StartDate.ToNullDT() != System.DBNull.Value && be.EndDate.ToNullDT() != System.DBNull.Value)
                    {
                        Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();
                        var al = lib.GetProjcectDays(be.StartDate, be.EndDate, be.CompleteDate);
                        if (al != null)
                        {
                            lblWholeDay.Text = al[0].ToString();
                            lblInjecDay.Text = al[1].ToString();
                            lblRestDay.Text = al[2].ToString();
                            lblDelayDay.Text = al[3].ToString();
                        }
                    }

                    if (be.StartDate.ToNullDT() != System.DBNull.Value) tbxStartDate.Text = be.StartDate.ToShortDateString();
                    if (be.EndDate.ToNullDT() != System.DBNull.Value) tbxEndDate.Text = be.EndDate.ToShortDateString();
                    if (be.PlanStartDate.ToNullDT() != System.DBNull.Value) tbxPlanStartDate.Text = be.PlanStartDate.ToShortDateString();
                    if (be.PlanEndDate.ToNullDT() != System.DBNull.Value) tbxPlanEndDate.Text = be.PlanEndDate.ToShortDateString();

                    tbxContent.Text = be.Contents.ToNull().ToReverseXSS();                    

                    tbxFolder.Text = this.GetLangName(
                                    be.FolderBase,
                                    be.FolderKo, be.FolderEn, be.FolderJa,
                                    be.FolderFr, be.FolderZhcn, be.FolderZhtw, PreLang).ToNull().ToReverseXSS();

                    tbxFolderID.Text = be.FolderID.ToString();

                    lblIsOpen.Text = (be.IsOpen == 1) ? GetResource("Project_ProjectInfo_Approval") : GetResource("Project_ProjectInfo_NoApproval");
                    ddlProcess.SelectedValue = be.Process.ToString().ToNull();
                    
                    tbxTitle.Text = this.GetLangName(
                                    be.SubjectBase,
                                    be.SubjectKo, be.SubjectEn, be.SubjectJa,
                                    be.SubjectFr, be.SubjectZhcn, be.SubjectZhtw, PreLang).ToNull().ToReverseXSS();

                    ddlStatus.SelectedValue = be.Status.ToString();

                    lblWriteDate.Text = Convert.ToDateTime(be.WriteDate).ToShortDateString();
                    
                    lblWriteUserName.Text = this.GetLangName(
                                    be.NameBase,
                                    be.NameKo, be.NameEn, be.NameJa,
                                    be.NameFr, be.NameZhcn, be.NameZhtw, PreLang).ToNull().ToReverseXSS();

                    #region 책임자 정보 가져오기
                    DAL.ProjectCharger charger = new Zeospace.DeskPlus.PMS.DAL.ProjectCharger();
                    List<BE.ProjectChargerInfo> alCharger = charger.GetProjectCharger(CompanyID, ProjectID);

                    if (alCharger != null)
                    {
                        string userIDList = "";
                        string userNameList = "";
                        string gununList = "";

                        foreach (BE.ProjectChargerInfo beCharger in alCharger)
                        {
                            userIDList += beCharger.UserID.ToString() + ",";
                            //deptName += Component.WebComponent.StringHelper.GetNameDBPreLang(PreLang, beCharger.UserName.Trim()) + ",";

                            string userName = this.GetLangName(
                                    beCharger.NameBase,
                                    beCharger.NameKo, beCharger.NameEn, beCharger.NameJa,
                                    beCharger.NameFr, beCharger.NameZhcn, beCharger.NameZhtw, PreLang);

                            userNameList += userName + ",";
                            gununList += "M" + ",";
                        }

                        if (alCharger.Count > 0)
                        {
                            ChargerName = userIDList + "‡" + userNameList + "‡" + gununList.TrimEnd(',');
                        }
                    }
                }
                #endregion
            }
        }


        /// <summary>
        /// Recs the status.
        /// </summary>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public string GetStatus(string _status)
        {
            if (_status.Length == 0) return "";
            string response = "";

            switch (_status)
            {
                case "1":
                    response = GetResource("List_Status1");
                    break;
                case "2":
                    response = GetResource("List_Status2");
                    break;
                case "3":
                    response = GetResource("List_Status3");
                    break;
                case "4":
                    response = GetResource("List_Status4");
                    break;
                case "5":
                    response = GetResource("List_Status5");
                    break;
            }


            return response;
        }
               

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "0"); }
            set { ViewState["CompanyID"] = value; }
        }

        /// <summary>
        /// UserID
        /// </summary>
        public int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the charger.
        /// </summary>
        /// <value>The name of the charger.</value>
        public string ChargerName
        {
            get { return GetViewStateString("ChargerName", ""); }

            set { ViewState["ChargerName"] = value; }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        protected void lbtnOk_Click(object sender, System.EventArgs e)
        {
            BLL.Project biz = new BLL.Project();
            DAL.Project dal = new Zeospace.DeskPlus.PMS.DAL.Project();
            //BE.ProjectInfo projInfo = new Zeospace.DeskPlus.PMS.BE.ProjectInfo();

            var al = dal.GetProject(CompanyID, ProjectID);

            foreach (var projInfo in al)
            {
                projInfo.CompanyID = CompanyID;
                projInfo.ProjectID = ProjectID;

                projInfo.PlanStartDate = (Request[tbxPlanStartDate.ClientID].ToString().Length > 0) ? Convert.ToDateTime(Request[tbxPlanStartDate.ClientID].ToString()) : System.DateTime.MaxValue;
                projInfo.PlanEndDate = (Request[tbxPlanEndDate.ClientID].ToString().Length > 0) ? Convert.ToDateTime(Request[tbxPlanEndDate.ClientID].ToString()) : System.DateTime.MaxValue;
                projInfo.StartDate = (Request[tbxStartDate.ClientID].ToString().Length > 0) ? Convert.ToDateTime(Request[tbxStartDate.ClientID].ToString()) : System.DateTime.MaxValue;
                projInfo.EndDate = (Request[tbxEndDate.ClientID].ToString().Length > 0) ? Convert.ToDateTime(Request[tbxEndDate.ClientID].ToString()) : System.DateTime.MaxValue;

                //다국어 개발 필요
                projInfo.SubjectBase = tbxTitle.Text;

                projInfo.Process = ddlProcess.SelectedValue.ToNumber<int>();
                projInfo.Status = ddlStatus.SelectedValue.Trim().ToNumber<int>();
                projInfo.FolderID = tbxFolderID.Text.Trim().ToNumber<int>();

                //다국어 개발 필요
                projInfo.FolderBase = tbxFolder.Text;

                projInfo.Contents = tbxContent.Text;
                projInfo.WriteUserID = UserID;

                projInfo.ChargeDeptID = hdChargerID.Value.Trim(',');

                bool result = biz.Mod(projInfo);

                string msg = (result) ? GetResource("MSG_SUCCESS") : GetResource("MSG_FAIL");
                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.ClientMessage.Alert(msg);

                string url = @"location='{0}';";
                url = string.Format(url, "ProjectInfo.aspx?" + ProjParam);

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", msg, url);

                //Response.Write("<script language='javascript'>");
                //Response.Write("document.location.href=\"ProjectInfo.aspx?" + ProjParam + "\"");
                //Response.Write("</script>");
            }
        }
        #endregion
    }
}
