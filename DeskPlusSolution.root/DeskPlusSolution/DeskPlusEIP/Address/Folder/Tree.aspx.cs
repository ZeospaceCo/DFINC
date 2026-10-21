using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Folder
{
    public partial class Tree : Base.BasePageAddress
    {
        #region 변수설정
        public int CategoryID;
        public string Types = String.Empty;
        public string RealType = String.Empty;
        public string Gubun = String.Empty;
        public string UseAddRootPrivateCategory = String.Empty;
        private string eventArgs = String.Empty;

        public int AccordionActive = 0;

        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Folder.Tree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();
            UseAddRootPrivateCategory = AppSettingHelper.GetAppSetting(AppSettingType.ADDRESS, "UseAddRootPrivateCategory", "N");
        }
        #endregion

        #region (함수)
        #endregion

        #region (속성)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            Types = GetDecryptParam("types", "GG");
            Gubun = GetDecryptParam("gubun", "");
            AccordionActive = GetDecryptParam("accact", "0").ToNumber<int>();
        }
        /// <summary>
        /// 작성자 : shine
        /// 개인 주소록 그룹 삭제
        /// </summary>
        private void PrivateDel()
        {
            DAL.PrivateCategory category = new DAL.PrivateCategory();
            CategoryID = tbCID.Text.ToNumber<int>();

            if (CategoryID > 0)
            {
                if (category.Del(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID))
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("categoryID", "0");
                    paraDic.Add("types", "PG");
                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    Response.Redirect("/DeskPlusEIP/Address/Folder/Tree.aspx?p=" + encParams, false);
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PrivateDel()");
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                }
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 즐겨찾기 즐겨찾기 그룹 삭제
        /// </summary>
        private void FavoriteDel()
        {
            DAL.FavorCategory category = new DAL.FavorCategory();
            CategoryID = tbCID.Text.ToNumber<int>();

            if (CategoryID > 0)
            {
                if (category.Del(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID))
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("categoryID", "0");
                    paraDic.Add("types", "FG");
                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    Response.Redirect("/DeskPlusEIP/Address/Folder/Tree.aspx?p=" + encParams, false);
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("FavoriteDel()");
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                }
            }
        }

        #endregion

        #region (이벤트핸들러)

        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            LoadParam();
            string args = eventArgs;

            switch (args)
            {
                case "PG":          // 개인주소록
                    PrivateDel();
                    break;
                case "FG":          // 즐겨찾기
                    FavoriteDel();
                    break;
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}