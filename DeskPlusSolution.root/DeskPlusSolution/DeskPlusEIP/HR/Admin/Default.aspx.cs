using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.HR.Web.Admin
{
    public partial class Default : Base.BasePageHRAdmin
    {
        #region 변수설정
        public string AccActive = "0"; //accordianActive

        public string MenuID = "";

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Admin.Default).Assembly;	        //어셈블리 선언
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
            GetContentURL(MenuID);
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            MenuID = GetDecryptParam("menuID", "HoliAdmin");
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

            paraDic.Add("menuID", MenuID);

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Menu/Tree.aspx?p=" + encParams;
        }

        // Content URL
        private void GetContentURL(string _menuID)
        {
            string listPage = "/DeskPlusEIP/ETC/Holiday/ListAdmin.aspx";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            switch (_menuID)
            {
                case "HoliAdmin":          //연차관리
                    listPage = "/DeskPlusEIP/ETC/Holiday/ListAdmin.aspx";
                    paraDic.Add("menuID", "HoliAdmin");

                    break;
                case "WorkAdmin":          //근무유형관리
                    listPage = "./Content/WorkAdmin.aspx";
                    paraDic.Add("menuID", "WorkAdmin");

                    break;
                case "RoleAdmin":         //조회권한관리
                    listPage = "./Content/RoleAdmin.aspx";
                    paraDic.Add("menuID", "RoleAdmin");

                    break;
                case "AttendanceRoleAdmin":         //수정권한관리
                    listPage = "./Content/AttendanceRoleAdmin.aspx";
                    paraDic.Add("menuID", "AttendanceRoleAdmin");

                    break;
                default:
                    paraDic.Add("menuID", "HoliAdmin");
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