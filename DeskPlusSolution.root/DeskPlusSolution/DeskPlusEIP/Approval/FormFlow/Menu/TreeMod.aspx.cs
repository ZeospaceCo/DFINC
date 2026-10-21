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
    public partial class TreeMod : Base.BasePageApproval
    {
        #region 변수설정
        public int DocumentID = 0;
        public int GroupID = 0;
        public int Sequence = 0;
        public string ApprovalTag = "";
        public string AccountTag = "";
        public int TargetUserID = 0;
        public int TargetDeptID = 0;
        public string FolderStatusID = "";
        public int ProxyUserID = 0;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Menu.TreeMod).Assembly;	//어셈블리 선언
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
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
            Sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            ApprovalTag = GetDecryptParam("approvalTag", "");
            AccountTag = GetDecryptParam("accountTag", "");
            TargetUserID = GetDecryptParam("targetUserID", "0").ToNumber<int>();
            TargetDeptID = GetDecryptParam("targetDeptID", "0").ToNumber<int>();
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();
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
            #region 겸직정보
            ddlAdditionDept.Items.Clear();

            int selIdx = 0;
            Account.DAL.User uDal = new Account.DAL.User();
            List<Account.BE.UserInfo> list = uDal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, false);
            if (list.Count > 0)
            {
                int i = 0;
                string deptname = "", titlename = "", dutyname = "";
                foreach (Account.BE.UserInfo info in list)
                {
                    deptname = GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang).ToReverseXSS();
                    titlename = GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang).ToReverseXSS();
                    dutyname = GetLangName(info.DutyBase, info.DutyKo, info.DutyEn, info.DutyJa, info.DutyFr, info.DutyZhCn, info.DutyZhTw, PreLang).ToReverseXSS();

                    // 최상위 부서 정보 가져오기
                    Account.BLL.Dept dept = new Account.BLL.Dept();
                    int rootDeptID = dept.GetRootDeptID(CurrentUserInfo.CompanyID, info.DeptID, false);

                    string rootDeptName = "";
                    Zeospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
                    List<Account.BE.DeptInfo> deptlist = dDal.GetDeptLists(CurrentUserInfo.CompanyID, rootDeptID, true);
                    foreach (Account.BE.DeptInfo Info in deptlist)
                    {
                        rootDeptName = GetLangName(Info.NameBase, Info.NameKo, Info.NameEn, Info.NameJa, Info.NameFr, Info.NameZhcn, Info.NameZhtw, PreLang).ToReverseXSS();
                    }

                    ListItem item = new ListItem(deptname + "|" + titlename
                        , info.DeptID + "|" + deptname + "|" + info.CodeTitleID + "|" + titlename + "|" + info.CodeDutyID + "|" + dutyname + "|" + rootDeptID + "|" + rootDeptName);

                    item.Attributes.Add("deptID", info.DeptID.ToString());

                    ddlAdditionDept.Items.Add(item);

                    if (info.DeptID.Equals(CurrentUserInfo.DeptID))
                    {
                        selIdx = i;
                    }
                    i++;
                }
            }
            else
            {
                // 최상위 부서 정보 가져오기
                Account.BLL.Dept dept = new Account.BLL.Dept();
                int rootDeptID = dept.GetRootDeptID(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, false);

                string rootDeptName = "";
                Zeospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
                List<Account.BE.DeptInfo> deptlist = dDal.GetDeptLists(CurrentUserInfo.CompanyID, rootDeptID, true);
                foreach (Account.BE.DeptInfo Info in deptlist)
                {
                    rootDeptName = GetLangName(Info.NameBase, Info.NameKo, Info.NameEn, Info.NameJa, Info.NameFr, Info.NameZhcn, Info.NameZhtw, PreLang).ToReverseXSS();
                }

                ListItem item = new ListItem(CurrentUserInfo.DeptName + "|" + CurrentUserInfo.TitleName
                        , CurrentUserInfo.DeptID + "|" + CurrentUserInfo.DeptName + "|" + CurrentUserInfo.CodeTitleID + "|" + CurrentUserInfo.TitleName + "|" + CurrentUserInfo.CodeDutyID + "|" + CurrentUserInfo.DutyName + "|" + rootDeptID + "|" + rootDeptName);

                item.Attributes.Add("deptID", CurrentUserInfo.DeptID.ToString());

                ddlAdditionDept.Items.Add(item);
            }

            selIdx = (ddlAdditionDept.Items[selIdx] != null) ? selIdx : 0;
            ddlAdditionDept.Items[selIdx].Selected = true;
            #endregion

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