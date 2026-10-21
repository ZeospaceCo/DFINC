using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Admin.Content
{
    public partial class MoveCopy : Base.BasePageAddressAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CategoryID;
        public string Types = "";
        public string ListID = "";
        public string Gubun = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Admin.Content.MoveCopy).Assembly;	//어셈블리 선언
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
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            ListID = GetDecryptParam("listID", "");//구분자(이동:MOVE,복사:COPY)
            Types = GetDecryptParam("types", ""); //원본카테고리(회사,공용,개인,전달)
            Gubun = GetDecryptParam("gubun", "");//구분자(이동:MOVE,복사:COPY)            
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //이동(개인주소록, 전달받은 주소록만 가능함)
            if (Gubun == "MOVE")
            {
                ltlTitle.Text = GetResource("MoveAddress");
            }
            else if (Gubun == "COPY") //즐겨찾기 이외엔 카피 가능함
            {
                ltlTitle.Text = GetResource("CopyAddress");
            }
        }

        #endregion

        #region (속성)

        #endregion
    }
}