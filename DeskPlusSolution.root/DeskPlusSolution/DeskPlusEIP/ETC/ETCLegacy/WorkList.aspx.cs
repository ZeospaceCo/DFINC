using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.ETC.Web.ETCLegacy
{
    public partial class WorkList : Base.BasePageETC
    {
        public string Callback;
        public string TagID;
        public int CurrentPage = 1;
        public string TotalCount = String.Empty;  //총카운트

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.ETCLegacy.WorkList).Assembly;	    //어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();

                GetListCount();
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            Callback = GetDecryptParam("callback", "");
            TagID = GetDecryptParam("tagID", "");
        }


        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnSubmit.Text = resMng.GetString("Confirm");
                cbtnCancel.Text = resMng.GetString("Close");
                //cbtnSearch.Text = this.resMng.GetString("Search");      
                lblSelectedReceiptor.Text = resMng.GetString("SearchType");
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2014-09-05
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.PayWork dal = new DAL.PayWork();
            TotalCount = dal.GetPageCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID).ToString();
        }
    }
}