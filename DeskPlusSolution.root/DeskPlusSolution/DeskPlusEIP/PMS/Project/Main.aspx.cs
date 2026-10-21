using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.PMS.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Drawing;

namespace Zeospace.DeskPlus.PMS.Web.Project
{
    public partial class Main : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        protected Zeospace.DeskPlus.Component.Chart.GaugeBar GaugeBar1;
        public string BoaList = "";
        public string UnitList = "";
        public string BoaHeaderList = "";
        public string UnitHeaderList = "";
        public int WeightTotal = 0;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Project.Main).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetProjInfo();
                ProjectBoardList();
            }
        }
        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 초기값 설정
		/// 작성자     : 유신종
		/// 최초작성일 : 2009-01-28
        /// </summary>
        private void SetProjInfo()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id     
            UserID = CurrentUserInfo.UserID; //userid
            #endregion

            DAL.Project bll = new Zeospace.DeskPlus.PMS.DAL.Project();
            var al = bll.GetProject(CompanyID, ProjectID);

            foreach (var projInfo in al)
            {   
                lbProjectName.Text = this.GetLangName(
                                    projInfo.SubjectBase,
                                    projInfo.SubjectKo, projInfo.SubjectEn, projInfo.SubjectJa,
                                    projInfo.SubjectFr, projInfo.SubjectZhcn, projInfo.SubjectZhtw, PreLang);

                lbProjectWUserName.Text = this.GetLangName(
                                    projInfo.NameBase,
                                    projInfo.NameKo, projInfo.NameEn, projInfo.NameJa,
                                    projInfo.NameFr, projInfo.NameZhcn, projInfo.NameZhtw, PreLang);


                if (projInfo.StartDate.ToNullDT() != System.DBNull.Value) lbSDate.Text = projInfo.StartDate.ToShortDateString();
                if (projInfo.EndDate.ToNullDT() != System.DBNull.Value) lbEDate.Text = projInfo.EndDate.ToShortDateString();
                if (projInfo.PlanStartDate.ToNullDT() != System.DBNull.Value) lbPlanSDate.Text = projInfo.PlanStartDate.ToShortDateString();
                if (projInfo.PlanEndDate.ToNullDT() != System.DBNull.Value) lbPlanEDate.Text = projInfo.PlanEndDate.ToShortDateString();

                if (projInfo.StartDate.ToNullDT() != System.DBNull.Value) ltlStartDate.Text = projInfo.StartDate.ToShortDateString();
                if (projInfo.EndDate.ToNullDT() != System.DBNull.Value) ltlEndDate.Text = projInfo.EndDate.ToShortDateString();

                if (ltlEndDate.Text.Length > 0)
                {
                    int compare = projInfo.EndDate.CompareTo(System.DateTime.Now);
                    if (compare > 0)
                    {
                        ltlProcessDate.Text = System.DateTime.Now.ToShortDateString();
                    }
                }

                //this.phProcess.Controls.Add(Page.ParseControl(GetResource("Unit_Register_ltlProcess")));
                //this.phProcess.Controls.Add(CreateChart(100, projInfo.Process));

                ProjProcess = projInfo.Process.ToString() + "%";
                ProjProcessStr = projInfo.Process.ToString() + "%";

                if (projInfo.Process > 100)
                {
                    ProjProcessStr = "100%";
                }

                if (projInfo.StartDate.ToNullDT() != System.DBNull.Value && projInfo.EndDate.ToNullDT() != System.DBNull.Value)
                {
                    Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();
                    List<int> ProjectDays = lib.GetProjcectDays(projInfo.StartDate, projInfo.EndDate, projInfo.CompleteDate);

                    lbTotalDays.Text = ProjectDays[0].ToString();
                    lbInDays.Text = ProjectDays[1].ToString();
                    lbRemainDays.Text = ProjectDays[2].ToString();
                    lbDelayDays.Text = ProjectDays[3].ToString();

                    if (ProjectDays[1] > 0)
                    {
                        float tmpTotalNum = Convert.ToSingle(Convert.ToInt32(Convert.ToSingle(ProjectDays[1]) / ProjectDays[0].ToString().ToNumber<int>() * 100));
                        //this.phPeriod.Controls.Add(CreateChart(100, tmpTotalNum));
                        ProjPeriod = tmpTotalNum.ToString() + "%";
                        ProjPeriodStr = tmpTotalNum.ToString() + "%";

                        if (tmpTotalNum > 100)
                        {
                            ProjPeriodStr = "100%";
                        }
                    }
                }
                else
                {
                    //this.phPeriod.Controls.Add(CreateChart(100, 0));
                    ProjPeriod = "0%";
                    ProjPeriodStr = "0%";
                }

                //this.phPeriod.Controls.Add(Page.ParseControl(GetResource("Main_DatePercent")));

                DAL.ProjectUser projUser = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();
                projUser.Mod(CompanyID, ProjectID, UserID);
            }
            
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 챠트 구현하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-02-06
        /// </summary>
        /// <param name="_maxNum">The _max num.</param>
        /// <param name="totalNum">The total num.</param>
        /// <returns></returns>
        private Zeospace.DeskPlus.Component.Chart.GaugeBar CreateChart(float _maxNum, float _totalNum)
        {
            GaugeBar1 = new Zeospace.DeskPlus.Component.Chart.GaugeBar();
            GaugeBar1.ImgPath = this.BaseImgPath + "/Component/Chart/";
            GaugeBar1.BorderColor = Color.White;
            GaugeBar1.BackColor = Color.White;
            GaugeBar1.ForeColor = Color.DarkGray;
            GaugeBar1.Width = new System.Web.UI.WebControls.Unit(350, UnitType.Pixel);

            GaugeBar1.Maximum = _maxNum;
            GaugeBar1.Value = _totalNum;

            GaugeBar1.ParticipantCnt = "";
            GaugeBar1.ParticipantUnit = "";
            GaugeBar1.ParticipantTitle = "";

            return GaugeBar1;
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 프로젝트별 게시판 메뉴값 가져오기
        /// </summary>
        private void ProjectBoardList()
        {            
            DAL.Menu dal = new DAL.Menu();
            DataTableReader dtr = dal.GetMenuBoaList(CurrentUserInfo.CompanyID, ProjectID);
            BoaList = "";
            BoaHeaderList = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BoaList += dtr["MenuID"].ToString() + ",";
                        BoaHeaderList += this.GetLangName(
                                                dtr["MenuBase"].ToString(),
                                                dtr["MenuKo"].ToString(),
                                                dtr["MenuEn"].ToString(),
                                                dtr["MenuJa"].ToString(),
                                                dtr["MenuFr"].ToString(),
                                                dtr["MenuZhCn"].ToString(),
                                                dtr["MenuZhTw"].ToString(), PreLang) + ",";
                    }

                    BoaList = BoaList.TrimEnd(',');
                    BoaHeaderList = BoaHeaderList.TrimEnd(',');
                }
            }


            BE.UnitInfo listInfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();
            DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();
            //가중치 합산 가져오기
            WeightTotal = list.GetUnitWeightPercent(CurrentUserInfo.CompanyID, ProjectID);

            dtr = null;

            //단위업무 아이디값 및 아이디 가져오기
            dtr = dal.GetMenuUnitList(CurrentUserInfo.CompanyID, ProjectID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        UnitList += dtr["MenuID"].ToString() + ",";

                        UnitHeaderList += this.GetLangName(
                                                dtr["MenuBase"].ToString(),
                                                dtr["MenuKo"].ToString(),
                                                dtr["MenuEn"].ToString(),
                                                dtr["MenuJa"].ToString(),
                                                dtr["MenuFr"].ToString(),
                                                dtr["MenuZhCn"].ToString(),
                                                dtr["MenuZhTw"].ToString(), PreLang) + ",";
                    }

                    UnitList = UnitList.TrimEnd(',');
                    UnitHeaderList = UnitHeaderList.TrimEnd(',');
                }
            }
        }
        

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "1"); }
            set { ViewState["CompanyID"] = value; }
        }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProjProcess
        {
            get { return GetViewStateString("ProjProcess", ""); }
            set { ViewState["ProjProcess"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProjPeriod
        {
            get { return GetViewStateString("ProjPeriod", ""); }
            set { ViewState["ProjPeriod"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProjProcessStr
        {
            get { return GetViewStateString("ProjProcessStr", ""); }
            set { ViewState["ProjProcessStr"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProjPeriodStr
        {
            get { return GetViewStateString("ProjPeriodStr", ""); }
            set { ViewState["ProjPeriodStr"] = value; }
        }

        #endregion


        #region Event Handler (이벤트 핸들러)

        #endregion
    }
}
