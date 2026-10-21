using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Code
{
    public partial class Add : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string CodeID;
        public int SysCodeID; //syscodeid값
        public string ParentCodeID;

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Code.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();

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
            if (ParentCodeID != "0") LoadSysCodeParent();
            if (CodeID != "") LoadSysCode();
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "codeorder asc" : Order;

            CodeID = GetDecryptParam("codeID", "");
            ParentCodeID = GetDecryptParam("parentCodeID", "");
            SysCodeID = GetDecryptParam("sysCodeID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 시스템 코드 정보가져오기
        /// </summary>
        private void LoadSysCodeParent()
        {
            Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
            List<Zeospace.DeskPlus.Common.BE.SYSCodeInfo> al = dal.GetSysCodeByCodeID(CurrentUserInfo.CompanyID, ParentCodeID);

            foreach (Zeospace.DeskPlus.Common.BE.SYSCodeInfo info in al)
            {
                ltlParentCodeName.Text = this.GetLangName(                    
                            info.NameBase,
                            info.NameKo,info.NameEn, info.NameJa,
                            info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

                ltlParentCodeDescription.Text = info.Description;
            }
        }


        /// <summary>
        /// 코드 정보 가져오기
        /// </summary>
        private void LoadSysCode()
        {
            Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
            List<Zeospace.DeskPlus.Common.BE.SYSCodeInfo> al = dal.GetSysCode(CurrentUserInfo.CompanyID, SysCodeID);

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
                //cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnClose.Text = resMng.GetString("Cancel"); //목록

                if (String.IsNullOrEmpty(CodeID))
                {
                    cbtnAdd.Text = resMng.GetString("Save"); //추가
                }
                else
                {
                    cbtnAdd.Text = resMng.GetString("Save"); //수정
                }

                ddlActive.Items.Clear();
                ddlActive.Items.Add(new ListItem(resMng.GetString("Yes"), "1"));
                ddlActive.Items.Add(new ListItem(resMng.GetString("No"), "0"));
            }
        }

        /// <summary>
        /// 추가 및 수정
        /// </summary>
        private void Save()
        {
            Zeospace.DeskPlus.Common.BE.SYSCodeInfo be = new DeskPlus.Common.BE.SYSCodeInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ParentCodeID = ParentCodeID;
            be.NameBase = ctbCodeName.Text.Trim();
            be.InterfaceCode = ctbInterfaceCode.Text.Trim();            

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


            Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
            if (CodeID != "")
            {
                //수정
                be.CodeID = CodeID;
                dal.Mod(be);
            }
            else
            {
                //추가
                be.CodeID = ctbCodeID.Text.Trim();
                dal.Add(be);
            }

            string dqName = "SysCode" + CurrentUserInfo.CompanyID.ToString();//applicaton name
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application[dqName] = null;
            HttpContext.Current.Application.UnLock();
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
                paraDic.Add("parentCodeID", ParentCodeID);
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/EIP/Admin/Code/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}