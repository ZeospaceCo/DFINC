using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.Web.Base;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Admin.Position
{
    public partial class List : BasePageAccountAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CodeID;
        public string Scope;
        public string Gubun;
        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.Position.List).Assembly;	//어셈블리 선언
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
            ltlHeader.Text = resMng.GetString("CodeManage");

            ddlActive.Items.Clear();                    
            ddlActive.Items.Add(new ListItem(GetResource("ActiveYes"), "1"));
            ddlActive.Items.Add(new ListItem(GetResource("ActiveNo"), "0"));
            ddlActive.SelectedValue = "1";

            #region 직급추가
            ddlAccessTitle.Items.Clear();

            this.ddlAccessTitle.Items.Add(new ListItem(resMng.GetString("MSG_SELITEM"), "0"));

            Zeospace.DeskPlus.Approval.DAL.AccessGrade cDal = new Zeospace.DeskPlus.Approval.DAL.AccessGrade();

            DataSet ds = cDal.GetListIsAccess(CurrentUserInfo.CompanyID, "1");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlAccessTitle.Items.Add(new ListItem(GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang)
                                                      , dr["AccessID"].ToString()));
            }
            #endregion

            ddlSearchField.SelectedValue = SearchField;
            ddlListCount.SelectedValue = RecordPerPage;
            ctbSearch.Text = SearchValue;            

            GetListCount();

            cbtnMove.Visible = false;
            cbtnAdd.Visible = true;
            cbtnDel.Visible = true;

            if (Gubun == "mod")
            {                
                ddlSearchField.SelectedValue = SearchField;
                ctbSearch.Text = SearchValue;

                Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
                List<Zeospace.DeskPlus.Common.BE.CodeInfo> al = dal.GetCodeByID(CurrentUserInfo.CompanyID, CodeID);

                foreach (Zeospace.DeskPlus.Common.BE.CodeInfo info in al)
                {
                    ctbName.Text = info.NameBase.ToReverseXSS();
                    ltlHeader.Text = UtilHelper.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                                info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

                    ctbMuliLang.Text = GetMuiltiLang(
                        info.NameKo.ToReverseXSS(), info.NameEn.ToReverseXSS(), info.NameJa.ToReverseXSS(), 
                        info.NameFr.ToReverseXSS(), info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS());

                    ctbDescription.Text = info.Description.ToReverseXSS();
                    ctbInterfaceCode.Text = info.InterfaceCode.ToReverseXSS();
                    ddlActive.SelectedValue = info.ActiveCode;

                    ddlAccessTitle.SelectedValue = info.AccessID;

                    if (info.Type == "S") //루트
                    {
                        cbtnAdd.Visible = false;
                        cbtnDel.Visible = false;
                        cbtnMove.Visible = true;
                    }

                    if (info.ParentCodeID > 0)
                    {
                        List<Zeospace.DeskPlus.Common.BE.CodeInfo> alPID = dal.GetCodeByID(CurrentUserInfo.CompanyID, info.ParentCodeID);
                        foreach (Zeospace.DeskPlus.Common.BE.CodeInfo infoPID in alPID)
                        {
                            string name = this.GetLangName(                                
                                infoPID.PathNameBase,
                                infoPID.PathNameKo,
                                infoPID.PathNameEn, infoPID.PathNameJa,
                                infoPID.PathNameFr, infoPID.PathNameZhcn,
                                infoPID.PathNameZhtw, PreLang);

                            switch (PreLang)
                            {
                                case "ko":
                                    name = (infoPID.NameKo == "") ? infoPID.PathNameBase : name;
                                    break;
                                case "en":
                                    name = (infoPID.NameEn == "") ? infoPID.PathNameBase : name;
                                    break;
                                case "ja":
                                    name = (infoPID.NameJa == "") ? infoPID.PathNameBase : name;
                                    break;
                                case "fr":
                                    name = (infoPID.NameFr == "") ? infoPID.PathNameBase : name;
                                    break;
                                case "zhcn":
                                    name = (infoPID.NameZhcn == "") ? infoPID.PathNameBase : name;
                                    break;
                                case "zhtw":
                                    name = (infoPID.NameZhtw == "") ? infoPID.PathNameBase : name;
                                    break;
                            }

                            ltlParentCodeID.Text = name.Substring(1, name.Length - 1);
                        }
                    }
                }
            }
            else
            {
                cbtnAdd.Visible = true;
                cbtnDel.Visible = false;
                cbtnMove.Visible = false;
            }
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            this.GetPagingParam();

            CodeID = GetDecryptParam("codeID", "0").ToNumber<int>();
            Scope = GetDecryptParam("scope", "T");
            Gubun = GetDecryptParam("gubun", "add");
            //Order = (Order.Length == 0) ? "WriteDate desc" : Order;
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
                //cbtnCancel.Text = resMng.GetString("Cancel");     
                cbtnAdd.Text = (Gubun == "add") ? resMng.GetString("BtnSave") : resMng.GetString("BtnSave");  // 등록 : 수정
                cbtnDel.Text = resMng.GetString("Admin_UserManager_GridHeader8");           // 삭제
                cbtnMove.Text = resMng.GetString("Admin_UserList_MSG_HTUserDeptOrder");     //"순서";
                cbtnSearch.Text = this.resMng.GetString("Admin_DeptManager_btnSearch");

                cbtnDel.Visible = (Gubun == "add") ? false : true;

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
            
            string[] dateValue = DateHelper.GetSearchTerm(SearchTerm, SearchValue, PreLangReal);

            TotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                "0", CodeID.ToString(), false, SearchField, SearchValue, dateValue[0], dateValue[1]).ToString();
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        private void Register()
        {
            Zeospace.DeskPlus.Common.BE.CodeInfo be = new DeskPlus.Common.BE.CodeInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ParentCodeID = CodeID;
            be.ActiveCode = ddlActive.SelectedValue;

            be.NameBase = ctbName.Text.Trim();
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

            be.InterfaceCode = ctbInterfaceCode.Text.Trim();
            be.Type = "U";
            be.Scope = Scope;
            be.Extension1 = "";
            be.Extension2 = "";
            be.Description = ctbDescription.Text.Trim();
            be.WriteUserID = CurrentUserInfo.UserID;
            be.AccessID = ddlAccessTitle.SelectedValue;

            Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
            int codeID = dal.Add(be);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();            
            paraDic.Add("codeID", codeID.ToString());
            string url = @"parent.frmLeft.location='{0}';";
            string encParams = GetEncryptParam(Helpers.WebComponent.Utility.GetParameterDic(paraDic));
            url = string.Format(url, "/DeskPlusEIP/Account/Admin/Position/Tree.aspx?p=" + encParams);
            Helpers.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("CompleteRegister"), url);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 수정
        /// </summary>
        private void Modify()
        {
            Zeospace.DeskPlus.Common.BE.CodeInfo be = new DeskPlus.Common.BE.CodeInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.CodeID = CodeID;
            be.ActiveCode = ddlActive.SelectedValue;

            be.NameBase = ctbName.Text.Trim();
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

            be.InterfaceCode = ctbInterfaceCode.Text.Trim();
            be.Type = "U";
            be.Scope = Scope;
            be.Extension1 = "";
            be.Extension2 = "";
            be.Description = ctbDescription.Text.Trim();
            be.WriteUserID = CurrentUserInfo.UserID;
            be.AccessID = ddlAccessTitle.SelectedValue;

            Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
            dal.Mod(be);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("codeID", CodeID.ToString());
            string url = @"parent.frmLeft.location='{0}';";
            string encParams = GetEncryptParam(Helpers.WebComponent.Utility.GetParameterDic(paraDic));
            url = string.Format(url, "/DeskPlusEIP/Account/Admin/Position/Tree.aspx?p=" + encParams);
            Helpers.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("CompleteModify"), url);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        private void Del()
        {
            if (CurrentUserInfo.CompanyID > 0 && CodeID > 0)
            {
                Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
                dal.Del(CurrentUserInfo.CompanyID, CodeID);

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("codeID", "0");
                string url = @"parent.frmLeft.location='{0}';";
                string encParams = GetEncryptParam(Helpers.WebComponent.Utility.GetParameterDic(paraDic));
                url = string.Format(url, "/DeskPlusEIP/Account/Admin/Position/Tree.aspx?p=" + encParams);
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
                case "":
                    if (Gubun == "mod")
                    {
                        Modify();
                    }
                    else if (Gubun == "add")
                    {
                        Register();
                    }

                    break;
                case "del":
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