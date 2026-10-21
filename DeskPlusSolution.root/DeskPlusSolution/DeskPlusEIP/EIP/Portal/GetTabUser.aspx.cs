using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Portal
{
    public partial class GetTabUser : Base.BasePageEIP
    {
        #region 변수설정
        private string eventArgs = String.Empty;
        
        public string TabList = "";
        public string ExistTab = "0";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Portal.GetTabUser).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {

            int firstTab = GetTabList();
            TabID = (TabID == 0) ? firstTab : TabID;
            GetTabContent();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            TabID = GetDecryptParam("tabID", "0").ToNumber<int>();
        }


        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnOrderTab.Text = resMng.GetString("TabOrder");
                cbtnAddTab.Text = resMng.GetString("AddTab");
                cbtnConfirm.Text = resMng.GetString("SaveTab");
                cbtnDelTab.Text = resMng.GetString("DelTab");
                cbtnClose.Text = resMng.GetString("Closed");
                
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

                imglayout1.Src = BaseImgPath + "/Layout/1.gif";
                imglayout2.Src = BaseImgPath + "/Layout/2.gif";
                imglayout3.Src = BaseImgPath + "/Layout/3.gif";
                imglayout4.Src = BaseImgPath + "/Layout/4.gif";
                imglayout5.Src = BaseImgPath + "/Layout/5.gif";
                imglayout6.Src = BaseImgPath + "/Layout/6.gif";
                imglayout7.Src = BaseImgPath + "/Layout/7.gif";
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 탭 해더 리스트
        /// </summary>
        private int GetTabList()
        {
            DAL.Tab dal = new DAL.Tab();
            List<TabInfo> al = dal.GetTabList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID , false);

            StringBuilder sb = new StringBuilder();
            string name = string.Empty;
            int i = 1;
            int firstTabID = 0;

            if (al.Count > 0) ExistTab = "1";

            foreach (TabInfo info in al)
            {
                sb.Append(string.Format(@"<li><a alt='' title='' onclick='GetTabUser.GoTab({0})' ", info.TabID));

                if (TabID == 0 && i == 1)
                {
                    sb.Append(@" class=""tab_on""");
                    firstTabID = info.TabID;
                }

                if (TabID == info.TabID) sb.Append(@" class=""tab_on""");

                sb.Append("><span>");

                #region 이름 다국어 설정
                name = this.GetLangName(
                        info.NameBase.ToString().ToNull(), info.NameKo.ToString().ToNull(),
                        info.NameEn.ToString().ToNull(), info.NameJa.ToString().ToNull(),
                        info.NameFr.ToString().ToNull(), info.NameZhcn.ToString().ToNull(),
                        info.NameZhtw.ToString().ToNull(), PreLang);
                #endregion

                sb.Append(name);
                sb.Append("</span></a></li>");

                i += 1;
            }

            TabList = sb.ToString();

            return firstTabID;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 탭 내용
        /// </summary>
        /// <returns></returns>
        private void GetTabContent()
        {


            DAL.Tab dal = new DAL.Tab();
            List<TabInfo> al = dal.GetTab(CurrentUserInfo.CompanyID, TabID, false);

            foreach (TabInfo info in al)
            {
                ctbBaseName.Text = info.NameBase.ToReverseXSS();
                ctbMultiName.Text = GetMuiltiLang(
                        info.NameKo.ToString().ToReverseXSS(),
                        info.NameEn.ToString().ToReverseXSS(),
                        info.NameJa.ToString().ToReverseXSS(),
                        info.NameFr.ToString().ToReverseXSS(),
                        info.NameZhcn.ToString().ToReverseXSS(),
                        info.NameZhtw.ToString().ToReverseXSS());
                
                ddlActive.SelectedValue = info.ActiveTab;

                switch (info.TabLayout)
                {
                    case 1:
                        rbLayout1.Checked = true;
                        imglayout1.Src = BaseImgPath + "/Layout/1_on.gif";
                        break;
                    case 2:
                        rbLayout2.Checked = true;
                        imglayout2.Src = BaseImgPath + "/Layout/2_on.gif";
                        break;
                    case 3:
                        rbLayout3.Checked = true;
                        imglayout3.Src = BaseImgPath + "/Layout/3_on.gif";
                        break;
                    case 4:
                        rbLayout4.Checked = true;
                        imglayout4.Src = BaseImgPath + "/Layout/4_on.gif";
                        break;
                    case 5:
                        rbLayout5.Checked = true;
                        imglayout5.Src = BaseImgPath + "/Layout/5_on.gif";
                        break;
                    case 6:
                        rbLayout6.Checked = true;
                        imglayout6.Src = BaseImgPath + "/Layout/6_on.gif";
                        break;
                    case 7:
                        rbLayout7.Checked = true;
                        imglayout7.Src = BaseImgPath + "/Layout/7_on.gif";
                        break;
                }
            }

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        private void Del()
        {
            DAL.Tab dal = new DAL.Tab();
            bool response = dal.Del(CurrentUserInfo.CompanyID, TabID);
            if (response)
            {
                string url = @"location='{0}';";
                url = string.Format(url, "GetTabUser.aspx");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("SuccessDel"), url);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail",
                            GetResource("ProcessingError"), "");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            //LoadParam();

            string args = eventArgs;
            switch (args)
            {
                case "del":  //삭제
                    Del();
                    break;
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

        #region Property (프로퍼티)

        /// <summary>
        /// 선택한 tabID
        /// </summary>
        public int TabID
        {
            get
            {
                return GetViewStateInt("TabID", "");
            }
            set
            {
                ViewState["TabID"] = value;
            }
        }

        #endregion
    }
}