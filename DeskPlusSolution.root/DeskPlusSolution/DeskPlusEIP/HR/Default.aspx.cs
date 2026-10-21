using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.HR.Web
{
    public partial class Default : Base.BasePageHR
    {
        #region 변수설정
        public string AccActive = "0"; //accordianActive

        public string FolderStatusID = "";

        public string LeftURL = "";
        public string ContentURL = "";
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
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Default).Assembly;	        //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #endregion

        #region (함수)

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetLeftURL();
            GetContentURL(FolderStatusID);
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FolderStatusID = GetDecryptParam("folderStatusID", "Private");
        }

        //Left URL
        private void GetLeftURL()
        {
            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("accActive", AccActive);

            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            paraDic.Add("folderStatusID", FolderStatusID);

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Menu/Tree.aspx?p=" + encParams;
        }

        // Content URL
        private void GetContentURL(string _menuID)
        {
            string listPage = "./Content/ListPrivate.aspx";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            switch (_menuID)
            {
                case "Attendance":          //개인 근태 현황표
                    listPage = "./Content/ListPrivate.aspx";
                    paraDic.Add("folderStatusID", "Private");

                    break;
                case "Private":          //개인 근태 현황표
                    listPage = "./Content/ListPrivate.aspx";
                    paraDic.Add("folderStatusID", "Private");

                    break;
                case "Dept":         //부서 근태 현황표
                    listPage = "./Content/ListDept.aspx";
                    paraDic.Add("folderStatusID", "Dept");

                    break;
                case "Company":        //회사 근태 현황표
                    listPage = "./Content/ListCompany.aspx";
                    paraDic.Add("folderStatusID", "Company");

                    break;
                default:
                    paraDic.Add("folderStatusID", "Private");
                    break;
            }

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = listPage + "?p=" + encParams;
        }



        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}