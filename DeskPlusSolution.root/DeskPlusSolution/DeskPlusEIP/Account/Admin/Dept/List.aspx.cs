using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Admin.Dept
{
    public partial class List : Base.BasePageAccountAdmin
    {
        #region Declaration (컨트롤 선언)
        
        #endregion

        #region 변수설정                
        public int DeptID;        
        public string Gubun;
        public string ParentDeptID;

        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.Dept.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            ltlHeader.Text = resMng.GetString("DeptInfo");
            ddlSearchGubun.SelectedValue = SearchGubun;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();            

            if (Gubun == "mod")
            {
                DAL.Dept dal = new DAL.Dept();
                List<BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, DeptID, false);
                int parentDeptID = 0;
                foreach (BE.DeptInfo info in al)
                {
                    ctbDeptName.Text = info.NameBase.ToReverseXSS();
                    ltlHeader.Text = UtilHelper.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                                info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

                    parentDeptID = info.ParentDeptID;

                    cbxRootDept.Checked = (info.RootDept == "1" || info.RootDept == "3") ? true : false;
                    cbxRealDept.Checked = (info.RootDept == "2" || info.RootDept == "3") ? true : false;

                    ctbMuliLang.Text = GetMuiltiLang(
                        info.NameKo.ToReverseXSS(), info.NameEn.ToReverseXSS(), info.NameJa.ToReverseXSS(), info.NameFr.ToReverseXSS(), info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS());

                    ctbShortName.Text = info.ShortName.ToReverseXSS();
                    ctbDescription.Text = info.Description.ToReverseXSS();
                    ctbZipCode.Text = info.ZipCode;
                    ctbAddress1.Text = info.Address1;
                    ctbAddress2.Text = info.Address2.ToReverseXSS();
                    ctbPhone.Text = info.Phone.ToReverseXSS();
                    ctbFax.Text = info.Fax.ToReverseXSS();
                    ctbHomepage.Text = info.HomePage.ToReverseXSS();
                    ctbDeptCode.Text = info.DeptCode;
                    cbxActive.Checked = (info.ActiveDept == "1") ? true :false;
                }

                al = null;
                al = dal.GetDeptLists(CurrentUserInfo.CompanyID, parentDeptID, false);
                foreach (BE.DeptInfo info in al)
                {
                    this.ctbParentName.Text = UtilHelper.GetLangName(
                        info.NameBase, info.NameKo, info.NameEn,
                        info.NameJa, info.NameFr, info.NameZhcn,
                        info.NameZhtw, PreLang).ToReverseXSS();
                }

                ParentDeptID = parentDeptID.ToString();
                
            }
            else if (Gubun == "add")
            {
                DAL.Dept dal = new DAL.Dept();
                List<BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, DeptID, false);
                
                foreach (BE.DeptInfo info in al)
                {
                    this.ctbParentName.Text = UtilHelper.GetLangName(
                        info.NameBase, info.NameKo, info.NameEn,
                        info.NameJa, info.NameFr, info.NameZhcn,
                        info.NameZhtw, PreLang).ToReverseXSS();
                }

                ParentDeptID = "-1";
            }

            //추가 및 최상위는 상위부서변경 버튼 감춘다.
            cbtnChangeParentDept.Visible = (Gubun == "add") ? false : true;

            if (ParentDeptID.ToNumber<int>() < 0)
            {
                cbtnChangeParentDept.Visible = false;
            }
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            this.GetPagingParam();

            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();            
            Gubun = GetDecryptParam("gubun", "add");            
            SearchGubun = (DeptID == 0) ? "Whole" : "Internal";
            Order = (Order.Length == 0) ? "TitleCodeOrder asc, DutyCodeOrder asc, UserDeptOrder desc, DeptOrder asc, WriteDate desc" : Order;
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = (Gubun == "add") ? resMng.GetString("BtnSave") : resMng.GetString("BtnSave");                
                cbtnDel.Text = resMng.GetString("Admin_UserManager_GridHeader8");                
                cbtnMove.Text = resMng.GetString("Admin_UserList_MSG_HTUserDeptOrder");
                cbtnDel.Visible = (Gubun == "add" || DeptID == 0) ? false : true;
                cbtnSearch.Text = this.resMng.GetString("Admin_DeptManager_btnSearch");
                cbtnChangeParentDept.Text = this.resMng.GetString("ChangeUpperDept");

                #region 구분자
                ddlSearchGubun.Items.Clear();
                ddlSearchGubun.Items.Add(new ListItem(this.resMng.GetString("InternalSearch"), "Internal"));
                ddlSearchGubun.Items.Add(new ListItem(this.resMng.GetString("WholeSearch"), "Whole"));
                #endregion

                #region 인사검색필드
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Private_AccountInfo_ltUserName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Private_AccountInfo_ltTitle"), "ManyTitleName"));
                #endregion

                

            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 부서 사용자 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            DAL.User biz = new DAL.User();

            //검색은 전체에서 검색한다.
            //string deptID = (SearchValue.Length > 0) ? "0" : DeptID.ToString();
            string deptID = (SearchGubun == "Whole") ? "0" : DeptID.ToString();
            string[] dateValue = DateHelper.GetSearchTerm(SearchTerm, SearchValue, PreLangReal);

            TotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                deptID, "0", false, SearchField, SearchValue, dateValue[0], dateValue[1]).ToString();
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        private void Register()
        {
            BE.DeptInfo be = new BE.DeptInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ParentDeptID = DeptID;

            if (cbxRootDept.Checked && cbxRealDept.Checked) //가상부서 + 최상위부서
            {
                be.RootDept = "3";
            }            
            else if (cbxRealDept.Checked) //가상부서만
            {
                be.RootDept = "2";
            }
            else if (cbxRootDept.Checked) //최상위부서만
            {
                be.RootDept = "1";
            }
            else
            {
                be.RootDept = "0";
            }
            
            be.RealDept = ""; //모르겠음.
            be.ActiveDept = (cbxActive.Checked) ? "1" : "0";

            be.NameBase = ctbDeptName.Text.Trim();
            string multiName = ctbMuliLang.Text.Trim();

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.NameKo = multiName.Split(',')[0];
                be.NameEn = multiName.Split(',')[1];
                be.NameJa = multiName.Split(',')[2];
                be.NameFr = multiName.Split(',')[3];
                be.NameZhcn = multiName.Split(',')[4];
                be.NameZhtw = multiName.Split(',')[5];
            }

            be.ShortName = ctbShortName.Text.Trim();
            be.Description = ctbDescription.Text.Trim();
            be.DeptCode = ctbDeptCode.Text.Trim();

            be.ZipCode = ctbZipCode.Text.Trim();
            be.Address1 = ctbAddress1.Text.Trim();
            be.Address2 = ctbAddress2.Text.Trim();
            be.Phone = ctbPhone.Text.Trim();
            be.Fax = ctbFax.Text.Trim();
            be.HomePage = ctbHomepage.Text.Trim();
            be.WriteUserID = CurrentUserInfo.UserID;

            DAL.Dept dal = new DAL.Dept();
            int deptID = dal.Add(be);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();            
            paraDic.Add("deptID", deptID.ToString());
            string url = @"parent.frmFolderLeft.location='{0}';";
            string encParams = GetEncryptParam(Helpers.WebComponent.Utility.GetParameterDic(paraDic));
            url = string.Format(url, "/DeskPlusEIP/Account/Admin/Dept/Tree.aspx?p=" + encParams);
            Helpers.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("CompleteRegister"), url);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 수정
        /// </summary>
        private void Modify()
        {
            BE.DeptInfo be = new BE.DeptInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.DeptID = DeptID;

            if (cbxRootDept.Checked && cbxRealDept.Checked) //가상부서 + 최상위부서
            {
                be.RootDept = "3";
            }
            else if (cbxRealDept.Checked) //가상부서만
            {
                be.RootDept = "2";
            }
            else if (cbxRootDept.Checked) //최상위부서만
            {
                be.RootDept = "1";
            }
            else
            {
                be.RootDept = "0";
            }

            be.RealDept = ""; //모르겠음.
            be.ActiveDept = (cbxActive.Checked) ? "1" : "0";

            be.NameBase = ctbDeptName.Text.Trim();
            string multiName = ctbMuliLang.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.NameKo = multiName.Split(',')[0];
                be.NameEn = multiName.Split(',')[1];
                be.NameJa = multiName.Split(',')[2];
                be.NameFr = multiName.Split(',')[3];
                be.NameZhcn = multiName.Split(',')[4];
                be.NameZhtw = multiName.Split(',')[5];
            }

            be.ShortName = ctbShortName.Text.Trim();
            be.Description = ctbDescription.Text.Trim();
            be.DeptCode = ctbDeptCode.Text.Trim();

            be.ZipCode = ctbZipCode.Text.Trim();
            be.Address1 = ctbAddress1.Text.Trim();
            be.Address2 = ctbAddress2.Text.Trim();
            be.Phone = ctbPhone.Text.Trim();
            be.Fax = ctbFax.Text.Trim();
            be.HomePage = ctbHomepage.Text.Trim();
            be.WriteUserID = CurrentUserInfo.UserID;

            DAL.Dept dal = new DAL.Dept();
            dal.Mod(be);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();            
            paraDic.Add("deptID", DeptID.ToString());
            string url = @"parent.frmFolderLeft.location='{0}';";
            string encParams = GetEncryptParam(Helpers.WebComponent.Utility.GetParameterDic(paraDic));
            url = string.Format(url, "/DeskPlusEIP/Account/Admin/Dept/Tree.aspx?p=" + encParams);
            Helpers.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("CompleteModify"), url);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        private void Del()
        {
            if (CurrentUserInfo.CompanyID > 0 && DeptID > 0)
            {
                DAL.Dept dal = new DAL.Dept();
                List<BE.DeptInfo> al = dal.GetParentDeptLists(CurrentUserInfo.CompanyID, DeptID, false);

                if (al.Count > 0)
                {
                    Helpers.WebComponent.Utility.ShowMessageBox(Page, "existUser", GetResource("AlertExitParentDept"), "");
                    return;
                }


                DAL.UserDept userDeptBiz = new DAL.UserDept();
                int cnt = userDeptBiz.GetCountByDeptID(CurrentUserInfo.CompanyID, DeptID, false);
                if (cnt > 0)
                {
                    Helpers.WebComponent.Utility.ShowMessageBox(Page, "existUser", GetResource("AlertExitUserByDeptID"), "");
                    return;
                }

                dal.Del(CurrentUserInfo.CompanyID, DeptID);
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("deptID", "0");
                string url = @"parent.frmFolderLeft.location='{0}';";
                string encParams = GetEncryptParam(Helpers.WebComponent.Utility.GetParameterDic(paraDic));
                url = string.Format(url, "/DeskPlusEIP/Account/Admin/Dept/Tree.aspx?p=" + encParams);
                Helpers.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("CompleteDelete"), url);
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            LoadParam();

            string args = eventArgs;

            switch (args)
            {
                case "" :
                    if (Gubun == "mod")
                    {
                        Modify();
                    }
                    else if (Gubun == "add")
                    {
                        Register();
                    }

                    break;
                case "del" :
                    Del();
                    break;
            }            
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }


        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}