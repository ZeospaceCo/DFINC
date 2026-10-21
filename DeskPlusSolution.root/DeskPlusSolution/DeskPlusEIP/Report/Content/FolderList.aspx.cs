using System;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class FolderList : Base.BasePageReport
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.FolderList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     :
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
            
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     :
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                

            }
        }
    }
}