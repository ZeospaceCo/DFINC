using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Address
{
    public partial class List : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string gubun = "";
        public string firstChar = "";
        public int pageNo = 1;

        public string action = "";
        public string mailID = "";
        public string code = "";
        public string mail_sn = "";
        public string hearder = "";
        public string history = "";
        public string searchValue = "";
        public string fastPath = "";
        public string fileID = "";

        public string callPage = "";
        public string SwfUploadGUID = "";
        public string FileCheck = "";

        public int folderID = 0;
        public string order = "";
        public string publicGubun = "";
        public string shareGubun = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Address.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();

                tbSearchValue.Value = searchValue;

                GetListCount();
            }

        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            gubun = GetDecryptParam("gubun", "G");
            firstChar = GetDecryptParam("FirstChar", "");
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();

            action = GetDecryptParam("Action", "");
            mailID = GetDecryptParam("MailID", "");
            code = GetDecryptParam("Code", "");
            mail_sn = GetDecryptParam("Mail_sn", "");
            hearder = GetDecryptParam("Hearder", "");
            history = GetDecryptParam("History", "1");

            searchValue = GetDecryptParam("sv", "");
            fastPath = GetDecryptParam("FastPath", "0");
            fileID = GetDecryptParam("FileID", "");
            SwfUploadGUID = GetDecryptParam("SwfUploadGUID", "");
            FileCheck = GetDecryptParam("FileCheck", "");
            callPage = GetDecryptParam("CallPage", "Send");

            folderID = GetDecryptParam("FolderID", "0").ToNumber<int>();
            order = GetDecryptParam("Order", "WriteDate desc");
            shareGubun = GetDecryptParam("ShareGubun", "N");
            publicGubun = GetDecryptParam("PublicGubun", "");
        }

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-27
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            string lastChar = String.Empty;
            switch (firstChar)
            {
                case "ㄱ":
                    lastChar = "ㄴ";
                    break;
                case "ㄴ":
                    lastChar = "ㄷ";
                    break;
                case "ㄷ":
                    lastChar = "ㄹ";
                    break;
                case "ㄹ":
                    lastChar = "ㅁ";
                    break;
                case "ㅁ":
                    lastChar = "ㅂ";
                    break;
                case "ㅂ":
                    lastChar = "ㅅ";
                    break;
                case "ㅅ":
                    lastChar = "ㅇ";
                    break;
                case "ㅇ":
                    lastChar = "ㅈ";
                    break;
                case "ㅈ":
                    lastChar = "ㅊ";
                    break;
                case "ㅊ":
                    lastChar = "ㅋ";
                    break;
                case "ㅋ":
                    lastChar = "ㅌ";
                    break;
                case "ㅌ":
                    lastChar = "ㅍ";
                    break;
                case "ㅍ":
                    lastChar = "ㅎ";
                    break;
                case "ㅎ":
                    lastChar = "A";
                    break;
                case "A":
                    lastChar = "Z";
                    break;
                case "0":
                    lastChar = "9";
                    break;

                default:
                    break;
            }

            Ws.Data.List dal = new Ws.Data.List();
            int Total = dal.GetTotalCnt(CompanyID, UserID, firstChar, lastChar, gubun, searchValue);

            int recsInLastPage = Total % 10;
            int TotalPage = 0;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
            {
                TotalPage = (Total / 10);
            }
            else
            {
                TotalPage = (Total / 10) + 1;
            }

            if (TotalPage < pageNo)
            {
                pageNo = 1;
            }

            TotalNum.InnerText = TotalPage.ToString();
            PageNum.InnerText = pageNo.ToString();
        }


        #endregion
    }
}