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
using System.Text;
using System.Collections.Generic;
using Zeospace.DeskPlus.PMS.Web.Module;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Project
{
    public partial class JList : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)        
        public int FolderID;
        public int Status;
        public string SiteMapName = String.Empty;//사이트맵
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Project.JList).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();

                LoadData();
            }


            #region 버튼 이미지

            //완료프로젝트 버튼 비활성화
            if (Status == 3)
            {
                cbtnFolder.Visible = false;
                cbtnWrite.Visible = false;
            }

            #endregion
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            // 관리자 권한 
            cbtnAdmin.Visible = false;
            if (AccessAuth) cbtnAdmin.Visible = true;

            ddlSearchTerm.SelectedValue = SearchTerm;            
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            GetSiteMap();  //사이트맵
        }

        #endregion


        #region Method (함수)
        

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = resMng.GetString("Admin_Menu"); //관리자
                cbtnSearch.Text = resMng.GetString("Search"); //검색
                cbtnFolder.Text = resMng.GetString("Manage_Progress_Folder"); //진행폴더관리
                cbtnWrite.Text = resMng.GetString("Request"); //신청

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("List_ProjectName"), "ManySubject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ProjectList_Header2"), "ManyName"));

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("Unit_whole");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {   
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            Status = GetDecryptParam("status", "0").ToNumber<int>();
        }

        /// <summary>
        /// 사이트맵
        /// </summary>
        private void GetSiteMap()
        {
            if (FolderID > 0)
            {
                DAL.Folder dal = new DAL.Folder();
                DataTableReader dr = dal.GetFolderByID(CurrentUserInfo.CompanyID, FolderID);

                if (dr != null)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SiteMapName = this.GetLangName(
                                dr["FolderBase"].ToString().ToNull(),
                                dr["FolderKo"].ToString().ToNull(), dr["FolderEn"].ToString().ToNull(),
                                dr["FolderJa"].ToString().ToNull(), dr["FolderFr"].ToString().ToNull(),
                                dr["FolderZhcn"].ToString().ToNull(), dr["FolderZhtw"].ToString().ToNull(), PreLang);
                        }
                    }
                }
            }

            if (Status == 3) SiteMapName = resMng.GetString("Module_Node4");
            if (SiteMapName == "") SiteMapName = resMng.GetString("Module_Node3");

        }
        
        #endregion

        #region Property (프로퍼티)
        #endregion


        #region Event Handler (이벤트 핸들러)  
        #endregion
    }
}