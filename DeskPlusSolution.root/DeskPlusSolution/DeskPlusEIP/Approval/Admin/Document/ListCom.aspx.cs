using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Document
{
    public partial class ListCom : Base.BasePageApprovalAdmin
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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Document.ListCom).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
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
            SetLanguage();

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = this.RecordPerPage.ToString();
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

                cbtnSearch.Text = resMng.GetString("OrganBtnSearch"); // "검색";
                cbtnDel.Text = resMng.GetString("DelCompletely"); //"영구삭제";

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("Subject"), "Subject"));                //제목
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("ReportUser"), "ManyName"));             //상신자
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("DocNumber"), "DocNumber"));          //문서번호
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("DeptName"), "DeptManyName"));         //부서명
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("ReportYear"), "Number2"));            //기안년도
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("FormName2"), "FormMany"));           //양식명
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("ContentTEXT"), "ContentTEXT"));        //본문검색

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");

            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
            this.Order = "CompleteDate desc";


        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}