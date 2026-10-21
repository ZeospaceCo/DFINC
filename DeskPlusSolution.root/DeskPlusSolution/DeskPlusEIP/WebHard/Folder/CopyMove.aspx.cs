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

namespace Zeospace.DeskPlus.WebHard.Web.Folder
{
    public partial class CopyMove : Base.BasePageWebHard
    {
        #region 변수처리
        public int FolderID;
        public int RealFolderID;
        public string FileList;
        public int UID;
        public int PrivateQuota = 0;// 개인에게 할당된 용량
        public string Gubun;
        public string ListType;
        public string Order = String.Empty; //sort   

        public string Types = string.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Folder.CopyMove).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-01-13
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            // 개인에게 할당된 용량 가져오기 (MB)
            PrivateQuota = CurrentUserInfo.PrivateQuota;

            LoadParam();
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-13
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            RealFolderID = GetDecryptParam("realFolderID", "0").ToNumber<int>();
            UID = GetDecryptParam("uID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "0");
            ListType = GetDecryptParam("listType", "0");
            FileList = GetDecryptParam("fileList", "0");          
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "WriteDate desc" : Order;

            Types = GetDecryptParam("types", "");
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}