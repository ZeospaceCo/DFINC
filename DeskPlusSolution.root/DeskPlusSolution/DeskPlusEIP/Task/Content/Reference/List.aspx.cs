using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Task.Web.Content.Reference
{
    public partial class List : Base.BasePageTask
    {
        #region 변수설정
        public int TreeTotalCnt = 0;
        public int TreeNoReadCnt = 0;   
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Content.Reference.List).Assembly;	//어셈블리 선언
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
            GetListCount();

            // 사용자 설정 상태 가져오기
            DAL.Status dal = new DAL.Status();
            List<BE.StatusInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "CC", CurrentUserInfo.UserID);

            foreach (BE.StatusInfo _Info in al)
            {
                SearchStatus = _Info.Status;
            }

            foreach (string list in SearchStatus.Split(','))
            {
                if (list == "1")
                {
                    cbxStatus.Items[0].Selected = true;
                }

                if (list == "2")
                {
                    cbxStatus.Items[1].Selected = true;
                }

                if (list == "4")
                {
                    cbxStatus.Items[2].Selected = true;
                }

                if (list == "6")
                {
                    cbxStatus.Items[3].Selected = true;
                }
            }

            #region PageSize 설정하기

            ListCount = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.TASK, "ListCount", "20");//페이지리스트수

            DAL.ListUser dalUser = new DAL.ListUser();
            List<BE.ListUserInfo> alUser = dalUser.GetList(CurrentUserInfo.CompanyID, "CC", CurrentUserInfo.UserID);

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
                
                ddlSearchField.Items[0].Text = resMng.GetString("Title");
                ddlSearchField.Items[1].Text = "D-Day";
                ddlSearchField.Items[2].Text = resMng.GetString("DocNumber");
                ddlSearchField.Items[3].Text = resMng.GetString("RequestDate");
                ddlSearchField.Items[4].Text = resMng.GetString("StartDate");
                ddlSearchField.Items[5].Text = resMng.GetString("EndDate");
                ddlSearchField.Items[6].Text = resMng.GetString("Requester");

                cbxStatus.Items.Clear();
                cbxStatus.Items.Add(new ListItem("<p class=\"cw_r\">" + resMng.GetString("StandBy") + "</p>", "1"));       //대기             
                cbxStatus.Items.Add(new ListItem("<p class=\"cw_i\">" + resMng.GetString("Progress") + "</p>", "2,3"));    //접수+처리중
                cbxStatus.Items.Add(new ListItem("<p class=\"cw_e\">" + resMng.GetString("Complete") + "</p>", "4,5"));    //완료+재검토
                cbxStatus.Items.Add(new ListItem("<p class=\"cw_re\">" + resMng.GetString("Delay") + "</p>", "6"));        //지연
            }
        }

        /// <summary>
        ///  작성자 : famo23
        ///  작성일자 : 2015-04-21
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-17
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.ListCC dal = new DAL.ListCC();
            TreeTotalCnt = dal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", "", "", "", CurrentUserInfo.UserID, 0, 0, 0);
            TreeNoReadCnt = dal.GetNoReadCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (TreeNoReadCnt < 0) TreeNoReadCnt = 0;
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}