using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Admin.Category
{
    public partial class List : Base.BasePageCommunityAdmin
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort     
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
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Admin.Category.List).Assembly;	//어셈블리 선언
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
            GetParentCategory();
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
                //CbtnSend.Text = resMng.GetString("Send");           //전달;
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnDel.Text = resMng.GetString("Del");

                cbtnAddSub.Text = resMng.GetString("Add");
                cbtnModSub.Text = resMng.GetString("Mod");
                cbtnDelSub.Text = resMng.GetString("Del");
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            //FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            //SearchField = GetDecryptParam("searchField", "");
            //SearchValue = GetDecryptParam("searchValue", "");
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {

        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 대분류 카테고리 리스트 가져오기
        /// </summary>
        private void GetParentCategory()
        {
            DAL.Category category = new DAL.Category();
            DataTableReader dtr = category.GetCategory(CurrentUserInfo.CompanyID);

            this.lbxParentCategory.Items.Clear();

            if (dtr != null)
            {
                string catName = "";
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {

                        catName = this.GetLangName(
                             dtr["NameBase"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameKo"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameEn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameJa"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameFr"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhcn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhtw"].ToString().ToNull().ToReverseXSS(), PreLang);

                        // ┐┘
                        this.lbxParentCategory.Items.Add(new ListItem(catName,
                             dtr["CategoryID"].ToString() + "┐" + 
                             dtr["NameBase"].ToString().ToNull().ToReverseXSS() + "┐" +

                             GetMuiltiLang(dtr["NameKo"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameEn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameJa"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameFr"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhcn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhtw"].ToString().ToNull().ToReverseXSS())));

                       
                        
                    } 
                }
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}