using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portlet
{
    public partial class AddFolder : Base.BasePageEIPAdmin
    {
        #region 변수설정
        public int CategoryID;
        public string Gubun;

        public int CurrentPage = 0;
        public string RecordPerPage = string.Empty; //RecordPerPage
        public string SearchField = string.Empty; //SearchField
        public string SearchValue = string.Empty; //SearchValue        
        public string Order = string.Empty; //Order
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddFolder).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            if (Gubun == "add") cbtnDel.Visible = false;

            ddlActiveCategory.Items.Clear();
            ddlActiveCategory.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
            ddlActiveCategory.Items.Add(new ListItem(resMng.GetString("NotUsed"), "0"));

            GetCategory();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "");

            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnDel.Text = resMng.GetString("Del");
                cbtnAdd.Text = (Gubun == "add") ? resMng.GetString("Save") : resMng.GetString("Save");
                cbtnMove.Text = resMng.GetString("Orders");
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 내용가져오기
        /// </summary>
        private void GetCategory()
        {
            string name = resMng.GetString("PortletManagement");
            if (CategoryID > 0)
            {
                DAL.PortletCategory dal = new DAL.PortletCategory();
                DataSet ds = dal.GetPortletCategory(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID, true, false);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {       
                        #region 전체경로
                        /*
                        string name = this.GetLangName(
                                dr["PathNameBase"].ToString(),
                                dr["PathNameKo"].ToString(),
                                dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                                dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(),
                                dr["PathNameZhTw"].ToString(), PreLang);

                        switch (PreLang)
                        {
                            case "ko":
                                name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "en":
                                name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "ja":
                                name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "fr":
                                name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "zhcn":
                                name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "zhtw":
                                name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                        }

                        ltlHeader.Text = name.Substring(1, name.Length - 1);
                        */

                        name = this.GetLangName(
                               dr["NameBase"].ToString(),dr["NameKo"].ToString(),dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                               dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),dr["NameZhTw"].ToString(), PreLang);

                        #endregion

                        if (Gubun == "add")
                        {
                            ltlParentCategory.Text = this.GetLangName(
                                dr["NameBase"].ToString(),
                                dr["NameKo"].ToString(),
                                dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                                dr["NameZhTw"].ToString(), PreLang);

                        }
                        else if (Gubun == "mod")
                        {
                            ctbName.Text = dr["NameBase"].ToString().ToReverseXSS();

                            this.ctbMultiLang.Text = this.GetMuiltiLang(
                                dr["NameKo"].ToString().ToReverseXSS(),
                                dr["NameEn"].ToString().ToReverseXSS(),
                                dr["NameJa"].ToString().ToReverseXSS(),
                                dr["NameFr"].ToString().ToReverseXSS(),
                                dr["NameZhCn"].ToString().ToReverseXSS(),
                                dr["NameZhTw"].ToString().ToReverseXSS()
                                );

                            ctbDesc.Text = dr["Description"].ToString().ToReverseXSS();
                            ddlActiveCategory.SelectedValue = dr["ActiveCategory"].ToString();

                            GetParent(dr["ParentCategoryID"].ToString().ToNumber<int>());
                        }
                    }
                }
            }

            ltlHeader.Text = name;
        }

        /// <summary>
        /// 상위카테고리명 가져오기
        /// </summary>
        /// <param name="_parentCategoryID"></param>
        private void GetParent(int _parentCategoryID)
        {
            if (_parentCategoryID > 0)
            {
                DAL.PortletCategory dal = new DAL.PortletCategory();
                DataSet ds = dal.GetPortletCategory(CurrentUserInfo.CompanyID, _parentCategoryID, CurrentUserInfo.UserID, true, false);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ltlParentCategory.Text = dr["NameBase"].ToString();

                        /*
                        ctbMultiLang.Text = GetMuiltiLang(
                            dr["NameKo"].ToString().ToReverseXSS(),
                            dr["NameEn"].ToString().ToReverseXSS(),
                            dr["NameJa"].ToString().ToReverseXSS(),
                            dr["NameFr"].ToString().ToReverseXSS(),
                            dr["NameZhcn"].ToString().ToReverseXSS(),
                            dr["NameZhtw"].ToString().ToReverseXSS());
                        */
                    }
                }
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        

        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}