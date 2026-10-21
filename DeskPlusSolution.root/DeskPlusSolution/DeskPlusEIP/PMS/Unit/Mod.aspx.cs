using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Unit
{
    public partial class Mod : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        public string Contents = string.Empty; //내용
        public string PageNo;
        public string PageListUrl;
        public int UnitID;
        public int TemplateID;

        public string Status = string.Empty;

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

            this.ddlTemplate.Attributes.Add("onchange", "Mod.TemplateChange()");
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
            PageNo = GetDecryptParam("pageNo", "0");
            PageListUrl = GetDecryptParam("pageListUrl", "List.aspx");
            UnitID = GetDecryptParam("unitID", "0").ToNumber<int>();
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
            if (UnitID > 0) //edit
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                {
                    //ByFreeman 20120106 X-Free 에디터 추가
                    /*
                    string strScript = String.Empty;
                    if (Request.Path.IndexOf("Mod.aspx") > 0)
                    {
                        strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.contents.value;" + "<" + "/script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                        NonActiveX = "F";
                    }
                    else
                    {   //스크립트에서 처리한다
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

                    #endregion 에디터 본문 이미지 첨부 경로
                }

                LoadEditMode();
            }
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
        /// 메서드명 :
        /// 작성목적 : 수정모드
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private void LoadEditMode()
        {
            DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();

            var alList = list.GetUnit(CurrentUserInfo.CompanyID, ProjectID, UnitID);
            foreach (var listInfo in alList)
            {
                if (listInfo != null)
                {
                    tbxTitle.Text = listInfo.Subjects.ToReverseXSS();
                    listInfo.Contents = listInfo.Contents.ToEditorXSS();

                    string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
                    string templateFooter = "</BODY></HTML>";

                    if (TemplateID > 0)
                    {
                        tbHiddenHTML_contents.Value = templateHeader + GetTemplate() + templateFooter;
                        tbHiddenMime_contents.Text = templateHeader + GetTemplate() + templateFooter;
                        Contents += GetTemplate();
                    }
                    else
                    {
                        tbHiddenHTML_contents.Value = listInfo.Contents;
                        tbHiddenMime_contents.Text = templateHeader + listInfo.Contents + templateFooter;
                        Contents += listInfo.Contents;
                    }

                    //tbxChargeDeptName.Text = listInfo.ChargeDeptID;
                    tbxCode.Text = listInfo.Code;
                    tbxExecuteMoney.Text = listInfo.ExecuteMoney.ToString();
                    ddlProcess.SelectedValue = listInfo.Process.ToString();
                    //tbxRelateDeptName.Text = listInfo.RelateDeptName;

                    ddlStatus.SelectedValue = listInfo.Status.ToString();
                    tbxWeightPercent.Text = listInfo.WeightPercent.ToString();
                    tbxPlanMoney.Text = listInfo.PlanMoney.ToString();

                    tbxEndDate.Text = (listInfo.EndDate.ToNullDT() == System.DBNull.Value) ? "" : listInfo.EndDate.ToShortDateString();
                    tbxPlanEndDate.Text = (listInfo.PlanEndDate.ToNullDT() == System.DBNull.Value) ? "" : listInfo.PlanEndDate.ToShortDateString();
                    tbxStartDate.Text = (listInfo.StartDate.ToNullDT() == System.DBNull.Value) ? "" : listInfo.StartDate.ToShortDateString();
                    tbxPlanStartDate.Text = (listInfo.PlanStartDate.ToNullDT() == System.DBNull.Value) ? "" : listInfo.PlanStartDate.ToShortDateString();

                    string deptID = "";
                    string deptName = "";
                    string deptGunun = "";

                    //관계부서(InputDiv3)
                    if (listInfo.RelateDeptID.Length > 0)
                    {
                        Zeospace.DeskPlus.Account.DAL.Dept dal = new Account.DAL.Dept();
                        var alDeptList = dal.GetDeptLists(CurrentUserInfo.CompanyID, listInfo.RelateDeptID, true);

                        foreach (var deptInfo in alDeptList)
                        {
                            deptID += deptInfo.DeptID + ",";
                            deptName += this.GetLangName(
                                    deptInfo.NameBase,
                                    deptInfo.NameKo, deptInfo.NameEn, deptInfo.NameJa,
                                    deptInfo.NameFr, deptInfo.NameZhcn, deptInfo.NameZhtw, PreLang) + ",";
                            deptGunun += "G" + ",";
                            //tmpLinkedDeptName += "G_" + deptInfo.DeptID + "_‡InputDiv3" + ",";
                        }

                        LinkedDeptName = deptID + "‡" + deptName + "‡" + deptGunun.TrimEnd(',');
                    }

                    deptID = "";
                    deptName = "";
                    deptGunun = "";

                    //담당부서(InputDiv2)
                    if (listInfo.ChargeDeptID.Length > 0)
                    {
                        Zeospace.DeskPlus.Account.DAL.Dept dal = new Account.DAL.Dept();
                        var alDeptList = dal.GetDeptLists(CurrentUserInfo.CompanyID, listInfo.ChargeDeptID, true);

                        foreach (var deptInfo in alDeptList)
                        {
                            deptID += deptInfo.DeptID + ",";
                            deptName += this.GetLangName(
                                    deptInfo.NameBase,
                                    deptInfo.NameKo, deptInfo.NameEn, deptInfo.NameJa,
                                    deptInfo.NameFr, deptInfo.NameZhcn, deptInfo.NameZhtw, PreLang) + ",";
                            deptGunun += "G" + ",";
                            //tmpLinkedDeptName += "G_" + deptInfo.DeptID + "_‡InputDiv3" + ",";
                        }

                        ChargerDeptName = deptID + "‡" + deptName + "‡" + deptGunun.TrimEnd(',');
                    }

                    deptID = "";
                    deptName = "";
                    deptGunun = "";

                    #region 책임자 정보 가져오기

                    DAL.UnitCharger charger = new Zeospace.DeskPlus.PMS.DAL.UnitCharger();
                    List<BE.UnitChargerInfo> al = charger.GetUnitCharger(CurrentUserInfo.CompanyID, ProjectID, UnitID);

                    if (al != null)
                    {
                        string ownerIDList = "";
                        string ownerNameList = "";
                        string gununList = "";

                        string chargerValue = "";

                        foreach (BE.UnitChargerInfo be in al)
                        {
                            string userName = this.GetLangName(
                                    be.NameBase,
                                    be.NameKo, be.NameEn, be.NameJa,
                                    be.NameFr, be.NameZhcn, be.NameZhtw, PreLang);

                            //chargerValue += Component.WebComponent.StringHelper.GetNameDBPreLang(PreLang, be.UserName.Trim()) + ",";
                            chargerValue += userName + ",";

                            ownerIDList += be.UserID.ToString() + ",";
                            //deptName += Component.WebComponent.StringHelper.GetNameDBPreLang(PreLang, be.UserName.Trim()) + ",";
                            ownerNameList += userName + ",";
                            gununList += "M" + ",";
                        }

                        if (al.Count > 0)
                        {
                            ChargerName = ownerIDList + "‡" + ownerNameList + "‡" + gununList.TrimEnd(',');
                        }

                        chargerValue = chargerValue.TrimEnd(',');

                        //tbxCharger.Text = chargerValue;
                    }

                    #endregion 책임자 정보 가져오기
                }

                DAL.UnitAttach attachfile = new Zeospace.DeskPlus.PMS.DAL.UnitAttach();
                LoadAttachFiles(attachfile.GetAttachFile(CurrentUserInfo.CompanyID, ProjectID, UnitID));
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 첨부파일 정보 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_attachFileList">The _attach file list.</param>
        private void LoadAttachFiles(List<BE.UnitAttachInfo> _attachFileList)
        {
            if (_attachFileList != null)
            {
                string saveFileList = "";

                foreach (BE.UnitAttachInfo attachFile in _attachFileList)
                {
                    saveFileList += "S>" + attachFile.AttachFileID + ">" + attachFile.FilePath + ">" + attachFile.FileName + ">" + attachFile.FileGUID + "|";
                }

                saveFileList = saveFileList.TrimEnd('|');
                tbHiddenFileList.Text = saveFileList;				//첨부파일 정보를 저장하며 리스트박스와 동기화
                tbHiddenSaveFileList.Text = saveFileList;			//서버 파일 삭제시 비교로 사용 될 원본 파일 리스트(FileID 포함)
            }
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
        /// 작성목적 : 수정 담기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private BE.UnitInfo EditMode()
        {
            //1. 게시판 리스트
            BE.UnitInfo listinfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();

            listinfo.CompanyID = CurrentUserInfo.CompanyID;
            listinfo.ProjectID = ProjectID;
            listinfo.UnitID = UnitID;
            listinfo.Code = tbxCode.Text;
            listinfo.Subjects = tbxTitle.Text;
            listinfo.Contents = GetContents();
            listinfo.Process = ddlProcess.SelectedValue.ToNumber<int>();
            listinfo.PlanMoney = (tbxPlanMoney.Text.Length > 0) ? tbxPlanMoney.Text.ToNumber<int>() : 0;
            listinfo.ExecuteMoney = (tbxExecuteMoney.Text.Length > 0) ? tbxExecuteMoney.Text.ToNumber<int>() : 0;
            listinfo.Status = ddlStatus.SelectedValue.ToNumber<int>();
            listinfo.WeightPercent = (tbxWeightPercent.Text.Length > 0) ? tbxWeightPercent.Text.ToNumber<int>() : 0;

            listinfo.PlanStartDate = DateHelper.GetCultureDateConvertString(Request[tbxPlanStartDate.ClientID].ToString(), "", PreLangReal);
            listinfo.PlanEndDate = DateHelper.GetCultureDateConvertString(Request[tbxPlanEndDate.ClientID].ToString(), "", PreLangReal);

            listinfo.StartDate = DateHelper.GetCultureDateConvertString(Request[tbxStartDate.ClientID].ToString(), "", PreLangReal);
            listinfo.EndDate = DateHelper.GetCultureDateConvertString(Request[tbxEndDate.ClientID].ToString(), "", PreLangReal);

            listinfo.UnitCharger = hdChargerID.Value.TrimEnd(','); //책임자코드 (unitcharger 테이블에 저장될 사용자)
            listinfo.ChargeDeptID = hdChargeDeptName.Value.Trim(','); //담당부서명
            listinfo.RelateDeptID = hdRelateDeptName.Value.TrimEnd(','); //관련부서명

            listinfo.PlanDay = GetPlanDay(listinfo.StartDate, listinfo.EndDate);
            listinfo.WriteUserID = CurrentUserInfo.UserID;

            #region 첨부파일

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
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

            if (UnitID > 0)
            {
                if (bll.Mod(EditMode()))
                {
                    paraDic.Add("projectID", ProjectID.ToString());
                    paraDic.Add("menuID", MenuID.ToString());
                    paraDic.Add("menuType", MenuType.ToString());
                    paraDic.Add("unitID", UnitID.ToString());
                    paraDic.Add("currentPage", CurrentPage.ToString());
                    paraDic.Add("recordPerPage", RecordPerPage.ToString());
                    paraDic.Add("searchField", SearchField);
                    paraDic.Add("searchValue", SearchValue);
                    paraDic.Add("searchTerm", SearchTerm);
                    paraDic.Add("searchTermValue", SearchTermValue);
                    paraDic.Add("order", Order);

                    paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                    paraDic.Add("alertMsg", GetResource("GoList"));
                    paraDic.Add("goURL", "/DeskplusEIP/PMS/Unit/View.aspx");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
            }
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}