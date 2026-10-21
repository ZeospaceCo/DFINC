using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Address.Web.Content
{
    public partial class UserFolder : Base.BasePageAddress
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CategoryID;
        public string Types = "";
        public string FolderList = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.UserFolder).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"적용";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                ltlTitle.Text = resMng.GetString("FolderManage"); //그룹관리
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();            
            Types = GetDecryptParam("types", ""); //원본카테고리(회사,공용,개인,전달)
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            StringBuilder sb = new StringBuilder();
            DataTableReader dtr = null;

            if (Types == "PG")
            {

                DAL.PrivateCategory dal = new DAL.PrivateCategory();
                dtr = dal.GetPrivateParentCategory(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID);
                
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            string name = this.GetLangName(
                                dtr["NameBase"].ToString(), 
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            sb.Append("<li id='");
                            sb.Append(dtr["CategoryID"].ToString());
                            sb.Append("'>");
                            sb.Append(name);
                            sb.Append("</li>");
                        }
                    }
                }
            }
            else if (Types == "FG")
            {

                DAL.FavorCategory dal = new DAL.FavorCategory();
                dtr = dal.GetFavorParentCategory(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            string name = this.GetLangName(
                                dtr["NameBase"].ToString(), 
                                dtr["NameKo"].ToString(), 
                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            sb.Append("<li id='");
                            sb.Append(dtr["CategoryID"].ToString());
                            sb.Append("'>");
                            sb.Append(name);
                            sb.Append("</li>");
                        }
                    }
                }
            }

            FolderList = sb.ToString();
        }

        #endregion

        #region (속성)

        #endregion
    }
}