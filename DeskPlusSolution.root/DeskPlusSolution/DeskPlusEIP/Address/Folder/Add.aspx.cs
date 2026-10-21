using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Folder
{
    public partial class Add : Base.BasePageAddress
    {
        #region 변수처리
        public int CategoryID;
        public string Types = String.Empty;
        public string Gubun = String.Empty;
        private string MultiName = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Folder.Add).Assembly;	//어셈블리 선언
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
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("BtnSave");       //"확인";
            }
        }

        private void LoadData()
        {
            LoadParam();
            LoadCategory();
        }
        #endregion

        #region (함수)

        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            Types = GetDecryptParam("types", "");
            Gubun = GetDecryptParam("gubun", "");
        }

        /// <summary>
        /// 작성자 : shine
        /// 선택한 그룹의 카테고리 정보 가져오기
        /// </summary>
        private void LoadCategory()
        {
            DataSet ds = null;

            if (Types == "PG")              // 개인주소록
            {
                ltHeader.Text = GetResource("PrivateAddressFolderAdd");
                DAL.PrivateCategory category = new DAL.PrivateCategory();
                ds = category.GetPrivateCategory(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID);
            }
            else if (Types == "FG")         // 즐겨찾기
            {
                ltHeader.Text = GetResource("FavoriteAddressFolderAdd");
                DAL.FavorCategory category = new DAL.FavorCategory();
                ds = category.GetFavoCategory(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID);
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string pName = this.GetLangName(
                        dr["pNameBase"].ToString().ToReverseXSS(),
                        dr["pNameKo"].ToString().ToReverseXSS(),
                        dr["pNameEn"].ToString().ToReverseXSS(),
                        dr["pNameJa"].ToString().ToReverseXSS(),
                        dr["pNameFr"].ToString().ToReverseXSS(),
                        dr["pNameZhcn"].ToString().ToReverseXSS(),
                        dr["pNameZhtw"].ToString().ToReverseXSS(), PreLang);

                    string name = this.GetLangName(
                        dr["NameBase"].ToString().ToReverseXSS(),
                        dr["NameKo"].ToString().ToReverseXSS(),
                        dr["NameEn"].ToString().ToReverseXSS(),
                        dr["NameJa"].ToString().ToReverseXSS(),
                        dr["NameFr"].ToString().ToReverseXSS(),
                        dr["NameZhCn"].ToString().ToReverseXSS(),
                        dr["NameZhTw"].ToString().ToReverseXSS(), PreLang);

                    //CTBParentName.Text = String.IsNullOrEmpty(pName) ? name : pName;
                    CTBParentName.Text = String.IsNullOrEmpty(name) ? pName : name;

                    if (Gubun == "mod")
                    {
                        if (Types == "PG")
                        {
                            ltHeader.Text = GetResource("PrivateAddressFolderMod");
                            CTBParentName.Text = String.IsNullOrEmpty(pName) ? GetResource("PrivateAddress") : pName;
                        }
                        else if (Types == "FG")
                        {
                            ltHeader.Text = GetResource("FavoriteAddressFolderMod");
                            CTBParentName.Text = String.IsNullOrEmpty(pName) ? GetResource("Favorite") : pName;
                        }

                        CTBName.Text = dr["NameBase"].ToString().ToNull().ToReverseXSS();
                        CTBEnglishName.Text = this.GetMuiltiLang(dr["NameKo"].ToString().ToReverseXSS(),
                            dr["NameEn"].ToString().ToReverseXSS(),
                            dr["NameJa"].ToString().ToReverseXSS(),
                            dr["NameFr"].ToString().ToReverseXSS(),
                            dr["NameZhCn"].ToString().ToReverseXSS(),
                            dr["NameZhTw"].ToString().ToReverseXSS());

                    }

                    ParentCaID = dr["ParentCategoryID"].ToString().ToNumber<int>();
                    CategoryOrder = dr["CategoryOrder"].ToString().ToNumber<int>();
                }
            }
            else
            {
                CTBParentName.Text = Types == "PG" ? GetResource("PrivateAddress") : GetResource("Favorite");
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 그룹 추가
        /// </summary>
        private void PrivateAdd()
        {
            Address.BE.PrivateCategoryInfo categoryInfo = new BE.PrivateCategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.ParentCategoryID = CategoryID;
            categoryInfo.UserID = CurrentUserInfo.UserID;
            categoryInfo.NameBase = CTBName.Text;

            MultiName = CTBEnglishName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                categoryInfo.NameKo = MultiName.Split(',')[0];
                categoryInfo.NameEn = MultiName.Split(',')[1];
                categoryInfo.NameJa = MultiName.Split(',')[2];
                categoryInfo.NameFr = MultiName.Split(',')[3];
                categoryInfo.NameZhcn = MultiName.Split(',')[4];
                categoryInfo.NameZhtw = MultiName.Split(',')[5];
            }


            DAL.PrivateCategory bizCat = new DAL.PrivateCategory();

            if (bizCat.Add(categoryInfo) > 0)
            {
                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PrivateAdd()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 그룹 수정
        /// </summary>
        private void PrivateMod()
        {
            Address.BE.PrivateCategoryInfo categoryInfo = new BE.PrivateCategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.CategoryID = CategoryID;
            categoryInfo.ParentCategoryID = ParentCaID;
            categoryInfo.UserID = CurrentUserInfo.UserID;
            categoryInfo.CategoryOrder = CategoryOrder;
            categoryInfo.NameBase = CTBName.Text.ToReverseXSS();

            MultiName = CTBEnglishName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                categoryInfo.NameKo = MultiName.Split(',')[0];
                categoryInfo.NameEn = MultiName.Split(',')[1];
                categoryInfo.NameJa = MultiName.Split(',')[2];
                categoryInfo.NameFr = MultiName.Split(',')[3];
                categoryInfo.NameZhcn = MultiName.Split(',')[4];
                categoryInfo.NameZhtw = MultiName.Split(',')[5];
            }

            DAL.PrivateCategory bizCat = new DAL.PrivateCategory();

            if (bizCat.Mod(categoryInfo))
            {
                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PrivateMod()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }

        /// <summary>
        /// 작성자 : shine
        /// 즐겨찾기 그룹 추가
        /// </summary>
        private void FavoriteAdd()
        {
            BE.FavorCategoryInfo categoryInfo = new BE.FavorCategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.ParentCategoryID = CategoryID;
            categoryInfo.UserID = CurrentUserInfo.UserID;
            categoryInfo.NameBase = CTBName.Text;

            MultiName = CTBEnglishName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                categoryInfo.NameKo = MultiName.Split(',')[0];
                categoryInfo.NameEn = MultiName.Split(',')[1];
                categoryInfo.NameJa = MultiName.Split(',')[2];
                categoryInfo.NameFr = MultiName.Split(',')[3];
                categoryInfo.NameZhcn = MultiName.Split(',')[4];
                categoryInfo.NameZhtw = MultiName.Split(',')[5];
            }

            categoryInfo.FolderID = 0;
            categoryInfo.Types = Types;

            DAL.FavorCategory bizCat = new DAL.FavorCategory();

            if (bizCat.Add(categoryInfo) > 0)
            {
                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("FavoriteAdd()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 즐겨찾기 그룹 수정
        /// </summary>
        private void FavoriteMod()
        {
            BE.FavorCategoryInfo categoryInfo = new BE.FavorCategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.CategoryID = CategoryID;
            categoryInfo.ParentCategoryID = ParentCaID;
            categoryInfo.UserID = CurrentUserInfo.UserID;
            categoryInfo.CategoryOrder = CategoryOrder;

            categoryInfo.NameBase = CTBName.Text;

            MultiName = CTBEnglishName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                categoryInfo.NameKo = MultiName.Split(',')[0];
                categoryInfo.NameEn = MultiName.Split(',')[1];
                categoryInfo.NameJa = MultiName.Split(',')[2];
                categoryInfo.NameFr = MultiName.Split(',')[3];
                categoryInfo.NameZhcn = MultiName.Split(',')[4];
                categoryInfo.NameZhtw = MultiName.Split(',')[5];
            }

            categoryInfo.FolderID = 0;
            categoryInfo.Types = Types;

            DAL.FavorCategory category = new DAL.FavorCategory();

            if (category.Mod(categoryInfo))
            {
                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("FavoriteMod()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }
        #endregion

        #region (속성)

        private int ParentCaID
        {
            get { return ViewState["ParentCaID"].ToString().ToNumber<int>(); }
            set { ViewState["ParentCaID"] = value; }
        }

        private int CategoryOrder
        {
            get { return ViewState["CategoryOrder"].ToString().ToNumber<int>(); }
            set { ViewState["CategoryOrder"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            switch (Types)
            {
                case "PG":  // 개인주소록
                    if (Gubun == "add")
                    {
                        PrivateAdd();
                    }
                    else if (Gubun == "mod")
                    {
                        PrivateMod();
                    }
                    break;
                case "FG":  // 즐겨찾기
                    if (Gubun == "add")
                    {
                        FavoriteAdd();
                    }
                    else if (Gubun == "mod")
                    {
                        FavoriteMod();
                    }
                    break;
            }
        }

        #endregion
    }
}