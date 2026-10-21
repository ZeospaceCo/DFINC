using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Component.QRCode;

namespace Zeospace.DeskPlus.RM.Web.Admin.Content
{
    public partial class Add : Zeospace.DeskPlus.RM.Web.Base.BasePageRMAdmin
    {
        #region 변수설정
        public int CategoryID;
        public string QRPath = String.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Admin.Content.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadFolderInfo();
        }
    
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 선택한 폴더의 정보 가져오기
        /// </summary>
        private void LoadFolderInfo()
        {
            if (CategoryID > 0)
            {
                DataSet ds = null;

                DAL.Category dal = new DAL.Category();
                ds = dal.GetFolder(CurrentUserInfo.CompanyID, CategoryID);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //string pName = this.GetLangName(dr["pNameBase"].ToString(), dr["pNameKo"].ToString(), dr["pNameEn"].ToString(), dr["pNameJa"].ToString(), dr["pNameFr"].ToString(), dr["pNameZhcn"].ToString(), dr["pNameZhtw"].ToString(), PreLang);
                    string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                    ltFolderName.Text = name;              
                }
            }            
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Submit");   //확인";
                cbtnList.Text = resMng.GetString("Cancel");         //목록";

                ddlAdminConfirm.Items.Clear();
                ddlAdminConfirm.Items.Add(new ListItem(this.resMng.GetString("No"), "0"));
                ddlAdminConfirm.Items.Add(new ListItem(this.resMng.GetString("Yes"), "1"));

                ddlQRCode.Items.Clear();
                ddlQRCode.Items.Add(new ListItem(this.resMng.GetString("No"), "0"));
                ddlQRCode.Items.Add(new ListItem(this.resMng.GetString("Yes"), "1"));

                rblIcon.Items.Clear();
                rblIcon.Items.Add(new ListItem("<i class='fa fa-building' style='font-size:2em;'></i>", "1"));
                rblIcon.Items.Add(new ListItem("<i class='fa fa-address-book' style='font-size:2em;'></i>", "2"));
                rblIcon.Items.Add(new ListItem("<i class='fa fa-calendar' style='font-size:2em;'></i>", "3"));
                rblIcon.Items.Add(new ListItem("<i class='fa fa-car' style='font-size:2em;'></i>", "4"));
                rblIcon.Items.Add(new ListItem("<i class='fa fa-comments' style='font-size:2em;'></i>", "5"));
                rblIcon.Items.Add(new ListItem("<i class='fa fa-fax' style='font-size:2em;'></i>", "6"));
                rblIcon.Items.Add(new ListItem("<i class='fa fa-desktop' style='font-size:2em;'></i>", "7"));
                rblIcon.Items.Add(new ListItem("<i class='fa fa-microphone' style='font-size:2em;'></i>", "8"));
                rblIcon.Items.Add(new ListItem("<i class='fa fa-print' style='font-size:2em;'></i>", "9"));

                /*
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon1.png'/>", "1"));
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon2.png'/>", "2"));
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon3.png'/>", "3"));
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon4.png'/>", "4"));
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon5.png'/>", "5"));
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon6.png'/>", "6"));
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon7.png'/>", "7"));
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon8.png'/>", "8"));
                rblIcon.Items.Add(new ListItem("<img alt='' src='" + this.BaseImgPath + "/RM/rs_icon9.png'/>", "9"));
                */

                rblIcon.SelectedValue = "1";
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
        }

     
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 자원 등록
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {            
            BE.ListInfo info = new BE.ListInfo();
            BLL.List bll = new BLL.List();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = CategoryID;
            info.Code = ctbCode.Text.Trim();
            info.Title = ctbName.Text.Trim();
            info.Content = ctbDesc.Text.Trim(); 
            info.Qty = ctbCount.Text.Trim().ToNumber<int>();
            info.UsableQty = (ctbReCount.Text.Trim() != "") ? ctbReCount.Text.Trim().ToNumber<int>() : 0;
            info.IconName = rblIcon.SelectedValue;
            info.Place = ctbPlace.Text.Trim();
            info.Authority = ddlAdminConfirm.SelectedValue;
            info.QRCodeUse = ddlQRCode.SelectedValue;

            if (ddlQRCode.SelectedValue == "1")
            {
                QRPath = System.Guid.NewGuid().ToString() + ".png";                
            }

            info.QRCodePath = QRPath;
            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteUserName = CurrentUserInfo.Name;
            info.WriteDateYM = FileHelper.GetYearMonth();

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            List<BE.ListUserInfo> al = new List<BE.ListUserInfo>();

            string[] arrManager = tbManager.Text.Split(';');
            for(int i=0; i< arrManager.Length; i++)
            {
                string[] arrTmpManager = arrManager[i].Split('|');

                BE.ListUserInfo userInfo = new BE.ListUserInfo();
                userInfo.CompanyID = CurrentUserInfo.CompanyID;
                userInfo.ManagerUserID = arrTmpManager[0].ToNumber<int>();
                userInfo.ManagerName = arrTmpManager[1].ToString();
                userInfo.Gubun = (arrTmpManager[0].ToString() == tbManagerOne.Text) ? "1" : "0";

                al.Add(userInfo);
            }
            
            return bll.Add(info, al);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 자원 등록
        /// </summary>
        private void Save()
        {
            int listID = Addition();

            if (listID > 0)
            {
                if (ddlQRCode.SelectedValue == "1")
                {
                    int qrSize = 2;
                    int qrVersion = 10;
                    string qrCorrection = "M";
                    string qrFileName = QRPath;

                    string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                    string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");

                    Dictionary<string, string> paraDic1 = new Dictionary<string, string>();
                    paraDic1.Add("companyID", CurrentUserInfo.CompanyID.ToString());
                    paraDic1.Add("categoryID", CategoryID.ToString());
                    paraDic1.Add("listID", listID.ToString());
                    string encParams1 = GetEncryptParam(Utility.GetParameterDic(paraDic1));
                    string qrContent = serverName + "/DeskPlusEIP/RM/Content/ViewQRCode.aspx?p=" + encParams1;

                    string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    filePath = filePath + "{0}/ResourceManager/Admin/{1}/{2}/";
                    string qrPath = string.Format(filePath, CurrentUserInfo.CompanyID, FileHelper.GetYearMonth(), listID);
                    qrPath = FileHelper.GetServerMapPath(qrPath);

                    QRCode qr = new Component.QRCode.QRCode();
                    if (qr.Create(qrSize, qrVersion, qrCorrection, qrPath, qrContent, qrFileName))
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("categoryID", CategoryID.ToString());
                        paraDic.Add("listID", listID.ToString());
                        paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                        paraDic.Add("alertMsg", GetResource("GoView"));
                        paraDic.Add("goURL", "/DeskPlusEIP/RM/Admin/Content/View.aspx");

                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                        Utility.Redirect(encParams);
                    }
                }
                else
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("categoryID", CategoryID.ToString());
                    paraDic.Add("listID", listID.ToString());
                    paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                    paraDic.Add("alertMsg", GetResource("GoView"));
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Admin/Content/View.aspx");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
            }
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
        /// 작성목적 : 확인버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            Save();
        }

        #endregion
    }
}