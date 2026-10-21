using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using Zeospace.DeskPlus.EIP.BE;
using System.Drawing;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portlet
{
    public partial class Mod : Base.BasePageEIPAdmin
    {
        #region 변수설정
        public int CategoryID;
        public int PortletID;
        public string PhotoName = "";        

        public int CurrentPage = 0;
        public string RecordPerPage = string.Empty; //RecordPerPage
        public string SearchField = string.Empty; //SearchField
        public string SearchValue = string.Empty; //SearchValue        
        public string Order = string.Empty; //Order
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Portlet.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "UIType");

            //UI 형식 (T : 템플릿/ L: 내부페이지Layer/ F : IFrame)
            ddlUIType.Items.Clear();
            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang).ToReverseXSS();

                ddlUIType.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }

            dv = null;
            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "UITemplate");

            //UI 템플릿 형식 ( L : 리스트 / B : 블러그 / A : 앨범)
            ddlUITemplate.Items.Clear();
            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlUITemplate.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }

            dv = null;
            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TargetDataType");

            //Data 호출 형식 (R : RSS 호출 / S : SQL Storeprocedure Name)
            ddlDataType.Items.Clear();
            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlDataType.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }

            dv = null;
            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "LinkType");

            //More Link 형식 (P : 팝업창 /  N : 새창 / C : 하단 프레임)
            ddlLinkType.Items.Clear();
            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlLinkType.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }

            //팝업/새창일 경우 Width|Height 값 저장 ex) 500|300
            ddlPopupWidth.Items.Clear();
            ddlPopupWidth.Items.Add(new ListItem(resMng.GetString("Width"), "1"));
            ddlPopupWidth.Items.Add(new ListItem(resMng.GetString("WidthAuto"), "0"));

            ddlPopupHeight.Items.Clear();
            ddlPopupHeight.Items.Add(new ListItem(resMng.GetString("Height"), "1"));
            ddlPopupHeight.Items.Add(new ListItem(resMng.GetString("HeightAuto"), "0"));

            //개인화허용유무(1 : 허용 / 0 : 불가)
            ddlPersonal.Items.Clear();
            ddlPersonal.Items.Add(new ListItem(resMng.GetString("Allow"), "1"));
            ddlPersonal.Items.Add(new ListItem(resMng.GetString("NotAllow"), "0"));

            //활성화 유무(1 : 활성 , 0 : 비활성)
            ddlActive.Items.Clear();
            ddlActive.Items.Add(new ListItem(resMng.GetString("Active_Short"), "1"));
            ddlActive.Items.Add(new ListItem(resMng.GetString("Inactive_Short"), "0"));

            GetPortlet();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성자 : 심재운
        /// 포틀릿 정보 가져오기
        /// </summary>
        private void GetPortlet()
        {
            DAL.Portlet dal = new DAL.Portlet();
            List<BE.PortletInfo> al = dal.GetPortlet(CurrentUserInfo.CompanyID, CategoryID, PortletID);

            foreach (PortletInfo info in al)
            {
                ltlCategoryName.Text = this.GetLangName(
                                        info.CatNameBase.ToString(),
                                        info.CatNameKo.ToString(),
                                        info.CatNameEn.ToString(),
                                        info.CatNameJa.ToString(),
                                        info.CatNameFr.ToString(),
                                        info.CatNameZhcn.ToString(),
                                        info.CatNameZhtw.ToString(), PreLang);

                ctbPortletName.Text = info.NameBase.ToReverseXSS();

                ctbMultiLang.Text = GetMuiltiLang(
                        info.NameKo.ToString().ToReverseXSS(),
                        info.NameEn.ToString().ToReverseXSS(),
                        info.NameJa.ToString().ToReverseXSS(),
                        info.NameFr.ToString().ToReverseXSS(),
                        info.NameZhcn.ToString().ToReverseXSS(),
                        info.NameZhtw.ToString().ToReverseXSS());

                ctbDescription.Text = info.Description.ToReverseXSS();

                ctbPortletCode.Text = info.PortletCode.ToReverseXSS();

                tbPhotoOLD.Text = info.PhotoName; //사진이미지
                //PhotoName = info.PhotoName;

                PhotoName = "";
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "");
                string thumImgPath = this.ImgPath + "/Icon/rss.gif";

                if (info.PhotoName.Length > 0)
                {
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    thumImgPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/EIP/Portlet/" + info.PhotoName.Split('.')[0] + "_S." + info.PhotoName.Split('.')[1];
                }

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", thumImgPath);
                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                PhotoName = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                
                ddlDataType.SelectedValue = info.DataType;
                ctnDataURL.Text = info.DataURL;
                ddlLinkType.SelectedValue = info.LinkType;
                ctbLinkURL.Text = info.LinkURL;
                //ddlPopupWidth.SelectedValue = info.LinkPopupSize;
                //ddlPopupHeight.SelectedValue = info.LinkPopupSize;
                ddlPersonal.SelectedValue = info.IsPesonalization;
                ddlActive.SelectedValue = info.ActivePortlet;
                ddlSendMethod.SelectedValue = info.SendMethod;

                ddlUIType.SelectedValue = info.UIType;
                //탬플릿 경우
                if (info.UIType == "T")
                {
                    ctbUIURL.Text = "";
                    //UI 템플릿 형식 ( L : 리스트 / B : 블러그 / A : 앨범)
                    ddlUITemplate.SelectedValue = info.UITemplate;
                }
                else
                {
                    ctbUIURL.Text = info.UIURL;
                }

                //link 설정이 하단프레임 일 경우
                if (info.LinkType == "C")
                {
                    ctbPopupWidth.Text = "";
                    ctbPopupHeight.Text = "";
                    ddlPopupWidth.SelectedValue = "0";
                    ddlPopupHeight.SelectedValue = "0";                    
                }
                else
                {
                    if(info.LinkPopupSize.Length > 0)
                    {
                        ddlPopupWidth.SelectedValue = "1";//가로
                        ddlPopupHeight.SelectedValue = "1";//세로

                        string[] sizeStr = info.LinkPopupSize.Split('|');
                        if (sizeStr[0] == "")
                        {
                            ctbPopupWidth.Text = "";//가로 빈값
                            ddlPopupWidth.SelectedValue = "0";//가로 자동
                        }
                        else
                        {
                            ctbPopupWidth.Text = sizeStr[0]; //가로 입력
                            ddlPopupWidth.SelectedValue = "1";//가로
                        }

                        if (sizeStr[1] == "")
                        {
                            ctbPopupHeight.Text = "";//세로 빈값
                            ddlPopupHeight.SelectedValue = "0";//세로자동
                        }
                        else
                        {
                            ctbPopupHeight.Text = sizeStr[0];//세로 입력
                            ddlPopupHeight.SelectedValue = "1";//세로
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            PortletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnPreView.Text = resMng.GetString("Preview");
                cbtnMod.Text = resMng.GetString("Save");
                cbtnMove.Text = resMng.GetString("ChangeGroup");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnDataCheck.Text = resMng.GetString("DataCheck");
                cbtnLinkURLCheck.Text = resMng.GetString("LinkURLCheck");
                cbtnSwfFileAdd.Text = resMng.GetString("AddImg_120_60");
                cbtnADDParam.Text = resMng.GetString("AddParameter");
                cbtnADDAuth.Text = resMng.GetString("AddAuth");
                cbtnAddIP.Text = resMng.GetString("AddIP");
            }
        }




        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {

            try
            {
                LoadParam();

                BE.PortletInfo info = new BE.PortletInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = CategoryID;
                info.PortletID = PortletID;
                info.NameBase = ctbPortletName.Text.Trim();

                string multiName = ctbMultiLang.Text.Trim();
                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.Description = ctbDescription.Text.Trim();

                #region 이미지 추가
                info.PhotoName = tbPhotoOLD.Text;
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                if (tbPhotoNEW.Text.Length > 0)
                {
                    #region SPOOL 파일을 ADDRESS 폴더에 복사하기
                    string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                    souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, tbPhotoNEW.Text);                    

                    string destFile = baseFileDirectory + "{0}/EIP/Portlet/{1}";
                    string[] fileStr = tbPhotoNEW.Text.Split('.');
                    destFile = string.Format(destFile, CurrentUserInfo.CompanyID, SwfUploadGUID + "." + fileStr[1]);

                    System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFile));
                    info.PhotoName = SwfUploadGUID + "." + fileStr[1];
                    #endregion

                    #region 섬네일 이미지

                    //포틀릿 폴더 생성하기
                    FileHelper.CreateSubFolder(baseFileDirectory + CurrentUserInfo.CompanyID + "/EIP/Portlet/" + SwfUploadGUID);

                    System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(souceFile));
                    //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                    Size thumbnailSize = new Size(120, 60);

                    System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                    destFile = baseFileDirectory + "{0}/EIP/Portlet/{1}";
                    destFile = string.Format(destFile, CurrentUserInfo.CompanyID, SwfUploadGUID + "_S." + fileStr[1]);
                    thumbnail.Save(Server.MapPath(destFile));
                    #endregion
                }

                #endregion

                //UI 형식 (T : 템플릿/ L: 내부페이지Layer/ F : IFrame)
                info.UIType = ddlUIType.SelectedValue;

                //포틀릿종류 ( T : 탭포틀릿 / P : 일반 포틀릿)
                info.PortletType = "P";

                //탬플릿 경우
                if (info.UIType == "T")
                {
                    info.UIURL = "";
                    //UI 템플릿 형식 ( L : 리스트 / B : 블러그 / A : 앨범)
                    info.UITemplate = ddlUITemplate.SelectedValue;
                    switch (info.UITemplate)
                    {
                        case "L":
                            info.UIURL = "/DeskPlusEIP/Portlet/Template/List.ascx";
                            break;
                        case "B":
                            info.UIURL = "/DeskPlusEIP/Portlet/Template/Blog.ascx";
                            break;
                        case "A":
                            info.UIURL = "/DeskPlusEIP/Portlet/Template/Album.ascx";
                            break;
                    }
                }
                else
                {
                    info.UIURL = ctbUIURL.Text.Trim();
                    info.UITemplate = "";
                }

                //Data 호출 형식 (R : RSS 호출 / S : SQL Storeprocedure Name)
                info.DataType = ddlDataType.SelectedValue;
                info.DataURL = ctnDataURL.Text.Trim();

                //More Link 형식 (P : 팝업창 /  N : 새창 / C : 하단 프레임)
                info.LinkType = ddlLinkType.SelectedValue;

                //link 설정이 하단프레임 일 경우
                if (info.LinkType == "C")
                {
                    info.LinkPopupSize = "";
                }
                else
                {
                    if (ddlPopupWidth.SelectedValue == "0") ctbPopupWidth.Text = "";
                    if (ddlPopupHeight.SelectedValue == "0") ctbPopupHeight.Text = "";

                    //팝업/새창일 경우 Width|Height 값 저장 ex) 500|300
                    info.LinkPopupSize = ctbPopupWidth.Text.Trim() + "|" + ctbPopupHeight.Text.Trim();
                }

                info.LinkURL = ctbLinkURL.Text.Trim();
                //개인화허용유무(1 : 허용 / 0 : 불가)
                info.IsPesonalization = ddlPersonal.SelectedValue;
                info.ActivePortlet = ddlActive.SelectedValue;
                info.SendMethod = ddlSendMethod.SelectedValue;
                info.WriteUserID = CurrentUserInfo.UserID;
                info.IsAdmin = "1";//관리자
                info.PortletCode = ctbPortletCode.Text.Trim();

                DAL.Portlet dal = new DAL.Portlet();                

                if (dal.Mod(info))
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();

                    paraDic.Add("categoryID", CategoryID.ToString());                    
                    paraDic.Add("currentPage", CurrentPage.ToString());
                    paraDic.Add("recordPerPage", RecordPerPage.ToString());
                    paraDic.Add("order", Order);
                    paraDic.Add("alertMsg", GetResource("GoList"));  //상세수정페이지로 이동합니다.
                    paraDic.Add("goURL", "/DeskPlusEIP/EIP/Admin/Portlet/List.aspx");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        } 

        #endregion
    }
}