using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.BLL;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration
{
    public partial class ModSetting : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public int FormID = 0;
        public int ZQCount = 0;
        public int ZCCount = 0;
        public int ZPCount = 0;
        public string Contents = string.Empty; //내용
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.ModSetting).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            LoadFormInfo(FormID);
            LoadCount(FormID);
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
            FormID = GetDecryptParam("formID", "0").ToNumber<int>();
        }
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                // 버튼
                cbtnSeqOrder.Text = resMng.GetString("AppLineOrder");        //"결재라인순서";
                cbtnSeqLine.Text = resMng.GetString("SetAppLine");       //"결재라인지정";
                cbtnConfirm.Text = resMng.GetString("Btn_Save");      //"확인";
                cbtnCancel.Text = resMng.GetString("Cancel");     //"취소";
                cbtnAddReceive.Text = resMng.GetString("Receive") + resMng.GetString("Add");               //"추가";
                cbtnAddCarboncopy.Text = resMng.GetString("Carboncopy") + resMng.GetString("Add");          //"추가";

                // Items

                //1.공통/2.자사/3.연동/4.결재망
                LoadFolder();

                rblIsLegacy.Items.Clear();
                rblIsLegacy.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
                rblIsLegacy.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));

                rblIsAfterSubmitCall.Items.Clear();
                rblIsAfterSubmitCall.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
                rblIsAfterSubmitCall.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));

                rblLinked.Items.Clear();
                rblLinked.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
                rblLinked.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));

                rblLinkedType.Items.Clear();
                //rblLinkedType.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));
                rblLinkedType.Items.Add(new ListItem(resMng.GetString("SetAdministrator"), "1"));   //관리자지정(변경필요)
                rblLinkedType.Items.Add(new ListItem(resMng.GetString("User1"), "2"));  //사용자(필수)
                rblLinkedType.Items.Add(new ListItem(resMng.GetString("User2"), "3"));  //사용자(선택)

                // 관리자 지정의 경우 edms페이지 권한폴더 팝업을 띄워 준다. add seo
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("CompanyID", CurrentUserInfo.CompanyID.ToString());
                paraDic.Add("USERID", CurrentUserInfo.LogInID.ToString());
                paraDic.Add("USERPASSWORD", CurrentUserInfo.Password.ToString());
                paraDic.Add("targetURL", "/DeskplusEDM/Folder/Dialog/transEDM/GetFolder.aspx");
                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                rblLinkedType.Items[0].Attributes.Add("onclick", "ModSetting.PopEDMAuthFolder('" + encParams + "')");


                rblIsActive.Items.Clear();
                rblIsActive.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
                rblIsActive.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));

                rblIsMobile.Items.Clear();
                rblIsMobile.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
                rblIsMobile.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));

                rblIsBundle.Items.Clear();
                rblIsBundle.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
                rblIsBundle.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));

                rblIsEnforce.Items.Clear();
                rblIsEnforce.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
                rblIsEnforce.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));
            }
        }

        /// <summary>
        /// 양식정보 가져오기
        /// </summary>
        /// <param name="_formID"></param>
        private void LoadFormInfo(int _formID)
        {
            if (_formID.Equals(0)) return;

            DAL.Form fDal = new DAL.Form();
            List<FormInfo> fList = fDal.GetForm(CurrentUserInfo.CompanyID, _formID);

            foreach (FormInfo fInfo in fList)
            {
                //문서기본정보 & 양식
                ctbFormName.Text = this.GetLangName(fInfo.FormBase, fInfo.FormKo, fInfo.FormEn, fInfo.FormJa, fInfo.FormJa, fInfo.FormZhcn, fInfo.FormZhtw, PreLang);
                ctbNameCode.Text = fInfo.NameCode;
                ctbFormNameKO.Text = fInfo.FormKo;
                ctbFormNameEN.Text = fInfo.FormEn;
                ctbFormNameJA.Text = fInfo.FormJa;
                ctbFormNameZHCN.Text = fInfo.FormZhcn;
                ctbFormNameZHTW.Text = fInfo.FormZhtw;

                //대상폴더정보
                ddlType.SelectedValue = fInfo.TypeInfo;
                tbFolderID.Text = fInfo.FolderID.ToString();
                ctbFolderName.Text = this.GetLangName(fInfo.NameBase, fInfo.NameKo, fInfo.NameEn, fInfo.NameJa, fInfo.NameJa, fInfo.NameZhcn, fInfo.NameZhtw, PreLang);
                //lbFolderPath.Text = this.GetLangName(fInfo.PathNameBase, fInfo.PathNameKo, fInfo.PathNameEn, fInfo.PathNameJa, fInfo.PathNameFr, fInfo.PathNameZhcn, fInfo.PathNameZhtw, PreLang);

                //연계파일정보
                rblIsLegacy.SelectedValue = fInfo.IsLegacy;
                rblIsAfterSubmitCall.SelectedValue = fInfo.IsAfterSubmitCall;
                ctbNickName.Text = fInfo.NickName;
                ctbCustomFileName.Text = fInfo.CustomFileName;

                //EDM이관설정
                rblLinked.SelectedValue = fInfo.IsLinked;
                rblLinkedType.SelectedValue = fInfo.IsLinkedType;
                ctbIsLinkedEtc.Text = fInfo.IsLinkedEtc;

                // 수신확인
                if (fInfo.ContentHTML.ToString().IndexOf("ZQ") > 0)
                {
                    ZQCount = ZQCount + 1;
                }

                if (fInfo.ContentHTML.ToString().IndexOf("ZP") > 0)
                {
                    ZQCount = ZQCount + 1;
                    ZPCount = ZPCount + 1;
                }

                // 참조확인
                if (fInfo.ContentHTML.ToString().IndexOf("ZC") > 0)
                {
                    ZCCount = ZCCount + 1;
                }

                //기타정보
                rblIsBundle.SelectedValue = fInfo.IsBundle;
                rblIsEnforce.SelectedValue = fInfo.IsForce;
                rblIsActive.SelectedValue = fInfo.IsActive;
                rblIsMobile.SelectedValue = fInfo.IsMobile;
            }

            #region 수신참조
            DAL.FormAppointer dal = new DAL.FormAppointer();
            List<BE.FormAppointerInfo> list = dal.GetFormAppointer(CurrentUserInfo.CompanyID, _formID);

            string listAppointerKind = "";    //구분            
            string listDeptID = "";           //수신참조부서ID
            string listUserID = "";           //수신참조ID
            string listDeptName = "";         //수신참조부서명
            string listUserName = "";         //수신참조명
            string listIsDept = "";           //수신참조부서구분
            string listIsActive = "";         //사용구분

            int i = 0;

            foreach (BE.FormAppointerInfo datainfo in list)
            {
                if (i == 0)
                {
                    listAppointerKind = datainfo.AppointerKind.ToString();                    
                    listDeptID = datainfo.DeptID.ToString();
                    listUserID = datainfo.UserID.ToString();
                    listDeptName = datainfo.DeptName.ToString();
                    listUserName = datainfo.UserName.ToString();
                    listIsDept = datainfo.IsDept.ToString();
                    listIsActive = datainfo.IsActive.ToString();
                }
                else
                {
                    listAppointerKind = listAppointerKind + "," + datainfo.AppointerKind.ToString();                    
                    listDeptID = listDeptID + "," + datainfo.DeptID.ToString();
                    listUserID = listUserID + "," + datainfo.UserID.ToString();
                    listDeptName = listDeptName + "," + datainfo.DeptName.ToString();
                    listUserName = listUserName + "," + datainfo.UserName.ToString();
                    listIsDept = listIsDept + "," + datainfo.IsDept.ToString();
                    listIsActive = listIsActive + "," + datainfo.IsActive.ToString();
                }
                i = i + 1;
            }

            tbAppointerKind.Text = listAppointerKind;            
            tbDeptID.Text = listDeptID;
            tbUserID.Text = listUserID;
            tbDeptName.Text = listDeptName;
            tbUserName.Text = listUserName;
            tbIsDept.Text = listIsDept;
            tbIsActive.Text = listIsActive;
            #endregion
        }

        private void LoadCount(int _formID)
        {
            if (_formID.Equals(0)) return;

            #region 진행중인 양식 건수
            DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
            ltDocCnt.Text = dpDal.GetDocCount(CurrentUserInfo.CompanyID, _formID).ToString();
            #endregion

            #region 임시저정 건수
            DAL.Draft dfDal = new DAL.Draft();
            ltDraftCnt.Text = dfDal.GetDraftCount(CurrentUserInfo.CompanyID, _formID).ToString();
            #endregion
        }

        /// <summary>
        /// 폴더정보 가져오기
        /// </summary>
        private void LoadFolder()
        {
            ddlType.Items.Clear();

            DAL.Folder fdal = new DAL.Folder();
            List<BE.FolderInfo> list = fdal.GetFolderRoot(CurrentUserInfo.CompanyID);

            if (list.Count > 0)
            {
                List<BE.FolderInfo> listType1 = list.Where(x => x.TypeInfo.Equals("1")).ToList<BE.FolderInfo>();
                if (listType1.Count > 0)
                {
                    ddlType.Items.Add(new ListItem(resMng.GetString("FolderType1"), listType1[0].FolderID.ToString()));      //공통
                }
                List<BE.FolderInfo> listType2 = list.Where(x => x.TypeInfo.Equals("2")).ToList<BE.FolderInfo>();
                if (listType2.Count > 0)
                {
                    ddlType.Items.Add(new ListItem(resMng.GetString("FolderType2"), listType2[0].FolderID.ToString()));      //자사
                }
                List<BE.FolderInfo> listType3 = list.Where(x => x.TypeInfo.Equals("3")).ToList<BE.FolderInfo>();
                if (listType3.Count > 0)
                {
                    ddlType.Items.Add(new ListItem(resMng.GetString("FolderType3"), listType3[0].FolderID.ToString()));      //연동

                }
                List<BE.FolderInfo> listType4 = list.Where(x => x.TypeInfo.Equals("4")).ToList<BE.FolderInfo>();
                if (listType4.Count > 0)
                {
                    ddlType.Items.Add(new ListItem(resMng.GetString("FolderType4"), listType4[0].FolderID.ToString()));      //결재망
                }
            }
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <returns></returns>
        public bool Mod()
        {
            BE.FormInfo fInfo = new FormInfo();
            fInfo.CompanyID = CurrentUserInfo.CompanyID;
            fInfo.FormID = FormID;
            fInfo.FormBase = ctbFormName.Text;
            fInfo.FormKo = ctbFormNameKO.Text;
            fInfo.FormEn = ctbFormNameEN.Text;
            fInfo.FormJa = ctbFormNameJA.Text;
            fInfo.FormFr = ctbFormNameFR.Text;
            fInfo.FormZhcn = ctbFormNameZHCN.Text;
            fInfo.FormZhtw = ctbFormNameZHTW.Text;
            fInfo.NameCode = ctbNameCode.Text;
            fInfo.FolderID = (tbFolderID.Text.Equals("")) ? 0 : tbFolderID.Text.ToNumber<int>();
            fInfo.IsLegacy = rblIsLegacy.SelectedValue;
            fInfo.IsAfterSubmitCall = rblIsAfterSubmitCall.SelectedValue;
            fInfo.CustomFileName = ctbCustomFileName.Text;
            fInfo.NickName = ctbNickName.Text;
            fInfo.IsLinked = rblLinked.SelectedValue;
            fInfo.IsLinkedType = rblLinkedType.SelectedValue;
            fInfo.IsLinkedEtc = ctbIsLinkedEtc.Text;
            fInfo.IsForce = rblIsEnforce.SelectedValue;
            fInfo.IsBundle = rblIsBundle.SelectedValue;
            fInfo.IsActive = rblIsActive.SelectedValue;
            fInfo.IsMobile = rblIsMobile.SelectedValue;
            fInfo.ModifyUserID = CurrentUserInfo.UserID;

            BLL.Forms fdal = new BLL.Forms();

            string listAppointerKind = tbAppointerKind.Text;            
            string listDeptID = tbDeptID.Text;
            string listUserID = tbUserID.Text;
            string IsDept = tbIsDept.Text;
            string listIsActive = tbIsActive.Text;

            return fdal.ModEtc(fInfo, listAppointerKind, listDeptID, listUserID, IsDept, listIsActive);
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                switch (EventArgs.ToUpper())
                {
                    case "MOD":
                        bool rtnval = Mod();
                        if (rtnval)
                        {
                            Dictionary<string, string> parmDic = new Dictionary<string, string>();

                            parmDic.Add("currentPage", CurrentPage.ToString());
                            parmDic.Add("recordPerPage", RecordPerPage.ToString());
                            parmDic.Add("searchField", SearchField);
                            parmDic.Add("searchValue", SearchValue);
                            parmDic.Add("searchTerm", SearchTerm);
                            parmDic.Add("searchTermValue", SearchTermValue);
                            parmDic.Add("order", Order);
                            parmDic.Add("formID", FormID.ToString());

                            parmDic.Add("alertType", AlertMsgEnum.Alert_Success.ToString());
                            parmDic.Add("alertMsg", GetResource("GoList"));
                            parmDic.Add("goURL", "/DeskPlusEIP/Approval/Admin/FormGeneration/List.aspx");

                            string encParams = GetEncryptParam(Utility.GetParameterDic(parmDic));
                            Utility.Redirect(encParams);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", AlertMsgEnum.Alert_Error.ToString(), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리
            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}