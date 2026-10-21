using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text.RegularExpressions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.Text;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.Web.Template
{
    public partial class View : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)        
        public string PageNo;
        public string PageListUrl;
        public int TemplateID;
        private string eventArgs = String.Empty;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Template.View).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }

            #region 버튼 이미지

            //cbsWrite.Icon = BaseImgPath + "/Board/Button/but_write1.gif";
            //cbsEdit.Icon = BaseImgPath + "/Board/Button/but_modify.gif";
            //cbsDel.Icon = BaseImgPath + "/Board/Button/but_delete.gif";
            //cbsBack.Icon = BaseImgPath + "/Board/Button/but_list.gif";       
            #endregion

            //cbsDel.ConfirmMessage = GetResource("Private_BoardView_IsDelMSG");
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id
            UserID = CurrentUserInfo.UserID;		//유저 id
            DeptID = CurrentUserInfo.DeptID;		//부서 id			
            #endregion

            //자료 뿌려주기
            LoadDataContent();
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Writing"); //작성하기  
                cbtnCancel.Text = resMng.GetString("Cancel"); //취소 
                cbtnMod.Text = resMng.GetString("Mod"); //수정 
                cbtnDel.Text = resMng.GetString("Unit_Register_Delete"); //삭제 

                ltlName.Text = resMng.GetString("Board_List_Name");
                ltlTitle.Text = resMng.GetString("Board_List_Title");
                ltlWriteDate.Text = resMng.GetString("Board_List_DateTime");     

            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            SearchField = Server.UrlDecode(GetDecryptParam("searchField", ""));
            SearchValue = Server.UrlDecode(GetDecryptParam("searchValue", ""));
            PageNo = GetDecryptParam("pageNo", "0");
            PageListUrl = GetDecryptParam("pageListUrl", "JList.aspx");
            TemplateID = GetDecryptParam("templateID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 자료 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        private void LoadDataContent()
        {
            DAL.Template list = new Zeospace.DeskPlus.PMS.DAL.Template();

            //1. 내용 가져오기
            var alList = list.GetTemplate(CompanyID, ProjectID, TemplateID);

            foreach (var listInfo in alList)
            {

                if (listInfo != null)
                {
                    lblTitle.Text = listInfo.Subjects;



                    #region 본문내용

                    string formContent = listInfo.Contents;
                    FormContent = formContent.Replace("/DeskPlusEIP", "http://" + Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "") + "/DeskPlusEIP");

                    FormContent = FormContent.Replace("&nbsp;", "");
                    FormContent = Regex.Replace(FormContent, "<script", "<xscript", RegexOptions.IgnoreCase);
                    FormContent = Regex.Replace(FormContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
                    FormContent = Regex.Replace(FormContent, "<form", "<xform", RegexOptions.IgnoreCase);
                    FormContent = Regex.Replace(FormContent, "</form>", "</xform>", RegexOptions.IgnoreCase);

                    formContent = Server.HtmlDecode(formContent).Replace("<%", "&lt;%");
                    formContent = Regex.Replace(formContent, "<script", "<xscript", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "<form", "<xform", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "</form>", "</xform>", RegexOptions.IgnoreCase);

                    pnContent.Controls.Add(new LiteralControl(formContent));

                    #endregion

                    #region 작성일자

                    lblWriteDate.Text = listInfo.WriteDate.ToShortDateString();

                    #endregion

                    int writeUserID = listInfo.WriteUserID;

                    #region 수정버튼 활성화 여부

                    //cbsEdit.IsDisabled = (writeUserID == UserID) ? "0" : "1"; //수정모드 비활성화
                    //cbsDel.IsDisabled = (writeUserID == UserID) ? "0" : "1"; //삭제버튼 활성화	

                    #endregion


                    #region 짧은답글 비활성화

                    #endregion

                    #region 이름

                    this.hlName.NavigateUrl = "javascript:DeskPlus.Common.viewAccountInfo('" + CompanyID + "','" + writeUserID + "');";

                    string userName = this.GetLangName(
                                    listInfo.NameBase,
                                    listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                    listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang);

                    hlName.Text = GetWriteUserName(userName);

                    #endregion

                }
            }
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 작성자 이름설정하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <returns></returns>
        private string GetWriteUserName(string _writeUserName)
        {
            //return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.StringHelper.GetNameDBPreLang(PreLang, _writeUserName);
            return _writeUserName;
        }

        /// <summary>
        /// Dels this instance.
        /// </summary>
        private void Del()
        {
            LoadParam();

            BE.TemplateInfo be = new TemplateInfo();
            be.CompanyID = CompanyID;
            be.ProjectID = ProjectID;
            be.TemplateID = TemplateID;
            be.WriteUserID = UserID;

            DAL.Template bll = new Zeospace.DeskPlus.PMS.DAL.Template();

            if (bll.Del(be))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            }


            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("pageListUrl", PageListUrl.ToString());
            paraDic.Add("currentPage", "1");
            paraDic.Add("recordPerPage", RecordPerPage.ToString());
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("order", Order);

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            string url = PageListUrl + "?p=" + encparm;

            // 자바스크립트 주소 이동하기
            string scripts = @"<script type=""text/javascript"">location = '" + url + "';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);

        }
        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get
            {
                return GetViewStateInt("CompanyID", "0");
            }
            set
            {
                ViewState["CompanyID"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get
            {
                return GetViewStateInt("UserID", "0");
            }
            set
            {
                ViewState["UserID"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the dept ID.
        /// </summary>
        /// <value>The dept ID.</value>
        public int DeptID
        {
            get
            {
                return GetViewStateInt("DeptID", "0");
            }
            set
            {
                ViewState["DeptID"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the content of the form.
        /// </summary>
        /// <value>The content of the form.</value>
        public string FormContent
        {
            get
            {
                return GetViewStateString("FormContent", "");
            }
            set
            {
                ViewState["FormContent"] = value;
            }
        }




        #endregion

        #region Event Handler (이벤트 핸들러)
        

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();
                string args = eventArgs;
                switch (args)
                {
                    case "DEL":  //삭제
                        Del();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
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
    }
}
