using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class FavoriteAdd : Base.BasePageBoard
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
            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.FavoriteAdd).Assembly;	//어셈블리 선언
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
                CbtnConfirm.Text = resMng.GetString("Confirm");       //"확인";
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
        /// 작성자 : 심재운
        /// 선택한 즐겨찾기 정보 가져오기 (추가 및 수정)
        /// </summary>
        private void LoadFavorite()
        {
            ltHeader.Text = (string.Compare(Gubun, "Mod", true) == 0) ? GetResource("FavoriteFolderMod") : GetResource("FavoriteFolderAdd");

            DAL.Favorite dal = new DAL.Favorite();
            DataTableReader dtr = dal.GetFavoriteList(CurrentUserInfo.CompanyID, FavoriteID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string name = this.GetLangName(
                            dtr["NameBase"].ToString().ToReverseXSS(),
                            dtr["NameKo"].ToString().ToReverseXSS(), dtr["NameEn"].ToString().ToReverseXSS(), dtr["NameJa"].ToString().ToReverseXSS(),
                            dtr["NameFr"].ToString().ToReverseXSS(), dtr["NameZhCn"].ToString().ToReverseXSS(), dtr["NameZhTw"].ToString().ToReverseXSS(), PreLang);

                        if (string.Compare(Gubun, "Mod", true) == 0)
                        {
                            string pName = this.GetLangName(
                            dtr["pNameBase"].ToString(),
                            dtr["pNameKo"].ToString(), dtr["pNameEn"].ToString(), dtr["pNameJa"].ToString(),
                            dtr["pNameFr"].ToString(), dtr["pNameZhCn"].ToString(), dtr["pNameZhTw"].ToString(), PreLang);

                            ctbParentName.Text = String.IsNullOrEmpty(pName) ? GetResource("FavoriteFolder") : pName;
                            ctbName.Text = dtr["NameBase"].ToString().ToNull().ToReverseXSS();

                            ctbEnglishName.Text = this.GetMuiltiLang(
                                dtr["NameKo"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameEn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameJa"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameFr"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhcn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhtw"].ToString().ToNull().ToReverseXSS());
                        }
                        else
                        {
                            ctbParentName.Text = String.IsNullOrEmpty(name) ? GetResource("FavoriteFolder") : name;
                        }

                        ParentFID = dtr["ParentFavoriteID"].ToString().ToNumber<int>();
                        FavoriteOrder = dtr["FavoriteOrder"].ToString().ToNumber<int>();
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기 폴더 추가
        /// </summary>
        private void AddFavorite()
        {
            Board.BE.FavoriteInfo info = new BE.FavoriteInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.FavoriteID = 0;
            info.ParentFavoriteID = FavoriteID;
            info.UserID = CurrentUserInfo.UserID;
            info.FavoriteOrder = 0;
            info.NameBase = this.ctbName.Text;

            MultiName = this.ctbEnglishName.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                info.NameKo = MultiName.Split(',')[0];
                info.NameEn = MultiName.Split(',')[1];
                info.NameJa = MultiName.Split(',')[2];
                info.NameFr = MultiName.Split(',')[3];
                info.NameZhcn = MultiName.Split(',')[4];
                info.NameZhtw = MultiName.Split(',')[5];
            }

            info.CategoryID = 0;            
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
        /// 작성자 : 심재운
        /// 즐겨찾기 폴더 수정
        /// </summary>
        private void ModFavorite()
        {
            Board.BE.FavoriteInfo info = new BE.FavoriteInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.FavoriteID = FavoriteID;
            info.ParentFavoriteID = ParentFID;
            info.UserID = CurrentUserInfo.UserID;
            info.FavoriteOrder = FavoriteOrder;
            info.NameBase = this.ctbName.Text;

            MultiName = this.ctbEnglishName.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                info.NameKo = MultiName.Split(',')[0];
                info.NameEn = MultiName.Split(',')[1];
                info.NameJa = MultiName.Split(',')[2];
                info.NameFr = MultiName.Split(',')[3];
                info.NameZhcn = MultiName.Split(',')[4];
                info.NameZhtw = MultiName.Split(',')[5];
            }

            info.CategoryID = 0;            
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

            if (string.Compare(Gubun, "Add", true) == 0)
            {
                AddFavorite();
            }
            else if (string.Compare(Gubun, "Mod", true) == 0)
            {
                ModFavorite();
            }
        }

        #endregion
    }
}