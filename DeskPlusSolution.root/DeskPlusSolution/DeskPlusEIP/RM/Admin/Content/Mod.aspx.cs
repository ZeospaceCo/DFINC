using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.RM.BE;
using Zeospace.DeskPlus.Component.QRCode;

namespace Zeospace.DeskPlus.RM.Web.Admin.Content
{
    public partial class Mod : Zeospace.DeskPlus.RM.Web.Base.BasePageRMAdmin
    {
        #region 변수설정
        public int CategoryID;
        public int ListID;
        public int CurrentPage = 0;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어
        public string SearchTerm = String.Empty;  //기간검색키워드
        public string SearchTermValue = String.Empty;  //기간검색어       
        public string Order = String.Empty; //sort       
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Admin.Content.Mod).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-02-22
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetData();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 자원 정보 가져오기
        /// </summary>
        private void GetData()
        {
            DAL.List dal = new DAL.List();

            DataTableReader dtr = dal.GetListInfo(CurrentUserInfo.CompanyID, ListID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        
                        ltFolderName.Text = this.GetLangName(dtr["CNameBase"].ToString(), dtr["CNameKo"].ToString(),
                                            dtr["CNameEn"].ToString(), dtr["CNameJa"].ToString(), dtr["CNameFr"].ToString(),
                                            dtr["CNameZhcn"].ToString(), dtr["CNameZhtw"].ToString(), PreLang);


                        ctbName.Text = dtr["Title"].ToString().ToReverseXSS();
                        ctbCount.Text = dtr["Qty"].ToString();
                        ctbCode.Text = dtr["Code"].ToString().ToReverseXSS();
                        ctbReCount.Text = dtr["UsableQty"].ToString();
                        ctbPlace.Text = dtr["Place"].ToString().ToReverseXSS();
                        ddlAdminConfirm.SelectedValue = dtr["Authority"].ToString();
                        ddlQRCode.SelectedValue = dtr["QRCodeUse"].ToString();
                        QRCheck = dtr["QRCodeUse"].ToString();
                        QRCodePath = dtr["QRCodePath"].ToString();
                        rblIcon.SelectedValue = dtr["IconName"].ToString();
                        ctbDesc.Text = dtr["Content"].ToString().ToReverseXSS();
                        TmpCategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                        WriteDateYMD = dtr["WriteDateYMD"].ToString();
                    }
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
                cbtnCancel.Text = resMng.GetString("Cancel");     //취소";

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
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "WriteDate desc" : Order;
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 자원 수정
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {

            BE.ListInfo info = new BE.ListInfo();
            BLL.List bll = new BLL.List();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = TmpCategoryID;
            info.ListID = ListID;
            info.Code = ctbCode.Text.Trim();
            info.Title = ctbName.Text.Trim();
            info.Content = ctbDesc.Text.Trim();
            info.Qty = ctbCount.Text.Trim().ToNumber<int>();
            info.UsableQty = (ctbReCount.Text.Trim() != "") ? ctbReCount.Text.Trim().ToNumber<int>() : 0;
            info.IconName = rblIcon.SelectedValue;
            info.Place = ctbPlace.Text.Trim();
            info.Authority = ddlAdminConfirm.SelectedValue;
            info.QRCodeUse = ddlQRCode.SelectedValue;

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if ((QRCheck == "0") && (ddlQRCode.SelectedValue == "1"))
            {
                QRPath = System.Guid.NewGuid().ToString() + ".png";                
                QRCodePath = filePath + "{0}/ResourceManager/Admin/{1}/{2}/{3}";
                QRCodePath = string.Format(QRCodePath, CurrentUserInfo.CompanyID, WriteDateYMD.Substring(0, 6), ListID, QRPath);
            }          

            info.QRCodePath = QRCodePath;

            info.ModifyUserID = CurrentUserInfo.UserID;
            info.ModifyUserName = CurrentUserInfo.Name;
            info.WriteDateYM = WriteDateYMD.Substring(0, 6);

            string spoolfilePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(spoolfilePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            List<BE.ListUserInfo> al = new List<BE.ListUserInfo>();

            string[] arrManager = tbManager.Text.Split(';');
            for (int i = 0; i < arrManager.Length; i++)
            {
                string[] arrTmpManager = arrManager[i].Split('|');

                BE.ListUserInfo userInfo = new BE.ListUserInfo();
                userInfo.CompanyID = CurrentUserInfo.CompanyID;
                userInfo.ListID = ListID;
                userInfo.ManagerUserID = arrTmpManager[0].ToNumber<int>();
                userInfo.ManagerName = arrTmpManager[1].ToString();
                userInfo.Gubun = (arrTmpManager[0].ToString() == tbManagerOne.Text) ? "1" : "0";

                al.Add(userInfo);
            }


            return bll.Mod(info, al);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 자원 수정
        /// </summary>
        private void ModList()
        {
            if (Modify())
            {
                if ((QRCheck == "0") && (ddlQRCode.SelectedValue == "1"))
                {
                    int qrSize = 2;
                    int qrVersion = 10;
                    string qrCorrection = "M";
                    string qrFileName = QRPath;

                    string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                    string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");

                    Dictionary<string, string> paraDic1 = new Dictionary<string, string>();
                    paraDic1.Add("companyID", CurrentUserInfo.CompanyID.ToString());
                    paraDic1.Add("categoryID", TmpCategoryID.ToString());
                    paraDic1.Add("listID", ListID.ToString());
                    string encParams1 = GetEncryptParam(Utility.GetParameterDic(paraDic1));
                    string qrContent = serverName + "/DeskPlusEIP/RM/Content/ViewQRCode.aspx?p=" + encParams1;

                    string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    filePath = filePath + "{0}/ResourceManager/Admin/{1}/{2}/";
                    string qrPath = string.Format(filePath, CurrentUserInfo.CompanyID, WriteDateYMD.Substring(0, 6), ListID);
                    qrPath = FileHelper.GetServerMapPath(qrPath);

                    QRCode qr = new Component.QRCode.QRCode();
                    if (qr.Create(qrSize, qrVersion, qrCorrection, qrPath, qrContent, qrFileName))
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("categoryID", CategoryID.ToString());
                        paraDic.Add("listID", ListID.ToString());
                        paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
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
                    paraDic.Add("listID", ListID.ToString());
                    paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
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
        /// MDeptName
        /// </summary>
        public string MDeptName
        {
            get { return GetViewStateString("MDeptName", ""); }
            set { ViewState["MDeptName"] = value; }
        }

        /// <summary>
        /// MTitleName
        /// </summary>
        public string MTitleName
        {
            get { return GetViewStateString("MTitleName", ""); }
            set { ViewState["MTitleName"] = value; }
        }

        /// <summary>
        /// MDutyName
        /// </summary>
        public string MDutyName
        {
            get { return GetViewStateString("MDutyName", ""); }
            set { ViewState["MDutyName"] = value; }
        }

        /// <summary>
        /// MName
        /// </summary>
        public string MName
        {
            get { return GetViewStateString("MName", ""); }
            set { ViewState["MName"] = value; }
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
        /// WriteDateYMD
        /// </summary>
        public string WriteDateYMD
        {
            get { return GetViewStateString("WriteDateYMD", ""); }
            set { ViewState["WriteDateYMD"] = value; }
        }

        /// <summary>
        /// TmpCategoryID
        /// </summary>
        public int TmpCategoryID
        {
            get { return GetViewStateInt("TmpCategoryID", "0"); }
            set { ViewState["TmpCategoryID"] = value; }
        }

        /// <summary>
        /// QRCheck
        /// </summary>
        public string QRCheck
        {
            get { return GetViewStateString("QRCheck", "0"); }
            set { ViewState["QRCheck"] = value; }
        }

        /// <summary>
        /// QRCodePath
        /// </summary>
        public string QRCodePath
        {
            get { return GetViewStateString("QRCodePath", "0"); }
            set { ViewState["QRCodePath"] = value; }
        } 


        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 확인버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            ModList();
        }
        #endregion
    }
}