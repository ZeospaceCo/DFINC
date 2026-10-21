using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Drawing;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portlet
{
    public partial class AddTab : Base.BasePageEIPAdmin
    {
        #region 변수설정
        public int CategoryID;        

        public int CurrentPage = 0;
        public string RecordPerPage = string.Empty; //RecordPerPage
        public string SearchField = string.Empty; //SearchField
        public string SearchValue = string.Empty; //SearchValue        
        public string Order = string.Empty; //Order
        public string PortletCode = string.Empty;
        
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddTab).Assembly;	//어셈블리 선언
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
            GetCategory();

            //활성화 유무(1 : 활성 , 0 : 비활성)
            ddlActive.Items.Clear();
            ddlActive.Items.Add(new ListItem(resMng.GetString("Active_Short"), "1"));
            ddlActive.Items.Add(new ListItem(resMng.GetString("Inactive_Short"), "0"));
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
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
                cbtnAdd.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnSwfFileAdd.Text = resMng.GetString("AddImg_120_60");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 카테고리정보
        /// </summary>
        private void GetCategory()
        {
            if (CategoryID > 0)
            {
                DAL.PortletCategory dal = new DAL.PortletCategory();
                DataSet ds = dal.GetPortletCategory(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID, true, true);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        #region 전체경로

                        string name = this.GetLangName(
                                dr["PathNameBase"].ToString(),
                                dr["PathNameKo"].ToString(),
                                dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                                dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(),
                                dr["PathNameZhTw"].ToString(), PreLang);

                        switch (PreLang)
                        {
                            case "ko":
                                name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "en":
                                name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "ja":
                                name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "fr":
                                name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "zhcn":
                                name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "zhtw":
                                name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                        }

                        ltlHeader.Text = name.Substring(1, name.Length - 1);

                        #endregion
                    }
                }
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

                //포틀릿종류 ( T : 탭포틀릿 / P : 일반 포틀릿)
                info.PortletType = "T";
                info.UIURL = ctbUIURL.Text.Trim();                
                info.ActivePortlet = ddlActive.SelectedValue;
                info.WriteUserID = CurrentUserInfo.UserID;
                info.IsAdmin = "1";//관리자
                info.Description = ctbDescription.Text.Trim();
                info.PortletCode = ctbPortletCode.Text.Trim();

                #region 이미지 추가

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

                DAL.Portlet dal = new DAL.Portlet();
                int portletID = dal.Add(info);

                if (portletID > 0)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();

                    paraDic.Add("categoryID", CategoryID.ToString());
                    paraDic.Add("portletID", portletID.ToString());
                    paraDic.Add("currentPage", CurrentPage.ToString());
                    paraDic.Add("recordPerPage", RecordPerPage.ToString());
                    paraDic.Add("order", Order);
                    paraDic.Add("alertMsg", GetResource("GoModifyList"));  //상세수정페이지로 이동합니다.
                    paraDic.Add("goURL", "/DeskPlusEIP/EIP/Admin/Portlet/ModTab.aspx");

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