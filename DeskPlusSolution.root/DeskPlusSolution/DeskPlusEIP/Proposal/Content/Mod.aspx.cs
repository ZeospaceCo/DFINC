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
    public partial class Mod : Base.BasePageProposal
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int ProposalNo = 0;
        public string IsAttach = "";
        public string MenuID = "";
        public string Status = "";
        public string AttachList = "";
        public string CoWorkList = "";
        public string JudgeList = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.Mod).Assembly;	//어셈블리 선언
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
            LoadList();
            LoadCoworkList();
            LoadJudgeList();
        }

        #endregion

        #region (함수)

        private void LoadParam()
        {
            ProposalNo = GetDecryptParam("proposalNo", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "");
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
            RblCode.Items.Add(new ListItem("경영본부", "1"));
            RblCode.Items.Add(new ListItem("반도체본부(반도체마케팅/품질보증포함)", "2"));           
            RblCode.Items.Add(new ListItem("시스템본부(시스템마케팅/품질보증포함)", "3"));
            RblCode.Items.Add(new ListItem("마이크로본부", "4"));

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
        /// 작성자 : shine
        /// 용도 : 제안 내용 가져오기
        /// </summary>
        private void LoadList()
        {
            int writeUserID = 0;
            int statusID = 0;
            bool isAuth = false;

            DAL.List biz = new DAL.List();

            DataTableReader dtr = biz.GetList(CurrentUserInfo.CompanyID, ProposalNo);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        writeUserID = dtr["UserID"].ToString().ToNumber<int>();
                        statusID = dtr["Status"].ToString().ToNumber<int>();

                        // 상위부서
                        string pDeptName = GetLangName(dtr["PNameBase"].ToString().ToNull(), dtr["PNameKo"].ToString().ToNull(),
                            dtr["PNameEn"].ToString().ToNull(), dtr["PNameJa"].ToString().ToNull(),
                            dtr["PNameFr"].ToString().ToNull(), dtr["PNameZhcn"].ToString().ToNull(),
                            dtr["PNameZhtw"].ToString().ToNull(), PreLang);
                        // 부서
                        string deptName = GetLangName(dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                            dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                            dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                            dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                        // 이름
                        string name = GetLangName(dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                            dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                            dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                            dtr["NameZhtw"].ToString().ToNull(), PreLang);

                        // 소속
                        ltlDeptName.Text = pDeptName + " " + deptName;
                        // 접수일자
                        ltlWriteDate.Text = Convert.ToDateTime(dtr["WriteDate"].ToString()).ToShortDateString();
                        // 직위
                        ltlTitleName.Text = GetLangName(dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                            dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                            dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                            dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        // 사번
                        ltlEmpNo.Text = dtr["EmployeeNumber"].ToString().ToNull();
                        // 년월
                        string yyyymm = dtr["YYYYMM"].ToString().Substring(2, 4);
                        // 번호
                        string no = dtr["ReceiptNo"].ToString().PadLeft(3, '0');
                        // 접수번호
                        ltlNo.Text = "IP" + yyyymm + "-" + no;
                        // 작성자
                        ltlWriteName.Text = CurrentUserInfo.Name;
                        // 제안구분
                        ddlGubun.SelectedValue = dtr["Gubun"].ToString();
                        // 제안제목
                        ctbTitle.Text = dtr["Title"].ToString();
                        // 집계코드
                        RblCode.SelectedValue = dtr["Bul_Select"].ToString();

                        // 1. 현행 및 문제점
                        tbBefor.InnerText = dtr["BeforeCont"].ToString().Replace("\r\n", "<br />");
                        // 2. 개선안
                        tbAfter.InnerText = dtr["AfterCont"].ToString().Replace("\r\n", "<br />");
                        // 3. 제안의 예상효과
                        tbEffect.InnerText = dtr["BeforeEffect"].ToString().Replace("\r\n", "<br />");
                        // 비고
                        tbEtc.InnerText = dtr["Etc1"].ToString().Replace("\r\n", "<br />");

                        //접근여부
                        isAuth = (CurrentUserInfo.UserID == writeUserID) ? true : false;
                    }
                }
            }

            if (!isAuth)
            {
                Utility.ShowMessageBoxNoAuth(Page, "권한이 없습니다.", "");
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 공동 제안자 정보 가져오기
        /// </summary>
        private void LoadCoworkList()
        {
            DAL.Person person = new DAL.Person();
            DataTableReader dtr = person.GetPersonInfo(CurrentUserInfo.CompanyID, ProposalNo);

            string userID = "";
            string name = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        userID += dtr["UserID"].ToString() + ",";
                        name += dtr["Name"].ToString() + ",";
                    }
                }
            }

            userID = userID.TrimEnd(',');
            name = name.TrimEnd(',');

            CoWorkList = userID + "‡" + name;
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 1차 심사위원 정보 가져오기
        /// </summary>
        private void LoadJudgeList()
        {
            DAL.JudgeUser judge = new DAL.JudgeUser();
            DataTableReader dtr = judge.GetJudgeUserInfo(CurrentUserInfo.CompanyID, ProposalNo);

            string userID = "";
            string name = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        userID += dtr["UserID"].ToString() + ",";
                        name += GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                PreLang) + ",";
                    }
                }
            }

            userID = userID.TrimEnd(',');
            name = name.TrimEnd(',');
            JudgeList = userID + "‡" + name;
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 제안 추가하기
        /// 작성자   : shine
        /// </summary>
        /// <returns></returns>
        private bool ModContent()
        {
            BE.ListInfo be = new ListInfo();
            BLL.List bll = new BLL.List();

            #region 내용
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ProposalNo = ProposalNo;
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

            return bll.Mod(be);
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
            try
            {
                LoadParam();
                ModContent();

                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                paraDic.Add("currentPage", "1");
                paraDic.Add("listCount", ListCount);
                paraDic.Add("proposalNo", ProposalNo.ToString());
                paraDic.Add("menuID", "MyList");

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", "보기 화면로 이동합니다.");
                paraDic.Add("goURL", "/DeskplusEIP/Proposal/Content/View.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", "처리 시 오류가 발생되었습니다. 관리자에게 문의하시기 바랍니다.", "");
            }
        }
        #endregion
    }
}