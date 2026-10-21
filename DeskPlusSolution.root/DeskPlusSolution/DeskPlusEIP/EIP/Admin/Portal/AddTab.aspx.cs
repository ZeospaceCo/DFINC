using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portal
{
    public partial class AddTab : Zeospace.DeskPlus.EIP.Web.Base.BasePageEIPAdmin
    {
        #region 변수설정

        

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Portal.AddTab).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
                cbtnList.Text = resMng.GetString("TabSetting");
                cbtnOrderTab.Text = resMng.GetString("TabOrder");           
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Closed");

                ddlIsModify.Items.Clear();
                ddlIsModify.Items.Add(new ListItem(resMng.GetString("CanNotUse"), "0"));
                ddlIsModify.Items.Add(new ListItem(resMng.GetString("CanUse"), "1"));

                ddlActive.Items.Clear();
                ddlActive.Items.Add(new ListItem(resMng.GetString("Active"), "1"));
                ddlActive.Items.Add(new ListItem(resMng.GetString("Inactive"), "0"));

                imglayout1.Alt = resMng.GetString("Col_1");
                imglayout2.Alt = resMng.GetString("Col_2_Share");
                imglayout3.Alt = resMng.GetString("Col_2_Left");
                imglayout4.Alt = resMng.GetString("Col_2_Right");
                imglayout5.Alt = resMng.GetString("Col_3_Center");
                imglayout6.Alt = resMng.GetString("Col_3_Share");
                imglayout7.Alt = resMng.GetString("Col_4_Share");
            }

            imglayout1.Src = BaseImgPath + "/Layout/1.gif";
            imglayout2.Src = BaseImgPath + "/Layout/2.gif";
            imglayout3.Src = BaseImgPath + "/Layout/3.gif";
            imglayout4.Src = BaseImgPath + "/Layout/4.gif";
            imglayout5.Src = BaseImgPath + "/Layout/5.gif";
            imglayout6.Src = BaseImgPath + "/Layout/6.gif";
            imglayout7.Src = BaseImgPath + "/Layout/7.gif";
        }


        

        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 추가
        /// 작성자     : shimpark
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string name = ctbBaseName.Text.Trim();

                if (name.Length > 0)
                {
                    BE.TabInfo info = new BE.TabInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.NameBase = name;

                    string multiName = this.ctbMultiName.Text.Trim();
                    if (multiName != "" && multiName.IndexOf(",") != -1)
                    {
                        info.NameKo = multiName.Split(',')[0];
                        info.NameEn = multiName.Split(',')[1];
                        info.NameJa = multiName.Split(',')[2];
                        info.NameFr = multiName.Split(',')[3];
                        info.NameZhcn = multiName.Split(',')[4];
                        info.NameZhtw = multiName.Split(',')[5];
                    }

                    info.TabOrder = 0;

                    if (rbLayout1.Checked) info.TabLayout = 1;
                    if (rbLayout2.Checked) info.TabLayout = 2;
                    if (rbLayout3.Checked) info.TabLayout = 3;
                    if (rbLayout4.Checked) info.TabLayout = 4;
                    if (rbLayout5.Checked) info.TabLayout = 5;
                    if (rbLayout6.Checked) info.TabLayout = 6;
                    if (rbLayout7.Checked) info.TabLayout = 7;

                    info.ActiveTab = ddlActive.SelectedValue;
                    info.IsModify = ddlIsModify.SelectedValue;
                    info.IsAdmin = "1"; //관리자
                    info.WriteUserID = CurrentUserInfo.UserID;

                    DAL.Tab dal = new DAL.Tab();
                    int tabID = dal.Add(info);

                    if (tabID != -1)
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("tabID", tabID.ToString());
                        string url = @"location='{0}';";                        
                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        url = string.Format(url, "GetTab.aspx?p=" + encParams);
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("SUCCESS"), url);
                    }
                    else
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", 
                            GetResource("ProcessingError"), "");
                    }                   
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }

        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}