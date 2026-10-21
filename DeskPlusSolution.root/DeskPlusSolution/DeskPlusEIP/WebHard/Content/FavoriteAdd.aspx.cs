using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.WebHard.Web.Content
{
    public partial class FavoriteAdd : Base.BasePageWebHard
    {
        #region 변수처리
        public int FavoriteID;
        public string Gubun = String.Empty;
        private string MultiName = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Content.FavoriteAdd).Assembly;	//어셈블리 선언
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
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"확인";
                ctbParentName.TitleAtt = resMng.GetString("ParentFolder");
                ctbName.TitleAtt = resMng.GetString("PresentFolder");
                ctbMultiName.TitleAtt = resMng.GetString("MultiNames");
            }
        }

        private void LoadData()
        {
            LoadParam();

            LoadFavorite();
        }
        #endregion

        #region (함수)

        private void LoadParam()
        {
            FavoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 :2011-12-26
        /// 작성목적 : 선택한 즐겨찾기 정보 가져오기 (추가 및 수정)
        /// </summary>
        private void LoadFavorite()
        {
            ltHeader.Text = (Gubun == "mod") ? GetResource("FavoriteFolderMod") : GetResource("FavoriteFolderAdd");

            DAL.Favorite dal = new DAL.Favorite();
            DataTableReader dtr = dal.GetFavoriteList(CurrentUserInfo.CompanyID, FavoriteID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        if (Gubun == "mod")
                        {
                            string pName = this.GetLangName(
                            dtr["pNameBase"].ToString(),
                            dtr["pNameKo"].ToString(), dtr["pNameEn"].ToString(), dtr["pNameJa"].ToString(),
                            dtr["pNameFr"].ToString(), dtr["pNameZhcn"].ToString(), dtr["pNameZhtw"].ToString(), PreLang);

                            ctbParentName.Text = String.IsNullOrEmpty(pName) ? GetResource("FavoriteFolder") : pName.ToReverseXSS();
                            ctbName.Text = dtr["NameBase"].ToString().ToNull().ToReverseXSS();

                            ctbMultiName.Text = this.GetMuiltiLang(
                                dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(),
                                dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(),
                                dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull()).ToReverseXSS();
                        }
                        else
                        {
                            ctbParentName.Text = String.IsNullOrEmpty(name) ? GetResource("FavoriteFolder") : name.ToReverseXSS();
                        }

                        ParentFID = dtr["ParentFavoriteID"].ToString().ToNumber<int>();
                        FavoriteOrder = dtr["FavoriteOrder"].ToString().ToNumber<int>();
                    }
                }
                else
                {
                    ctbParentName.Text = GetResource("FavoriteFolder");
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-26
        /// 작성목적 : 즐겨찾기 폴더 추가
        /// </summary>
        private void AddFavorite()
        {
            BE.FavoriteInfo info = new BE.FavoriteInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ParentFavoriteID = FavoriteID;
            info.UserID = CurrentUserInfo.UserID;
            info.NameBase = this.ctbName.Text;

            MultiName = this.ctbMultiName.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                info.NameKo = MultiName.Split(',')[0];
                info.NameEn = MultiName.Split(',')[1];
                info.NameJa = MultiName.Split(',')[2];
                info.NameFr = MultiName.Split(',')[3];
                info.NameZhcn = MultiName.Split(',')[4];
                info.NameZhtw = MultiName.Split(',')[5];
            }

            info.FolderID = 0;
            info.Types = "FC";

            DAL.Favorite dal = new DAL.Favorite();

            if (dal.AddFavorite(info) > 0)
            {
                string sc = "FavoriteAdd.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddFavorite()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-26
        /// 작성목적 : 즐겨찾기 폴더 수정
        /// </summary>
        private void ModFavorite()
        {
            BE.FavoriteInfo info = new BE.FavoriteInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.FavoriteID = FavoriteID;
            info.ParentFavoriteID = ParentFID;
            info.UserID = CurrentUserInfo.UserID;
            info.FavoriteOrder = FavoriteOrder;
            info.NameBase = this.ctbName.Text;

            MultiName = this.ctbMultiName.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                info.NameKo = MultiName.Split(',')[0];
                info.NameEn = MultiName.Split(',')[1];
                info.NameJa = MultiName.Split(',')[2];
                info.NameFr = MultiName.Split(',')[3];
                info.NameZhcn = MultiName.Split(',')[4];
                info.NameZhtw = MultiName.Split(',')[5];
            }

            info.FolderID = 0;
            info.Types = "FC";

            DAL.Favorite dal = new DAL.Favorite();

            if (dal.ModFavorite(info))
            {
                string sc = "FavoriteAdd.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModFavorite()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }


        #endregion

        #region (속성)

        private int ParentFID
        {
            get { return ViewState["ParentFID"].ToString().ToNumber<int>(); }
            set { ViewState["ParentFID"] = value; }
        }

        private int FavoriteOrder
        {
            get { return ViewState["FavoriteOrder"].ToString().ToNumber<int>(); }
            set { ViewState["FavoriteOrder"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            if (Gubun == "add")
            {
                AddFavorite();
            }
            else if (Gubun == "mod")
            {
                ModFavorite();
            }
        }

        #endregion
    }
}