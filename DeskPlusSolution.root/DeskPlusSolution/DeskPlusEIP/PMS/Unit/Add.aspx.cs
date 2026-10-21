using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Unit
{
    public partial class Add : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        public string content = "";
        public string Status = string.Empty;
        public int TemplateID;

        #endregion Declaration (컨트롤 선언)

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Unit.Add).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();
            }

            #region 버튼 이미지

            this.ddlTemplate.Attributes.Add("onchange", "Add.TemplateChange()");
            ltlMenuName.Text = MenuName;

            #endregion 버튼 이미지
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadCUMode();
            LoadTemplateList();
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("Cancel");

                cbtnDeptUserTree.Text = resMng.GetString("Search");
                cbtnDeptUserTree2.Text = resMng.GetString("Search");
                cbtnDeptUserTree1.Text = resMng.GetString("Search");

                ltlChargeDeptName.Text = resMng.GetString("Unit_Register_ltlChargeDeptName");
                ltlCharger.Text = resMng.GetString("Unit_Register_ltlCharger");
                ltlCode.Text = resMng.GetString("Unit_Register_ltlCode");
                ltlExecuteMoney.Text = resMng.GetString("Unit_Register_ltlExecuteMoney");
                ltlPlanEndDate.Text = resMng.GetString("Unit_Register_ltlPlanEndDate");
                ltlPlanMoney.Text = resMng.GetString("Unit_Register_ltlPlanMoney");
                ltlPlanStartDate.Text = resMng.GetString("Unit_Register_ltlPlanStartDate");
                ltlProcess.Text = resMng.GetString("Unit_Register_ltlProcess");
                ltlRelateDeptName.Text = resMng.GetString("Unit_Register_ltlRelateDeptName");
                ltlStartDate.Text = resMng.GetString("Unit_Register_ltlStartDate");
                ltlEndDate.Text = resMng.GetString("Unit_Register_ltlEndDate");
                ltlStatus.Text = resMng.GetString("Unit_Register_ltlStatus");
                ltlTemplate.Text = resMng.GetString("Unit_Register_ltlTemplate");
                ltlTitle.Text = resMng.GetString("Unit_Register_ltlTitle");
                ltlWeightPercent.Text = resMng.GetString("Unit_Register_ltlWeightPercent");

                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status1"), "1"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status2"), "2"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status3"), "3"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status4"), "4"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status5"), "5"));
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            TemplateID = GetDecryptParam("templateID", "0").ToNumber<int>();
            Status = GetDecryptParam("status", "0");
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 템플릿
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-21
        /// </summary>
        private void LoadTemplateList()
        {
            DAL.Template bll = new Zeospace.DeskPlus.PMS.DAL.Template();
            List<BE.TemplateInfo> al = bll.GetTemplate(CurrentUserInfo.CompanyID, ProjectID);

            if (al != null)
            {
                ddlTemplate.Items.Add(new ListItem(GetResource("FormatApply"), "0"));

                foreach (BE.TemplateInfo be in al)
                {
                    ddlTemplate.Items.Add(new ListItem(be.Subjects, be.TemplateID.ToString()));
                }
            }

            ddlTemplate.SelectedValue = TemplateID.ToString();
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가, 수정, 답변시 에디터 설정하기
        /// 작성자   : 심재운
        /// </summary>
        private void LoadCUMode()
        {
            #region 템플릿 바인딩

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            if (TemplateID > 0)
                tbHiddenMime_contents.Text = templateHeader + GetTemplate() + templateFooter;
            else
                tbHiddenMime_contents.Text = templateHeader + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("Add.aspx") > 0)
                {
                    strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.HtmlValue(form1.tbHiddenMime_contents.value);</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                    NonActiveX = "F";
                }
                else
                {
                    //스크립트에서 처리한다
                    NonActiveX = "T";
                }
                */
                NonActiveX = "T";

                #region 에디터 본문 이미지 첨부 경로
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/PMS/Unit/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
                #endregion

            }

            #endregion 템플릿 바인딩
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 템플릿 적용
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private string GetTemplate()
        {
            if (TemplateID > 0)
            {
                DAL.Template bll = new Zeospace.DeskPlus.PMS.DAL.Template();
                var alList = bll.GetTemplate(CurrentUserInfo.CompanyID, ProjectID, TemplateID);

                foreach (var be in alList) return be.Contents;
            }

            return "";
        }

        /// <summary>
        /// 태그프리 에디터 설정
        /// </summary>
        /// <returns></returns>
        private string GetContents()
        {
            string contents = "";
            if (NonActiveX == "F") //activex
            {
                string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                string filePath = FileHelper.GetServerMapPath(UploadPath);
                string serverNameFolderName = serverName + UploadPath;

                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                string contentValue = tbHiddenMime_contents.Text;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass util
                    = new DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass();

                util.ProcessDecoding(contentValue, filePath, serverNameFolderName, 0);
                contents = util.DecodedHTML;
            }
            else if (NonActiveX == "T") //x-free
            {
                contents = tbHiddenHTML_contents.Value;
            }

            return contents;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 계획일수
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-22
        /// </summary>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_endDate">The _end date.</param>
        /// <returns></returns>
        private int GetPlanDay(DateTime _startDate, DateTime _endDate)
        {
            if (_startDate.ToNullDT() != System.DBNull.Value && _endDate.ToNullDT() != System.DBNull.Value)
            {
                Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();
                List<int> al = lib.GetProjcectDays(_startDate, _endDate, System.DateTime.Now);
                return al[0];
            }

            return 0;
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// 내용
        /// </summary>
        public string Contents
        {
            get
            {
                return Server.HtmlEncode(content);
            }
        }

        /// <summary>
        /// TagFree의 Mime 을 사용하여 이미지를 업로드 할지 (Y : Mime 을 사용 , N : 업로드 컴포넌트를 사용) - 원인은 중국서버에서 Mime의 이상작동
        /// </summary>
        public string MimeUseChk
        {
            get
            {
                try
                {
                    return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MimeUseChk", "N");
                }
                catch
                {
                    return "Y";
                }
            }
        }

        /// <summary>
        /// 업로드할 대상 경로
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }

            set { ViewState["UploadPath"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the charger.
        /// </summary>
        /// <value>The name of the charger.</value>
        public string ChargerName
        {
            get { return GetViewStateString("ChargerName", ""); }

            set { ViewState["ChargerName"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the linked dept.
        /// </summary>
        /// <value>The name of the linked dept.</value>
        public string LinkedDeptName
        {
            get { return GetViewStateString("LinkedDeptName", ""); }

            set { ViewState["LinkedDeptName"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the charger dept.
        /// </summary>
        /// <value>The name of the charger dept.</value>
        public string ChargerDeptName
        {
            get { return GetViewStateString("ChargerDeptName", ""); }

            set { ViewState["ChargerDeptName"] = value; }
        }

        /// <summary>
        /// ActiveX 유무
        /// </summary>
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
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
                if (MenuID == 0)
                {
                    return lib.MenuName(CurrentUserInfo.CompanyID, ProjectID, 0, 2, PreLang);
                }
                else
                {
                    return lib.MenuName(CurrentUserInfo.CompanyID, ProjectID, MenuID, PreLang);
                }
            }
        }

        #endregion Property (프로퍼티)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가, 답변 담기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private BE.UnitInfo WriteMode()
        {
            string content = GetContents();
            //string content = (EditorName == "TAGFREE") ? SetEditorFileFolder() : FCKEditor.GetFCKeditorValue();

            //1. 게시판 리스트
            BE.UnitInfo listinfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();

            listinfo.CompanyID = CurrentUserInfo.CompanyID;
            listinfo.ProjectID = ProjectID;
            listinfo.UnitID = 0;
            listinfo.Code = tbxCode.Text;
            listinfo.Subjects = tbxTitle.Text;
            listinfo.Contents = content;
            listinfo.Process = ddlProcess.SelectedValue.ToNumber<int>();
            listinfo.PlanMoney = (tbxPlanMoney.Text.Length > 0) ? tbxPlanMoney.Text.ToNumber<int>() : 0;
            listinfo.ExecuteMoney = (tbxExecuteMoney.Text.Length > 0) ? tbxExecuteMoney.Text.ToNumber<int>() : 0;
            listinfo.Status = ddlStatus.SelectedValue.ToNumber<int>();

            listinfo.UnitCharger = hdChargerID.Value.TrimEnd(','); //책임자코드 (unitcharger 테이블에 저장될 사용자)
            listinfo.ChargeDeptID = hdChargeDeptName.Value.Trim(','); //담당부서명
            listinfo.RelateDeptID = hdRelateDeptName.Value.TrimEnd(','); //관련부서명

            listinfo.PlanStartDate = DateHelper.GetCultureDateConvertString(Request[tbxPlanStartDate.ClientID].ToString(), "", PreLangReal);
            listinfo.PlanEndDate = DateHelper.GetCultureDateConvertString(Request[tbxPlanEndDate.ClientID].ToString(), "", PreLangReal);

            listinfo.StartDate = DateHelper.GetCultureDateConvertString(Request[tbxStartDate.ClientID].ToString(), "", PreLangReal);
            listinfo.EndDate = DateHelper.GetCultureDateConvertString(Request[tbxEndDate.ClientID].ToString(), "", PreLangReal);

            listinfo.PlanDay = GetPlanDay(listinfo.StartDate, listinfo.EndDate);
            listinfo.WeightPercent = (tbxWeightPercent.Text.Length > 0) ? tbxWeightPercent.Text.ToNumber<int>() : 0;

            listinfo.WriteUserID = CurrentUserInfo.UserID;

            #region 첨부파일

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            listinfo.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            #endregion 첨부파일

            return listinfo;
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가글
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void lbtnWrite_Click(object sender, System.EventArgs e)
        {
            LoadParam();

            BLL.Unit bll = new Zeospace.DeskPlus.PMS.BLL.Unit();
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            int rtnListID = bll.Add(WriteMode());

            if (rtnListID > 0)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            }

            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("rqProjectID", this.RqProjectID.ToString());
            paraDic.Add("unitID", rtnListID.ToString());
            paraDic.Add("recordPerPage", RecordPerPage.ToString());
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("order", Order);
            paraDic.Add("status", Status);

            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoList"));
            paraDic.Add("goURL", "/DeskplusEIP/PMS/Unit/View.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}