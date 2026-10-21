using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Admin.Folder
{
    public partial class Add : Base.BasePageAddressAdmin
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Admin.Folder.Add).Assembly;	//어셈블리 선언
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
                ltHeader.Text = resMng.GetString("CommonAddressFolderAdd");
                cbtnFolderOrder.Text = resMng.GetString("FolderManage"); //그룹관리
                cbtnFolderMove.Text = resMng.GetString("FolderMove");//그룹이동
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
        /// 작성자 : 심재운
        /// 선택한 그룹의 카테고리 정보 가져오기
        /// </summary>
        private void LoadCategory()
        {
            if (CategoryID == 0) cbtnFolderMove.Visible = false;

            string useAddRootCommonCategory = AppSettingHelper.GetAppSetting(AppSettingType.ADDRESS, "UseAddRootCommonCategory", "N");
            if (useAddRootCommonCategory == "N" && CategoryID == 0)
            {
                CbtnConfirm.Visible = false;
                lblMsg.Text = resMng.GetString("XmlOptionMsg");
            }

            DataSet ds = null;

            DAL.CommonCategory category = new DAL.CommonCategory();
            ds = category.GetCommonCategory(CurrentUserInfo.CompanyID, CategoryID);

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
                        ltHeader.Text = GetResource("CommonAddressFolderMod");
                        CTBParentName.Text = String.IsNullOrEmpty(pName) ? GetResource("CommonAddress") : pName;
                        CTBName.Text = dr["NameBase"].ToString().ToNull().ToReverseXSS();

                        CTBEnglishName.Text = this.GetMuiltiLang(dr["NameKo"].ToString().ToReverseXSS(),
                            dr["NameEn"].ToString().ToReverseXSS(),
                            dr["NameJa"].ToString().ToReverseXSS(),
                            dr["NameFr"].ToString().ToReverseXSS(),
                            dr["NameZhCn"].ToString().ToReverseXSS(),
                            dr["NameZhTw"].ToString().ToReverseXSS());
                    }
                }
            }
            else
            {
                CTBParentName.Text = GetResource("CommonAddress");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 그룹 추가
        /// </summary>
        private void Register()
        {
            Address.BE.CommonCategoryInfo categoryInfo = new BE.CommonCategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.ParentCategoryID = CategoryID;
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

            categoryInfo.WriteUserID = CurrentUserInfo.UserID;

            DAL.CommonCategory bizCat = new DAL.CommonCategory();

            int categoryID = bizCat.Add(categoryInfo);
            if (categoryID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("categoryID", categoryID.ToString());
                string url = @"parent.frmLeft.location='{0}';";
                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                url = string.Format(url, "/DeskPlusEIP/Address/Admin/Menu/Menu.aspx?p=" + encParams);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("Complete"), url);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PrivateAdd()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 그룹 수정
        /// </summary>
        private void Mod()
        {
            Address.BE.CommonCategoryInfo categoryInfo = new BE.CommonCategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.CategoryID = CategoryID;
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

            categoryInfo.WriteUserID = CurrentUserInfo.UserID;

            DAL.CommonCategory bizCat = new DAL.CommonCategory();

            if (bizCat.Mod(categoryInfo))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("categoryID", CategoryID.ToString());
                string url = @"parent.frmLeft.location='{0}';";
                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                url = string.Format(url, "/DeskPlusEIP/Address/Admin/Menu/Menu.aspx?p=" + encParams);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "modSuccess", GetResource("Complete"), url);

            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PrivateMod()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }
        #endregion

        #region (속성)



        #endregion

        #region (이벤트핸들러)

        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            if (Gubun == "add")
            {
                Register();
            }
            else if (Gubun == "mod")
            {
                Mod();
            }
        }

        #endregion
    }
}