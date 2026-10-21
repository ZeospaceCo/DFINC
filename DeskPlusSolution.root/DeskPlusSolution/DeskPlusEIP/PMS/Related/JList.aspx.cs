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
using Zeospace.DeskPlus.PMS.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Related
{
    public partial class JList : Base.BasePagePMS
    {
        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Related.JList).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }


            #region 버튼 이미지
            //cbtnFindit.Icon = BaseImgPath + "/Board/Button/but_search.gif";
            #endregion


        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {            
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnSearch.Text = resMng.GetString("Search");
                cbtnAdd.Text = resMng.GetString("Add");


                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("List_ProjectName"), "ManySubject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ProjectList_Header2"), "ManyName"));
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {

        }


        
        #endregion
    }
}