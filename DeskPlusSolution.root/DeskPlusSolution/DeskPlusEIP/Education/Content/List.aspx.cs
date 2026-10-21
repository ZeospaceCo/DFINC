using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
namespace Zeospace.DeskPlus.Education.Web.Content
{
    public partial class List : Base.BasePageEducation
    {
        #region 변수설정
        public string RecordPerPage = String.Empty; 
        public string TotalCount = String.Empty;  
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
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Content.List).Assembly;	 
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
            ddlItem.Items.Clear();
            ddlItem.Items.Add(new ListItem("전체", ""));

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");

            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ActiveCode"].ToString() == "1")
                {
                    string name = this.GetLangName(dv[i]["NameBase"].ToString(), dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                                                dv[i]["NameFr"].ToString(), dv[i]["NameZhcn"].ToString(), dv[i]["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                    ddlItem.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
                }
            }

            ddlItem.SelectedValue = SearchItem;

            ddlComplete.Items.Clear();
            ddlComplete.Items.Add(new ListItem("전체", ""));
            ddlComplete.Items.Add(new ListItem("완료", "3"));
            ddlComplete.Items.Add(new ListItem("미완료", "0"));
            ddlComplete.SelectedValue = SearchComplete;

            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("전체", ""));
            ddlStatus.Items.Add(new ListItem("대기", "1"));
            ddlStatus.Items.Add(new ListItem("진행중", "2"));
            ddlStatus.Items.Add(new ListItem("완료", "3"));
            ddlStatus.Items.Add(new ListItem("지연", "4"));
            ddlStatus.SelectedValue = SearchStatus;

           
            #region PageSize 설정하기          

            RecordPerPage = "10";

            DAL.ListUser dalUser = new DAL.ListUser();
            List<BE.ListUserInfo> al = dalUser.GetList(CurrentUserInfo.CompanyID, "UL", CurrentUserInfo.UserID);

            foreach (BE.ListUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            #endregion

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
                cbtnSearch.Text = "검색";
            }
        }

        #endregion
    }
}