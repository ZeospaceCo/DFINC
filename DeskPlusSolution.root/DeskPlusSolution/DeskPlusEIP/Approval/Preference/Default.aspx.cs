using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Preference
{
    public partial class Default : Base.BasePageApproval
    {
        #region 변수설정
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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Default).Assembly;	//어셈블리 선언
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

            GetLeftURL();
            GetContentURL(MenuID);
        }

        #endregion

        #region (함수)

        private void LoadParam()
        {
            MenuID = GetDecryptParam("menuID", "");
        }

        //Left URL
        private void GetLeftURL()
        {
            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("menuID", MenuID);
            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Menu/Tree.aspx?p=" + encParams;
        }

        // Content URL
        private void GetContentURL(string _menuID)
        {
            string listPage = "./Favorite/List.aspx";

            _menuID = (_menuID.Equals("")) ? "F" : _menuID;

            switch (_menuID)
            {
                case "F": //자주지정하는 결재자
                    listPage = "./Favorite/List.aspx";
                    break;
                case "P": //대결자 관리
                    listPage = "./Proxy/List.aspx";
                    break;
                case "S": //사인,인장관리
                    listPage = "./Signature/List.aspx";
                    break;
                case "U": //개인 결재망 관리
                    listPage = "./Flow/List.aspx";
                    break;
                case "FF": //자주사용하는양식관리
                    listPage = "./FavoriteForm/List.aspx";
                    break;
                default:
                    listPage = "./Favorite/List.aspx";
                    break;
            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("menuID", _menuID);

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