using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Folder
{
    public partial class Default : Base.BasePageApproval
    {
        #region 변수설정
        public string AccActive = "0"; //accordianActive
        public string CabinetID;

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Default).Assembly;	//어셈블리 선언
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

            GetDefaultMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            GetLeftURL();
            GetContentURL(CabinetID);
        }

        #endregion

        #region (함수)

        private void LoadParam()
        {
            CabinetID = GetDecryptParam("cabinetID", "");
            if (CabinetID == "")
            {
                //다이렉트 호출 시 사용
                CabinetID = GetParm("cabinetID", "");
            }
        }


        /// <summary>
        /// 초기메뉴값 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        private void GetDefaultMenu(int _companyID, int _userID)
        {
            if (CabinetID.Equals(""))
            {
                Account.DAL.UserEnvironment bizEnv = new Account.DAL.UserEnvironment();
                List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = bizEnv.GetUserEnv(_companyID, _userID);

                if (list != null)
                {
                    foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo info in list)
                    {
                        CabinetID = (info.ApprovalPublicMenu.Equals("")) ? "Dept" : info.ApprovalPublicMenu;
                    }
                }
            }

            if (CabinetID.ToUpper().IndexOf("DEPT") > -1) { AccActive = "0"; }

            if (CabinetID.ToUpper().IndexOf("PRIVATE") > -1 || CabinetID.ToUpper().IndexOf("APPROVAL") > -1
                || CabinetID.ToUpper().IndexOf("REJECT") > -1 || CabinetID.ToUpper().IndexOf("ARBITRARY") > -1) { AccActive = "1"; }
            
            if (CabinetID.ToUpper().IndexOf("CARBONCOPY") > -1) { AccActive = "2"; }

            if (CabinetID.ToUpper().IndexOf("ENFORCE") > -1) { AccActive = "3"; }
        }

        //Left URL
        private void GetLeftURL()
        {
            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("accActive", AccActive);
            paraDic.Add("cabinetID", CabinetID);            

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Menu/Tree.aspx?p=" + encParams;
        }

        // Content URL
        private void GetContentURL(string _menuID)
        {
            string listPage = "/DeskplusEIP/Approval/Folder/Dept/List.aspx";

            switch (_menuID)
            {
                case "Dept":
                    listPage = "/DeskplusEIP/Approval/Folder/Dept/List.aspx";
                    break;
                case "Dept_Common":
                    listPage = "/DeskplusEIP/Approval/Folder/Dept/CommonList.aspx";
                    break;
                case "Dept_Doc":
                    listPage = "/DeskplusEIP/Approval/Folder/Dept/List.aspx";
                    break;
                case "Dept_Receipt":
                    listPage = "/DeskplusEIP/Approval/Folder/Dept/ReceiptList.aspx";
                    break;
                case "Dept_Send":
                    listPage = "/DeskplusEIP/Approval/Folder/Dept/SendList.aspx";
                    break;
                case "Private":
                    listPage = "/DeskplusEIP/Approval/Folder/Private/List.aspx";
                    break;
                case "Private_Doc":
                    listPage = "/DeskplusEIP/Approval/Folder/Private/List.aspx";
                    break;
                case "Private_Folder":
                    listPage = "/DeskplusEIP/Approval/Folder/Private/ListFolder.aspx";
                    break;
                case "Approval":
                    listPage = "/DeskPlusEIP/Approval/Folder/Approve/List.aspx";
                    break;
                case "Approval_Doc":
                    listPage = "/DeskPlusEIP/Approval/Folder/Approve/List.aspx";
                    break;
                case "Approval_Folder":
                    listPage = "/DeskPlusEIP/Approval/Folder/Approve/ListFolder.aspx";
                    break;
                case "Reject":
                    listPage = "/DeskPlusEIP/Approval/Folder/Approve/ListReject.aspx";
                    break;
                case "Reject_Doc":
                    listPage = "/DeskplusEIP/Approval/Folder/Approve/ListReject.aspx";
                    break;
                case "Reject_Folder":
                    listPage = "/DeskplusEIP/Approval/Folder/Approve/ListRejectFolder.aspx";
                    break;
                case "Arbitrary":
                    listPage = "/DeskPlusEIP/Approval/Folder/Arbitrary/List.aspx";
                    break;
                case "Arbitrary_Doc":
                    listPage = "/DeskplusEIP/Approval/Folder/Arbitrary/List.aspx";
                    break;
                case "Arbitrary_Folder":
                    listPage = "/DeskplusEIP/Approval/Folder/Arbitrary/ListFolder.aspx";
                    break;
                case "Carboncopy":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/List.aspx";
                    break;
                case "Carboncopy_Doc":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/List.aspx";
                    break;
                case "Carboncopy_Forward":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ForwardList.aspx";
                    break;
                case "Carboncopy_CirReceipt":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirReceiptList.aspx";
                    break;
                case "Carboncopy_CirSend":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirSendList.aspx";
                    break;
                case "Enforce":
                    listPage = "/DeskPlusEIP/Approval/Folder/Enforce/List.aspx";
                    break;
                case "EnforceSend":
                    listPage = "/DeskPlusEIP/Approval/Folder/Enforce/ListSend.aspx";
                    break;
                case "EnforceSendAdmin":
                    listPage = "/DeskPlusEIP/Approval/Folder/EnforceAdmin/ListSend.aspx";
                    break;
                case "EnforceSubmit":
                    listPage = "/DeskPlusEIP/Approval/Folder/Enforce/ListSubmit.aspx";
                    break;
                case "EnforceSubmitAdmin":
                    listPage = "/DeskPlusEIP/Approval/Folder/EnforceAdmin/ListSubmit.aspx";
                    break;
                case "Impo":
                    listPage = "/DeskPlusEIP/Approval/Folder/Impo/List.aspx";
                    break;
            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("formID", "0");
            paraDic.Add("yyyy", "");
            paraDic.Add("yyyymm", "");
            paraDic.Add("cabinetID", _menuID);
            paraDic.Add("types", "S");
            paraDic.Add("headerName", "");
            paraDic.Add("gubun", _menuID);

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