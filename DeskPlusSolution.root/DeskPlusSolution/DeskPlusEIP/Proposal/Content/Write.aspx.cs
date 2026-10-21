using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Proposal.BE;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Proposal.Web.Content
{
    public partial class Write : Base.BasePageProposal
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.Write).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            LoadCodeInfo();
            LoadUserInfo();
        }

        #endregion

        #region (함수)

        private void LoadParam()
        {
            
        }

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
        }

        private void LoadCodeInfo()
        {
            RblCode.Items.Clear();
            RblCode.Items.Add(new ListItem("관리본부", "1"));
            RblCode.Items.Add(new ListItem("영업본부", "2"));
            RblCode.Items.Add(new ListItem("기술연구소", "3"));
            RblCode.Items.Add(new ListItem("천안공장", "4"));

            //RblCode.Items[0].Selected = true;
        }


        /// <summary>
        /// 작성자 : shine
        /// 용도 : 작성자 정보 가져오기
        /// </summary>
        private void LoadUserInfo()
        {
            int pIndex = 0;
            int pDeptID = 0;

            Account.DAL.Dept dalDept = new Dept();

            // 접수일자
            ltlWriteDate.Text = System.DateTime.Now.Date.ToShortDateString();
            // 직위
            ltlTitleName.Text = CurrentUserInfo.TitleName;
            // 사번
            ltlEmpNo.Text = CurrentUserInfo.EmployeeNumber;
            // 작성자
            ltlWriteName.Text = CurrentUserInfo.Name;
            // 상위부서명
            ltlDeptName.Text = "";

            // 상위부서 DeptIDIndex
            pIndex = CurrentUserInfo.UpDeptIDInfo.Split(',').Length - 2;

            if (pIndex > -1)
            {
                // 상위부서 아이디
                pDeptID = CurrentUserInfo.UpDeptIDInfo.Split(',')[pIndex].ToNumber<int>();

                var deptInfo = dalDept.GetDeptLists(CurrentUserInfo.CompanyID, pDeptID, true);

                if (deptInfo != null)
                {
                    foreach (var dept in deptInfo)
                    {
                        ltlDeptName.Text = this.GetLangName(dept.NameBase.ToString().ToNull(),
                                                    dept.NameKo.ToString().ToNull(),
                                                    dept.NameEn.ToString().ToNull(),
                                                    dept.NameJa.ToString().ToNull(),
                                                    dept.NameFr.ToString().ToNull(),
                                                    dept.NameZhcn.ToString().ToNull(),
                                                    dept.NameZhtw.ToString(), PreLang);
                    }
                }
            }

        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 제안 추가하기
        /// 작성자   : shine
        /// </summary>
        /// <returns></returns>
        private int WriteMode()
        {
            BE.ListInfo be = new ListInfo();
            BLL.List bll = new BLL.List();

            #region 내용
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ProposalNo = 0;
            be.ReceiptNo = 0;
            be.Title = ctbTitle.Text;
            be.Gubun = ddlGubun.SelectedValue;
            be.UserID = CurrentUserInfo.UserID;
            be.Name = CurrentUserInfo.Name;
            be.Status = "2";
            be.BeforeCont = tbBefor.InnerText;
            be.AfterCont = tbAfter.InnerText;
            be.BeforeEffect = tbEffect.InnerText;
            be.AfterEffect = "";
            be.Etc1 = tbEtc.InnerText;
            be.Bul_Select = RblCode.SelectedValue.ToNumber<int>();

            be.CoWorkUserIDList = hdCoWorkID.Value.Trim();
            be.JudgeUserIDList = hdJudgeID.Value.Trim();

            #endregion

            #region 첨부파일

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);            

            #endregion

            return bll.Add(be);
        }

        #endregion

        #region (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            LoadParam();

            int proposalNo = WriteMode();

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("currentPage", "1");
            paraDic.Add("listCount", ListCount);
            paraDic.Add("proposalNo", proposalNo.ToString());
            paraDic.Add("menuID", "MyList");

            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", "보기 화면로 이동합니다.");
            paraDic.Add("goURL", "/DeskplusEIP/Proposal/Content/View.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }
        #endregion
    }
}