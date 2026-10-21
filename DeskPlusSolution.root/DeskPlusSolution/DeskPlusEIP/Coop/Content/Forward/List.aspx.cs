using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.Web.Content.Forward
{
    public partial class List : Base.BasePageCoop
    {
        #region 변수설정

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Forward.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
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

            //RblStatus.SelectedValue = SearchStatus;
            ddlSearchField.SelectedValue = SearchField;
            ddlItemID.SelectedValue = ItemID.ToString();
            ddlItemSubID.SelectedValue = ItemSubID.ToString();
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = ListCount;//페이지 카운트
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
                cbtnAdd.Text = resMng.GetString("Request");

                ddlSearchField.Items[0].Text = resMng.GetString("Title");
                ddlSearchField.Items[1].Text = resMng.GetString("DocNumber");
                ddlSearchField.Items[2].Text = resMng.GetString("Second") + resMng.GetString("Group");
                ddlSearchField.Items[3].Text = resMng.GetString("RequestDate");
                ddlSearchField.Items[4].Text = resMng.GetString("StartDate");
                ddlSearchField.Items[5].Text = resMng.GetString("EndDate");
                ddlSearchField.Items[6].Text = resMng.GetString("Requester");

                cbtnSearch.Text = resMng.GetString("OrganBtnSearch");

                LoadItem();
            }
        }

        /// <summary>
        /// 검색 1,2 차 바인딩
        /// 작성자 : 심재운
        /// </summary>
        private void LoadItem()
        {
            DAL.Item itemDal = new DAL.Item();
            string itemName = string.Empty;

            // 1차 분류 값 가져오기
            var itemList = itemDal.GetItem(CurrentUserInfo.CompanyID, false);

            ddlItemID.Items.Clear();
            ddlItemID.Items.Add(new ListItem(resMng.GetString("First") + resMng.GetString("Group"), "0"));

            if (itemList != null)
            {
                foreach (var item in itemList)
                {
                    itemName = this.GetLangName(
                            item.NameBase.ToString().ToNull(), item.NameKo.ToNull(),
                            item.NameEn.ToNull(), item.NameJa.ToNull(),
                            item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                            item.NameZhtw.ToNull(), PreLang);
                    ddlItemID.Items.Add(new ListItem(itemName, item.ItemID.ToString()));
                }

                ddlItemID.SelectedValue = ItemID.ToString();
            }

            DAL.ItemSub itemSubDal = new DAL.ItemSub();
            
            // 2차 분류 값 가져오기
            var itemSubList = itemSubDal.GetItemSub(CurrentUserInfo.CompanyID, ItemID, true);

            ddlItemSubID.Items.Clear();
            ddlItemSubID.Items.Add(new ListItem(resMng.GetString("Second") + resMng.GetString("Group"), "0"));

            if (itemSubList != null)
            {
                foreach (var item in itemSubList)
                {
                    itemName = this.GetLangName(
                            item.NameBase.ToString().ToNull(), item.NameKo.ToNull(),
                            item.NameEn.ToNull(), item.NameJa.ToNull(),
                            item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                            item.NameZhtw.ToNull(), PreLang);
                    ddlItemSubID.Items.Add(new ListItem(itemName, item.ItemSubID.ToString()));
                }

                ddlItemSubID.SelectedValue = ItemSubID.ToString();
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {

        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}