using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.IO;
using System.Data;
using System.Globalization;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class Add : Base.BasePageEducationAdmin
    {
        #region 변수설정        
        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AccessAuth) return;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.Add).Assembly;	
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();

                #region 
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Education/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; 
                #endregion
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            LoadEditor();

            ddlItem.Items.Clear();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");

            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ActiveCode"].ToString() == "1")
                {
                    string name = this.GetLangName(dv[i]["NameBase"].ToString(), dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                                                dv[i]["NameFr"].ToString(), dv[i]["NameZhcn"].ToString(), dv[i]["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                    ddlItem.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
                }
            }

            ddlItem.SelectedValue = SearchItem;

            ddlAlarmDay.Items.Clear();
            ddlAlarmDay.Items.Add(new ListItem("미알람", "0"));
            ddlAlarmDay.Items.Add(new ListItem("1", "1"));
            ddlAlarmDay.Items.Add(new ListItem("2", "2"));
            ddlAlarmDay.Items.Add(new ListItem("3", "3"));
            ddlAlarmDay.Items.Add(new ListItem("4", "4"));
            ddlAlarmDay.Items.Add(new ListItem("5", "5"));
            ddlAlarmDay.Items.Add(new ListItem("6", "6"));
            ddlAlarmDay.Items.Add(new ListItem("7", "7"));
        }

        /// <summary>
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnList.Text = "목록";
                cbtnAdd.Text = "등록";
                cbtnWhole.Text = "전체선택";
                cbtnReceive.Text = "사용자,부서선택";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadEditor()
        {
            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";
            tbHiddenMime_contents.Text = templateHeader + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                NonActiveX = "T";
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetContents()
        {
            string contents = "";
            if (NonActiveX == "F") 
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
        /// 요청하기
        /// </summary>
        /// <returns></returns>
        private void Addition()
        {
            if (!AccessAuth) return;

            string content = GetContents();      

            BE.ListInfo be = new BE.ListInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.Title = tbxTitle.Text.Trim();
            be.Contents = content;
            be.ContentText = tbHiddenText_contents.Text.Trim();
            be.ItemID = ddlItem.SelectedValue;
            be.StartDate = DateHelper.GetCultureDate(Request[this.tbxStartDate.ClientID].ToString(), "yyyyMMdd", PreLangReal);
            be.EndDate = DateHelper.GetCultureDate(Request[this.tbxEndDate.ClientID].ToString(), "yyyyMMdd", PreLangReal);
            be.PopUpYN= (cbPopUp.Checked) ? "Y" : "N";
            be.AlarmDay = ddlAlarmDay.SelectedValue.ToNumber<int>();

            be.WriteUserID = CurrentUserInfo.UserID;
            be.ToUserList = hdReceiveID.Value.Trim();
            string allTargetYN = Request["hdAllTargetYN"] ?? Request["AllTargetYN"] ?? "N";
            be.AllTargetYN = allTargetYN.Trim().ToUpperInvariant();
            be.SenderName = CurrentUserInfo.Name;
            be.PreLang = PreLang;

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);       

            BLL.List dal = new BLL.List();
            int listID = dal.Add(be);

            if (listID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();               
                paraDic.Add("searchTarget", SearchTarget);
                paraDic.Add("searchItem", SearchItem);
                paraDic.Add("searchDateValue", SearchDateValue);
                paraDic.Add("listID", listID.ToString());
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", "보기 화면로 이동합니다.");
                paraDic.Add("goURL", "/DeskplusEIP/Education/Admin/View.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                    "Education.Admin.Add.Addition failed. listID=" + listID.ToString());
                Utility.ShowMessageBox(Page, "addFail", "처리하는데 실패했습니다.", "BtnCheck = 'N';");
            }
        }

        #endregion

        #region (속성)

        /// <summary>
        /// 업로드할 대상 경로
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
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

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {            
            try
            {
                if (!AccessAuth) return;

                string args = eventArgs;
                switch (args)
                {
                    case "ADD":  //요청하기
                        Addition();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnAdd_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(
                    Page, "fail", "처리하는데 실패했습니다.", "BtnCheck = 'N';");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceControl"></param>
        /// <param name="eventArgument"></param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}
