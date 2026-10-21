using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.Web.Draft
{
    public partial class AddList : Base.BasePageTask
    {
        #region 변수설정

        public string Callback;
        public string TotalCount = String.Empty;  //총카운트

        #endregion 

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";	//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Draft.AddList).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
                GetListCount();
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
            
            // 기본 정보
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            Callback = GetDecryptParam("callback", "");
        }       

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnSubmit.Text = resMng.GetString("BtnConfirm");
                cbtnCancel.Text = resMng.GetString("Close");
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch");

                ddlSearchField.Items[0].Text = resMng.GetString("Title");

                lblSelectedReceiptor.Text = resMng.GetString("SelectDraft");
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
            ddlIsGubun.Items.Add(new ListItem(resMng.GetString("IsGubun1"), "1"));
            ddlIsGubun.Items.Add(new ListItem(resMng.GetString("IsGubun2"), "2"));
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-27
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.Draft dal = new DAL.Draft();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", CurrentUserInfo.UserID, 0, 0,0).ToString();
        }

        #endregion 
    }
}