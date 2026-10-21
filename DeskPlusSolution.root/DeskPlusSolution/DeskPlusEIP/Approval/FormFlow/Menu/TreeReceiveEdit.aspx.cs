using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.DAL;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Menu
{
    public partial class TreeReceiveEdit : Base.BasePageApproval
    {
        #region 변수설정
        public int DocumentID = 0;
        public string ApprovalState = "";
        public string AccountTag = "";
        public string FolderStatusID = "";
        public StringBuilder AccessTitleGrade = new StringBuilder("");    
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Menu.TreeReceiveEdit).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                this.tbOpinion.Attributes.Add("maxlength", "500");

                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            DocumentID = GetDecryptParam("docID", "0").ToNumber<int>();
            ApprovalState = GetDecryptParam("state", "0").ToString();
            AccountTag = GetDecryptParam("accountTag", "0").ToString();
            FolderStatusID = GetDecryptParam("folderStatusID", "0").ToString();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            // 0. 초기화 
            initData();

            // 1. 설정정보 체크 (겸직/문서종류/열람권한)
            GetSettingInfo();
        }

        private void initData()
        {
            #region 조직도 초기화
            UCDeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCDeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;

            UCDeptUserTreeScript1.UseActiveDept = true;
            UCDeptUserTreeScript1.UseIcon = true;
            UCDeptUserTreeScript1.UseMyRootDept = false;
            UCDeptUserTreeScript1.UseVirtualTree = false;

            UCDeptUserTreeScript1.UseRootCheckBox = false;
            UCDeptUserTreeScript1.UseCheckBoxGroup = false;
            UCDeptUserTreeScript1.UseCheckBoxMember = false;
            #endregion
        }

        /// <summary>
        /// setting info
        /// </summary>
        private void GetSettingInfo()
        {
            #region 문서종류
            //보안문서여부 : 일반(0)/보안(1)/긴급(2)
            ddlGuard.Items.Clear();

            ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Normal"), "0"));
            ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Security"), "1"));
            ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Emergency"), "2"));

            ddlGuard.SelectedIndex = 0;
            #endregion

            /*
            #region 열람권한
            ddlAccessTitle.Items.Clear();

            Zeospace.DeskPlus.Common.DAL.Code cDal = new Code();
            DataSet ds = cDal.GetCodeByScope(CurrentUserInfo.CompanyID, "T", true);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlAccessTitle.Items.Add(new ListItem(GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang)
                                                      , dr["CodeID"].ToString()));
            }

            ddlAccessTitle.SelectedIndex = ddlAccessTitle.Items.Count - 1;
            #endregion
            */

            #region 열람권한

            string accessBase = "";
            ddlAccessTitle.Items.Clear();

            Zeospace.DeskPlus.Approval.DAL.AccessGrade cDal = new Zeospace.DeskPlus.Approval.DAL.AccessGrade();

            DataSet ds = cDal.GetListIsAccess(CurrentUserInfo.CompanyID, "1");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlAccessTitle.Items.Add(new ListItem(GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang)
                                                      , dr["AccessID"].ToString()));

                AccessTitleGrade.Append("<tr id='grd_" + dr["AccessID"].ToString() + "'>");
                AccessTitleGrade.Append("<th>");
                AccessTitleGrade.Append(GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang));
                AccessTitleGrade.Append("</th>");
                AccessTitleGrade.Append("<td>");
                AccessTitleGrade.Append(GetLangName(dr["AccessGradeUser"].ToString().ToReverseXSS(), dr["AccessGradeUserKo"].ToString().ToReverseXSS(), dr["AccessGradeUserEn"].ToString().ToReverseXSS(), dr["AccessGradeUserJa"].ToString().ToReverseXSS(), dr["AccessGradeUserFr"].ToString().ToReverseXSS(), dr["AccessGradeUserZhcn"].ToString().ToReverseXSS(), dr["AccessGradeUserZhtw"].ToString().ToReverseXSS(), PreLang));
                AccessTitleGrade.Append("</td>");
                AccessTitleGrade.Append("</tr>");

                accessBase = (dr["IsBase"].ToString().Equals("1") && accessBase.Equals("")) ? dr["AccessID"].ToString() : accessBase;
            }

            // 기본값처리
            if (ddlAccessTitle.Items.FindByValue(accessBase) != null)
            {
                ddlAccessTitle.SelectedValue = accessBase;
            }
            else
            {
                ddlAccessTitle.SelectedIndex = ddlAccessTitle.Items.Count - 1;
            }

            #endregion

            #region 참조열람
            ddlCarbonView.Items.Clear();
            ddlCarbonView.Items.Add(new ListItem(this.resMng.GetString("Use"), "1"));
            ddlCarbonView.Items.Add(new ListItem(this.resMng.GetString("NotUse"), "0"));
            ddlCarbonView.SelectedIndex = ddlCarbonView.Items.Count - 1;

            string reportUserCarbonViewOption = GetEnvKeyValue(CurrentUserInfo.CompanyID, Helpers.AppSettingType.APPROVAL, "ReportUserCarbonViewOption", "0");
            if (reportUserCarbonViewOption == "1")
                trCarbonView.Visible = true;
            else
                trCarbonView.Visible = false;

            #endregion
        }

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}