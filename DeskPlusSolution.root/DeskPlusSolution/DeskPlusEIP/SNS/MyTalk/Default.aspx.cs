using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.SNS.Web.MyTalk
{
    public partial class Default : Base.BasePageMyTalk
    {
        #region 변수설정
        public int RUserID = 0;    //댓글 받을 사람ID
        public string LeftURL = "";
        public string ContentURL = "";
        #endregion 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.MyTalk.Default).Assembly;	//어셈블리 선언
            #endregion


            #region 이미지 처리

            #endregion

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
            GetURL();
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            RUserID = GetDecryptParam("rUserID", CurrentUserInfo.UserID.ToString()).ToNumber<int>();
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
                //cbtnCancel.Text = GetResource("Cancel");
            }
        }

        private void GetURL()
        {
            // Left URL
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("rUserID", RUserID.ToString());

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Menu/Menu.aspx?p=" + encParams;
            ContentURL = "./Content/List.aspx?p=" + encParams;
        }
    }
}