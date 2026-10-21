using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Community.Web.Site.Main.Mail.Address
{
    public partial class Default : Base.BasePageCommunity
    {
        #region 변수설정  
        public string Gubun = String.Empty;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Main.Mail.Address.Default).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-07
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");     //"확인";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                cbtnSearch.Text = resMng.GetString("Search");       //"검색";
                cbtnAdd.Text = resMng.GetString("AddUser");         //"사용자추가";


                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Email"), "Mail"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-07
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            Gubun = GetDecryptParam("gubun", "0");
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}