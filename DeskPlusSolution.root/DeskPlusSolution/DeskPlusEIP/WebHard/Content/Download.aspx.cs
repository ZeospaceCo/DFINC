using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.WebHard.Web.Content
{
    public partial class Download : Base.BasePageWebHard
    {
        #region 변수설정
        public string Order = String.Empty; //sort   
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
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Content.Download).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2011-11-04
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
        /// 작성일자 : 2011-11-04
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Confirm");         //확인;
                CbtnClose.Text = resMng.GetString("Close");         //닫기;    
                CTBFolder.TitleAtt = resMng.GetString("Folder"); //폴더
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-04
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FileList = GetDecryptParam("fileList", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "WriteDate desc" : Order;
        }


        #endregion

        #region (프로퍼티)


        /// <summary>
        /// FileList
        /// </summary>
        public string FileList
        {
            get { return GetViewStateString("FileList", ""); }
            set { ViewState["FileList"] = value; }
        } 


        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}