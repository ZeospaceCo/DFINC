using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz;
using Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz.Model;

namespace Zeospace.DeskPlus.ETC.Web.Customize.User
{
    public partial class ListUser : Base.BasePageETC
    {
        #region 변수설정
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
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Customize.User.ListUser).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();

                // 비용 데이터 조회 - 카드포함
                LoadExpenseData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadExpenseData()
        {
            // 사용자 입력 비용을 조회 해온다. 
            ExpenseUserInfo info = new ExpenseUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LegacyKey = "";
            info.PONo = "";
            info.CardApprovalNo = "";
            info.Status = "";

            DalLegacy biz = new DalLegacy();
            DataTableReader dtr = biz.GetExpenseUserListUser(info);
            RepeaterEx1.DataSource = dtr;
            RepeaterEx1.DataBind();

        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
        }

        #endregion

        #region (함수)

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
            }

            ddlSearchField.Items.Clear();
            ddlSearchField.Items.Add(new ListItem("통합검색", "all"));
            ddlSearchField.Items.Add(new ListItem("Voucher No", "VoucherNo"));
            ddlSearchField.Items.Add(new ListItem("Vendor Name", "ExVendorNM"));
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            // ListID = GetDecryptParam("listID", "").ToString();
            // DocID = GetDecryptParam("docID", "0").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-14
        /// 작성목적 : 핸들러
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                //string args = eventArgs;
                //switch (args)
                //{
                //    case "AddExpense":  //비용 한줄 추가
                //        AddExpense();
                //        break;
                //    case "AddCard":  //카드 여러줄 추가 
                //        AddCard();
                //        break;
                //    case "DelItem":  //카드 여러줄 추가 
                //        DelItem();
                //        break;
                //    //case "DelAuth":  //삭제
                //    //    DelAuthTemp();
                //    //    break;
                //}
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }
        /// <summary>
        /// 작성자   : seo
        /// 작성일자 : 2011-01-09
        /// 작성목적 : 리스트 바인딩시 설정
        /// </summary>
        /// <param name="Sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void RepeaterEx_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("DdlAuthManage"));
                //HtmlInputHidden hdnAuthValue = (HtmlInputHidden)e.Item.FindControl("HdnAuthValue") as HtmlInputHidden;
                //string appDataPath = AppSettingHelper.GetAppSetting(AppSettingType.Config, "AppDataPath", "");
                //appDataPath = Server.MapPath(appDataPath + "/FolderAuthType/" + CurrentUserInfo.PreferredLanguage + ".xml");
                //List<XmlDataInfo> al = Helpers.UtilHelper.XmlLoadData(appDataPath);

                //foreach (XmlDataInfo be in al)
                //{
                //    ddl.Items.Add(new ListItem(be.KeyText, be.KeyValue));
                //}

                //ddl.SelectedValue = hdnAuthValue.Value;
            }
        }
        #endregion
    }
}