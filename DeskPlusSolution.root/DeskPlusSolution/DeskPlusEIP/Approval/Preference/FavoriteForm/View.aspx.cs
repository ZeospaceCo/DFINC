using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Approval.BE;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Approval.Web.Preference.FavoriteForm
{
    public partial class View : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public int FormID = 0;
        public string Contents = string.Empty; //내용
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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.FavoriteForm.View).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            LoadFormInfo(FormID);
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
            FormID = GetDecryptParam("formID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }
        /// <summary>
        /// 양식정보 가져오기
        /// </summary>
        /// <param name="_formID"></param>
        private void LoadFormInfo(int _formID)
        {
            if (_formID.Equals(0)) return;

            DAL.Form fDal = new DAL.Form();
            List<FormInfo> fList = fDal.GetForm(CurrentUserInfo.CompanyID, _formID);

            foreach (FormInfo fInfo in fList)
            {
                htaFormHTML.Value = fInfo.ContentHTML.Replace("Func_ApprovalRegistForm.css", "Func_ApprovalView.css");
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}