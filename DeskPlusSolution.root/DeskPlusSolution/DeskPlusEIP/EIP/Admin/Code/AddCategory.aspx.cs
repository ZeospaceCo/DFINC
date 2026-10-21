using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Code
{
    public partial class AddCategory : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string CodeID;

        public int CurrentPage = 0;
        public string RecordPerPage = string.Empty; //RecordPerPage
        public string SearchField = string.Empty; //SearchField
        public string SearchValue = string.Empty; //SearchValue        
        public string Order = string.Empty; //Order
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Code.AddCategory).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {   
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            ddlActive.Items.Clear();
            ddlActive.Items.Add(new ListItem(resMng.GetString("Yes"), "1"));
            ddlActive.Items.Add(new ListItem(resMng.GetString("No"), "0"));
            
            if (CodeID != "") LoadSysCode();
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CodeID = GetDecryptParam("codeID", "");

            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "codeorder asc" : Order;
        }       


        /// <summary>
        /// 코드 정보 가져오기
        /// </summary>
        private void LoadSysCode()
        {
            Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
            List<Zeospace.DeskPlus.Common.BE.SYSCodeInfo> al = dal.GetSysCodeByCodeID(CurrentUserInfo.CompanyID, CodeID);

            foreach (Zeospace.DeskPlus.Common.BE.SYSCodeInfo info in al)
            {
                ctbCodeID.Text = info.CodeID;
                ctbCodeName.Text = info.NameBase.ToReverseXSS();
                ctbMultiLang.Text = this.GetMuiltiLang(
                            info.NameKo.ToReverseXSS(), info.NameEn.ToReverseXSS(), info.NameJa.ToReverseXSS(),
                            info.NameFr.ToReverseXSS(), info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS());

                ctbDescription.Text = info.Description.ToReverseXSS();
                ctbInterfaceCode.Text = info.InterfaceCode.ToReverseXSS();
                ddlActive.SelectedValue = info.ActiveCode;
                SYSCodeID = info.SYSCodeID;
                ParentCodeID = info.ParentCodeID;
            }
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnCheckMultiValue.Text = resMng.GetString("CheckMultiValue");
                cbtnAdd.Text = (CodeID == "") ? resMng.GetString("Save") : resMng.GetString("Save");
                cbtnCheckMultiValue.Visible = (CodeID == "") ? true : false;
            }
        }

        /// <summary>
        /// 추가 및 수정
        /// </summary>
        private int Save()
        {
            Zeospace.DeskPlus.Common.BE.SYSCodeInfo be = new DeskPlus.Common.BE.SYSCodeInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            
            be.NameBase = ctbCodeName.Text.Trim();
            be.InterfaceCode = ctbInterfaceCode.Text.Trim();
            be.ParentCodeID = "0";

            string multiName = ctbMultiLang.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.NameKo = multiName.Split(',')[0];
                be.NameEn = multiName.Split(',')[1];
                be.NameJa = multiName.Split(',')[2];
                be.NameFr = multiName.Split(',')[3];
                be.NameZhcn = multiName.Split(',')[4];
                be.NameZhtw = multiName.Split(',')[5];
            }

            be.ActiveCode = ddlActive.SelectedValue;
            be.Description = ctbDescription.Text.Trim();
            be.WriteUserID = CurrentUserInfo.UserID;

            int response = 0;
            Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
            if (CodeID != "")
            {
                //수정
                be.CodeID = CodeID;
                be.ParentCodeID = ParentCodeID;
                dal.Mod(be);
                response = SYSCodeID;
            }
            else
            {
                //추가
                be.CodeID = ctbCodeID.Text.Trim();
                response = dal.Add(be);
            }

            string dqName = "SysCode" + CurrentUserInfo.CompanyID.ToString();//applicaton name
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application[dqName] = null;
            HttpContext.Current.Application.UnLock();

            return response;
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();
                Save();


                Dictionary<string, string> paraDic = new Dictionary<string, string>();                
                paraDic.Add("sysCodeID", SYSCodeID.ToString());
                string url = @"parent.frmLeft.location='{0}';";
                string encParams = GetEncryptParam(Helpers.WebComponent.Utility.GetParameterDic(paraDic));
                url = string.Format(url, "/DeskplusEIP/EIP/Admin/Code/Tree.aspx?p=" + encParams);
                Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("SUCCESS"), url);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }

        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// SYSCodeID 아이디
        /// </summary>
        /// <value>
        /// The SYS code ID.
        /// </value>
        public int SYSCodeID
        {
            get { return GetViewStateString("SYSCodeID", "0").ToNumber<int>(); }
            set { ViewState["SYSCodeID"] = value; }
        }

        /// <summary>
        /// ParentCodeID
        /// </summary>
        public string ParentCodeID
        {
            get { return GetViewStateString("ParentCodeID", ""); }
            set { ViewState["ParentCodeID"] = value; }
        }

        #endregion
    }
}