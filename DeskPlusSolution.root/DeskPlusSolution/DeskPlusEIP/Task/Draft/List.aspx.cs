using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Task.Web.Draft
{
    public partial class List : Base.BasePageTask
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
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Draft.List).Assembly;	//어셈블리 선언
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
        /// 
        /// </summary>
        private void LoadData()
        {
            // 분류 정보 가져오기 
            LoadItem();
          
            #region PageSize 설정하기

            ListCount = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.TASK, "ListCount", "20");//페이지리스트수

            DAL.ListUser dalUser = new DAL.ListUser();
            List<BE.ListUserInfo> alUser = dalUser.GetList(CurrentUserInfo.CompanyID, "DR", CurrentUserInfo.UserID);

            foreach (BE.ListUserInfo userInfo in alUser)
            {
                ListCount = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = ListCount;

            #endregion

            // 기본 정보
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;                   
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnSearch.Text = resMng.GetString("OrganBtnSearch");
                cbtnDel.Text = resMng.GetString("Del");
                
                ddlSearchField.Items[0].Text = resMng.GetString("Title");
            }
        }

        /// <summary>
        ///  작성자 : famo23
        ///  작성일자 : 2015-04-24
        ///  작성목적 : 대분류, 소분류 정보가져오기
        /// </summary>
        private void LoadItem()
        {
            ddlIsGubun.Items.Clear();
            ddlIsGubun.Items.Add(new ListItem(resMng.GetString("IsGubun"), "0"));
            ddlIsGubun.Items.Add(new ListItem(resMng.GetString("ShortIsGubun1"), "1"));
            ddlIsGubun.Items.Add(new ListItem(resMng.GetString("ShortIsGubun2"), "2"));
            ddlIsGubun.SelectedValue = IsGubun.ToString();

            // 대분류 값 가져오기
            DAL.Item itemDal = new DAL.Item();
            var itemList = itemDal.GetItem(CurrentUserInfo.CompanyID, true);

            ddlItem.Items.Clear();
            ddlItem.Items.Add(new ListItem(resMng.GetString("Category"), "0"));

            if (itemList != null)
            {
                foreach (var item in itemList)
                {
                    string tmpName = this.GetLangName(
                                  item.NameBase, item.NameKo,
                                  item.NameEn, item.NameJa,
                                  item.NameFr, item.NameZhcn,
                                  item.NameZhtw, PreLang);

                    if (tmpName.Length > 6) tmpName = tmpName.Substring(0, 5) + "..";
                    ddlItem.Items.Add(new ListItem(tmpName, item.ItemID.ToString()));
                }

                ddlItem.SelectedValue = ItemID.ToString();
            }

            // 소분류 값 가져오기
            DAL.ItemSub itemSubDal = new DAL.ItemSub();
            var itemSubList = itemSubDal.GetItemSub(CurrentUserInfo.CompanyID, ItemID, true);

            ddlItemSub.Items.Clear();
            ddlItemSub.Items.Add(new ListItem(resMng.GetString("SubCategory"), "0"));

            if (itemSubList != null)
            {
                foreach (var item in itemSubList)
                {
                    string tmpName = this.GetLangName(
                                  item.NameBase, item.NameKo,
                                  item.NameEn, item.NameJa,
                                  item.NameFr, item.NameZhcn,
                                  item.NameZhtw, PreLang);

                    if (tmpName.Length > 6) tmpName = tmpName.Substring(0, 5) + "..";
                    ddlItemSub.Items.Add(new ListItem(tmpName, item.ItemSubID.ToString()));
                }

                ddlItemSub.SelectedValue = ItemSubID.ToString();
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}