using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.PMS.Web.Project
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
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Project.ProjectInfo).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
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
            #endregion

            LoadProject();

            ltlMenuName.Text = MenuName;
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
                        List<int> al = lib.GetProjcectDays(be.StartDate, be.EndDate, be.CompleteDate);
                        lblWholeDay.Text = al[0].ToString();
                        lblInjecDay.Text = al[1].ToString();
                        lblRestDay.Text = al[2].ToString();
                        lblDelayDay.Text = al[3].ToString();
                    }

                    if (be.StartDate.ToNullDT() != System.DBNull.Value) lblStartDate.Text = be.StartDate.ToShortDateString();
                    if (be.EndDate.ToNullDT() != System.DBNull.Value) lblEndDate.Text = be.EndDate.ToShortDateString();

                    if (be.PlanStartDate.ToNullDT() != System.DBNull.Value) lblPlanStartDate.Text = be.PlanStartDate.ToShortDateString();
                    if (be.PlanEndDate.ToNullDT() != System.DBNull.Value) lblPlanEndDate.Text = be.PlanEndDate.ToShortDateString();

                    lblContent.Text = Regex.Replace(be.Contents.Replace("\r\n", "<br>"), "<img", "<ximg", RegexOptions.IgnoreCase);

                    lblFolder.Text = this.GetLangName(
                                    be.FolderBase,
                                    be.FolderKo, be.FolderEn, be.FolderJa,
                                    be.FolderFr, be.FolderZhcn, be.FolderZhtw, PreLang);

                    //(1:대기;2:진행;3:완료;4:지연;5:보류)
                    lblIsOpen.Text = (be.IsOpen == 1) ? GetResource("Project_ProjectInfo_Approval") : GetResource("Project_ProjectInfo_NoApproval");
                    lblProcess.Text = be.Process.ToString();
                    lblStatus.Text = GetStatus(be.Status.ToString());
                    lblTitle.Text = this.GetLangName(
                                    be.SubjectBase,
                                    be.SubjectKo, be.SubjectEn, be.SubjectJa,
                                    be.SubjectFr, be.SubjectZhcn, be.SubjectZhtw, PreLang);

                    lblWriteDate.Text = Convert.ToDateTime(be.WriteDate).ToShortDateString();
                    //lblWriteUserName.Text = Component.WebComponent.StringHelper.GetNameDBPreLang(PreLang, be.WriteUserName.Trim());

                    lblWriteUserName.Text = this.GetLangName(
                                    be.NameBase,
                                    be.NameKo, be.NameEn, be.NameJa,
                                    be.NameFr, be.NameZhcn, be.NameZhtw, PreLang);


                    #region 책임자 정보 가져오기
                    DAL.ProjectCharger charger = new Zeospace.DeskPlus.PMS.DAL.ProjectCharger();
                    List<BE.ProjectChargerInfo> alCharger = charger.GetProjectCharger(CompanyID, ProjectID);

                    if (alCharger != null)
                    {
                        string nameList = "";

                        foreach (BE.ProjectChargerInfo beCharger in alCharger)
                        {

                            string userName = this.GetLangName(
                                    beCharger.NameBase,
                                    beCharger.NameKo, beCharger.NameEn, beCharger.NameJa,
                                    beCharger.NameFr, beCharger.NameZhcn, beCharger.NameZhtw, PreLang);

                            //name += Component.WebComponent.StringHelper.GetNameDBPreLang(PreLang, beCharger.UserName.Trim()) + ",";
                            nameList += userName.Trim() + ",";
                        }

                        lblCharger.Text = nameList.TrimEnd(',');
                    }
                    #endregion
                }
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
            get
            {
                return GetViewStateInt("CompanyID", "0");
            }
            set
            {
                ViewState["CompanyID"] = value;
            }
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메뉴제목
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-22
        /// </summary>
        /// <value>The name of the menu.</value>
        public string MenuName
        {
            get
            {
                Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();
                return lib.MenuName(CompanyID, ProjectID, MenuID, PreLang);
            }
        }
        #endregion


        #region Event Handler (이벤트 핸들러)

        #endregion
    }
}
