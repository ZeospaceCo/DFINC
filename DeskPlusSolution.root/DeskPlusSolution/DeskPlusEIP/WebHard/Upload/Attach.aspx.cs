using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections;

namespace Zeospace.DeskPlus.WebHard.Web.Upload
{
    public partial class Attach : Base.BasePageWebHard
    {
        #region 변수처리
        public int FolderID;
        public string TmpValue;
        public int UID;
        public string GUID = "";
        public string CallBackEvent = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Upload.Attach).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-27
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
        /// 작성일자 : 2012-01-27
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            TmpValue = GetDecryptParam("tmpValue", "0");
            UID = GetDecryptParam("uID", "0").ToNumber<int>();
            CallBackEvent = GetDecryptParam("callBackEvent", "");
            GUID = GetDecryptParam("guid", "");            
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}